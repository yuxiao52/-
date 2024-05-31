using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{

    [SerializeField] protected Transform playerCheck;// 玩家检测点
    [SerializeField] protected float playerCheckDistance;// 玩家检测距离
    [SerializeField] protected LayerMask playerLayer;// 玩家层

    #region Stunned 眩晕
    [Header("Stunned info")]
    public float stunDuration;// 眩晕持续时间
    public Vector2 stunDistance;// 眩晕距离
    protected bool canBeStunned;// ? 是否可以被眩晕
    [SerializeField] protected GameObject counterImage;// ? 计数器图片
    #endregion

    [Header("Move info")]
    public float moveSpeed;// 移动速度
    // public float moveDistance;// 移动距离
    public float idleTime;// 待机时间
    public float defaulMoveSpeed;// 默认移动速度

    [Header("Attack info")]
    public float attackDistance;// 攻击距离
    public float attackCooldown;// 攻击冷却时间
    [HideInInspector] public float lastAttackTime;// 上一次攻击时间
    public float battleTime;// 战斗时间


    public EnemyStateMachine stateMachine;// 状态机

    protected override void Awake()
    {
        base.Awake();

        stateMachine = new EnemyStateMachine();

        defaulMoveSpeed = moveSpeed;
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();
    }

    // 冻结时间
    public virtual void FreezeTime(bool _timeFrozen)
    {
        if (_timeFrozen)
        {
            moveSpeed = 0;
            anim.speed = 0;
        }
        else
        {
            moveSpeed = defaulMoveSpeed;
            anim.speed = 1;
        }
    }

    // 冻结计时器
    protected virtual IEnumerator FreezeTimerFor(float _seconds)
    {
        FreezeTime(true);
        yield return new WaitForSeconds(_seconds);
        FreezeTime(false);
    }

    // 打开反击窗口
    public virtual void OpenCounterAttackWindow()
    {
        canBeStunned = true;
        counterImage.SetActive(true);
    }

    // 关闭反击窗口
    public virtual void CloseCounterAttackWindow()
    {
        canBeStunned = false;
        counterImage.SetActive(false);
    }

    // 是否可以被眩晕
    public virtual bool CanBeStunned()
    {
        if (canBeStunned)
        {
            CloseCounterAttackWindow();
            return true;
        }
        else
        {
            return false;
        }
    }

    // 动画完成触发状态切换
    public virtual void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    // 玩家检测
    public virtual RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(playerCheck.position, Vector2.right * facingDirection, playerCheckDistance, playerLayer);

    // 画线工具
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        // 玩家检测线
        Gizmos.DrawLine(playerCheck.position, new Vector3(playerCheck.position.x + playerCheckDistance * facingDirection, playerCheck.position.y));
        Gizmos.color = Color.red;
        // 攻击检测线
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDistance * facingDirection, transform.position.y));
    }

}
