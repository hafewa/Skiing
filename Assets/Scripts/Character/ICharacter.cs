using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class ICharacter : MonoBehaviour
{
    public ParticleSystem hudun; //道具护盾特效
    public ParticleSystem SpeedPropEffect; //道具加速特效

    public float heightForJump = 0.5f; //人物离地一定距离进入起跳状态

    public float m_wingHeight = 25;
    public float m_wingUpForce = 10;

    public float m_wingTime;
    public float m_wingTimeLast;

    public float m_shieldTime;
    public float m_shieldHoldTime; //护盾触发后的持续时间
    public float m_shieldTimeLast;
    public bool m_isShieldUsed = true;

    public Transform PropStateShow;

    public float m_speedUpForce = 100; //加速道具加速的的力
    public float m_speedUpTime;
    public float m_speedUpTimeLast;

    public int m_nRunFrm = 0;

    public int SpeedUpGold;

    public Skidmarks skidmarks;
    public ParticleSystem LandingEff;
    public ParticleSystem DieEff;
    public ParticleSystem SpeedLine;

    public int m_nPos = 0; //AllPlayers的index索引

    public Renderer _bodyRenderer; //身体渲染
    public Renderer _skiRenderer; //滑板渲染

    public float m_PassTime = 0;

    public bool isDie = false;
    public bool isStart = false;
    public bool isGameOver = false;
    public bool isGround;
    public bool firstLand; //第一次落地标记

    public float firstSpeedUpForce = 300;

    public StateID currStateId = StateID.Ready;

    public Rigidbody rg;
    public Collider col;
    public Animation anim;

    public FSMSystem fsm;


    public float speedUpForce = 50f;
    public int jumpForce = 60; //起跳力的大小
    public float horizontalVelocityMax = 5f; //横向最大速度
    [Range(1, 150)] public float horizontalForceSensitivity = 1; //左有滑动灵敏度
    public float stopForceStrength = 50;

    public bool leftAndRightBtnIsDown = false; //左右滑动是否按下
    public float curHorizontalDirection;
    public float curHorizontalForce;
    public float curJumpForce;
    public float curStopForce;

    public float horizontalVelocityLimit = 1.5f; //判断为左右滑动状态的最小速率
    public float horizontalVelocityAnimMax = 15f; //左右动画帧最后一针速率值

    public float forwardDynamicFriction = 0.0312f; //前进动摩擦系数
    public float horizontalDynamicFriction = 0.1f; //左右动摩擦系数
    public float curDynamicFriction;
    public Vector3 curVelocity;
    public float curSpeed;
    public float maxSpeed = 50;
    public float maxSpeedUpWithTime;

    public Vector2 mousePosOffset = Vector2.zero; //鼠标拖拽位移

    public AudioManager audioMng
    {
        get { return GameFacade.Instance.audioMng; }
    }

    public UIManager uiMng
    {
        get { return GameFacade.Instance.uiMng; }
    }

    public MapManager mapMng
    {
        get { return GameFacade.Instance.mapMng; }
    }

    public PlayerData playerData
    {
        get { return GameFacade.Instance.dataMng.playerData; }
    }

    public StageData stageData
    {
        get { return GameFacade.Instance.dataMng.stageData; }
    }

    public virtual void OnTriggerExit(Collider other)
    {
        if (other.tag == "Obstacle")
        {
            var ani = other.transform.parent.GetComponent<Animation>();
            if (ani != null)
            {
                var collider = other;
                var animStr = "Take 001";
                AnimationState state = ani[animStr];
                state.enabled = true;
                ani.Play(animStr);
                TimerSvcTool.Instance.AddTimeTask((tid) =>
                {
                    // Debug.Log("qqqqqqqqqqqqqqqqq");
                    if (ani != null)
                    {
                        ani.Play(animStr);
                        state.time = 0;
                        ani.Sample();
                        state.enabled = false;
                    }

                    if (collider != null)
                    {
                        collider.isTrigger = false;
                    }
                }, 3, PETimeUnit.Second);
            }

            if (!m_isShieldUsed && m_shieldTimeLast > 0)
            {
                m_isShieldUsed = true;
                m_shieldTimeLast = m_shieldHoldTime;
            }
        }
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag == "End")
        {
        }


        if (other.tag == "SpeedUp")
        {
            var PassStageCount = Player.Instance.stageData.m_PassStageCount;
            SpeedUpGold += (int) (Random.Range(4, 12) * MathTool.GetSpeedUpGold(PassStageCount));
        }

        if (other.tag == "Obstacle")
        {
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.tag == "SpeedUp")
        {
            rg.AddForce(0, 0, speedUpForce);
            // Debug.Log("SpeedUp");
        }
    }

    public virtual void OnCollisionEnter(Collision collision)
    {
        //print("OnCollisionEnter:" + collision.gameObject.name + " tag:" + collision.gameObject.tag);
        if (collision.gameObject.tag == "Player")
        {
            return;
        }

        if (collision.gameObject.tag == "Obstacle" && !isDie && !isGameOver)
        {
            fsm.PerformTransition(Transition.Die);
        }

        if (collision.gameObject.tag == "Terrain")
        {
            isGround = true;

            rg.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        }
    }

    public bool isbottomUp; //人物是否颠倒
    public float bottomUpTime; //人物颠倒时间

    public virtual void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Terrain")
        {
            isGround = true;

            //开局落地加速
            if (m_nRunFrm > 25 && firstLand)
            {
                firstLand = false;
                rg.AddForce(transform.forward * firstSpeedUpForce);
            }

            if (currStateId == StateID.Ready || currStateId == StateID.StartJump)
            {
                fsm.PerformTransition(Transition.Landing);
            }

            //人物是否颠倒
            var axisXangle = transform.eulerAngles.x;
            isbottomUp = (axisXangle > 50 || axisXangle < -50);

            //速度降低后的恢复
            // if (m_nRunFrm > 250 && curSpeed < 20)
            // {
            //     curfirstSpeedUpForce += Time.deltaTime;
            //     rg.AddForce(Vector3.forward *
            //                 (curfirstSpeedUpForce > firstSpeedUpForce ? firstSpeedUpForce : curfirstSpeedUpForce));
            // }
        }
    }

    public virtual void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Terrain")
        {
            isGround = false;

            rg.constraints = RigidbodyConstraints.FreezeRotation;

            isbottomUp = false;
        }
    }

    protected virtual void FixedUpdate()
    {
        // rg.centerOfMass = transform.position;

        var dt = Time.deltaTime;
        fsm.Update(this);

        if (isStart && !isDie)
        {
            m_nRunFrm++;
            m_PassTime += Time.deltaTime;

            if (isbottomUp)
            {
                bottomUpTime += dt;
                if (bottomUpTime > 1f)
                {
                    fsm.PerformTransition(Transition.Die);
                }
            }
            else
            {
                bottomUpTime = 0;
            }

            if (rg != null)
            {
                curVelocity = rg.velocity;
                curSpeed = curVelocity.magnitude;
            }

            maxSpeedUpWithTime = maxSpeed * Mathf.Pow(1.13f, m_PassTime / 60);
            if (col != null)
            {
                if (curSpeed > maxSpeedUpWithTime && isGround)
                {
                    SetDynamicFriction(curDynamicFriction + Time.deltaTime);
                }
                else
                {
                    switch (currStateId)
                    {
                        case StateID.Forward:
                            SetDynamicFriction(forwardDynamicFriction);
                            break;
                        case StateID.Left:
                            SetDynamicFriction(horizontalDynamicFriction);
                            break;
                        case StateID.Right:
                            SetDynamicFriction(horizontalDynamicFriction);
                            break;
                    }
                }
            }

            //左右速度超过最大值时,停止施加力
            if ((rg.velocity.x < -horizontalVelocityMax && curHorizontalForce < 0) ||
                (rg.velocity.x > horizontalVelocityMax && curHorizontalForce > 0))
            {
                curHorizontalForce = 0;
            }

            rg.AddForce(curHorizontalForce, curJumpForce, 0);

            curJumpForce = 0;

            //过终点后减速
            if (isGameOver)
            {
                curStopForce += Time.deltaTime * stopForceStrength;
                if (rg.velocity.z > 0)
                {
                    rg.AddForce(0, 0, -curStopForce);
                }
            }
            else
            {
                curStopForce = 0;
            }

            //加速状态
            if (m_speedUpTimeLast > 0)
            {
                m_speedUpTimeLast -= Time.deltaTime;

                if (curSpeed < maxSpeedUpWithTime * 1.5f)
                {
                    rg.AddForce(Vector3.forward * m_speedUpForce);
                }

                SetCannotDie();
            }

            //格挡一次状态
            if (m_shieldTimeLast > 0 && !m_isShieldUsed)
            {
                m_shieldTimeLast -= dt;

                SetCannotDie();
            }

            if (m_shieldTimeLast < 0 && m_speedUpTimeLast < 0)
            {
                SetCanDie();
            }

            //飞行状态
            if (m_wingTimeLast > 0)
            {
                m_wingTimeLast -= dt;
                RaycastHit hit;
                if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hit, 100))
                {
                    if (Vector3.Distance(transform.position, hit.point) < m_wingHeight)
                    {
                        rg.AddForce(Vector3.up * m_wingUpForce);
                    }
                }
            }

            if (!isGround)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, Vector3.down, out hit, 100))
                {
                    //人物在空中固定x轴的角度范围
                    if (Vector3.Distance(transform.position, hit.point) > 2)
                    {
                        var curXAngle = Mathf.Clamp(transform.eulerAngles.x, -20f, 0f);
                        var axisXAngle = Mathf.Lerp(transform.eulerAngles.x, Vector3.Angle(Vector3.up, hit.normal), dt);
                        transform.rotation = Quaternion.Euler(axisXAngle, 0, 0);
                        // print("qqqqqqqqqqqqqq:" + hit.normal);
                        // print("Angle:" + Vector3.Angle(Vector3.up, hit.normal));
                    }

                    //人物离地一定距离进入起跳状态
                    if (currStateId != StateID.StartJump &&
                        Vector3.Distance(transform.position, hit.point) > heightForJump)
                    {
                        fsm.PerformTransition(Transition.StartJump);
                    }
                }
            }
        }
    }


    protected virtual void Awake()
    {
        rg = GetComponent<Rigidbody>();
        anim = GetComponent<Animation>();
        col = GetComponent<Collider>();
    }

    protected virtual void Start()
    {
        InitFSM();
    }

    void InitFSM()
    {
        fsm = new FSMSystem(this);

        FSMState ready = new Ready(fsm);
        AddAllTransition(ready);
        FSMState forward = new Forward(fsm);
        AddAllTransition(forward);
        FSMState up = new Up(fsm);
        AddAllTransition(up);
        FSMState left = new Left(fsm);
        AddAllTransition(left);
        FSMState down = new Down(fsm);
        AddAllTransition(down);
        FSMState right = new Right(fsm);
        AddAllTransition(right);
        FSMState die = new Die(fsm);
        AddAllTransition(die);
        FSMState speedUp = new SpeedUp(fsm);
        AddAllTransition(speedUp);
        FSMState jump = new Jump(fsm);
        AddAllTransition(jump);
        FSMState show = new Show(fsm);
        AddAllTransition(show);
        FSMState showSpeedUp = new ShowSpeedUp(fsm);
        AddAllTransition(showSpeedUp);
        FSMState startJump = new StartJump(fsm);
        AddAllTransition(startJump);
        FSMState landing = new Landing(fsm);
        AddAllTransition(landing);

        fsm.AddStates(ready, left, forward, up, down, right, die, speedUp, jump, show, showSpeedUp, startJump,
            landing); //加入状态机
    }

    void AddAllTransition(FSMState fSMState)
    {
        //除了自己给自己连线
        if (fSMState.ID != StateID.Ready)
            fSMState.AddTransition(Transition.Ready, StateID.Ready); //添加当前状态可以转换到另外状态的条件
        if (fSMState.ID != StateID.Forward)
            fSMState.AddTransition(Transition.Forward, StateID.Forward);
        if (fSMState.ID != StateID.Up)
            fSMState.AddTransition(Transition.Up, StateID.Up);
        if (fSMState.ID != StateID.Left)
            fSMState.AddTransition(Transition.Left, StateID.Left);
        if (fSMState.ID != StateID.Down)
            fSMState.AddTransition(Transition.Down, StateID.Down);
        if (fSMState.ID != StateID.Right)
            fSMState.AddTransition(Transition.Right, StateID.Right);
        if (fSMState.ID != StateID.Die)
            fSMState.AddTransition(Transition.Die, StateID.Die);
        if (fSMState.ID != StateID.SpeedUp)
            fSMState.AddTransition(Transition.SpeedUp, StateID.SpeedUp);
        if (fSMState.ID != StateID.Jump)
            fSMState.AddTransition(Transition.Jump, StateID.Jump);
        if (fSMState.ID != StateID.Show)
            fSMState.AddTransition(Transition.Show, StateID.Show);
        if (fSMState.ID != StateID.ShowSpeedUp)
            fSMState.AddTransition(Transition.ShowSpeedUp, StateID.ShowSpeedUp);
        if (fSMState.ID != StateID.StartJump)
            fSMState.AddTransition(Transition.StartJump, StateID.StartJump);
        if (fSMState.ID != StateID.Landing)
            fSMState.AddTransition(Transition.Landing, StateID.Landing);
    }

    public virtual void ReStart()
    {
        if (currStateId != StateID.Ready)
        {
            fsm.PerformTransition(Transition.Ready);
        }
    }

    public void StartReSet()
    {
        rg.useGravity = true;
        rg.isKinematic = false;
        col.enabled = true;
        isStart = true;
    }

    public virtual void Ready()
    {
        m_wingTimeLast = 0;
        m_speedUpTimeLast = 0;
        m_shieldTimeLast = 0;

        isbottomUp = false;
        bottomUpTime = 0;

        m_nRunFrm = 0;

        rg.velocity = Vector3.zero;
        rg.useGravity = false;
        rg.isKinematic = true;
        col.enabled = false;

        isDie = false;
        isStart = false;
        isGround = false;
        firstLand = true;

        m_PassTime = 0;

        mousePosOffset = Vector2.zero;

        skidmarks?.ClearSkid();

        SpeedUpGold = 0;
        SetLocalEulerAngles(Vector3.zero);
        SetDynamicFriction(forwardDynamicFriction);
    }

    public virtual void Die()
    {
        isDie = true;
        rg.isKinematic = true;
        DieEff.Play();
    }

    public virtual void Landing()
    {
        LandingEff.Play();
    }

    public virtual void Up()
    {
    }

    public virtual void Down()
    {
    }

    public virtual void Left()
    {
        SetDynamicFriction(horizontalDynamicFriction);
    }

    public virtual void Right()
    {
        SetDynamicFriction(horizontalDynamicFriction);
    }

    public virtual void Forward()
    {
        SetDynamicFriction(forwardDynamicFriction);
    }

    public virtual void Jump()
    {
    }

    public Vector3 curAxis;
    public float curDir = 1; //相机环绕顺逆时针切换
    public float camRotateSpeed = 10; //相机环绕角速度
    public float curCamRotateAngle = 0; //相机当前环绕角度
    public float maxCamRotateAngle = 60; //最大环绕角度

    public virtual void StartJump()
    {
        curCamRotateAngle = 0;
        curDir = (Random.Range(0, 2) == 0 ? 1 : -1);
        curAxis = (Random.Range(0, 2) == 0 ? Vector3.left : Vector3.up);
    }

    /// <summary>
    /// 设置动摩擦系数
    /// </summary>
    /// <param name="dynamicFriction"></param>
    void SetDynamicFriction(float dynamicFriction)
    {
        col.material.dynamicFriction = dynamicFriction;

        curDynamicFriction = col.material.dynamicFriction;
    }

    /// <summary>
    /// 状态切换
    /// </summary>
    /// <param name="curStateID"></param>
    public void StateChange(StateID curStateID)
    {
        // Debug.Log(rg.velocity);
        var velocity = rg.velocity;
        if (Mathf.Abs(velocity.x) < horizontalVelocityLimit)
        {
            if (curStateID != StateID.Forward)
            {
                fsm.PerformTransition(Transition.Forward);
            }
        }
        else
        {
            if (velocity.x <= -horizontalVelocityLimit)
            {
                if (curStateID != StateID.Left)
                {
                    fsm.PerformTransition(Transition.Left);
                }
            }
            else if (velocity.x > horizontalVelocityLimit)
            {
                if (curStateID != StateID.Right)
                {
                    fsm.PerformTransition(Transition.Right);
                }
            }
        }
    }

    public float PlayAnim(string animStr, bool isloop = true, AnimPlayModel playModel = AnimPlayModel.Play)
    {
        var clip = anim.GetClip(animStr);
        if (clip)
        {
            switch (playModel)
            {
                case AnimPlayModel.Play:
                    anim.Play(animStr);
                    break;
                case AnimPlayModel.PlayQueued:
                    anim.PlayQueued(animStr);
                    break;
            }

            if (isloop)
            {
                anim.wrapMode = WrapMode.Loop;
            }
            else
            {
                anim.wrapMode = WrapMode.Once;
            }

            return clip.length;
        }
        else
        {
            Debug.LogErrorFormat("人物:'{0}',没有动画'{1}'", name, animStr);
        }

        return 0;
    }

    // 播放某一帧动画
    public void PlayAnimFrame(string animStr, float normalizedTime = 0f)
    {
        var clip = anim.GetClip(animStr);
        if (clip)
        {
            anim.Play(animStr);
            anim[animStr].normalizedTime = normalizedTime;
            //print("animStr:" + animStr);
            //print("normalizedTime:" + normalizedTime);
        }
        else
        {
            Debug.LogWarningFormat("人物:'{0}',没有动画'{1}'", name, animStr);
        }
    }

    public void SetPlayerSkin(int skinIndex)
    {
        _bodyRenderer.material.SetTexture("_MainTex",
            Resources.Load<Texture>("Textures/Body/" + String.Format("HX_{0:D3}_body_col", skinIndex)));

        _skiRenderer.material.SetTexture("_MainTex",
            Resources.Load<Texture>("Textures/Skis/" + String.Format("ban-{0:D3}-uv", skinIndex)));
    }

    public void SetLocalEulerAngles(Vector3 vector3)
    {
        transform.localEulerAngles = vector3;
    }

    public void LeavePropState()
    {
        if (PropStateShow != null)
        {
            for (int i = 0; i < PropStateShow.childCount; i++)
            {
                var child = PropStateShow.GetChild(i);
                child.SetActive(false);
            }
        }
    }

    public void EnterPropState(string propName, float propStateTime)
    {
        switch (propName)
        {
            case "SpeedUp":
                m_speedUpTimeLast = propStateTime;


                SpeedLine.Play();
                SpeedPropEffect.Play();
                
                TimerSvcTool.Instance.AddTimeTask((tid) =>
                {
                    SpeedLine.Stop();
                    SpeedPropEffect.Stop();

                    SpeedLine.Clear();
                    SpeedPropEffect.Clear();
                }, propStateTime, PETimeUnit.Second);

                audioMng.PlayAudioEffect(MusicType.SpeedUp);
                audioMng.PlayAudioEffect(MusicType.Wind);

                break;
            case "Shield":
                m_shieldTimeLast = propStateTime;
                m_isShieldUsed = false;

                hudun.Play();
                TimerSvcTool.Instance.AddTimeTask((tid) =>
                {
                    hudun.Stop();
                    hudun.Clear();
                }, propStateTime, PETimeUnit.Second);
                break;
            case "Wing":
                m_wingTimeLast = propStateTime;
                break;
            default:

                break;
        }

        if (PropStateShow != null)
        {
            for (int i = 0; i < PropStateShow.childCount; i++)
            {
                var child = PropStateShow.GetChild(i);
                child.SetActive(child.name == propName);
                if (child.name == propName)
                {
                    child.DOPunchScale(Vector3.one * 0.2f, propStateTime, 5);
                }
            }
        }
    }

    void SetCannotDie()
    {
        Collider[] colliders =
            Physics.OverlapSphere(transform.position, 10);

        if (colliders.Length != 0)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                // print("tag:"+colliders[i].tag);
                if (colliders[i].tag == "Obstacle")
                {
                    colliders[i].isTrigger = true;
                }
            }
        }
    }

    void SetCanDie()
    {
        Collider[] colliders =
            Physics.OverlapSphere(transform.position, 10);

        if (colliders.Length != 0)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                var ani = colliders[i].GetComponent<Animation>();
                // print("tag:"+colliders[i].tag);
                if (colliders[i].tag == "Obstacle" && (ani == null || !ani.isPlaying))
                {
                    colliders[i].isTrigger = false;
                }
            }
        }
    }
}