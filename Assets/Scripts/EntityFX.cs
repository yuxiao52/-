using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFX : MonoBehaviour
{

    private SpriteRenderer sr;// 精灵渲染器

    [Header("Flash FX")]
    [SerializeField] private Material hitMat;// 受击时显示的材料
    [SerializeField] private float flashDuration;// 闪烁持续时间
    private Material originalMat;// 原始材料

    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        originalMat = sr.material;
    }

    // 闪烁特效
    private IEnumerator FlashFX()
    {
        sr.material = hitMat;
        yield return new WaitForSeconds(flashDuration);
        sr.material = originalMat;
    }

    // 红色闪烁效果
    private void RedColorBlink()
    {
        if(sr.color != Color.white)
        {
            sr.color = Color.white;
        }
        else
        {
            sr.color = Color.red;
        }
    }

    // 取消红色闪烁效果
    private void CancelRedBlink()
    {
        CancelInvoke();// 取消调用
        sr.color = Color.white;
    }
}
