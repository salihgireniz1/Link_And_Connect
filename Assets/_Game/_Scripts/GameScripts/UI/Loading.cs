using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Loading : MonoBehaviour
{
    public float appearTime = 1f;
    public float disappearTime = 0.5f;

    [SerializeField]
    private GameObject onboarding;

    CanvasGroup group;
    
    private void Start()
    {
        Activate();
    }
    IEnumerator LoadingRoutine()
    {
        yield return new WaitForSeconds(appearTime); 
        
        if (LevelManager.Instance.FirstStart)
        {
            onboarding.SetActive(true);
            //LevelManager.Instance.FirstStart = false;
            //HintManager.Instance.GetFirstPossibleMatch();
        }
        else
        {
            GameManager.Instance.UpdateGameState(GameState.playing);
        }
        group.DOFade(0f, disappearTime)
            .OnComplete(() => Complete());
    }
    void Complete()
    {
        gameObject.SetActive(false);
    }
    public void Activate()
    {
        if(!TryGetComponent(out group))
        {
            group = gameObject.AddComponent<CanvasGroup>();
        }
        group.alpha = 1f;
        gameObject.SetActive(true);
        StartCoroutine(LoadingRoutine());
        GameManager.Instance.UpdateGameState(GameState.hold);
    }
}