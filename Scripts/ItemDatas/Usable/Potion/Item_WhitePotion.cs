using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_WhitePotion : ItemData
{
    public Item_WhitePotion() : base(
        "i0006", 
        "Usable", 
        "�Ͼ� ����", 
        "���� ������ ����� �����̴�. HP�� �� 300 ȸ����Ų��.", 
        "96", "48", true)
    {

    }

    public override void UseItem(object param)
    {
        base.UseItem(param);
        Debug.Log("HP�� ȸ���˴ϴ�.");
    }
}
