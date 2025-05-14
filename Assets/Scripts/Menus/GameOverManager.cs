using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] GameObject gameOver;
    public static bool isOver;
    void Start()
    {
    }
    void Update()
    {
    }
    public void Over(){
        if (gameOver != null) {
            gameOver.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;
            isOver = true;
        } else {
            Debug.LogWarning("Game Over menu UI is not assigned!");
        }
    }
    public void Restart(){
        gameOver.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isOver = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void MainMenu(){
        StartCoroutine(LoadMainMenuWithDelay());
    }

    IEnumerator LoadMainMenuWithDelay(){
        isOver = false;
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        yield return new WaitForSecondsRealtime(0.1f);
        SceneManager.LoadScene("Menu");
    }
}
