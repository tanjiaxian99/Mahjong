using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomListPanel : MonoBehaviour {

    [SerializeField]
    private GameObject roomListPanel;

    [SerializeField]
    private GameObject createRoomPanel;

    void Awake() {
        DefaultUI();
    }

    public void OnClickCreateRoom() {
        roomListPanel.SetActive(false);
        createRoomPanel.SetActive(true);
    }

    private void DefaultUI() {
        createRoomPanel.SetActive(false);
        createRoomPanel.transform.GetChild(3).GetComponent<Text>().enabled = false;
    }
}
