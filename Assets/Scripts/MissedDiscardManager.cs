using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class MissedDiscardManager : MonoBehaviour, IResetVariables {

    private Dictionary<Player, List<Tile>> missedDiscard;

    public MissedDiscardManager() {
        missedDiscard = new Dictionary<Player, List<Tile>>();
    }

    /// <summary>
    /// Updates the Missed Discard Dictionary with the tile the discardPlayer discarded
    /// </summary>
    public void UpdateMissedDiscard(Player discardPlayer, Tile discardTile) {
        if (discardPlayer == PhotonNetwork.LocalPlayer) {
            return;
        }

        if (!missedDiscard.ContainsKey(discardPlayer)) {
            missedDiscard.Add(discardPlayer, new List<Tile>() { discardTile });
            return;
        }

        missedDiscard[discardPlayer].Add(discardTile);
    }


    /// <summary>
    /// Returns true if the tile is a Missed Discard
    /// </summary>
    public bool IsMissedDiscard(Tile discardTile) {
        foreach (Player player in missedDiscard.Keys) {
            foreach (Tile tile in missedDiscard[player]) {
                if (tile == discardTile) {
                    return true;
                }
            }
        }
        return false;
    }


    /// <summary>
    /// Reset Missed Discard Dictionary. Called when the local player discards a tile
    /// </summary>
    public void ResetMissedDiscard() {
        var playerList = missedDiscard.Keys;
        foreach (Player player in playerList) {
            missedDiscard[player].Clear();
        }
    }


    /// <summary>
    /// Called when the player can Pong/Win but the discard tile is a Missed Discard
    /// </summary>
    public void MissedDiscardUI() {
        Debug.LogError("Called MissedDiscardUI");
        EventsManager.EventCanPongKong(false);
        // TODO
    }


    public void ResetVariables() {
        missedDiscard.Clear();
    }
}
