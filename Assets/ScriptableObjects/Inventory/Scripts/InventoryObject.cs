using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject, ISerializationCallbackReceiver
{
    public string savePath;
    private ItemDatabaseObject database;
    public List<InventorySlot> Container = new List<InventorySlot>();

    private void OnEnable() {
#if UNITY_EDTOR
        database = (ItemDatabaseObject)AssetDatabase.LoadAssetAtPath("Assets/Resources/Database.asset", typeof(ItemDatabaseObject));
#else
        database = Resources.Load<ItemDatabaseObject>("Database");
#endif
    }

    public void AddItem(ItemObject _item, int _amount) {
        for (int i = 0; i < Container.Count; i++) {
            if (Container[i].Item == _item) {
                Container[i].AddAmount(_amount);
                return;
            }
        }
        Container.Add(new InventorySlot(database.GetID(_item),_item, _amount));
    }

    public void Save() {
        string saveData = JsonUtility.ToJson(this, true);
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream file = File.Create(string.Concat(Application.persistentDataPath, savePath));
        binaryFormatter.Serialize(file,saveData);
        file.Close();
    }

    public void Load() {
        string path = string.Concat(Application.persistentDataPath, savePath);
        if(File.Exists(path)) {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream file = File.Open(path, FileMode.Open);
            JsonUtility.FromJsonOverwrite(binaryFormatter.Deserialize(file).ToString(), this);
            file.Close();
        }
    }

    public void OnAfterDeserialize() {
        for (int i = 0; i < Container.Count; i++) {
            ItemObject item = database.GetItem(Container[i].ID);
            if(item != null) {
                Container[i].Item = item;
            }
        }
    }

    public void OnBeforeSerialize() {
    }
}

[System.Serializable]
public class InventorySlot {
    public int ID;
    public ItemObject Item;
    public int amount;
    public InventorySlot(int _id, ItemObject _item, int _amount) {
        ID = _id;
        Item = _item;
        amount = _amount;
    }

    public void AddAmount(int value) {
        amount += value;
    }
}