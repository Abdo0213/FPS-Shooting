using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinManager : MonoBehaviour
{
    [SerializeField] GameObject winScreen;
    public static bool isWin;
    [SerializeField] private Target[] enemies;
    
    private bool hasWon = false;

    void Start()
    {
        StartCoroutine(CheckWinCondition());
    }

    IEnumerator CheckWinCondition()
    {
        while (!hasWon)
        {
            if (AllEnemiesDead())
            {
                Win();
                hasWon = true;
            }
            else
            {
                // Optional: remove this to stop the warning spam
                Debug.LogWarning("Enemies remain! Cannot open door.");
            }
            yield return new WaitForSeconds(1f); // check every second instead of every frame
        }
    }

    public void Win()
    {
        if (winScreen != null)
        {
            winScreen.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;
            isWin = true;
        }
        else
        {
            Debug.LogWarning("Win menu UI is not assigned!");
        }
    }

    public void MainMenu()
    {
        StartCoroutine(LoadMainMenuWithDelay());
    }

    IEnumerator LoadMainMenuWithDelay()
    {
        isWin = false;
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        yield return new WaitForSecondsRealtime(0.1f);
        SceneManager.LoadScene("Menu");
    }

    private bool AllEnemiesDead()
    {
        if (enemies == null || enemies.Length == 0) return true;
        foreach (Target enemy in enemies)
        {
            if (enemy != null && enemy.gameObject.activeInHierarchy && !enemy.IsDead)
            {
                return false;
            }
        }
        return true;
    }
}
