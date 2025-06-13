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
    
    [Header("Animation")]
    public Animator safeAnimator; // 🔥 Аниматор двери сейфа (перетяни объект с дверцей!)
    public string openTrigger = "OpenSafe"; // 🔥 Триггер анимации (лучше чем Bool)
    
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
        else
        {
            Debug.Log("Сейф уже открыт!");
        }
    }
    
    public string GetInteractionText()
    {
        return isOpen ? "Сейф открыт" : "Открыть сейф";
    }
    
    public void OpenCodeUI()
    {
        Debug.Log("Открытие UI кода сейфа");

        if (uiOpen)
        {
            Debug.Log("UI кода уже открыто!");
            return;
        }
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
        Debug.Log("Закрытие UI кода сейфа");
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
        
        // 🎬 АНИМАЦИЯ ОТКРЫТИЯ ДВЕРИ СЕЙФА
        if (safeAnimator != null)
        {
            safeAnimator.SetTrigger(openTrigger);
            Debug.Log("Запущена анимация открытия сейфа!");
        }
        
        // Показываем ключ с небольшой задержкой (после начала анимации)
        Invoke("ShowKey", 0.5f);
        
        // Закрываем UI с задержкой
        Invoke("CloseCodeUI", 0.5f);
    }
    
    void ShowKey()
    {
        if (key != null)
        {
            key.SetActive(true);
            Debug.Log("Ключ появился в сейфе!");
        }
    }
    
    void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}