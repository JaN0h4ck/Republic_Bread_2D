using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Database", menuName = "Inventory System/Items/Database")]
public class ItemDatabaseObject : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField]
    private ItemObject[] items;
    private Dictionary<ItemObject, int> ItemDatbase = new();


    public int GetID(ItemObject _item) { 
        return ItemDatbase[_item];
    }

    public ItemObject GetItem(int _id) {
        foreach (var id in ItemDatbase) {
            if (id.Value == _id)
                return id.Key;
        }
        return null;
    }

    public void OnAfterDeserialize() {
        ItemDatbase = new Dictionary<ItemObject, int>();
        for (int i = 0; i < items.Length; i++) {
            ItemDatbase.Add(items[i], i);
        }
    }

    public void OnBeforeSerialize() {
    }
}
