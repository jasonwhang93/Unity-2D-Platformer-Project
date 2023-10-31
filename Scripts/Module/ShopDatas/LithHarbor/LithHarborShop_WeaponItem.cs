using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LithHarborShop_WeaponItem : MonoBehaviour, INPC
{
    [HideInInspector] public List<string> shopItemCodeList; // ���� �ʱ�ȭ ����
    public bool isSpriteDirLeft = false;

    public bool IsSpriteDirLeft { get { return isSpriteDirLeft; } }
    public List<string> ShopItemCodeList { get { return shopItemCodeList; } }

    private void Awake()
    {
        shopItemCodeList = new List<string>(); // ���⿡�� �ʱ�ȭ

        AddCode();
    }

    public void AddCode()
    {
        shopItemCodeList.Add("i0065");
    }

    public void PrintCode()
    {
        Debug.Log("shopItemCodeList Count = " + shopItemCodeList.Count);
    }
}
