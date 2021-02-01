using System.Collections;
using UnityEngine;

public class BasePanel : MonoBehaviour
{
    Rect lastSafeArea;

    protected GuideSystem guideSystem {
        get {
            return GameFacade.Instance.sysMng.guideSystem;
        }
    }
    // public GuideData guideData {
    //     get {
    //         return GameFacade.Instance.dataMng.guideData;
    //     }
    // }

    public PlayerData playerData {
        get {
            return GameFacade.Instance.dataMng.playerData;
        }
    }
    public StageData stageData {
        get {
            return GameFacade.Instance.dataMng.stageData;
        }
    }

    public AudioManager audioMng {
        get {
            return GameFacade.Instance.audioMng;
        }
    }

    public MapManager mapMng {

        get {
            return GameFacade.Instance.mapMng;
        }
    }

    public UIManager uiMng {

        get {
            return GameFacade.Instance.uiMng;
        }
    }

    public virtual void Awake() {
        canvasGroup = gameObject.AddComponent<CanvasGroup>();

        AdjustSaftyRect();
    }

    public bool m_bIsOpened = false;

    public CanvasGroup canvasGroup;
    public virtual void OnEnter() {
        if (canvasGroup != null) {
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1f;
        }
        m_bIsOpened = true;
        SetChildIndex();
    }
    public virtual void OnPause() {
        if (canvasGroup != null) {
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0f;
        }
        m_bIsOpened = false;
    }
    public virtual void OnResume() {
        if (canvasGroup != null) {
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1f;
        }
        m_bIsOpened = true;
    }
    public virtual void OnExit() {
        if (canvasGroup != null) {
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0f;
        }
        m_bIsOpened = false;
    }

    public virtual void OnBottom() {
        if (canvasGroup != null) {
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 1f;
        }
    }

    private void SetChildIndex() {
        int count = transform.parent.childCount;
        this.transform.SetSiblingIndex(count - 1);
    }

    /// <summary>
    /// 各种屏幕适配
    /// </summary>
    protected void AdjustSaftyRect() {
        var safeArea = /*GameFacade.Instance.safeAreaTest;//*/ Screen.safeArea;
        if (lastSafeArea != safeArea) {
            //Debug.Log("safeArea:" + safeArea);

            for (int i = 0; i < transform.childCount; i++) {
                var childTran = transform.GetChild(i).GetComponent<RectTransform>();
                //Rect UIArea = childTran.rect;
                //Debug.Log(childTran.name + "-UIArea:" + UIArea);
                var ratioX = safeArea.width / Screen.width;
                var ratioY = safeArea.height / Screen.height;
                var curOffsetMin = childTran.offsetMin;
                var curOffsetMax = childTran.offsetMax;
                childTran.offsetMin = new Vector2(curOffsetMin.x * ratioX, curOffsetMin.y * ratioY) + new Vector2(safeArea.x, safeArea.y);
                childTran.offsetMax = new Vector2(curOffsetMax.x * ratioX, curOffsetMax.y * ratioY) + new Vector2(safeArea.x + safeArea.width - Screen.width, safeArea.y + safeArea.height - Screen.height);
            }
            lastSafeArea = safeArea;
        }
    }

    /// <summary>
    /// TimeScale=0时 UI动画的播放
    /// </summary>
    /// <param name="animation"></param>
    /// <param name="clipName"></param>
    /// <param name="useTimeScale"></param>
    /// <param name="onComplete"></param>
    /// <returns></returns>
    public static IEnumerator Play(Animation animation, string clipName, bool useTimeScale = false, System.Action onComplete = null) {
        if (!useTimeScale) {
            AnimationState _currState = animation[clipName];
            bool isPlaying = true;
            float _startTime = 0F;
            float _progressTime = 0F;
            float _timeAtLastFrame = 0F;
            float _timeAtCurrentFrame = 0F;
            float deltaTime = 0F;
            animation.Play(clipName);
            _timeAtLastFrame = Time.realtimeSinceStartup;
            while (isPlaying) {
                _timeAtCurrentFrame = Time.realtimeSinceStartup;
                deltaTime = _timeAtCurrentFrame - _timeAtLastFrame;
                _timeAtLastFrame = _timeAtCurrentFrame;

                _progressTime += deltaTime;
                _currState.normalizedTime = _progressTime / _currState.length;
                animation.Sample();
                if (_progressTime >= _currState.length) {
                    if (_currState.wrapMode != WrapMode.Loop) {
                        isPlaying = false;
                    }
                    else {
                        _progressTime = 0.0f;
                    }
                }
                yield return new WaitForEndOfFrame();
            }
            yield return null;
            if (onComplete != null) {
                onComplete();
            }
        }
        else {
            animation.Play(clipName);
        }
    }
}
