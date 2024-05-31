using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{

    private GameObject camera;// 主相机

    [SerializeField] private float parallaxEffect;// 视差效果

    private float xPosition;// 背景的初始位置
    private float length;// 背景的长度

    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.Find("Main Camera");

        xPosition = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    // void Update()
    // {
    //     float distanceToMove = camera.transform.position.x * parallaxEffect;// 背景移动的距离
    //     transform.position = new Vector3(xPosition + distanceToMove, transform.position.y);

    //     float distanceMoved = camera.transform.position.x * (1 - parallaxEffect);// 背景相对主相机移动距离
    //     if (distanceMoved > xPosition + length)
    //     {
    //         xPosition += length;
    //     }
    //     else if (distanceMoved < xPosition - length)
    //     {
    //         xPosition -= length;
    //     }
    // }

    void FixedUpdate()
    {
        float distanceToMove = camera.transform.position.x * parallaxEffect;// 背景移动的距离
        transform.position = new Vector3(xPosition + distanceToMove, transform.position.y);

        float distanceMoved = camera.transform.position.x * (1 - parallaxEffect);// 背景相对主相机移动距离
        if (distanceMoved > xPosition + length)
        {
            xPosition += length;
        }
        else if (distanceMoved < xPosition - length)
        {
            xPosition -= length;
        }
    }
}
