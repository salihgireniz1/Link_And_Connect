using DG.Tweening;
using UnityEngine;

public class MenuCamera : MonoBehaviour
{
    public float moveDuration;
    public Ease ease  = Ease.Linear;
    public int roomToShowIndex;
    private void OnEnable()
    {
        RoomManager.OnRoomInit += NextRoom;
    }
    private void OnDisable()
    {
        RoomManager.OnRoomInit -= NextRoom;
    }
    public void NextRoom()
    {
        roomToShowIndex = RoomManager.Instance.rooms.IndexOf(RoomManager.Instance.currentRoom);

        Vector3 pos = RoomManager.Instance.currentRoom.cameraPosition;
        transform.DOMove(pos, moveDuration)
            .SetEase(ease)
            .OnComplete(()=>RoomManager.Instance.BringMenuBack());
    }
    public void MoveToIndex()
    {
        Room room = RoomManager.Instance.rooms[roomToShowIndex];
        Vector3 pos = room.cameraPosition;
        transform.DOMove(pos, moveDuration)
            .SetEase(ease);

        RoomManager.Instance.InfoPopUp(room);
    }
}