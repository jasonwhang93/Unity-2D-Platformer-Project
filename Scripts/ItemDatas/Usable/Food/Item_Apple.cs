using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Apple : ItemData
{
    public Item_Apple() : base(
        "i0040", 
        "Usable", 
        "���", 
        "������ �� ���� ����̴�. HP�� �� 30 ȸ����Ų��.", 
        "3", "1", true)
    {

    }

    public override void UseItem(object param)
    {
        base.UseItem(param);
        Debug.Log("HP�� ȸ���˴ϴ�.");
    }
}
