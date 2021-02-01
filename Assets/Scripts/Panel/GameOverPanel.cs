using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class GameOverPanel : BasePanel
{
    public Transform Fail;
    
    public override void Awake() {
        base.Awake();

        UnityAction<BaseEventData> click = new UnityAction<BaseEventData>(ReStartClick);
        EventTrigger.Entry myclick = new EventTrigger.Entry();
        myclick.eventID = EventTriggerType.PointerClick;
        myclick.callback.AddListener(click);
        var evenTrigger = Fail.gameObject.AddComponent<EventTrigger>();
        evenTrigger.triggers.Add(myclick);
        
    }

    void ReStartClick(BaseEventData baseEventData) {
        Player.Instance.ReStart();
    }
}