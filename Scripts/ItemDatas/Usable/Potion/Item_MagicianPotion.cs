using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_MagicianPotion : ItemData
{
    public Item_MagicianPotion() : base(
        "i0026", 
        "Usable", 
        "�������� ����", 
        "������ ������ �ش�. 3�а� ������ 5 �����Ѵ�.", 
        "500", "250", true)
    {

    }

    public override void UseItem(object param)
    {
        base.UseItem(param);
        Debug.Log("������ �����մϴ�.");
    }
}
