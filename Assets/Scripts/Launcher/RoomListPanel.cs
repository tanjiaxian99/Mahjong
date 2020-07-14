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

    public override void OnRoomListUpdate(List<RoomInfo> roomList) {
        foreach (RoomInfo roomInfo in roomList) {
            Room room = Instantiate(roomPrefab, content);
            room.SetRoomInfo(roomInfo);
        }

    }

    public void OnClickCreateRoom() {
        roomListPanel.SetActive(false);
        createRoomPanel.SetActive(true);
    }
}
