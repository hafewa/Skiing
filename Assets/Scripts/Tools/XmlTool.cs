using System.Collections.Generic;

public class XmlTool : ClassInstanceBase<XmlTool>
{
    public Dictionary<int, SkinModel> skinCfg = new Dictionary<int, SkinModel>();
    public Dictionary<int, StageModel> stageCfg = new Dictionary<int, StageModel>();
    public Dictionary<int, WeatherModel> weatherCfg = new Dictionary<int, WeatherModel>(); 
    public Dictionary<int, StuntModel> stuntCfg = new Dictionary<int, StuntModel>();
    public Dictionary<int, TipsModel> tipsCfg = new Dictionary<int, TipsModel>();

    public Dictionary<int, GuideModel> GuideDestCfg = new Dictionary<int, GuideModel>();
    public Dictionary<int, GuideModel> GuideSpeedUpCfg = new Dictionary<int, GuideModel>();

    public XmlTool() {
        LoadAllConfigs();//构造里面加载所有配置
    }

    private void LoadAllConfigs() {
        // skinCfg = LoadingXML<SkinModel>("Skin");//皮肤设置
        stageCfg = LoadingXML<StageModel>("Stage");//关卡配置
        weatherCfg = LoadingXML<WeatherModel>("Weather");//天气配置
        // stuntCfg = LoadingXML<StuntModel>("Stunt");//特技动作配置
        // tipsCfg = LoadingXML<TipsModel>("Tips");//小提示配置
        //
        // GuideDestCfg = LoadingXML<GuideModel>("GuideDestination");//终点引导配置
        // GuideSpeedUpCfg = LoadingXML<GuideModel>("GuideSpeedUp");//加速圈引导配置
    }

    // Loading所有XML或者指定好的XML
    private Dictionary<int, T> LoadingXML<T>(string xmlName) {
        return XMLParser.ParserXML<T>(xmlName);//调用XML解析类的解析方法
    }
}