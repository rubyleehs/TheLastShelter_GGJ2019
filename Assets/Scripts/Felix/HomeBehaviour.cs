using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeBehaviour : LiveEntity
{
    public GameObject[] alliesObj;
    public static float alliesSpawnCooldown;
    public static float alliesDespawnCooldown;
    float timer; //used to countdown for spawn and despawn

    public GameManager gm;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public override void Attack() { }

    public override void Die()
    {
        gm.loseGame();
    }
}
