public class Die : FSMState
{
    public Die(FSMSystem fsm) : base(fsm) {
        stateID = StateID.Die;
    }

    public override void DoBeforeEntering() {
        fsm._character.PlayAnim("Die", false);

        fsm._character.Die();
    }

    public override void DoAfterLeaving() {

    }
    public override void Act(ICharacter cha = null) {
    }

    public override void Reason(ICharacter cha = null) {
    }
}