using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal_Skill : Skill
{

    [SerializeField] private GameObject crystalPrefab;// 水晶预制体
    private GameObject currentCrystal;// 当前水晶
    [SerializeField] private float crystalDuration;// 水晶持续时间

    [Header("Explode crystal")]
    [SerializeField] private bool canExplode;// 是否可以爆炸

    [Header("Move crystal")]
    [SerializeField] private bool canMoveToEnemy;// 是否可以移动到敌人
    [SerializeField] private float moveSpeed;// 移动速度

    [Header("Multi crystal")]
    [SerializeField] private bool canUseMultiCrystal;// 是否可以使用多重水晶
    [SerializeField] private int crystalAmounts;// 水晶数量
    [SerializeField] private float multiCrystalCooldown;// 多水晶冷却时间
    [SerializeField] private float useTimeWindow;// 使用时间窗口
    [SerializeField] private List<GameObject> crystalList = new List<GameObject>();// 水晶列表

    [Header("Crystal mirage")]
    [SerializeField] private bool canInterchangeCreateClone;// 是否可以交换位置后创建分身

    public override void UseSkill()
    {
        base.UseSkill();

        if (CanUseMultiCrystal())
            return;

        if (currentCrystal == null)
        {
            CreateCrystal();
        }
        else
        {
            // 限制玩家在水晶可以移动时瞬移
            if (canMoveToEnemy)
                return;

            // 爆炸前与角色交换位置
            Vector2 playerPos = player.transform.position;
            player.transform.position = currentCrystal.transform.position;
            currentCrystal.transform.position = playerPos;

            // 交换位置后创建分身
            if (canInterchangeCreateClone)
            {
                SkillManager.instance.clone.CreateClone(currentCrystal.transform, Vector3.zero);
                Destroy(currentCrystal);
            }
            else
            {
                // 水晶爆炸
                currentCrystal.GetComponent<Crystal_Skill_Controller>()?.FinishCrystal();
            }

        }
    }

    //  创建水晶
    public void CreateCrystal()
    {
        currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
        Crystal_Skill_Controller currentCrystalScript = currentCrystal.GetComponent<Crystal_Skill_Controller>();
        currentCrystalScript.SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, FindClosestEnemy(currentCrystal.transform));
    }

    //  创建黑洞水晶
    public void CreateBlackholeCrystal(Transform _enemyTransform)
    {
        currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
        Crystal_Skill_Controller currentCrystalScript = currentCrystal.GetComponent<Crystal_Skill_Controller>();
        currentCrystalScript.SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, _enemyTransform);
    }

    // // 当前水晶选择随机目标
    // public void CurrentCrystalChooseRandomTarget() => currentCrystal.GetComponent<Crystal_Skill_Controller>().ChooseRandomEnemy();

    // 判断是否可以多重水晶
    private bool CanUseMultiCrystal()
    {
        if (canUseMultiCrystal)
        {
            if (crystalList.Count > 0)
            {
                if (crystalList.Count == crystalAmounts)
                {
                    Invoke("ResetAbility", useTimeWindow);
                }

                cooldown = 0;

                GameObject crystalToSpawn = crystalList[crystalList.Count - 1];// 获取最后一个水晶
                GameObject newCrystal = Instantiate(crystalToSpawn, player.transform.position, Quaternion.identity);

                crystalList.Remove(crystalToSpawn);// TODO 此处有概率引起报错：ObjectDisposedException: SerializedProperty crystalList.Array.data[2] has disappeared!   解决方法：测试的时候如果Inspector没有看着脚本或脚本折叠起来就不会报这个错。报错是关于UnityEditor的，可能是官方的接口更新显示相关的报错。不是脚本的错误

                newCrystal.GetComponent<Crystal_Skill_Controller>()
                    .SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, FindClosestEnemy(newCrystal.transform));

                if (crystalList.Count <= 0)
                {
                    cooldown = multiCrystalCooldown;// 设置冷却时间
                    RefillCrystal();// 重新填充水晶
                }

                return true;
            }
        }
        return false; ;
    }

    // 重新填充水晶
    private void RefillCrystal()
    {
        int addAmount = crystalAmounts - crystalList.Count;
        for (int i = 0; i < addAmount; i++)
        {
            crystalList.Add(crystalPrefab);
        }
    }

    // 重置技能
    private void ResetAbility()
    {
        if (cooldownTimer > 0)
            return;

        cooldownTimer = multiCrystalCooldown;
        RefillCrystal();// 重新填充水晶
    }

}
