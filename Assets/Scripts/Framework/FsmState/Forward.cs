using UnityEngine;

public class Forward : FSMState
{
    public Forward(FSMSystem fsm) : base(fsm)
    {
        stateID = StateID.Forward;
    }

    public override void DoBeforeEntering()
    {
        fsm._character.PlayAnim("Forward", true);
        
        fsm._character.Forward();
    }

    public override void DoAfterLeaving()
    {
    }

    public override void Act(ICharacter cha = null)
    {
        
    }

    public override void Reason(ICharacter cha = null)
    {
        cha.StateChange(stateID);
    }
}