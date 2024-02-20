using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum RotationDirection
{
    YZAxis,
    XYAxis,
    XZAxis
}
public class RotateWithMouse : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;

    [SerializeField]
    private Transform controlPoint;

    [SerializeField]
    private float rotationSensitivity;

    [SerializeField]
    private RotationDirection rotationDirection = RotationDirection.YZAxis;

    Vector3 touchPosition;
    private void Awake()
    {
        if(mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }
    private void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f);
            touchPosition = mainCamera.ScreenToWorldPoint(mousePos);
            Vector3 direction = touchPosition - controlPoint.position;

            controlPoint.rotation = Quaternion.Slerp
                (
                controlPoint.rotation,
                GetRotation(direction),
                rotationSensitivity * Time.fixedDeltaTime
                );
        }
    }
    public Quaternion GetRotation(Vector3 direction)
    {
        float angle = 0f;
        Quaternion rotation = Quaternion.identity;
        switch (rotationDirection)
        {
            case RotationDirection.YZAxis:
                angle = Mathf.Atan2(direction.z, direction.y) * Mathf.Rad2Deg;
                rotation = Quaternion.AngleAxis(angle - 90, Vector3.right);
                return rotation;
            case RotationDirection.XYAxis:
                angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
                return rotation;
            case RotationDirection.XZAxis:
                angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
                rotation = Quaternion.AngleAxis(angle - 90, Vector3.down);
                return rotation;
            default:
                return Quaternion.identity;
        }
    }
}
