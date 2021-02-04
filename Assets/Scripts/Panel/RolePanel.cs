using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum BuyType
{
    Body,//皮肤
    Skis,//滑雪板
}

public class RolePanel : BasePanel, IPointerDownHandler, IDragHandler
{
    public Transform SkisContent;
    public SkisItem skisItem;
    
    public Text DescTxt;
    public Transform BuyPanel;
    public Button CloseBtn;
    public Button BuyBtn;
    public Text BuyTxt;
    BuyType mBuyType;
    int mBuyPrice;
    int mItemModelID;
    Button mItemBtn;
    
    public Button YesBtn;
    public Button NoBtn;
    
    public Transform GetGoldPanel;
    
    public Transform BodyContent;
    public BodyItem bodyItem;
    
    private Transform charCamTrans;
    
    public Text nameTxt;
    public Text GoldTxt;
    
    public Button ReturnBtn;

    public Vector2 startPos;
    
    public override void Awake() {
        base.Awake();
        Init();
    }

    private void Init()
    {
        BuyBtn.onClick.AddListener(BuyBtnClick);
        
        CloseBtn.onClick.AddListener(() =>
        {
            audioMng.BtnClick();
            BuyPanel.SetActive(false);
        });
        
        YesBtn.onClick.AddListener(() =>
        {
            MessageTool.ShowMessage("功能开发中,敬请期待!");
            GetGoldPanel.SetActive(false);
            // AdSDKCenter.GetInstance().SaveLog("激励视频", "缺钱激励", "1");
            // FunctionDispatcher.HandleItemClick(MainListItemId.Ad, MainListItemId.SHOW_REWARD_AD);
            // GameFacade.Instance.audioMng.SetBGM(false);
            // GameFacade.Instance.DelayEvent(1000, () =>
            // {
            //     //FunctionDispatcher.HandleItemClick(MainListItemId.Ad, MainListItemId.LOAD_REWARD_AD_V);
            // });
        });
        
        NoBtn.onClick.AddListener(() =>
        {
            GetGoldPanel.SetActive(false);
        });

        //设置人物展示
        if (charCamTrans == null) {
            charCamTrans = GameObject.FindGameObjectWithTag("CharShowCam").transform;
        }
        charCamTrans.gameObject.SetActive(false);
        
        ReturnBtn.onClick.AddListener(() =>
        {
            audioMng.BtnClick();
            if (charCamTrans != null)
            {
                charCamTrans.gameObject.SetActive(false);
            }
            
            Player.Instance.SetLocalEulerAngles(Vector3.zero);
            Player.Instance.fsm.PerformTransition(Transition.Ready);
            uiMng.PopPanel();
        });
        
        
        InitContent();
        
        #region 显示当前皮肤属性
        ShowSkinProperty(playerData.GetPlayerSkinID());
        #endregion
    }
    
    public override void OnEnter() {
        base.OnEnter();

        RefreshUI();

        //设置人物展示相机的相对位置
        charCamTrans.localPosition = Player.Instance.transform.position + Player.Instance.transform.forward * 2.347f + new Vector3(0, 1.39f, 0);
        charCamTrans.localEulerAngles = new Vector3(4f, 180f + Player.Instance.transform.localEulerAngles.y, 0);
        charCamTrans.localScale = Vector3.one;
        charCamTrans.gameObject.SetActive(true);
        
        // Player.Instance.EnterRolePanel();
        mapMng.SetRolePanelEnvironment();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        startPos = eventData.position;
        Player.Instance.SetStartRoate();
    }

    public void OnDrag(PointerEventData eventData)
    {
        float roateY = (startPos.x - eventData.position.x) * 0.4f;
        float roateX = (startPos.y - eventData.position.y) * 0.4f;
        Player.Instance.SetPlayerRoate(roateY, roateX);
    }

    public void RefreshGold() {
        GoldTxt.text = PrefsTool.GetPlayerGold().ToString();
    }
    
    public void RefreshUI() {
        // GetGoldPanel.SetActive(false);
        // BuyPanel.SetActive(false);
        RefreshGold();

        RefreshContent();
    }
    
    /// <summary>
    /// 显示皮肤属性
    /// </summary>
    /// <param name="modelID"></param>
    public void ShowSkinProperty(int modelID) {
        var skinModel = XmlTool.Instance.skinCfg[modelID];
        nameTxt.text = skinModel.Name;
        // MPHTxt.text = string.Format("+ {0} MPH", skinModel.RateMPH);
        // GTxt.text = string.Format("+ {0} G", skinModel.RateG);
    }
    
    /// <summary>
    /// 显示滑雪板属性
    /// </summary>
    /// <param name="modelID"></param>
    public void ShowSkisProperty(int modelID) {
        var skinModel = XmlTool.Instance.skisCfg[modelID];
        nameTxt.text = skinModel.Name;
        // MPHTxt.text = string.Format("+ {0} MPH", skinModel.RateMPH);
        // GTxt.text = string.Format("+ {0} G", skinModel.RateG);
    }
    
