using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    public float speed = 5f; // 移动速度
    public float startX = -10f; // 起始点
    public float targetX = 10f; // 目标点

    void Update()
    {
        MoveCloud();
    }

    void MoveCloud()
    {
        // 移动
        float step = speed * Time.deltaTime;
        transform.Translate(Vector3.right * step);

        // 检查是否到达目标点，如果到达，则重置位置到起始点
        if (transform.position.x >= targetX)
        {
            transform.position = new Vector3(startX, transform.position.y, transform.position.z);
        }
    }

}
