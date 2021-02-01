using UnityEngine.UI;
using UnityEngine;
using System.Linq;
using DG.Tweening;

public class LoadingPanel : BasePanel
{
    public Text TipsTxt;

    public Image Progress;
    public Text ThreeePointTxt;
    Tweener points3Txt;

    public float minStayTime = 1.5f;
    public float m_LastTime;
    public static bool g_bFirstLoad = false;
    bool isLoadCompleted; //加载完成

    public override void Awake()
    {
        base.Awake();
        points3Txt = ThreeePointTxt.DOText("...", 1.5f).SetLoops(-1).SetAutoKill(false);
    }

    public override void OnEnter()
    {
        base.OnEnter();
        m_LastTime = 0;
        Progress.fillAmount = 0;
        points3Txt.Restart();

        // TipsTxt.text = XmlTool.Instance.tipsCfg.ToArray().ArrayRandom().Value.Desc;
        isLoadCompleted = false;
    }

    public void MapLoaded()
    {
        isLoadCompleted = true;
    }

    private void Update()
    {
        m_LastTime += Time.deltaTime;

        //if (mapMng.request != null) {
        //    if (!mapMng.request.isDone) {
        //        Progress.fillAmount = mapMng.request.progress;
        //    }
        //    else {
        //        Progress.fillAmount += Time.deltaTime;
        //    }
        //}
        //else 
        {
            Progress.fillAmount += Time.deltaTime;
        }

        //loading至少要有1.5秒，否则提示文字看不清楚的
        if (isLoadCompleted && m_LastTime > minStayTime)
        {
            uiMng.PushPanel(PanelType.StartPanel);
            isLoadCompleted = false;
        }
    }
}