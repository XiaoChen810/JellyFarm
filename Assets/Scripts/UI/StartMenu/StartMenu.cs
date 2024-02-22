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

    private bool animationPlayed = false; // ���ڱ�Ƕ����Ƿ��Ѿ����Ź�
    public float minAnimationDuration = 2.0f; // ��С����ʱ��
    private Animator anim;
    void Start()
    {
        // Application.targetFrameRate = 60;

        PlayBtn.onClick.AddListener(OnPlayButtonClicked);
        SettingBtn.onClick.AddListener(OnSettingButtonClicked);
        ResetBtn.onClick.AddListener(OnResetButtonClicked);

        // ��ʼ��ʱ���ؼ��ض���
        if (loadingScreen != null)
        {
            anim = loadingScreen.GetComponent<Animator>();
            loadingScreen.SetActive(false);
        }
    }

    public void OnPlayButtonClicked()
    {
        // ����� Play ��ťʱ���ø÷���
        // ���ü��ض���
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(true);
            animationPlayed = false;
        }

        // �첽������һ������
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
        // �첽������һ������
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextSceneName);
        asyncLoad.allowSceneActivation = false;

        // �ȴ��������
        while (!asyncLoad.isDone)
        {
            // �������û�в��Ź������Ѿ����ټ�����һ��
            if (asyncLoad.progress >= 0.1f && !animationPlayed)
            {
                if (anim != null)
                {
                    anim.SetInteger("Load", 1);
                    yield return new WaitForSeconds(0.83f);

                    anim.SetInteger("Load", 2);
                    yield return new WaitForSeconds(minAnimationDuration);

                    animationPlayed = true; // ��Ƕ����Ѿ����Ź�
                }
            }

            if (animationPlayed)
            {
                asyncLoad.allowSceneActivation = true;
            }
            

            yield return null; // �ȴ���һ֡
        }

        // �ڼ�����ɺ�ִ��һЩ����
        Debug.Log("Loading completed!");

        // ���ü��ض���
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(false);
        }

    }
}
