using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinPanelController : MonoBehaviour
{
    public event System.Action OnOpenWinPanel;
    [SerializeField] private WinPanelSettings _settings;
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private Image _winPanelImage;
    [SerializeField] private Image _nextButtonImage;
    [SerializeField] private Button _nextButton;
    private ButtonAnimation _nextButtonAnimation;

    private void Awake()
    {
        _backgroundImage.sprite = _settings.BackgroundSprite;
        _winPanelImage.sprite = _settings.WinSprite;
        _nextButtonImage.sprite = _settings.ButtonSprite;
        _nextButtonAnimation = new ButtonAnimation(_nextButton);
    }

    private void OnEnable()
    {
        _nextButtonAnimation.ShowButtonWithGrowingAnimation(1f, DG.Tweening.Ease.Linear);
    }

    private void OnDisable()
    {
        _nextButtonAnimation.Reset();
    }

    private void Start()
    {
        //LevelManager.Instance.SaveLevel();
        _nextButton.onClick.AddListener(NextButtonClicked);
    }

    private void NextButtonClicked()
    {
        //LevelManager.Instance.LoadNextLevel();
        ClosePanel();
    }

    public void OpenPanel()
    {
        gameObject.SetActive(true);
        OnOpenWinPanel?.Invoke();
    }

    private void ClosePanel()
    {
        gameObject.SetActive(false);
    }


}
