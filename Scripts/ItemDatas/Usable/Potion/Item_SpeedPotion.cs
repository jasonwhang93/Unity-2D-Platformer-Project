using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_SpeedPotion : ItemData
{
    public Item_SpeedPotion() : base(
        "i0024", 
        "Usable", 
        "�ӵ������ ����", 
        "������ �̵��� �� �ִ�.3�а� �̵��ӵ��� �����Ѵ�.", 
        "400", "200", true)
    {

    }

    public override void UseItem(object param)
    {
        base.UseItem(param);
        Debug.Log("�̵��ӵ��� �����մϴ�.");
    }
}
