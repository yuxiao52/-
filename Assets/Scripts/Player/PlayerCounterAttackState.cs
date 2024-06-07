using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{

    private bool canCreateClone;// 是否可以创建分身

    public PlayerCounterAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTime = player.counterAttackDuration;
        player.anim.SetBool("SuccessfulCounterAttack", false);

        canCreateClone = true;
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
                // 敌人处于可被眩晕状态时，才能被反击
                if (hit.GetComponent<Enemy>().CanBeStunned())
                {
                    stateTime = 10;// * any value bigger than 1
                    player.anim.SetBool("SuccessfulCounterAttack", true);

                    if (canCreateClone)
                    {
                        player.skill.clone.CreateCloneOnCounterAttack(hit.transform);// 反击时创建分身
                        canCreateClone = false;
                    }
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
