using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPanel : MonoBehaviour
{
    public Button continueButton;
    public Button quitButton;

    void Start()
    {
        // ��Ӱ�ť����¼�������
        continueButton.onClick.AddListener(ContinueGame);
        quitButton.onClick.AddListener(ExitGame);
    }

    void ContinueGame()
    {
        // TODO: ��������Ӽ�����Ϸ���߼�
        Debug.Log("Continue Game");
        Time.timeScale = 1.0f;
        gameObject.SetActive(false);
    }

    void ExitGame()
    {
        // �˳���Ϸ
        Debug.Log("Exit Game");
        Application.Quit();
    }
}
