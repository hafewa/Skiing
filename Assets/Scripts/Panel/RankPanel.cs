using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RankPanel : BasePanel
{
    public Text GoldTxt;
    public Text GoldTxt2;
    public Text GoldDoubleTxt;
    
    int rewardGold;
    
    public RankItem[] items;

    // public Sprite rankBg1;
    // public Sprite rankBg2;
    // public Sprite rankBgSelf;
    
    public Button GetGoldBtn;
    public Button GetDoubleGoldBtn;

    public Transform RewardPanel;

    public override void Awake()
    {
        base.Awake();

        UnityAction<BaseEventData> click = new UnityAction<BaseEventData>(PointerDown);
        EventTrigger.Entry myclick = new EventTrigger.Entry();
        myclick.eventID = EventTriggerType.PointerDown;
        myclick.callback.AddListener(click);
        var evenTrigger = gameObject.AddComponent<EventTrigger>();
        evenTrigger.triggers.Add(myclick);
        
        
        GetGoldBtn.onClick.AddListener(() => { GetGold(1); });
        GetDoubleGoldBtn.onClick.AddListener(() => { GetGold(2); });
    }

    public override void OnEnter()
    {
        base.OnEnter();
        RewardPanel.SetActive(false);

        Player.Instance.isGameOver = true;
        audioMng.PlayAudioEffect(MusicType.Win);
        RefreshUI();
    }

    public void PointerDown(BaseEventData baseEventData)
    {
        RewardPanel.SetActive(true);
    }

    void GetGold(int times = 1) {
        audioMng.PlayAudioEffect(MusicType.GetGold);

        Player.Instance.ReStart();
        playerData.AddPlayerGold(rewardGold * times);

        if (times == 2) {
            
        }
        else {
            
        }

    }

    void RefreshUI()
    {
        var list = Player.Instance.GetRankArray();

        int rank;
        int playerRank = 1;
        for (int i = 0; i < list.Length; i++) {
            rank = i + 1;
            ICharacter character = list[i];
            Sprite temp;

            //var nameStr = "Player" + rank;
            var nameStr = character.name;
            if (character == Player.Instance) {
                nameStr = "我";
                // temp = rankBgSelf;
                playerRank = rank;
                items[i].Flag.SetActive(true);
            }
            else {
                items[i].Flag.SetActive(false);
                // if (rank % 2 == 1) {
                //     temp = rankBg1;
                // }
                // else {
                //     temp = rankBg2;
                // }
            }

            // items[i].transform.GetComponent<Image>().sprite = temp;
            items[i].NameTxt.text = nameStr;
            
            int passStageGold = MathTool.GetPassStageGold(stageData.m_PassStageCount);//200*1.02^关卡数
            if (rank > 3) {
                passStageGold /= 2;
            }
            character.SpeedUpGold += passStageGold;

            items[i].GoldTxt.text = character.SpeedUpGold.ToString();
        }
        
        int times = 1;

        switch (playerRank) {
            case 1:
                times = 5;
                break;
            case 2:
                times = 3;
                break;
            case 3:
                times = 2;
                break;
            default:
                times = 1;
                break;
        }
        
        var stageModel = stageData.m_CurStageModel;
        if (playerRank <= stageModel.PassCondition) {
            
            var PassStageCount = stageData.m_PassStageCount + 1;
            stageData.SetPassStageCount(PassStageCount);
            
            
            //张地图如果已经随机到一个BGM了，之后就不再换了
            uiMng.GetPanel<BattlePanel>(PanelType.BattlePanel)?.SetBattleBGM();
        }
        
        rewardGold = Player.Instance.SpeedUpGold * times;
        GoldTxt.text = rewardGold.ToString();
        GoldTxt2.text = rewardGold.ToString();
        GoldDoubleTxt.text = (rewardGold * 2).ToString();
    }
}