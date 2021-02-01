using UnityEngine;
using System.Collections;

public class ISystem
{

    // public MapManager mapMng {
    //
    //     get {
    //         return GameFacade.Instance.mapMng;
    //     }
    // }
    // protected GuideData guideData {
    //     get {
    //         return GameFacade.Instance.dataMng.guideData;
    //     }
    // }
    
    protected UIManager uiMng {
        get {
            return GameFacade.Instance.uiMng;
        }
    }

    public virtual void OnInit() { }
    public virtual void OnUpdate() { }
    public virtual void OnRelease() { }
}