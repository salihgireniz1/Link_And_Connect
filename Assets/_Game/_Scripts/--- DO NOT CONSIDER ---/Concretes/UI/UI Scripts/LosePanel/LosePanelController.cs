using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LosePanelController : MonoBehaviour
{
    [SerializeField] private LosePanelSettings _settings;
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private Image _losePanelImage;
    [SerializeField] private Image _loseButtonImage;
    [SerializeField] private Button _loseButton;
    private ButtonAnimation _loseButtonAnimation;

    private void Awake()
    {
        _backgroundImage.sprite = _settings.BackgroundSprite;
        _losePanelImage.sprite = _settings.LoseSprite;
        _loseButtonImage.sprite = _settings.ButtonSprite;
        _loseButtonAnimation = new ButtonAnimation(_loseButton);
    }

    private void OnEnable()
    {
        _loseButtonAnimation.ShowButtonWithGrowingAnimation(1f, DG.Tweening.Ease.Linear);
    }

    private void OnDisable()
    {
        _loseButtonAnimation.Reset();
    }

    private void Start()
    {
        _loseButton.onClick.AddListener(LoseButtonClicked);
    }

    private void LoseButtonClicked()
    {
        LevelManager.Instance.LoadLevel();
        ClosePanel();
    }

    public void OpenPanel()
    {
        gameObject.SetActive(true);
    }

    private void ClosePanel()
    {
        gameObject.SetActive(false);

    }
}
