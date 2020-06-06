﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Security.Cryptography;

public class Payment {
    // Agenda: Fresh tile, shooter pay all and paying for all players reconcile
    // Pending: Robbing the kong different payout (executed by GameManager). Sacred discard and missed discard (executed by GameManager).

    private Dictionary<Player, List<string>> instantPaymentDict;
    private Dictionary<string, int> handsToCheck;
    private Dictionary<PlayerManager.Wind, List<Tile>> windToTileDict;

    private List<Tile> seasonGroupTiles;
    private List<Tile> flowerGroupTiles;

    private PlayerManager playerManager;
    private int minPoint;
    bool shooterPay;

    Dictionary<int, int> kongTypeCount;

    /// <summary>
    /// Constructor to instantiate instantPaymentDict
    /// </summary>
    public Payment(List<Player> playerList, Dictionary<string, int> handsToCheck, PlayerManager playerManager) {
        instantPaymentDict = new Dictionary<Player, List<string>>();
        foreach (Player player in playerList) {
            instantPaymentDict.Add(player, new List<string>());
        }

        this.windToTileDict = new Dictionary<PlayerManager.Wind, List<Tile>>();
        this.windToTileDict.Add(PlayerManager.Wind.EAST, new List<Tile>() { new Tile(Tile.Suit.Season, Tile.Rank.One), new Tile(Tile.Suit.Flower, Tile.Rank.One) });
        this.windToTileDict.Add(PlayerManager.Wind.SOUTH, new List<Tile>() { new Tile(Tile.Suit.Season, Tile.Rank.Two), new Tile(Tile.Suit.Flower, Tile.Rank.Two) });
        this.windToTileDict.Add(PlayerManager.Wind.WEST, new List<Tile>() { new Tile(Tile.Suit.Season, Tile.Rank.Three), new Tile(Tile.Suit.Flower, Tile.Rank.Three) });
        this.windToTileDict.Add(PlayerManager.Wind.NORTH, new List<Tile>() { new Tile(Tile.Suit.Season, Tile.Rank.Four), new Tile(Tile.Suit.Flower, Tile.Rank.Four) });

        this.seasonGroupTiles = new List<Tile>() {
            new Tile(Tile.Suit.Season, Tile.Rank.One),
            new Tile(Tile.Suit.Season, Tile.Rank.Two),
            new Tile(Tile.Suit.Season, Tile.Rank.Three),
            new Tile(Tile.Suit.Season, Tile.Rank.Four)
        };

        this.flowerGroupTiles = new List<Tile>() {
            new Tile(Tile.Suit.Flower, Tile.Rank.One),
            new Tile(Tile.Suit.Flower, Tile.Rank.Two),
            new Tile(Tile.Suit.Flower, Tile.Rank.Three),
            new Tile(Tile.Suit.Flower, Tile.Rank.Four)
        };


        this.handsToCheck = handsToCheck;
        this.playerManager = playerManager;
        this.minPoint = handsToCheck["Min Point"];
        this.shooterPay = handsToCheck["Shooter Pay"] != 0;
    }

    
    /// <summary>
    /// Determine the need for instant payments to a remote player/from other players. Called when instantiating either local or remote open tiles. 
    /// </summary>
    public void InstantPayout(Player player, List<Tile> openTiles, bool isStartingHand) {
        this.CatAndRat(player, openTiles, isStartingHand);
        this.ChickenAndCentipede(player, openTiles, isStartingHand);
        this.CompleteAnimalGroupPayout(player, openTiles);
        this.BonusTileMatchSeatWindPair(player, playerManager.playerWind, openTiles, isStartingHand);
        this.CompleteSeasonGroupPayout(player, openTiles);
        this.CompleteFlowerGroupPayout(player, openTiles);
        this.KongPayout(player, openTiles);
    }


