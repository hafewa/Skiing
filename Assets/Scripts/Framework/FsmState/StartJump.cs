using UnityEngine;

public class StartJump : FSMState
{
    public StartJump(FSMSystem fsm) : base(fsm)
    {
        stateID = StateID.StartJump;
        clipName = "StartJump";
    }

    public override void DoBeforeEntering()
    {
        fsm._character.PlayAnim("StartJump", false);
        if (Random.Range(1, 100) > 50)
        {
            fsm._character.PlayAnim("Rotate", false, AnimPlayModel.PlayQueued);
        }
        else
        {
            fsm._character.PlayAnim("Squat", false, AnimPlayModel.PlayQueued);
        }

        fsm._character.StartJump();
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