using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_OrangePotion : ItemData
{
    public Item_OrangePotion() : base(
        "i0004", 
        "Usable", 
        "��Ȳ ����", 
        "���� ������ ���� �����̴�. HP�� �� 150 ȸ����Ų��.", 
        "48", "24", true)
    {

    }

    public override void UseItem(object param)
    {
        base.UseItem(param);
        Debug.Log("HP�� ȸ���˴ϴ�.");
    }
}
