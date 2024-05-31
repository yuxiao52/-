using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SwordType
{
    Regular,// 普通剑
    Bounce,// 反弹
    Pierce,// 穿透
    Spin// 旋转
}

public class Sword_Skill : Skill
{

    public SwordType swordType = SwordType.Regular;

    [Header("Skill info")]
    [SerializeField] private GameObject swordPrefab;// 剑预制体
    [SerializeField] private Vector2 launchForce;// 发射力
    [SerializeField] private float swordGravity;// 剑的重力
    [SerializeField] private float freezeTimeDuration;// 冻结时间
    [SerializeField] private float returnSpeed;// 返回速度

    private Vector2 finalDir;// 最终方向


    [Header("Aim dots")]
    [SerializeField] private GameObject dotPrefab;// 瞄准点预制体
    [SerializeField] private int dotNumber;// 瞄准点数量
    [SerializeField] private float dotSpace;// 瞄准点间距
    [SerializeField] private Transform dotsParent;// ? 瞄准点父物体

    private GameObject[] dots;// 瞄准点数组

    [Header("Bounce info")]
    [SerializeField] private int bounceAmount;// 反弹次数
    [SerializeField] private float bounceGravity;// 剑反弹时重力
    [SerializeField] private float bounceSpeed;// 剑反弹速度

    [Header("Pierce info")]
    [SerializeField] private int pierceAmount;// 穿透次数
    [SerializeField] private float pierceGravity;// 剑穿透时重力

    [Header("Spin info")]
    [SerializeField] private float maxTravelDistance = 7;// 最大移动距离
    [SerializeField] private float spinDuration = 2;// 旋转持续时间
    [SerializeField] private float spinGravity = 1;// 剑旋转时重力
    [SerializeField] private float hitCooldown = 0.35f;// 剑攻击冷却时间

    protected override void Start()
    {
        base.Start();

        GenerateDots();// 生成瞄准点

        SetupGravity();// 设置剑的重力
    }

    // 设置剑的重力
    private void SetupGravity()
    {
        if (swordType == SwordType.Bounce)
        {
            swordGravity = bounceGravity;
        }
        else if (swordType == SwordType.Pierce)
        {
            swordGravity = pierceGravity;
        }
        else if (swordType == SwordType.Spin)
        {
            swordGravity = spinGravity;
        }
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            finalDir = new Vector2(AimDirection().normalized.x * launchForce.x, AimDirection().normalized.y * launchForce.y);
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            for (int i = 0; i < dots.Length; i++)
            {
                dots[i].transform.position = DotsPosition(i * dotSpace);
            }
        }
    }


    // 创建剑
    public void CreateSword()
    {
        GameObject newSword = Instantiate(swordPrefab, player.transform.position, transform.rotation);
        Sword_Skill_Controller newSwordController = newSword.GetComponent<Sword_Skill_Controller>();

        if (swordType == SwordType.Bounce)
            newSwordController.SetupBounce(true, bounceAmount, bounceSpeed);
        else if (swordType == SwordType.Pierce)
            newSwordController.SetupPierce(pierceAmount);
        else if (swordType == SwordType.Spin)
            newSwordController.SetupSpin(true, maxTravelDistance, spinDuration, hitCooldown);


        newSwordController.SetupSword(finalDir, swordGravity, player, freezeTimeDuration, returnSpeed);

        player.AssignNewSword(newSword);

        DotsActive(false);// 隐藏瞄准点
    }

    #region Aim 瞄准
    // 瞄准方向
    public Vector2 AimDirection()
    {
        Vector2 playerPosition = player.transform.position;// 获取玩家当前位置
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);// 获取鼠标位置

        Vector2 direction = mousePosition - playerPosition;// 计算玩家指向鼠标的方向

        return direction;
    }

    // 生成瞄准点
    private void GenerateDots()
    {
        dots = new GameObject[dotNumber];// 创建瞄准点数组

        for (int i = 0; i < dotNumber; i++)
        {
            dots[i] = Instantiate(dotPrefab, player.transform.position, Quaternion.identity, dotsParent);// 创建瞄准点

            dots[i].SetActive(false);// 隐藏瞄准点
        }
    }

    // 显示或隐藏瞄准点
    public void DotsActive(bool _isActive)
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].SetActive(_isActive);// 显示或隐藏瞄准点
        }
    }

    // 瞄准点位置
    public Vector2 DotsPosition(float t)
    {
        Vector2 position = (Vector2)player.transform.position + new Vector2(
            AimDirection().normalized.x * launchForce.x,
            AimDirection().normalized.y * launchForce.y) * t + 0.5f * (Physics2D.gravity * swordGravity) * (t * t);// vt+1/2*g*t^2
        return position;
    }
    #endregion
}
