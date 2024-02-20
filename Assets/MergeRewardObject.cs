using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MergeRewardObject : MonoBehaviour
{
    public bool IsCollected { get; private set; } = true;
    public Sprite[] Images;
    Camera uiCam;
    public int myPrice = 1;
    public SpriteRenderer myImage;
    private void OnEnable()
    {
        uiCam = GameObject.FindGameObjectWithTag("UI_Camera").GetComponent<Camera>();
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one * 0.5f, .25f)
            .SetEase(Ease.OutBack);
    }
    public void SetReward(int combo)
    {
        myPrice = combo;
        myImage.sprite = Images[combo - 2];
    }
    public void GetMergeReward()
    {
        if (!IsCollected) return;
        IsCollected = true;
        GetComponent<BoxCollider2D>().enabled = false;
        Vector3 spawnPos = BallSpawner.Instance.AssingParticleCanvasPos(transform);
        Debug.Log("SPAWNPOS: " + spawnPos);
        MoneyManager.Instance.SpawnMoneyToSection(spawnPos, myPrice, 1);

        transform.DOScale(Vector3.zero, .25f)
            .SetEase(Ease.InBack)
            .OnComplete(CollectAndDestroy);
    }

    void CollectAndDestroy()
    {
        Debug.Log("EARNED REWARD!");
        Destroy(gameObject);
    }
}