    #region Bonus Tile Payouts

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
    /// Determine if the player has the Complete Animal Group. Runs locally.
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
            playerManager.points += minPoint * (int)Math.Pow(2, handsToCheck["Complete Animal Group Payout"]) * 3;
        } else {
            playerManager.points -= minPoint * (int)Math.Pow(2, handsToCheck["Complete Animal Group Payout"]);
        }
    }


    /// <summary>
    /// Determine if the player has the Bonus Tile Match Seat Wind Pair. Runs locally.
    /// </summary>
    public void BonusTileMatchSeatWindPair(Player player, PlayerManager.Wind playerWind, List<Tile> openTiles, bool isStartingHand) {
        if (openTiles.Contains(windToTileDict[playerWind][0]) && openTiles.Contains(windToTileDict[playerWind][1])) {

            if (instantPaymentDict[player].Contains("Bonus Tile Match Seat Wind Pair")) {
                return;
            } else {
                instantPaymentDict[player].Add("Bonus Tile Match Seat Wind Pair");
            }

        }

        // Hidden Bonus Tile Match Seat Wind check
        if (isStartingHand) {
            if (player == PhotonNetwork.LocalPlayer) {
                playerManager.points += minPoint * (int)Math.Pow(2, handsToCheck["Hidden Bonus Tile Match Seat Wind Pair"]) * 3;
            } else {
                playerManager.points -= minPoint * (int)Math.Pow(2, handsToCheck["Hidden Bonus Tile Match Seat Wind Pair"]);
            }

        } else {
            if (player == PhotonNetwork.LocalPlayer) {
                playerManager.points += minPoint * (int)Math.Pow(2, handsToCheck["Bonus Tile Match Seat Wind Pair"]) * 3;
            } else {
                playerManager.points -= minPoint * (int)Math.Pow(2, handsToCheck["Bonus Tile Match Seat Wind Pair"]);
            }
        }
    }


    /// <summary>
    /// Determine if the player has the Complete Season Group. Runs locally.
    /// </summary>
    public void CompleteSeasonGroupPayout(Player player, List<Tile> openTiles) {

        if (openTiles.Contains(seasonGroupTiles[0]) && openTiles.Contains(seasonGroupTiles[1]) && openTiles.Contains(seasonGroupTiles[2]) && openTiles.Contains(seasonGroupTiles[3])) {

            if (instantPaymentDict[player].Contains("Complete Season Group Payout")) {
                return;
            } else {
                instantPaymentDict[player].Add("Complete Season Group Payout");
            }

        }


        if (player == PhotonNetwork.LocalPlayer) {
            playerManager.points += minPoint * (int)Math.Pow(2, handsToCheck["Complete Season Group Payout"]) * 3;
        } else {
            playerManager.points -= minPoint * (int)Math.Pow(2, handsToCheck["Complete Season Group Payout"]);
        }
    }


    /// <summary>
    /// Determine if the player has the Complete Flower Group. Runs locally.
    /// </summary>
    public void CompleteFlowerGroupPayout(Player player, List<Tile> openTiles) {

        if (openTiles.Contains(flowerGroupTiles[0]) && openTiles.Contains(flowerGroupTiles[1]) && openTiles.Contains(flowerGroupTiles[2]) && openTiles.Contains(flowerGroupTiles[3])) {

            if (instantPaymentDict[player].Contains("Complete Flower Group Payout")) {
                return;
            } else {
                instantPaymentDict[player].Add("Complete Flower Group Payout");
            }

        }


        if (player == PhotonNetwork.LocalPlayer) {
            playerManager.points += minPoint * (int)Math.Pow(2, handsToCheck["Complete Flower Group Payout"]) * 3;
        } else {
            playerManager.points -= minPoint * (int)Math.Pow(2, handsToCheck["Complete Flower Group Payout"]);
        }
    }

    #endregion


    /// <summary>
    /// Determine if the player has a Kong. Runs locally.
    /// </summary>
    public void KongPayout(Player player, List<Tile> openTiles) {
        kongTypeCount = new Dictionary<int, int>();

        for (int i = 0; i < 4; i++) {
            kongTypeCount.Add(i, 0);
        }

        foreach (Tile tile in openTiles) {
            kongTypeCount[tile.kongType]++;
        }

        foreach (string instantPayment in instantPaymentDict[player]) {
            if (instantPayment == "Discard Kong") {
                kongTypeCount[1]--;
            } else if (instantPayment == "Exposed Kong") {
                kongTypeCount[2]--;
            } else if (instantPayment == "Concealed Kong") {
                kongTypeCount[3]--;
            }
        }

        if (kongTypeCount[1] == 0 && kongTypeCount[2] == 0 & kongTypeCount[3] == 0) {
            return;
        }

        // Concealed Kong vs Discard and Exposed Kong check
        if (kongTypeCount[3] > 0) {
            instantPaymentDict[player].Add("Concealed Kong");

            if (player == PhotonNetwork.LocalPlayer) {
                playerManager.points += minPoint * (int)Math.Pow(2, handsToCheck["Concealed Kong"]) * 3;
            } else {
                playerManager.points -= minPoint * (int)Math.Pow(2, handsToCheck["Concealed Kong"]);
            }
            return;

        } else if (kongTypeCount[2] > 0) {
            instantPaymentDict[player].Add("Exposed Kong");
        } else {
            instantPaymentDict[player].Add("Discard Kong");
        }

        if (player == PhotonNetwork.LocalPlayer) {
            playerManager.points += minPoint * (int)Math.Pow(2, handsToCheck["Discard and Exposed Kong"]) * 3;
        } else {
            playerManager.points -= minPoint * (int)Math.Pow(2, handsToCheck["Discard and Exposed Kong"]);
        }
    }


    /// <summary>
    /// Determine the number of points to give or remove from the local player. Runs locally after the game is won.
    /// </summary>
    public void HandPayout(Player winner, Player discardPlayer, int fan, List<string> winningCombos) {

        if (winner == PhotonNetwork.LocalPlayer) {
            // If the local player is the winner

            if (discardPlayer == null) {
                // Self-pick
                playerManager.points += minPoint * (int)Math.Pow(2, fan - 1) * 2 * 3;
            } else {
                playerManager.points += minPoint * (int)Math.Pow(2, fan - 1) * 4;
            }

        } else {
            // If the local player is the loser

            // Shooter pay
            if (shooterPay) {
                // If local player is the shooter
                if (discardPlayer == PhotonNetwork.LocalPlayer) {
                    playerManager.points -= minPoint * (int)Math.Pow(2, fan - 1) * 4;

                } else if (discardPlayer == null) {
                    // If the remote player self pick
                    playerManager.points -= minPoint * (int)Math.Pow(2, fan - 1) * 2;
                }
                return;
            }

            // Non-shooter pay
            if (discardPlayer == null || discardPlayer == PhotonNetwork.LocalPlayer) {
                // If the winner self-pick or if the local player discarded the winning tile
                playerManager.points -= minPoint * (int)Math.Pow(2, fan - 1) * 2;

            } else {
                // If the local player is a loser
                playerManager.points -= minPoint * (int)Math.Pow(2, fan - 1);
            }
        }
    }
}