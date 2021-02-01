using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MessageTool
{
    private static GameObject textClone;
    public static void ShowMessage(string msg, float showTime = 0.2f) {
        if (textClone == null) {
            textClone = Resources.Load<GameObject>("Prefabs/MessageTxt/MessageTxt");
        }
        GameObject go = GameObject.Instantiate(textClone, GameObject.Find("Canvas").transform);
        var txt = go.GetComponent<Text>();
        txt.text = msg;

        float ly = Random.Range(-350f, -200f);
        float my = Random.Range(80f, 160f);
        float hide = Random.Range(300f, 500f);
        go.transform.localPosition = new Vector3(0, ly, 0);

        DOTween.Sequence()
            .Append(go.transform.DOLocalMoveY(my, 0.3f))
            .AppendInterval(showTime)
            .Append(go.transform.DOLocalMoveY(hide, 1.2f))
            .Join(txt.DOFade(0, 1.2f))
            .OnComplete(() =>
            {
                GameObject.Destroy(go);
            });
    }
}