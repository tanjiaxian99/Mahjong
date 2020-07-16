using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class CreateRoomPanel : MonoBehaviourPunCallbacks {

    [SerializeField]
    private GameObject roomListPanel;

    [SerializeField]
    private GameObject createRoomPanel;

    [SerializeField]
    private GameObject createRoomFail;

    [SerializeField]
    private InputField roomName;

    [SerializeField]
    private InputField password;

    void Awake() {
        DefaultUI();
    }

    /// <summary>
    /// Called when the "Create Room" button is pressed
    /// </summary>
    public void OnClickCreateRoom() {
        if (!PhotonNetwork.IsConnected) {
            Debug.LogError("The player is not connected to the Photon MasterServer.");
            return;
        }

        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 4;
        options.PlayerTtl = 30;
        options.EmptyRoomTtl = 30;

        options.IsVisible = true;
        options.IsOpen = true;

        options.CustomRoomProperties = PropertiesManager.SetRoomPassword(password.text);
        options.CustomRoomPropertiesForLobby = new string[1] {
            PropertiesManager.RoomPasswordPropKey
        };
        
        PhotonNetwork.CreateRoom(roomName.text, options, TypedLobby.Default);
        roomName.text = "";
        password.text = "";
        Debug.Log("Room successfully created");
    }

    /// <summary>
    /// Called when the "Back To Lobby" button is pressed
    /// </summary>
    public void OnClickBackToLobby() {
        roomListPanel.SetActive(true);
        createRoomPanel.SetActive(false);
    }

    private void DefaultUI() {
        createRoomPanel.SetActive(false);
        createRoomFail.SetActive(false);
    }
}
