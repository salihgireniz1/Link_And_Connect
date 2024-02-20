using DG.Tweening;
using MoreMountains.Feedbacks;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    #region Events
    public static event Action<int, bool> OnHealthChanged;
    #endregion

    #region Properties
    public static HealthManager Instance;
    public int HealthCount
    {
        get
        {
            healthCount = ES3.Load("Health", 5);
            return healthCount;
        }
        set
        {
            healthCount = value;
            isFull = healthCount >= maxHealthCount ? true : false;
            /*if (isFull)
            {
                healthCount = maxHealthCount;
                if (healthText != null)
                {
                    healthText.text = maxHealthCount.ToString();
                }
                return;
            }*/
            if (isFull)
            {
                healthCount = maxHealthCount;
                Debug.Log("Health fulled!");
                if (counterText != null)
                {
                    counterText.text = "Full";
                }
            }

            ES3.Save("Health", healthCount);

            OnHealthChanged?.Invoke(healthCount, isFull);
            if (healthText != null)
            {
                healthText.text = healthCount.ToString();
            }
            /*if (isFull)
            {
                Debug.Log("Health fulled!");
                if (counterText != null)
                {
                    counterText.text = "Full";
                }
            }*/
        }
    }
    #endregion

    #region Variables
    [SerializeField] private int healthCount;

    [SerializeField] private float healthEarnIntervalInSeconds;

    [SerializeField] private int maxHealthCount = 5;

    [SerializeField] private float countTime = 0f;

    public TextMeshProUGUI counterText;
    public TextMeshProUGUI healthText;

    [SerializeField] private bool isFull;
    #endregion

    #region Monobehaviour Callbacks
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

        OfflineTimer.OnPlayerOnline += EarnOfflineHearts;
    }
    private void OnDisable()
    {
        OfflineTimer.OnPlayerOnline -= EarnOfflineHearts;
        ES3.Save("LastCounter", countTime);
    }
    private void Awake()
    {
        ResetCounter();
    }
    private void Update()
    {
        if (!isFull)
        {
            OnlineCounter();
        }
    }
    #endregion

    #region Methods
    public bool CanStartLevel()
    {
        if (HealthCount >= 1)
        {
            HealthCount -= 1;
            return true;
        }
        else
        {
            Debug.Log("Not Enough Health!");
            return false;
        }
    }
    public void EarnOnlineHealth(int healthCount)
    {
        HealthCount += healthCount;
    }
    public void EarnOfflineHearts(float offlineDuration)
    {
        int passedIntervalCount = Mathf.FloorToInt(offlineDuration / healthEarnIntervalInSeconds);

        float recordedCountTime = ES3.Load("LastCounter", healthEarnIntervalInSeconds + 1);
        countTime = recordedCountTime - (offlineDuration % healthEarnIntervalInSeconds);
        Debug.Log($"Last CT: {recordedCountTime} ---- Offline For: {offlineDuration} ---- New countTime: {countTime}.");
        if (countTime < 1f)
        {
            countTime = healthEarnIntervalInSeconds + countTime;
            passedIntervalCount += 1;
        }

        // We can have maxHealthCount amount of hearth. 
        // So we need to find difference between max and current health (i.e 5 - 3 = 2).
        // That means we can earn heart of difference (i.e. We can earn 2 hearths to reach max amount).
        int offlineEarnedHearts = Mathf.Min(passedIntervalCount, maxHealthCount - HealthCount);
        HealthCount += offlineEarnedHearts;
        Debug.Log("PLAYER EARNED " + offlineEarnedHearts + " OFFLINE HEALTH!");
    }
    public void ResetCounter()
    {
        countTime = healthEarnIntervalInSeconds + 1;
    }
    int min;
    int sec;
    public void OnlineCounter()
    {
        countTime -= Time.deltaTime;

        // Calculate minutes and seconds
        int minutes = Mathf.FloorToInt(countTime / 60f);
        int seconds = Mathf.FloorToInt(countTime % 60f);

        if (min != minutes || sec != seconds)
        {
            min = minutes;
            sec = seconds;

            // Format the counter text
            string counterTextFormatted = string.Format("{0:00}:{1:00}", minutes, seconds);

            // Update the counter text UI
            if (counterText != null)
            {
                counterText.text = counterTextFormatted;
            }

            if (minutes == 0 && seconds == 0)
            {
                Debug.Log("1 HEALTH EARNED");
                EarnOnlineHealth(1);
                countTime = healthEarnIntervalInSeconds + 1;
            }
        }
    }

    #endregion
}
