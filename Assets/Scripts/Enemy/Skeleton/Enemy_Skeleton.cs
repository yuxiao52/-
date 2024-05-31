using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Skeleton : Enemy
{

    #region State 状态
    public SkeletonIdleState idleState { get; private set; }// 待机状态
    public SkeletonMoveState moveState { get; private set; }// 移动状态
    public SkeletonBattleState battleState { get; private set; }// 战斗状态
    public SkeletonAttackState attackState { get; private set; }// 攻击状态
    public SkeletonStunnedState stunnedState { get; private set; }// 眩晕状态
    // public SkeletonDeadState deadState { get; private set; }// 死亡状态
    #endregion

    protected override void Awake()
    {
        base.Awake();

        idleState = new SkeletonIdleState(this, stateMachine, "Idle", this);
        moveState = new SkeletonMoveState(this, stateMachine, "Move", this);
        battleState = new SkeletonBattleState(this, stateMachine, "Move", this);
        attackState = new SkeletonAttackState(this, stateMachine, "Attack", this);
        stunnedState = new SkeletonStunnedState(this, stateMachine, "Stunned", this);
        // deadState = new SkeletonDeadState(this, stateMachine, "Dead", this);

    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();

        if(Input.GetKeyDown(KeyCode.U))
        {
            stateMachine.ChangeState(stunnedState);
            return;
        }
    }

    public override bool CanBeStunned()
    {
        if(base.CanBeStunned())
        {
            stateMachine.ChangeState(stunnedState);
            return true;
        }
        return false;
    }

}