using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Payment {
    // Agenda: Payment depending on Fan, shooter pay all, instant payout, Fresh tile, paying for all players, sacred discard

    private Dictionary<Player, List<string>> instantPaymentDict;
    private Dictionary<string, int> handsToCheck;
    private PlayerManager playerManager;
    private int minPoint;

    /// <summary>
    /// Constructor to instantiate instantPaymentDict
    /// </summary>
    public Payment(List<Player> playerList, Dictionary<string, int> handsToCheck, PlayerManager playerManager) {
        instantPaymentDict = new Dictionary<Player, List<string>>();

        foreach (Player player in playerList) {
            instantPaymentDict.Add(player, new List<string>());
        }

        this.handsToCheck = handsToCheck;
        this.playerManager = playerManager;
        this.minPoint = handsToCheck["Min Point"];
    }

    
    public void InstantPayout(Player player, List<Tile> openTiles, bool isStartingHand) {

        
    }


    /// <summary>
    /// Determine if the player has both the Cat and Rat. Runs locally.
    /// </summary>
    public void CatAndRat(Player player, List<Tile> openTiles, bool isStartingHand) {
        if (openTiles.Contains(new Tile(Tile.Suit.Animal, Tile.Rank.One)) && openTiles.Contains(new Tile(Tile.Suit.Animal, Tile.Rank.Two))) {

            if (instantPaymentDict[player].Contains("Cat and Rat")) {
                return;
            } else {
                instantPaymentDict[player].Add("Cat and Rat");
            }
            
        }

        // Hidden Cat and Rat check
        if (isStartingHand) {
            if (player == PhotonNetwork.LocalPlayer) {
                playerManager.points += minPoint * (int) Math.Pow(2, handsToCheck["Hidden Cat and Rat"]) * 3;
            } else {
                playerManager.points -= minPoint * (int)Math.Pow(2, handsToCheck["Hidden Cat and Rat"]);
            }

        } else {
            if (player == PhotonNetwork.LocalPlayer) {
                playerManager.points += minPoint * (int)Math.Pow(2, handsToCheck["Cat and Rat"]) * 3;
            } else {
                playerManager.points -= minPoint * (int)Math.Pow(2, handsToCheck["Cat and Rat"]);
            }
        }
    }


    /// <summary>
    /// Determine if the player has both the Chicken and Centipede. Runs locally.
    /// </summary>
    public void ChickenAndCentipede(Player player, List<Tile> openTiles, bool isStartingHand) {
        if (openTiles.Contains(new Tile(Tile.Suit.Animal, Tile.Rank.Three)) && openTiles.Contains(new Tile(Tile.Suit.Animal, Tile.Rank.Four))) {

            if (instantPaymentDict[player].Contains("Chicken and Centipede")) {
                return;
            } else {
                instantPaymentDict[player].Add("Chicken and Centipede");
            }

        }

        // Hidden Cat and Rat check
        if (isStartingHand) {
            if (player == PhotonNetwork.LocalPlayer) {
                playerManager.points += minPoint * (int)Math.Pow(2, handsToCheck["Hidden Chicken and Centipede"]) * 3;
            } else {
                playerManager.points -= minPoint * (int)Math.Pow(2, handsToCheck["Hidden Chicken and Centipede"]);
            }

        } else {
            if (player == PhotonNetwork.LocalPlayer) {
                playerManager.points += minPoint * (int)Math.Pow(2, handsToCheck["Chicken and Centipede"]) * 3;
            } else {
                playerManager.points -= minPoint * (int)Math.Pow(2, handsToCheck["Chicken and Centipede"]);
            }
        }
    }


    /// <summary>
    /// Determine if the player the Complete Animal Group. Runs locally.
    /// </summary>
    public void CompleteAnimalGroupPayout(Player player, List<Tile> openTiles) {
        if (instantPaymentDict[player].Contains("Cat and Rat") && instantPaymentDict[player].Contains("Chicken and Centipede")) {
            if (instantPaymentDict[player].Contains("Complete Animal Group")) {
                return;
            } else {
                instantPaymentDict[player].Add("Complete Animal Group");
            }
        }


        if (player == PhotonNetwork.LocalPlayer) {
            playerManager.points += minPoint * (int)Math.Pow(2, handsToCheck["Complete Animal Group Instant"]) * 3;
        } else {
            playerManager.points -= minPoint * (int)Math.Pow(2, handsToCheck["Complete Animal Group Instant"]);
        }
    }
}
