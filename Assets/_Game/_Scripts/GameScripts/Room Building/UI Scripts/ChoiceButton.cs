using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceButton : MonoBehaviour
{
    public Button button;
    public Image image;
    public bool isLocked;

    [ShowIf("isLocked")]
    public Image lockImage;

    [ShowIf("isLocked")]
    public Button adButton;

    RoomProperty rp;
    PropertyVariant pv;
    public void Init(RoomProperty property, PropertyVariant data)
    {
        rp = property;
        pv = data;
        if(button == null)
        {
            button = GetComponent<Button>();
        }
        if (image == null)
        {
            image = GetComponent<Image>();
        }
        image.sprite = data.variantImage;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => property.SwitchVariants(data, isLocked));
        HandleLock();
    }
    public void HandleLock()
    {
        if(isLocked) LockTheButton();
        else UnlockTheButton();
    }
    public void UnlockTheButton()
    {
        isLocked = false;
        rp.SwitchVariants(pv, isLocked);
        if (lockImage != null)
        {
            lockImage.gameObject.SetActive(false);
        }
        if (adButton != null)
        {
            adButton.gameObject.SetActive(false);
        }
    }
    public void LockTheButton()
    {
        //button.interactable = false;
        if (lockImage != null)
        {
            lockImage.gameObject.SetActive(true);
        }
        if (adButton != null)
        {
            adButton.gameObject.SetActive(true);
        }
    }
}