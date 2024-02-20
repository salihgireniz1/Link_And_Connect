using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RoomProperty : MonoBehaviour
{
    public bool IsActive
    {
        get => ES3.Load(propertyName + "_isActive", false);
        set => ES3.Save(propertyName + "_isActive", value);
    }
    public bool CanBePurchased
    {
        get => ES3.Load(propertyName + "_canPurchased", false);
        set => ES3.Save(propertyName + "_canPurchased", value);
    }

    public string propertyName = "test";

    [Space]
    public List<PropertyVariant> propertyVariants = new List<PropertyVariant>();

    [SerializeField]
    private PropertyVariant currentVariant;

    public GameObject uiObjectPrefab;
    public Transform parentOfSpawnedObject;
    public Camera uiCam;
    public Vector3 worldOffset;

    public bool defaultOpen = false;
    public List<RoomProperty> propertiesToOpen = new List<RoomProperty>();
    public ParticleSystem particle;
    GameObject respondPlusUI;
    private void OnEnable()
    {
        particle = GetComponentInChildren<ParticleSystem>();
        propertyName = gameObject.name;

        if (defaultOpen)
        {
            CanBePurchased = true;
        }

        propertyVariants = GetComponentsInChildren<PropertyVariant>().ToList();
        foreach (var item in propertyVariants)
        {
            item.gameObject.SetActive(false);
        }
        EditRoomButton.OnEditingRoom += ShowPlus;
        ChoiceController.OnChoiceInit += HidePlus;
    }
    private void OnDisable()
    {
        EditRoomButton.OnEditingRoom -= ShowPlus;
        ChoiceController.OnChoiceInit -= HidePlus;
    }
    public void SpawnPlus()
    {
        respondPlusUI = SpawnAtParticleCanvas.Spawn
            (
                uiObjectPrefab,
                transform.position + worldOffset,
                parentOfSpawnedObject,
                uiCam
            );
        HidePlus();
        respondPlusUI.GetComponent<Plus>().roomProperty = this;
    }
    public void ShowPlus()
    {
        if (!CanBePurchased) return;

        if (respondPlusUI != null && !IsActive)
        {
            respondPlusUI.SetActive(true);
        }
    }
    public void HidePlus()
    {
        if (respondPlusUI != null)
        {
            respondPlusUI.SetActive(false);
        }
    }
    public void InitializeProperty()
    {
        if (IsActive) LoadProperty();
        else UnloadProperty();
    }
    public void SaveProperty()
    {
        IsActive = true;
        ES3.Save(propertyName + "_Saved_Index", GetIndexOfVariant(currentVariant));

        foreach (var prop in propertiesToOpen)
        {
            prop.CanBePurchased = true;
        }
    }
    public void LoadProperty()
    {
        SwitchVariants(GetSavedVariant());
    }
    public void UnloadProperty()
    {
        SpawnPlus();
    }
    public PropertyVariant GetSavedVariant()
    {
        int savedVariantIndex = ES3.Load(propertyName + "_Saved_Index", -1);

        if (savedVariantIndex == -1) return null;

        PropertyVariant variant = GetVariantFromIndex(savedVariantIndex);
        return variant;
    }
    protected int GetIndexOfVariant(PropertyVariant variant)
    {
        if (propertyVariants != null && propertyVariants.Contains(variant))
        {
            return propertyVariants.IndexOf(variant);
        }
        return 0;
    }
    protected PropertyVariant GetVariantFromIndex(int index)
    {
        if (propertyVariants != null && propertyVariants.Count > index)
        {
            return propertyVariants[index];
        }
        return null;
    }
    public Button doneButton;
    public WatchAdToUnlockChoice watchAdToUnlockChoice;
    public Sprite adSprite;
    public Sprite tickSprite;
    public void SwitchVariants(PropertyVariant propertyVariant, bool lockCond = false)
    {
        if (doneButton)
        {
            doneButton.onClick.RemoveAllListeners();
            if (lockCond)
            {
                doneButton.onClick.AddListener(watchAdToUnlockChoice.WatchAndUnlock);
                // Also change sprite.
                doneButton.GetComponent<Image>().sprite = adSprite;
            }
            else
            {
                doneButton.onClick.AddListener(RoomManager.Instance.CheckRoomCompleted);
                doneButton.GetComponent<Image>().sprite = tickSprite;
                // Also change sprite.
            }
            // doneButton.interactable = !lockCond;
        }
        if (currentVariant == propertyVariant) return;
        if (currentVariant != null)
        {
            currentVariant.Deactivate();
        }
        propertyVariant.Activate(currentVariant == null);
        currentVariant = propertyVariant;
        if (particle) particle.Play();

        if (!lockCond)
        {
            SaveProperty();
        }
    }
}
