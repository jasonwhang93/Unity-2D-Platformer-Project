using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface INPC
{
    bool IsSpriteDirLeft { get; }
    List<string> ShopItemCodeList { get; }
}


public class ShopManager : MonoBehaviour
{
    public GameObject shopUI;

    public Transform contentsTransform; // Contents ������Ʈ�� Transform
    private GameObject shopContentPrefab; // Shop Content Prefab

    public ItemDatabase itemDB;
    private INPC npcScript;

    [HideInInspector] public GameObject shopManagerNPC;
    public Image shopManagerNPCImage;
    private LithHarborShop_UsableItem lithHarborShopUsableItemList;

    public Transform playerContentsTransform; // Contents ������Ʈ�� Transform
    [SerializeField] private Button[] tabButton;
    private int currentTabIndex = 1;

    public InventoryManager playerInventory;

    public Text playerMoney;

    // �������� �����ϰų� �Ǹ��� �� �� �� �� �ȳ����ֱ� ���� �����ڽ�
    public GameObject buyCountNoticeBox;
    public GameObject buyNoticeBox;
    public GameObject SellCountNoticeBox;
    public GameObject sellNoticeBox;

    [HideInInspector] public OpenDialog buyDialog;
    [HideInInspector] public OpenDialog sellDialog;

    [HideInInspector] public bool isInputFieldFocused = false;

    // Start �޼��� ���� �Ʒ� �ڵ带 �߰��մϴ�:
    public Button btnBuyItem; // Inspector���� "Area :: BtnBuyItem" ��ư�� �������ּ���.
    public Button btnSellItem; // Inspector���� "Area :: BtnBuyItem" ��ư�� �������ּ���.

    private void Awake()
    {
        shopContentPrefab = Resources.Load<GameObject>("Prefabs/UI/Shop Content");

        currentTabIndex = 1;
    }

    private void Start()
    {
        // ���� ���� �� ���� UI�� ��Ȱ��ȭ
        CloseShopUI();

        if (btnBuyItem != null)
        {
            btnBuyItem.onClick.AddListener(OnBuyAndSellItemButtonClicked);
        }

        if (btnSellItem != null)
        {
            btnSellItem.onClick.AddListener(OnBuyAndSellItemButtonClicked);
        }

        EventManager.instance.AddEvent("Shop :: UIToggle", (p) =>
        {
            // �κ��丮 â�� Ȱ��ȭ ���¸� ���
            shopUI.SetActive(!shopUI.activeInHierarchy);

            if (shopUI.activeInHierarchy)
            {
                // NPC ���� ���� �� �̹��� �� ��ũ��Ʈ ����
                if (p != null && p is GameObject)
                {
                    shopManagerNPC = p as GameObject;
                    SpriteRenderer npcSprite = shopManagerNPC.GetComponent<SpriteRenderer>();
                    if (npcSprite != null)
                    {
                        shopManagerNPCImage.sprite = npcSprite.sprite;

                        // NPC �̹��� ���� ����
                        npcScript = shopManagerNPC.GetComponent<INPC>();

                        if (npcScript != null)
                        { 
                            // �θ� ��ü�� Area :: ShopNPC UI�� localScale�� ����
                            shopManagerNPCImage.transform.parent.localScale = npcScript.IsSpriteDirLeft
                                ? new Vector3(-1, 1, 1)
                                : new Vector3(1, 1, 1);
                        }
                    }
                }

                OpenShopUI();
                UpdateShopUI();
                tabButton[currentTabIndex].Select();
                UpdatePlayerMoneyUI();
            }
        });

        EventManager.instance.AddEvent("OpenBuyCountNoticeBox", (p) =>
        {
            buyCountNoticeBox.SetActive(true);
            buyCountNoticeBox.GetComponent<OpenDialog>().SetFocusToInputField(); // ��Ŀ�� ����
        });

        EventManager.instance.AddEvent("OpenBuyNoticeBox", (p) =>
        {
            buyNoticeBox.SetActive(true);
        });

        EventManager.instance.AddEvent("OpenSellCountNoticeBox", (p) =>
        {
            SellCountNoticeBox.SetActive(true);
            SellCountNoticeBox.GetComponent<OpenDialog>().SetFocusToInputField(); // ��Ŀ�� ����
        });

        EventManager.instance.AddEvent("OpenSellNoticeBox", (p) =>
        {
            sellNoticeBox.SetActive(true);
        });
    }

