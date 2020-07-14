using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class RoomListPanel : MonoBehaviourPunCallbacks {

    [SerializeField]
    private GameObject roomListPanel;

    [SerializeField]
    private GameObject createRoomPanel;

    [SerializeField]
    private Transform content;

    [SerializeField]
    private Room roomPrefab;

    private List<Room> roomsList;
    void Awake() {
        roomsList = new List<Room>();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList) {
        foreach (RoomInfo roomInfo in roomList) {

            if (roomInfo.RemovedFromList) {
                int index = roomsList.FindIndex(x => x.RoomInfo.Name == roomInfo.Name);
                if (index != -1) {
                    Destroy(roomsList[index].gameObject);
                    roomsList.RemoveAt(index);
                }

            } else {
                Room room = Instantiate(roomPrefab, content);
                if (roomsList != null) {
                    room.SetRoomInfo(roomInfo);
                    roomsList.Add(room);
                }
                
            }            
        }
    }

    public void OnClickCreateRoom() {
        roomListPanel.SetActive(false);
        createRoomPanel.SetActive(true);
    }
}
