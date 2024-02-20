using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class BallPiece : MonoBehaviour
{
    public Achivement target;
    public Canvas canvas;
    public float progressAmount;
    public float spreadTime = 0.7f;
    public float goingUpTime = 0.8f;
    public float spreadTolerance = 250.0f;
    public float rotatingTolerance = 120.0f;
    private Sequence MoneySequence;
    private void Awake()
    {
        DOTween.SetTweensCapacity(2000, 250);
    }
    private void OnEnable()
    {
        if (target != null) MoveBallsToAchievement();
    }
    [Button]
    public void MoveBallsToAchievement()
    {
        MoneySequence = DOTween.Sequence();
        Vector3 randomPos = new Vector3
            (
                Random.Range(-spreadTolerance, spreadTolerance), 
                Random.Range(-spreadTolerance, spreadTolerance), 
                0f
            );

        // Get the position of the canvas in world space
        Vector3 canvasPos = canvas.transform.position;

        // Get the local position of the UI element
        Vector3 uiLocalPos = transform.localPosition;
        uiLocalPos += randomPos;

        // Combine the canvas position and the local position to get the world position of the UI element
        Vector3 uiWorldPos = canvasPos + uiLocalPos;
        float randomizer = Random.Range(.75f, 1.25f);
        float randomDuration = spreadTime * randomizer;
        MoneySequence
            .Append(transform.DOLocalMove(uiWorldPos, randomDuration)).SetEase(Ease.OutSine)
            .Join(transform.DORotate(Vector3.forward * Random.Range(-rotatingTolerance, rotatingTolerance), randomDuration))
            .Append(transform.DOMove(target.transform.position, randomDuration)).SetEase(Ease.InSine)
            .Join(transform.DORotate(Vector3.zero, randomDuration))
            .OnComplete(() => CompleteSequence());
    }
    void CompleteSequence()
    {
        if(target != null)
        {
            AchivementManager.Instance.ProgressAchivement(target, progressAmount);
        }
        int activeBallCount = 0;
        foreach (Transform child in transform.parent)
        {
            if (child.gameObject.activeInHierarchy) activeBallCount += 1;
        }

        gameObject.SetActive(false);
    }
}