using UnityEngine;

public class Arrows : MonoBehaviour
{
    public int indexShiftAmount;
    Animator animator;
    MenuCamera cameraHandler;
    bool isHidden;
    private void OnEnable()
    {
        EditRoomButton.OnEditingRoom += Show;
        RoomManager.OnEditingDone += Hide;
    }
    private void OnDisable()
    {
        EditRoomButton.OnEditingRoom -= Show;
        RoomManager.OnEditingDone -= Hide;
    }
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        cameraHandler = FindObjectOfType<MenuCamera>();
    }
    void Show()
    {
        if (isHidden)
        {
            isHidden = false;
            animator.SetTrigger("Show");
        }
    }
    public void Hide()
    {
        if (!isHidden)
        {
            isHidden = true;
            animator.SetTrigger("Hide");
        }
    }
    public void ShiftCamera()
    {
        cameraHandler.roomToShowIndex += indexShiftAmount;
        cameraHandler.roomToShowIndex = Mathf.Clamp(cameraHandler.roomToShowIndex, 0, RoomManager.Instance.rooms.Count-1);

        cameraHandler.MoveToIndex();
    }
}
