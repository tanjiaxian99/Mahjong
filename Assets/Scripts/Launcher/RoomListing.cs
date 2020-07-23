using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomListing : MonoBehaviourPunCallbacks {

    public GameObject Launcher { get; set; }

    public GameObject RoomListPanel { private get; set; }

    public PasswordPanel PasswordPanel { private get; set; }

    [SerializeField]
    private Text roomName;

    [SerializeField]
    private Text numberOfPlayers;

    [SerializeField]
    private Text privateSetting;

    public RoomInfo RoomInfo { get; private set; }

    public void SetRoomInfo(RoomInfo roomInfo) {
        RoomInfo = roomInfo;

        roomName.text = roomInfo.Name;
        numberOfPlayers.text = roomInfo.PlayerCount.ToString() + " / 4";

        string hashedPassword = PropertiesManager.GetRoomPassword(RoomInfo);

        if (hashedPassword == "???B???????o?$'?A?d??L???xR?U") {
            privateSetting.text = "Open";
        } else {
            privateSetting.text = "Private";
        }
    }

    public void OnClickJoinRoom() {
        if (RoomInfo.PlayerCount == RoomInfo.MaxPlayers) {
            Launcher.GetComponent<Launcher>().OnJoinRoomFailed(32765, "Game full");
            return;
        }

        string hashedPassword = PropertiesManager.GetRoomPassword(RoomInfo);
        if (hashedPassword == "???B???????o?$'?A?d??L???xR?U") {
            PhotonNetwork.JoinRoom(RoomInfo.Name);
        } else {
            PasswordPanel.RoomInfo = RoomInfo;

            RoomListPanel.SetActive(false);
            PasswordPanel.gameObject.SetActive(true);
        }
    }
}
