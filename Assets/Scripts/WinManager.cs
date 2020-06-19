using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class WinManager : MonoBehaviour {

    [SerializeField]
    private GameObject scriptManager;

    private GameManager gameManager;

    private PlayerManager playerManager;

    private TilesManager tilesManager;

    private PayAllDiscard payAllDiscard;

    private SacredDiscardManager sacredDiscardManager;

    private MissedDiscardManager missedDiscardManager;

    private FanCalculator fanCalculator;

    private Payment payment;

    private void Start() {
        gameManager = scriptManager.GetComponent<GameManager>();
        playerManager = scriptManager.GetComponent<PlayerManager>();
        tilesManager = scriptManager.GetComponent<TilesManager>();
        payAllDiscard = scriptManager.GetComponent<PayAllDiscard>();
        sacredDiscardManager = scriptManager.GetComponent<SacredDiscardManager>();
        missedDiscardManager = scriptManager.GetComponent<MissedDiscardManager>();
        fanCalculator = scriptManager.GetComponent<FanCalculator>();
        payment = scriptManager.GetComponent<Payment>();
    }

    /// <summary>
    /// Returns true if the local player can win. Called when a tile has been discarded, when the player draws a tile, and when
    /// the player Kongs.
    /// </summary>
    public bool CanWin(string discardType = "Normal") {
        PlayerManager.Wind? discardPlayerWind = null;
        if (discardType == "Normal") {
            if (gameManager.discardPlayer == null) {
                discardPlayerWind = null;
            } else {
                discardPlayerWind = (PlayerManager.Wind)gameManager.windsDict[gameManager.discardPlayer.ActorNumber];
            }
        } else if (discardType == "Kong") {
            discardPlayerWind = (PlayerManager.Wind)gameManager.windsDict[gameManager.kongPlayer.ActorNumber];
        } else if (discardType == "Bonus") {
            discardPlayerWind = (PlayerManager.Wind)gameManager.windsDict[gameManager.bonusPlayer.ActorNumber];
        }

        PlayerManager.Wind prevailingWind = gameManager.prevailingWind;
        int numberOfTilesLeft = gameManager.numberOfTilesLeft;
        int turn = gameManager.turnManager.Turn;

        if (discardType == "Normal") {
            (playerManager.fanTotal, playerManager.winningCombos) = fanCalculator.CalculateFan(
            playerManager, tilesManager, gameManager.latestDiscardTile, discardPlayerWind, prevailingWind, numberOfTilesLeft, turn, gameManager.AllPlayersOpenTiles());
        } else if (discardType == "Kong") {
            (playerManager.fanTotal, playerManager.winningCombos) = fanCalculator.CalculateFan(
            playerManager, tilesManager, gameManager.latestKongTile, discardPlayerWind, prevailingWind, numberOfTilesLeft, turn, gameManager.AllPlayersOpenTiles());
        } else if (discardType == "Bonus") {
            List<Tile> allOpenTiles = gameManager.AllPlayersOpenTiles();
            allOpenTiles.Add(gameManager.latestBonusTile);
            (playerManager.fanTotal, playerManager.winningCombos) = fanCalculator.CalculateFan(
            playerManager, tilesManager, gameManager.latestBonusTile, discardPlayerWind, prevailingWind, numberOfTilesLeft, turn, allOpenTiles);
        }

        if (playerManager.fanTotal > 0) {
            return true;
        }

        return false;
    }


    /// <summary>
    /// Called when the player can win
    /// </summary>
    public void WinUI() {
        if (sacredDiscardManager.sacredDiscard != null && sacredDiscardManager.sacredDiscard == gameManager.latestDiscardTile) {
            sacredDiscardManager.SacredDiscardUI();
            return;
        }

        if (missedDiscardManager.IsMissedDiscard(gameManager.latestDiscardTile)) {
            missedDiscardManager.MissedDiscardUI();
            return;
        }

        Debug.LogErrorFormat("The player can win with {0} fan and the following combos: ", playerManager.fanTotal);
        playerManager.winningCombos.ForEach(Debug.LogError);
        // TODO
    }


    /// <summary>
    /// Called when "Ok" button is clicked for the win
    /// </summary>
    public void OnWinOk() {
        // Check if the discard tile is a high risk discard
        if (payAllDiscard.shouldPayForAll(playerManager, tilesManager, gameManager.prevailingWind, gameManager.latestDiscardTile, "Win")) {
            PropertiesManager.SetPayAllDiscard(gameManager.discardPlayer);
        }

        if (playerManager.winningCombos.Contains("Robbing the Kong")) {
            PropertiesManager.SetPayAllDiscard(gameManager.kongPlayer);
        }

        if (playerManager.winningCombos.Contains("Robbing the Eighth")) {
            PropertiesManager.SetPayAllDiscard(gameManager.bonusPlayer);
        }

        // Raise an event to inform remote players of the win
        Dictionary<int, string[]> winInfo = new Dictionary<int, string[]>() {
            [playerManager.fanTotal] = playerManager.winningCombos.ToArray()
        };
        EventsManager.EventPlayerWin(winInfo);

        // Update DiscardTilesPropkey to remove the discard tile used for the win
        if (gameManager.latestDiscardTile != null) {
            PropertiesManager.SetDiscardTile(new Tuple<int, Tile, float>(-1, new Tile(0, 0), 0));
        }

        int numberOfTilesLeft = gameManager.numberOfTilesLeft;
        bool isFreshTile = gameManager.isFreshTile;

        if (playerManager.winningCombos.Contains("Robbing the Kong")) {
            payment.HandPayout(PhotonNetwork.LocalPlayer, gameManager.kongPlayer, playerManager.fanTotal, playerManager.winningCombos, numberOfTilesLeft, isFreshTile);
            payment.RevertKongPayout();
        } else if (playerManager.winningCombos.Contains("Robbing the Eighth")) {
            payment.HandPayout(PhotonNetwork.LocalPlayer, gameManager.bonusPlayer, playerManager.fanTotal, playerManager.winningCombos, numberOfTilesLeft, isFreshTile);
        } else {
            payment.HandPayout(PhotonNetwork.LocalPlayer, gameManager.discardPlayer, playerManager.fanTotal, playerManager.winningCombos, numberOfTilesLeft, isFreshTile);
        }


        // TODO: Show win screen
    }


    /// <summary>
    /// Called when "Skip" button is clicked for the win
    /// </summary>
    public void OnWinSkip() {
        // TODO
        EventsManager.EventWinUpdate(false);
    }


    /// <summary>
    /// Called by the remote player when the local player has won.
    /// </summary>
    public void RemoteWin(Player winner, int fanTotal, List<string> winningCombos) {
        int numberOfTilesLeft = gameManager.numberOfTilesLeft;
        bool isFreshTile = gameManager.isFreshTile;

        if (winningCombos.Contains("Robbing the Kong")) {
            payment.HandPayout(winner, gameManager.kongPlayer, fanTotal, winningCombos, numberOfTilesLeft, isFreshTile);
            payment.RevertKongPayout();
        } else if (winningCombos.Contains("Robbing the Eighth")) {
            payment.HandPayout(winner, gameManager.bonusPlayer, fanTotal, winningCombos, numberOfTilesLeft, isFreshTile);
        } else {
            payment.HandPayout(winner, gameManager.discardPlayer, fanTotal, winningCombos, numberOfTilesLeft, isFreshTile);
        }
    }
}
