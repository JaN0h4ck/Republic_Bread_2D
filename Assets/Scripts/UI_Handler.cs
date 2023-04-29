using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UI_Handler : MonoBehaviour
{
    private UI_Input inputActions;
    private InputAction toggleInventory;

    private GameObject inventorySystem;

    private bool isInventoryOpen;
    // Start is called before the first frame update
    void Awake()
    {
        inputActions = new UI_Input();
        inventorySystem = GetComponentInChildren<DisplayInventory>().transform.gameObject;
        inventorySystem.SetActive(false);
        isInventoryOpen = false;
    }

    void OnEnable() {
        toggleInventory = inputActions.UI.ToggleInventory;
        toggleInventory.performed += ToggleInventory;
        toggleInventory.Enable();
    }

    private void ToggleInventory(InputAction.CallbackContext obj) {
        if(isInventoryOpen) {
            inventorySystem.SetActive(false);
            isInventoryOpen = false;
        } else {
            inventorySystem.SetActive(true);
            isInventoryOpen = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
