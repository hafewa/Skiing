using System.Collections.Generic;
using UnityEngine;

public class UIManager : IManager
{
    public override void OnInit() { }
    public override void OnUpdate() { }
    public override void OnRelease() { }
    
    public Dictionary<PanelType, BasePanel> _dictBasePanel;
    public Stack<BasePanel> _stackBasePanel;

    const string panelPath = "Prefabs/Panel/";

    Transform _canvasTransform;
    private BasePanel HelpGetBasePanel(PanelType _panelType) {
        if (_dictBasePanel == null) _dictBasePanel = new Dictionary<PanelType, BasePanel>();
        BasePanel value;
        _dictBasePanel.TryGetValue(_panelType, out value);
        if (value == null) {
            GameObject go = GameObject.Instantiate(Resources.Load<GameObject>(panelPath + _panelType));
            _canvasTransform = GameObject.Find("Canvas").transform;
            go.transform.SetParent(_canvasTransform, false);
            _dictBasePanel.Add(_panelType, go.GetComponent<BasePanel>());
        }
        return _dictBasePanel[_panelType];
    }

    public BasePanel PushPanel(PanelType panelType, bool IsSpecial = false) {
        if (IsSpecial == true) {
            GameObject go = GameObject.Instantiate(Resources.Load<GameObject>(panelPath + panelType));
            _canvasTransform = GameObject.Find("Canvas").transform;
            go.transform.SetParent(_canvasTransform, false);
            BasePanel basePanel = go.GetComponent<BasePanel>();
            return basePanel;
        }

        BasePanel basepanel = HelpGetBasePanel(panelType);
        if (_stackBasePanel == null) {
            _stackBasePanel = new Stack<BasePanel>();
        }
        if (_stackBasePanel.Count > 0) {

            if (_stackBasePanel.Peek() != basepanel) {
                _stackBasePanel.Peek().OnPause();
                basepanel.OnEnter();
                _stackBasePanel.Push(basepanel);
            }
        }
        else {
            basepanel.OnEnter();
            _stackBasePanel.Push(basepanel);
        }

        return basepanel;
    }
    public void PopPanel() {
        if (_stackBasePanel.Count > 1) {
            _stackBasePanel.Pop().OnExit();
            _stackBasePanel.Peek().OnResume();
        }
    }
    public T GetPanel<T>(PanelType _panelType) where T : BasePanel {
        Transform go = GameObject.Find("Canvas").transform.Find(_panelType + "(Clone)");
        if (go != null) {
            return go.GetComponent<BasePanel>() as T;
        }
        else {
            return null;
        }
    }
}
