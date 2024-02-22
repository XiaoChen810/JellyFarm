using UnityEngine;
using UnityEngine.Events;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance; // ����ʵ��

    public AudioClip mainMusic; // ������

    private AudioSource musicAudioSource; // ���ڲ��������ֵ� AudioSource

    private void Awake()
    {
        // ���õ���ʵ��
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �����ڳ����л�ʱ
        }
        else
        {
            Destroy(gameObject);
        }

        // ��ʼ����ƵԴ
        musicAudioSource = gameObject.GetComponent<AudioSource>();
        musicAudioSource.clip = mainMusic;
        musicAudioSource.loop = true; // ѭ������
    }

    private void Start()
    {
        PlayMainMusic();
    }

    // ����������
    public void PlayMainMusic()
    {
        musicAudioSource.Play();
    }


    // ������Ч
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
