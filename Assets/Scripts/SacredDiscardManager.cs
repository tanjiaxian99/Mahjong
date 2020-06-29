using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SacredDiscardManager : MonoBehaviour, IResetVariables {

    public Tile sacredDiscard;

    /// <summary>
    /// Called when the player can Pong/Win but the discard tile is a Sacred Discard
    /// </summary>
    public void TileIsSacredDiscard() {
        StartCoroutine(UI.Instance.GeneralUI("Sacred Discard", sacredDiscard));
    }


    public void ResetVariables() {
        sacredDiscard = null;
    }
}
