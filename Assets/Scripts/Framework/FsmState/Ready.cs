public class Ready : FSMState
{
    public Ready(FSMSystem fsm) : base(fsm) {
        stateID = StateID.Ready;
    }

    public override void DoBeforeEntering() {
        fsm._character.PlayAnim("Ready");

        fsm._character.Ready();
    }

    public override void DoAfterLeaving() {
        fsm._character.StartReSet();
    }
    public override void Act(ICharacter cha = null) {
    }

    public override void Reason(ICharacter cha = null) {
    }
}