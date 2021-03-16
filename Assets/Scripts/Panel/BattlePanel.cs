using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BattlePanel : BasePanel
{    
    public Text GoldTxt;
    public RadialBlurEffect blurEffect;
        
    public Image PassCondition;
    public float PassConditionShowTime;
    public Image PassConditionBG;
    public Text ConditionBigTxt;
    public Text ConditionTxt;
    
    public Transform _Encourage;
    
    public GameObject[] m_vRankComp = { null, null, null, null, null, null, null, null };
    
    float initDis;// 初始距离
    float curDistance;//当前距离
    
    public Image Pointer;
    public Image Progress;
    
    public Text StageTxt;
    public Text TimeTxt;
    
    public Button ReStartBtn;

    private void FixedUpdate()
    {
        if (Player.Instance.isGameOver || !Player.Instance.isEnterBattle)
        {
            return;
        }

        #region  终点距离进度

        curDistance = Player.Instance.endTran == null
            ? float.MaxValue
            : MathTool.GetZDis(Player.Instance.endTran.position, Player.Instance.transform.position);
        Progress.fillAmount = 1 - curDistance / initDis;
        var width = Progress.GetComponent<RectTransform>().sizeDelta.x;
        var pos = Pointer.transform.localPosition;
        pos.x = width * Progress.fillAmount;
        Pointer.transform.localPosition = pos;

        #endregion
        
        if (Player.Instance.isTimeLimit) {
            var stageModel = stageData.m_CurStageModel;
            var leftTime = stageModel.PassTime - Player.Instance.m_PassTime;
            
            if (leftTime < 0) {
                Player.Instance.fsm.PerformTransition(Transition.Die);
                // var panel = GameFacade.Instance.uiMng.GetPanel<GameOverPanel>(PanelType.GameOverPanel);
                // panel.SetResoultDesc(string.Format("闯关超时", stageModel.PassCondition), FailType.OverTime);
                return;
            }
            TimeTxt.text = leftTime.ToString("f0");// + "s";
        }
        
        //排位UI动画
        var list = Player.Instance.GetRankArray();
        for (int i = 0; i < list.Length && i < 8; i++) {
            int nPos = list[i].m_nPos;
            int nRank = i + 1;
            var pComp = m_vRankComp[nPos];
            var ppTmp = pComp.transform.GetChild(0).GetComponent<Text>();
            ppTmp.text = nRank.ToString();

            // //玩家排名改变
            // if (list[i] == Player.Instance) {
            //     if (mPlayerRank != nRank) {
            //         if (mPlayerRank != -1 && nRank < mPlayerRank && !IsGuideModel) {
            //             mSurpassQueue.Enqueue(nRank);
            //             ShowPlayerSurpass();
            //         }
            //         mPlayerRank = nRank;
            //     }
            // }

            float fToPos = 30 - nRank * 40;
            if (nPos == 7) {
                if (nRank > 5)
                    fToPos = -220;
            }
            else {
                if (pComp.transform.localPosition.y < -180) {
                    float fToScale = pComp.transform.GetComponent<RectTransform>().localScale.x * 0.9f;
                    pComp.transform.GetComponent<RectTransform>().localScale = new Vector2(fToScale, fToScale);
                }
                else {
                    var v2 = pComp.transform.GetComponent<RectTransform>().sizeDelta;
                    float fToScale = pComp.transform.GetComponent<RectTransform>().localScale.x * 1.05f + 0.05f;
                    if (fToScale > 1)
                        fToScale = 1;
                    pComp.transform.GetComponent<RectTransform>().localScale = new Vector2(fToScale, fToScale);
                }
            }
            pComp.transform.Translate(new Vector3(0, (fToPos - pComp.transform.localPosition.y) * 0.1f));

        }
        
    }

    public override void Awake()
    {
        base.Awake();
        Init();
    }

    void Init()
    {
        blurEffect = GameObject.Find("Main Camera").GetComponent<RadialBlurEffect>();
        
        ReStartBtn.onClick.AddListener(() => { Player.Instance.ReStart(); });
        
        SetBattleBGM();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        audioMng.PlayBGAudio(bgnType, playerData.IsBgMusicOn ? 1 : 0);
        Player.Instance.isEnterBattle = true;
        
        RefreshUI();
    }

    public override void OnExit()
    {
        base.OnExit();
        Player.Instance.isEnterBattle = false;
    }

    public override void OnPause()
    {
        base.OnPause();
        Player.Instance.isEnterBattle = false;
    }

    public override void OnResume()
    {
        base.OnResume();
        Player.Instance.isEnterBattle = true;
    }

    public void SetHorizontalForce(float h)
    {
        Debug.Log("h:" + h);
        Player.Instance.leftAndRightBtnIsDown = (h != 0);
        Player.Instance.curHorizontalDirection = h;

        if (h == 0)
        {
            Player.Instance.SetHorizontalForce(h);
        }
    }

    public void JumpBtnClick()
    {
        Player.Instance.SetJumpForce(Player.Instance.jumpForce);
    }

    public void RefreshGold() {
        GoldTxt.text = PrefsTool.GetPlayerGold().ToString();
    }

    public void RefreshUI()
    {
        TimeTxt.SetActive(Player.Instance.isTimeLimit);
        
        StageTxt.text = (stageData.m_PassStageCount + 1).ToString();

        initDis = Player.Instance.endTran == null
            ? float.MaxValue
            : MathTool.GetZDis(Player.Instance.endTran.position, Player.Instance.transform.position);

        // initRank();
        InitEncourage();
        ConditionShow();
        RefreshGold();
    }
    
    public void initRank() {
        for (int i = 0; i < 8; i++) {
            var ppTmp = m_vRankComp[i].transform.GetChild(1).GetComponent<Text>();
            ppTmp.text = Player.Instance.AllPlayers[i].name;
        }

    }

    void InitEncourage() {
        _Encourage.SetActive(true);
        for (int i = 0; i < _Encourage.childCount; i++) {
            _Encourage.GetChild(i).SetActive(false);
        }
    }

    private bool isShowEncourage = false;
    public void ShowEncourage(Encourage encourage, float time = 1f)
    {
        if (isShowEncourage) return;
        
        //Debug.LogFormat("Encourage:{0}", encourage);
        MusicType musicType;
        if (Enum.TryParse(encourage.ToString(), true, out musicType)) {
            audioMng.PlayAudioEffect(musicType);
        }
        if (encourage != Encourage.Miss)
        {
            ShowBlurBySpeedUp();
        }
        for (int i = 0; i < _Encourage.childCount; i++) {
            var child = _Encourage.GetChild(i);
            if (child.name == encourage.ToString()) {
                child.DOScale(Vector3.one, 0f);
                child.SetActive(true);

                child.DOShakeScale(time)
                    .OnStart(() =>
                    {
                        isShowEncourage = true;
                    })
                    .OnComplete(() =>
                    {
                        child.SetActive(false);
                        isShowEncourage = false;
                    });
                break;
            }

        }
    }
    
    void ConditionShow() {
        var stageModel = stageData.m_CurStageModel;
        // WindTxt.text = string.Format("风阻:{0}%", stageModel.Wind * 100);
        float passTime = 0;//目标动画播放时间
        PassConditionBG.transform.DOPunchScale(Vector3.one * 0.2f, PassConditionShowTime, 5)
            .OnStart(() =>
            {
                audioMng.PlayAudioEffect(MusicType.CountDown);
                string str = (stageModel.PassCondition == 8) ? "到达终点" : string.Format("达到前{0}名", stageModel.PassCondition);
                if (Player.Instance.isTimeLimit) {
                    str = string.Format("{0}秒内通关", stageModel.PassTime);
                }
                ConditionTxt.text = str;
                ConditionBigTxt.fontSize = 80;
                ConditionBigTxt.text = str;
                PassConditionBG.DOFade(1, 0);
                ConditionBigTxt.DOFade(1, 0);
                ConditionBigTxt.transform.DOLocalMove(Vector3.zero, 0);
                PassCondition.DOFade(0, 0);
                ConditionTxt.DOFade(0, 0);
            })
            .OnUpdate(() =>
            {
                passTime += Time.deltaTime;
                if (passTime > 1) {
                    passTime = 0;
                    audioMng.PlayAudioEffect(MusicType.CountDown);
                }
            })
            .OnComplete(() =>
            {
                if (!Player.Instance.isStart)
                {
                    // Player.Instance.fsm.PerformTransition(Transition.Forward);
                    Player.Instance.StartReSet();
                
                    foreach (var item in Player.Instance.AIPlayers)
                    {
                        // item.fsm.PerformTransition(Transition.Forward);
                        item.StartReSet();
                    }
                }

                PassConditionBG.DOFade(0, PassConditionShowTime);

                ConditionBigTxt.transform.DOMove(ConditionTxt.transform.position, PassConditionShowTime)
                .OnStart(() =>
                {
                    PassCondition.DOFade(1, PassConditionShowTime);
                })
                .OnUpdate(() =>
                {
                    ConditionBigTxt.fontSize = (int)Mathf.Lerp(ConditionBigTxt.fontSize, ConditionTxt.fontSize, Time.deltaTime);
                })
                .OnComplete(() =>
                {
                    ConditionBigTxt.DOFade(0, PassConditionShowTime);
                    ConditionTxt.DOFade(1, PassConditionShowTime);
                });
            });
    }
    
    void ShowBlurBySpeedUp()
    {
        Tweener tweener = DOTween.To(() => blurEffect.blurFactor, x => blurEffect.blurFactor = x, 0.03f, 0.2f);
        tweener.OnComplete(SpeedUpComplete);
    }

    void SpeedUpComplete()
    {
        DOTween.To(() => blurEffect.blurFactor, x => blurEffect.blurFactor = x, 0f, 0.2f);
    }
    

    MusicType bgnType = MusicType.BGM_Battle0;
    public void SetBattleBGM() {
        //BGM增至9首，初期开放5首，之后每20关新增一首（BGM纪子易提供）
        var passStageCount = stageData.m_PassStageCount;
        int maxIndex = 5;
        if (passStageCount > 20) {
            maxIndex += (passStageCount / 20);
            maxIndex = Mathf.Clamp(maxIndex, 5, 8);
        }
        int bgName = UnityEngine.Random.Range(1, maxIndex + 1);// "BGM_Battle" + UnityEngine.Random.Range(0, 1 + 1);
        bgnType = (MusicType)bgName;


    }
}