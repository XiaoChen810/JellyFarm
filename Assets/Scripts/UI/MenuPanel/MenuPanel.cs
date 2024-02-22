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
        // 添加按钮点击事件监听器
        continueButton.onClick.AddListener(ContinueGame);
        quitButton.onClick.AddListener(ExitGame);
    }

    void ContinueGame()
    {
        // TODO: 在这里添加继续游戏的逻辑
        Debug.Log("Continue Game");
        Time.timeScale = 1.0f;
        gameObject.SetActive(false);
    }

    void ExitGame()
    {
        // 退出游戏
        Debug.Log("Exit Game");
        Application.Quit();
    }
}
