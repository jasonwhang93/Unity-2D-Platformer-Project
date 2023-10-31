using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot
{
    // ��ø�� �� �ִ� �ִ� ������ ����
    public const int MAX_STACK_COUNT = 200;

    public ItemData data;
    public int itemCount;

    public InventorySlot(ItemData _data, int _itemCount)
    {
        data = _data;
        itemCount = _itemCount;
    }

    public void Init()
    {
        data = new Item_Null();
        itemCount = 0;
    }

    public void Insert(InventorySlot data)
    {
        if (data == null)
        {
            Debug.LogError("Attempted to insert a null InventorySlot.");
            return;
        }

        this.data = data.data;
        itemCount = data.itemCount;
    }

    // ������ ������ �����ϴ� �޼���
    public void AddItemCount(int amount)
    {
        itemCount += amount;
    }

    public void RemoveItemCount(int amount)
    {
        itemCount = Mathf.Max(itemCount - amount, 0); // �������� ������ 0 �̸��� ���� �ʵ��� �մϴ�.
    }
}
