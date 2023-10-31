using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopContent : MonoBehaviour
{
    // ������ �̹���, �̸�, �ǸŰ��ݿ� ���� public ����
    public Image itemImage;
    public Text itemNameText;
    public Text itemPriceText;
    public Image itemCointImage;
    public Text itemCountText;

    // ������ ������
    private ItemData itemData;

    public Image cellSprite1;
    public Image cellSprite2;

    // ���� ���õ� ���� �����ۿ� ���� ����
    [HideInInspector] public static ShopContent currentSelectedShopContent = null;
    private Color defaultColor = Color.white;
    private Color disabledColor = new Color(0, 240 / 255f, 255 / 255f, 1);

    private float lastClickTime = 0f;
    private const float DOUBLE_CLICK_DELAY = 0.25f;

    [HideInInspector] public bool isSellingItem = false;

    public ItemData ItemData
    {
        get { return itemData; }
    }

    // ��ư Ŭ�� �� ȣ��� �޼���
    public void OnButtonClicked()
    {
        // ������ ���õ� ���� �������� �ְ� ���� ���� �����۰� �ٸ� ���
        if (currentSelectedShopContent != null && currentSelectedShopContent != this)
        {
            currentSelectedShopContent.cellSprite1.color = defaultColor;
            currentSelectedShopContent.cellSprite2.color = defaultColor;
        }

        // ���� ���� �������� ���õ� ���� ���������� ����
        currentSelectedShopContent = this;

        cellSprite1.color = disabledColor;
        cellSprite2.color = disabledColor;

        if ((Time.time - lastClickTime) < DOUBLE_CLICK_DELAY)
        {
            EventManager.instance.SendEvent("Shop CountNoticeBox :: UIToggle");

            string eventName = "";
            if (isSellingItem) // �Ǹ��� ���
            {
                eventName = itemData.itemIsStackable ? "OpenSellCountNoticeBox" : "OpenSellNoticeBox";

            }
            else // ������ ���
            {
                eventName = itemData.itemIsStackable ? "OpenBuyCountNoticeBox" : "OpenBuyNoticeBox";
            }
            EventManager.instance.SendEvent(eventName);
        }
        lastClickTime = Time.time;
    }

    // ������ ���� ���� �޼���
    public void SetItemData(ItemData data, bool isSelling)
    {
        itemData = data;
        isSellingItem = isSelling;

        bool isItemNull = itemData is Item_Null;  // ���� ItemData ��ü�� Item_Null Ÿ������ Ȯ���մϴ�.

        // �̹��� ����
        if (itemImage != null)
        {
            itemImage.sprite = itemData.itemImage;
            itemImage.enabled = !isItemNull;
        }

        // ������ �̸� ����
        if (itemNameText != null)
        {
            itemNameText.text = itemData.itemName;
            itemNameText.enabled = !isItemNull; // Item_Null�̸� �̸� �ؽ�Ʈ�� ����ϴ�.
        }

        // ������ ���� ���� ����
        if (itemPriceText != null && !isSelling)
        {
            itemPriceText.text = itemData.itemPurchasePrice.ToString();
            itemPriceText.enabled = !isItemNull; // Item_Null�̸� ���� �ؽ�Ʈ�� ����ϴ�.
        }

        if(itemPriceText != null && isSelling)
        {
            itemPriceText.text = itemData.itemSellPrice.ToString();
            itemPriceText.enabled = !isItemNull;
        }

        // ���� ������ ���� ������ �̹��� ����
        if (itemCointImage != null)
        {
            itemCointImage.enabled = !isItemNull;
        }
    }

    // ������ ���� ���� �޼���
    // ������ ���� ���� �޼���
    public void SetItemCount(InventorySlot slot, bool isSelling)
    {
        if (itemCountText != null && slot != null)
        {
            if (slot.data.itemIsStackable && !isSelling) // isStackable�� ��ø ������ ���������� Ȯ���ϴ� �Ӽ��̶�� �����մϴ�.
            {
                itemCountText.text = slot.itemCount.ToString();
                itemCountText.enabled = true;
            }
            else
            {
                itemCountText.text = "";
                itemCountText.enabled = false;
            }
        }
    }

    public void ClearItem()
    {
        // ������ ���� �� �̹��� ���� �ʱ�ȭ
        itemData = null;
        if (itemImage != null) itemImage.sprite = null;
        if (itemNameText != null) itemNameText.text = "";
        if (itemPriceText != null) itemPriceText.text = "";
    }

}
