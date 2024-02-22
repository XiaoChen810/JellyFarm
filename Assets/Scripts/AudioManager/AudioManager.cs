using UnityEngine;
using UnityEngine.Events;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance; // 单例实例

    public AudioClip mainMusic; // 主音乐

    private AudioSource musicAudioSource; // 用于播放主音乐的 AudioSource

    private void Awake()
    {
        // 设置单例实例
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 保留在场景切换时
        }
        else
        {
            Destroy(gameObject);
        }

        // 初始化音频源
        musicAudioSource = gameObject.GetComponent<AudioSource>();
        musicAudioSource.clip = mainMusic;
        musicAudioSource.loop = true; // 循环播放
    }

    private void Start()
    {
        PlayMainMusic();
    }

    // 播放主音乐
    public void PlayMainMusic()
    {
        musicAudioSource.Play();
    }


    // 播放音效
    public void PlaySound_Buy()
    {
        AudioClip soundClip = Resources.Load<AudioClip>("Audio/Buy");
        musicAudioSource.PlayOneShot(soundClip);
    }
    public void PlaySound_Touch()
    {
        AudioClip soundClip = Resources.Load<AudioClip>("Audio/Touch");
        musicAudioSource.PlayOneShot(soundClip);
    }
    public void PlaySound_Grow()
    {
        AudioClip soundClip = Resources.Load<AudioClip>("Audio/Grow");
        musicAudioSource.PlayOneShot(soundClip);
    }
    public void PlaySound_Sell()
    {
        AudioClip soundClip = Resources.Load<AudioClip>("Audio/Sell");
        musicAudioSource.PlayOneShot(soundClip);
    }
    public void PlaySound_Button()
    {
        AudioClip soundClip = Resources.Load<AudioClip>("Audio/Button");
        musicAudioSource.PlayOneShot(soundClip);
    }
    public void PlaySound_Fail()
    {
        AudioClip soundClip = Resources.Load<AudioClip>("Audio/Fail");
        musicAudioSource.PlayOneShot(soundClip);
    }
    public void PlaySound_Clear()
    {
        AudioClip soundClip = Resources.Load<AudioClip>("Audio/Clear");
        musicAudioSource.PlayOneShot(soundClip);
    }
}
