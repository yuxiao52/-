using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{
    public PlayerCounterAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTime = player.counterAttackDuration;
        player.anim.SetBool("SuccessfulCounterAttack", false);

    }

    public override void Update()
    {
        base.Update();

        player.SetVelocityZero();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                if (hit.GetComponent<Enemy>().CanBeStunned())
                {
                    stateTime = 10;// * any value bigger than 1
                    player.anim.SetBool("SuccessfulCounterAttack", true);

                }
            }
        }

        if (stateTime <= 0 || triggerChangeState)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
