using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackhole_Skill : Skill
{

    [SerializeField] private GameObject blackholePrefab;// 黑洞预制体
    [SerializeField] private float maxSize;// 最大
    [SerializeField] private float growSpeed;// 扩大速度
    [SerializeField] private float shrinkSpeed;// 收缩速度
    [SerializeField] private float blackholeDuration;// 黑洞持续时间
    [Space]
    [SerializeField] private int attackAmounts = 4;// 攻击次数
    [SerializeField] private float cloneAttackCooldown = 0.3f;// 攻击冷却时间

    Blackhole_Skill_Controller currentBlackhole;// 当前黑洞

    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();

        GameObject newBlackhole = Instantiate(blackholePrefab, player.transform.position, Quaternion.identity);

        currentBlackhole = newBlackhole.GetComponent<Blackhole_Skill_Controller>();

        currentBlackhole.SetupBlackhole(maxSize, growSpeed, shrinkSpeed, attackAmounts, cloneAttackCooldown, blackholeDuration);
    }

    // 检测是否完成技能
    public bool SkillCompleted()
    {
        if (!currentBlackhole)
            return false;

        if (currentBlackhole.playerCanExitState)
        {
            currentBlackhole = null;
            return true;
        }

        return false;
    }

    // 获取黑洞半径
    public float GetBlackholeRadius()
    {
        return maxSize / 2;
    }
}
