
using UnityEngine;

public interface Spell
{
    float spellRecoveryTime { get; }
    float spellCastTime { get; }
    string spellImagePath { get; }
    int probabilityWeight { get; }

    void CastSpell(MultiStaffObject multiStaff, SingleStaff singleStaff);
}
