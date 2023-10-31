using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LithHarborShop_UsableItem : MonoBehaviour, INPC
{
    [HideInInspector] public List<string> shopItemCodeList; // ���� �ʱ�ȭ ����
    public bool isSpriteDirLeft = true;

    public bool IsSpriteDirLeft { get { return isSpriteDirLeft; } }
    public List<string> ShopItemCodeList { get { return shopItemCodeList; } }

    private void Awake()
    {
        shopItemCodeList = new List<string>(); // ���⿡�� �ʱ�ȭ

        AddCode();
    }

    public void AddCode()
    {
        shopItemCodeList.Add("i0002");
        shopItemCodeList.Add("i0004");
        shopItemCodeList.Add("i0006");
        shopItemCodeList.Add("i0008");
        shopItemCodeList.Add("i0014");
        shopItemCodeList.Add("i0022");
        shopItemCodeList.Add("i0024");
        shopItemCodeList.Add("i0026");
        shopItemCodeList.Add("i0030");
        shopItemCodeList.Add("i0040");
        shopItemCodeList.Add("i0042");
        shopItemCodeList.Add("i0044");
        shopItemCodeList.Add("i0046");
        shopItemCodeList.Add("i0050");
        shopItemCodeList.Add("i0051");
        shopItemCodeList.Add("i0054");
        shopItemCodeList.Add("i0057");
        shopItemCodeList.Add("i0062");
        shopItemCodeList.Add("i0065");
        shopItemCodeList.Add("i0082");
    }

    public void PrintCode()
    {
        Debug.Log("shopItemCodeList Count = " + shopItemCodeList.Count);
    }
}
