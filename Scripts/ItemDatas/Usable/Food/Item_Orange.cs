using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Orange : ItemData
{
    public Item_Orange() : base(
        "i0044", 
        "Usable", 
        "������", 
        "�ð� ������ �������̴�.MP�� �� 50 ȸ����Ų��.", 
        "10", "5", true)
    {
        
    }

    public override void UseItem(object param)
    {
        base.UseItem(param);
        Debug.Log("MP�� ȸ���˴ϴ�.");
    }
}
