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

        this.upgradeUI = GameObject.FindGameObjectWithTag("UpgradePicker").GetComponent<UpgradeUI>();
        this.staffManager = GameObject.FindGameObjectWithTag("StaffInventory").GetComponent<StaffDragAndDrop>();
        this.playerUI = GetComponent<PlayerUI>();
        coordinator = GetComponent<PlayerMainCoordinator>();

        upgradeUI.playerMainCoordinator = coordinator;

        staffManager.playerMainCoordinator = coordinator;

        staffManager.staffMulti = coordinator.GetMultiStaffObject();

        playerUI.StartUI();
    }

    public void OnChooseUpgrade(InputAction.CallbackContext context)
    {
        if (!context.performed || !isOwned) return;
        playerUI.StopUI();
        upgradeUI.OpenUI(this);
    }

    public void OpenDragAndDrop()
    {
        staffManager.OpenUI(this);
    }

    public void CloseDragAndDrop()
    {
        staffManager.CloseUI();
        playerUI.StartUI();
    }
}
