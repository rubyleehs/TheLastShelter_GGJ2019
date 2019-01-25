using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : LiveEntity
{
    PathFinding pathfind;
    float atkTimer;
    float atkRange;
    float atkCooldown;

    // Start is called before the first frame update
    void Start()
    {
        pathfind = GetComponent<PathFinding>();
        atkTimer = atkCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        if (canAtk())
        {
            print("ATK!");
            Attack();
        }       
    }

    public override void Attack()
    {
        //reset cooldown
        atkTimer = atkCooldown;

        //trigger attack animation

        //cause damage to target

    }

    bool canAtk()
    {      
        //check cooldown
        if (atkTimer > 0) //still in cooldown            
            atkTimer -= GameManager.deltaTime;

        else //finished cooldown            
        {
            //check distance with target        
            if (pathfind.target.gameObject != null)
            {
                float distance = Vector2.Distance(pathfind.target.transform.position, transform.position);
                if (distance < atkRange)
                {
                    return true;
                }
            }            
        }
        return false;     
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
