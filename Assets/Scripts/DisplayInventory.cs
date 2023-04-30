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

    //public int X_SPACE_BETWEEN_ITEMS = 75;
    //public int X_START = 75;
    //public int Y_START = -75;
    //public int NUMBER_OF_COLUMNS = 3;
    //public int Y_SPACE_BETWEEN_ITEMS = 75;
    Dictionary<GameObject, InventorySlot> itemsDisplayed = new Dictionary<GameObject, InventorySlot>();

    void Start() {
        CreateSlots();
    }

    void Update() {
        UpdateSlots();
    }

    public void UpdateSlots() {
        foreach (KeyValuePair<GameObject, InventorySlot> _slot in itemsDisplayed) {
            if(_slot.Value.ID >= 0) {
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventory.database.GetItem(_slot.Value.Item.id).uiDisplay;
            } else {

            }
        }
    }

    public void CreateSlots() {
        itemsDisplayed = new Dictionary<GameObject, InventorySlot>();
        for (int i = 0; i < inventory.Container.Items.Length; i++) {
            var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);
            // Position is done by grid Layout

            itemsDisplayed.Add(obj, inventory.Container.Items[i]);
        }
    }

    //private Vector3 GetPosition(int i) {
    //    return new Vector3(X_START + (X_SPACE_BETWEEN_ITEMS * (i % NUMBER_OF_COLUMNS)), Y_START + (-Y_SPACE_BETWEEN_ITEMS * (i / NUMBER_OF_COLUMNS)), 0f);
    //}

}
