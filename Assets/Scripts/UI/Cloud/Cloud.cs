using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    public float speed = 5f; // �ƶ��ٶ�
    public float startX = -10f; // ��ʼ��
    public float targetX = 10f; // Ŀ���

    void Update()
    {
        MoveCloud();
    }

    void MoveCloud()
    {
        // �ƶ�
        float step = speed * Time.deltaTime;
        transform.Translate(Vector3.right * step);

        // ����Ƿ񵽴�Ŀ��㣬������������λ�õ���ʼ��
        if (transform.position.x >= targetX)
        {
            transform.position = new Vector3(startX, transform.position.y, transform.position.z);
        }
    }

}
