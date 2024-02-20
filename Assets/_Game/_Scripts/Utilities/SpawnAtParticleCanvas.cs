using UnityEngine;

public static class SpawnAtParticleCanvas
{
    public static GameObject Spawn(GameObject prefab, Vector3 position, Transform parent, Camera uiCamera)
    {
        GameObject obj = Object.Instantiate(prefab, parent);
        obj.GetComponent<RectTransform>().anchoredPosition = GetUIPosition(position, parent, uiCamera);
        return obj;
    }

    /// <summary>
    /// Get the screen space - camera UI position of a world point.
    /// </summary>
    /// <param name="respondedWorldPosition">The world point represented.</param>
    /// <param name="rectParent">The holder of UI object. Can be a canvas.</param>
    /// <param name="uiCamera">The UI camera that observes the screen space - camera canvas.</param>
    /// <returns></returns>
    public static Vector2 GetUIPosition(Vector3 respondedWorldPosition, Transform rectParent, Camera uiCamera)
    {
        RectTransform canvasRect = rectParent.GetComponent<RectTransform>();
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle
            (
                canvasRect,
        Camera.main.WorldToScreenPoint(respondedWorldPosition),
                uiCamera,
                out localPos
            );
        return localPos;
    }
}
