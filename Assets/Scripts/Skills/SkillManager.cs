using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{

    public static SkillManager instance;// 单例模式


    public Dash_Skill dash { get; private set; }// 冲刺技能
    public Clone_Skill clone { get; private set; }// 克隆技能
    public Sword_Skill sword { get; private set; }// 投掷剑技能
    public Blackhole_Skill blackhole { get; private set; }// 黑洞技能
    public Crystal_Skill crystal { get; private set; }// 水晶技能

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);// 防止重复实例化
            return;
        }
        instance = this;
    }

    private void Start()
    {
        dash = GetComponent<Dash_Skill>();
        clone = GetComponent<Clone_Skill>();
        sword = GetComponent<Sword_Skill>();
        blackhole = GetComponent<Blackhole_Skill>();
        crystal = GetComponent<Crystal_Skill>();
    }

}
