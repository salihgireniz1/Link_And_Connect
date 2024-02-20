using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentPanel : MonoBehaviour
{
    public int Index { get; set; }
    public float AlignedPosition { get; set; }

    Animator animator;
    private void OnEnable()
    {
        EditRoomButton.OnEditingRoom += RoomEditingMode;
        RoomManager.OnEditingDone += CancelRoomEditing;
    }
    private void OnDisable()
    {
        EditRoomButton.OnEditingRoom -= RoomEditingMode;
        RoomManager.OnEditingDone -= CancelRoomEditing;
    }
    private void Awake()
    {
        animator = GetComponent<Animator>();
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
