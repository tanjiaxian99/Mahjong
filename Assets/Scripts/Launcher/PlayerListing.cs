using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerListing : MonoBehaviour {

    public Player Player { get; private set; }

    [SerializeField]
    private GameObject hostStar;

    [SerializeField]
    private Text playerName;

    public void SetPlayerInfo(Player player) {
        this.Player = player;
        playerName.text = player.NickName;
    }

    public void ShowHostStar(bool show) {
        if (show) {
            hostStar.GetComponent<Image>().enabled = true;
        } else {
            hostStar.GetComponent<Image>().enabled = false;
        }
    }
}
