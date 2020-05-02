using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GameManager : MonoBehaviourPunCallbacks {
    #region Private Fields

    [Tooltip("The prefab to use for representing the player")]
    [SerializeField]
    private GameObject playerPrefab;

    [Tooltip("The GameObject used to represent a physical Mahjong table")]
    [SerializeField]
    private GameObject gameTable;

    #endregion

    #region MonoBehavior Callbacks

    void Start() {
        if (playerPrefab == null) {
            Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
        } else {
            Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManager.GetActiveScene().name);
            PlayerManager.Wind playerWind = AssignPlayerWind();
            MoveToWindSeat(playerWind);
            StretchGameTable(playerWind);
        }
    }

    #endregion

    #region MonoBehaviourPunCallbacks Callbacks

    /// <summary>
    /// When the player leaves the room, call the Launcher scene
    /// </summary>
    public override void OnLeftRoom() {
        SceneManager.LoadScene(0);
    }


    #endregion

    #region Private Methods

    // Assign a random player wind to the local player without clashing with the winds of other players
    private PlayerManager.Wind AssignPlayerWind() {
        // Retrieve winds of other players
        Player[] playerList = PhotonNetwork.PlayerListOthers;
        List<PlayerManager.Wind> winds = new List<PlayerManager.Wind>();
        foreach (Player player in playerList) {
            winds.Add((PlayerManager.Wind)player.CustomProperties["playerWind"]);
        }
        
        var rand = new System.Random();
        PlayerManager.Wind playerWind;

        // Assign the local player a different wind from current players
        while (true) {
            switch (rand.Next(4)) {
                case 0:
                    playerWind = PlayerManager.Wind.EAST;
                    break;
                case 1:
                    playerWind = PlayerManager.Wind.SOUTH;
                    break;
                case 2:
                    playerWind = PlayerManager.Wind.WEST;
                    break;
                default:
                    playerWind = PlayerManager.Wind.NORTH;
                    break;
            }

            if (!winds.Contains(playerWind)) {
                break;
            }
        }

        // Add local player's wind to customProperties
        Hashtable hash = new Hashtable();
        hash.Add("playerWind", playerWind);
        PhotonNetwork.SetPlayerCustomProperties(hash);

        Debug.LogErrorFormat("The local player has been assigned the {0} wind", playerWind);
        return playerWind;
    }


    // Instantiate player at the wind seat based on playerWind
    private void MoveToWindSeat(PlayerManager.Wind playerWind) {
        Vector3 playerPos;
        Quaternion cameraRotation;

        // Determine seat location based on playerWind and rotate main camera accordingly.
        switch (playerWind) {
            case PlayerManager.Wind.EAST:
                playerPos = new Vector3(15f, 1, 0);
                cameraRotation = Quaternion.Euler(90f, 0f, 90f);
                break;
            case PlayerManager.Wind.SOUTH:
                playerPos = new Vector3(0, 1, -15f);
                cameraRotation = Quaternion.Euler(90f, 0f, 0f);
                break;
            case PlayerManager.Wind.WEST:
                playerPos = new Vector3(-15f, 1, 0);
                cameraRotation = Quaternion.Euler(90f, 0f, 270f);
                break;
            default:
                playerPos = new Vector3(0, 1, 15f);
                cameraRotation = Quaternion.Euler(90f, 0f, 180f);
                break;
        }
        Camera.main.transform.rotation = cameraRotation;

        // Spawn a character for the local player. It gets synced by using PhotonNetwork.Instantiate
        PhotonNetwork.Instantiate(this.playerPrefab.name, playerPos, Quaternion.identity, 0);
    }


    // Stretch the GameTable to fill up the screen, depending on the player's seat
    private void StretchGameTable(PlayerManager.Wind playerWind) {
        Camera camera = Camera.main;
        float height = 2f * camera.orthographicSize;
        float width = height * camera.aspect;

        // Scale the GameTable along the x-z axes
        if (playerWind == PlayerManager.Wind.NORTH || playerWind == PlayerManager.Wind.SOUTH) {
            gameTable.transform.localScale = new Vector3(width, 1, height);
        } else {
            gameTable.transform.localScale = new Vector3(height, 1, width);
        }
    }

    #endregion

    #region Public Methods

    public void LeaveRoom() {
        PhotonNetwork.LeaveRoom();
    }


    #endregion
}
