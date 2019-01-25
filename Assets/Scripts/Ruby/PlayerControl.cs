using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : LiveEntity
{
    protected Vector2 velocity;
    protected float frictionCoeeficient;

    public override void Move(Vector2 direction)
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


}
