using UnityEngine;
using UnityEngine.InputSystem;

public class UI_Handler : MonoBehaviour {
    private Unified_Input inputActions;

    private GameObject inventorySystem;

    private bool isInventoryOpen;
    // Start is called before the first frame update
    void Awake() {
        inventorySystem = GetComponentInChildren<DisplayInventory>().transform.gameObject;
        inventorySystem.SetActive(false);
        isInventoryOpen = false;
    }

    void Start() {
        inputActions = InputContainer.Instance.inputActions;
        inputActions.UI.ToggleInventory.performed += ToggleInventory;
    }

    private void ToggleInventory(InputAction.CallbackContext obj) {
        if (isInventoryOpen) {
            inventorySystem.SetActive(false);
            isInventoryOpen = false;
            inputActions.Player.Interact.Enable();
            inputActions.Player.Inspect.Enable();
        } else {
            inventorySystem.SetActive(true);
            isInventoryOpen = true;
            inputActions.Player.Interact.Disable();
            inputActions.Player.Inspect.Disable();
        }
    }
}
