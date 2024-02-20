using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoSingleton<MenuController>
{
    public List<ContentData> menuItems = new List<ContentData>();

    public Content contentObj = null;
    public Transform contentHolder;
    public Sprite lockSprite;

    public Transform panelHolder;

    public Camera _uiCamera;

    public Dictionary<Content, ContentPanel> contentAndPanelDict = new Dictionary<Content, ContentPanel>();

    List<Content> contents = new List<Content>();
    RectTransform choiceBackground;
    Content currentActiveContent = null;
    Tween positionTween;
    Animator animator;

    private void OnEnable()
    {
        EditRoomButton.OnEditingRoom += RoomEditingMode;
    }

    private void OnDisable()
    {
        EditRoomButton.OnEditingRoom -= RoomEditingMode;
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        Initialize();
    }

    public void Initialize()
    {
        SpawnContents();
        StartCoroutine(SpawnContentsAsync());
    }

    void SpawnContents()
    {
        contentAndPanelDict.Clear();
        for (int i = 0; i < menuItems.Count; i++)
        {
            InitializeContent(i);
        }
    }
    void InitializeContent(int i)
    {
        var content = Instantiate(contentObj, contentHolder) as Content;
        content.gameObject.name = menuItems[i].header;
        content.FillContent(menuItems[i]);
        contents.Add(content);

        int width = Screen.width;
        if(menuItems[i].contentPanel != null)
        {
            var contentPanel = Instantiate(menuItems[i].contentPanel, panelHolder) as ContentPanel;
            contentPanel.GetComponent<RectTransform>().anchoredPosition = Vector3.right * width * i;
            contentAndPanelDict.Add(content, contentPanel);
        }
    }

    public IEnumerator SpawnContentsAsync()
    {
        yield return new WaitForSeconds(.25F);
        float swiftOffset = 150f / contents.Count;
        int middleIndex = Mathf.RoundToInt((contents.Count - 1) / 2);

        foreach (var item in contents)
        {
            int indexDifference = middleIndex - contents.IndexOf(item);
            float positionOffset = swiftOffset * indexDifference;
            Vector2 center = item.transform.position + new Vector3(positionOffset, 0);
            item.CenterPos = center;
        }
        SelectContent(contents[middleIndex]);
    }
    void ResizeBackground()
    {
        Vector2 contentWidth = new Vector2(Screen.width / contents.Count, 0f);
        Vector2 extraWidth = new Vector2(75f, 0f);
        choiceBackground.sizeDelta = contentWidth + extraWidth;
    }
    public void SelectContent(Content content)
    {
        if (currentActiveContent != null)
        {
            currentActiveContent.HandleDeselection();
        }
        currentActiveContent = content;
        currentActiveContent.HandleSelection();

        ShiftPanels();
    }
    public void ShiftPanels()
    {
        panelHolder.GetComponent<RectTransform>()
            .DOAnchorPos(-contentAndPanelDict[currentActiveContent].GetComponent<RectTransform>().anchoredPosition, .3f);

    }
    public void RepositionTheBackground()
    {
        if (positionTween != null && positionTween.IsPlaying())
        {
            positionTween.Kill();
        }
        Vector2 target = new Vector2
            (
                currentActiveContent.CenterPos.x,
                choiceBackground.localPosition.y
        );

        positionTween = choiceBackground.DOMove(target, 0.2f);
        positionTween.Play();
    }

    public void RoomEditingMode()
    {
        animator.SetTrigger("Close");
    }
    public void CancelRoomEditing()
    {
        animator.SetTrigger("Open");
    }
}