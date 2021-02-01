using UnityEngine;

public class GuideSystem : ISystem
{
    int pageIndex = 1;
    int MAXCOUNT;
    // public GuideType curType = GuideType.None;
    //
    // public GuideModel GetGuideModel(GuideType guideType) {
    //     GuideModel guideModel = null;
    //     if (guideType != curType) {
    //         curType = guideType;
    //         pageIndex = 1;
    //     }
    //
    //     GetGuideModelAndCount(out guideModel, out MAXCOUNT, pageIndex);
    //
    //     if (pageIndex > MAXCOUNT) {
    //         return null;
    //     }
    //     pageIndex++;
    //     return guideModel;
    // }

    // public void GetGuideModelAndCount(out GuideModel guideModel, out int MAXCOUNT, int index) {
    //     guideModel = null;
    //     MAXCOUNT = int.MaxValue;
    //
    //     switch (curType) {
    //         case GuideType.None:
    //             break;
    //         case GuideType.Dest:
    //             guideModel = guideData.GetGuideDestModel(out MAXCOUNT, index);
    //             break;
    //         case GuideType.SpeedUp:
    //             guideModel = guideData.GetGuideSpeedUpModel(out MAXCOUNT, index);
    //             break;
    //     }
    // }

    public override void OnInit() {
        base.OnInit();
    }

    public override void OnRelease() {
        base.OnRelease();
    }

    public override void OnUpdate() {
        base.OnUpdate();
    }

    // public GuideModel GetNextGuideModel() {
    //     GuideModel guideModel = null;
    //     int index = 0;
    //     if (pageIndex < index) {
    //         return null;
    //     }
    //     GetGuideModelAndCount(out guideModel, out MAXCOUNT, pageIndex - index);
    //
    //     return guideModel;
    // }
    //
    // public ActionType GetNextActionType() {
    //
    //     GuideModel guideModel = GetNextGuideModel();
    //
    //     if (guideModel == null) {
    //         return ActionType.None;
    //     }
    //     else {
    //         return guideModel.ActionType;
    //     }
    // }

    /// <summary>
    /// 引导判断
    /// </summary>
    // public void CheckAction() {
    //     var guidePanle = uiMng.GetPanel<GuidePanel>(PanelType.GuidePanel);
    //
    //     if (guidePanle != null) {
    //
    //         switch (GetNextActionType()) {
    //             case ActionType.None:
    //                 break;
    //             case ActionType.NearTheEnd:
    //                 guidePanle.NextGuide();
    //                 break;
    //             case ActionType.ShowRadar:
    //                 guidePanle.NextGuide();
    //                 guidePanle.SetTipImg(103f, new Vector2(178.8f, 178.8f));
    //                 break;
    //         }
    //     }
    // }
}
