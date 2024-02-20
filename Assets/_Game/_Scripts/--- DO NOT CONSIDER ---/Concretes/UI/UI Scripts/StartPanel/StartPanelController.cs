using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartPanelController : MonoBehaviour
{
    [SerializeField] private StartPanelSettings _settings;
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private Button _startButton;

    private void Start()
    {
        _startButton.onClick.AddListener(StartButtonClicked);
        _backgroundImage.sprite = _settings.Background;
    }

    private void StartButtonClicked()
    {
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
