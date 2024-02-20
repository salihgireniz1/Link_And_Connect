using System;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceController : MonoBehaviour
{
    public static event Action OnChoiceInit;
    public List<ChoiceButton> Buttons = new();
    Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        HideController();
    }
    public void InitButtons(RoomProperty roomProperty)
    {
        for (int i = 0; i < Buttons.Count; i++)
        {
            Buttons[i].Init(roomProperty, roomProperty.propertyVariants[i]);
        }
        ShowController();
        OnChoiceInit?.Invoke();
    }
    public void ShowController()
    {
        animator.SetTrigger("Show");
    }
    public void HideController()
    {
        animator.SetTrigger("Hide");
        Buttons[Buttons.Count - 1].isLocked = true;
    }
}