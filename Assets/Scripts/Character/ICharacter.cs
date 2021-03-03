using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ICharacter : MonoBehaviour
{
    public int m_nRunFrm = 0;
    
    public int SpeedUpGold;
    
    public Skidmarks skidmarks;
    public ParticleSystem LandingEff;
    public ParticleSystem DieEff;
    public ParticleSystem SpeedLine;
    
    public int m_nPos = 0;  //AllPlayers的index索引
    
    public Renderer _bodyRenderer;    //身体渲染
    public Renderer _skiRenderer;    //滑板渲染
    
    public float m_PassTime = 0;

    public bool isDie = false;
    public bool isStart = false;
    public bool isGameOver = false;
    public bool isGround;

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

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag == "End")
        {
        }


        if (other.tag == "SpeedUp")
        {
            var PassStageCount = Player.Instance.stageData.m_PassStageCount;
            SpeedUpGold += (int)(Random.Range(4,12) * MathTool.GetSpeedUpGold(PassStageCount));
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
            
            if (currStateId != StateID.Landing && (currStateId == StateID.Ready || currStateId == StateID.StartJump))
            {
                fsm.PerformTransition(Transition.Landing);
            }
        }
    }

    public virtual void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Terrain")
        {
            isGround = true;
        }
    }

    public virtual void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Terrain")
        {
            isGround = false;
            
            rg.constraints = RigidbodyConstraints.FreezeRotation;
            
            
            if (currStateId != StateID.StartJump && rg.velocity.y > 0)
            {
                // Debug.LogFormat("OnCollisionExit Terrain, rg.velocity: {0}",rg.velocity);
                fsm.PerformTransition(Transition.StartJump);
            }
        }
    }

    protected virtual void FixedUpdate()
    {
        fsm.Update(this);

        if (isStart && !isDie)
        {
            m_nRunFrm++;
            m_PassTime += Time.deltaTime;
            
            if (rg != null)
            {
                curVelocity = rg.velocity;
                curSpeed = curVelocity.magnitude;
            }

            if (col != null)
            {
                if (curSpeed > maxSpeed)
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
                
                curDynamicFriction = col.material.dynamicFriction;
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

        fsm.AddStates(ready, left, forward, up, down, right, die, speedUp, jump, show, showSpeedUp, startJump, landing); //加入状态机
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
        m_nRunFrm = 0;
        
        rg.velocity = Vector3.zero;
        rg.useGravity = false;
        rg.isKinematic = true;
        col.enabled = false;

        isDie = false;
        isStart = false;
        isGround = false;

        m_PassTime = 0;

        mousePosOffset = Vector2.zero;
        
        skidmarks?.ClearSkid();
        
        SpeedUpGold = 0;
        SetLocalEulerAngles(Vector3.zero);
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

    public virtual void StartJump()
    {
        
    }

    /// <summary>
    /// 设置动摩擦系数
    /// </summary>
    /// <param name="dynamicFriction"></param>
    void SetDynamicFriction(float dynamicFriction)
    {
        col.material.dynamicFriction = dynamicFriction;
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

    public void SetLocalEulerAngles(Vector3 vector3) {
        transform.localEulerAngles = vector3;
    }
}