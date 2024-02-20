using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EditRoomButton : MonoBehaviour
{
    public static event Action OnEditingRoom;
    public GameObject exitRoomButton;
    public float showDelay;
    public TextMeshProUGUI priceText;
    public Image moneyImage;
    public GameObject priceSection;
    public GameObject noMoneySect;
    private void OnEnable()
    {
        RoomManager.OnEditingDone += HideBackButton;
        MoneyManager.MoneyChanged += CheckAppearence;
    }
    private void OnDisable()
    {
        RoomManager.OnEditingDone -= HideBackButton;
        MoneyManager.MoneyChanged -= CheckAppearence;
    }
    public void StartEditingRoom()
    {
        OnEditingRoom?.Invoke();
        if (RoomManager.Instance.rooms[RoomManager.Instance.rooms.Count - 1].IsCompleted)
        {
            exitRoomButton.GetComponent<Button>().onClick.RemoveAllListeners();
            exitRoomButton.GetComponent<Button>().onClick.AddListener(() =>RoomManager.Instance.BringMenuBack());
            ShowBackButton();
        }
        else
        {
            HideBackButton();
            MoneyManager.Instance.SpendMoney(RoomManager.Instance.GetCurrentPrice());
        }
    }
    void ShowBackButton()
    {
        StartCoroutine(ShowBBRoutine());
        
    }

    private IEnumerator ShowBBRoutine()
    {
        yield return new WaitForSeconds(showDelay);
        exitRoomButton.SetActive(true);
    }
    public void CheckAppearence(int totalMoney)
    {
        int price = RoomManager.Instance.GetCurrentPrice();
        Button myButton = GetComponent<Button>();
        if (price == 1000)
        {
            // Final.
            myButton.interactable = false;
            priceSection.SetActive(false);
        }
        else
        {
            if (totalMoney >= price)
            {
                myButton.interactable = true;
                priceSection.SetActive(true);
                priceText.text = price.ToString("f0");
                noMoneySect.SetActive(false);
            }
            else
            {
                myButton.interactable = false;
                priceSection.SetActive(true);
                priceText.text = price.ToString("f0");
                noMoneySect.SetActive(true);
            }
        }
    }
    void HideBackButton()
    {
        exitRoomButton.SetActive(false);
        CheckAppearence(MoneyManager.Instance.Money);
    }
}
