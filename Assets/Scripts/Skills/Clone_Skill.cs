using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone_Skill : Skill
{

    [Header("Clone info")]
    [SerializeField] private GameObject clonePrefab;// 克隆预制体
    [SerializeField] private float cloneDuration;// 分身持续时间
    [Space]
    [SerializeField] private bool canAttack;// 是否可以攻击

    [SerializeField] private bool createCloneOnDashStart;// 是否在冲刺开始时创建分身
    [SerializeField] private bool createCloneOnDashOver;// 是否在冲刺结束时创建分身
    [SerializeField] private bool createCloneOnCounterAttack;// 是否在反击时创建分身

    [Header("Clone can duplicate")]
    [SerializeField] private bool canDuplicateClone;// 是否可以复制分身
    [SerializeField] private float chanceToDuplicateClone;// 复制分身的概率

    [Header("Crystal instead of clone")]
    public bool crystalInsteadOfClone;// 是否使用水晶代替分身

    public void CreateClone(Transform _clonePosition, Vector3 _offset)
    {
        if (crystalInsteadOfClone)
        {
            // 创建水晶
            SkillManager.instance.crystal.CreateCrystal();
            return;
        }

        GameObject newClone = Instantiate(clonePrefab);

        newClone.GetComponent<Clone_Skill_Controller>().SetupClone(_clonePosition, cloneDuration, canAttack, _offset, FindClosestEnemy(newClone.transform), canDuplicateClone, chanceToDuplicateClone);// ? 为什么不直接在这里写 Skill写使用技能的代码 SkillController写技能逻辑的代码
        // TODO 交换一下可能更好

    }

    // 冲刺开始时创建分身
    public void CreateCloneOnDashStart()
    {
        if (createCloneOnDashStart)
        {
            CreateClone(player.transform, Vector3.zero);
        }
    }

    // 冲刺结束时创建分身
    public void CreateCloneOnDashOver()
    {
        if (createCloneOnDashOver)
        {
            CreateClone(player.transform, Vector3.zero);
        }
    }

    // 反击时创建分身
    public void CreateCloneOnCounterAttack(Transform _enemyTransform)
    {
        if (createCloneOnCounterAttack)
        {
            StartCoroutine(CreateCloneWithDelay(_enemyTransform, new Vector3(2 * player.facingDirection, 0)));// 协程 延迟创建分身
        }
    }

    // 延迟创建分身
    private IEnumerator CreateCloneWithDelay(Transform _enemyTransform, Vector3 _offset)
    {
        yield return new WaitForSeconds(0.4f);
        CreateClone(_enemyTransform.transform, _offset);
    }
}
