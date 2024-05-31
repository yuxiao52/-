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
    public void SetupClone(Transform _newTransform, float _cloneDuration, bool _canAttack)
    {
        if (_canAttack)
        {
            anim.SetInteger("AttackNumber", Random.Range(1, 3));
        }
        transform.position = _newTransform.position;
        cloneTimer = _cloneDuration;

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
        foreach (var collider in colliders)
        {
            if (collider.GetComponent<Enemy>() != null)
                collider.GetComponent<Enemy>().Damage();
        }
    }

    // 面向最近的敌人
    private void FaceClosestTarget()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);
        float closestDistance = Mathf.Infinity;// 最近的距离
        Transform closestTarget = null;// 最近的敌人
        foreach (var collider in colliders)
        {
            if (collider.GetComponent<Enemy>() != null)
            {
                float distance = Vector2.Distance(transform.position, collider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = collider.transform;
                }
            }

            if (closestTarget != null)
            {
                if (transform.position.x > closestTarget.position.x)// 面向最近的敌人
                {
                    transform.Rotate(0, 180, 0);
                }
            }
        }
    }
}