    private void Update()
    {
        if (shopUI.activeInHierarchy && !isInputFieldFocused)
        {
            tabButton[currentTabIndex].Select();
        }

    }

    // "������ ���" ��ư Ŭ�� �� ȣ��� �޼���
    public void OnBuyAndSellItemButtonClicked()
    {
        if (ShopContent.currentSelectedShopContent != null)
        {
            ShopContent selectedContent = ShopContent.currentSelectedShopContent;

            EventManager.instance.SendEvent("Shop CountNoticeBox :: UIToggle");

            string eventName = "";
            if (selectedContent.isSellingItem) // �Ǹ��� ���
            {
                eventName = selectedContent.ItemData.itemIsStackable ? "OpenSellCountNoticeBox" : "OpenSellNoticeBox";
            }
            else // ������ ���
            {
                eventName = selectedContent.ItemData.itemIsStackable ? "OpenBuyCountNoticeBox" : "OpenBuyNoticeBox";
            }
            EventManager.instance.SendEvent(eventName);
        }
    }

    public void ClickEquipmentTab()
    {
        currentTabIndex = 0;

        UpdateShopUI();
    }

    public void ClickUseableTab()
    {
        currentTabIndex = 1;

        UpdateShopUI();
    }

    public void ClickOtherTab()
    {
        currentTabIndex = 2;

        UpdateShopUI();
    }

    public void ClickSetUpTab()
    {
        currentTabIndex = 3;

        UpdateShopUI();
    }

    public void ClickCashTab()
    {
        currentTabIndex = 4;

        UpdateShopUI();
    }

    public void OpenShopUI()
    {
        // ������ �߰��� ���� �����۵� ����
        foreach (Transform child in contentsTransform)
        {
            Destroy(child.gameObject);
        }

        // ���� UI�� Ȱ��ȭ
        shopUI.SetActive(true);

        foreach (string itemCode in npcScript.ShopItemCodeList)
        {
            ItemData itemData = itemDB.GetItemByCode(itemCode);

            if (itemData != null)
            {
                // Shop Content Prefab�� �����ϰ� Contents ������Ʈ�� �ڽ����� �߰�
                GameObject shopContentObject = Instantiate(shopContentPrefab, contentsTransform);

                // ShopContent ��ũ��Ʈ ��������
                ShopContent shopContent = shopContentObject.GetComponent<ShopContent>();

                if (shopContent != null)
                {
                    // ������ ���� ���� (���Ÿ� ���� ���̹Ƿ� isSelling�� false�� ����)
                    shopContent.SetItemData(itemData, false);
                }
                else
                {
                    Debug.LogError("ShopContent ��ũ��Ʈ�� ã�� �� �����ϴ�.");
                }
            }
        }
    }

    public void UpdateShopUI()
    {
        ClearAllShopContents();

        foreach (InventorySlot slot in playerInventory.inventory.GetItemsByType((ItemType)(currentTabIndex + 1)))
        {
            ItemData itemData = slot.data;

            // Item_Null�� �ƴ� �����۸� ��Ͽ� �߰�
            if (itemData != null && !(itemData is Item_Null))
            {
                // Shop Content Prefab�� �����ϰ� Contents ������Ʈ�� �ڽ����� �߰�
                GameObject shopContentObject = Instantiate(shopContentPrefab, playerContentsTransform);

                // ShopContent ��ũ��Ʈ ��������
                ShopContent shopContent = shopContentObject.GetComponent<ShopContent>();

                if (shopContent != null)
                {
                    // ������ ���� ���� (�ǸŸ� ���� ���̹Ƿ� isSelling�� true�� ����)
                    shopContent.SetItemData(itemData, true);

                    // ������ ���� ����
                    shopContent.SetItemCount(slot, false); // ���⼭�� �Ǹ� ����� �ƴϹǷ� false�� �����մϴ�.
                }
                else
                {
                    Debug.LogError("ShopContent ��ũ��Ʈ�� ã�� �� �����ϴ�.");
                }
            }
        }
    }

