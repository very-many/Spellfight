using UnityEngine;

public class TomeOfBalance : Upgrade
{
    public string upgradeImagePath => "Upgrades/Tome_Of_Balance";

    public string upgradeTitle => "Tome Of Balance";

    public string upgradeDescription => "Increases both health and recovery by " + _increase + "%";

    public int probabilityWeight => 10;

    private float _increase = 8f + Random.Range(1, 5);

    public void ApplyUpgrade(PlayerMainCoordinator stats)
    {
        ApplyBalance(stats);
    }

    public void ReApplyUpgradeStats(PlayerMainCoordinator stats)
    {
        ApplyBalance(stats);
    }

    private void ApplyBalance(PlayerMainCoordinator stats)
    {
        float newHealthMult = stats.GetHealthModifier() * (1 + (_increase / 100));
        stats.SetHealthModifier(newHealthMult);
        float newRecoveryMult = stats.GetRecoveryModifier() * (1 + (_increase / 100));
        stats.SetRecoveryModifier(newRecoveryMult);
    }
}
