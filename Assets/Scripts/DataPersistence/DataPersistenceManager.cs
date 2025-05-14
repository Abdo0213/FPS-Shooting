using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    private GameData gameData;
    private List<IDataPersistence> dataPersistencesObjects;
    private FileDataHandler dataHandler;
    public static DataPersistenceManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        dataPersistencesObjects = FindAllDataPersistenceObjects();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Refresh the list of persistence objects
        this.dataPersistencesObjects = FindAllDataPersistenceObjects();
        
        if (this.gameData != null)
        {
            StartCoroutine(LoadDataAfterDelay());
        }
    }
    public void StartFreshAtLevel(int levelIndex)
    {
        this.gameData = new GameData(); // Fresh default values
        this.gameData.level = levelIndex; // Set to desired level
        LoadingMenuManager.Instance.SwitchToScene(levelIndex); // Load that scene
    }
    private IEnumerator LoadDataAfterDelay()
    {
        // Wait for one frame to ensure all components are initialized
        yield return null;
        
        foreach (IDataPersistence dataPersistenceObj in dataPersistencesObjects)
        {
            if (dataPersistenceObj != null)
            {
                dataPersistenceObj.LoadData(gameData);
            }
            else
            {
                Debug.LogWarning("Found null IDataPersistence object in list");
            }
        }
    }

    public void NewGame()
    {
        this.gameData = new GameData();
        // Don't load data for a new game
        LoadingMenuManager.Instance.SwitchToScene(1);
    }

    public void LoadGame()
    {
        // Load the saved data first
        this.gameData = dataHandler.Load();
        
        if (this.gameData == null)
        {
            Debug.Log("No data was found. Starting new game.");
            NewGame();
            return;
        }
        
        // Scene loading will trigger data application in OnSceneLoaded
        LoadingMenuManager.Instance.SwitchToScene(gameData.level); 
    }

    public void SaveGame()
    {
        // Update scene index before saving
        gameData.level = SceneManager.GetActiveScene().buildIndex;
        
        foreach (IDataPersistence dataPersistenceObj in dataPersistencesObjects)
        {
            dataPersistenceObj.SaveData(ref gameData);
        }
        dataHandler.Save(gameData);
    }
    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>()
            .OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistenceObjects);
    }
}