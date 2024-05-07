using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("Debugging")]
    [SerializeField] private bool initializeDataIfNull = false;
    [Header("File Storage Config")]
    [SerializeField] private string fileName; 
    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;
    public static DataPersistenceManager instance {get; private set;}
    [SerializeField] private bool useEncryption;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.Log("Found more than one Data Persistence Manager in the scene. Destroying the Newest one.");
        Destroy(this.gameObject);
        return;
        }
        instance = this;
       DontDestroyOnLoad(this.gameObject);
    
    this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);

    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;

    }
        public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded Called");
    this.dataPersistenceObjects = FindAllDataPeresistenceObjects();
            LoadGame();
    }

        public void OnSceneUnloaded(Scene scene)
    {
        Debug.Log("OnSceneUnloaded Called");
        SaveGame();
    }

    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        this.gameData = dataHandler.Load();

      if (this.gameData == null && initializeDataIfNull)
        {
           NewGame();
        }

        if(this.gameData == null)
        {
            Debug.Log("No Data was found. A New Game needs to be started before data can be loaded.");
            return;
        }
        
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.loadData(gameData);
        }
        Debug.Log("Loaded room count = " + gameData.DoubleJumpItem);
    }

    public void SaveGame()
    {
if(this.gameData == null)
{
Debug.LogWarning("No data was found. A new Game needs to be started before data can be saved.");
return;
}
       
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(ref gameData);
        }
        Debug.Log("Saved room count = " + gameData.DoubleJumpItem);

        dataHandler.Save(gameData);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

     private List<IDataPersistence> FindAllDataPeresistenceObjects()
    {
        MonoBehaviour[] monoBehaviours = FindObjectsOfType<MonoBehaviour>();
        
        if (monoBehaviours == null)
        {
            Debug.LogWarning("No MonoBehaviour found in the scene.");
            return new List<IDataPersistence>();
        }

        IEnumerable<IDataPersistence> dataPersistenceObjects = monoBehaviours
            .OfType<IDataPersistence>();

        return dataPersistenceObjects.ToList();
    }
}