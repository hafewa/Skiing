using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum BuyType
{
    Body,//皮肤
    Ski,//滑雪板
}

public class RolePanel : BasePanel, IPointerDownHandler, IDragHandler
{
    public Text nameTxt;
    public Text GoldTxt;
    
    public Button ReturnBtn;

    public override void Awake() {
        base.Awake();
        Init();
    }

    private void Init()
    {
        ReturnBtn.onClick.AddListener(() =>
        {
            audioMng.BtnClick();
            // if (charCamTrans != null)
            // {
            //     charCamTrans.gameObject.SetActive(false);
            // }
            //
            // Player.Instance.SetLocalEulerAngles(Vector3.zero);
            Player.Instance.fsm.PerformTransition(Transition.Ready);
            uiMng.PopPanel();
        });
    }
    
    public override void OnEnter() {
        base.OnEnter();

        RefreshUI();

        // //设置人物展示相机的相对位置
        // charCamTrans.localPosition = Player.Instance.transform.position + Player.Instance.transform.forward * 2.347f + new Vector3(0, 1.39f, 0);
        // charCamTrans.localEulerAngles = new Vector3(4f, 180f + Player.Instance.transform.localEulerAngles.y, 0);
        // charCamTrans.localScale = Vector3.one;
        // charCamTrans.gameObject.SetActive(true);
        //
        // Player.Instance.EnterRolePanel();
        mapMng.SetRolePanelEnvironment();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnDrag(PointerEventData eventData)
    {
    }

    public void RefreshGold() {
        GoldTxt.text = PrefsTool.GetPlayerGold().ToString();
    }
    
    

    public void RefreshUI() {
        // GetGoldPanel.SetActive(false);
        // BuyPanel.SetActive(false);
        RefreshGold();

        // RefreshContent();
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
    /// 显示特技属性
    /// </summary>
    /// <param name="modelID"></param>
    public void ShowSkiProperty(int modelID) {
        var stuntModel = XmlTool.Instance.stuntCfg[modelID];
        nameTxt.text = stuntModel.Name;
        // SpeedUpDisTxt.text = string.Format("冲刺距离 {0}M", stuntModel.SpeedUpDis * 50);
        // PlayStunt(stuntModel.ClipName);
    }
}