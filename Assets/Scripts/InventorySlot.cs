using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    [Header("UI References")]
    public Image itemIcon;
    public TextMeshProUGUI itemCount;
    public Button slotButton;
    
    private PickupItem currentItem;
    private int count;
    
    void Start()
    {
        if (slotButton == null)
            slotButton = GetComponent<Button>();
            
        slotButton.onClick.AddListener(UseItem);
    }
    
    public void SetItem(PickupItem item, int itemCount)
    {
        currentItem = item;
        count = itemCount;
        
        if (item.itemIcon != null)
        {
            itemIcon.sprite = item.itemIcon;
            itemIcon.color = Color.white;
        }
        
        itemCount = count;
        if (count > 1)
        {
            this.itemCount.text = count.ToString();
            this.itemCount.gameObject.SetActive(true);
        }
        else
        {
            this.itemCount.gameObject.SetActive(false);
        }
        
        itemIcon.gameObject.SetActive(true);
    }
    
    public void ClearSlot()
    {
        currentItem = null;
        count = 0;
        itemIcon.gameObject.SetActive(false);
        itemCount.gameObject.SetActive(false);
    }
    
    public void UseItem()
    {
        if (currentItem != null)
        {
            Debug.Log($"Используем: {currentItem.itemName}");
            // Здесь будет логика использования предметов
        }
    }
}