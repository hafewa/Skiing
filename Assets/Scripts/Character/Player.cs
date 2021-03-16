using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Player : ICharacter
{
    
    BattlePanel battlePanel;

    public ICharacter[] AIPlayers;
    public ICharacter[] AllPlayers;

    public static Player Instance;

    public bool isEnterBattle;
    public bool isTimeLimit; //游戏是否限时

    public Transform startTran;
    public Transform endTran;

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "SpeedUp")
        {
            if (battlePanel == null)
            {
                battlePanel = GameFacade.Instance.uiMng.GetPanel<BattlePanel>(PanelType.BattlePanel);
            }

            battlePanel.ShowEncourage((Encourage) Random.Range(0, 3));
        }
    }

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (other.tag == "End")
        {
            uiMng.PushPanel(PanelType.RankPanel);
        }

        if (other.tag == "SpeedUp")
        {
            if (fsm._character.SpeedLine) fsm._character.SpeedLine.Play();
            fsm._character.audioMng.PlayAudioEffect(MusicType.SpeedUp);
            fsm._character.audioMng.PlayAudioEffect(MusicType.Wind);

            VibrateTool.Vibrate();
        }

        //吃金币
        if (other.tag == "Gold")
        {
            other.SetActive(false);

            playerData.AddPlayerGold(Random.Range(5, 11));
            
            if (battlePanel == null)
            {
                battlePanel = GameFacade.Instance.uiMng.GetPanel<BattlePanel>(PanelType.BattlePanel);
            }
            battlePanel.RefreshGold();
        }
    }

    public Vector3 startPos; //开始点击位置
    public Vector3 nextFramePos; //下一帧位置
    public bool startPosFlag = false; //开始点击标记

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        if (isStart)
        {
            var touchWidth = Screen.width / 2;
#if UNITY_EDITOR || UNITY_STANDALONE //电脑操作
            if (Input.GetKeyUp(KeyCode.Space))
            {
                SetJumpForce(jumpForce);
            }
#endif

#if UNITY_IPHONE || UNITY_ANDROID // 手机端拖拽
            if (Input.touchCount > 0)
            {
                if ((Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(0).phase == TouchPhase.Moved) &&
                    !startPosFlag)
                {
                    Debug.Log("======开始触摸=====");
                    startPosFlag = true;
                    startPos = Input.GetTouch(0).position; // - mousePosOffset;
                }

                if (Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Canceled)
                {
                    Debug.Log("======释放触摸=====");
                    startPosFlag = false;
                    mousePosOffset = Vector2.zero;
                }

                if (startPosFlag)
                {
                    nextFramePos = Input.GetTouch(0).position;
                    mousePosOffset += (Vector2) (nextFramePos - startPos);
                    mousePosOffset.x = Mathf.Clamp(mousePosOffset.x, -touchWidth, touchWidth);
                    startPos = nextFramePos;
                }
            }
#endif
#if UNITY_EDITOR || UNITY_STANDALONE //鼠标点击拖拽
            if ((Input.GetMouseButtonDown(0) || Input.GetMouseButton(0)) && !startPosFlag)
            {
                startPosFlag = true;
                startPos = Input.mousePosition; // - (Vector3)mousePosOffset;
            }

            if (Input.GetMouseButtonUp(0) || !Input.GetMouseButton(0))
            {
                startPosFlag = false;
                mousePosOffset = Vector2.zero;
            }

            if (startPosFlag)
            {
                nextFramePos = Input.mousePosition;
                mousePosOffset += (Vector2) (nextFramePos - startPos);
                mousePosOffset.x = Mathf.Clamp(mousePosOffset.x, -touchWidth, touchWidth);
                startPos = nextFramePos;
            }
#endif

            leftAndRightBtnIsDown = startPosFlag;
            if (leftAndRightBtnIsDown)
            {
                curHorizontalDirection = mousePosOffset.x / touchWidth;
            }
            else
            {
                curHorizontalDirection = 0;
            }
            
            SetHorizontalForce(curHorizontalDirection);

            mapMng.MoveMapChunk();

            base.FixedUpdate();
        }
    }

    protected override void Awake()
    {
        base.Awake();
        Instance = this;
    }

    protected override void Start()
    {
        base.Start();
        Init();
    }

    private void Init()
    {
        var skinID = playerData.GetPlayerSkinID();
        SetPlayerSkin(skinID);
        
        SetPlayerSkis(playerData.GetPlayerSkisID());
        
        CreateAIPlayer();
        ReStart();
    }

    void CreateAIPlayer()
    {

        var skidmarks = Resources.Load<GameObject>("Prefabs/Skidmarks/Skidmarks");
        
        var skiingSkid1 = GetComponent<SkiingSkid>();
        var sm1 = Instantiate(skidmarks).GetComponent<Skidmarks>();
        skiingSkid1.iCharater = this;
        skiingSkid1.skidmarksController = sm1;
        this.skidmarks = sm1;

        return;
        
        var aiPlayer = Resources.Load<GameObject>("Prefabs/Role/AIPlayer");
        AIPlayers = new ICharacter[7];
        AllPlayers = new ICharacter[8];

        this.name = "我";
        AllPlayers[7] = this;
        this.m_nPos = 7;

        for (int i = 0; i < 7; i++)
        {
            AIPlayer ai = (AIPlayer) Instantiate(aiPlayer).GetComponent<ICharacter>();
            AIPlayers[i] = ai;
            AllPlayers[i] = ai;
            ai.m_nPos = i;

            //初始化AI滑雪痕迹控制脚本
            var skiingSkid2 = ai.GetComponent<SkiingSkid>();
            var sm2 = Instantiate(skidmarks).GetComponent<Skidmarks>();
            skiingSkid2.iCharater = ai;
            skiingSkid2.skidmarksController = sm2;
            ai.skidmarks = sm2;
        }
    }

    public override void ReStart()
    {
        base.ReStart();
        GameFacade.Instance.dataMng.SetData();
        isTimeLimit = (stageData.m_CurStageModel.PassTime != -1);

        RefreshMap();
    }

    public void RefreshMap()
    {
        // mapMng.LoadMap();
        mapMng.LoadMap2();
    }

    public void SetMapData(Transform map)
    {
        var stageModel = stageData.m_CurStageModel;
        if (stageModel != null)
        {
            // var startPos = map.Find("StartPos/" + stageModel.StartPosName);
            // if (startPos != null)
            // {
            //     startTran = startPos;
            // }
            // else
            // {
            //     Debug.LogErrorFormat("起点:{0} 不存在", stageModel.StartPosName);
            // }

            var childIndex = int.Parse(stageModel.StartPosName);
            var startPos = map.Find("StartPos");
            if (startPos != null)
            {
                for (int i = 0; i < startPos.childCount; i++)
                {
                    startPos.GetChild(i).SetActive(false);
                }

                startTran = startPos.GetChild(childIndex);
                startTran.SetActive(true);
            }
            else
            {
                Debug.LogErrorFormat("起点:{0} 不存在", stageModel.StartPosName);
            }

            var root = map.Find("EndPos");
            if (root != null)
            {
                root.SetActive(true);
                for (int i = 0; i < root.childCount; i++)
                {
                    root.GetChild(i).SetActive(false);
                }

                endTran = root.GetChild(childIndex);
                endTran.SetActive(true);
            }

            // var endStrPath = string.Format("EndPos/PZ-zhongdian ({0})", startPos.GetSiblingIndex() + 1);
            // var PZ_zhongdian = map.Find(endStrPath);
            // if (PZ_zhongdian != null)
            // {
            //     var colliders = PZ_zhongdian.GetComponents<Collider>();
            //     foreach (var item in colliders)
            //     {
            //         item.enabled = false;
            //     }
            //
            //     PZ_zhongdian.SetActive(true);
            //     for (int i = 0; i < PZ_zhongdian.childCount; i++)
            //     {
            //         PZ_zhongdian.GetChild(i).SetActive(false);
            //     }
            //
            //     var Zhongdian_piaofu =
            //         PZ_zhongdian.Find(string.Format("Zhongdian-piaofu ({0})",
            //             Random.Range(1, 10))); // PZ_zhongdian.GetChild(Random.Range(0, 9));
            //     Zhongdian_piaofu?.SetActive(true);
            //     endTran = Zhongdian_piaofu?.Find("zhongdian");
            // }
            // else
            // {
            //     Debug.LogErrorFormat("终点:{0} 不存在", endStrPath);
            // }

            SetPlayerPos();
        }

        // for (int i = 0; i < 7; i++) {
        //     ((AIPlayer)AIPlayers[i]).SetMapData(map);
        // }

        uiMng.GetPanel<LoadingPanel>(PanelType.LoadingPanel)?.MapLoaded();
    }

    public void SetMapData2(Transform start, Transform end)
    {
        var stageModel = stageData.m_CurStageModel;
        if (stageModel != null)
        {
            var childIndex = int.Parse(stageModel.StartPosName);
            var startPos = start.Find("StartPos");
            if (startPos != null)
            {
                for (int i = 0; i < startPos.childCount; i++)
                {
                    startPos.GetChild(i).SetActive(false);
                }

                startTran = startPos.GetChild(childIndex);
                startTran.SetActive(true);
            }
            else
            {
                Debug.LogErrorFormat("起点:{0} 不存在", stageModel.StartPosName);
            }

            var root = (end == null ? null : end.Find("EndPos"));
            if (root != null)
            {
                root.SetActive(true);
                for (int i = 0; i < root.childCount; i++)
                {
                    root.GetChild(i).SetActive(false);
                }

                endTran = root.GetChild(childIndex);
                endTran.SetActive(true);
            }

            SetPlayerPos();
        }

        uiMng.GetPanel<LoadingPanel>(PanelType.LoadingPanel)?.MapLoaded();
    }

    List<int> _skinIndexList = new List<int>(14);

    void SetPlayerPos()
    {
        Vector3 startPos = startTran.position;
        transform.position = startPos;
        transform.rotation = Quaternion.Euler(Vector3.zero);

        _skinIndexList.Clear();
        for (int j = 1; j < 15; j++)
        {
            _skinIndexList.Add(j);
        }

        var pos = startPos;
        int half = AIPlayers.Length / 2;
        float space = 2f;
        for (int i = 0; i < half; i++)
        {
            pos.x -= space;
            AIPlayers[i].ReStart();
            AIPlayers[i].transform.position = pos;
            AIPlayers[i].transform.rotation = Quaternion.Euler(Vector3.zero);
            AIPlayers[i].name = RanNameTool.Instance.RandomName();

            //设置身体和皮肤材质
            var skinIndex = _skinIndexList.ListRandom();
            _skinIndexList.Remove(skinIndex);
            AIPlayers[i].SetPlayerSkin(skinIndex);
        }

        pos.x = startPos.x;
        for (int i = half; i < AIPlayers.Length; i++)
        {
            pos.x += space;
            AIPlayers[i].ReStart();
            AIPlayers[i].transform.position = pos;
            AIPlayers[i].transform.rotation = Quaternion.Euler(Vector3.zero);
            AIPlayers[i].name = RanNameTool.Instance.RandomName();

            //设置身体和皮肤材质
            var skinIndex = _skinIndexList.ListRandom();
            _skinIndexList.Remove(skinIndex);
            AIPlayers[i].SetPlayerSkin(skinIndex);
        }
    }


    public void SetHorizontalForce(float h)
    {
        curHorizontalForce = h * horizontalForceSensitivity;
    }

    public void SetJumpForce(float jumpForce)
    {
        curJumpForce = jumpForce;
    }

    public override void Die()
    {
        base.Die();

        fsm._character.audioMng.PlayAudioEffect(MusicType.Die);
        fsm._character.audioMng.PlayAudioEffect(MusicType.Lose);
        VibrateTool.Vibrate();
        var panel = (GameOverPanel) GameFacade.Instance.uiMng.PushPanel(PanelType.GameOverPanel);
        // panel.SetResoultDesc("碰触障碍物", FailType.Collision);
    }

    /// <summary>
    /// 获取玩家实时排名
    /// </summary>
    /// <returns></returns>
    public ICharacter[] GetRankArray()
    {
        var aliveList = AllPlayers.Where(n => n.isDie == false).OrderByDescending(n => n.transform.position.z)
            .ToArray();
        var isDieList = AllPlayers.Where(n => n.isDie == true).OrderByDescending(n => n.transform.position.z).ToArray();

        return aliveList.Concat(isDieList).ToArray();
    }

    private float startRoateY = 0;
    private float startRoateX = 0;
    public void SetStartRoate() {
        startRoateY = transform.localEulerAngles.y;
        startRoateX = transform.localEulerAngles.x;
    }

    public void SetPlayerRoate(float roateY, float roateX) {
        float anglesX = 0;
        if(currStateId == StateID.ShowSpeedUp) {
            anglesX = startRoateX + roateX;
        }
        SetLocalEulerAngles(new Vector3(anglesX, startRoateY + roateY, 0));
    }

    /// <summary>
    /// 设置玩家身体皮肤
    /// </summary>
    public void SetPlayerSkin(int skinID) {
        if (XmlTool.Instance.skinCfg.ContainsKey(skinID)) {
            var skinModel = XmlTool.Instance.skinCfg[skinID];
            
            _bodyRenderer.material.SetTexture("_MainTex",
                Resources.Load<Texture>("Textures/Body/" + skinModel.TextureName));

            playerData.SetPlayerSkinID(skinID);
        }
        else {
            Debug.LogErrorFormat("皮肤ID:{0}不存在!", skinID);
        }
    }

    /// <summary>
    /// 设置滑雪板皮肤
    /// </summary>
    /// <param name="skisID"></param>
    public void SetPlayerSkis(int skisID)
    {
        if (XmlTool.Instance.skisCfg.ContainsKey(skisID)) {
            var skinModel = XmlTool.Instance.skisCfg[skisID];
            
            
            _skiRenderer.material.SetTexture("_MainTex",
                Resources.Load<Texture>("Textures/Skis/" + skinModel.TextureName));

            playerData.SetPlayerSkisID(skisID);
        }
        else {
            Debug.LogErrorFormat("滑雪板ID:{0}不存在!", skisID);
        }
    }
}