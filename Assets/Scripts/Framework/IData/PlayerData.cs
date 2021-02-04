using System.Linq;

public class PlayerData : IData
{
    public bool IsBgMusicOn;
    public bool IsSoundOn;

    public override void OnInit() {
        IsBgMusicOn = PrefsTool.GetIsBgMusicOn();
        GameFacade.Instance.audioMng.SetBGM(IsBgMusicOn);

        IsSoundOn = PrefsTool.GetIsSoundOn();
        GameFacade.Instance.audioMng.SetSound(IsSoundOn);
    }
    public override void OnUpdate() { }
    public override void OnRelease() { }

    public int GetPlayerGold(string userID = "1001") {
        return PrefsTool.GetPlayerGold(userID);
    }
    public void AddPlayerGold(int gold, string userID = "1001") {
        PrefsTool.AddPlayerGold(gold, userID);
    }

    public int GetMaxSpeedLevel(string userID = "1001") {
        return PrefsTool.GetMaxSpeedLevel(userID);
    }
    public void SetMaxSpeedLevel(int level, string userID = "1001") {
        PrefsTool.SetMaxSpeedLevel(level, userID);
    }

    public int GetSpeedUpLevel(string userID = "1001") {
        return PrefsTool.GetSpeedUpLevel(userID);
    }
    public void SetSpeedUpLevel(int level, string userID = "1001") {
        PrefsTool.SetSpeedUpLevel(level, userID);
    }

    public int GetGoldEarningLevel(string userID = "1001") {
        return PrefsTool.GetGoldEarningLevel(userID);
    }
    public void SetGoldEarningLevel(int lv, string userID = "1001") {
        PrefsTool.SetGoldEarningLevel(lv, userID);
    }

    #region 身体皮肤

    public int GetPlayerSkinID(string userID = "1001") {
        return PrefsTool.GetPlayerSkinID(userID);
    }
    public void SetPlayerSkinID(int id, string userID = "1001") {
        PrefsTool.SetPlayerSkinID(id, userID);
    }

    public string[] GetOwnedSkinIDs(string userID = "1001") {
        return PrefsTool.GetOwnedSkinIDs(userID).Split(',');
    }
    public void SetOwnedSkinIDs(int skinID, string userID = "1001") {
        var array = GetOwnedSkinIDs(userID);
        if (!array.Contains(skinID.ToString())) {
            PrefsTool.SetOwnedSkinIDs(string.Join(",", array) + "," + skinID, userID);
        }
    }

    #endregion



    #region 滑雪板皮肤

    public int GetPlayerSkisID(string userID = "1001") {
        return PrefsTool.GetPlayerSkisID(userID);
    }
    public void SetPlayerSkisID(int id, string userID = "1001") {
        PrefsTool.SetPlayerSkisID(id, userID);
    }

    public string[] GetOwnedSkisIDs(string userID = "1001")
    {
        return PrefsTool.GetOwnedSkisIDs(userID).Split(',');
    } 
    public void SetOwnedSkisIDs(int skinID, string userID = "1001") {
        var array = GetOwnedSkisIDs(userID);
        if (!array.Contains(skinID.ToString())) {
            PrefsTool.SetOwnedSkisIDs(string.Join(",", array) + "," + skinID, userID);
        }
    }

    #endregion

    public string[] GetOwnedStuntIDs(string userID = "1001") {
        return PrefsTool.GetOwnedStuntIDs(userID).Split(',');
    }
    public void SetOwnedStuntIDs(int stuntID, string userID = "1001") {
        var array = GetOwnedStuntIDs(userID);
        if (!array.Contains(stuntID.ToString())) {
            PrefsTool.SetOwnedStuntIDs(string.Join(",", array) + "," + stuntID, userID);
        }
    }
    public bool HasStunt(int stuntID, string userID = "1001") {
        return GetOwnedStuntIDs(userID).Contains(stuntID.ToString());
    }
    
    
}
