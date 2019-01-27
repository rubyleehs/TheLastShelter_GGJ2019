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
    public float fleeSpeed;

    [Range(0,1)]
    public float wanderChance;

    private Vector2 targetPos;
    private Vector2 delta;
    private float timeSinceLastWanderCheck;

    protected override void Awake()
    {
        base.Awake();
        targetPos = transform.position;
        //FindRandomTargetPos(stepDistance.x,stepDistance.y);
    }

    private void Update()
    {
        SmartMove();
    }

    private void SmartMove()
    {
        delta = targetPos - (Vector2)transform.position ;
        timeSinceLastWanderCheck += GameManager.deltaTime;
        if (targetPos == Vector2.zero || timeSinceLastWanderCheck > wanderCheckIntervalDuration)
        {
            timeSinceLastWanderCheck = 0;
            if (Random.Range(0f, 1f) <= wanderChance) FindRandomTargetPos(stepDistance.x,stepDistance.y);
        }

        animator.SetBool("isMoving", rb.velocity.sqrMagnitude > 0.2f);
        animator.SetInteger("YFaceDir", (int)(cardinalLookAngle / 90) % 2);

        if (delta.sqrMagnitude > 2f && targetPos != Vector2.zero) Move(delta);
        else Move(Vector2.zero);

        if (targetPos != Vector2.zero) Face(delta, true);
        else Face(velocity, true);

    }

    private void FindRandomTargetPos(float minRange, float maxRange)
    {
        if (wanderFollow == null) wanderFollow = transform;
        bool isValidTargetPos = false;
        float d = 0;
        int i = 0;
        while (!isValidTargetPos && i < 50)
        {
            targetPos = (Vector2)wanderFollow.position + Random.insideUnitCircle * Random.Range(minRange, maxRange);
            d = (homePos - targetPos).magnitude;
            if (d > wanderRadius.x && d < wanderRadius.y) isValidTargetPos = true;
        }
    }

    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);
        timeSinceLastWanderCheck = 0;
        targetPos = Vector2.zero;

        Push(Random.Range(0,360) , fleeSpeed);
        //FindRandomTargetPos((stepDistance.x+ stepDistance.y)*0.5f,stepDistance.y);
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
