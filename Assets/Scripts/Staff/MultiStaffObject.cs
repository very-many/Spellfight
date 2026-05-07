using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MultiStaffObject", menuName = "Scriptable Objects/MultiStaffObject")]
public class MultiStaffObject : ScriptableObject
{
    public int MagicPower = 100;
    public float Recovery = 1f;
    public float ProjectileSize = 1f;
    public float ProjectileSpeed = 1f;

    public SingleStaff Staff_A;
    public SingleStaff Staff_B;
    public SingleStaff Staff_C;

    public List<Spell> Spellstorage;
}
