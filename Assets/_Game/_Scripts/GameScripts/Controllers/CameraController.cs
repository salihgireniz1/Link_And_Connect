using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] private float xOffset;
    [SerializeField] private float yOffset;
    [SerializeField] private float padding;
    private Camera mainCamera;
    private float ratio = 2500f;
    private float ratio2 = 1500f;

    private void OnEnable()
    {
        LevelManager.OnLevelLoaded += UpdateCameraPositionAndSize;
    }

    private void OnDisable()
    {
        LevelManager.OnLevelLoaded -= UpdateCameraPositionAndSize;
    }

    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
    }


    private void UpdateCameraPositionAndSize(Level level)
    {
        AutoResize(level.gridData);
    }
    void AutoResize(GridData grid)
    {
        yOffset = Screen.height / ratio;
        float centerX = (grid.width - 1) / 2f * grid.tileWidthLength;
        float centerY = (grid.height - 1) / 2f * grid.tileHeightLength;

        transform.position = new Vector3(centerX + xOffset, centerY + yOffset, -10f);

        float aspectRatio = (float)Screen.width / (float)Screen.height;
        float targetOrthoWidth = grid.width + padding * 2f;
        float targetOrthoHeight = grid.height / aspectRatio + padding * 2f;
        float targetOrthoSize = Mathf.Max(targetOrthoWidth, targetOrthoHeight) * 0.5f;
        mainCamera.orthographicSize = targetOrthoSize;
    }
}
