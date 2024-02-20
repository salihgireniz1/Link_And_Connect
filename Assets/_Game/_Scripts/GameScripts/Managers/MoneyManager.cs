using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class MoneyManager : MonoSingleton<MoneyManager>
{
    public static event Action<int> MoneyChanged;
    public static event Action<int> OnMoneyEarned;
    public static int inLevelEarned;
    public int Money
    {
        get => ES3.Load("Total_Money", 0);
        set
        {
            ES3.Save("Total_Money", value);
            MoneyChanged?.Invoke(value);
        }
    }

    [Header("Money UI Settings"), Space]

    [SerializeField]
    private int moneyPoolSize;

    [SerializeField]
    private GameObject moneyPrefab;

    [SerializeField]
    private RectTransform poolHolder;

    [SerializeField]
    private Canvas particleCanvas;

    [SerializeField]
    private Camera particleCamera;

    [SerializeField]
    private Transform target;

    [Header("Money Earning Settings"), Space]
    [SerializeField]
    private TextMeshProUGUI moneyText;

    Queue<GameObject> moneyPool = new();
    public int levelMoney;
    public float delayedCompleteDuration = 1f;
    private void Awake()
    {
        InitMoneyPool();
        GameManager.OnGameStateChanged += ResetInLevelEarned;
    }
    private void Start()
    {
        moneyText.text = Money.ToString();
        if (LevelManager.Instance.lastLevelEndMoney != 0)
        {
            StartCoroutine(LevelEndEarn());
        }
    }
    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= ResetInLevelEarned;
    }
    void ResetInLevelEarned(GameState state)
    {
        switch (state)
        {
            case GameState.loaded:
                inLevelEarned = 0;
                break;
            default:
                break;
        }
    }
    IEnumerator LevelEndEarn()
    {
        int money = LevelManager.Instance.lastLevelEndMoney;
        LevelManager.Instance.lastLevelEndMoney = 0;
        Debug.Log("MONEY: " + money);
        yield return new WaitForSeconds(delayedCompleteDuration);
        SpawnMoneyToSection(Vector2.zero, money, 10);
    }
    void InitMoneyPool()
    {
        moneyPool = new Queue<GameObject>();

        for (int i = 0; i < moneyPoolSize; i++)
        {
            GameObject money = Instantiate(moneyPrefab, poolHolder);
            money.SetActive(false);
            moneyPool.Enqueue(money);
        }
    }
    public int moneyMultiplier;
    public void SpawnForWinPanel(Transform spawnPos, Transform targetPos)
    {
        StartCoroutine(SpawnAndWait(spawnPos, targetPos));
    }
    IEnumerator SpawnAndWait(Transform spawnPos, Transform targetPos)
    {
        int leftMoves = MoveManager.Instance.MoveCount * moneyMultiplier;
        LevelManager.Instance.lastLevelEndMoney = leftMoves;
        int progressAmount = 1;
        float duration = delayedCompleteDuration / leftMoves;

        for (int i = 0; i < leftMoves; i++)
        {
            GameObject obj = moneyPool.Dequeue();
            MoneyPiece pieceInfo = obj.GetComponent<MoneyPiece>();

            pieceInfo.target = targetPos;
            pieceInfo.canvas = particleCanvas;
            pieceInfo.progressAmount = progressAmount;
            pieceInfo.canEarnMoney = false;

            obj.transform.position = spawnPos.position;
            obj.SetActive(true);
            moneyPool.Enqueue(obj);
            if (i % moneyMultiplier == 0)
            {
                MoveManager.Instance.DecreaseMoveCount();
            }
            yield return new WaitForSeconds(duration);
        }
    }
    [Button]
    public void SpawnMoneyToSection(Vector2 uiPosition, int price, int amount)
    {
        int moneyObjectCount = amount;
        int progressAmount = price / amount;
        Debug.Log("Progress: " + progressAmount);
        for (int i = 0; i < moneyObjectCount; i++)
        {
            GameObject obj = moneyPool.Dequeue();
            MoneyPiece pieceInfo = obj.GetComponent<MoneyPiece>();

            pieceInfo.canEarnMoney = true;
            pieceInfo.target = target;
            pieceInfo.canvas = particleCanvas;
            pieceInfo.progressAmount = progressAmount;

            obj.transform.position = uiPosition;
            obj.SetActive(true);
            moneyPool.Enqueue(obj);
        }
    }

    public void SpawnMoneyToSection(Vector3 uiPosition, int price, int amount)
    {
        int moneyObjectCount = amount;
        int progressAmount = price / amount;
        Debug.Log("Progress: " + progressAmount);
        for (int i = 0; i < moneyObjectCount; i++)
        {
            GameObject obj = moneyPool.Dequeue();
            MoneyPiece pieceInfo = obj.GetComponent<MoneyPiece>();

            pieceInfo.canEarnMoney = true;
            pieceInfo.target = target;
            pieceInfo.canvas = particleCanvas;
            pieceInfo.progressAmount = progressAmount;

            obj.GetComponent<RectTransform>().anchoredPosition3D = uiPosition;
            obj.SetActive(true);
            moneyPool.Enqueue(obj);
        }
    }
    Tween moneyTw;
    [Button]
    public void EarnMoney(int amount)
    {
        Money += amount;
        inLevelEarned += amount;
        OnMoneyEarned?.Invoke(amount);
        moneyText.text = Money.ToString();
        if (moneyTw != null && moneyTw.IsPlaying()) moneyTw.Kill();
        moneyText.transform.localScale = Vector2.one;
        moneyTw = moneyText.transform.DOPunchScale(1.01f * Vector2.one, .25f).OnComplete(() => moneyText.transform.localScale = Vector2.one);
        moneyTw.Play();
    }
    public void SpendMoney(int amount)
    {
        Money -= amount;
        OnMoneyEarned?.Invoke(-amount);
        moneyText.text = Money.ToString();
        if (moneyTw != null && moneyTw.IsPlaying()) moneyTw.Kill();
        moneyText.transform.localScale = Vector2.one;
        moneyTw = moneyText.transform.DOPunchScale(1.01f * Vector2.one, .25f).OnComplete(() => moneyText.transform.localScale = Vector2.one);
        moneyTw.Play();
    }
    public bool CanSpendMoney(int amount)
    {
        return amount <= Money;
    }
}