using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Meat : ItemData
{
    public Item_Meat() : base(
        "i0050", 
        "Usable", 
        "������ ���", 
        "�������� ������ ����̴�.HP�� �� 100 ȸ����Ų��.", 
        "10", "5", true)
    {

    }

    public override void UseItem(object param)
    {
        base.UseItem(param);
        Debug.Log("HP�� ȸ���˴ϴ�.");
    }
}
