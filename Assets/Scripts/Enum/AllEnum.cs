public enum PanelType
{
    None,
    BattlePanel,    //飞行界面
    BattlePanelV2,  //飞行界面 v2
    GameOverPanel,  //结束界面
    RankPanel,      //排名界面
    StartPanel,     //开始界面
    RolePanel,      //角色界面
    OfflinePanel,   //离线收益界面
    GainPanel,      //皮肤特技获得界面
    LoadingPanel,   //加载界面
    RewardPanel,    //奖励获取界面

    GuidePanel, //引导界面
}
public enum MusicType
{
    None,
    BGM_Battle0, //背景音乐
    BGM_Battle1, //背景音乐
    BGM_Battle2, //背景音乐
    BGM_Battle3, //背景音乐
    BGM_Battle4, //背景音乐
    BGM_Battle5, //背景音乐
    BGM_Battle6, //背景音乐
    BGM_Battle7, //背景音乐
    BGM_Battle8, //背景音乐
    Die,    //死亡
    Lose,   //失败
    Win,    //胜利
    SpeedUp,//加速
    Start,  //开始
    Wind,   //风声

    //穿圈反馈音效
    Perfect,
    Great,          
    Good,

    BtnClick,   //通用按钮是点击界面按钮的音效
    BuyOk,      //购买角色和特技成功
    GetGold,    //获取金币
    LevelUp,    //升级属性
    CountDown,  //倒计时
    Surpass,    //超越对手
}

public enum ActionType
{
    None,
    NearTheEnd,     //靠近终点事件
    ShowRadar,      //显示终点雷达
}

public enum AnimPlayModel
{
    Play,
    PlayQueued,
}

public enum Encourage
{
    Perfect = 0,
    Great = 1,
    Good = 2,
    Miss = 3,
}