using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class RoomListPanel : MonoBehaviourPunCallbacks {

    [SerializeField]
    private GameObject launcher;

    [SerializeField]
    private GameObject roomListPanel;

    [SerializeField]
    private GameObject joinRoomFailed;

    [SerializeField]
    private PasswordPanel passwordPanel;

    [SerializeField]
    private GameObject createRoomPanel;

    [SerializeField]
    private Transform content;

    [SerializeField]
    private RoomListing roomPrefab;

    private List<RoomListing> roomList;

    void Awake() {
        roomList = new List<RoomListing>();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomInfoList) {
        foreach (RoomInfo roomInfo in roomInfoList) {
            int index = roomList.FindIndex(x => x.RoomInfo.Name == roomInfo.Name);

            if (roomInfo.RemovedFromList) {
                if (index != -1) {
                    Destroy(roomList[index].gameObject);
                    roomList.RemoveAt(index);
                }

            } else {
                if (index == -1) {
                    RoomListing roomListing = Instantiate(roomPrefab, content);
                    roomListing.Launcher = launcher;
                    roomListing.RoomListPanel = roomListPanel;
                    roomListing.PasswordPanel = passwordPanel;

                    if (roomList != null) {
                        roomListing.SetRoomInfo(roomInfo);
                        roomList.Add(roomListing);
                    }

                } else {
                    roomList[index].SetRoomInfo(roomInfo);
                }                
            }            
        }
    }

    public void OnClickCreateRoom() {
        roomListPanel.SetActive(false);
        createRoomPanel.SetActive(true);
    }
}
