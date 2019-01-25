using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class LiveEntity : MonoBehaviour
{
    public Sprite[] sprites;//1st 4 sprites is facing right, rotating anticlockwise

    //Stats
    public float maxHP;
    public float currentHP;
    public float moveSpeed;

    //Runtime Values
    public float lookAngle; //in degrees; 0 is to the right, ANTICLOCKWISE direction is positive, from -180 to 180

    //Refrences
    public new Transform transform;
    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;

    public virtual void Awake()
    {
        if(transform == null) transform = GetComponent<Transform>();
        if(rb == null) rb = GetComponent<Rigidbody2D>();
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public abstract void Move(Vector2 direction);

    public virtual void Face(Vector2 direction)
    {
        lookAngle = Vector2.SignedAngle(Vector2.right, direction);
        float absLookAngle = Mathf.Abs(lookAngle);

        if(absLookAngle > 45 && absLookAngle < 135)
        {
            if (direction.y > 0) spriteRenderer.sprite = sprites[1]; //facing up
            else spriteRenderer.sprite = sprites[3]; //facing down
        }
        else
        {
            if (direction.x > 0) spriteRenderer.sprite = sprites[0]; //facing right
            else spriteRenderer.sprite = sprites[2]; //facing left
        }
    }

    public abstract void Attack();

    public virtual void TakeDamage(float amount)
    {
        currentHP -= amount;

        if (currentHP <= 0) Die();
        if (currentHP > maxHP) currentHP = maxHP;
    }

    public abstract void Die();
}
