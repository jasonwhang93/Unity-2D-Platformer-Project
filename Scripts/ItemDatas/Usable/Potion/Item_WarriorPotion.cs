using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_WarriorPotion : ItemData
{
    public Item_WarriorPotion() : base(
        "i0030", 
        "Usable", 
        "������ ����", 
        "���ݷ��� �������� �ش�. 3�а� ���ݷ��� 5 �����Ѵ�.", 
        "500", "250", true)
    {

    }

    public override void UseItem(object param)
    {
        base.UseItem(param);
        Debug.Log("���ݷ��� �����մϴ�.");
    }
}
