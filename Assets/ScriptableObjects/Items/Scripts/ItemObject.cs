using UnityEngine;

public enum ItemType {
    Food,
    Equipment,
    Craftable,
    Default
}
public abstract class ItemObject : ScriptableObject
{
    public GameObject prefab;
    public Sprite uiDisplay;
    public ItemType type;
    [TextArea(15, 20)]
    public string description;
}
