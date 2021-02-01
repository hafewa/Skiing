using UnityEngine;

public class Right : FSMState
{
    public Right(FSMSystem fsm) : base(fsm)
    {
        stateID = StateID.Right;
        clipName = "Right";
        clipName2 = "Right_Loop";
    }

    public override void DoBeforeEntering()
    {
        var len = fsm._character.PlayAnim("Right", false);
        //TimerSvcTool.Instance.AddTimeTask((TaskID) =>
        //{
        //    fsm._character.PlayAnim("Right_Loop", true);
        //}, len, PETimeUnit.Second);

        fsm._character.Right();
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
        cha.StateChange(stateID);
    }
}