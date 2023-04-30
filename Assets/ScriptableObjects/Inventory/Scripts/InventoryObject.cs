using JetBrains.Annotations;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public string savePath;
    public ItemDatabaseObject database;
    public Inventory Container;


    public void AddItem(Item _item, int _amount) {
        for (int i = 0; i < Container.Items.Length; i++) {
            if (Container.Items[i].Item.id == _item.id) {
                Container.Items[i].AddAmount(_amount);
                return;
            }
        }
        SetFirstEmptySlot(_item, _amount);
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