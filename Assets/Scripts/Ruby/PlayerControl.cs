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
    public GameManager gm; //to end the game when one of the player die

    [Header("Player Control Specifics")]
    public InputScheme inputScheme; 
    protected Vector2 inputAxis;

    [Header("Player Combat and Attack Arts")]
    public LayerMask canHitLayer;
    public LayerMask obstacleLayer;
    public CombatArt[] combatArts;
    protected int attackChainPosition;
    protected int comboCount;

    public float attackChainBreakDuration;
    public float comboChainBreakDuration;
    protected float durationSinceLastAttack = 999;

    [Header("Debugging")]
    public Transform combatArtHitbox;

       
    protected bool allowInput = true;
    protected bool isAttacking = false;

  

    private void Update()
    {
        durationSinceLastAttack += GameManager.deltaTime;

        HandlePlayerInput();
    }


    protected virtual void HandlePlayerInput()
    {
        //Movement related Inputs
        inputAxis = Vector2.zero;
        if (allowInput && !isAttacking)
        {
            //key inputs to axial input
            if (Input.GetKey(inputScheme.upKey)) inputAxis += Vector2.up;
            if (Input.GetKey(inputScheme.downKey)) inputAxis += Vector2.down;
            if (Input.GetKey(inputScheme.leftKey)) inputAxis += Vector2.left;
            if (Input.GetKey(inputScheme.rightKey)) inputAxis += Vector2.right;
            inputAxis = inputAxis.normalized;
        }

        Move(inputAxis);
        if (inputAxis.sqrMagnitude > 0.1f && !isAttacking) Face(inputAxis, true);

        animator.SetBool("isMoving", inputAxis.sqrMagnitude > 0.2f);
        animator.SetInteger("YFaceDir", (int)(cardinalLookAngle/90)%2);

        //Attack related inputs
        if (allowInput)
        {
            if (Input.GetKeyDown(inputScheme.attackKey)) Attack();
        }
    }

    public override void Attack()
    {
        if (isAttacking) return;
        Debug.Log("Attack!");

        if (durationSinceLastAttack > attackChainBreakDuration) attackChainPosition = 0;
        else
        {
            attackChainPosition++;
            attackChainPosition = attackChainPosition % combatArts.Length;
        }

        StartCoroutine(InitiateAttack(combatArts[attackChainPosition]));
    }

    IEnumerator InitiateAttack(CombatArt art)
    {
        isAttacking = true;
        comboCount++;

        combatEffectAnimator.Play(art.animationName);
        DealDamage(art);
        Push(art.momentumInccuredAngle + lookAngle, art.momentumInccuredMagnitude);
        yield return new WaitForSeconds(art.durationOfAttack);

        isAttacking = false;
        durationSinceLastAttack = 0;
    }

    public virtual void DealDamage(CombatArt art)
    {
        List<LiveEntity> entitiesToDamage = new List<LiveEntity>();
        GetLiveEntitiesWithinHitbox(art.hitboxInfo, canHitLayer, ref entitiesToDamage);

        for (int i = 0; i < entitiesToDamage.Count; i++)
        {
            Debug.Log(entitiesToDamage[i].name);
            entitiesToDamage[i].TakeDamage(art.damage);
            entitiesToDamage[i].Push(art.momentumCausedAngle + Vector2.SignedAngle(Vector2.right, entitiesToDamage[i].transform.position - transform.position), art.momentumCausedMagnitude);
        }

    }

    private void GetLiveEntitiesWithinHitbox(HitboxInfo hitbox, int checkMask, ref List<LiveEntity> outList)
    {
        Vector2 hitboxPos = (Vector2)transform.position + new Vector2(hitbox.position.x * transform.lossyScale.x, hitbox.position.y * transform.lossyScale.y).Rotate(cardinalLookAngle);
        Vector2 hitboxSize = new Vector2(hitbox.size.x * transform.lossyScale.x, hitbox.size.y * transform.lossyScale.y);
        Collider2D[] collidersWithinHitbox = Physics2D.OverlapBoxAll(hitboxPos, hitboxSize, cardinalLookAngle, checkMask);
        LiveEntity _liveEntityCheck = null;

        DrawDebugHitbox(hitbox.position, hitbox.size);

        for (int i = 0; i < collidersWithinHitbox.Length; i++)
        {
            _liveEntityCheck = collidersWithinHitbox[i].GetComponent<LiveEntity>();
            if (_liveEntityCheck == null) continue;
            Vector2 delta = collidersWithinHitbox[i].transform.position - transform.position;
            if (!Physics2D.Raycast(transform.position,delta,delta.magnitude,obstacleLayer))
            if (_liveEntityCheck != null && !outList.Contains(_liveEntityCheck))
            {
                outList.Add(_liveEntityCheck);
            }
        }
    }


    public override void Die()
    {
        gm.loseGame();
    }

    private void DrawDebugHitbox(Vector2 relativePos, Vector2 size)
    {
        Debug.Log("HitboxInfo");
        if (combatArtHitbox == null) return;
        combatArtHitbox.transform.localScale = size.Rotate(cardinalLookAngle);
        combatArtHitbox.transform.localPosition = relativePos.Rotate(cardinalLookAngle);
    }


}
