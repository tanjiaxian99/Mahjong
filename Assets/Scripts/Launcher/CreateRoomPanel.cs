using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.AI;

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
        Debug.Log("Mahjong/CreateRoomPanel: OnClickCreateRoom was called upon clicking Create Room Button");

        if (!PhotonNetwork.IsConnected) {
            Debug.LogError("The player is not connected to the Photon MasterServer.");
            return;
        }

        if (roomName.text == "") {
            StartCoroutine(InvalidRoomName());
            return;
        }

        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 4;
        options.PlayerTtl = 30;
        options.EmptyRoomTtl = 30;

        options.IsVisible = true;
        options.IsOpen = true;

        string hashedPassword = Launcher.EncodePassword(password.text);
        options.CustomRoomProperties = PropertiesManager.SetRoomPassword(hashedPassword);
        options.CustomRoomPropertiesForLobby = new string[1] {
            PropertiesManager.RoomPasswordPropKey
        };
        
        PhotonNetwork.CreateRoom(roomName.text, options, TypedLobby.Default);
        StopCoroutine(InvalidRoomName());
        StopCoroutine(CreateRoomFailed());
        createRoomFail.SetActive(false);

        roomName.text = "";
        password.text = "";
    }

    IEnumerator InvalidRoomName() {
        createRoomFail.GetComponentInChildren<Text>().text = "Room Name cannot be empty";
        createRoomFail.SetActive(true);
        yield return new WaitForSeconds(3f);
        createRoomFail.SetActive(false);
    }

    public override void OnCreateRoomFailed(short returnCode, string message) {
        Debug.LogFormat("Mahjong/CreateRoomPanel: OnJoinRoomFailed was called by PUN by returnCode {0} and message \"{1}\".", returnCode, message);

        if (message == "A game with the specified id already exist.") {
            StartCoroutine(CreateRoomFailed());
        }        
    }

    IEnumerator CreateRoomFailed() {
        createRoomFail.GetComponentInChildren<Text>().text = "Another room has the same name!";
        createRoomFail.SetActive(true);
        yield return new WaitForSeconds(3f);
        createRoomFail.SetActive(false);
    }

    /// <summary>
    /// Called when the "Back To Lobby" button is pressed
    /// </summary>
    public void OnClickBackToLobby() {
        roomListPanel.SetActive(true);
        createRoomPanel.SetActive(false);
        createRoomFail.SetActive(false);
    }

    private void DefaultUI() {
        createRoomPanel.SetActive(false);
        createRoomFail.SetActive(false);
    }
}
