
using UnityEngine;

public interface Spell
{
    string spellTitle { get; }
    float spellRecoveryTime { get; }
    float spellCastTime { get; }
    string spellImagePath { get; }
    int probabilityWeight { get; }

    void CastSpell(MultiStaffObject multiStaff, SingleStaff singleStaff);
}
