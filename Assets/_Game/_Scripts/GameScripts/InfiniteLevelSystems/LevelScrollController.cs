using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System;
using UnityEngine.Events;

public class LevelScrollController : MonoBehaviour
{

    public int activeSlotCount = 9; // Tek sayı yapmaya çalışalım.
    public int centerIndex = 4;

    [Header("Cache"), Space]
    [SerializeField] ScrollRect _scrollRect;
    [SerializeField] Slot _levelSlot;
    [SerializeField] ParticleSystem _completeLevelParticle;
    [SerializeField] Camera _uiCamera;
    [SerializeField] Button _levelStartButton;


    RectTransform _contentRectTransform;
    List<Slot> _levelSlots = new List<Slot>();
    TextMeshProUGUI _levelStartButtonText;
    Vector3 _buttonScale;

    private void OnEnable()
    {
        HealthManager.OnHealthChanged += HandleLevelButtonText;
    }
    private void OnDisable()
    {
        HealthManager.OnHealthChanged -= HandleLevelButtonText;
    }
    private void Awake()
    {
        _contentRectTransform = _scrollRect.content;
        _levelStartButtonText = _levelStartButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        _buttonScale = _levelStartButton.transform.localScale;
    }

    void Start()
    {
        loadingPanel = GameObject.FindGameObjectWithTag("LoadingPanel");
        CloseLevelStartButton();
        StartCoroutine(InitializeLevels());
    }

    IEnumerator InitializeLevels()
    {
        SpawnLevelSlots();
        int currentLevelIndex = LevelManager.Instance.GetCurrentLevel();
        Slot levelSlot;
        if(currentLevelIndex > 0)
        {
            for (int i = 0; i < centerIndex - missingFromBottom; i++)
            {
                levelSlot = _levelSlots[i];
                levelSlot.ClearSelectedFrame();
                levelSlot.SetBackgroundColor(Color.blue);
                levelSlot.MakeSelected();
                levelSlot.FillColoredLine();
            }
            yield return new WaitForEndOfFrame();

            levelSlot = _levelSlots[currentLevelIndex - 1]; // Last Passed Level Slot.
            RectTransform targetRectTransform = levelSlot.GetComponent<RectTransform>();
            Vector3 targetPos = new Vector2(0, targetRectTransform.anchoredPosition.y + (Screen.height * 0.5f));
            _contentRectTransform.anchoredPosition = -targetPos;
            PlayParticleOnUI(levelSlot);
            levelSlot.FillColoredLine();
        }

        yield return new WaitForSeconds(1.2f);

        levelSlot = _levelSlots[centerIndex - missingFromBottom]; // Next Level Slot.
        MoveScrollTo(levelSlot, (() =>
        {
            levelSlot.MakeSelected();

        }));

        OpenLevelStartButton();
        HandleLevelButtonText(HealthManager.Instance.HealthCount);

        loadingPanel.transform.localScale = Vector3.zero;
    }
    int missingFromBottom = 0;
    string levelText;
    public GameObject loadingPanel;
    public void HandleLevelButtonText(int health, bool isFull = false)
    {
        if (health >= 1)
        {
            _levelStartButton.interactable = true;
            levelText = "LEVEL" + " " + (LevelManager.Instance.GetCurrentLevel() + 1).ToString();
            UpdateLevelButton(ClickLevelStartButton, levelText);
            //UpdateButton(LevelManager.Instance.GetCurrentLevel());
        }
        else
        {
            if(String.IsNullOrEmpty(levelText))
                levelText = "LEVEL" + " " + (LevelManager.Instance.GetCurrentLevel() + 1).ToString();
            // It will show pop-up.
            UpdateLevelButton(ShowAdPopUp, levelText);
        }
    }
    public EnergyAdPanel energyPanel;
    void ShowAdPopUp()
    {
        energyPanel.ShowPanel();
    }
    void SpawnLevelSlots()
    {
        int currentLevelIndex = LevelManager.Instance.GetCurrentLevel();
        for (int i = 0; i < activeSlotCount + missingFromBottom; i++)
        {
            int distance = centerIndex - i;
            int myLevelIndex = currentLevelIndex - distance;
            myLevelIndex += 1;

            if (myLevelIndex <= 0)
            {
                missingFromBottom += 1;
                continue;
            }

            Slot levelSlot = Instantiate(_levelSlot);
            levelSlot.transform.SetParent(_scrollRect.content);
            levelSlot.transform.localPosition = new Vector3(
                levelSlot.transform.localPosition.x,
                levelSlot.transform.localPosition.y,
                0f);

            levelSlot.transform.localScale = Vector3.one;
            /*Level level = LevelManager.Instance.GetLevelInfo(i);
            levelSlot.SetLevelData(level, levelValueIndex);
            _levelSlots.Add(levelSlot);*/


            Level level = LevelManager.Instance.GetLevelInfo(i);
            levelSlot.SetLevelData(level, myLevelIndex);
            _levelSlots.Add(levelSlot);
        }
    }

