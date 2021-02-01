using UnityEngine;

/// <summary>
/// 玩家数据工具类
/// </summary>
public class PrefsTool
{
    private const int USERID = 1001;

    /// <summary>
    /// 获取已过关卡
    /// </summary>
    /// <param name="userID"></param>
    /// <returns></returns>
    public static int GetPassStageCount(string userID = "1001") {
        string key = "UserID" + userID.ToString() + "StagePassCount";
        if (!PlayerPrefs.HasKey(key)) {
            return 0;
        }
        return PlayerPrefs.GetInt(key);
    }
    public static void SetPassStageCount(int passStageCount, string userID = "1001") {
        string key = "UserID" + userID.ToString() + "StagePassCount";
        PlayerPrefs.SetInt(key, passStageCount);
    }

    /// <summary>
    /// 获取玩家金币
    /// </summary>
    /// <param name="userID"></param>
    /// <returns></returns>
    public static int GetPlayerGold(string userID = "1001") {
        string key = "UserID" + userID.ToString() + "UserGold";
        if (!PlayerPrefs.HasKey(key)) {
            return 0;
        }
        return PlayerPrefs.GetInt(key);
    }
    public static void SetPlayerGold(int goldCount, string userID = "1001") {
        string key = "UserID" + userID.ToString() + "UserGold";
        PlayerPrefs.SetInt(key, goldCount);
    }
    public static void AddPlayerGold(int goldCount, string userID = "1001") {
        string key = "UserID" + userID.ToString() + "UserGold";
        PlayerPrefs.SetInt(key, GetPlayerGold() + goldCount);
    }

    /// <summary>
    ///  获取最大速度等级
    /// </summary>
    /// <param name="userID"></param>
    /// <returns></returns>
    public static int GetMaxSpeedLevel(string userID = "1001") {
        string key = "UserID" + userID.ToString() + "MaxSpeedLevel";
        if (!PlayerPrefs.HasKey(key)) {
            return 1;
        }
        return PlayerPrefs.GetInt(key);

    }
    public static void SetMaxSpeedLevel(int level, string userID = "1001") {
        string key = "UserID" + userID.ToString() + "MaxSpeedLevel";
        PlayerPrefs.SetInt(key, level);
    }

    /// <summary>
    /// 速度提升等级
    /// </summary>
    /// <param name="userID"></param>
    /// <returns></returns>
    public static int GetSpeedUpLevel(string userID = "1001") {
        string key = "UserID" + userID.ToString() + "SpeedUpLevel";
        if (!PlayerPrefs.HasKey(key)) {
            return 1;
        }
        return PlayerPrefs.GetInt(key);
    }
    public static void SetSpeedUpLevel(int level, string userID = "1001") {
        string key = "UserID" + userID.ToString() + "SpeedUpLevel";
        PlayerPrefs.SetInt(key, level);
    }

    /// <summary>
    /// 获取当前地图索引
    /// </summary>
    /// <param name="userID"></param>
    /// <returns></returns>
    public static int GetMapIndex(string userID = "1001") {
        string key = "UserID" + userID.ToString() + "MapIndex";
        if (!PlayerPrefs.HasKey(key)) {
            return 0;
        }
        return PlayerPrefs.GetInt(key);
    }
    public static void SetMapIndex(int mapIndex, string userID = "1001") {
        string key = "UserID" + userID.ToString() + "MapIndex";
        PlayerPrefs.SetInt(key, mapIndex);
    }

    /// <summary>
    /// 震动开关设置
    /// </summary>
    /// <param name="userID"></param>
    /// <returns></returns>
    public static int GetCanVibrate(string userID = "1001") {
        string key = "UserID" + userID.ToString() + "CanVibrate";
        if (!PlayerPrefs.HasKey(key)) {
            return 1;
        }
        return PlayerPrefs.GetInt(key);
    }
    public static void SetCanVibrate(int bol, string userID = "1001") {
        string key = "UserID" + userID.ToString() + "CanVibrate";
        PlayerPrefs.SetInt(key, bol);
    }

