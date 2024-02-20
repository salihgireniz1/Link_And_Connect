using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Content : MonoBehaviour
{
    public Vector2 CenterPos {  get; set; }
    public bool IsLocked { get; private set; }

    [SerializeField]
    private Image image;

    [SerializeField]
    private TextMeshProUGUI headerText;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private Button button;
    public void FillContent(ContentData data)
    {
        IsLocked = data.isLocked;
        if(IsLocked)
        {
            image.sprite = MenuController.Instance.lockSprite;
            button.interactable = false;
            return;
        }
        animator = GetComponent<Animator>();

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(Select);

        image.sprite = data.contentImage;
        headerText.text = data.header;
    }
    public void Select()
    {
        MenuController.Instance.SelectContent(this);
    }
    public void HandleSelection()
    {
        animator.SetBool("IsSelected", true);
    }
    public void HandleDeselection()
    {
        animator.SetBool("IsSelected", false);
    }
    public void AssignCenter()
    {
        //CenterPos = button.transform.position;
    }
}