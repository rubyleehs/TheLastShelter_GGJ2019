using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCLogic : LiveEntity
{
    public GameManager gm;

    [Header("Wander Logic")]
    public Vector2 homePos;
    public Vector2 wanderRadius;
    public Vector2 stepDistance;
    public float wanderCheckIntervalDuration;
    public Transform wanderFollow;

    [Range(0,1)]
    public float wanderChance;

    private Vector2 targetPos;
    private Vector2 delta;
    private float timeSinceLastWanderCheck;

    protected override void Awake()
    {
        base.Awake();
        FindRandomTargetPos();
    }

    private void Update()
    {
        SmartMove();
    }

    private void SmartMove()
    {
        delta = targetPos - (Vector2)transform.position ;
        timeSinceLastWanderCheck += GameManager.deltaTime;
        if (delta.sqrMagnitude < 2f && timeSinceLastWanderCheck > wanderCheckIntervalDuration) 
        {
            timeSinceLastWanderCheck = 0;
            if (Random.Range(0f, 1f) <= wanderChance) FindRandomTargetPos();
        }

        animator.SetBool("isMoving", rb.velocity.sqrMagnitude > 0.2f);
        animator.SetInteger("YFaceDir", (int)(cardinalLookAngle / 90) % 2);

        if (delta.sqrMagnitude > 2f) Move(delta);
        Face(delta,true);


    }

    private void FindRandomTargetPos()
    {
        if (wanderFollow == null) wanderFollow = transform;
        bool isValidTargetPos = false;
        float d = 0;
        while (!isValidTargetPos)
        {
            targetPos = (Vector2)wanderFollow.position + Random.insideUnitCircle * Random.Range(stepDistance.x, stepDistance.y);
            d = (homePos - targetPos).magnitude;
            if (d > wanderRadius.x && d < wanderRadius.y) isValidTargetPos = true;
        }
    }
    
    public override void Attack()
    {
        throw new System.NotImplementedException();
    }

    public override void Die()
    {
        gm.loseGame();
    }

}
