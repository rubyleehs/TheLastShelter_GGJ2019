using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class LiveEntity : MonoBehaviour
{

    //Stats
    public float maxHP;
    public float currentHP;
    public float moveSpeed;

    //Runtime Values
    public float lookAngle;

    //Refrences
    public new Transform transform;
    public Rigidbody2D rb;

    public virtual void Awake()
    {
        transform = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
    }

    public abstract void Move(Vector2 direction);
    public abstract void Attack();

    public virtual void TakeDamage(float amount)
    {
        currentHP -= amount;

        if (currentHP <= 0) Die();
        if (currentHP > maxHP) currentHP = maxHP;
    }

    public abstract void Die();
}
