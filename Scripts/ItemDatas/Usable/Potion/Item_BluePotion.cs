using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_BluePotion : ItemData
{
    public Item_BluePotion() : base(
        "i0008", 
        "Usable", 
        "�Ķ� ����", 
        "Ǫ�� ���ʷ� ���� �����̴�. MP�� �� 100 ȸ����Ų��.", 
        "20", "10", true) 
    {

    }

    public override void UseItem(object param)
    {
        base.UseItem(param);
        Debug.Log("MP�� ȸ���˴ϴ�.");
    }
}
