using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboManager : MonoSingleton<ComboManager>
{
    public static event Action<int> OnCombo;
    public GameObject comboFloatingText;
    public Transform textHolderUI;
    public float[] scores;
    public Color32[] colors;
    public Camera uiCamera;
    public GameObject fireworks;
    public Dictionary<int, float> comboScoreDict = new();
    public Dictionary<int, Color32> comboColorDict = new();
    private void OnEnable()
    {
        LevelManager.OnLevelLoaded += Initialize;
    }
    private void OnDisable()
    {
        LevelManager.OnLevelLoaded -= Initialize;
    }
    public void Initialize(Level level)
    {
        if(scores == null) return;
        comboScoreDict = new();
        comboColorDict = new();
        for (int i = 0; i < scores.Length; i++)
        {
            comboScoreDict.Add(i + 2, scores[i]);
            comboColorDict.Add(i + 2, colors[i]);
        }
    }
    public float GetComboScore(int matchCount)
    {
        float comboScore = 0f;
        OnCombo?.Invoke(matchCount);
        if (comboScoreDict.ContainsKey(matchCount))
        {
            comboScore = comboScoreDict[matchCount];
        }
        return comboScore;
    }
    public Color32 GetComboColor(int matchCount)
    {
        Color32 comboColor = Color.green;
        if (comboColorDict.ContainsKey(matchCount))
        {
            comboColor = comboColorDict[matchCount];
        }
        else
        {
            comboColor = comboColorDict[comboColorDict.Count - 1];
        }
        return comboColor;
    }
    public void SpawnComboText(List<SpriteInfo> mergedSprites)
    {
        Color32 comboColor = GetComboColor(mergedSprites.Count);
        Vector3 comboPosition = mergedSprites[mergedSprites.Count - 1].transform.position;
        Vector2 uiPos = uiCamera.WorldToScreenPoint(comboPosition);
        
        // Calculate the size of the combo text object
        RectTransform comboRectTransform = comboFloatingText.transform.GetChild(0).GetComponent<RectTransform>();
        Vector2 comboSize = comboRectTransform.rect.size;

        // Calculate the offset to keep the combo text in the screen
        Vector2 offset = Vector2.zero;
        if (uiPos.x < comboSize.x / 2) // Left edge
        {
            offset.x = comboSize.x / 2  - uiPos.x;
        }
        else if (uiPos.x > Screen.width - comboSize.x / 2) // Right edge
        {
            offset.x = Screen.width - comboSize.x / 2 - uiPos.x;
        }
        if (uiPos.y < comboSize.y / 2) // Bottom edge
        {
            offset.y = comboSize.y / 2 - uiPos.y;
        }
        else if (uiPos.y > Screen.height - comboSize.y / 2) // Top edge
        {
            offset.y = Screen.height - comboSize.y / 2 - uiPos.y;
        }

        // Apply the offset to the spawn point
        Vector2 adjustedUiPos = uiPos + offset * 2f;

        GameObject comboObj = Instantiate(comboFloatingText, adjustedUiPos, Quaternion.identity, textHolderUI);
        comboObj.GetComponent<ComboText>().Initialize(mergedSprites.Count, comboColor);
        if(mergedSprites.Count == 6)
        {
            Instantiate(fireworks, comboPosition, Quaternion.identity);
        }
    }

}
