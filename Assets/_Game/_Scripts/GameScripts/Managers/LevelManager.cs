using System;
using System.Collections;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static event Action<Level> OnLevelLoaded;
    public LevelInfo LevelInfo => levelInfo;
    public Level latestLevel;

    [SerializeField]
    private LevelInfo levelInfo;
    public int lastLevelEndMoney;
    public static LevelManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<LevelManager>();
                if (instance == null)
                {
                    instance = new GameObject("LevelManager").AddComponent<LevelManager>();
                }
            }
            return instance;
        }
    }
    public int CurrentLevelIndex => currentLevelIndex;
    public bool FirstStart
    {
        get
        {
            return ES3.Load("FirstStart", true);
        }
        set
        {
            ES3.Save("FirstStart", value);
        }
    }
    private int currentLevelIndex = 0;
    private static LevelManager instance;
    private const string LEVEL_INDEX_PREF_KEY = "LevelIndex";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(transform.root.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public bool immediateStart = false;
    private void Start()
    {
        if (immediateStart)
            LoadLevel();
    }
    public bool LoopLevels = true;
    public Level GetLevelInfo(int index)
    {
        if (levelInfo == null || levelInfo.levels == null || levelInfo.levels.Count == 0)
        {
            latestLevel = new Level();
            return latestLevel;
        }
        if (LoopLevels)
        {
            latestLevel = levelInfo.levels[index % LevelInfo.levels.Count];
        }
        else
        {
            latestLevel = levelInfo.levels[Mathf.Min(index, LevelInfo.levels.Count - 1)];
        }
        return latestLevel;
    }
    public Level GetLevelInfo()
    {
        currentLevelIndex = GetCurrentLevel();
        if (levelInfo == null || levelInfo.levels == null || levelInfo.levels.Count == 0)
        {
            latestLevel = new Level();
            return latestLevel;
        }
        if (LoopLevels)
        {
            latestLevel = levelInfo.levels[currentLevelIndex % LevelInfo.levels.Count];
        }
        else
        {
            latestLevel = levelInfo.levels[Mathf.Min(currentLevelIndex, LevelInfo.levels.Count - 1)];
        }
        return latestLevel;
    }
    public void LoadLevel(bool retry = false)
    {
        StartCoroutine(LoadLevelCoroutine(retry));
        //UnityEngine.SceneManagement.SceneManager.LoadScene("Game Scene");
    }
    public void LoadMenu(bool winCond)
    {
        int count = transform.childCount;
        for (int i = 0; i < count; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        if (winCond) IncreaseLevel();
        //else HealthManager.Instance.HealthCount -= 1;

        if (!UnityEngine.SceneManagement.SceneManager.GetSceneByName("Room Scene").isLoaded)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Room Scene");
        }
    }
    private IEnumerator LoadLevelCoroutine(bool retry = false)
    {
        latestLevel = GetLevelInfo();
        if (retry)
        {
            GridData data = latestLevel.gridData;

            latestLevel = new Level
            {
                gridData = data,
                levelMoveCount = 1
            };
        }
        if (!UnityEngine.SceneManagement.SceneManager.GetSceneByName("Game Scene").isLoaded)
        {
            // Load the scene asynchronously
            AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Game Scene");

            // Wait until the scene is fully loaded
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }

        // Invoke the event after the scene is loaded
        OnLevelLoaded?.Invoke(latestLevel);
    }

    public void LoadNextLevel()
    {
        currentLevelIndex++;
        //Debug.Log("(\"++++++++++++++++ LOAD NEXT LEVEL");

        SaveLevelIndex();
        LoadLevel();
    }

    public void IncreaseLevel()
    {
        currentLevelIndex++;
        SaveLevelIndex();
    }

    private void SaveLevelIndex()
    {
        ES3.Save(LEVEL_INDEX_PREF_KEY, currentLevelIndex);
    }

    public int GetCurrentLevel()
    {
        return ES3.Load(LEVEL_INDEX_PREF_KEY, 0);
    }
}