using UnityEngine;

public class Forward : FSMState
{
    public Forward(FSMSystem fsm) : base(fsm)
    {
        stateID = StateID.Forward;
        clipName = "Forward";
    }

    public override void DoBeforeEntering()
    {
        fsm._character.PlayAnim("Forward", true);

        fsm._character.Forward();
    }

    public override void DoAfterLeaving()
    {
    }

    private float lastNewTime; //上一帧的动画帧时间

    public override void Act(ICharacter cha = null)
    {
        // clipName = (Player.Instance.curHorizontalDirection > 0 ? "Right" : "Left");
        float normalizedTime = Mathf.Abs(Player.Instance.curHorizontalDirection);
        float newTime = Mathf.Lerp(fsm._character.anim[clipName].normalizedTime, normalizedTime, 0.2f);

        if (Player.Instance.curHorizontalDirection > 0)
        {
            if (Player.Instance.curVelocity.x > 0)
            {
                clipName = "Right";
                normalizedTime = Mathf.Abs(Player.Instance.curHorizontalDirection);
                if (Mathf.Abs(lastNewTime - normalizedTime) > 0.1)
                {
                    normalizedTime = Mathf.Lerp(lastNewTime, normalizedTime, 0.02f);
                    lastNewTime = normalizedTime;
                }
                else
                {
                    lastNewTime = normalizedTime;
                }
            }
            else
            {
                clipName = "RightBig";
                normalizedTime = Mathf.Abs(Player.Instance.curVelocity.x / Player.Instance.horizontalVelocityMax);
                lastNewTime = normalizedTime;
            }

            fsm._character.PlayAnimFrame(clipName, normalizedTime);
        }
        else if (Player.Instance.curHorizontalDirection < 0)
        {
            if (Player.Instance.curVelocity.x < 0)
            {
                clipName = "Left";
                normalizedTime = Mathf.Abs(Player.Instance.curHorizontalDirection);
                if (Mathf.Abs(lastNewTime - normalizedTime) > 0.1)
                {
                    normalizedTime = Mathf.Lerp(lastNewTime, normalizedTime, 0.02f);
                    lastNewTime = normalizedTime;
                }
                else
                {
                    lastNewTime = normalizedTime;
                }
            }
            else
            {
                clipName = "LeftBig";
                normalizedTime = Mathf.Abs(Player.Instance.curVelocity.x / Player.Instance.horizontalVelocityMax);
                lastNewTime = normalizedTime;
            }

            fsm._character.PlayAnimFrame(clipName, normalizedTime);
        }
        else
        {
            lastNewTime = 0;
            fsm._character.PlayAnim("Forward", true);
        }
    }

    public override void Reason(ICharacter cha = null)
    {
        // cha.StateChange(stateID);
    }
}