using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBattleState : EnemyState
{

    private Transform player;
    private Enemy_Skeleton enemy;
    private int moveDirection;// 移动方向，1为向右，-1为向左

    public SkeletonBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Skeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.instance.player.transform;
    }

    public override void Update()
    {
        base.Update();

        if (enemy.IsPlayerDetected())
        {
            stateTimer = enemy.battleTime;

            if (enemy.IsPlayerDetected().distance < enemy.attackDistance)// 玩家在攻击范围内
            {
                if (CanAttack())
                {
                    stateMachine.ChangeState(enemy.attackState);
                    return;
                }
            }
        }
        else
        {
            if (stateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > 10)
            {
                stateMachine.ChangeState(enemy.idleState);
                return;
            }
        }

        if (player.position.x > enemy.transform.position.x)// 玩家在右边
            moveDirection = 1;
        else if (player.position.x < enemy.transform.position.x)// 玩家在左边
            moveDirection = -1;

        enemy.SetVelocity(enemy.moveSpeed * moveDirection, rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();
    }

    // 能否攻击
    private bool CanAttack()
    {
        if (Time.time - enemy.lastAttackTime >= enemy.attackCooldown)
            return true;

        return false;
    }

}
