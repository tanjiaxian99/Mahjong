﻿using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomListing : MonoBehaviour {

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

        string password = PropertiesManager.GetRoomPassword(RoomInfo);
        if (password == "") {
            privateSetting.text = "Open";
        } else {
            privateSetting.text = "Private";
        }
    }

    public void OnClickJoinRoom() {
        string password = PropertiesManager.GetRoomPassword(RoomInfo);
        if (password == "") {
            PhotonNetwork.JoinRoom(RoomInfo.Name);
        } else {
            PasswordPanel.RoomInfo = RoomInfo;

            RoomListPanel.SetActive(false);
            PasswordPanel.gameObject.SetActive(true);
        }
    }
}
