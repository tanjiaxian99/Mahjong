using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Launcher : MonoBehaviour {
    #region Private Fields
    /// <summary>
    /// This client's version number. Users are separated from each other by gameVersion.
    /// </summary>
    string gameVersion = "1";

    #endregion

    #region MonoBehaviour CallBacks

    void Awake() {
        // All clients in the same room will automatically load the same level as Master Client
        PhotonNetwork.AutomaticallySyncScene = true;
    }
}