    /// <summary>
    /// 金币获取等级
    /// </summary>
    /// <param name="userID"></param>
    /// <returns></returns>
    public static int GetGoldEarningLevel(string userID = "1001") {
        string key = "UserID" + userID.ToString() + "GoldEarningLevel";
        if (!PlayerPrefs.HasKey(key)) {
            return 1;
        }
        return PlayerPrefs.GetInt(key);
    }
    public static void SetGoldEarningLevel(int lv, string userID = "1001") {
        string key = "UserID" + userID.ToString() + "GoldEarningLevel";
        PlayerPrefs.SetInt(key, lv);

    }

    /// <summary>
    /// 获取玩家当前皮肤ID
    /// </summary>
    /// <param name="userID"></param>
    /// <returns></returns>
    public static int GetPlayerSkinID(string userID = "1001") {
        string key = "UserID" + userID.ToString() + "PlayerSkinID";
        if (!PlayerPrefs.HasKey(key)) {
            return 1;
        }
        return PlayerPrefs.GetInt(key);
    }
    public static void SetPlayerSkinID(int id, string userID = "1001") {
        string key = "UserID" + userID.ToString() + "PlayerSkinID";
        PlayerPrefs.SetInt(key, id);
    }

    /// <summary>
    /// 获取已经拥有的皮肤Id,以逗号隔开
    /// </summary>
    /// <param name="userID"></param>
    /// <returns></returns>
    public static string GetOwnedSkinIDs(string userID = "1001") {
        string key = "UserID" + userID.ToString() + "OwnedSkinIDs";
        if (!PlayerPrefs.HasKey(key)) {
            return "1";
        }
        return PlayerPrefs.GetString(key);
    }
    public static void SetOwnedSkinIDs(string ids, string userID = "1001") {
        string key = "UserID" + userID.ToString() + "OwnedSkinIDs";
        PlayerPrefs.SetString(key, ids);
    }

    /// <summary>
    /// 获取已拥有的特效
    /// </summary>
    /// <param name="userID"></param>
    /// <returns></returns>
    public static string GetOwnedStuntIDs(string userID = "1001") {
        string key = "UserID" + userID.ToString() + "OwnedStuntIDs";
        if (!PlayerPrefs.HasKey(key)) {
            return "1,4,7";
        }
        return PlayerPrefs.GetString(key);
    }
    public static void SetOwnedStuntIDs(string ids, string userID="1001") {
        string key = "UserID" + userID.ToString() + "OwnedStuntIDs";
        PlayerPrefs.SetString(key, ids);
    }

    /// <summary>
    /// 返回音乐开关设置 1:开    0:关
    /// </summary>
    /// <param name="userID"></param>
    /// <returns></returns>
    public static bool GetIsBgMusicOn(string userID = "1001") {
        string key = "UserID" + userID.ToString() + "IsBgMusicOn";
        if (!PlayerPrefs.HasKey(key)) {
            return true;
        }
        return PlayerPrefs.GetInt(key) == 1;
    }
    public static void SetIsBgMusicOn(int value, string userID = "1001") {
        string key = "UserID" + userID.ToString() + "IsBgMusicOn";
        PlayerPrefs.SetInt(key, value);
    }

    /// <summary>
    /// 返回音效开关设置 1:开    0:关
    /// </summary>
    /// <param name="userID"></param>
    /// <returns></returns>
    public static bool GetIsSoundOn(string userID = "1001") {
        string key = "UserID" + userID.ToString() + "IsSoundOn";
        if (!PlayerPrefs.HasKey(key)) {
            return true;
        }
        return PlayerPrefs.GetInt(key) == 1;
    }
    public static void SetIsSoundOn(int value, string userID = "1001") {
        string key = "UserID" + userID.ToString() + "IsSoundOn";
        PlayerPrefs.SetInt(key, value);
    }
}
