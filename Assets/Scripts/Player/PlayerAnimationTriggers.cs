using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();

    // 动画完成触发
    private void AnimationFinishTrigger()
    {
        player.AnimationFinishTrigger();
    }

    // 攻击触发
    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);
        foreach (var collider in colliders)
        {
            if (collider.GetComponent<Enemy>() != null)
                collider.GetComponent<Enemy>().Damage();
            // if (collider.CompareTag("Enemy"))
            // {
            //     collider.GetComponent<Enemy>().TakeDamage(player.attackDamage);
            // }
        }
    }

    // 投掷剑
    private void ThrowSword()
    {
        SkillManager.instance.sword.CreateSword();
    }
}
