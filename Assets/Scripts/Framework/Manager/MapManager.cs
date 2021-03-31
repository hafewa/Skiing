using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : IManager
{
    public Transform mapRoot;
    public Light DirectionalLight;

    public StageData stageData {
        get {
            return GameFacade.Instance.dataMng.stageData;
        }
    }

    public UIManager uiMng {

        get {
            return GameFacade.Instance.uiMng;
        }
    }

    int mapQueCount = 2;
    Queue<string> mapQue = new Queue<string>();//地图队列,限制地图加载的个数
    public override void OnInit() {
        mapRoot = GameObject.Find("MapRoot").transform;
        DirectionalLight = GameObject.Find("Directional Light").GetComponent<Light>();

        for (int i = 0; i < mapRoot.childCount; i++) {
            var child = mapRoot.GetChild(i);
            mapQue.Enqueue(child.name);
        }
    }
    public override void OnUpdate() { }
    public override void OnRelease() { }

    public Transform curMap;
    public ResourceRequest request;
    public void LoadMap() {
        uiMng.PushPanel(PanelType.LoadingPanel);
        
        var stageModel = stageData.m_CurStageModel;
        string mapName = "Snow Mountain";
        if (stageModel != null) {
            mapName = stageModel.MapName;
        }

        curMap = null;
        for (int i = 0; i < mapRoot.childCount; i++) {
            var child = mapRoot.GetChild(i);
            bool isCurMap = (child.name == mapName);
            child.SetActive(isCurMap);
            if (isCurMap) {
                curMap = child.transform;
                Player.Instance.SetMapData(curMap);
            }
        }

        if (curMap == null) {

            mapQue.Enqueue(mapName);
            ClampMapQue(mapQueCount);

            GameFacade.Instance.StartCoroutine(LoadAsyncMap(mapName));
            //var map = Resources.Load<GameObject>("Prefabs/Map/" + mapName);
            //var aa = Object.Instantiate(map, mapRoot);
            //aa.name = mapName;
            //Player.Instance.SetMapData(aa.transform);
        }
        SetEnvironment();
    }

    /// <summary>
    /// 显示加载的地图个数
    /// </summary>
    /// <param name="count"></param>
    void ClampMapQue(int count) {
        while (mapQue.Count > count) {
            var mapName = mapQue.Dequeue();
            var child = mapRoot.Find(mapName);
            if (child != null) {
                Object.Destroy(child.gameObject);
            }
        }
    }

    IEnumerator LoadAsyncMap(string mapName) {
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(0.1f);

        request = Resources.LoadAsync<GameObject>("Prefabs/Map/" + mapName);        
        yield return request;

        var mapT = request.asset as GameObject;
        if (!mapT) {
            Debug.Log("地图异步加载失败！");
        }
        else {
            //Debug.Log("地图异步加载完成了！");
            var aa = Object.Instantiate(mapT, mapRoot);
            aa.name = mapName;
            curMap = aa.transform;
            Player.Instance.SetMapData(curMap);
        }
    }

    public void SetEnvironment() {
        //return;

        var stageModel = stageData.m_CurStageModel;
        if (stageModel != null) {
            if (XmlTool.Instance.weatherCfg.ContainsKey(stageModel.WeatherId)) {
                var weatherModel = XmlTool.Instance.weatherCfg[stageModel.WeatherId];

                DirectionalLight.color = weatherModel.LightColor.ToColor();
                DirectionalLight.intensity = weatherModel.LightIntensity;

                RenderSettings.skybox = Resources.Load<Material>("Materials/SkyBox/" + weatherModel.SkyBox);
                RenderSettings.ambientMode = (UnityEngine.Rendering.AmbientMode)weatherModel.AmbientMode;
                RenderSettings.ambientSkyColor = weatherModel.AmbientSkyColor.ToColor(false);
                if (weatherModel.AmbientMode == 1) {
                    RenderSettings.ambientEquatorColor = weatherModel.AmbientSkyColor.ToColor(false);
                    RenderSettings.ambientGroundColor = weatherModel.AmbientSkyColor.ToColor(false);
                }

                RenderSettings.fog = (weatherModel.Fog == 1);
                if (RenderSettings.fog) {
                    RenderSettings.fogMode = FogMode.Linear;
                    RenderSettings.fogColor = weatherModel.FogColor.ToColor();
                    RenderSettings.fogStartDistance = 150;
                    RenderSettings.fogEndDistance = 350;
                }
            }
            else {
                Debug.LogErrorFormat("天气配置ID:{0}不存在", stageModel.WeatherId);
            }
        }
        else {
            Debug.LogErrorFormat("该关卡配置为空");
        }
    }

    public void SetRolePanelEnvironment() {
        RenderSettings.ambientMode = (UnityEngine.Rendering.AmbientMode.Flat);
        RenderSettings.ambientSkyColor = Color.white;
    }

    
    Queue<Transform> endlessMapQue = new Queue<Transform>();//无限地图队列
    /// <summary>
    /// 拼接地图
    /// </summary>
    public void LoadMap2()
    {
        uiMng.PushPanel(PanelType.LoadingPanel);

        curMap = null;
        for (int i = 0; i < mapRoot.childCount; i++) {
            var child = mapRoot.GetChild(i);
            
            if (child != null) {
                Object.Destroy(child.gameObject);
            }
        }

        endlessMapQue.Clear();
        
        string mapChunkNameStart = "Snow Mountain2_Start";
        string mapChunkNameMiddle = "Snow Mountain2_Middle";
        string mapChunkNameEnd = "Snow Mountain2_End";
        
        var mapChunkStart = Resources.Load<GameObject>("Prefabs/Map/" + mapChunkNameStart);  
        var mapChunkMiddle = Resources.Load<GameObject>("Prefabs/Map/" + mapChunkNameMiddle);  
        // var mapChunkEnd = Resources.Load<GameObject>("Prefabs/Map/" + mapChunkNameEnd);  
        
        var start =  Object.Instantiate(mapChunkStart, mapRoot);
        // var end =  Object.Instantiate(mapChunkEnd, mapRoot);

        mapChunkPos = Vector3.zero;
        start.transform.position = mapChunkPos;
        nextChunkPos(ref mapChunkPos);
        ShowProp(start.transform);

        var mapChunkMiddle2_1 = Resources.Load<GameObject>("Prefabs/Map/Snow Mountain2_Middle1");
        for (int i = 0; i < 1; i++)
        {
            var middle =  Object.Instantiate(mapChunkMiddle2_1, mapRoot);
            middle.transform.position = mapChunkPos;
            nextChunkPos(ref mapChunkPos);
            ShowProp(middle.transform);
            endlessMapQue.Enqueue(middle.transform);
        }

        var mapChunkMiddle2_2 = Resources.Load<GameObject>("Prefabs/Map/Snow Mountain2_Middle2");
        for (int i = 0; i < 1; i++)
        {
            var middle =  Object.Instantiate(mapChunkMiddle2_2, mapRoot);
            middle.transform.position = mapChunkPos;
            nextChunkPos(ref mapChunkPos);
            ShowProp(middle.transform);
            endlessMapQue.Enqueue(middle.transform);
        }

        var mapChunkMiddle2_3 = Resources.Load<GameObject>("Prefabs/Map/Snow Mountain2_Middle3");
        for (int i = 0; i < 1; i++)
        {
            var middle =  Object.Instantiate(mapChunkMiddle2_3, mapRoot);
            middle.transform.position = mapChunkPos;
            nextChunkPos(ref mapChunkPos);
            ShowProp(middle.transform);
            endlessMapQue.Enqueue(middle.transform);
        }

        var mapChunkMiddle2_4 = Resources.Load<GameObject>("Prefabs/Map/Snow Mountain2_Middle4");
        for (int i = 0; i < 1; i++)
        {
            var middle =  Object.Instantiate(mapChunkMiddle2_4, mapRoot);
            middle.transform.position = mapChunkPos;
            nextChunkPos(ref mapChunkPos);
            ShowProp(middle.transform);
            endlessMapQue.Enqueue(middle.transform);
        }

        var mapChunkMiddle2_5 = Resources.Load<GameObject>("Prefabs/Map/Snow Mountain2_Middle5");
        for (int i = 0; i < 1; i++)
        {
            var middle =  Object.Instantiate(mapChunkMiddle2_5, mapRoot);
            middle.transform.position = mapChunkPos;
            nextChunkPos(ref mapChunkPos);
            ShowProp(middle.transform);
            endlessMapQue.Enqueue(middle.transform);
        }
        
        
        // end.transform.position = mapChunkPos;

        Player.Instance.SetMapData2(start.transform, null);
        
        SetEnvironment();
    }

    public Vector3 lastPos;
    public Vector3 mapChunkPos;
    public void nextChunkPos(ref Vector3 pos)
    {
        lastPos = pos;
        pos.z += 200;
        pos.y -= 75;
    }

    public void MoveMapChunk()
    {
        if (Mathf.Abs(Player.Instance.transform.position.y - lastPos.y) < 150)
        {
            var middle = endlessMapQue.Dequeue();
            middle.position = mapChunkPos;
            nextChunkPos(ref mapChunkPos);
            ShowProp(middle);
            endlessMapQue.Enqueue(middle);
        }
    }

    /// <summary>
    /// 显示道具
    /// </summary>
    public void ShowProp(Transform mapChunk)
    {
        var nodeRoot = mapChunk.Find("Props");
        if (nodeRoot != null)
        {
            for (int i = 0; i < nodeRoot.childCount; i++)
            {
                var child = nodeRoot.GetChild(i); 
                var indexRandom = Random.Range(1, 3);
                for (int j = 0; j < child.childCount; j++)
                {
                    child.GetChild(j).SetActive(indexRandom == j);
                }
            }
        }
    }
}
