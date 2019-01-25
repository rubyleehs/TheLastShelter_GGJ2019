using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Combat Art")]
public abstract class CombatArt : ScriptableObject
{
    public HitboxInfo[] hitboxInfo;

    public float damage;

    public float momentumInccuredAngle;
    public float momentumInccuredMagnitude;

    public float momentumCausedAngle;
    public float momentumCausedMagnitude;
}
