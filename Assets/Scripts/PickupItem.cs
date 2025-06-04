using UnityEngine;

public class PickupItem : MonoBehaviour, IInteractable
{
    [Header("Item Settings")]
    public string itemName = "Ключ";
    public Sprite itemIcon;
    public int stackSize = 1;
    
    public void Interact()
    {
        // Пытаемся добавить в инвентарь
        if (InventoryManager.Instance.AddItem(this))
        {
            gameObject.SetActive(false); // Убираем предмет
            Debug.Log($"Подобран: {itemName}");
        }
        else
        {
            Debug.Log("Инвентарь полон!");
        }
    }
    
    public string GetInteractionText()
    {
        return $"Взять {itemName}";
    }
}