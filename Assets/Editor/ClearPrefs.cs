using UnityEditor;
using UnityEngine;

public class ClearPrefs {

    [MenuItem("Tools/清除注册表")]
    public static void 清除() {
        PlayerPrefs.DeleteAll();
        Debug.LogWarning("UnityPrefs注册表已经清空");
    }
}