using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackhole_Skill_Controller : MonoBehaviour
{

    public float maxSize;// 最大
    public float growSpeed;// 扩大速度
    public bool canGrow;// 是否可以扩大

    public List<Transform> targets;// 目标列表

    private void Update()
    {
        if (canGrow)
        {
            Grow();
        }
    }

    private void Grow()
    {
        // 扩大
        transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        // 判断是否达到最大大小
        if (transform.localScale.x >= maxSize)
        {
            canGrow = false;
        }
    }

    // 碰撞检测
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.GetComponent<Enemy>() != null)
        {
            collider.GetComponent<Enemy>().FreezeTime(true);
            targets.Add(collider.transform);
        }
    }
}
