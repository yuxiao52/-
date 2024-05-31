using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{

    [Header("Attack details")]
    public Vector2[] attackMovement;// 攻击移动
    public float counterAttackDuration = 0.2f;// 反击持续时间

    public bool isBusy;// 是否正在执行任务

    [Header("Move info")]
    public float moveSpeed;// 移动速度
    public float jumpForce;// 跳跃力
    public float swordReturnImpact;// 剑返回冲击

    #region Dash 冲刺
    [Header("Dash info")]
    public float dashSpeed;// 冲刺速度
    public float dashDuration;// 冲刺持续时间
    public float dashDirection { get; private set; }// 冲刺方向
    // public float dashDistance = 1f;// 冲刺距离
    #endregion

    public SkillManager skill { get; private set; }// 技能管理器
    public GameObject sword { get; private set; }// 剑

    #region State 状态
    public PlayerStateMachine stateMachine { get; private set; }// 状态机

    public PlayerIdleState idleState { get; private set; }// 待机状态
    public PlayerMoveState moveState { get; private set; }// 移动状态
    public PlayerJumpState jumpState { get; private set; }// 跳跃状态
    public PlayerFallState fallState { get; private set; }// 下落状态
    public PlayerDashState dashState { get; private set; }// 冲刺状态
    public PlayerWallSlideState wallSlideState { get; private set; }// 墙壁滑行状态
    public PlayerWallJumpState wallJumpState { get; private set; }// 墙壁跳跃状态

    public PlayerPrimaryAttackState primaryAttack { get; private set; }// 玩家主要攻击
    public PlayerCounterAttackState counterAttack { get; private set; }// 玩家反击

    public PlayerAimSwordState aimSword { get; private set; }// 玩家瞄准剑
    public PlayerCatchSwordState catchSword { get; private set; }// 玩家抓取剑
    #endregion

    // 唤醒状态
    protected override void Awake()
    {
        base.Awake();

        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        fallState = new PlayerFallState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallSlideState = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        wallJumpState = new PlayerWallJumpState(this, stateMachine, "Jump");

        primaryAttack = new PlayerPrimaryAttackState(this, stateMachine, "Attack");
        counterAttack = new PlayerCounterAttackState(this, stateMachine, "CounterAttack");

        aimSword = new PlayerAimSwordState(this, stateMachine, "AimSword");
        catchSword = new PlayerCatchSwordState(this, stateMachine, "CatchSword");
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);

        skill = SkillManager.instance;
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();

        CheckForDashInput();
    }

    // 分配剑
    public void AssignNewSword(GameObject _newSword)
    {
        sword = _newSword;
    }

    // 接住剑
    public void CatchTheSword()
    {
        stateMachine.ChangeState(catchSword);
        Destroy(sword);
    }

    // 忙于
    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;
        yield return new WaitForSeconds(_seconds);
        isBusy = false;
    }

    // 动画完成触发状态切换
    public void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    // 检测冲刺输入
    public void CheckForDashInput()
    {
        if (IsWallDetected())
            return;

        if (Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.instance.dash.CanUseSkill())
        {
            dashDirection = Input.GetAxisRaw("Horizontal");
            if (dashDirection == 0)
            {
                dashDirection = facingDirection;
            }
            stateMachine.ChangeState(dashState);
        }
    }





}
