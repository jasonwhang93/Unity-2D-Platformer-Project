using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Chocolate : ItemData
{
    public Item_Chocolate() : base(
        "i0082", 
        "Usable", 
        "���ݸ�", 
        "������ ���� ���ϰ� ���� ��ũ ���ݸ��̴�. ���ڽ�ƽ�� ��ᰡ �Ǳ⵵ �Ѵٴµ�.. HP�� MP�� ���� 1000�� ȸ������ �ش�.", 
        "2100", "1500", true)
    {

    }

    public override void UseItem(object param)
    {
        base.UseItem(param);
        Debug.Log("HP�� MP�� ȸ���˴ϴ�.");
    }
}
