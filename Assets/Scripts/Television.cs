using UnityEngine;

public class Television : MonoBehaviour, IInteractable
{
    [Header("TV Settings")]
    public bool isOn = true;  // 🔥 Изначально включен
    public bool isFixed = false;
    
    [Header("Screen Materials")]
    public Material offMaterial;      // Черный материал
    public Material staticMaterial;   // Материал с помехами
    public Material codeMaterial;     // Материал с кодом
    
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip switchSound;     // 🔥 Переименовали: звук переключения
    public AudioClip staticSound;

    [Header("Screen Settings")]
    public Renderer screenRenderer;
    public int screenMaterialIndex = 1;

    void Start()
    {
        // 🔥 При запуске игры обновляем экран и звук
        UpdateScreen();
        StartInitialAudio();
    }

    void StartInitialAudio()
    {
        // Если телевизор включен при старте - запускаем статик
        if (isOn && !isFixed && staticSound != null && audioSource != null)
        {
            audioSource.clip = staticSound;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    void UpdateScreen()
    {
        if (screenRenderer == null) return;
        
        Material[] materials = screenRenderer.materials;
        
        if (!isOn)
            materials[screenMaterialIndex] = offMaterial;
        else if (!isFixed)
            materials[screenMaterialIndex] = staticMaterial;
        else
            materials[screenMaterialIndex] = codeMaterial;
            
        screenRenderer.materials = materials;
    }
    
    public void Interact()
    {
        TogglePower();
    }
    
    public string GetInteractionText()
    {
        return isOn ? "Выключить телевизор" : "Включить телевизор";
    }
    
    void TogglePower()
    {
        isOn = !isOn;
        UpdateScreen();
        PlaySounds();
    }
    
    void PlaySounds()
    {
        if (audioSource == null) return;
        
        // 🔥 ВСЕГДА играем звук переключения при нажатии (ГРОМКО!)
        if (switchSound != null)
        {
            audioSource.PlayOneShot(switchSound, 8.0f); // Максимальная громкость для переключения
        }
        
        // Управляем статиком
        if (isOn && !isFixed)
        {
            // Включили - запускаем статик (с небольшой задержкой после звука переключения)
            if (staticSound != null)
            {
                Invoke("PlayStatic", 0.4f); // Задержка чтобы не перебивал звук переключения
            }
        }
        else
        {
            // Выключили или починен - останавливаем статик с задержкой
            if (audioSource.clip == staticSound)
            {
                Invoke("StopStatic", 0.2f); // 🔥 Задержка чтобы звук переключения успел проиграться
            }
        }
    }
    
    void StopStatic()
    {
        if (audioSource != null && audioSource.clip == staticSound)
        {
            audioSource.Stop();
        }
    }
    
    void PlayStatic()
    {
        if (audioSource != null && staticSound != null && isOn && !isFixed)
        {
            audioSource.clip = staticSound;
            audioSource.loop = true;
            audioSource.Play();
        }
    }
    
    public void FixTV()
    {
        isFixed = true;
        UpdateScreen();
        
        // Останавливаем статик когда починили (тоже с задержкой если нужно)
        if (audioSource != null && audioSource.clip == staticSound)
        {
            StopStatic();
        }
        
        Debug.Log("Телевизор починен! Код на экране.");
    }
}