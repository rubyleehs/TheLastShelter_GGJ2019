using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{
    private static GameObject bgmInstance;
    public AudioClip gameplayBGM;
    public AudioClip menuBGM;
    private void Awake()
    {
        DontDestroyOnLoad(this);
        
        if (!bgmInstance)
            bgmInstance = gameObject;
        else
            Destroy(gameObject);
    }

}
