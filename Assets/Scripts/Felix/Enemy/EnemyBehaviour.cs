using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : LiveEntity
{
    //reference
    PathFinding pathfind;
    public GameObject atkEffect;
    Animator atkAnim;

    public float atkRange;
    public float atkCooldown;
    float atkTimer;
    bool canMove = true; //to prevent enemy to move while attacking


    // Start is called before the first frame update
    void Start()
    {
        pathfind = GetComponent<PathFinding>();
        atkTimer = atkCooldown;
        atkAnim = atkEffect.GetComponent<Animator>();
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
        //stand still while executing attack
        canMove = false;

        //reset cooldown
        atkTimer = atkCooldown;

        //trigger attack animation
        atkEffect.SetActive(true);
        StartCoroutine(disableAtkEffect(.5f));

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
        if (canMove)
        {
            base.Move(direction);
            Face(direction);
        }
    }

    IEnumerator disableAtkEffect(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        atkEffect.SetActive(false);
        canMove = true;
    }
}
