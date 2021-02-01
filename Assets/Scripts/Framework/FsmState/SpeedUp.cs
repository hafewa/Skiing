using System.Collections.Generic;
using UnityEngine;

public class SpeedUp : FSMState
{
    public SpeedUp(FSMSystem fsm) : base(fsm)
    {
        stateID = StateID.SpeedUp;
    }

    public override void DoBeforeEntering()
    {
        //fsm._character.PlayAnim(clipName, false);
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