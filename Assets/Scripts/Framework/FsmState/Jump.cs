using UnityEngine;

public class Jump : FSMState
{
    public Jump(FSMSystem fsm) : base(fsm)
    {
        stateID = StateID.Jump;
        clipName = "Jump";
    }

    public override void DoBeforeEntering()
    {
    }

    public override void DoAfterLeaving()
    {
    }

    public override void Act(ICharacter cha = null)
    {
    }

    public override void Reason(ICharacter cha = null)
    {
    }
}