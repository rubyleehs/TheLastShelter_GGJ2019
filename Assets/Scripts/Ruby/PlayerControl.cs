using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct InputScheme
{
    public KeyCode upKey;
    public KeyCode downKey;
    public KeyCode leftKey;
    public KeyCode rightKey;

    public KeyCode attackKey;
}

public class PlayerControl : LiveEntity
{
    public InputScheme inputScheme; //because I never worked with changing unity input system for local multi. going to safe,longer way. change if you want.
    protected Vector2 inputAxis;

    protected Vector2 velocity;
    public float stationarySpeedBoostRatio; //to overcome inertia so player don't feel "sticky"

    protected bool allowInput = true;

    private void Update()
    {
        HandlePlayerInput();
    }

    public override void Move(Vector2 direction)
    {
        //to simulate inertia/knockback without using actual physics (somehow unity physics for player control never really felt right for me)
        if (velocity.sqrMagnitude <= moveSpeed * moveSpeed)
        {
            if (velocity.sqrMagnitude > 1f) velocity += direction.normalized * moveSpeed * rb.mass * GameManager.deltaTime;
            else velocity += direction.normalized * moveSpeed * rb.mass * GameManager.deltaTime * stationarySpeedBoostRatio;
        }

        velocity -= velocity.normalized * rb.mass * rb.drag * GameManager.deltaTime;
        //don't detect super small movements so it doesnt feel slippery
        if (velocity.sqrMagnitude >= 0.5f)
        {
            rb.velocity = velocity;
        }
        else
        {
            velocity = Vector2.zero;
            rb.velocity = Vector2.zero;
        }
    }

    protected virtual void HandlePlayerInput()
    {
        //Movement related Inputs
        if (allowInput)
        {
            //key inputs to axial input
            inputAxis = Vector2.zero;
            if (Input.GetKey(inputScheme.upKey)) inputAxis += Vector2.up;
            if (Input.GetKey(inputScheme.downKey)) inputAxis += Vector2.down;
            if (Input.GetKey(inputScheme.leftKey)) inputAxis += Vector2.left;
            if (Input.GetKey(inputScheme.rightKey)) inputAxis += Vector2.right;
            inputAxis = inputAxis.normalized;
        }

        Move(inputAxis);
        if (inputAxis.sqrMagnitude > 0.1f) Face(inputAxis);


        //Attack related inputs
        if (allowInput)
        {
            if (Input.GetKey(inputScheme.attackKey)) Attack();
        }
    }

    public override void Attack()
    {
        throw new System.NotImplementedException();
    }

    public override void Die()
    {
        //maybe respawn or something after set time.
        throw new System.NotImplementedException();
    }


}
