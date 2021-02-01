using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

static public class UnityEngineExtensions
{
    /// <summary>
    /// Returns the component of Type type. If one doesn't already exist on the GameObject it will be added.
    /// </summary>
    /// <typeparam name="T">The type of Component to return.</typeparam>
    /// <param name="gameObject">The GameObject this Component is attached to.</param>
    /// <returns>Component</returns>
    static public T GetOrAddComponent<T>(this GameObject gameObject) where T : Component {
        return gameObject.GetComponent<T>() ?? gameObject.AddComponent<T>();
    }

    static public T ListRandom<T>(this List<T> list) {
        int index = Random.Range(0, list.Count);
        return list[index];
    }

    static public T ArrayRandom<T>(this T[] list) {
        int index = Random.Range(0, list.Length);
        return list[index];
    }
    /// <summary>
    /// 列表中取出一定个数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="count">随机的个数</param>
    /// <returns></returns>
    static public List<T> ListRandom<T>(this List<T> list, int count) {
        List<int> indexList = new List<int>();
        for (int i = 0; i < list.Count; i++) {
            indexList.Add(i);
        }

        List<T> tempList = new List<T>();
        for (int i = 0; i < count; i++) {
            var index = indexList.ListRandom();
            indexList.Remove(index);
            tempList.Add(list[index]);
        }

        return tempList;

    }

    /// <summary>
    /// 设置组件是否激活
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="component"></param>
    /// <param name="boo"></param>
    static public void SetActive<T>(this T component, bool boo = true) where T : Component {
        component.gameObject.SetActive(boo);
    }
    /// <summary>
    /// 文字组件设置内容
    /// </summary>
    /// <param name="Tex"></param>
    /// <param name="text"></param>
    static public void SetText(this Text Tex, string text) {
        Tex.text = text;
    }

    static public int Last(this string[] arr) {
        return arr.Length > 0 ? int.Parse(arr[arr.Length - 1]) : 0;
    }

    public static void SetText(this Transform transform, string nodePath, string txt, Color color) {
        var node = transform.Find(nodePath);
        if (node != null) {
            var text = node.GetComponent<Text>();
            text.text = txt;
            text.color = color;
        }
    }

    /// <summary>
    /// string2Color
    /// </summary>
    /// <param name="str"></param>
    /// <param name="is255"></param>
    /// <returns></returns>
    public static Color ToColor(this string str, bool is255 = true) {
        var strList = str.Split(',');
        float den = (is255 ? 255 : 1);

        float alpha = (strList.Length == 4 ? float.Parse(strList[3]) / den : 1);

        return new Color(float.Parse(strList[0]) / den, float.Parse(strList[1]) / den, float.Parse(strList[2]) / den, alpha);
    }
}