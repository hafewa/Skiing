using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BodyItem : MonoBehaviour, IPointerClickHandler, IPointerDownHandler
{
    public Image Dot;
    public RolePanel rolePanel;
    public Transform Lock;
    public SkinModel skinModel;
    public Button BuyBtn;
    public Text BuyText;

    public Image SkinIcon;

    public PlayerData playerData {
        get {
            return GameFacade.Instance.dataMng.playerData;
        }
    }
    public AudioManager audioMng {
        get {
            return GameFacade.Instance.audioMng;
        }
    }

    private void Awake() {
        BuyBtn.onClick.AddListener(BuyBtnClick);
    }


    void BuyBtnClick() {
        audioMng.BtnClick();
        var playerGold = playerData.GetPlayerGold();
        if (skinModel.Price > playerGold) {
            // MessageTool.ShowMessage("金币不足!", 0.5f);
             rolePanel.OpenGetGoldPanel();
        }
        else {
            //TODO弹出购买界面
            rolePanel.OpenBuyPanel(BuyType.Body, skinModel.Price, skinModel.ID, BuyBtn);
        }
    }

    public void RefreshUI(int passStageCount, string[] ownedSkinIDs) {
        //var skinModel = XmlTool.Instance.skinCfg[skinID];
        if (skinModel == null) return;

        SkinIcon.sprite = Resources.Load<Sprite>("UI/Body/" + skinModel.IconName);

        Lock.SetActive(passStageCount < skinModel.UnlockStage);

        BuyText.text = skinModel.Price.ToString();
        BuyBtn.SetActive(!Lock.gameObject.activeSelf && !ownedSkinIDs.Contains(skinModel.ID.ToString()));

        Dot.SetActive(BuyBtn.gameObject.activeSelf && skinModel.Price <= playerData.GetPlayerGold());
    }

    public void OnPointerClick(PointerEventData eventData) {
        //MessageTool.ShowMessage("OnPointerClick!", 0.5f);
        audioMng.BtnClick();
        if (skinModel == null) return;

        //switch (skinModel.ID)
        //{
        //    case 1:
        //        FunctionDispatcher.HandleItemClick(MainListItemId.Ad, MainListItemId.SHOW_FULL_SCREEN_AD);
        //        FunctionDispatcher.HandleItemClick(MainListItemId.Ad, MainListItemId.LOAD_FULL_SCREEN_AD_V);
        //        break;
        //    case 2:
        //        FunctionDispatcher.HandleItemClick(MainListItemId.Ad, MainListItemId.SHOW_REWARD_AD);
        //        FunctionDispatcher.HandleItemClick(MainListItemId.Ad, MainListItemId.LOAD_REWARD_AD_V);
        //        break;
        //    case 3:
                
        //        break;
        //    case 4:
                
        //        break;
        //}
        //return;

        if (Lock.gameObject.activeSelf) {
            //Debug.LogFormat("皮肤过第{0}关后解锁!", skinModel.UnlockStage);
            MessageTool.ShowMessage(string.Format("通过{0}关解锁新皮肤：{1}!", skinModel.UnlockStage, skinModel.Name), 1f);
            return;
        }

        if (BuyBtn.IsActive()) {
            BuyBtnClick();
            return;
        }

        var ownedSkinIDs = playerData.GetOwnedSkinIDs();
        if (ownedSkinIDs.Contains(skinModel.ID.ToString())) {
            Player.Instance.SetPlayerSkin(skinModel.ID);
            rolePanel.ShowSkinProperty(skinModel.ID);
        }
    }

    public void OnPointerDown(PointerEventData eventData) {
        //MessageTool.ShowMessage("OnPointerDown!", 0.5f);
    }
}
