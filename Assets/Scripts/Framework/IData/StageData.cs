public class StageData : IData
{
    public int m_PassStageCount;
    public StageModel m_CurStageModel;
    public int nFailedTime  = 0; //失败次数
    public override void OnInit() { }
    public override void OnUpdate() { }
    public override void OnRelease() { }

    private int GetPassStageCount(string userID = "1001") {
        return PrefsTool.GetPassStageCount(userID);
    }

    public void SetPassStageCount(int passStageCount, string userID = "1001") {
        PrefsTool.SetPassStageCount(passStageCount, userID);
        nFailedTime = 0;
    }

    public override void SetData() {
        m_PassStageCount = GetPassStageCount();
        nFailedTime = 0;
        int curStage = (m_PassStageCount < XmlTool.Instance.stageCfg.Count ? m_PassStageCount + 1 : m_PassStageCount % XmlTool.Instance.stageCfg.Count + 1);
        
        m_CurStageModel = XmlTool.Instance.stageCfg[curStage];
    }
}