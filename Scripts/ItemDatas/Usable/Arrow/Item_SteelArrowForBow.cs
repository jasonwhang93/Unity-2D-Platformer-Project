using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_SteelArrowForBow : ItemData
{
    public Item_SteelArrowForBow() : base(
    "i0062",
    "Usable",
    "�������� ȭ��",
    "ȭ���� ����ִ� ȭ�����̴�. �ݵ�� ���ð� �Բ� ����ؾ� �Ѵ�.",
    "1",
    "0",
    true)
    {

    }

    public override void UseItem(object param)
    {
        base.UseItem(param);
        Debug.Log("Ȱ�� ���� ȭ���� �Һ��մϴ�.");
    }
}
