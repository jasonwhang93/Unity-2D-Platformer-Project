using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_DexterityPotion : ItemData
{
    public Item_DexterityPotion() : base(
        "i0022", 
        "Usable", 
        "��ø���� ����", 
        "����� �绡�� ����.3�а� DEX�� 5 �����Ѵ�", 
        "500", "250", true)
    {

    }

    public override void UseItem(object param)
    {
        base.UseItem(param);
        Debug.Log("DEX�� �����մϴ�.");
    }
}
