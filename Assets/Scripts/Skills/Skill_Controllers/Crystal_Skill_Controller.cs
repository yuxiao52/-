using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal_Skill_Controller : MonoBehaviour
{

    private Animator anim => GetComponent<Animator>();
    private CircleCollider2D cd => GetComponent<CircleCollider2D>();

    private float crystalTimer;// 水晶计时器

    private bool canExplode;// 是否可以爆炸
    private bool canMove;// 是否可以移动
    private float moveSpeed;// 移动速度

    private bool canGrow;// 是否可以增长
    private float growSpeed = 5;// 增长速度

    public Transform closestEnemy;// 最近的敌人

    private float crystalDirection = 1;// 水晶移动方向
    private bool isChangeFacingDirection = true;// 是否改变水晶移动方向 // * 固定水晶移动方向

    [SerializeField] private LayerMask whatIsEnemy;// 敌人层

    public void SetupCrystal(float _crystalDuration, bool _canExplode, bool _canMove, float _moveSpeed, Transform _closestEnemy)
    {
        crystalTimer = _crystalDuration;
        canExplode = _canExplode;
        canMove = _canMove;
        moveSpeed = _moveSpeed;
        closestEnemy = _closestEnemy;
    }

    private void Update()
    {
        crystalTimer -= Time.deltaTime;

        if (crystalTimer < 0)
        {
            FinishCrystal();
        }

        if (canMove)
        {
            // 修复攻击范围内没有敌人会报错的bug
            if (closestEnemy != null)
            {
                transform.position = Vector2.MoveTowards(transform.position, closestEnemy.position, moveSpeed * Time.deltaTime);

                if (Vector2.Distance(transform.position, closestEnemy.position) < 1)
                {
                    FinishCrystal();// 如果到达敌人位置，完成水晶技能
                    canMove = false;
                }
            }
            else
            {
                // 使水晶移动方向为玩家朝向，并固定水晶移动方向
                if (isChangeFacingDirection)
                {
                    crystalDirection = PlayerManager.instance.player.facingDirection;
                    isChangeFacingDirection = false;
                }
                transform.position = Vector2.MoveTowards(transform.position, transform.position + new Vector3(5 * crystalDirection, 0), moveSpeed * Time.deltaTime);

            }
        }

        if (canGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(3, 3), growSpeed * Time.deltaTime);
        }
    }

    // // 随机选择范围内敌人
    // public void ChooseRandomEnemy()
    // {
    //     float radius = SkillManager.instance.blackhole.GetBlackholeRadius();
    //     // 获取范围内所有敌人
    //     Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, whatIsEnemy);

    //     // 随机选择一个敌人
    //     if (colliders.Length > 0)
    //     {
    //         int randomIndex = Random.Range(0, colliders.Length);
    //         closestEnemy = colliders[randomIndex].transform;
    //     }
    // }

    // 动画爆炸事件
    private void AnimationExplodeEvent()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, cd.radius);// 获取所有碰撞体

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
                hit.GetComponent<Enemy>().Damage();
        }
    }

    // 完成水晶技能
    public void FinishCrystal()
    {
        if (canExplode)
        {
            canGrow = true;
            anim.SetTrigger("Explode");// 播放爆炸动画
        }
        else
            SelfDestroy();
    }

    // 自动销毁
    public void SelfDestroy() => Destroy(gameObject);
}
