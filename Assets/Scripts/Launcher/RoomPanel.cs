using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;

public class RoomPanel : MonoBehaviourPunCallbacks {

    [SerializeField]
    private GameObject roomPanel;

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

    private void Start() {
        DefaultUI();
    }

    #endregion

    private void DefaultUI() {
        roomPanel.SetActive(false);

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
