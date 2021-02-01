public class Up : FSMState
{
    public Up(FSMSystem fsm) : base(fsm) {
        stateID = StateID.Up;
        clipName = "Up";
    }

    public override void DoBeforeEntering() {
        fsm._character.PlayAnim(clipName);
        fsm._character.Up();
    }

    public override void DoAfterLeaving() {

    }
    public override void Act(ICharacter cha = null) {
        
    }

    public override void Reason(ICharacter cha = null) {
        
    }
}