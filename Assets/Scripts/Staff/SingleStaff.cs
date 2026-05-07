using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleStaff
{
    public List<Spell> SpellList;
    public int cooldownTimerinMs = 0;

    public void CastSpells(MultiStaffObject staff, Vector3 targetPosition, Quaternion targetRotation)
    {
        if (cooldownTimerinMs > 0)
        {
            return;
        }

        foreach (Spell spell in SpellList)
        {
            cooldownTimerinMs = spell.recoveryTimeInMs;
            spell.CastSpell(staff, targetPosition, targetRotation);
        }
    }
}
