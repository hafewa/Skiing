using UnityEngine;

public class ShowSpeedUp : FSMState
{
    public ShowSpeedUp(FSMSystem fsm) : base(fsm)
    {
        stateID = StateID.ShowSpeedUp;
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
        //clipLength -= Time.deltaTime;
        //if (clipLength < 0) {
        //    fsm.PerformTransition(Transition.Show);
        //}
    }
}