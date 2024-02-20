using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class Slot : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _levelText;
    [SerializeField] Image _frameImage;
    [SerializeField] Image _backgroundImage;
    [SerializeField] Image _selectedFrameImage;
    [SerializeField] Image _coloredLine;

    int _levelIndex;
    public int LevelIndex { get => _levelIndex; }

    Level _level = new Level();
    public Level Level { get => _level; set => _level = value; }


    void SetLevelIndex(int levelIndex)
    {
        _levelIndex = levelIndex;
        _levelText.text = _levelIndex.ToString();
    }

    public void SetBackgroundColor(Color color)
    {
        _backgroundImage.DOColor(color, 0.3f).SetEase(Ease.Linear);
    }

    void SetFrameColor(Color color)
    {
        _frameImage.DOColor(color, 0.3f).SetEase(Ease.Linear); 
    }

    public void SetLevelData(Level level, int levelIndex)
    {
        _level = level;
        SetLevelIndex(levelIndex);   
    }

    public void MakeSelected()
    {
        StartCoroutine(FillSelectedFrameCoroutine());
    }

    IEnumerator FillSelectedFrameCoroutine()
    {
        float value = 0;

        while (value < 1)
        {
            _selectedFrameImage.fillAmount = value;
            value += Time.deltaTime;
            yield return null;
        }

        _selectedFrameImage.fillAmount = 1f;
        SetBackgroundColor(Color.green);

    }

    public void ClearSelectedFrame()
    {
        _selectedFrameImage.fillAmount = 0;
    }

    public void FillColoredLine()
    {
        StartCoroutine(FillColoredLineCoroutine());
    }

    IEnumerator FillColoredLineCoroutine()
    {
        float value = 0;

        while (value < 1)
        {
            _coloredLine.fillAmount = value;
            value += Time.deltaTime;
            yield return null;
        }
        _selectedFrameImage.fillAmount = 1f;
    }


}
