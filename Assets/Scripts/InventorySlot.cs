using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    [Header("UI References")]
    public Image itemIcon;
    public TextMeshProUGUI itemCount;
    public Button slotButton;
    public Image slotBackground; // 🔥 Фон слота для подсветки
    
    [Header("Visual Settings")]
    public Color normalColor = Color.white;
    public Color activeColor = Color.yellow; // 🔥 Цвет активного предмета
    
    private PickupItem currentItem;
    private int count;
    private int slotIndex; // 🔥 Индекс этого слота
    
    void Start()
    {
        if (slotButton == null)
            slotButton = GetComponent<Button>();
            
        if (slotBackground == null)
            slotBackground = GetComponent<Image>();
            
        slotButton.onClick.AddListener(SelectItem);
        
        // 🔥 Найти свой индекс в массиве слотов
        FindSlotIndex();
    }
    
    void FindSlotIndex()
    {
        // Находим свой индекс среди слотов инвентаря
        InventorySlot[] allSlots = FindObjectOfType<InventoryManager>().inventorySlots;
        for (int i = 0; i < allSlots.Length; i++)
        {
            if (allSlots[i] == this)
            {
                slotIndex = i;
                break;
            }
        }
    }
    
    public void SetItem(PickupItem item, int itemCount, bool isActive = false)
    {
        currentItem = item;
        count = itemCount;
        
        if (item.itemIcon != null)
        {
            itemIcon.sprite = item.itemIcon;
            itemIcon.color = Color.white;
        }
        
        // Показываем количество
        this.itemCount.text = count.ToString();
        this.itemCount.gameObject.SetActive(true);
        
        itemIcon.gameObject.SetActive(true);
        
        // 🔥 Визуальная индикация активности
        UpdateActiveVisual(isActive);
    }
    
    public void ClearSlot()
    {
        currentItem = null;
        count = 0;
        itemIcon.gameObject.SetActive(false);
        itemCount.gameObject.SetActive(false);
        
        // 🔥 Сбрасываем подсветку
        UpdateActiveVisual(false);
    }
    
    void UpdateActiveVisual(bool isActive)
    {
        if (slotBackground != null)
        {
            slotBackground.color = isActive ? activeColor : normalColor;
        }
    }
    
    public void SelectItem()
    {
        if (currentItem != null)
        {
            // 🔥 Сообщаем InventoryManager о выборе этого слота
            InventoryManager.Instance.SelectActiveItem(slotIndex);
            Debug.Log($"Выбран слот {slotIndex}: {currentItem.itemName}");
        }
    }
    
    // 🔥 Старая функция UseItem больше не нужна, но оставим для совместимости
    public void UseItem()
    {
        SelectItem();
    }
}