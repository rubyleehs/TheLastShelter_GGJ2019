using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    [HideInInspector] public AudioSource audio;
    public AudioClip attackSFX;
    public AudioClip negativeSFX; //get hit by enemies
    public AudioClip dieSFX;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

}
