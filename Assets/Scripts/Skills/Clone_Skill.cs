using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone_Skill : Skill
{

    [Header("Clone info")]
    [SerializeField] private GameObject clonePrefab;// 克隆预制体
    [SerializeField] private float cloneDuration;// 分身持续时间
    [SerializeField] private bool canAttack;// 是否可以攻击

    public void CreateClone(Transform _clonePosition)
    {
        GameObject newClone = Instantiate(clonePrefab);

        newClone.GetComponent<Clone_Skill_Controller>().SetupClone(_clonePosition,cloneDuration,canAttack);// ? 为什么不直接在这里写

    }
}
