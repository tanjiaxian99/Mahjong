using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Room : MonoBehaviour {

    [SerializeField]
    private Text roomName;

    [SerializeField]
    private Text numberOfPlayers;

    [SerializeField]
    private Text privateSetting;

    public void SetRoomInfo(RoomInfo roomInfo) {
        roomName.text = roomInfo.Name;
        numberOfPlayers.text = roomInfo.PlayerCount.ToString() + " / 4";

        string password = (string) roomInfo.CustomProperties[CreateRoomPanel.RoomPasswordPropKey];
        if (password == "") {
            privateSetting.text = "Open";
        } else {
            privateSetting.text = "Private";
        }
    }
}
