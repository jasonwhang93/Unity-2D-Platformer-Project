using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_RedPotion : ItemData
{
    public Item_RedPotion() : base(
        "i0002", 
        "Usable", 
        "���� ����", 
        "���� ���ʷ� ���� �����̴�. HP�� �� 50 ȸ����Ų��.", 
        "5", "3", true) 
    {

    }

    public override void UseItem(object param)
    {
        base.UseItem(param);
        Debug.Log("HP�� ȸ���˴ϴ�.");
    }
}
