using UnityEngine;

public class Firebolt : Spell
{
    public int recoveryTimeInMs = 2000;

    public int castTimeInMs = 500;

    int Spell.recoveryTimeInMs => recoveryTimeInMs;

    int Spell.castTimeInMs => castTimeInMs;

    public void CastSpell(MultiStaffObject staff, Vector3 targetPosition, Quaternion targetRotation)
    {
        throw new System.NotImplementedException();
    }
}
