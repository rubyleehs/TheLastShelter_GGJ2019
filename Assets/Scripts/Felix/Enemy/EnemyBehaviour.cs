using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : LiveEntity
{   

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public override void Attack()
    {
        throw new System.NotImplementedException();
    }

    public override void Die()
    {
        throw new System.NotImplementedException();
    }

    public override void Move(Vector2 direction)
    {       
        transform.Translate(direction * moveSpeed * GameManager.deltaTime);
    }
}
