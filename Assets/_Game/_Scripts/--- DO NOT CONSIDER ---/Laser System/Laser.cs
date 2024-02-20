using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Sirenix.OdinInspector;

public class Laser : MonoBehaviour
{
    [Header("RAYCAST SETTINGS"), Space(20)]
    [SerializeField]
    private Transform source;

    [SerializeField]
    private int maxLength = 100;

    [SerializeField]
    private LayerMask hitLayer;

    [SerializeField]
    [Tooltip("Can raycast hit isTrigger=true objects?")]
    QueryTriggerInteraction triggerInteraction = QueryTriggerInteraction.UseGlobal;

    [SerializeField]
    [Tooltip("The object that shows the hitted point.")]
    private GameObject hitCursor;

    [SerializeField]
    [Tooltip("The canvas that holds hit cursor as UI element.")]
    private Canvas cursorCanvas;

    [Header("LINE SETTINGS"), Space(20)]
    [SerializeField]
    private LineRenderer lineRenderer;

    [SerializeField]
    [Tooltip("Cut the line when ray hit the desired type of object.")]
    [OnValueChanged("ChangeCanReflect")]
    private bool breakOnHit;

    [SerializeField]
    [Tooltip("Reflect the line from hit surface.")]
    [ShowIf("breakOnHit")]
    private bool canReflect;

    [SerializeField, Range(1, 10)]
    [Tooltip("Max amount of reflection that the line can do.")]
    [ShowIf("canReflect")]
    private int reflectionCount = 1;


    private Ray ray;
    private RaycastHit hit;
    private float remainingLength;
    private int layerMask;
    private GameObject currentCursor;
    private void Awake()
    {
        if (lineRenderer == null)
        {
            if (!TryGetComponent(out lineRenderer))
            {
                lineRenderer = gameObject.AddComponent<LineRenderer>();
            }
        }
        if (source == null)
        {
            source = this.transform;
        }
    }
    private void FixedUpdate()
    {
        LaserShot();
    }

    private void ChangeCanReflect()
    {
        if (breakOnHit == false)
        {
            canReflect = false;
        }
    }

    public void LaserShot()
    {

        ray = new Ray(source.position, source.forward);
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, source.transform.position);
        remainingLength = maxLength;

        for (int i = 0; i < reflectionCount + 1; i++)
        {
            CheckRaycast();
        }
    }

    void CheckRaycast()
    {

        if (Physics.Raycast(ray, out hit, remainingLength, hitLayer, triggerInteraction))
        {
            if (breakOnHit)
            {
                HandleRayHit();
            }
            ShowHitCursor();
        }
        else
        {
            lineRenderer.positionCount += 1;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, ray.origin + ray.direction * remainingLength);
            Vector3 newVector = lineRenderer.GetPosition(lineRenderer.positionCount - 1);
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, newVector);
        }
        lineRenderer.material.mainTextureScale = new Vector2(1f / lineRenderer.startWidth, 1.0f);
    }
    void ShowHitCursor()
    {
        //Vector3 cursorPosition = Camera.main.WorldToScreenPoint(hit.point);
        Vector3 cursorPosition = Camera.main.WorldToScreenPoint(lineRenderer.GetPosition(1));
        if(currentCursor == null)
        {
            currentCursor = Instantiate(hitCursor, hit.point, Quaternion.identity, cursorCanvas.transform) as GameObject;
        }
        else
        {
            currentCursor.transform.position = cursorPosition;
        }
    }
    void HandleRayHit()
    {
        lineRenderer.positionCount += 1;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);
        remainingLength -= Vector3.Distance(ray.origin, hit.point);
        if (canReflect)
        {
            ray = new Ray(hit.point, Vector3.Reflect(ray.direction, hit.normal));
        }
    }
}
