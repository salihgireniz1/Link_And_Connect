using System;
using UnityEngine;

public class OfflineTimer : MonoBehaviour
{
    public static OfflineTimer Instance;
    public static event Action<float> OnPlayerOnline;

    DateTime lastEarnDate = DateTime.Now; 
    private void OnEnable()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }
    private void OnDisable()
    {
        SaveData();
    }
    private void Start()
    {
        float offlineDuration = CalculateAFKTime();
        Debug.Log("PLAYER WERE OFFLINE FOR " + offlineDuration.ToString("f0") + " SECONDS.");
        OnPlayerOnline?.Invoke(offlineDuration);
    }
    private void OnApplicationFocus(bool focus)
    {
        if (focus == true)
        {
            LoadData();
        }
        else
        {
            SaveData();
        }
    }
    float CalculateAFKTime()
    {
        return (float)(DateTime.Now - lastEarnDate).TotalSeconds;
    }
    void LoadData()
    {
        lastEarnDate = ES3.Load("Last_Date", DateTime.Now);
    }
    void SaveData()
    {
        ES3.Save("Last_Date", DateTime.Now);
    }
}
