using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{

    public static SkillManager instance;// 单例模式


    public Dash_Skill dash { get; private set; }// 冲刺技能
    public Clone_Skill clone { get; private set; }// 克隆技能
    public Sword_Skill sword { get; private set; }// 投掷剑技能

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);// 防止重复实例化
            return;
        }
        instance = this;
        // DontDestroyOnLoad(instance.gameObject);
    }

    private void Start()
    {
        dash = GetComponent<Dash_Skill>();
        clone = GetComponent<Clone_Skill>();
        sword = GetComponent<Sword_Skill>();
    }

}
