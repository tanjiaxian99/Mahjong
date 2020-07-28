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

        options.CustomRoomProperties = PropertiesManager.SetRoomOptions(hashedPassword, InitialSettings());
        options.CustomRoomPropertiesForLobby = new string[1] {
            PropertiesManager.RoomPasswordPropKey,
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

    private Dictionary<string, int> InitialSettings() {
        Dictionary<string, int> settings = new Dictionary<string, int>() {
            ["Fan Limit"] = 5,

            ["Heavenly Hand"] = 10,
            ["Earthly Hand"] = 10,
            ["Humanly Hand"] = 10,

            ["Bonus Tile Match Seat Wind"] = 1,
            ["Animal"] = 1,
            ["Complete Animal Group"] = 5,
            ["Complete Season Group"] = 2,
            ["Complete Flower Group"] = 2,
            ["Robbing the Eighth"] = 10,
            ["All Flowers and Seasons"] = 10,

            ["Seat Wind Combo"] = 1,
            ["Prevailing Wind Combo"] = 1,
            ["Dragon"] = 1,

            ["Fully Concealed"] = 1,
            ["Triplets"] = 2,
            ["Half Flush"] = 2,
            ["Full Flush"] = 4,
            ["Lesser Sequence"] = 1,
            ["Full Sequence"] = 4,
            ["Mixed Terminals"] = 4,
            ["Pure Terminals"] = 10,
            ["All Honour"] = 10,
            ["Hidden Treasure"] = 10,
            ["Full Flush Triplets"] = 10,
            ["Full Flush Full Sequence"] = 10,
            ["Full Flush Lesser Sequence"] = 5,
            ["Nine Gates"] = 10,
            ["Four Lesser Blessings"] = 2,
            ["Four Great Blessings"] = 10,
            ["Pure Green Suit"] = 4,
            ["Three Lesser Scholars"] = 3,
            ["Three Great Scholars"] = 10,
            ["Eighteen Arhats"] = 10,
            ["Thirteen Wonders"] = 10,

            ["Winning on Replacement Tile for Flower"] = 1,
            ["Winning on Replacement Tile for Kong"] = 1,
            ["Kong on Kong"] = 10,

            ["Robbing the Kong"] = 1,
            ["Winning on Last Available Tile"] = 1,

            ["Dragon Tile Set Pay All"] = 1,
            ["Wind Tile Set Pay All"] = 1,
            ["Point Limit Pay All"] = 1,
            ["Full Flush Pay All"] = 1,
            ["Pure Terminals Pay All"] = 1,

            ["Min Point"] = 1,
            ["Shooter Pay"] = 1,

            ["Hidden Cat and Rat"] = 2,
            ["Cat and Rat"] = 1,
            ["Hidden Chicken and Centipede"] = 2,
            ["Chicken and Centipede"] = 1,
            ["Complete Animal Group Payout"] = 2,
            ["Hidden Bonus Tile Match Seat Wind Pair"] = 2,
            ["Bonus Tile Match Seat Wind Pair"] = 1,
            ["Complete Season Group Payout"] = 2,
            ["Complete Flower Group Payout"] = 2,
            ["Concealed Kong Payout"] = 2,
            ["Discard and Exposed Kong Payout"] = 1,
        };

        return settings;
    }

    private void DefaultUI() {
        createRoomPanel.SetActive(false);
        createRoomFail.SetActive(false);
    }
}
