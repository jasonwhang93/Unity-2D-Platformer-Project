using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_SubiThrowingStars : ItemData
{
    public Item_SubiThrowingStars() : base(
    "i0065",
    "Equipment",
    "���� ǥâ",
    "REQ LEV : 10 ��ö�� ���� ǥâ�̴�. ���� ���� ��������� ��� ����ߴٸ� �ٽ� �����ؾ� �Ѵ�. ���ݷ� + 15",
    "500","250", false)
    {

    }

    public override void UseItem(object param)
    {
        base.UseItem(param);
        Debug.Log("ǥâ�� ������ ǥâ�� �Һ��մϴ�.");
    }
}
