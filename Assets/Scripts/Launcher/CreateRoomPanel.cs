using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class CreateRoomPanel : MonoBehaviourPunCallbacks {

    [SerializeField]
    private Text roomName;

    [SerializeField]
    private Text password;

    /// <summary>
    /// The password for the current room
    /// </summary>
    public static readonly string RoomPasswordPropKey = "rp";

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

        Debug.LogError(password.text);
        Hashtable ht = new Hashtable();
        ht.Add(RoomPasswordPropKey, password.text);
        options.CustomRoomProperties = ht;
        options.CustomRoomPropertiesForLobby = new string[1] {
            RoomPasswordPropKey
        };
        
        PhotonNetwork.CreateRoom(roomName.text, options, TypedLobby.Default);
        Debug.Log("Room successfully created");
    }

}
