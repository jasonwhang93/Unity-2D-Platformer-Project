using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_ReturnScrollNearestTown : ItemData
{
    public Item_ReturnScrollNearestTown() : base(
        "i0051",
        "Other", 
        "���� ��ȯ �ֹ���", 
        "���� ��ġ���� ���� ����� ������ ��ȯ�� �� �ִ� �ֹ����̴�. �� �� ����ϸ� �������. ��� ���� ��ȭ�������� ������ �� �ִ�.", 
        "400", "200", true)
    {

    }

    public override void UseItem(object param)
    {
        base.UseItem(param);
        Debug.Log("���� ����� ������ ��ȯ�մϴ�.");
    }
}
