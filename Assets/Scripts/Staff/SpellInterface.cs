
using UnityEngine;

public interface Spell
{
    int recoveryTimeInMs { get; }
    int castTimeInMs { get; }

    void CastSpell(MultiStaffObject staff, Vector3 targetPosition, Quaternion targetRotation);
}
