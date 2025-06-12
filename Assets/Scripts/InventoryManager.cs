using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("UI")]
    public GameObject inventoryPanel;
    public InventorySlot[] inventorySlots;
    
    [Header("Active Item UI")]
    public TextMeshProUGUI activeItemText; // 🔥 Показывает активный предмет

    [Header("Settings")]
    public int inventorySize = 8;

    private PickupItem[] items;
    private int[] itemCounts;
    private int activeSlotIndex = -1; // 🔥 Индекс активного слота (-1 = ничего не выбрано)

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
                Debug.Log($"Добавлен {item.itemName} в стак. Количество: {itemCounts[i]}/{item.stackSize}");
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
                Debug.Log($"Добавлен новый {item.itemName}. Количество: 1/{item.stackSize}");
                return true;
            }
        }

        Debug.Log("Инвентарь полон! Не могу добавить " + item.itemName);
        return false;
    }

    void UpdateInventoryDisplay()
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (i < inventorySize && items[i] != null)
            {
                bool isActive = (i == activeSlotIndex); // 🔥 Проверяем активность
                inventorySlots[i].SetItem(items[i], itemCounts[i], isActive);
            }
            else
            {
                inventorySlots[i].ClearSlot();
            }
        }
        
        UpdateActiveItemDisplay(); // 🔥 Обновляем отображение активного предмета
    }
    
    // 🔥 НОВАЯ ФУНКЦИЯ: выбрать активный предмет
    public void SelectActiveItem(int slotIndex)
    {
        if (slotIndex < inventorySize && items[slotIndex] != null)
        {
            if (activeSlotIndex == slotIndex)
            {
                // Повторный клик - снимаем выделение
                activeSlotIndex = -1;
                Debug.Log("Предмет снят с выделения");
            }
            else
            {
                // Выбираем новый предмет
                activeSlotIndex = slotIndex;
                Debug.Log($"Активный предмет: {items[slotIndex].itemName}");
            }
            
            UpdateInventoryDisplay();
        }
    }
    
    // 🔥 НОВАЯ ФУНКЦИЯ: получить активный предмет
    public string GetActiveItemName()
    {
        if (activeSlotIndex >= 0 && activeSlotIndex < inventorySize && items[activeSlotIndex] != null)
        {
            return items[activeSlotIndex].itemName;
        }
        return null;
    }
    
    // 🔥 НОВАЯ ФУНКЦИЯ: есть ли активный предмет определенного типа
    public bool HasActiveItem(string itemName)
    {
        string activeItem = GetActiveItemName();
        return activeItem != null && activeItem == itemName;
    }
    
    void UpdateActiveItemDisplay()
    {
        if (activeItemText != null)
        {
            string activeItem = GetActiveItemName();
            if (activeItem != null)
            {
                activeItemText.text = $"Активно: {activeItem}";
                activeItemText.gameObject.SetActive(true);
            }
            else
            {
                activeItemText.text = "Ничего не выбрано";
                activeItemText.gameObject.SetActive(false);
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
                Debug.Log($"Использован {itemName}. Осталось: {itemCounts[i]}");
                
                if (itemCounts[i] <= 0)
                {
                    items[i] = null;
                    itemCounts[i] = 0;
                    
                    // 🔥 Если использованный предмет был активным - сбрасываем выделение
                    if (activeSlotIndex == i)
                    {
                        activeSlotIndex = -1;
                    }
                }
                UpdateInventoryDisplay();
                return;
            }
        }
    }
    
    public int GetItemCount(string itemName)
    {
        for (int i = 0; i < inventorySize; i++)
        {
            if (items[i] != null && items[i].itemName == itemName)
            {
                return itemCounts[i];
            }
        }
        return 0;
    }
}