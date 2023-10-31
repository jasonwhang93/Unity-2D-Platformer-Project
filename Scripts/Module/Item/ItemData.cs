using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : ItemBase
{
    public ItemData(string code, string type, string name, string descript, string purchasePrice, string sellPrice, bool isStackable)
    {
        itemCode = code;
        itemStringType = type;
        itemName = name;
        itemDescription = descript;
        itemPurchasePrice = purchasePrice;
        itemSellPrice = sellPrice;
        itemIsStackable = isStackable;

        itemImage = Resources.Load<Sprite>($"Sprite/Item/Useable/{code}");
        itemEnumType = SetItemType(type);
    }

    public override void UseItem(object param)
    {
        // ������ ��뿡 �ʿ��� ������ ����
        Debug.Log($"{itemName}��(��) ����մϴ�.");
    }
}
