using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using System.Xml.Serialization;

public class PlayerReady : MonoBehaviourPunCallbacks {

    [SerializeField]
    private Transform playerListPanel;

    public void OnClickReady() {
        PropertiesManager.SetPlayerReadyDict(PhotonNetwork.LocalPlayer);
    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged) {
        if (propertiesThatChanged.ContainsKey(PropertiesManager.PlayerReadyPropKey)) {
            Dictionary<int, bool> playerReadyDict = PropertiesManager.GetPlayerReadyDict();
            
            foreach (int actorNumber in playerReadyDict.Keys) {
                Player player = PhotonNetwork.CurrentRoom.GetPlayer(actorNumber);

                UpdateReadyPlayer(player, playerReadyDict[player.ActorNumber]);
            }
        }
    }

    public void UpdateReadyPlayer(Player player, bool ready) {
        foreach (Transform child in playerListPanel) {
            PlayerListing playerListing = child.GetComponent<PlayerListing>();

            if (playerListing.Player == player) {
                Image image = child.GetComponent<Image>();

                if (!ready) {
                    image.color = new Color(1f, 0f, 0f);
                } else {
                    image.color = new Color(0f, 1f, 0f);
                }
                return;
            }
        }
    }
}
