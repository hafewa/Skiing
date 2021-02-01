public class StageModel
{
    public int ID;          
    public string MapName;      //地图名字
    public int WeatherId;       //天气表ID
    public int PassCondition;   //过关条件(排名前几过关)

    public string StartPosName; //起点节点名字

    public int PassTime;        // 过关时间

    public int AILevel;         //AI等级

    public float Wind;          //风阻系数

}

public class SkinModel
{
    public int ID;
    public string Name;                //皮肤名字
    public string TextureName;      //材质贴图
    public string IconName;         //UI Icon
    public float RateMPH;           //皮肤时速加成系数
    public float RateG;             //皮肤加速加成系数
    public float RateX;             //皮肤金币加成系数
    public int UnlockStage;         //解锁关卡
    public int Price;               //购买价格
}

public class WeatherModel
{
    public int ID;
    public string Name;     //天气名字
    public string SkyBox;   //天空盒名字
    public int AmbientMode;  //环境光模式 <!-- AmbientMode:SkyBox=1; Gradient=1; Color=3; -->
    public string AmbientSkyColor;  //天空颜色 AmbientMode=color 参数
    public string AmbientEquatorColor;  //AmbientMode=Gradient 参数
    public string AmbientGroundColor;  //AmbientMode=Gradient 参数

    public string LightColor;   //平行光颜色
    public float LightIntensity;    //平行光强度

    public int Fog;     //是否开启雾效
    public string FogColor; //雾效颜色
}

public class StuntModel
{
    public int ID;
    public string Name;     //特技名
    public string ClipName; //动画名字
    public string IconName; //UI名字
    public float SpeedUpDis;    //加速距离
    public int Price;       //购买价格
    public int UnlockStage;         //解锁关卡
}

public class TipsModel
{
    public int ID;
    public string Desc;     //提示描述
}

public class GuideModel
{
    public int ID;
    public float ContentPosY;   //整体显示内容Y轴坐标
    public string SpeakDec;     //文字信息
    public int ShowMask;        //是否显示半透明背景

    public string ShowHand;     //显示手指
    public string ShowTip;      //显示提示图标

    public ActionType ActionType;   //事件触发类型

    public string HLUIPath;         //高亮UI路径

    public string AnimName;         //ui动画名字
}


