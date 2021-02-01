using System.Collections.Generic;
using UnityEngine;

public abstract class FSMState
{
    protected string clipName;
    protected string clipName2;
    protected float clipLength;

    Dictionary<Transition, StateID> map = new Dictionary<Transition, StateID>();
    public StateID lastID;//上一个状态

    protected StateID stateID;
    public StateID ID { get { return stateID; } }
    protected FSMSystem fsm;
    public FSMState(FSMSystem fsm) {
        this.fsm = fsm;
    }

    public void AddTransition(Transition trans, StateID id) {
        if (trans == Transition.NullTransition) {
            Debug.LogError("不允许NullTransition"); return;
        }
        if (id == StateID.NullStateID) {
            Debug.LogError("不允许NullStateID"); return;
        }
        if (map.ContainsKey(trans)) {
            Debug.LogError("添加转换条件和对应的转换状态已经存在");
        }
        map.Add(trans, id);
    }
    public void RemoveTransition(Transition trans) {
        if (trans == Transition.NullTransition) {
            Debug.LogError("不允许NullTransition"); return;
        }
        if (map.ContainsKey(trans) == false) {
            Debug.LogError("删除转换条件根本不在于字典中");
        }
        map.Remove(trans);
    }
    public StateID GetOutputState(Transition trans) {
        if (map.ContainsKey(trans)) {
            return map[trans];
        }
        return StateID.NullStateID;
    }
    public virtual void DoBeforeEntering() {

    }
    public virtual void DoAfterLeaving() {

    }
    public abstract void Act(ICharacter cha = null);
    public abstract void Reason(ICharacter cha = null);

}
public enum Transition
{
    NullTransition = 0,
    Ready,  // 准备
    Forward, // 向前
    Up,     //向上
    Left,   // 向左
    Right,  // 向右
    Down,   // 向下
    Die,    //死亡
    SpeedUp,    //加速
    Jump,       //跳
    Show,       //展示
    ShowSpeedUp,    //展示加速动作
    StartJump,    //开始起跳
    Landing,    //落地
}
public enum StateID
{
    NullStateID,
    Ready,  // 准备
    Forward, // 向前
    Up,     //向上
    Left,   // 向左
    Right,  // 向右
    Down,   // 向下
    Die,    //死亡
    SpeedUp,    //加速
    Jump,       //跳
    Show,       //展示
    ShowSpeedUp,    //展示加速动作
    StartJump,    //开始起跳
    Landing,    //落地
}
