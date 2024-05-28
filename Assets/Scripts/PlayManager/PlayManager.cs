using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayManager : MonoBehaviour
{
    public static PlayManager instance;

    private void Awake()
    {
        instance = this;
        if(instance == null )
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(gameObject);

#if UNITY_ANDROID
        Application.targetFrameRate = 60;
#endif
    }

}
