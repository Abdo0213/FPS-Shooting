using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingMenuManager : MonoBehaviour
{

    public static LoadingMenuManager Instance;
    public GameObject m_LoadingScreenObject;
    public Slider progressBar;
    public float minLoadTime = 35f;
    public Texture2D cursorTexture;
    public Vector2 hotSpot = Vector2.zero;
    public CursorMode cursorMode = CursorMode.Auto;
    public AudioSource audioSource;
    private void Awake()
    {
        if(Instance !=null && Instance != this){
            Destroy(this.gameObject);
        }
        else {
            Instance = this;
            //DontDestroyOnLoad(this.gameObject);
        }
        if (m_LoadingScreenObject != null)
            m_LoadingScreenObject.SetActive(false);
    }

    public void SwitchToScene(int id, bool unloadPrevious = true)
    {
        if (m_LoadingScreenObject == null)
        {
            Debug.LogError("Loading screen object is null!");
            return;
        }
        m_LoadingScreenObject.SetActive(true);
        progressBar.value = 0f;
        GetComponent<AudioSource>().Play();
        StartCoroutine(SwitchToSceneAsync(id, unloadPrevious));
    }

    IEnumerator SwitchToSceneAsync(int id, bool unloadPrevious)
    {
        float elapsed = 0f;
        float fillValue = 0f;

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(id, LoadSceneMode.Single);
        asyncLoad.allowSceneActivation = false;
        // Simulated loading time (if you want a minimum)
        
        while (elapsed < minLoadTime)
        {
            elapsed += Time.deltaTime;
            fillValue = Mathf.Clamp01(elapsed / minLoadTime);
            progressBar.value = fillValue;
            yield return null;
        }

        // Wait until the async scene has loaded to 90%
        while (asyncLoad.progress < 0.9f)
        {
            yield return null;
        }

        progressBar.value = 1f;

        // Optional delay
        yield return new WaitForSeconds(1f);

        asyncLoad.allowSceneActivation = true;

        // Wait one more frame for scene activation
        yield return null;

        // Set the loaded scene as active (optional)
        Scene loadedScene = SceneManager.GetSceneByBuildIndex(id);
        if (loadedScene.IsValid()){}
            SceneManager.SetActiveScene(loadedScene);

        // Optionally unload the previous scene (if you loaded additively earlier)
        if (unloadPrevious)
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            if (currentSceneName != loadedScene.name)
                SceneManager.UnloadSceneAsync(currentSceneName);
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        m_LoadingScreenObject.SetActive(false);
    }
    void Start()
    {
        Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.Auto);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
