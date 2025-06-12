using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryTrigger : MonoBehaviour
{
    [Header("Victory Settings")]
    public bool doorMustBeOpen = true; // Дверь должна быть открыта
    public ExitDoor exitDoor; // Ссылка на дверь
    // public float delayBeforeRestart = 3f;
    
    [Header("Victory UI")]
    public GameObject victoryPanel; // UI панель победы (опционально)
    
    void Start()
    {
        // Триггер должен быть включен
        GetComponent<Collider>().isTrigger = true;
    }
    
    void OnTriggerEnter(Collider other)
    {
        // Проверяем что это игрок
        if (other.CompareTag("Player"))
        {
            // Проверяем условие победы
            if (CanWin())
            {
                TriggerVictory();
            }
            else
            {
                Debug.Log("Дверь должна быть открыта!");
            }
        }
    }
    
    bool CanWin()
    {
        if (!doorMustBeOpen) return true;
        
        // Проверяем что дверь открыта и разблокирована
        if (exitDoor != null)
        {
            return !exitDoor.isLocked && exitDoor.isOpen;
        }
        
        return true; // Если дверь не назначена, считаем что можно
    }
    
    void TriggerVictory()
    {
        Debug.Log("🎉 ПОБЕДА! Вы выбрались из комнаты! 🎉");
        
        // Показываем UI панель победы
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        
        // Останавливаем игру или перезапускаем уровень
        // Invoke("RestartLevel", delayBeforeRestart);
    }
    
    void RestartLevel()
    {
        // Перезагружаем сцену или переходим в главное меню
        Time.timeScale = 1f; // На случай если была пауза
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
        // Или переход в главное меню:
        // SceneManager.LoadScene("MainMenu");
    }
    
    // Публичные методы для UI кнопок
    public void RestartGame()
    {
        SceneManager.LoadScene("MainMenu");
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}