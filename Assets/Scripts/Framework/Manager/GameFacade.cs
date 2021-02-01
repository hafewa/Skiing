using UnityEngine;

public class GameFacade : UnityInstanceBase<GameFacade>
{
    [Header("功能逻辑开关")]
    public bool useGM = false;

    public bool isCanVibrate;
    
    public UIManager uiMng;
    public AudioManager audioMng;
    public MapManager mapMng;
    public DataManager dataMng;
    public SystemManager sysMng;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this.gameObject);

        OnInit();
    }

    public void OnInit()
    {
        uiMng = new UIManager();
        audioMng = new AudioManager();
        mapMng = new MapManager();
        dataMng = new DataManager();
        sysMng = new SystemManager();

        uiMng.OnInit();
        audioMng.OnInit();
        mapMng.OnInit();
        dataMng.OnInit();
        sysMng.OnInit();
    }

    private void FixedUpdate()
    {
        uiMng.OnUpdate();
        audioMng.OnUpdate();
        mapMng.OnUpdate();
        dataMng.OnUpdate();
        sysMng.OnUpdate();
    }

    private void OnDestroy() {
        OnRelease();
        StopAllCoroutines();
    }

    public void OnRelease() {
        if (uiMng != null)
            uiMng.OnRelease();
        if (audioMng != null)
            audioMng.OnRelease();
        if (mapMng != null)
            mapMng.OnRelease();
        if (dataMng != null)
            dataMng.OnRelease();
        if (sysMng != null)
            sysMng.OnRelease();
    }

    public void StartGame() {
        Time.timeScale = 1f;
    }
    public void PauseGame() {
        Time.timeScale = 0f;
    }
    public void ResumeGame() {
        Time.timeScale = 1f;
    }
    public void ExitGame() {
        Time.timeScale = 1f;
    }
}