using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    public string nextSceneName = "NextSceneName";
    public Button PlayBtn;
    public Button SettingBtn;
    public Button ResetBtn;
    public GameObject SettingPanel;
    public GameObject loadingScreen;

    private bool animationPlayed = false; // 用于标记动画是否已经播放过
    public float minAnimationDuration = 2.0f; // 最小播放时间
    private Animator anim;
    void Start()
    {
        // Application.targetFrameRate = 60;

        PlayBtn.onClick.AddListener(OnPlayButtonClicked);
        SettingBtn.onClick.AddListener(OnSettingButtonClicked);
        ResetBtn.onClick.AddListener(OnResetButtonClicked);

        // 初始化时隐藏加载动画
        if (loadingScreen != null)
        {
            anim = loadingScreen.GetComponent<Animator>();
            loadingScreen.SetActive(false);
        }
    }

    public void OnPlayButtonClicked()
    {
        // 当点击 Play 按钮时调用该方法
        // 启用加载动画
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(true);
            animationPlayed = false;
        }

        // 异步加载下一个场景
        StartCoroutine(LoadNextSceneAsync());
    }

    public void OnSettingButtonClicked()
    {
        SettingPanel.SetActive(!SettingPanel.activeSelf);
        Debug.Log("ClickSettingButton");
    }

    public void OnResetButtonClicked()
    {
        ES3.DeleteFile();
        Debug.Log("ClickResetButton");
    }


    IEnumerator LoadNextSceneAsync()
    {
        // 异步加载下一个场景
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextSceneName);
        asyncLoad.allowSceneActivation = false;

        // 等待加载完成
        while (!asyncLoad.isDone)
        {
            // 如果动画没有播放过并且已经至少加载了一次
            if (asyncLoad.progress >= 0.1f && !animationPlayed)
            {
                if (anim != null)
                {
                    anim.SetInteger("Load", 1);
                    yield return new WaitForSeconds(0.83f);

                    anim.SetInteger("Load", 2);
                    yield return new WaitForSeconds(minAnimationDuration);

                    animationPlayed = true; // 标记动画已经播放过
                }
            }

            if (animationPlayed)
            {
                asyncLoad.allowSceneActivation = true;
            }
            

            yield return null; // 等待下一帧
        }

        // 在加载完成后执行一些操作
        Debug.Log("Loading completed!");

        // 禁用加载动画
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(false);
        }

    }
}
