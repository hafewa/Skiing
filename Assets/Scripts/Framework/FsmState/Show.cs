using UnityEngine;

public class Show : FSMState
{
    public Show(FSMSystem fsm) : base(fsm)
    {
        stateID = StateID.Show;
        clipName = "Show";
    }

    public override void DoBeforeEntering()
    {
        fsm._character.PlayAnim(clipName, true);
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