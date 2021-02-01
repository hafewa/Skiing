using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class StartPanel : BasePanel
{
    public Button RoleBtn;
    public Button StartBtn;
    
    public Toggle[] toggles;
    public Text GoldTxt;
    
    
    public Button VibrateBtn;

    public Player Player {
        get {
            return Player.Instance;
        }
    }
    public override void Awake() {
        base.Awake();
        Init();
        
    }

    public override void OnEnter() {
        base.OnEnter();
        
        audioMng.PlayBGAudio(MusicType.BGM_Battle0);
        Player.Instance.isGameOver = false;
        
        RefreshUI();
    }

    public override void OnResume() {
        base.OnResume();

        RefreshUI();
    }

    void Init() {

        toggles[0].isOn = true;

        StartBtn.onClick.AddListener(StartClick);
        StartBtn.transform.DOPunchScale(Vector3.one * 0.1f, 1f, 1).SetLoops(-1);
        
        VibrateBtn.onClick.AddListener(VibrateBtnClick);
        
        RoleBtn.onClick.AddListener(() =>
        {
            audioMng.BtnClick();
            uiMng.PushPanel(PanelType.RolePanel);
        });
        
        InitGM();
    }

    void InitGM() {
        string gmPath = "Root/";
        #region 设置过关关卡
        var passStageCount = transform.Find(gmPath + "GM/SetStageCount/InputField").GetComponent<InputField>();
        transform.Find(gmPath + "GM/SetStageCount/Button").GetComponent<Button>().onClick.AddListener(() =>
        {
            var count = int.Parse(passStageCount.text);
            if (count < 1) {
                MessageTool.ShowMessage("关卡不能小于1!", 2);
                return;
            }
            stageData.SetPassStageCount(count - 1);
            stageData.SetData();
            Player.Instance.RefreshMap();
        });
        #endregion

        #region 加金币
        var gold = transform.Find(gmPath + "GM/AddGold/InputField").GetComponent<InputField>();
        transform.Find(gmPath + "GM/AddGold/Button").GetComponent<Button>().onClick.AddListener(() =>
        {
            playerData.AddPlayerGold(int.Parse(gold.text));
            RefreshUI();
        });
        #endregion

        var gm = transform.Find(gmPath + "GM").gameObject;
        if (GameFacade.Instance.useGM) {
            gm.SetActive(true);
        }
        else {
            gm.SetActive(false);
        }
    }

    public void StartClick() {
        audioMng.BtnClick();
        //if (!Player.isStart) {
        //    Player.fsm.PerformTransition(Transition.Jump);

        //    foreach (var item in Player.AIPlayers) {
        //        item.fsm.PerformTransition(Transition.Jump);
        //    }
        //}

        uiMng.PushPanel(PanelType.BattlePanel);
    }
    public void RefreshGold() {
        GoldTxt.text = PrefsTool.GetPlayerGold().ToString();
    }

    public void RefreshUI() {

        RefreshGold();
        
        RefreshVibrateBtn();
        
        // RefreshDot();
    }

    void RefreshDot() {
        int roleDotCount = 0;

        var passStageCount = stageData.m_PassStageCount;
        var ownedSkinIDs = playerData.GetOwnedSkinIDs();
        var ownedStuntIDs = playerData.GetOwnedStuntIDs();
        var playerGold = playerData.GetPlayerGold();

        foreach (var item in XmlTool.Instance.skinCfg) {
            var skinModel = item.Value;
            bool isLock = (passStageCount < skinModel.UnlockStage);
            bool isBuyBtnShow = (!isLock && !ownedSkinIDs.Contains(skinModel.ID.ToString()));
            if(isBuyBtnShow && skinModel.Price <= playerGold) {
                roleDotCount++;
            }
        }

        foreach (var item in XmlTool.Instance.stuntCfg) {
            var stuntModel = item.Value;
            bool isLock = (passStageCount < stuntModel.UnlockStage);
            bool isBuyBtnShow = (!isLock && !ownedStuntIDs.Contains(stuntModel.ID.ToString()));
            if (isBuyBtnShow && stuntModel.Price <= playerGold) {
                roleDotCount++;
            }
        }

    }
    
    #region 设置模块
    void VibrateBtnClick() {
        audioMng.BtnClick();

        GameFacade.Instance.isCanVibrate = (PrefsTool.GetCanVibrate() == 1);
        PrefsTool.SetCanVibrate(GameFacade.Instance.isCanVibrate ? 0 : 1);

        RefreshVibrateBtn();
        VibrateTool.Vibrate();
    }
    
    void RefreshVibrateBtn() {
        GameFacade.Instance.isCanVibrate = (PrefsTool.GetCanVibrate() == 1);
        if (GameFacade.Instance.isCanVibrate) {
            VibrateBtn.targetGraphic.color = Color.white;
            // VibrateBtn.image.sprite = IsVibrateBtnON;
        }
        else {
            VibrateBtn.targetGraphic.color = new Color(1, 1, 1, 0.5f);
            // VibrateBtn.image.sprite = IsVibrateBtnOff;
        }
    }
    #endregion
    
}
