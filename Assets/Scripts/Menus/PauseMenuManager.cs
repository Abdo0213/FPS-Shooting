using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
public class PauseMenuManager : MonoBehaviour
{
    
    [SerializeField] GameObject pauseMenuScreen;
    public static bool isPaused;
    [SerializeField] GameObject settingScreen;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(isPaused){
                Continue();
            } else{
                Pause();
            }
        }
    }
    public void Pause(){
        if (pauseMenuScreen != null) {
            pauseMenuScreen.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;
            isPaused = true;
        } else {
            Debug.LogWarning("Pause menu UI is not assigned!");
        }
    }
    public void Continue(){
        Debug.Log("entered");
        settingScreen.SetActive(false);
        pauseMenuScreen.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isPaused = false;
    }
    public void Save(){
        DataPersistenceManager.instance.SaveGame();
    }
    public void Settings(){
        pauseMenuScreen.SetActive(false);
        settingScreen.SetActive(true);
    }
    public void ReturnPause(){
        settingScreen.SetActive(false);
        pauseMenuScreen.SetActive(true);
    }
    public void MainMenu(){
        StartCoroutine(LoadMainMenuWithDelay());
    }

    IEnumerator LoadMainMenuWithDelay(){
        isPaused = false;
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        yield return new WaitForSecondsRealtime(0.1f); // ensures delay even if game is paused
        SceneManager.LoadScene("Menu");
    }
}
