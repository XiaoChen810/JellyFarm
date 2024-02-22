using UnityEngine;

public class ChangeCursor : MonoBehaviour
{
    public Texture2D hammerCursor;

    void Start()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto); // ��ʼ�����

        UIManager.Instance.OnClickToolBtn += SetHammerCursor;
    }

    void Update()
    {   
        if (Input.GetMouseButtonDown(1)) // �Ҽ�����ʱ����
        {
            ResetCursor();
        }
    }

    void SetHammerCursor()
    {
        Cursor.SetCursor(hammerCursor, Vector2.zero, CursorMode.Auto);
    }
    void ResetCursor()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}
