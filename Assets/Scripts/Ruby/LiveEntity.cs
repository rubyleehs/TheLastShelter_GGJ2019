using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class LiveEntity : MonoBehaviour
{
    public Sprite[] sprites;//0 = face right, 1 = up, 2 = down

    [Header("Stats")]
    public float maxHP;
    public float currentHP;

    public float moveSpeed;    
    public float stationarySpeedBoostRatio; //to overcome inertia so player don't feel "sticky"    

    //Runtime Values
    [HideInInspector]
    protected float lookAngle; //in degrees; 0 is to the right, ANTICLOCKWISE direction is positive, from -180 to 180
    protected float cardinalLookAngle; //rounded to cardinals
    protected Vector2 velocity;

    [Header("Refrences")]
    public new Transform transform;
    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;
    public Transform combatEffectRotator;
    protected Transform combatEffect;
    protected Animator combatEffectAnimator;
    protected Animator animator;

    [Header("Death Animation")]
    public float deathAnimDuration;
    public float deathScaleMultiplier;
    private Vector3 originalScale;
    private Color originalColor;
    private Color endColor;

    protected bool hasDied = false;

    protected virtual void Awake() //if you get an acessibility error, change your awake to be protected instead of public. 
    {
        if(transform == null) transform = GetComponent<Transform>();
        if(rb == null) rb = GetComponent<Rigidbody2D>();
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        if (animator == null) animator = GetComponent<Animator>();
        if (combatEffect == null) combatEffect = combatEffectRotator.GetChild(0).transform; 
        if (combatEffect != null && combatEffectAnimator == null) combatEffectAnimator = combatEffect.GetComponent<Animator>();

        originalScale = transform.localScale;
        originalColor = spriteRenderer.color;
        endColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
    }

    public virtual void Move(Vector2 direction)
    {
        //to simulate inertia/knockback without using actual physics (somehow unity physics for player control never really felt right for me)
        if (velocity.sqrMagnitude <= moveSpeed * moveSpeed)
        {
            if (velocity.sqrMagnitude > 1f) velocity += direction.normalized * moveSpeed * rb.mass * GameManager.deltaTime;
            else velocity += direction.normalized * moveSpeed * rb.mass * GameManager.deltaTime * stationarySpeedBoostRatio;
        }

        velocity -= velocity.normalized * rb.mass * rb.drag * GameManager.deltaTime;
        //don't detect super small movements so it doesnt feel slippery
        if (velocity.sqrMagnitude >= 0.2f)
        {
            rb.velocity = velocity;
        }
        else
        {
            velocity = Vector2.zero;
            rb.velocity = Vector2.zero;
        }
    }

    public virtual void Face(Vector2 direction, bool isCardinal)
    {
        lookAngle = Vector2.SignedAngle(Vector2.right, direction);
        float absLookAngle = Mathf.Abs(lookAngle);

        if(absLookAngle > 45 && absLookAngle < 135)
        {
            //spriteRenderer.flipX = false;
            if (direction.y > 0)
            {
                SetSprite(1); //facing up
                cardinalLookAngle = 90;
            }
            else
            {
                SetSprite(2); //facing down
                cardinalLookAngle = -90;
            }
        }
        else
        {
            SetSprite(0);
            if (direction.x > 0)
            {
                spriteRenderer.flipX = false; //facing right
                cardinalLookAngle = 0;
            }
            else
            {
                spriteRenderer.flipX = true; //facing left
                cardinalLookAngle = 180;
            }
        }

        if (isCardinal) combatEffectRotator.eulerAngles = Vector3.forward * cardinalLookAngle;
        else combatEffectRotator.eulerAngles = Vector3.forward * lookAngle;
    }

    private void SetSprite(int index)
    {
        if (index > sprites.Length - 1) return;
        spriteRenderer.sprite = sprites[index];
    }

    public abstract void Attack();

    public virtual void TakeDamage(float amount)
    {
        currentHP -= amount;

        if (currentHP <= 0) Die();
        if (currentHP > maxHP) currentHP = maxHP;
    }

    public abstract void Die();

    public void Push(float pushAngle, float pushMomentum)//knockback basically
    {
        velocity += new Vector2(Mathf.Cos(pushAngle * Mathf.Deg2Rad), Mathf.Sin(pushAngle * Mathf.Deg2Rad)) * (pushMomentum / rb.mass);
    }

    public virtual IEnumerator DieAnim()
    {
        float t = 0;
        float smoothProgress = 0;
        while(smoothProgress < 1)
        {
            t += GameManager.deltaTime;
            smoothProgress = Mathf.SmoothStep(0, 1, t / deathAnimDuration);
            transform.localScale = Vector3.Lerp(originalScale, originalScale * deathScaleMultiplier, smoothProgress);
            spriteRenderer.color = Color.Lerp(originalColor, endColor, smoothProgress);
            yield return new WaitForFixedUpdate();
        }

        gameObject.SetActive(false);

        transform.localScale = originalScale;
        spriteRenderer.color = originalColor;
    }
}
