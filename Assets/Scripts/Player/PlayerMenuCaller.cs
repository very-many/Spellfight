using Mirror;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMenuCaller : NetworkBehaviour
{
    public UpgradeUI upgradeUI;

    public StaffDragAndDrop staffManager;

    public PlayerUI playerUI;

    public PlayerMainCoordinator coordinator;

    private void Start()
    {
        if (!isOwned) return;
        coordinator = GetComponent<PlayerMainCoordinator>();

        WireExternalUi();

        this.playerUI = GetComponent<PlayerUI>();
        playerUI.StartUI();
    }

    public void OnChooseUpgrade(InputAction.CallbackContext context)
    {
        if (!context.performed || !isOwned) return;
        if (upgradeUI == null) return;
        playerUI.StopUI();
        upgradeUI.OpenUI(this);
    }

    public void OpenDragAndDrop()
    {
        if (staffManager == null) return;
        staffManager.OpenUI(this);
    }

    public void CloseDragAndDrop()
    {
        if (staffManager == null || upgradeUI == null) return;
        staffManager.CloseUI();
        playerUI.StartUI();
    }

    public void WireExternalUi()
    {
        this.upgradeUI = GameObject.FindGameObjectWithTag("UpgradePicker")?.GetComponent<UpgradeUI>();
        this.staffManager = GameObject.FindGameObjectWithTag("StaffInventory")?.GetComponent<StaffDragAndDrop>();

        Debug.Log("Found Upgrade UI: " + (upgradeUI != null).ToString());
        Debug.Log("Found Staff Manager: " + (staffManager != null).ToString());
        if (upgradeUI == null || staffManager == null || coordinator == null) return;

        upgradeUI.playerMainCoordinator = coordinator;
        staffManager.playerMainCoordinator = coordinator;
        staffManager.staffMulti = coordinator.GetMultiStaffObject();
    }
}
