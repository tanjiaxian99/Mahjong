using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;
public class GameManager : MonoBehaviourPunCallbacks {
    #region Private Fields

    [Tooltip("The prefab to use for representing the player")]
    public GameObject playerPrefab;

    #endregion

    #region MonoBehavior Callbacks

    void Start() {
        if (playerPrefab == null) {
            Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
        } else {
            Debug.LogFormat("We are Instantiating LocalPlayer from {0}", Application.loadedLevelName);
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

    #region Public Methods


    public void LeaveRoom() {
        PhotonNetwork.LeaveRoom();
    }


    #endregion
}
