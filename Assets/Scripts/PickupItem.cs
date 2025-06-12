using UnityEngine;

public class PickupItem : MonoBehaviour, IInteractable
{
    [Header("Item Settings")]
    public string itemName = "Ключ";
    public Sprite itemIcon;
    public int stackSize = 4; // 🔥 Максимум 3 в стаке
    
    [Header("Audio")]
    public AudioClip pickupSound;
    
    public void Interact()
    {
        // Пытаемся добавить в инвентарь
        if (InventoryManager.Instance.AddItem(this))
        {
            // 🔊 Играем звук подбора
            PlayPickupSound();
            
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
    
    void PlayPickupSound()
    {
        if (pickupSound != null)
        {
            // Создаем временный объект для звука
            GameObject tempAudio = new GameObject("TempAudio");
            AudioSource audioSource = tempAudio.AddComponent<AudioSource>();
            audioSource.clip = pickupSound;
            audioSource.Play();
            
            // Уничтожаем через длину клипа
            Destroy(tempAudio, pickupSound.length);
        }
    }
}