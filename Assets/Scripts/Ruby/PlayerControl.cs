using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : LiveEntity
{
    protected Vector2 velocity;
    protected float frictionCoefficient;
    protected float initialSpeedBoost; //to overcome inertia so player don't feel "sticky"

    public override void Move(Vector2 direction)
    {
        //to simulate momentum without using actual physics
        if (velocity.sqrMagnitude <= moveSpeed * moveSpeed) velocity += direction.normalized * moveSpeed * rb.mass * frictionCoefficient * GameManager.deltaTime;
        velocity -= velocity.normalized * rb.mass * frictionCoefficient * GameManager.deltaTime;

        //don't detect super small movements so it doesnt feel slippery
        if (velocity.sqrMagnitude >= 0.5f)
        {
            rb.velocity = velocity;
        }



    }
    public override void Attack()
    {
        throw new System.NotImplementedException();
    }

    public override void Die()
    {
        throw new System.NotImplementedException();
    }


}
