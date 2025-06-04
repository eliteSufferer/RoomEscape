using UnityEngine;

public class Television : MonoBehaviour, IInteractable
{
    [Header("TV Settings")]
    public bool isOn = false;
    public bool isFixed = false;
    
    [Header("Screen Materials")]
    public Material offMaterial;      // Черный материал
    public Material staticMaterial;   // Материал с помехами
    public Material codeMaterial;     // Материал с кодом
    
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip turnOnSound;
    public AudioClip staticSound;

    [Header("Screen Settings")]
public Renderer screenRenderer;
public int screenMaterialIndex = 1; // Индекс материала экрана (обычно 0, 1, 2...)

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
        PlaySound();
    }
    
    
    void PlaySound()
    {
        if (audioSource == null) return;
        
        if (isOn && turnOnSound != null)
        {
            audioSource.PlayOneShot(turnOnSound);
        }
    }
    
    public void FixTV()
    {
        isFixed = true;
        UpdateScreen();
        Debug.Log("Телевизор починен! Код на экране.");
    }
}