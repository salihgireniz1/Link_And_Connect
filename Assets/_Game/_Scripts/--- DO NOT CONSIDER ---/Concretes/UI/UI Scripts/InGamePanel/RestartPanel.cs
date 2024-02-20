using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RestartPanel : MonoBehaviour
{
    [SerializeField] private RestartPanelSettings _settings;
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private Button _button;
    [SerializeField] private Image _buttonImage;

    private void Awake()
    {
        _backgroundImage.sprite = _settings.BackgroundSprite;
        _buttonImage.sprite = _settings.ButtonSprite;
    }

    private void Start()
    {
        _button.onClick.AddListener(RestartButtonClicked);
    }


    private void RestartButtonClicked()
    {
        LevelManager.Instance.LoadLevel();
    }

    public void SetPanelActive(bool value)
    {
        transform.gameObject.SetActive(value);
    }

}
