using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public Inventory inventory; // �κ��丮 ��ü

    [SerializeField] private GameObject inventoryUI; // �κ��丮 UI GameObject

    [SerializeField] private Button[] tabButton; // �� ��ư �迭
    private int tabButtonIndex = 1; // ���� ���õ� �� ��ư �ε���

    [SerializeField] private GameObject inventoryContentPrefab; // �κ��丮 ������ ������ ������
    [SerializeField] private Transform parentForContents; // ������ ������ �θ� Transform

    private List<InventoryContent> inventorySlots = new List<InventoryContent>(); // �κ��丮 ������ �����ϴ� ����Ʈ

    [SerializeField] private int inventorySlotCount = 24; // �κ��丮 ������ ����

    [SerializeField] private Transform followCursor; // ������ ����ٴϴ� Ŀ�� Transform

    [SerializeField] private Text playerMoneyText; // �÷��̾� �������� ǥ���ϴ� UI Text

    private void Awake()
    {
        inventory = new Inventory(); // �κ��丮 �ʱ�ȭ

        CreateInventorySlots(); // �κ��丮 ���� ����
    }

    private void Start()
    {
        AddItemFunc(); // ������ �ʱ� �߰�

        DisplayUseableItems(); // ��� ������ ������ �� ǥ��

        // �κ��丮 UI ��� �̺�Ʈ ������ ���
        EventManager.instance.AddEvent("Inventory :: UIToggle", (p) =>
        {
            // �κ��丮 â�� Ȱ��ȭ ���¸� ���
            inventoryUI.SetActive(!inventoryUI.activeInHierarchy);
            UpdateInventoryUI((ItemType)(tabButtonIndex + 1));
            UpdatePlayerMoneyUI(); // �÷��̾� ������ ������Ʈ
        });
    }

    private void Update()
    {
        if (inventoryUI.activeInHierarchy)
        {
            tabButton[tabButtonIndex].Select(); // Ȱ��ȭ�� �κ��丮 UI���� ���� ���õ� �� ��ư�� ���� ���·� ����
        }
    }

    private void CreateInventorySlots()
    {
        if (inventoryContentPrefab == null || parentForContents == null)
        {
            Debug.LogError("Inventory Content Prefab or Parent Transform is not set in the inspector!");
            return;
        }

        // �κ��丮 ������ �����ϰ� ����Ʈ�� �߰�
        for (int i = 0; i < inventorySlotCount; i++)
        {
            GameObject createdSlot = Instantiate(inventoryContentPrefab, parentForContents);
            InventoryContent contentComponent = createdSlot.GetComponent<InventoryContent>();

            if (contentComponent != null)
            {
                contentComponent.slotIndex = i;
                contentComponent.inventorySlot = new InventorySlot(new Item_Null(), 0); // InventorySlot ��ü ����
                contentComponent.SetInventoryManager(this);
                inventorySlots.Add(contentComponent);  // ���� ����Ʈ�� �߰�
            }
        }

        inventory.InitializeInventory(); // �κ��丮 �ʱ�ȭ
    }

    private void AddItemFunc()
    {
        // �������� �߰�

        // �Һ������ �߰�
        AddItem(new Item_RedPotion(), 3);
        AddItem(new Item_BluePotion(), 3);
        AddItem(new Item_OrangePotion(), 3);
        AddItem(new Item_WhitePotion(), 3);

        // ��Ÿ������ �߰�

        // ��ġ������ �߰�

        // ĳ�þ����� �߰�
    }

    // �������� �κ��丮�� �߰��ϴ� �Լ�
    public void AddItem(ItemData data, int count = 1)
    {
        List<InventorySlot> currentItemList = inventory.GetItemsByType(data.itemEnumType);

        if (count > 0) // ������ �߰� ����
        {
            if (data.itemIsStackable) // �������� ��ø ������ ���
            {
                foreach (var slot in currentItemList)
                {
                    if (slot.data.itemCode == data.itemCode)
                    {
                        int spaceInSlot = InventorySlot.MAX_STACK_COUNT - slot.itemCount;

                        if (spaceInSlot >= count)
                        {
                            slot.AddItemCount(count);
                            count = 0;
                            break;
                        }
                        else
                        {
                            slot.AddItemCount(spaceInSlot);
                            count -= spaceInSlot;
                        }
                    }
                }
            }

            // ���� �߰����� ���� �������� �����ִ� ���, ���ο� �� ���Կ� �߰��մϴ�.
            while (count > 0)
            {
                int amountToAdd = data.itemIsStackable ? System.Math.Min(count, InventorySlot.MAX_STACK_COUNT) : 1;
                InventorySlot newSlot = new InventorySlot(data, amountToAdd);

                bool slotFound = false;

                // Item_Null�� �ʱ�ȭ�� ������ ã�Ƽ� ������ �߰�
                for (int i = 0; i < currentItemList.Count; i++)
                {
                    if (currentItemList[i].data is Item_Null)
                    {
                        currentItemList[i] = newSlot;
                        slotFound = true;
                        count -= amountToAdd;
                        break;
                    }
                }

                // �� �̻� �� ������ ������ �ߴ��մϴ�.
                if (!slotFound)
                {
                    Debug.LogWarning("No available slot to add item!");
                    break;
                }
            }
        }
        else if (count < 0) // ������ ���� ����
        {
            int totalItemCount = 0;

            foreach (var slot in currentItemList)
            {
                if (slot.data.itemCode == data.itemCode)
                {
                    totalItemCount += slot.itemCount;
                }
            }

            if (totalItemCount + count >= 0) // �� ������ ���� ���ҽ�Ű���� �ϴ� �������� ū�� Ȯ��
            {
                int amountToRemove = -count; // ����� ��ȯ

                foreach (var slot in currentItemList)
                {
                    if (slot.data.itemCode == data.itemCode && !(slot.itemCount == InventorySlot.MAX_STACK_COUNT))
                    {
                        if (slot.itemCount <= amountToRemove)
                        {
                            amountToRemove -= slot.itemCount;
                            slot.Init(); // ������ ���� �ʱ�ȭ
                        }
                        else
                        {
                            slot.RemoveItemCount(amountToRemove);
                            amountToRemove = 0;
                            break;
                        }
                    }
                }

                // ���� ������ �ִٸ�, �� ���ִ� ���Կ��� ���ҽ�Ų��.
                if (amountToRemove > 0)
                {
                    foreach (var slot in currentItemList)
                    {
                        if (slot.data.itemCode == data.itemCode)
                        {
                            if (slot.itemCount > amountToRemove)
                            {
                                slot.RemoveItemCount(amountToRemove);
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                Debug.LogWarning("Attempting to remove more items than available!");
                return;  // ���⿡�� return�� ����Ͽ� �޼��带 �����մϴ�.
            }
        }

        // �κ��丮 UI ������Ʈ
        UpdateInventoryUI(data.itemEnumType);
    }


    // ������ ������ ���� �κ��丮 UI�� ������Ʈ�ϴ� �Լ�
    public void UpdateInventoryUI(ItemType itemType)
    {
        // ������ �������� UI�� ����
        var items = inventory.GetItemsByType(itemType);

        for (int i = 0; i < inventorySlots.Count; i++)
        {
            if (i < items.Count)
            {
                inventorySlots[i].UpdateItem(items[i]);
            }
            else
            {
                inventorySlots[i].ClearItem();
            }
        }
    }

    // ��� ������ �� ǥ��
    public void DisplayEquipmentItems()
    {
        ClearAllSlots(); // ��� ������ �ʱ�ȭ

        tabButtonIndex = 0;

        UpdateInventoryUI(ItemType.Equipment);
    }

    // �Һ� ������ �� ǥ��
    public void DisplayUseableItems()
    {
        ClearAllSlots(); // ��� ������ �ʱ�ȭ

        tabButtonIndex = 1;

        UpdateInventoryUI(ItemType.Usable);
    }

    // ��Ÿ ������ �� ǥ��
    public void DisplayOtherItems()
    {
        ClearAllSlots(); // ��� ������ �ʱ�ȭ

        tabButtonIndex = 2;

        UpdateInventoryUI(ItemType.Other);
    }

    // ��ġ ������ �� ǥ��
    public void DisplaySetUpItems()
    {
        ClearAllSlots(); // ��� ������ �ʱ�ȭ

        tabButtonIndex = 3;

        UpdateInventoryUI(ItemType.SetUp);
    }

    // ĳ�� ������ �� ǥ��
    public void DisplayCashItems()
    {
        ClearAllSlots(); // ��� ������ �ʱ�ȭ

        tabButtonIndex = 4;

        UpdateInventoryUI(ItemType.Cash);
    }

    // ��� ������ �ʱ�ȭ�ϴ� �Լ�
    private void ClearAllSlots()
    {
        foreach (InventoryContent slot in inventorySlots)
        {
            slot.ClearItem();
        }
    }

    // �������� �����ϴ� �Լ�
    public void SwitchItem(ItemType itemType, int cell1, int cell2)
    {
        if (cell1 < 0 || cell2 < 0) return;

        inventory.SwitchItem(itemType, cell1, cell2);

        UpdateInventoryUI(itemType);
    }

    // ���콺 Ŀ�� ���� ������ �̹����� ǥ���ϴ� �Լ�
    public void ShowFollowCursorItem(Sprite sprite)
    {
        followCursor.GetComponent<Image>().sprite = sprite;
        followCursor.gameObject.SetActive(true);
    }

    // ���콺 Ŀ�� ���� ǥ�õ� ������ �̹����� ����� �Լ�
    public void HideFollowCursorItem()
    {
        followCursor.gameObject.SetActive(false);
    }

    // ���콺 Ŀ�� ���� ǥ�õ� ������ �̹����� ���󰡰� �ϴ� �Լ�
    public void FollowCursorItem(Vector2 mousePosition)
    {
        followCursor.position = mousePosition;
    }

    // �÷��̾� ������ ������ ������Ʈ�ϴ� �Լ�
    public void UpdatePlayerMoneyUI()
    {
        //playerMoneyText.text = playerData.playerMoney.ToString();
        playerMoneyText.text = string.Format("{0:N0}", PlayerData.playerMoney);
    }

    // �������� ������ ��ġ(itemPurchasePrice)�� �������� �����ϴ� �Լ�
    public void SortInventoryByValue()
    {
        // ������ ����� �����ɴϴ�. ������ ������ ���� ������ �� �ֵ��� �����Ǿ� �־�� �մϴ�.
        List<InventorySlot> itemList = inventory.GetItemsByType((ItemType)(tabButtonIndex + 1));

        itemList.Sort((slot1, slot2) =>
        {
            string value1Str = slot1.data.itemPurchasePrice;
            string value2Str = slot2.data.itemPurchasePrice;

            if (value1Str == "-1" && value2Str != "-1")
            {
                return 1;
            }
            else if (value1Str != "-1" && value2Str == "-1")
            {
                return -1;
            }
            else if (value1Str == "-1" && value2Str == "-1")
            {
                return 0;
            }
            else
            {
                int value1 = int.Parse(value1Str);
                int value2 = int.Parse(value2Str);
                return value1.CompareTo(value2);
            }
        });

        // ���ĵ� ������ ����� �ٽ� �κ��丮�� �߰��մϴ�.
        foreach (InventorySlot slot in itemList)
        {
            inventory.AddItemToList(slot);
        }

        // UI�� ������Ʈ�մϴ�.
        UpdateInventoryUI((ItemType)(tabButtonIndex + 1));
    }

    public int GetTotalItemCountOfItem(ItemData item)
    {
        int totalCount = 0;

        foreach (InventorySlot slot in inventory.GetItemsByType(item.itemEnumType))
        {
            if (slot.data.itemCode == item.itemCode)
            {
                totalCount += slot.itemCount;
            }
        }

        return totalCount;
    }
}
