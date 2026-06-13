using UnityEngine;

public class Jump : Spell
{
    public string spellTitle => "Jump";

    public float spellRecoveryTime => 1.5f;

    public float spellCastTime => 0.1f;

    public string spellImagePath => "Spells/Jump";

    public int probabilityWeight => 5;

    public void CastSpell(MultiStaffObject multiStaff, SingleStaff singleStaff)
    {
        multiStaff.spellcasting.Jump();
    }
}
