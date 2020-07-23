﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using UnityEngine.UI;

public class RoomPanel : MonoBehaviourPunCallbacks {

    [SerializeField]
    private GameObject roomListPanel;

    [SerializeField]
    private GameObject roomPanel;

    [SerializeField]
    private GameObject roomName;

    [SerializeField]
    private GameObject playerListPanel;

    [SerializeField]
    private GameObject buttonsPanel;

    #region Singleton Initialization

    private static RoomPanel _instance;

    public static RoomPanel Instance { get { return _instance; } }

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

    #endregion

    private void Start() {
        DefaultUI();
    }

    public void SetRoomName() {
        roomName.GetComponentInChildren<Text>().text = PhotonNetwork.CurrentRoom.Name;
    }

    public void OnClickLeaveRoom() {
        PhotonNetwork.LeaveRoom();

        roomPanel.SetActive(false);
        roomListPanel.SetActive(true);
    }

    private void DefaultUI() {
        roomPanel.SetActive(false);

        roomName.SetActive(true);

        playerListPanel.SetActive(true);
        foreach (Transform child in playerListPanel.transform) {
            child.gameObject.SetActive(false);
        }

        buttonsPanel.SetActive(true);
        foreach (Transform child in buttonsPanel.transform) {
            child.gameObject.SetActive(true);
        }
    }

}
