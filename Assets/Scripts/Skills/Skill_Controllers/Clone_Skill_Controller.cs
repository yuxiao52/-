using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone_Skill_Controller : MonoBehaviour
{

    private SpriteRenderer sr;// 分身渲染器
    private Animator anim;// 分身动画
    [SerializeField] private float colorLoosingSpeed;// 分身颜色消失速度

    private float cloneTimer;// 分身计时器
    [SerializeField] private Transform attackCheck;// 分身攻击检测点
    [SerializeField] private float attackCheckRadius = 0.8f;// 分身攻击检测半径

    private Transform closestEnemy;// 最近的敌人

    private bool canDuplicateClone;// 是否可以复制分身
    private int facingDirection = 1;// 分身朝向
    private float chanceToDuplicateClone;// 复制分身的概率

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        cloneTimer -= Time.deltaTime;
        if (cloneTimer <= 0)
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, sr.color.a - (colorLoosingSpeed * Time.deltaTime));
            if (sr.color.a <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    // 设置分身
    public void SetupClone(Transform _newTransform, float _cloneDuration, bool _canAttack, Vector3 _offset, Transform _closestEnemy, bool _canDuplicateClone, float _chanceToDuplicateClone)
    {
        if (_canAttack)
        {
            anim.SetInteger("AttackNumber", Random.Range(1, 3));
        }
        transform.position = _newTransform.position + _offset;// 设置分身位置 _offset偏移量
        cloneTimer = _cloneDuration;

        canDuplicateClone = _canDuplicateClone;
        chanceToDuplicateClone = _chanceToDuplicateClone;

        closestEnemy = _closestEnemy;
        FaceClosestTarget();// 面向最近的敌人
    }

    // 动画完成触发
    private void AnimationFinishTrigger()
    {
        cloneTimer = -0.1f;
    }

    // 攻击触发
    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                hit.GetComponent<Enemy>().Damage();

                // 有概率复制分身
                if (canDuplicateClone)
                {
                    if (Random.Range(0, 100) < chanceToDuplicateClone)
                    {
                        SkillManager.instance.clone.CreateClone(hit.transform, new Vector3(0.5f * facingDirection, 0));
                    }
                }
            }
        }
    }

    // 面向最近的敌人
    private void FaceClosestTarget()
    {
        if (closestEnemy != null)
        {
            if (transform.position.x > closestEnemy.position.x)// 面向最近的敌人
            {
                facingDirection = -1;
                transform.Rotate(0, 180, 0);
            }
        }
    }
}
