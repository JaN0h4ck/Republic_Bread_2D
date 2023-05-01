using JetBrains.Annotations;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;
using static UnityEditor.Progress;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public string savePath;
    public ItemDatabaseObject database;
    public Inventory Container;


    public void AddItem(Item _item, int _amount) {
        for (int i = 0; i < Container.Items.Length; i++) {
            if (Container.Items[i].ID == _item.id) {
                Container.Items[i].AddAmount(_amount);
                return;
            }
        }
        SetFirstEmptySlot(_item, _amount);
    }

    public void SwapItems(InventorySlot item1, InventorySlot item2) {
        InventorySlot temp = new InventorySlot(item2.ID, item2.Item, item2.amount);
        item2.UpdateSlot(item1.ID, item1.Item, item1.amount);
        item1.UpdateSlot(temp.ID, temp.Item, temp.amount);
    }

    public void CraftItem(InventorySlot item1, InventorySlot item2) {
        ItemObject craftedItem = database.GetItemObject(database.GetItemObject(item2.Item.id).craftedInto.ID);
        item2.UpdateSlot(craftedItem.ID, new Item(craftedItem), 1);
        item1.UpdateSlot(-1, null, 0);
    }

    public void RemoveItem(Item _item) {
        for (int i = 0; i < Container.Items.Length; i++) {
            if (Container.Items[i].Item == _item) {
                Container.Items[i].UpdateSlot(-1, null, 0);
            }
        }
    }

    public InventorySlot SetFirstEmptySlot(Item _item, int amount) {
        for (int i = 0;i < Container.Items.Length;i++) {
            if (Container.Items[i].ID <= -1) {
                Container.Items[i].UpdateSlot(_item.id, _item, amount);
                return Container.Items[i];
            }
        }
        //Inventory full
        return null;
    }

    [ContextMenu("Save")]
    public void Save() {
        string saveData = JsonUtility.ToJson(this, true);
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream file = File.Create(string.Concat(Application.persistentDataPath, savePath));
        binaryFormatter.Serialize(file,saveData);
        file.Close();
    }

    [ContextMenu("Load")]
    public void Load() {
        string path = string.Concat(Application.persistentDataPath, savePath);
        if(File.Exists(path)) {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream file = File.Open(path, FileMode.Open);
            JsonUtility.FromJsonOverwrite(binaryFormatter.Deserialize(file).ToString(), this);
            file.Close();
        }
    }

    [ContextMenu("Clear")]
    public void Clear() {
        Container = new Inventory();
    }

}

[System.Serializable]
public class Inventory {
    public InventorySlot[] Items = new InventorySlot[24];
}

[System.Serializable]
public class InventorySlot {
    public int ID;
    public Item Item;
    public int amount;
    public InventorySlot() {
        ID = -1;
        Item = null;
        amount = 0;
    }
    public InventorySlot(int _id, Item _item, int _amount) {
        ID = _id;
        Item = _item;
        amount = _amount;
    }

    public void UpdateSlot(int _id, Item _item, int _amount) {
        ID = _id;
        Item = _item;
        amount = _amount;
    }

    public void AddAmount(int value) {
        amount += value;
    }
}