using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{

    [SerializeField] protected float cooldown;// 技能冷却时间
    protected float cooldownTimer;// 技能冷却时间计时器
    // [SerializeField] protected float duration;// 技能持续时间
    // [SerializeField] protected float skillDamage;// 技能伤害
    // [SerializeField] protected float skillRange;// 技能范围

    protected Player player;// 玩家引用

    protected virtual void Start()
    {
        player = PlayerManager.instance.player;
    }

    protected virtual void Update()
    {
        cooldownTimer -= Time.deltaTime;
    }

    // 能否使用技能
    public virtual bool CanUseSkill()
    {
        if (cooldownTimer <= 0)
        {
            UseSkill();
            cooldownTimer = cooldown;
            return true;
        }
        else
        {
            Debug.Log("Skill is on cooldown");
            return false;
        }
    }

    // 使用技能
    public virtual void UseSkill()
    {
        Debug.Log("Skill is used");
    }

}
