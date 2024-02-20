using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelPanel : MonoBehaviour
{
    [SerializeField] private LevelPanelSettings _levelPanelSettings;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private Image _levelPanelImage;

    private void Awake()
    {
        _levelPanelImage.sprite = _levelPanelSettings.BackgroundSprite; 
    }
    private void Start()
    {
        RefreshLevelText();
    }

    private void RefreshLevelText()
    {
     //   _levelText.text = "Level" + " " + LevelManager.Instance.level.ToString();
    }

    public void SetPanelActive(bool value)
    {
        transform.gameObject.SetActive(value);
    }
}
