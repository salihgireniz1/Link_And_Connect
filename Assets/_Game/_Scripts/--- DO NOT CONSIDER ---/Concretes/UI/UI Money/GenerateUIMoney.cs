using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenerateUIMoney : MonoBehaviour
{
    [HideInInspector] public Queue<GameObject> moneyInPool;
    [SerializeField] private GameObject _moneyPrefab;
    [SerializeField] private int _poolSize;
    [SerializeField] private ScorePanel _scorePanel;

    [SerializeField] private int multipleCoinSize = 6;
    private int moneyAmount;

    private void OnEnable()
    {
        EventManager.OnCollectMoneyUI += GenerateCoin;
    }
    private void OnDisable()
    {
        EventManager.OnCollectMoneyUI -= GenerateCoin;
    }

    private void Awake()
    {
        moneyInPool = new Queue<GameObject>();
        //_moneyPrefab.GetComponent<Image>().sprite = _scorePanel.Settings.CollectibleSprite;

        for (int i = 0; i < _poolSize; i++)
        {
            GameObject willCreateCoin = Instantiate(_moneyPrefab, transform);
            willCreateCoin.SetActive(false);
            moneyInPool.Enqueue(willCreateCoin);
        }
    }
    public void SpawnBallPiecesToAchivement(Vector3 generatePoint, Achivement achivement)
    {
        for (int i = 0; i < multipleCoinSize; i++)
        {
            GameObject obj = moneyInPool.Dequeue();
            obj.GetComponent<BallPiece>().target = achivement;
            obj.SetActive(true);
            obj.transform.position = Camera.main.WorldToScreenPoint(generatePoint);
            moneyInPool.Enqueue(obj);
        }
    }
    public void GenerateCoin(Vector3 generatePoint, bool multiple)
    {
        if (multiple)
        {
            moneyAmount = multipleCoinSize;
        }
        else
        {
            moneyAmount = 1;
        }

        for (int i = 0; i < moneyAmount; i++)
        {
            GameObject obj = moneyInPool.Dequeue();
            obj.SetActive(true);
            obj.transform.position = Camera.main.WorldToScreenPoint(generatePoint);
            moneyInPool.Enqueue(obj);
        }
    }
}
