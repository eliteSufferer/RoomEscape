using UnityEngine;

public class ExitDoor : MonoBehaviour, IInteractable
{
    [Header("Door Settings")]
    public bool isLocked = true;
    public bool isOpen = false;
    public string requiredItem = "Ключ";
    
    [Header("Animation")]
    public Animator doorAnimator;
    public string openParameter = "IsOpen";
    
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip unlockSound;
    public AudioClip doorOpenSound;
    public AudioClip doorCloseSound;
    
    public void Interact()
    {
        if (isLocked)
        {
            // Дверь заперта - нужен активный ключ
            if (InventoryManager.Instance.HasActiveItem(requiredItem))
            {
                UnlockDoor();
            }
            else
            {
                Debug.Log("Дверь заперта. Выберите ключ в инвентаре!");
            }
        }
        else
        {
            // Дверь разблокирована - можно открывать/закрывать
            ToggleDoor();
        }
    }
    
    public string GetInteractionText()
    {
        if (isLocked)
        {
            if (InventoryManager.Instance.HasActiveItem(requiredItem))
            {
                return "Открыть дверь ключом";
            }
            else if (InventoryManager.Instance.HasItem(requiredItem))
            {
                return "Дверь заперта (выберите ключ в инвентаре)";
            }
            else
            {
                return "Дверь заперта (нужен ключ)";
            }
        }
        else
        {
            return isOpen ? "Закрыть дверь" : "Открыть дверь";
        }
    }
    
    void UnlockDoor()
    {
        // Разблокируем дверь
        isLocked = false;
        
        // Используем ключ из инвентаря
        InventoryManager.Instance.UseItem(requiredItem);
        
        // Звук разблокировки
        if (audioSource != null && unlockSound != null)
        {
            audioSource.PlayOneShot(unlockSound);
        }
        
        // Сразу открываем дверь
        OpenDoor();
        
        Debug.Log("Дверь разблокирована и открыта!");
        
        // Победа через 2 секунды
        Invoke("ShowVictoryMessage", 2f);
    }
    
    void ToggleDoor()
    {
        if (isOpen)
        {
            CloseDoor();
        }
        else
        {
            OpenDoor();
        }
    }
    
    void OpenDoor()
    {
        isOpen = true;
        
        // Анимация открытия
        if (doorAnimator != null)
        {
            doorAnimator.SetBool(openParameter, true);
        }
        
        // Звук открытия
        if (audioSource != null && doorOpenSound != null)
        {
            audioSource.PlayOneShot(doorOpenSound);
        }
        
        Debug.Log("Дверь открыта!");
    }
    
    void CloseDoor()
    {
        isOpen = false;
        
        // Анимация закрытия
        if (doorAnimator != null)
        {
            doorAnimator.SetBool(openParameter, false);
        }
        
        // Звук закрытия
        if (audioSource != null && doorCloseSound != null)
        {
            audioSource.PlayOneShot(doorCloseSound);
        }
        
        Debug.Log("Дверь закрыта!");
    }
    
    void ShowVictoryMessage()
    {
        Debug.Log("🎉 Поздравляем! Дверь открыта, теперь можете выйти из комнаты! 🎉");
    }
}