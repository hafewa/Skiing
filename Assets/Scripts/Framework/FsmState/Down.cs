using UnityEngine;

public class Down : FSMState
{
    public Down(FSMSystem fsm) : base(fsm) {
        stateID = StateID.Down;
        clipName = "Down";
    }

    public override void DoBeforeEntering() {
        //fsm._character.PlayAnim("Down", false);
        fsm._character.Down();
    }

    public override void DoAfterLeaving() {

    }
    public override void Act(ICharacter cha = null) {
        
    }

    public override void Reason(ICharacter cha = null) {
    }
}