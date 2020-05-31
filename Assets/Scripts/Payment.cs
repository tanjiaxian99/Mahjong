using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Security.Cryptography;

public class Payment {
    // Agenda: Payment depending on Fan, shooter pay all, instant payout, Fresh tile, paying for all players, sacred discard

    private Dictionary<Player, List<string>> instantPaymentDict;
    private Dictionary<string, int> handsToCheck;
    private Dictionary<PlayerManager.Wind, List<Tile>> windToTileDict;

    private List<Tile> seasonGroupTiles;
    private List<Tile> flowerGroupTiles;

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
}
