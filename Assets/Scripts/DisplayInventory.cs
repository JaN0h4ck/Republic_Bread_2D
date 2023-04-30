using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DisplayInventory : MonoBehaviour
{
    public GameObject inventoryPrefab;
    public InventoryObject inventory;

    public int X_SPACE_BETWEEN_ITEMS = 75;
    public int X_START = 75;
    public int Y_START = -75;
    public int NUMBER_OF_COLUMNS = 3;
    public int Y_SPACE_BETWEEN_ITEMS = 75;
    Dictionary<GameObject, InventorySlot> itemsDisplayed = new Dictionary<GameObject, InventorySlot>();

    void Start() {
        CreateSlots();
    }

    void Update() {
        
    }

    public void CreateSlots() {
        itemsDisplayed = new Dictionary<GameObject, InventorySlot>();
    }

    private Vector3 GetPosition(int i) {
        return new Vector3(X_START + (X_SPACE_BETWEEN_ITEMS * (i % NUMBER_OF_COLUMNS)), Y_START + (-Y_SPACE_BETWEEN_ITEMS * (i / NUMBER_OF_COLUMNS)), 0f);
    }

}
