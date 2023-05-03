using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DisplayInventory : MonoBehaviour {
    private MouseItem mouseItem = new MouseItem();

    public GameObject inventoryPrefab;
    public InventoryObject inventory;

    [SerializeField]
    private TextMeshProUGUI itemDisplayText;

    [SerializeField]
    private CursorTextureObject cursorTextures;

    Dictionary<GameObject, InventorySlot> itemsDisplayed = new Dictionary<GameObject, InventorySlot>();

    void Start() {
        CreateSlots();
    }

    void Update() {
        UpdateSlots();
    }

    private void UpdateSlots() {
        foreach (KeyValuePair<GameObject, InventorySlot> _slot in itemsDisplayed) {
            InventorySlotAdapter adapter = _slot.Key.GetComponent<InventorySlotAdapter>();
            if (_slot.Value.ID >= 0) {
                adapter.SetItemImage(inventory.database.GetItemObject(_slot.Value.Item.id).uiDisplay);
                adapter.SetItemImageColor(new Color(1, 1, 1, 1));
                adapter.SetItemCountText(_slot.Value.amount == 1 ? "" : _slot.Value.amount.ToString("n0"));
            } else {
                adapter.ResetItemImage();
                adapter.SetItemImageColor(new Color(1, 1, 1, 0));
                adapter.SetItemCountText("");
            }
        }
    }

    private void CreateSlots() {
        itemsDisplayed = new Dictionary<GameObject, InventorySlot>();
        for (int i = 0; i < inventory.Container.Items.Length; i++) {
            var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);
            // Position is done by grid Layout

            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
            AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnDragStart(obj); });
            AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj); });
            AddEvent(obj, EventTriggerType.Drag, delegate { OnDragging(obj); });

            itemsDisplayed.Add(obj, inventory.Container.Items[i]);
        }
    }


    #region Input
    private void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action) {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = type;
        entry.callback.AddListener((data) => { action((BaseEventData)data); });
        trigger.triggers.Add(entry);
    }

    private void OnEnter(GameObject obj) {
        Cursor.SetCursor(cursorTextures.HoverCursor, cursorTextures.Hotspot, CursorMode.Auto);
        mouseItem.hoverObject = obj;

        // Prevent trying to access an item that doesn't exist
        if (itemsDisplayed.ContainsKey(obj) && itemsDisplayed[obj].ID >= 0) {
            mouseItem.hoverSlot = itemsDisplayed[obj];
            itemDisplayText.text = itemsDisplayed[obj].Item.name;
            int id = itemsDisplayed[obj].ID;
            if (mouseItem._object != null && id >= 0) {
                ItemObject hoverItem = inventory.database.GetItemObject(id);
                if (hoverItem.isCraftable && mouseItem.item.ID == hoverItem.requiredItem.ID) {
                    obj.GetComponent<InventorySlotAdapter>().SetBackgroundColor(Color.green);
                    mouseItem.canCraft = true;
                } else {
                    obj.GetComponent<InventorySlotAdapter>().SetBackgroundColor(Color.red);
                }
            }
        }
    }
    private void OnExit(GameObject obj) {
        Cursor.SetCursor(cursorTextures.DefaultCursor, cursorTextures.Hotspot, CursorMode.Auto);
        mouseItem.hoverObject = null;
        mouseItem.hoverSlot = null;
        itemDisplayText.text = "";
        mouseItem.canCraft = false;
        obj.GetComponent<InventorySlotAdapter>().resetBackgroundColor();
    }
    private void OnDragStart(GameObject obj) {
        var mouseObject = new GameObject();
        var rt = mouseObject.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(100, 100);
        mouseObject.transform.SetParent(transform.parent);
        if (itemsDisplayed[obj].ID >= 0) {
            var img = mouseObject.AddComponent<Image>();
            img.sprite = inventory.database.GetItemObject(itemsDisplayed[obj].ID).uiDisplay;
            img.raycastTarget = false;
        }

        mouseItem._object = mouseObject;
        mouseItem.item = itemsDisplayed[obj];
    }
    private void OnDragEnd(GameObject obj) {
        if (mouseItem.hoverObject) {
            if (mouseItem.canCraft) {
                inventory.CraftItem(itemsDisplayed[obj], itemsDisplayed[mouseItem.hoverObject]);
            } else {
                inventory.SwapItems(itemsDisplayed[obj], itemsDisplayed[mouseItem.hoverObject]);
            }
        } else {
            //inventory.RemoveItem(itemsDisplayed[obj].Item);
        }
        Destroy(mouseItem._object);
        mouseItem.item = null;
    }
    private void OnDragging(GameObject obj) {
        if (mouseItem._object != null) {
            mouseItem._object.GetComponent<RectTransform>().position = Input.mousePosition;
        }
    }
    #endregion
}

public class MouseItem {
    public GameObject _object;
    public InventorySlot item;
    public InventorySlot hoverSlot;
    public GameObject hoverObject;
    public bool canCraft = false;
}
