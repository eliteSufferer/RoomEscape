using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource musicSource;
    
    void Start()
    {
        // Разблокировать курсор в главном меню
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene"); // замени на название твоей игровой сцены
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
    
    public void SetMusicVolume(float volume)
    {
        if (musicSource != null)
        {
            musicSource.volume = volume;
        }
    }
}