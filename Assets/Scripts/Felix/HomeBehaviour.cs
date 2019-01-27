using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeBehaviour : LiveEntity
{ 
    public GameManager gm;

    PlayerSound homeSFX;
    Animator houseAnim;

    void Start()
    {
        homeSFX = GetComponent<PlayerSound>();
        houseAnim = GetComponent<Animator>();
    }

    public override void Attack() { }

    public override void Die()
    {
        gm.loseGame();
    }

    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);
        //play damage animation
        houseAnim.SetTrigger("dmg");

        //randoming which house damage audioclip to play
        int SFXToPlay = Random.Range(0, 1);
        switch(SFXToPlay)
        {
            case 0:
                //swap clip tp play
                AudioClip temp = homeSFX.audio.clip;
                homeSFX.audio.clip = homeSFX.negativeSFX;
                homeSFX.negativeSFX = temp;
                homeSFX.audio.Play();
                break;
            case 1:
                //directly play without swapping
                homeSFX.audio.Play();
                break;
        }
    }
}
