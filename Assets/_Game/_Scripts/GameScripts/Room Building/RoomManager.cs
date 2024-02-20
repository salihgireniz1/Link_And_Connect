using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomManager : MonoSingleton<RoomManager>
{
    public int LastPriceIndex
    {
        get => ES3.Load("Last_Price", 0);
        set
        {
            if (value >= prices.Length)
            {
                value = 1000;
            }
            ES3.Save("Last_Price", value);
        }
    }

    public static event Action OnEditingDone;
    public static event Action OnRoomInit;
    public static event Action<Room> OnRoomComplete;
    public List<Room> rooms = new List<Room>();

    public Room currentRoom;
    public ChoiceController choiceController;
    public GameObject infoLocked;
    public float completePassDuration = 2f;

    public int[] prices = new int[25];

    public Material visibleMat;
    public Material siluetteMat;

    public Image barImageToFill;

    [SerializeField] private ProgressHandler progressHandler;

    GameObject currentInfoPopUp;
    Room lastRoom;
    private void OnEnable()
    {
        EditRoomButton.OnEditingRoom += ShowCurrentRoomInfo;
    }
    private void OnDisable()
    {
        EditRoomButton.OnEditingRoom -= ShowCurrentRoomInfo;
    }

    private void Start()
    {
        InitializeCurrentRoom();
        StartCoroutine(ActivateProgress());
    }
    IEnumerator ActivateProgress()
    {
        progressHandler.gameObject.SetActive(false);
        yield return new WaitForSeconds(completePassDuration);
        progressHandler.gameObject.SetActive(true);
    }
    public int GetCurrentPrice()
    {
        if (LastPriceIndex > prices.Length - 1) return 1000;
        return prices[LastPriceIndex];
    }
    void InitializeCurrentRoom()
    {
        currentRoom = null;
        foreach (Room room in rooms)
        {
            if (!room.IsCompleted)
            {
                currentRoom = room;
                currentRoom.Init();
                OnRoomInit?.Invoke();
                //progressHandler.HandleFill(currentRoom.GetRoomProgressBetweenZeroAndOne());
                
                barImageToFill?.DOFillAmount(currentRoom.GetRoomProgressBetweenZeroAndOne(), .2f);
                break;
            }
            else
            {
                room.Init();
            }
        }
        if(currentRoom == null)
        {
            currentRoom = rooms[rooms.Count - 1];
            currentRoom.Init();
            OnRoomInit?.Invoke();
            //progressHandler.HandleFill(currentRoom.GetRoomProgressBetweenZeroAndOne());
            barImageToFill?.DOFillAmount(currentRoom.GetRoomProgressBetweenZeroAndOne(), .2f);
        }
    }
    public void BringMenuBack()
    {
        MenuController.Instance.CancelRoomEditing();
        HideInfo();
        OnEditingDone?.Invoke();
    }
    public void CheckRoomCompleted()
    {
        //progressHandler.HandleFill(currentRoom.GetRoomProgressBetweenZeroAndOne());
        barImageToFill?.DOFillAmount(currentRoom.GetRoomProgressBetweenZeroAndOne(), .2f);
        LastPriceIndex += 1;
        choiceController.HideController();
        if (IsRoomCompleted())
        {
            StartCoroutine(CompleteRoutine());
        }
        else
        {
            // Bring the menu back.
            BringMenuBack();
        }
    }
    IEnumerator CompleteRoutine()
    {
        yield return new WaitForSeconds(completePassDuration);
        OnRoomComplete?.Invoke(currentRoom);
        InitializeCurrentRoom();
    }
    public bool IsRoomCompleted()
    {
        currentRoom.CheckPropertyActivities();
        return currentRoom.IsCompleted;
    }

    public void ShowCurrentRoomInfo()
    {
        lastRoom = null;
        InfoPopUp(currentRoom);
    }
    public void HideInfo()
    {
        if (currentInfoPopUp != null)
        {
            currentInfoPopUp.SetActive(false);
        }
    }
    public void InfoPopUp(Room room)
    {
        if (room == lastRoom) return;
        lastRoom = room;
        HideInfo();

        if (room.IsCompleted || room == currentRoom)
        {
            currentInfoPopUp = room.roomInfo;
            room.SetAsVisible();
        }
        else
        {
            currentInfoPopUp = infoLocked;
            room.SetAsSiluette();
        }
        currentInfoPopUp.SetActive(true);
    }
}