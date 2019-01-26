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
    public int atkDmg;
    float atkTimer = 0;
    float inRangeDelay = 0.3f; //when enemy is in the atk range (to player), delay for 0.2s then only attack
    float delayTimer;          //used to countdown inRangeDelay
    bool canMove = true; //to prevent enemy to move while attacking


    // Start is called before the first frame update
    void Start()
    {
        pathfind = GetComponent<PathFinding>();
        atkAnim = atkEffect.GetComponent<Animator>();
        delayTimer = inRangeDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasDied && canAtk())
        {            
            Attack();
        }       
    }

    public override void Attack()
    {
        //stand still while executing attack
        canMove = false;

        //reset cooldown
        atkTimer = atkCooldown;
        delayTimer = inRangeDelay; //reset delay timer once enemy is not in range to player

        //trigger attack animation
        atkEffect.SetActive(true);
        StartCoroutine(disableAtkEffect(.3f));

        //cause damage to target
        pathfind.target.GetComponent<LiveEntity>().TakeDamage(atkDmg);
    }

    bool canAtk()
    {
        //calculate cooldown
        atkTimer -= GameManager.deltaTime;

        //check distance with target        
        if (pathfind.target.gameObject != null)
        {
            float distance = Vector2.Distance(pathfind.target.transform.position, transform.position);
            if (distance < atkRange * transform.lossyScale.x)
            {
                //if enemy has target in range, dont move, just attack
                canMove = false;
                //countdown for delay attack                
                delayTimer -= GameManager.deltaTime;                
                //check cooldown
                if (atkTimer < 0 && delayTimer < 0) //cooldown finish && delay finish     
                    return true;                
            }
            else
            {
                canMove = true;
                delayTimer = inRangeDelay; //reset delay timer once enemy is not in range to player
            }
                
        }
        return false;     
    }

    public override void Die()
    {
        //trigger animation?
        if (hasDied) return;
        hasDied = true;
        //destroy object (set inactive back to pool)
        StartCoroutine(DieAnim());

        //deduct the totalEnemy in WaveManager
        WaveManager.totalEnemy--;
    }



    public override void Move(Vector2 direction)
    {
        if (!canMove) direction = Vector2.zero;

        base.Move(direction);
        if(direction.sqrMagnitude > 0.1f) Face(direction, false);

    }

    IEnumerator disableAtkEffect(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        atkEffect.SetActive(false);        
    }
}
