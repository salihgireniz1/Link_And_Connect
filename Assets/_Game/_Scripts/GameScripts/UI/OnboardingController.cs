using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OnboardingController : MonoBehaviour
{
    public GameObject nextButton;
    public GameObject startButton;
    public TextMeshProUGUI explanationTextHolder;
    public string lastExplanation;
    public Animator childAnimator;

    CanvasGroup group;

    private void Start()
    {
        group = gameObject.AddComponent<CanvasGroup>();
        GameManager.Instance.UpdateGameState(GameState.hold);
    }
    public void NextButtonClicked()
    {
        explanationTextHolder.text = lastExplanation;
        nextButton.SetActive(false);
        childAnimator.SetTrigger("Next");
        startButton.SetActive(true);
    }
    public void StartButtonClicked()
    {
        GameManager.Instance.UpdateGameState(GameState.playing);
        group.DOFade(0f, .25f)
            .OnComplete(() => Complete());
    }
    void Complete()
    {
        gameObject.SetActive(false);
    }
}
