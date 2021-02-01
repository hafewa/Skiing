using UnityEngine;
using UnityEngine.UI;

public class RanNameTool : ClassInstanceBase<RanNameTool>
{

    string[] nameOne;
    string[] nameTwo;
    //string[] nameThree;
    public RanNameTool() {
        this.LoadParserNameConfig();
    }
    private void LoadParserNameConfig() {
        TextAsset textAsset = Resources.Load<TextAsset>("Configs/Text/NameConfig");
        string textt = textAsset.text.Replace("\r\n", "|");
        string[] strs = textt.Split('S');
        string nameOneTemp = strs[0];
        nameOne = nameOneTemp.Split('|');//奇数全是姓

        string nameTwoTemp = strs[1];
        nameTwo = nameTwoTemp.Split('|');//奇数全是名

        //string nameThreeTemp = strs[2];
        //nameThree = nameThreeTemp.Split('|');//奇数全是名

    }

    private int ReturnOddNum(int num) {
        if (num % 2 != 0) {
            return num;
        }
        else {
            if (num < 200) {
                return num + 1;
            }
            else {
                return num - 1;
            }
        }
    }

    public string RandomName() {
        int one = ReturnOddNum(UnityEngine.Random.Range(1, 201));
        int two = ReturnOddNum(UnityEngine.Random.Range(1, 201));
        //int three = ReturnOddNum(UnityEngine.Random.Range(1, 201));

        string realName = nameOne[one].Split('=')[1] + nameTwo[two].Split('=')[1];// + nameThree[three].Split('=')[1];
        return realName;
    }
}