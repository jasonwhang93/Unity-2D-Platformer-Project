using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_ManaElixir : ItemData
{
    public Item_ManaElixir() : base(
        "i0014",
        "Cash", 
        "���� �ٸ���", 
        "������ ����̴�. MP�� �� 300 ȸ����Ų��.", 
        "186", "93", true)
    {
        
    }

    public override void UseItem(object param)
    {
        base.UseItem(param);
        Debug.Log("MP�� ȸ���˴ϴ�.");
    }
}
