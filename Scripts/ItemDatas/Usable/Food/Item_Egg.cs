using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Egg : ItemData
{
    public Item_Egg() : base(
        "i0042", 
        "Usable", 
        "�ް�", 
        "���� ������ ����̴�.HP�� �� 50 ȸ����Ų��.", 
        "5", "2", true)
    {

    }

    public override void UseItem(object param)
    {
        base.UseItem(param);
        Debug.Log("HP�� ȸ���˴ϴ�.");
    }
}
