using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    EnemyBehaviour enemyMovement;

    // Start is called before the first frame update
    void Start()
    {
        enemyMovement = GetComponent<EnemyBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void pathfinding()
    {
        GameObject target = null;
        float nearestDistance = 9999;
        //Determine target based on nearest distance
        for (int x = 0; x < GameManager.friendlies.Count; x++)
        {
            //calculate distance between this object with all the possible target
            float distance = Vector2.Distance(transform.position, GameManager.friendlies[x].transform.position);
            if (nearestDistance > distance)
            {
                //set the nearest target and distance threshold
                target = GameManager.friendlies[x].gameObject;
                nearestDistance = distance;
            }
        }
        //calculate the direction to go
        Vector2 direction = (target.transform.position - transform.position).normalized;
        //move this object
        enemyMovement.Move(direction);
    }
}
