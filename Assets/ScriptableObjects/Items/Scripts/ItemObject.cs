using UnityEngine;

public enum ItemType {
    Food,
    Equipment,
    Craftable,
    Default
}
public abstract class ItemObject : ScriptableObject
{
    public int ID;
    public Sprite uiDisplay;
    public ItemType type;
    [TextArea(15, 20)]
    public string description;
}

[System.Serializable]
public class Item {
    public string name;
    public int id;
    public Item(ItemObject _item) {
        name = _item.name;
        id = _item.ID;
    }
}