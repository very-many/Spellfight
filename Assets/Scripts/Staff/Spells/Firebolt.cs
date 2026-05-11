using UnityEngine;

public class Firebolt : Spell
{
    public float fireBoltRecoveryTime = 2000;

    public float fireBoltCastTime = 500;

    float Spell.spellRecoveryTime => fireBoltRecoveryTime;  
    float Spell.spellCastTime => fireBoltCastTime;

    public void CastSpell(MultiStaffObject staff, Vector3 targetPosition, Quaternion targetRotation)
    {
        throw new System.NotImplementedException();
    }
}
