using UnityEngine;

public class ExitDoor : MonoBehaviour, IInteractable
{
    [Header("Door Settings")]
    public bool isLocked = true;
    public string requiredItem = "Ключ";
    
    [Header("Animation")]
    public Animator doorAnimator;
    public string openAnimationTrigger = "Open";
    
    public void Interact()
    {
        if (isLocked)
        {
            if (HasRequiredKey())
            {
                OpenDoor();
            }
            else
            {
                Debug.Log("Дверь заперта. Нужен ключ!");
            }
        }
        else
        {
            Debug.Log("Дверь уже открыта!");
        }
    }
    
    public string GetInteractionText()
    {
        if (isLocked)
        {
            return HasRequiredKey() ? "Открыть дверь ключом" : "Дверь заперта (нужен ключ)";
        }
        else
        {
            return "Выйти из комнаты";
        }
    }
    
    bool HasRequiredKey()
    {
        // Проверяем есть ли ключ в инвентаре
        return InventoryManager.Instance.HasItem(requiredItem);
    }
    
    void OpenDoor()
    {
        isLocked = false;
        
        // Используем ключ из инвентаря
        InventoryManager.Instance.UseItem(requiredItem);
        
        // Анимация двери
        if (doorAnimator != null)
        {
            doorAnimator.SetTrigger(openAnimationTrigger);
        }
        
        Debug.Log("Дверь открыта! Победа!");
        
        // Через несколько секунд можно перезагрузить уровень или показать экран победы
        Invoke("GameWon", 2f);
    }
    
    void GameWon()
    {
        Debug.Log("Вы выбрались из комнаты!");
        // Здесь можно добавить экран победы или перезагрузку уровня
    }
}