    private void ClearAllShopContents()
    {
        foreach (Transform content in playerContentsTransform)
        {
            Destroy(content.gameObject);
        }
    }

    // ������ UI ������Ʈ�ϴ� �޼���
    public void UpdatePlayerMoneyUI()
    {
        if (playerMoney != null)
        {
            playerMoney.text = string.Format("{0:N0}", PlayerData.playerMoney);
        }
    }

    public void CloseShopUI()
    {
        shopUI.SetActive(false);
    }

    private void OpenCountNoticeBox()
    {
        buyCountNoticeBox.SetActive(true);
    }

    // ������� �����ݰ� �������� ������ ���Ͽ� ���Ű� �������� Ȯ���ϴ� �Լ�
    public bool CanAfford(int price)
    {
        return PlayerData.playerMoney >= price;
    }


    // �������� ������ �� ȣ���� �޼��� �� �߰����� ��� ���� ����
    public void BuyItem()
    {
        if (ShopContent.currentSelectedShopContent != null && ShopContent.currentSelectedShopContent.ItemData != null)
        {
            ItemData itemToBuy = ShopContent.currentSelectedShopContent.ItemData;

            if (itemToBuy.itemIsStackable)
            {
                // ��ø ������ ������ ���� ����
                buyDialog.ToggleInputField(true); // ���� �Է� �ʵ� Ȱ��ȭ
                buyDialog.gameObject.SetActive(true);
            }
            else
            {
                // ��ø �Ұ����� ������ ���� ����
                buyDialog.ToggleInputField(false); // ���� �Է� �ʵ� ��Ȱ��ȭ
                buyDialog.gameObject.SetActive(true);
            }
        }
        else
        {
            Debug.LogError("������ �������� �������ּ���!");
        }
    }

    // �������� �����ϴ� �Լ�
    public void BuyItem(ItemData itemToBuy, int quantity)
    {
        int itemPrice;
        if (int.TryParse(itemToBuy.itemPurchasePrice, out itemPrice))
        {
            int totalCost = itemPrice * quantity;

            // �������� ����� ���
            if (CanAfford(totalCost))
            {
                // �����ݿ��� ������ ������ ����
                PlayerData.playerMoney -= totalCost;

                // �κ��丮�� ������ �߰�
                for (int i = 0; i < quantity; i++)
                {
                    playerInventory.AddItem(itemToBuy);
                }

                // UI ������Ʈ
                UpdatePlayerMoneyUI();
                UpdateShopUI();
            }
            else
            {
                Debug.LogError("�������� �����մϴ�!");
            }
        }
        else
        {
            Debug.LogError("������ ���� ������ �߸��Ǿ����ϴ�!");
        }
    }

    public void SellItem(ItemData itemToSell, int quantity)
    {
        int itemSellPrice;

        // �÷��̾ ���� �ش� �������� �� ���� Ȯ��
        int totalOwned = playerInventory.GetTotalItemCountOfItem(itemToSell);

        if (totalOwned < quantity)
        {
            Debug.LogError("�Ǹ��Ϸ��� ������ ���� �������� �����ϴ�!");
            return; // �Ǹ� ������ ���� �������� ������ ���� �ߴ�
        }

        if (int.TryParse(itemToSell.itemSellPrice, out itemSellPrice))
        {
            int totalEarned = itemSellPrice * quantity;

            // �����ݿ� ������ �Ǹ� ������ �߰�
            PlayerData.playerMoney += totalEarned;

            // �κ��丮���� ������ ����
            for (int i = 0; i < quantity; i++)
            {
                playerInventory.AddItem(itemToSell, -1);
            }

            // UI ������Ʈ
            UpdatePlayerMoneyUI();
            UpdateShopUI();
        }
        else
        {
            Debug.LogError("������ �Ǹ� ���� ������ �߸��Ǿ����ϴ�!");
        }
    }

}
