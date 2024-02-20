using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DebugPanel : MonoBehaviour
{
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private Button _debugButton;
    private int _inputLevel;

    private void Start()
    {
        _debugButton.onClick.AddListener(LoadLevel);
    }

    public void LoadLevel()
    {
        _inputLevel = int.Parse(_inputField.text);
        //LevelManager.Instance.LoadLevel(_inputLevel);
    }

    public void SetPanelActive(bool value)
    {
        gameObject.SetActive(value);
    }
}
