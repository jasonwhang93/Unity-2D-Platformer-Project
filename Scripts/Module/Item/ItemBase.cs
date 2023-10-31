using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    None,
    Equipment,
    Usable,
    Other,
    SetUp,
    Cash
}

public abstract class ItemBase
{
    public string itemCode { get; protected set; }
    public string itemStringType { get; protected set; }
    public string itemName { get; protected set; }
    public string itemDescription { get; protected set; }
    public string itemPurchasePrice { get; protected set; }
    public string itemSellPrice { get; protected set; }
    public Sprite itemImage { get; protected set; }
    public Sprite itemRawImage { get; protected set; }
    public ItemType itemEnumType { get; protected set; }
    public bool itemIsStackable { get; protected set; }

    public abstract void UseItem(object param);


    // ������ Ÿ�� ��
    public ItemType SetItemType(string typeToCompare)
    {
        foreach (ItemType itemType in System.Enum.GetValues(typeof(ItemType)))
        {
            if (string.Equals(typeToCompare, itemType.ToString(), System.StringComparison.OrdinalIgnoreCase))
            {
                return itemType;
            }
        }

        Debug.LogWarning("�ùٸ��� ���� ������ Ÿ�� ���ڿ�: " + typeToCompare);
        return ItemType.None; // �⺻�� �Ǵ� ���� ó�� ��Ŀ� ���� ��ȯ
    }
}
