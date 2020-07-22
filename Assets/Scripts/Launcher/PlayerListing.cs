using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerListing : MonoBehaviour {

    [SerializeField]
    public Player Player { get; private set; }

    [SerializeField]
    private Text playerName;

    public void SetPlayerInfo(Player player) {
        this.Player = player;
        playerName.text = player.NickName;
    }
}
