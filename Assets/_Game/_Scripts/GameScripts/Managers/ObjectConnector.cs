using MoreMountains.Feedbacks;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectConnector : MonoBehaviour
{
    public static event Action OnStartConnecting;
    public LineRenderer lineRenderer;
    public Color32 connectionColor;
    private List<SpriteInfo> connectedObjects = new List<SpriteInfo>();

    SpriteInfo lastTouchedSprite = null;
    MMF_Player soundFeedbacks;
    ComboSound comboSound;
    private void Start()
    {
        // Reset the line renderer position count
        lineRenderer.positionCount = 0;
        comboSound = GetComponent<ComboSound>();
        soundFeedbacks = GetComponent<MMF_Player>();
    }
    void Update()
    {
        if (GameManager.Instance.GameState != GameState.playing) return;

        if (Input.GetMouseButton(0))
        {
            // Check if the mouse is over a connectable object
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider == null) return;

            if (hit.collider.TryGetComponent(out SpriteInfo hitObject))
            {
                BackgroundInfo bgInfo = hitObject as BackgroundInfo;
                if (bgInfo != null && connectedObjects.Count == 0) return;
                Connect(hitObject);
                lastTouchedSprite = hitObject;
            }
            else if (hit.collider.TryGetComponent(out MergeRewardObject mergeRewardObject))
            {
                mergeRewardObject.GetMergeReward();
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            MergeManager.Instance.HandleMergeRequest(ref connectedObjects);

            // Reset the line renderer position count
            lineRenderer.positionCount = 0;
            comboSound.ResetPitch();
        }
    }
    void Connect(SpriteInfo hitObject)
    {
        if (!connectedObjects.Contains(hitObject))
        {
            if(connectedObjects.Count == 0)
            {
                OnStartConnecting?.Invoke();
            }
            // Set the end object
            SpriteInfo endObject = hitObject;

            // Add the end object to the list of connected objects
            connectedObjects.Add(endObject);
            endObject.UpdateBGColor(connectionColor);

            // Play pitched sound.
            soundFeedbacks.PlayFeedbacks();
            comboSound.PlayComboSound();
        }
        else
        {
            LostConnectionFrom(hitObject);
        }
        // Update the line position count and positions
        AssignLinePoints();
    }
    void LostConnectionFrom(SpriteInfo hitObject)
    {
        int hitObjectIndex = connectedObjects.IndexOf(hitObject);
        for (int i = hitObjectIndex + 1; i < connectedObjects.Count; i++)
        {
            connectedObjects[i].UpdateBGColor(connectedObjects[i].bgDefaultColor);
            connectedObjects.Remove(connectedObjects[i]);
            comboSound.GoBackOnSound();
        }
    }
    void AssignLinePoints()
    {
        lineRenderer.positionCount = connectedObjects.Count;
        for (int i = 0; i < connectedObjects.Count; i++)
        {
            lineRenderer.SetPosition(i, connectedObjects[i].transform.position);
        }
    }
}
