using UnityEngine;

public class Left : FSMState
{
    public Left(FSMSystem fsm) : base(fsm)
    {
        stateID = StateID.Left;
        clipName = "Left";
        clipName2 = "Left_Loop";
    }

    public override void DoBeforeEntering()
    {
        var len = fsm._character.PlayAnim("Left", false);
        //fsm._character.anim["Left"].normalizedTime = 0;
        //TimerSvcTool.Instance.AddTimeTask((TaskID) =>
        //{
        //    fsm._character.PlayAnim("Left_Loop", true);
        //}, len, PETimeUnit.Second);
        fsm._character.Left();
    }

    public override void DoAfterLeaving()
    {
    }

    public override void Act(ICharacter cha = null)
    {
        var normalizedTime = (Mathf.Abs(fsm._character.rg.velocity.x) - fsm._character.horizontalVelocityLimit) /
                             fsm._character.horizontalVelocityAnimMax;
        fsm._character.PlayAnimFrame(clipName, normalizedTime);
    }

    public override void Reason(ICharacter cha = null)
    {
        // cha.StateChange(stateID);
    }
}