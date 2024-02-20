using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public bool IsCompleted
    {
        get => ES3.Load(roomName + "_isCompleted", false);
        set => ES3.Save(roomName + "_isCompleted", value);
    }
    public string roomName = "room";

    public List<RoomProperty> properties = new List<RoomProperty>();
    public Vector3 cameraPosition;
    public GameObject roomInfo;
    public ParticleSystem roomPS;

    public MeshRenderer roomMesh;
    public void Init()
    {
        roomPS = transform.GetChild(2).GetComponent<ParticleSystem>();
        roomMesh = GetComponentInChildren<MeshRenderer>();
        if(IsCompleted ) { roomPS.Play(); }
        foreach (RoomProperty property in properties)
        {
            property.InitializeProperty();
        }
    }
    public float GetRoomProgressBetweenZeroAndOne()
    {
        int totalCount = properties.Count;
        int completedCount = 0;
        foreach (RoomProperty property in properties)
        {
            if (property.IsActive)
            {
                completedCount += 1;
            }
        }
        return (float)completedCount / (float)totalCount;
    }
    public void CheckPropertyActivities()
    {
        foreach (RoomProperty property in properties)
        {
            if(!property.IsActive)
            {
                return;
            }
        }
        // No break means all properties activated.
        // Handle room complete processes.
        CompleteRoom();
    }
    public void CompleteRoom()
    {
        IsCompleted = true;
        // Animations etc. 
        roomPS.Play();
    }

    public void SetAsVisible()
    {
        if (roomMesh != null)
        {
            roomMesh.material = RoomManager.Instance.visibleMat;
        }
    }

    public void SetAsSiluette()
    {
        if (roomMesh != null)
        {
            roomMesh.material = RoomManager.Instance.siluetteMat;
        }
    }
}
