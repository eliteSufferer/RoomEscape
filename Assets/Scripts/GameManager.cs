using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [Header("UI Panels")]
    public GameObject pauseMenuPanel;
    
    [Header("Audio")]
    public AudioSource musicSource;
    public UnityEngine.UI.Slider volumeSlider; // 🔥 Добавляем ссылку на слайдер
    
    private bool isPaused = false;
    
    void Awake()
    {
        Instance = this;
    }
    
    void Start()
    {
        // 🔥 Инициализируем слайдер текущей громкостью
        InitializeVolumeSlider();
    }
    
    void InitializeVolumeSlider()
    {
        if (volumeSlider != null && musicSource != null)
        {
            volumeSlider.value = musicSource.volume;
        }
    }
    
    void Update()
    {
        // ESC для паузы
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }
    
    public void TogglePause()
    {
        isPaused = !isPaused;
        
        if (isPaused)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }
    
    void PauseGame()
    {
        Time.timeScale = 0f;
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(true);
        }
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
    void ResumeGame()
    {
        Time.timeScale = 1f;
        pauseMenuPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    // Кнопки меню
    public void ResumeButton()
    {
        ResumeGame();
        isPaused = false;
    }
    
    public void MainMenuButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
    
    public void QuitButton()
    {
        Application.Quit();
    }
    
    // Громкость музыки
    public void SetMusicVolume(float volume)
    {
        if (musicSource != null)
        {
            musicSource.volume = volume;
        }
    }
}