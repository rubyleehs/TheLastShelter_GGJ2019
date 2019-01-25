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
    [Header("Player Control Specifics")]
    public InputScheme inputScheme; //because I never worked with changing unity input system for local multi. going to safe,longer way. change if you want.
    protected Vector2 inputAxis;

    [Header("Player Combat and Attack Arts")]
    public LayerMask canHitLayer;
    public CombatArt[] combatArts;
    protected int attackChainPosition;
    protected int comboCount;

    public float attackChainBreakDuration;
    public float comboChainBreakDuration;
    protected float durationSinceLastAttack;

    [Header("Debugging")]
    public Transform combatArtHitbox;


    protected bool allowInput = true;
    protected bool isAttacking = false;

    private void Update()
    {
        HandlePlayerInput();
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
        if (isAttacking) return;

        isAttacking = true;


    }

    public virtual void DealDamage(CombatArt art)
    {
        List<LiveEntity> entitiesToDamage = new List<LiveEntity>();
        GetLiveEntitiesWithinHitbox(art.hitboxInfo, canHitLayer, ref entitiesToDamage);

        for (int i = 0; i < entitiesToDamage.Count; i++)
        {
            entitiesToDamage[i].TakeDamage(art.damage);
            //entitiesToDamage[i].Push(art.momentumCausedAngle + Vector2.SignedAngle(Vector2.right, entitiesToDamage[i].transform.position - transform.position), art.momentumCausedMagnitude * weapon.momentumCausedMultiplier);
        }

    }

    private void GetLiveEntitiesWithinHitbox(HitboxInfo[] hitboxes, int checkMask, ref List<LiveEntity> outList)
    {
        for (int i = 0; i < hitboxes.Length; i++)
        {
            GetLiveEntitiesWithinHitbox(hitboxes[i], checkMask, ref outList);
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

            if (_liveEntityCheck != null && !outList.Contains(_liveEntityCheck))
            {
                outList.Add(_liveEntityCheck);
            }
        }
    }


    public override void Die()
    {
        //maybe respawn or something after set time.
        throw new System.NotImplementedException();
    }

    private void DrawDebugHitbox(Vector3 relativePos, Vector2 size)
    {
        if (combatArtHitbox == null) return;
        combatArtHitbox.transform.localScale = size;
        combatArtHitbox.transform.localPosition = relativePos;
    }


}
