using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class StartPanel : BasePanel
{
    public Text GoldLvTxt;
    public Text GoldEarningTxt;
    public Text GoldRateTxt;
    public Button GoldBtn;
    public Text GoldBtnTxt;

    public Text GLvTxt;
    public Text GTxt;
    public Text GRateTxt;
    public Button GBtn;
    public Text GBtnTxt;

    public Text MPHLvTxt;
    public Text MPHTxt;
    public Text MPHRateTxt;
    public Button MPHBtn;
    public Text MPHBtnTxt;

    public Button BgMusicBtn;
    public Button SoundBtn;

    public Sprite IsBgMusicBtnON;
    public Sprite IsBgMusicBtnOff;
    public Sprite IsSoundBtnON;
    public Sprite IsSoundBtnOff;

    public Button RoleBtn;
    public Button StartBtn;

    public Toggle[] toggles;
    public Text GoldTxt;


    public Button VibrateBtn;

    public Player Player
    {
        get { return Player.Instance; }
    }

    public override void Awake()
    {
        base.Awake();
        Init();
    }

    public override void OnEnter()
    {
        base.OnEnter();

        audioMng.PlayBGAudio(MusicType.BGM_Battle0, playerData.IsBgMusicOn ? 1 : 0);
        Player.Instance.isGameOver = false;

        RefreshUI();
    }

    public override void OnResume()
    {
        base.OnResume();

        RefreshUI();
    }

    void Init()
    {
        toggles[0].isOn = true;

        StartBtn.onClick.AddListener(StartClick);
        StartBtn.transform.DOPunchScale(Vector3.one * 0.1f, 1f, 1).SetLoops(-1);

        MPHBtn.onClick.AddListener(MPHBtnClick);
        GBtn.onClick.AddListener(SpeedUpGBtnClick);
        GoldBtn.onClick.AddListener(GoldBtnClick);

        VibrateBtn.onClick.AddListener(VibrateBtnClick);
        BgMusicBtn.onClick.AddListener(BgMusicBtnClick);
        SoundBtn.onClick.AddListener(SoundBtnClick);

        RoleBtn.onClick.AddListener(() =>
        {
            audioMng.BtnClick();
            uiMng.PushPanel(PanelType.RolePanel);
        });

        InitGM();
    }

    void InitGM()
    {
        string gmPath = "Root/";

        #region 设置过关关卡

        var passStageCount = transform.Find(gmPath + "GM/SetStageCount/InputField").GetComponent<InputField>();
        transform.Find(gmPath + "GM/SetStageCount/Button").GetComponent<Button>().onClick.AddListener(() =>
        {
            var count = int.Parse(passStageCount.text);
            if (count < 1)
            {
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
        if (GameFacade.Instance.useGM)
        {
            gm.SetActive(true);
        }
        else
        {
            gm.SetActive(false);
        }
    }

    public void StartClick()
    {
        audioMng.BtnClick();
        //if (!Player.isStart) {
        //    Player.fsm.PerformTransition(Transition.Jump);

        //    foreach (var item in Player.AIPlayers) {
        //        item.fsm.PerformTransition(Transition.Jump);
        //    }
        //}

        uiMng.PushPanel(PanelType.BattlePanel);
    }

    public void RefreshGold()
    {
        GoldTxt.text = PrefsTool.GetPlayerGold().ToString();
    }

    public void RefreshUI()
    {
        RefreshGold();

        RefreshMPH();
        RefreshSpeedUpG();
        RefreshGoldEarning();

        RefreshVibrateBtn();
        RefreshBgMusicBtn();
        RefreshSoundBtn();

        RefreshDot();
    }

    void RefreshDot()
    {
        int roleDotCount = 0;

        var passStageCount = stageData.m_PassStageCount;
        var ownedSkinIDs = playerData.GetOwnedSkinIDs();
        // var ownedStuntIDs = playerData.GetOwnedStuntIDs();
        var ownedSkisIDs = playerData.GetOwnedSkisIDs();
        var playerGold = playerData.GetPlayerGold();

        foreach (var item in XmlTool.Instance.skinCfg)
        {
            var skinModel = item.Value;
            bool isLock = (passStageCount < skinModel.UnlockStage);
            bool isBuyBtnShow = (!isLock && !ownedSkinIDs.Contains(skinModel.ID.ToString()));
            if (isBuyBtnShow && skinModel.Price <= playerGold)
            {
                roleDotCount++;
            }
        }

        // foreach (var item in XmlTool.Instance.stuntCfg) {
        //     var stuntModel = item.Value;
        //     bool isLock = (passStageCount < stuntModel.UnlockStage);
        //     bool isBuyBtnShow = (!isLock && !ownedStuntIDs.Contains(stuntModel.ID.ToString()));
        //     if (isBuyBtnShow && stuntModel.Price <= playerGold) {
        //         roleDotCount++;
        //     }
        // }

        foreach (var item in XmlTool.Instance.skisCfg)
        {
            var skinModel = item.Value;
            bool isLock = (passStageCount < skinModel.UnlockStage);
            bool isBuyBtnShow = (!isLock && !ownedSkisIDs.Contains(skinModel.ID.ToString()));
            if (isBuyBtnShow && skinModel.Price <= playerGold)
            {
                roleDotCount++;
            }
        }
    }

    int maxLevel = 60; //最大提升等级

    #region 最大时速

    public void MPHBtnClick()
    {
        audioMng.PlayAudioEffect(MusicType.LevelUp);

        int lv = playerData.GetMaxSpeedLevel();
        var PlayerGold = playerData.GetPlayerGold();
        var UpgradeGold = MathTool.GetUpgradeGoldForMPH(lv);
        if (PlayerGold < UpgradeGold)
        {
            MessageTool.ShowMessage("金币不足!");
            return;
        }

        playerData.AddPlayerGold(-UpgradeGold);
        playerData.SetMaxSpeedLevel(lv + 1);
        // TDGAItem.OnPurchase("速度升级", 1, UpgradeGold);
        // AdSDKCenter.GetInstance().SaveLog("花钱", "速度升级", lv.ToString());
        RefreshUI();
    }

    void RefreshMPH()
    {
        int lv = playerData.GetMaxSpeedLevel();
        MPHLvTxt.text = /*"Lv." +*/ lv.ToString();
        MPHTxt.text = MathTool.GetMPHForShow(lv).ToString("F0") /* + "MPH"*/;
        MPHRateTxt.text = "+" + ((MathTool.GetTotalMPHGrowth(lv) / MathTool.BaseMPH) * 100).ToString("F0") + "%";

        //var PlayerGold = playerData.GetPlayerGold();
        var UpgradeGold = MathTool.GetUpgradeGoldForMPH(lv);
        //MPHBtn.interactable = (PlayerGold >= UpgradeGold && lv < maxLevel);
        MPHBtnTxt.text = UpgradeGold.ToString();
        MPHBtn.SetActive(lv < maxLevel);
    }

    #endregion

    #region 加速度等级提升

    void SpeedUpGBtnClick()
    {
        audioMng.PlayAudioEffect(MusicType.LevelUp);

        int lv = playerData.GetSpeedUpLevel();
        var PlayerGold = playerData.GetPlayerGold();
        var UpgradeGold = MathTool.GetUpgradeGoldForG(lv);
        if (PlayerGold < UpgradeGold)
        {
            MessageTool.ShowMessage("金币不足!");
            return;
        }

        playerData.AddPlayerGold(-UpgradeGold);
        playerData.SetSpeedUpLevel(lv + 1);
        // TDGAItem.OnPurchase("加速度升级", 1, UpgradeGold);
        // AdSDKCenter.GetInstance().SaveLog("花钱", "加速度升级", lv.ToString());
        RefreshUI();
    }

    void RefreshSpeedUpG()
    {
        int lv = playerData.GetSpeedUpLevel();
        GLvTxt.text = /*"Lv." + */lv.ToString();
        GTxt.text = MathTool.GetSpeedUpG(lv).ToString("F0") /* + "G"*/;
        GRateTxt.text = "+" + ((MathTool.GetTotalSpeedUpGGrowth(lv) - 1) * 100).ToString("F0") + "%";

        //var PlayerGold = playerData.GetPlayerGold();
        var UpgradeGold = MathTool.GetUpgradeGoldForG(lv);
        //GBtn.interactable = (PlayerGold >= UpgradeGold && lv < maxLevel);
        GBtnTxt.text = UpgradeGold.ToString();
        GBtn.SetActive(lv < maxLevel);
    }

    #endregion

    #region 金币获取

    void GoldBtnClick()
    {
        audioMng.PlayAudioEffect(MusicType.LevelUp);

        int lv = playerData.GetGoldEarningLevel();
        var PlayerGold = playerData.GetPlayerGold();
        var UpgradeGold = MathTool.GetUpgradeGoldForEarnings(lv);
        if (PlayerGold < UpgradeGold)
        {
            MessageTool.ShowMessage("金币不足!");
            return;
        }

        playerData.AddPlayerGold(-UpgradeGold);
        playerData.SetGoldEarningLevel(lv + 1);
        // TDGAItem.OnPurchase("离线金币升级", 1, UpgradeGold);
        // AdSDKCenter.GetInstance().SaveLog("花钱", "离线金币升级", lv.ToString());
        RefreshUI();
    }

    void RefreshGoldEarning()
    {
        int lv = playerData.GetGoldEarningLevel();
        GoldLvTxt.text = /*"Lv." + */lv.ToString();
        GoldEarningTxt.text = MathTool.GetXLv(lv).ToString("F0") /* + "X"*/;
        GoldRateTxt.text = "+" + ((MathTool.GetTotalXGrowth(lv) - 1) * 100).ToString("F0") + "%";

        //var PlayerGold = playerData.GetPlayerGold();
        var UpgradeGold = MathTool.GetUpgradeGoldForEarnings(lv);
        //GoldBtn.interactable = (PlayerGold >= UpgradeGold && lv < maxLevel);
        GoldBtnTxt.text = UpgradeGold.ToString();
        GoldBtn.SetActive(lv < maxLevel);
    }

    #endregion

    #region 设置模块

    void VibrateBtnClick()
    {
        audioMng.BtnClick();

        GameFacade.Instance.isCanVibrate = (PrefsTool.GetCanVibrate() == 1);
        PrefsTool.SetCanVibrate(GameFacade.Instance.isCanVibrate ? 0 : 1);

        RefreshVibrateBtn();
        VibrateTool.Vibrate();
    }

    void RefreshVibrateBtn()
    {
        GameFacade.Instance.isCanVibrate = (PrefsTool.GetCanVibrate() == 1);
        if (GameFacade.Instance.isCanVibrate)
        {
            VibrateBtn.targetGraphic.color = Color.white;
            // VibrateBtn.image.sprite = IsVibrateBtnON;
        }
        else
        {
            VibrateBtn.targetGraphic.color = new Color(1, 1, 1, 0.5f);
            // VibrateBtn.image.sprite = IsVibrateBtnOff;
        }
    }

    //音乐
    void BgMusicBtnClick()
    {
        audioMng.BtnClick();

        playerData.IsBgMusicOn = !playerData.IsBgMusicOn;
        PrefsTool.SetIsBgMusicOn(playerData.IsBgMusicOn ? 1 : 0);

        RefreshBgMusicBtn();
    }

    void RefreshBgMusicBtn()
    {
        if (playerData.IsBgMusicOn)
        {
            BgMusicBtn.image.sprite = IsBgMusicBtnON;
        }
        else
        {
            BgMusicBtn.image.sprite = IsBgMusicBtnOff;
        }

        //BgMusicBtn.targetGraphic.DOFade(playerData.IsBgMusicOn ? 1 : 0.5f, 0);
        audioMng.SetBGM(playerData.IsBgMusicOn);
    }

    //音效
    void SoundBtnClick()
    {
        audioMng.BtnClick();

        playerData.IsSoundOn = !playerData.IsSoundOn;
        PrefsTool.SetIsSoundOn(playerData.IsSoundOn ? 1 : 0);

        RefreshSoundBtn();
    }

    void RefreshSoundBtn()
    {
        if (playerData.IsSoundOn)
        {
            SoundBtn.image.sprite = IsSoundBtnON;
        }
        else
        {
            SoundBtn.image.sprite = IsSoundBtnOff;
        }

        //SoundBtn.targetGraphic.DOFade(playerData.IsSoundOn ? 1 : 0.5f, 0);
        audioMng.SetSound(playerData.IsSoundOn);
    }

    #endregion
}