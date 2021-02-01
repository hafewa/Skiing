using UnityEngine;

public class Landing : FSMState
{
    public Landing(FSMSystem fsm) : base(fsm)
    {
        stateID = StateID.Landing;
        clipName = "Landing";
    }

    public override void DoBeforeEntering()
    {
        clipLength = fsm._character.PlayAnim("Landing", false);

        fsm._character.Landing();
    }

    public override void DoAfterLeaving()
    {
    }

    public override void Act(ICharacter cha = null)
    {
    }

    public override void Reason(ICharacter cha = null)
    {
        clipLength -= Time.deltaTime;
        
        if (clipLength < 0) {
            fsm.PerformTransition(Transition.Forward);
        }
    }
}