using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BallSpawner : MonoSingleton<BallSpawner>
{
    [Header("Ball Settings"), Space]

    [SerializeField]
    private int ballPoolSize;

    [SerializeField]
    private GameObject ballPrefab;

    [SerializeField]
    private RectTransform ballsHolder;

    [SerializeField]
    private Canvas particleCanvas;

    [SerializeField]
    private Camera particleCamera;

    Queue<GameObject> ballPool = new();
    private void Awake()
    {
        ballPool = new Queue<GameObject>();

        for (int i = 0; i < ballPoolSize; i++)
        {
            GameObject willCreateCoin = Instantiate(ballPrefab, ballsHolder);
            willCreateCoin.SetActive(false);
            ballPool.Enqueue(willCreateCoin);
        }
    }
    public void SpawnBallPieceToAchivement(List<SpriteInfo> matches, Achivement achivement)
    {
        float totalProgress = ComboManager.Instance.GetComboScore(matches.Count);

        int matchCount = matches.Count * 2;
        float progressAmount = totalProgress / (float)matchCount;
        for (int i = 0; i < matchCount; i++)
        {
            GameObject obj = ballPool.Dequeue();
            BallPiece pieceInfo = obj.GetComponent<BallPiece>();

            pieceInfo.target = achivement;
            pieceInfo.canvas = particleCanvas;
            pieceInfo.progressAmount = progressAmount;

            RectTransform uiElementRectTransform = obj.GetComponent<RectTransform>();
            
            uiElementRectTransform.localPosition = AssingParticleCanvasPos(matches[matches.Count - 1].transform);

            obj.SetActive(true);
            ballPool.Enqueue(obj);
        }
    }
    public Vector2 AssingParticleCanvasPos(Transform worldObj)
    {
        // Get the screen position of the 3D object
        Vector3 objectScreenPos = Camera.main.WorldToScreenPoint(worldObj.position);

        // Convert the screen position to a canvas position
        Vector2 canvasPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(ballsHolder, objectScreenPos, particleCamera, out canvasPos);
        return canvasPos;
    }
    public Vector2 AssingParticleCanvasPos(Vector2 worldObj)
    {
        // Get the screen position of the 3D object
        Vector3 objectScreenPos = particleCamera.WorldToScreenPoint(worldObj);
        // Convert the screen position to a canvas position
        Vector2 canvasPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(ballsHolder, objectScreenPos, particleCamera, out canvasPos);
        return canvasPos;
    }
}
