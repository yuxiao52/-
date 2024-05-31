using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Sword_Skill_Controller : MonoBehaviour
{

    private Animator anim;// 剑动画
    private Rigidbody2D rb;// 剑刚体
    private CircleCollider2D cd;// 剑碰撞器
    private Player player;// 玩家引用

    private bool canRotate = true;// 是否可以旋转
    private bool isReturning = false;// 是否返回
    private float returnSpeed = 12f;// 返回速度


    private float freezeTimeDuration;// 冻结时间持续时间


    [Header("Bounce info")]
    private bool isBouncing;// 是否反弹
    private int bouncesAmount;// 反弹次数
    private float bounceSpeed = 20f;// 反弹速度
    private List<Transform> enemyTargets;// 敌人目标
    private int targetIndex;// 目标索引

    [Header("Pierce info")]
    private int pierceAmount;// 穿透次数

    [Header("Spin info")]
    private float maxTravelDistance;// 最大移动距离
    private float spinDuration;// 旋转持续时间
    private float spinTimer;// 旋转计时器
    private bool wasStopped;// 是否停止移动
    private bool isSpinning;// 是否旋转

    private float hitTimer;// 攻击计时器
    private float hitCooldown;// 攻击冷却时间

    private float spinDirection;// 旋转移动方向

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();// 获取动画组件
        rb = GetComponent<Rigidbody2D>();// 获取刚体组件
        cd = GetComponent<CircleCollider2D>();// 获取碰撞器组件
    }

    private void Update()
    {
        if (canRotate)
            transform.right = rb.velocity;// 剑的朝向跟随速度

        if (isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);// 剑返回
            if (Vector2.Distance(transform.position, player.transform.position) < 1)
                player.CatchTheSword();// 剑返回后销毁剑
        }

        BounceLogic();// 剑反弹逻辑

        SpinLogic();// 剑旋转逻辑
    }

    // 销毁剑
    private void DestroyMe()
    {
        Destroy(gameObject);
    }

    // 剑旋转逻辑
    private void SpinLogic()
    {
        if (isSpinning)
        {
            if (Vector2.Distance(transform.position, player.transform.position) > maxTravelDistance && !wasStopped)
            {
                StopWhenSpinning();// 剑超出最大移动距离时停止移动
            }

            if (wasStopped)
            {
                spinTimer -= Time.deltaTime;// 减少旋转计时器

                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + spinDirection, transform.position.y), 1.5f * Time.deltaTime);

                if (spinTimer <= 0)
                {
                    isReturning = true;// 设置剑返回
                    isSpinning = false;// 设置剑停止旋转
                }

                hitTimer -= Time.deltaTime;// 减少攻击计时器

                if (hitTimer <= 0)
                {
                    hitTimer = hitCooldown;// 设置攻击计时器

                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1);
                    foreach (var hit in colliders)
                    {
                        if (hit.GetComponent<Enemy>() != null)
                            SwordSkillDamage(hit.GetComponent<Enemy>());// 旋转剑攻击敌人
                    }
                }
            }
        }
    }

    // 剑停止移动
    private void StopWhenSpinning()
    {
        wasStopped = true;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;// 停止剑的移动
        spinTimer = spinDuration;// 设置旋转计时器
    }

    // 剑反弹逻辑
    private void BounceLogic()
    {
        if (isBouncing && enemyTargets.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyTargets[targetIndex].position, bounceSpeed * Time.deltaTime);// 剑反弹
            if (Vector2.Distance(transform.position, enemyTargets[targetIndex].position) < 0.1f)
            {
                SwordSkillDamage(enemyTargets[targetIndex].GetComponent<Enemy>());// 反弹剑攻击敌人

                targetIndex++;
                bouncesAmount--;

                if (targetIndex >= enemyTargets.Count)// 超出敌人目标数量
                    targetIndex = 0;

                if (bouncesAmount <= 0)// 剑反弹次数达到上限
                {
                    isBouncing = false;
                    isReturning = true;
                    // cd.enabled = true;// 设置剑的碰撞器为可用
                }
            }
        }
    }

    // 设置剑
    public void SetupSword(Vector2 _direction, float _gravityScale, Player _player, float _freezeTimeDuration, float _returnSpeed)
    {

        player = _player;// 获取玩家引用
        freezeTimeDuration = _freezeTimeDuration;// 设置冻结时间
        returnSpeed = _returnSpeed;// 设置剑返回速度

        rb.velocity = _direction;// 设置剑的初始位置和方向
        rb.gravityScale = _gravityScale;// 设置剑的重力大小

        if (pierceAmount <= 0)
            anim.SetBool("Rotation", true);// 设置剑的旋转动画为播放状态

        spinDirection = Math.Clamp(rb.velocity.x, -1, 1);// 设置旋转剑的移动方向

        Invoke("DestroyMe", 7);// 设置剑的销毁时间
    }

    //碰撞检测
    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if (isReturning)
            return;// 如果剑正在返回，则不进行后续操作

        if (_collision.GetComponent<Enemy>() != null)
        {
            Enemy enemy = _collision.GetComponent<Enemy>();
            SwordSkillDamage(enemy);// 剑技能受伤
        }

        SetupTargetsForBounce(_collision);// 添加反弹范围内敌人目标

        StuckInto(_collision);// 剑插入物体
    }

    // 设置反弹
    public void SetupBounce(bool _isBouncing, int _bouncesAmount, float _bounceSpeed)
    {
        isBouncing = _isBouncing;// 设置剑是否反弹
        bouncesAmount = _bouncesAmount;// 设置剑反弹次数
        bounceSpeed = _bounceSpeed;// 设置剑反弹速度

        enemyTargets = new List<Transform>();// 初始化敌人目标列表
    }

    // 设置穿透
    internal void SetupPierce(int _pierceAmount)
    {
        pierceAmount = _pierceAmount;
    }

    // 设置旋转
    internal void SetupSpin(bool _isSpinning, float _maxTravelDistance, float _spinDuration, float _hitCooldown)
    {
        isSpinning = _isSpinning;
        maxTravelDistance = _maxTravelDistance;
        spinDuration = _spinDuration;
        hitCooldown = _hitCooldown;
        // isSpinning = true;
    }

    // 剑技能受伤
    private void SwordSkillDamage(Enemy enemy)
    {
        enemy.Damage();// 敌人受伤
        enemy.StartCoroutine("FreezeTimerFor", freezeTimeDuration);// 敌人冻结
    }

    // 添加反弹范围内敌人目标
    private void SetupTargetsForBounce(Collider2D _collision)
    {
        if (_collision.GetComponent<Enemy>() != null)
        {
            if (isBouncing && enemyTargets.Count <= 0)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);
                foreach (var collider in colliders)
                {
                    if (collider.GetComponent<Enemy>() != null)
                        enemyTargets.Add(collider.transform);
                }
            }
        }
    }

    //  剑插入物体
    private void StuckInto(Collider2D _collision)
    {

        if (isSpinning)
        {
            StopWhenSpinning();// 剑碰到物体时停止移动
            return;
        }

        if (pierceAmount > 0 && _collision.GetComponent<Enemy>() != null)// 剑有穿透次数
        {
            pierceAmount--;// 减少穿透次数
            return;
        }

        canRotate = false;// 设置剑的旋转状态为假
        cd.enabled = false;// 设置剑的碰撞器为不可用

        rb.isKinematic = true;// 设置剑为不可移动状态
        rb.constraints = RigidbodyConstraints2D.FreezeAll;// 设置剑的刚体约束为不可移动

        if (isBouncing && enemyTargets.Count > 0)// 反弹且有敌人目标 不停止旋转
            return;

        anim.SetBool("Rotation", false);// 设置剑的旋转动画为不可播放状态
        transform.parent = _collision.transform;// * 设置剑的父物体为碰撞物体
    }

    // 剑返回
    public void ReturnSword()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;// 设置剑的刚体约束为不可移动 // * 注意：这里需要设置为不可移动，否则剑会因为重力而继续移动,修复剑只要不触碰到物体就无法收回的bug
        //rb.isKinematic = false;// 设置剑为可移动状态
        transform.parent = null;// 设置剑的父物体为空
        isReturning = true;// 设置剑的返回状态为真
    }
}
