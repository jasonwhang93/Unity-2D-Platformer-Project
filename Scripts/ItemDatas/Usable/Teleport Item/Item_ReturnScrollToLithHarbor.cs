using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_ReturnScrollToLithHarbor : ItemData
{
    public Item_ReturnScrollToLithHarbor() : base(
        "i0054",
        "Usable",
        "���� �ױ� ��ȯ �ֹ���",
        "���� �ױ��� ��ȯ�� �� �ִ� �ֹ����̴�. �� �� ����ϸ� �������.",
        "500","250", true)
    {

    }

    public override void UseItem(object param)
    {
        base.UseItem(param);
        Debug.Log("���� �ױ��� ��ȯ�մϴ�.");
    }
}
