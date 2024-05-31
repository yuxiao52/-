using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected PlayerStateMachine stateMachine;// 状态机
    protected Player player;// 玩家

    protected Rigidbody2D rb;// 玩家的刚体组件

    protected float xInput; // 玩家输入的x轴移动量
    protected float yInput; // 玩家输入的y轴移动量

    private string animBoolName;// 动画状态参数名

    protected float stateTime;// 状态持续时间
    protected bool triggerChangeState; // 是否触发状态切换

    public PlayerState(Player _player, PlayerStateMachine _stateMachine, String _animBoolName)
    {
        this.player = _player;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        player.anim.SetBool(animBoolName, true);// 设置动画状态为true
        stateTime = 0;
        rb = player.rb;
        triggerChangeState = false;
    }

    public virtual void Update()
    {
        stateTime -= Time.deltaTime;


        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        player.anim.SetFloat("yVelocity", rb.velocity.y);
    }

    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName, false);// 设置动画状态为false
    }

    // 动画完成触发状态切换
    public virtual void AnimationFinishTrigger()
    {
        triggerChangeState = true;
    }
}
