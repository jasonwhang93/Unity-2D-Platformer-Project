using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_ArrowForBow : ItemData
{
    public Item_ArrowForBow() : base(
    "i0057",
    "SetUp",
    "Ȱ���� ȭ��",
    "ȭ���� ����ִ� ȭ�����̴�. �ݵ�� Ȱ�� �Բ� ����ؾ� �Ѵ�.",
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
