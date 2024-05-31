using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public int facingDirection { get; private set; } = 1;// 面向方向
    protected bool facingRight = true;// 面向右

    #region Component 组件
    public Animator anim { get; private set; }// 动画控制器
    public Rigidbody2D rb { get; private set; }// 刚体
    // public Transform tf { get; private set; }// transform
    // public SpriteRenderer sr { get; private set; }// 精灵渲染器
    public EntityFX fx { get; private set; }// 特效组件
    #endregion

    #region Knockback 击退
    [Header("Knockback info")]
    [SerializeField] protected Vector2 knockbackDistance;// 击退距离
    [SerializeField] protected float knockbackDuration;// 击退持续时间
    protected bool isKnocked;// 是否正在被击退
    // public float knockbackSpeed;// 击退速度
    // public float knockbackXScale;// 击退x轴缩放
    // public float knockbackYScale;// 击退y轴缩放
    #endregion

    #region Collision 碰撞
    [Header("Collision info")]
    public Transform attackCheck;// 攻击检测点
    public float attackCheckRadius;// 攻击检测半径
    [SerializeField] protected Transform groundCheck;// 地面检测点
    [SerializeField] protected float groundCheckDistance;// 地面检测距离
    [SerializeField] protected LayerMask groundLayer;// 地面层
    [SerializeField] protected Transform wallCheck;// 墙壁检测点
    [SerializeField] protected float wallCheckDistance;// 墙壁检测距离
    [SerializeField] protected LayerMask wallLayer;// 墙壁层
    #endregion

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        fx = GetComponentInChildren<EntityFX>();

    }

    protected virtual void Update()
    {

    }

    // 受伤
    public virtual void Damage()
    {
        fx.StartCoroutine("FlashFX");// 闪烁
        StartCoroutine("HitKnockback");// 被击退
    }

    // 被击退
    protected virtual IEnumerator HitKnockback()
    {
        isKnocked = true;

        rb.velocity = new Vector2(knockbackDistance.x * -facingDirection, knockbackDistance.y);// TODO playerDirection

        yield return new WaitForSeconds(knockbackDuration);
        isKnocked = false;
    }

    // 设置速度
    public virtual void SetVelocity(float _xVelocity, float _yVelocity)
    {
        if (isKnocked) return;// 正在被击退

        rb.velocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }

    // 设置速度为0
    public virtual void SetVelocityZero()
    {
        if (isKnocked) return;// 正在被击退

        rb.velocity = Vector2.zero;
    }

    // 检测地面碰撞
    public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);
    // 检测墙壁碰撞
    public virtual bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, wallCheckDistance, groundLayer);

    // 画线工具
    protected virtual void OnDrawGizmos()
    {
        //Gizmos.color = Color.red;
        // 地面检测线
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        // 墙壁检测线
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
        // 攻击检测圆
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }

    // 翻转
    public virtual void Flip()
    {
        facingDirection = facingDirection * -1;
        facingRight = !facingRight;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    // 翻转控制
    public virtual void FlipController(float _xVelocity)
    {
        if (_xVelocity > 0 && !facingRight)
        {
            Flip();
        }
        else if (_xVelocity < 0 && facingRight)
        {
            Flip();
        }
    }
}
