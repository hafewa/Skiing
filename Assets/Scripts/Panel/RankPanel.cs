using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RankPanel : BasePanel
{
    public Transform[] items;

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
                items[i].transform.Find("Flag").SetActive(true);
            }
            else {
                items[i].transform.Find("Flag").SetActive(false);
                // if (rank % 2 == 1) {
                //     temp = rankBg1;
                // }
                // else {
                //     temp = rankBg2;
                // }
            }

            // items[i].transform.GetComponent<Image>().sprite = temp;
            items[i].transform.Find("NameTxt").GetComponent<Text>().text = nameStr;
        }
        
        var stageModel = stageData.m_CurStageModel;
        if (playerRank <= stageModel.PassCondition) {
            
            var PassStageCount = stageData.m_PassStageCount + 1;
            stageData.SetPassStageCount(PassStageCount);
            
            
            //张地图如果已经随机到一个BGM了，之后就不再换了
            uiMng.GetPanel<BattlePanel>(PanelType.BattlePanel)?.SetBattleBGM();
        }
        
    }
}