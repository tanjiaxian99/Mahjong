using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;
public class GameManager : MonoBehaviourPunCallbacks {
    #region Photon Callbacks

    /// <summary>
    /// When the player leaves the room, call the Launcher scene
    /// </summary>
    public override void OnLeftRoom() {
        SceneManager.LoadScene(0);
    }


    #endregion


    #region Private Methods

    /// <summary>
    /// The Master Client will load the GameRoom and subsequent players will join
    /// </summary>
    void LoadArena() {
        if (!PhotonNetwork.IsMasterClient) {
            Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the Master Client");
        }
        Debug.LogFormat("PhotonNetwork : Loading GameRoom");
        PhotonNetwork.LoadLevel("GameRoom");
    }

    #endregion

    #region Public Methods


    public void LeaveRoom() {
        PhotonNetwork.LeaveRoom();
    }


    #endregion
}
