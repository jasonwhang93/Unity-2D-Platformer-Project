using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_SniperPotion : ItemData
{
    public Item_SniperPotion() : base(
        "i0032", 
        "Usable", 
        "������ ����", 
        "��ø���� �������� �ش�. 5�а� DEX�� 5 �����Ѵ�.", 
        "500", "250", true)
    {

    }

    public override void UseItem(object param)
    {
        base.UseItem(param);
        Debug.Log("DEX�� �����մϴ�.");
    }
}
