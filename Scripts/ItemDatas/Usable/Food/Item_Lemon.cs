using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Lemon : ItemData
{
    public Item_Lemon() : base(
        "i0046", 
        "Usable", 
        "����", 
        "�ſ� �� �����̴�.MP�� �� 150 ȸ����Ų��.", 
        "93", "45", true)
    {

    }

    public override void UseItem(object param)
    {
        base.UseItem(param);
        Debug.Log("MP�� ȸ���˴ϴ�.");
    }
}
