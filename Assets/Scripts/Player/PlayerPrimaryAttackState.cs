using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{

    private int comboCounter;// 组合计数器

    private float lastTimeAttacked;// 上次攻击时间

    private float comboWindow = 2;// 组合窗口

    public PlayerPrimaryAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        xInput = 0;// TODO we need this to fix bug on attack direction

        if (comboCounter > 2 || Time.time - lastTimeAttacked >= comboWindow)
            comboCounter = 0;

        player.anim.SetInteger("ComboCounter", comboCounter);// 设置动画组合计数器

        float attackDirection = player.facingDirection;
        if (xInput != 0)
        {
            attackDirection = xInput;
        }

        player.SetVelocity(player.attackMovement[comboCounter].x * attackDirection, player.attackMovement[comboCounter].y);// 攻击移动

        stateTime = 0.1f;//移动到攻击过渡
    }

    public override void Update()
    {
        base.Update();

        if (stateTime < 0)
            rb.velocity = Vector2.zero;

        if (triggerChangeState)
            stateMachine.ChangeState(player.idleState);
    }

    public override void Exit()
    {
        base.Exit();

        player.StartCoroutine("BusyFor", 0.12f);

        comboCounter++;
        lastTimeAttacked = Time.time;
    }

}
