using System;
using UnityEngine;
using UnityEngine.UI;

public class Plus : MonoBehaviour
{
    public Button button;
    public RoomProperty roomProperty;
    public Vector3 worldOffset;
    ChoiceController choiceController;
    RectTransform rt;

    private void Awake()
    {
        choiceController = FindObjectOfType<ChoiceController>();
        button.onClick.AddListener(OpenChoices);
        rt = GetComponent<RectTransform>();
    }
    public void OpenChoices()
    {
        choiceController.InitButtons(roomProperty);
        roomProperty.SwitchVariants(roomProperty.propertyVariants[0]);
        RoomManager.Instance.HideInfo();
        foreach (var item in FindObjectsOfType<Arrows>())
        {
            item.Hide();
        }
    }
    private void LateUpdate()
    {
        if (choiceController != null)
        {
            AlignPosition();
        }
    }

    private void AlignPosition()
    {
        Vector3 propertyPos = roomProperty.transform.position + worldOffset;

        Vector2 uiPos = SpawnAtParticleCanvas.GetUIPosition
            (
                propertyPos, 
                roomProperty.parentOfSpawnedObject, 
                roomProperty.uiCam
            );
        rt.anchoredPosition = uiPos;
    }
}
