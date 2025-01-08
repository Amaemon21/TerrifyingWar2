using NaughtyAttributes;
using UnityEngine;

public abstract class InventoryItemConfig : ScriptableObject
{
    [field: SerializeField, BoxGroup("Global item config"), HorizontalLine] public RarityType ItemRarity { get; private set;}
    [field: SerializeField, BoxGroup("Global item config")] public ItemType ItemType { get; private set;}
    [field: SerializeField, BoxGroup("Global item config")] public string ItemID { get; private set;}
    [field: SerializeField, BoxGroup("Global item config")] public string ItemName { get; private set;}
    [field: SerializeField, BoxGroup("Global item config")] public string ItemDescription { get; private set;}
    [field: SerializeField, BoxGroup("Global item config")] public bool IsStackable { get; private set;}
    [field: SerializeField, BoxGroup("Global item config"), Min(0)] public int ItemCount { get; private set;}
    [field: SerializeField, BoxGroup("Global item config"), ShowAssetPreview] public Sprite ItemSprite { get; private set;}
    [field: SerializeField, BoxGroup("Global item config"), ShowAssetPreview] public InventoryItemObject ItemPrefab { get; private set;}

    public void AddCount(int value)
    {
        if (!IsStackable)
        {
            ItemCount = 1;
        }
        else
        {
            if (value >= 0)
            {
                ItemCount += value;
            }
        }
    }

    public void RemoveCount(int value)
    {
        if (!IsStackable)
        {
            ItemCount = 0;
        }
        else
        {
            if (value >= 0)
            {
                ItemCount -= value;
            }
            
            if (ItemCount < 0)
            {
                ItemCount = 0;
            }
        }
    }

    public void ResetCount()
    {
        ItemCount = 0;
    }

    public void Setup(InventoryItemObject itemPrefab)
    {
        ItemPrefab = itemPrefab;
        ItemName = itemPrefab.name;
    }
}