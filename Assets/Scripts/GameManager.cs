using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;
public class GameManager : MonoBehaviourPunCallbacks {
    #region Private Fields

    [Tooltip("The prefab to use for representing the player")]
    [SerializeField]
    private GameObject playerPrefab;

    private List<PlayerManager.Wind> playerWinds;

    #endregion

    #region MonoBehavior Callbacks

    void Start() {
        if (playerPrefab == null) {
            Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
        } else {
            Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManager.GetActiveScene().name);
            // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
            PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 1f, -5.5f), Quaternion.identity, 0);
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

    // Assign a random player wind to each new player
    //private void playerWindStart() {
    //    var rand = new System.Random();
    //    switch (rand.Next(4)) {
    //        case 0:
    //            playerWind = Wind.EAST;
    //            break;
    //        case 1:
    //            playerWind = Wind.SOUTH;
    //            break;
    //        case 2:
    //            playerWind = Wind.WEST;
    //            break;
    //        case 3:
    //            playerWind = Wind.NORTH;
    //            break;
    //    }
    //}

    #endregion

    #region Public Methods


    public void LeaveRoom() {
        PhotonNetwork.LeaveRoom();
    }


    #endregion
}
