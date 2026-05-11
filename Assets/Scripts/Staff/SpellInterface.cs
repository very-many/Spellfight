
using UnityEngine;

public interface Spell
{
    float spellRecoveryTime { get; }
    float spellCastTime { get; }

    void CastSpell(MultiStaffObject staff, Vector3 targetPosition, Quaternion targetRotation);
}
