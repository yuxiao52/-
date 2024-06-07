using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackhole_Skill_Controller : MonoBehaviour
{

    [SerializeField] private GameObject hotKeyPrefab;// 热键预制体
    [SerializeField] private List<KeyCode> keyCodeList;// 热键列表

    public float maxSize;// 最大
    public float growSpeed;// 扩大速度
    public float shrinkSpeed;// 收缩速度
    private bool canGrow = true;// 是否可以扩大
    private bool canShrink;// 是否可以收缩
    private float blackholeTimer;// 黑洞计时器


    private int attackAmounts = 4;// 攻击次数
    private float cloneAttackCooldown = 0.3f;// 攻击冷却时间
    private float cloneAttackTimer;// 攻击计时器
    private bool cloneAttackReleased;// 攻击是否释放
    private bool canCreateHotKeys = true;// 是否可以创建热键 // * 控制后面进入的敌人不生成热键
    private bool playerCanDisapear = true;// 玩家是否可以消失

    private List<Transform> targets = new List<Transform>();// 目标列表
    private List<GameObject> createdHotKeys = new List<GameObject>();// 热键列表

    public bool playerCanExitState { get; private set; }// 玩家是否可以退出状态

    private void Update()
    {
        cloneAttackTimer -= Time.deltaTime;
        blackholeTimer -= Time.deltaTime;

        if (blackholeTimer <= 0)
        {
            if (targets.Count > 0)
            {
                blackholeTimer = Mathf.Infinity;// 防止再次进入黑洞

                ReleaseCloneAttack();
            }
            else
            {
                FinishBlackholeAbility();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ReleaseCloneAttack();// 释放分身攻击
        }

        CloneAttackLogic();// 分身攻击逻辑

        if (canGrow && !canShrink)
        {
            Grow();// 扩大
        }

        if (canShrink)
        {
            Shrink();// 收缩
        }
    }

    // 设置黑洞
    public void SetupBlackhole(float _maxSize, float _growSpeed, float _shrinkSpeed, int _attackAmounts, float _cloneAttackCooldown, float _blackholeDuration)
    {
        maxSize = _maxSize;
        growSpeed = _growSpeed;
        shrinkSpeed = _shrinkSpeed;
        attackAmounts = _attackAmounts;
        cloneAttackCooldown = _cloneAttackCooldown;

        blackholeTimer = _blackholeDuration;

        if (SkillManager.instance.clone.crystalInsteadOfClone)
        {
            playerCanDisapear = false;// 玩家不会消失
        }
    }

    // 释放分身攻击
    private void ReleaseCloneAttack()
    {
        if (targets.Count <= 0)
            return;

        cloneAttackReleased = true;
        canCreateHotKeys = false;
        DestroyHotKeys();

        if (playerCanDisapear)
        {
            playerCanDisapear = false;
            PlayerManager.instance.player.MakeTransprent(true);
        }
    }

    // 分身攻击逻辑
    private void CloneAttackLogic()
    {
        if (cloneAttackTimer <= 0 && cloneAttackReleased && attackAmounts > 0)
        {
            cloneAttackTimer = cloneAttackCooldown;

            int randomIndex = Random.Range(0, targets.Count);

            float xOffset;// X轴偏移量
            if (Random.Range(0, 100) > 50)
                xOffset = 1.5f;
            else
                xOffset = -1.5f;

            if (SkillManager.instance.clone.crystalInsteadOfClone)
            {
                // SkillManager.instance.crystal.CreateCrystal();// 创建水晶
                // SkillManager.instance.crystal.CurrentCrystalChooseRandomTarget();// 随机选择一个敌人
                // SkillManager.instance.crystal.GetComponent<Crystal_Skill_Controller>().closestEnemy = targets[randomIndex];// 设置水晶的目标 // * NullReferenceException: Object reference not set to an instance of an object
                SkillManager.instance.crystal.CreateBlackholeCrystal(targets[randomIndex]);// 创建黑洞水晶 随机选择选中的目标
            }
            else
            {
                SkillManager.instance.clone.CreateClone(targets[randomIndex], new Vector3(xOffset, 0, 0));// 创建分身
            }

            attackAmounts--;

            if (attackAmounts <= 0)
            {
                Invoke("FinishBlackholeAbility", 0.5f);// 完成黑洞技能
            }
        }
    }

    // 完成黑洞技能
    private void FinishBlackholeAbility()
    {
        DestroyHotKeys();// 销毁热键
        playerCanExitState = true;

        canShrink = true;
        cloneAttackReleased = false;
    }

    // 碰撞检测
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().FreezeTime(true);
            CreateHotKey(collision);
        }
    }

    // 碰撞退出
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().FreezeTime(false);
        }
    }

    // 扩大
    private void Grow()
    {
        transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
    }

    // 收缩
    private void Shrink()
    {
        transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shrinkSpeed * Time.deltaTime);
        if (transform.localScale.x < 0)
            Destroy(gameObject);
    }

    // 创建热键
    private void CreateHotKey(Collider2D collision)
    {
        if (keyCodeList.Count <= 0)
            return;

        if (!canCreateHotKeys)// 如果不能创建热键
            return;

        GameObject newHotKey = Instantiate(hotKeyPrefab, collision.transform.position + new Vector3(0, 2), Quaternion.identity);// 创建热键
        createdHotKeys.Add(newHotKey);// 添加到热键列表

        KeyCode chooseKey = keyCodeList[Random.Range(0, keyCodeList.Count)];// 随机选择一个热键
        keyCodeList.Remove(chooseKey);// 移除该热键

        Blackhole_HotKey_Controller newHotKeyScript = newHotKey.GetComponent<Blackhole_HotKey_Controller>();

        newHotKeyScript.SetupHotKey(chooseKey, collision.transform, this);// 设置热键
    }

    // 添加目标
    public void AddEnemyToList(Transform _enemy) => targets.Add(_enemy);

    // 销毁热键
    private void DestroyHotKeys()
    {
        if (createdHotKeys.Count <= 0)
            return;

        for (int i = 0; i < createdHotKeys.Count; i++)
        {
            Destroy(createdHotKeys[i]);
        }
    }

}
