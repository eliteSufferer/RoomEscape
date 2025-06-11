using UnityEngine;
using TMPro;

public class SafeController : MonoBehaviour, IInteractable
{
    [Header("Safe Settings")]
    public string correctCode = "1234";
    public bool isOpen = false;
    public int maxCodeLength = 4;
    
    [Header("UI")]
    public GameObject codePanel;
    public TextMeshProUGUI codeDisplay;
    public TextMeshProUGUI statusText;
    
    [Header("Key")]
    public GameObject key;
    
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip buttonSound;
    public AudioClip openSound;
    public AudioClip errorSound;
    
    private string currentCode = "";
    private bool uiOpen = false;
    
    public void Interact()
    {
        if (!isOpen)
        {
            OpenCodeUI();
        }
    }
    
    public string GetInteractionText()
    {
        return isOpen ? "Сейф открыт" : "Открыть сейф";
    }
    
    public void OpenCodeUI()
    {
        uiOpen = true;
        codePanel.SetActive(true);
        currentCode = "";
        UpdateDisplay();
        statusText.text = "Введите код:";
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
    public void CloseCodeUI()
    {
        uiOpen = false;
        codePanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    public void AddDigit(int digit)
    {
        if (currentCode.Length < maxCodeLength)
        {
            // 🔊 Звук нажатия кнопки
            PlaySound(buttonSound);
            
            currentCode += digit.ToString();
            UpdateDisplay();
        }
    }
    
    public void ClearCode()
    {
        // 🔊 Звук нажатия кнопки
        PlaySound(buttonSound);
        
        currentCode = "";
        UpdateDisplay();
        statusText.text = "Введите код:";
    }
    
    public void SubmitCode()
    {
        if (currentCode == correctCode)
        {
            OpenSafe();
        }
        else
        {
            // 🔊 Звук ошибки
            PlaySound(errorSound);
            
            statusText.text = "Неверный код!";
            currentCode = "";
            UpdateDisplay();
        }
    }
    
    void UpdateDisplay()
    {
        string display = currentCode;
        for (int i = currentCode.Length; i < maxCodeLength; i++)
        {
            display += "-";
        }
        codeDisplay.text = display;
    }
    
    void OpenSafe()
    {
        isOpen = true;
        statusText.text = "Сейф открыт!";
        
        // 🔊 Звук открытия
        PlaySound(openSound);
        
        if (key != null)
        {
            key.SetActive(true);
        }
        
        Invoke("CloseCodeUI", 1.5f);
    }
    
    void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}