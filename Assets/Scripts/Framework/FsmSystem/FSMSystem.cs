using System.Collections.Generic;
using UnityEngine;

public class FSMSystem
{
    public ICharacter _character;
    public FSMSystem(ICharacter character) {
        _character = character;
    }

    Dictionary<StateID, FSMState> states = new Dictionary<StateID, FSMState>();
    private FSMState currentState;//当前状态的类[这个类是继承FSMState的]【当前状态干什么在这个类里面随你写】
    private StateID currentStateID;//当前状态名称枚举
    public void Update(ICharacter cha = null) {
        if (currentState == null) {
            Debug.LogError("我是空的");
        }
        else {
            //Debuger.LogError("我当前的状态" + currentStateID);
        }
        currentState.Act(cha);
        currentState.Reason(cha);
    }

    public void AddStates(params FSMState[] states) {
        foreach (var item in states) {
            AddState(item);
        }
    }
    public void AddState(FSMState s) {
        if (s == null) {
            Debug.LogError("FSMState不能为空"); return;
        }
        if (states.ContainsKey(s.ID)) {
            Debug.LogError("状态ID已经存在了，无法重复添加"); return;
        }
        if (currentState == null) {
            currentState = s;
            currentStateID = s.ID;
            _character.currStateId = s.ID;
            currentState.lastID = StateID.NullStateID;
            currentState.DoBeforeEntering();//执行新状态里面的进入前的方法s

        }

        states.Add(s.ID, s);
    }

    public void DeleteState(StateID id) {
        if (id == StateID.NullStateID) {
            Debug.LogError("无法删除空状态"); return;
        }
        if (states.ContainsKey(id) == false) {
            Debug.LogError("无法删除不存在的状态"); return;
        }
        states.Remove(id);
    }
    public void PerformTransition(Transition trans) {
        //前3个if是校验
        if (trans == Transition.NullTransition) {
            Debug.LogError("无法执行转换条件"); return;
        }
        StateID id = currentState.GetOutputState(trans);
        if (id == StateID.NullStateID) {
            Debug.LogWarning("当前状态" + currentStateID + "无法根据转换条件" + trans + "发生转换"); return;
        }
        if (states.ContainsKey(id) == false) {
            Debug.LogError("在状态里面不存在状态" + id + ",无法进行状态转换"); return;
        }
        FSMState state = states[id];
        currentState.DoAfterLeaving();//执行本来状态里面的离开后的方法
        StateID lastTemp = currentState.ID;//记录上个状态
        currentState = state;//将新状态给他
        currentStateID = id;
        _character.currStateId = currentStateID;
        currentState.lastID = lastTemp;
        currentState.DoBeforeEntering();//执行新状态里面的进入前的方法s
    }
}
