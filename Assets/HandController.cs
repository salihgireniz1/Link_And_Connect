using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HandController : MonoBehaviour
{
    public Color32 fullColor;
    public Color32 transparent;
    public Vector3 offset;
    bool isActive;
    Image handImage;
    private void Awake()
    {
        handImage = GetComponent<Image>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isActive) return;
            isActive = true;
            handImage.DOColor(fullColor, .15f);
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (!isActive) return;
            isActive = false;
            handImage.DOColor(transparent, .15f);
        }
        if (Input.GetMouseButton(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 10f;
            transform.position = mousePosition + offset;
        }
    }
}