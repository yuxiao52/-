using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{

    protected EnemyStateMachine stateMachine;// 状态机

    protected Enemy enemyBase;// 敌人
    protected Rigidbody2D rb;// 刚体

    private string animBoolName;// 动画状态参数名

    protected float stateTimer;// 状态持续时间
    protected bool triggerChangeState;// 是否触发状态切换

    public EnemyState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName)
    {
        this.enemyBase = _enemyBase;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        triggerChangeState = false;
        rb = enemyBase.rb;
        enemyBase.anim.SetBool(animBoolName, true);
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }

    public virtual void Exit()
    {
        enemyBase.anim.SetBool(animBoolName, false);
    }
    
    // 动画完成触发状态切换
    public virtual void AnimationFinishTrigger()
    {
        triggerChangeState = true;
    }

}
