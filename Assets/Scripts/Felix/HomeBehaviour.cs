using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeBehaviour : LiveEntity
{ 
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
