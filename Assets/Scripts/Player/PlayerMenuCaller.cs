using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMenuCaller : MonoBehaviour
{
    public UpgradeUI UpgradeUI;

    public StaffDragAndDrop StaffManager;

    public PlayerMainCoordinator coordinator;

    private void Awake()
    {
        coordinator = GetComponent<PlayerMainCoordinator>();

        UpgradeUI.playerMainCoordinator = coordinator;

        StaffManager.playerMainCoordinator = coordinator;

        StaffManager.staffMulti = coordinator.GetMultiStaffObject();
    }

    public void OnChooseUpgrade(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        UpgradeUI.OpenUI(this);
    }

    public void OpenDragAndDrop()
    {
        StaffManager.OpenUI(this);
    }
}