    /// <summary>
    /// 显示特技属性
    /// </summary>
    /// <param name="modelID"></param>
    public void ShowSkiProperty(int modelID) {
        var stuntModel = XmlTool.Instance.stuntCfg[modelID];
        nameTxt.text = stuntModel.Name;
        // SpeedUpDisTxt.text = string.Format("冲刺距离 {0}M", stuntModel.SpeedUpDis * 50);
        // PlayStunt(stuntModel.ClipName);
    }

    void InitContent()
    {
        foreach (var skinModel in XmlTool.Instance.skinCfg) {
            var item = Instantiate(bodyItem, BodyContent);
            item.skinModel = XmlTool.Instance.skinCfg[skinModel.Key];
            item.rolePanel = this;
        }

        bodyItem.SetActive(false);

        foreach (var skinModel in XmlTool.Instance.skisCfg)
        {
            var item = Instantiate(skisItem, SkisContent);
            item.skinModel = XmlTool.Instance.skisCfg[skinModel.Key];
            item.rolePanel = this;
        }

        skisItem.SetActive(false);
    }

    void RefreshContent() {
        var passStageCount = stageData.m_PassStageCount;
        var ownedSkinIDs = playerData.GetOwnedSkinIDs();
        for (int i = 0; i < BodyContent.childCount; i++) {
            var bodyItem = BodyContent.GetChild(i).GetComponent<BodyItem>();
            if (bodyItem != null) {
                bodyItem.RefreshUI(passStageCount, ownedSkinIDs);
            }
        }

        var ownedStuntIDs = playerData.GetOwnedSkisIDs();
        for (int i = 0; i < SkisContent.childCount; i++) {
            var skisItem = SkisContent.GetChild(i).GetComponent<SkisItem>();
            if (skisItem != null) {
                skisItem.RefreshUI(passStageCount, ownedStuntIDs);
            }
        }
    }

    public void OpenGetGoldPanel() {
        // if(m_nRemainTime <= 0)
        // {
        //     MessageTool.ShowMessage("金币不足!", 0.5f);
        // }
        // else
        {
            GetGoldPanel.SetActive(true);
        }
    }

    public void OpenBuyPanel(BuyType buyType, int price, int itemModelID, Button ItemBtn) {
        mItemBtn = ItemBtn;
        mItemModelID = itemModelID;
        mBuyType = buyType;
        mBuyPrice = price;
        BuyTxt.text = price.ToString();
        BuyPanel.SetActive(true);

        switch (mBuyType) {
            case BuyType.Body:
                DescTxt.text = "该皮肤已解锁，是否立即购买";
                break;
            case BuyType.Skis:
                DescTxt.text = "该特技已解锁，是否立即购买";
                break;
        }
    }

    void BuyBtnClick() {
        audioMng.PlayAudioEffect(MusicType.BuyOk);

        BuyPanel.SetActive(false);
        mItemBtn.SetActive(false);
        switch (mBuyType) {
            case BuyType.Body:
                playerData.AddPlayerGold(-mBuyPrice);       //扣钱
                playerData.SetOwnedSkinIDs(mItemModelID);   //设置拥有的皮肤ID
                Player.Instance.SetPlayerSkin(mItemModelID);//装备皮肤
                ShowSkinProperty(mItemModelID);
                MessageTool.ShowMessage("购买成功!", 0.5f);
                // AdSDKCenter.GetInstance().SaveLog("花钱", "购买皮肤", mItemModelID.ToString());
                // TDGAItem.OnPurchase("购买皮肤", 1, mBuyPrice);
                //Dictionary<string, int> dic = new Dictionary<string, int>();
                //dic.Add("level", "50-60");      //级别区间，注意是字符串哟！
                //dic.Add("map", "沼泽地阿卡村"); //地图场景 
                //dic.Add("mission", "屠龙副本"); //关卡。
                //dic.Add("reason", "PK致死");    //死亡原因 
                //dic.Add("coin", "10000～20000"); //携带金币数量
                //TalkingDataGA.OnEvent("dead", dic);


                break;
            case BuyType.Skis:
                playerData.AddPlayerGold(-mBuyPrice);       //扣钱
                playerData.SetOwnedSkisIDs(mItemModelID);   //设置拥有的特技ID
                Player.Instance.SetPlayerSkis(mItemModelID);//装备皮肤
                ShowSkisProperty(mItemModelID);
                //不必装备,钻圈时选出拥有的特技随机播放
                MessageTool.ShowMessage("购买成功!", 0.5f);
                // AdSDKCenter.GetInstance().SaveLog("花钱", "购买特技", mItemModelID.ToString());
                // TDGAItem.OnPurchase("购买特技", 1, mBuyPrice);
                break;
        }
        RefreshUI();

        // var panel = (GainPanel)uiMng.PushPanel(PanelType.GainPanel);
        // panel.RefreshUI(mBuyType, mItemModelID);
    }
}