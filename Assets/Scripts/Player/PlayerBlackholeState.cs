using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlackholeState : PlayerState
{

    private float flyTime = 0.4f;// 上升时间
    private bool skillUsed;// 技能是否已经使用

    private float defaultGravity;// 默认重力
    public PlayerBlackholeState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        defaultGravity = player.rb.gravityScale;// 保存默认重力

        skillUsed = false;
        stateTime = flyTime;
        rb.gravityScale = 0;
    }

    public override void Update()
    {
        base.Update();

        if (stateTime > 0)
            rb.velocity = new Vector2(0, 15);

        if (stateTime <= 0)
        {
            rb.velocity = new Vector2(0, -0.1f);

            if (!skillUsed)
            {
                if (player.skill.blackhole.CanUseSkill())
                    skillUsed = true;
            }
        }

        if (player.skill.blackhole.SkillCompleted())
            stateMachine.ChangeState(player.fallState);
    }

    public override void Exit()
    {
        base.Exit();

        player.rb.gravityScale = defaultGravity;// 恢复重力
        player.MakeTransprent(false);// 恢复透明度
    }

}
