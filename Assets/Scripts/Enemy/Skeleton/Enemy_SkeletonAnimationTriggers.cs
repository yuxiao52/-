using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_SkeletonAnimationTriggers : MonoBehaviour
{
    
    private Enemy_Skeleton enemy => GetComponentInParent<Enemy_Skeleton>();
    
    private void AnimationFinishTrigger()
    {
        enemy.AnimationFinishTrigger();
    }

    // 攻击触发
    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius);
        foreach (var collider in colliders)
        {
            if (collider.GetComponent<Player>() != null)
                collider.GetComponent<Player>().Damage();
            // if (collider.CompareTag("Enemy"))
            // {
            //     collider.GetComponent<Enemy>().TakeDamage(player.attackDamage);
            // }
        }
    }

    // 打开反击窗口
    private void OpenCounterWindow() => enemy.OpenCounterAttackWindow();
    // 关闭反击窗口
    private void CloseCounterWindow() => enemy.CloseCounterAttackWindow();
}
