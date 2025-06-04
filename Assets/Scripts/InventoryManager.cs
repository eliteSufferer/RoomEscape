using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("UI")]
    public GameObject inventoryPanel;
    public InventorySlot[] inventorySlots;

    [Header("Settings")]
    public int inventorySize = 8;

    private PickupItem[] items;
    private int[] itemCounts;

    void Awake()
    {
        Instance = this;
        items = new PickupItem[inventorySize];
        itemCounts = new int[inventorySize];
    }

    void Update()
    {
        // Открыть/закрыть инвентарь по Tab
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventory();
        }
    }

    public void ToggleInventory()
    {
        bool isActive = !inventoryPanel.activeSelf;
        inventoryPanel.SetActive(isActive);

        if (isActive)
        {
            UpdateInventoryDisplay();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public bool AddItem(PickupItem item)
    {
        // Ищем существующий стак
        for (int i = 0; i < inventorySize; i++)
        {
            if (items[i] != null && items[i].itemName == item.itemName && itemCounts[i] < item.stackSize)
            {
                itemCounts[i]++;
                UpdateInventoryDisplay();
                return true;
            }
        }

        // Ищем пустой слот
        for (int i = 0; i < inventorySize; i++)
        {
            if (items[i] == null)
            {
                items[i] = item;
                itemCounts[i] = 1;
                UpdateInventoryDisplay();
                return true;
            }
        }

        return false; // Инвентарь полон
    }

    void UpdateInventoryDisplay()
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (i < inventorySize && items[i] != null)
            {
                inventorySlots[i].SetItem(items[i], itemCounts[i]);
            }
            else
            {
                inventorySlots[i].ClearSlot();
            }
        }
    }
    
    public bool HasItem(string itemName)
{
    for (int i = 0; i < inventorySize; i++)
    {
        if (items[i] != null && items[i].itemName == itemName)
        {
            return true;
        }
    }
    return false;
}

public void UseItem(string itemName)
{
    for (int i = 0; i < inventorySize; i++)
    {
        if (items[i] != null && items[i].itemName == itemName)
        {
            itemCounts[i]--;
            if (itemCounts[i] <= 0)
            {
                items[i] = null;
                itemCounts[i] = 0;
            }
            UpdateInventoryDisplay();
            return;
        }
    }
}
}