    void PlayParticleOnUI(Slot levelSlot)
    {
        RectTransform uiRectTransform = levelSlot.GetComponent<RectTransform>(); // UI nesnesinin RectTransform bileşeni
        Vector3 uiPosition = uiRectTransform.position; // UI nesnesinin dünya koordinatlarındaki pozisyonu

        if(_uiCamera == null)
        {
            _uiCamera = GameObject.FindGameObjectWithTag("UI_Camera").GetComponent<Camera>();
        }

        // UI nesnesinin dünya koordinatlarını viewport koordinatlarına dönüştürme
        Vector3 viewportPosition = _uiCamera.WorldToViewportPoint(uiPosition);

        // Viewport koordinatlarını screen koordinatlarına dönüştürme
        Vector3 screenPosition = new Vector3(viewportPosition.x * Screen.width, viewportPosition.y * Screen.height, viewportPosition.z);

        // Screen Space - Camera Canvas'in dünya koordinatlarındaki pozisyonu
        Vector3 canvasWorldPosition = GetComponent<RectTransform>().position;

        // Screen Space - Camera Canvas'in dünya koordinatlarındaki pozisyonunu screen koordinatlarına dönüştürme
        Vector3 canvasScreenPosition = _uiCamera.WorldToScreenPoint(canvasWorldPosition);

        // Particle System'a aktarılacak olan screen koordinatları
        Vector3 particleScreenPosition = new Vector3(screenPosition.x, screenPosition.y, canvasScreenPosition.z);

        // Particle System'a screen koordinatlarını dünya koordinatlarına dönüştürme
        Vector3 particleWorldPosition = _uiCamera.ScreenToWorldPoint(particleScreenPosition);

        // Particle System nesnesinin pozisyonunu güncelleme
        //_completeLevelParticle.transform.position = particleWorldPosition;
        //_completeLevelParticle.Play();
    }

    void MoveScrollTo(Slot levelSlot, System.Action onComplete = null)
    {
        Slot slot = levelSlot;
        RectTransform targetRectTransform = slot.GetComponent<RectTransform>();
        Vector3 targetPos = new Vector2(0, targetRectTransform.anchoredPosition.y + (Screen.height * 0.325f));
        _contentRectTransform.DOAnchorPos(-targetPos, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
        {
            onComplete?.Invoke();
        });
    }
    void UpdateLevelButton(UnityAction buttonAction, string buttonExp)
    {
        _levelStartButton.onClick.RemoveAllListeners();
        _levelStartButton.onClick.AddListener(buttonAction);
        _levelStartButtonText.text = buttonExp;
    }
    void UpdateButton(int currentLevelIndex)
    {
        _levelStartButton.onClick.RemoveAllListeners();
        _levelStartButton.onClick.AddListener(ClickLevelStartButton);
        _levelStartButtonText.text = "LEVEL" + " " + (currentLevelIndex + 1).ToString();
    }

    void ClickLevelStartButton()
    {
        if (HealthManager.Instance.CanStartLevel())
        {
            loadingPanel.transform.localScale = Vector3.one;
            LevelManager.Instance.LoadLevel();
        }
    }

    void OpenLevelStartButton()
    {
        _levelStartButton.interactable = true;
        _levelStartButton.gameObject.SetActive(true);
        _levelStartButton.transform.DOScale(_buttonScale, 0.5f).SetEase(Ease.InOutQuad);
    }

    void CloseLevelStartButton()
    {
        _levelStartButton.interactable = false;
        _levelStartButton.gameObject.SetActive(false);
        _levelStartButton.transform.localScale = Vector3.zero;
    }
}
