using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SingleStaff : MonoBehaviour
{
    public List<Spell> SpellList;
    public float spellCoolDownTimer = 0;

    public void CastSpells(MultiStaffObject staff, Vector3 targetPosition, Quaternion targetRotation)
    {
        if (spellCoolDownTimer > 0)
        {
            return;
        }

        float cooldownTime = 0;

        foreach (Spell spell in SpellList)
        {
            spell.CastSpell(staff, targetPosition, targetRotation);
            cooldownTime = cooldownTime + spell.spellRecoveryTime;

            if (false) { break; } //TODO if the staff isn't being cast anymore; break
        }
        spellCoolDownTimer = cooldownTime;
    }

    public void Update() { 
        if (spellCoolDownTimer > 0) {
            spellCoolDownTimer -= Time.deltaTime;
        }
    }
}