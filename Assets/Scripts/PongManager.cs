using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PongManager : MonoBehaviour {

    #region Serialize Field

    [SerializeField]
    private GameObject PongCombo;

    [SerializeField]
    private GameObject KongComboZero;

    [SerializeField]
    private GameObject KongComboOne;

    [SerializeField]
    private GameObject KongComboTwo;

    [SerializeField]
    private GameObject scriptManager;

    #endregion

    private GameManager gameManager;

    private PlayerManager playerManager;

    private TilesManager tilesManager;

    private PayAllDiscard payAllDiscard;

    private SacredDiscardManager sacredDiscardManager;

    private MissedDiscardManager missedDiscardManager;

    private void Start() {
        gameManager = scriptManager.GetComponent<GameManager>();
        playerManager = scriptManager.GetComponent<PlayerManager>();
        tilesManager = scriptManager.GetComponent<TilesManager>();
        payAllDiscard = scriptManager.GetComponent<PayAllDiscard>();
        sacredDiscardManager = scriptManager.GetComponent<SacredDiscardManager>();
        missedDiscardManager = scriptManager.GetComponent<MissedDiscardManager>();
    }


    /// <summary>
    /// Called when the player can Pong
    /// </summary>
    public void PongUI(Tile discardTile) {
        if (sacredDiscardManager.sacredDiscard != null && sacredDiscardManager.sacredDiscard == discardTile) {
            sacredDiscardManager.TileIsSacredDiscard();
            return;
        }

        if (missedDiscardManager.IsMissedDiscard(discardTile)) {
            missedDiscardManager.MissedDiscardUI(discardTile);
            return;
        }

        Transform spritesPanel = PongCombo.transform.GetChild(0);

        // Instantiate the tile sprites
        for (int i = 0; i < 3; i++) {
            Transform imageTransform = spritesPanel.GetChild(i);
            Image image = imageTransform.GetComponent<Image>();
            image.sprite = DictManager.Instance.spritesDict[discardTile];
        }
        PongCombo.SetActive(true);
    }


    /// <summary>
    /// Called when "Ok" is clicked for Pong Combo
    /// </summary>
    public void OnPongOk() {
        Tile latestDiscardTile = gameManager.latestDiscardTile;

        // Update MasterClient that the player want to Pong
        EventsManager.EventCanPongKong(true);

        // Check if the discard tile is a high risk discard
        if (payAllDiscard.shouldPayForAll(playerManager, tilesManager, gameManager.prevailingWind, latestDiscardTile, "Pong")) {
            PropertiesManager.SetPayAllPlayer(gameManager.discardPlayer);
        }

        PongCombo.SetActive(false);
        KongComboZero.SetActive(false);
        KongComboOne.SetActive(false);
        KongComboTwo.SetActive(false);

        // Update discard tile properties to indicate to all players to remove the latest discard tile
        PropertiesManager.SetDiscardTile(new Tuple<int, Tile, float>(-1, new Tile(0, 0), 0));

        // Update both the player's hand and the combo tiles list. 2 tiles are removed from the player's hand and 3 tiles are
        // added to combo tiles.
        List<Tile> pongTiles = new List<Tile>();
        for (int i = 0; i < 3; i++) {
            pongTiles.Add(latestDiscardTile);

            if (i < 2) {
                tilesManager.hand.Remove(latestDiscardTile);
            }
        }
        tilesManager.comboTiles.Add(pongTiles);

        playerManager.InstantiateLocalHand();
        playerManager.InstantiateLocalOpenTiles();

        // The local player automatically update that it is his turn
        playerManager.myTurn = true;
        playerManager.canTouchHandTiles = true;
    }


    /// <summary>
    /// Called when "Skip" button is clicked for Pong Combo
    /// </summary>
    public void OnPongSkip() {
        // Update MasterClient that the player doesn't want to Pong
        EventsManager.EventCanPongKong(false);

        missedDiscardManager.UpdateMissedDiscard(gameManager.discardPlayer, gameManager.latestDiscardTile);

        PongCombo.SetActive(false);
        KongComboZero.SetActive(false);
        KongComboOne.SetActive(false);
        KongComboTwo.SetActive(false);
    }
}
