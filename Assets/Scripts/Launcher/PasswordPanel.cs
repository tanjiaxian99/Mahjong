using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;

public class PasswordPanel : MonoBehaviour {

    [SerializeField]
    private GameObject roomListPanel;

    [SerializeField]
    private GameObject passwordPanel;

    [SerializeField]
    private InputField passwordInputField;

    [SerializeField]
    private GameObject wrongPasswordPanel;

    public RoomInfo RoomInfo { get; set; }

    #region Singleton Initialization

    private static PasswordPanel _instance;

    public static PasswordPanel Instance { get { return _instance; } }

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }

        DefaultUI();
    }

    #endregion

    public void OnClickJoinRoom() {
        string hashedInput = Launcher.EncodePassword(passwordInputField.text);
        string password = PropertiesManager.GetRoomPassword(RoomInfo);

        if (hashedInput == password) {
            passwordInputField.text = "";
            passwordPanel.SetActive(false);
            PhotonNetwork.JoinRoom(RoomInfo.Name);
        } else {
            StartCoroutine(WrongPassword());
        }
    }
    
    public void OnClickBackToLobby() {
        StopCoroutine(WrongPassword());
        wrongPasswordPanel.SetActive(false);
        passwordInputField.text = "";

        passwordPanel.SetActive(false);
        roomListPanel.SetActive(true);
    }

    IEnumerator WrongPassword() {
        wrongPasswordPanel.SetActive(true);
        yield return new WaitForSeconds(3f);
        wrongPasswordPanel.SetActive(false);
    }

    private void DefaultUI() {
        passwordPanel.SetActive(false);
        foreach (Transform child in passwordPanel.transform) {
            child.gameObject.SetActive(true);
        }
        passwordPanel.transform.GetChild(3).gameObject.SetActive(false);
    }
}
