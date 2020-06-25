﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Security.Cryptography;

public class Payment : MonoBehaviour, IResetVariables {

    [SerializeField]
    private GameObject scriptManager;
    private Dictionary<string, int> settingsDict;
    private PlayerManager playerManager;

    private Dictionary<Player, List<string>> instantPaymentDict;
    private Player latestKongPlayer;
    private string latestKongType;

    private List<Tile> seasonGroupTiles;
    private List<Tile> flowerGroupTiles;
    
    private int minPoint;
    bool shooterPay;

    Dictionary<int, int> kongTypeCount;

    private void Start() {
        Settings settings = scriptManager.GetComponent<Settings>();
        settingsDict = settings.settingsDict;
        playerManager = scriptManager.GetComponent<PlayerManager>();      

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

        this.minPoint = settingsDict["Min Point"];
        this.shooterPay = settingsDict["Shooter Pay"] != 0;
    }

    public void InitializeInstantPaymentDict(List<Player> playerList) {
        instantPaymentDict = new Dictionary<Player, List<string>>();
        foreach (Player player in playerList) {
            instantPaymentDict.Add(player, new List<string>());
        }
    }

    /// <summary>
    /// For Unit Testing.
    /// </summary>
    public Payment(List<Player> playerList, Dictionary<string, int> settingsDict, PlayerManager playerManager) {
        instantPaymentDict = new Dictionary<Player, List<string>>();
        foreach (Player player in playerList) {
            instantPaymentDict.Add(player, new List<string>());
        }

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


        this.settingsDict = settingsDict;
        this.playerManager = playerManager;
        this.minPoint = settingsDict["Min Point"];
        this.shooterPay = settingsDict["Shooter Pay"] != 0;
    }
    

    /// <summary>
    /// Determine the need for instant payments to a remote player/from other players. Called when instantiating either local or remote open tiles. 
    /// </summary>
    public void InstantPayout(Player player, List<Tile> openTiles, int turn, int numberOfTilesLeft, bool isFreshTile, Player discardPlayer, PlayerManager.Wind playerWind) {

        // Turn 0: Hidden Instant Payout. Turn 1: Fan in First Round
        bool isStartingHand = turn == 0;
        

        if (settingsDict["Hidden Cat and Rat"] > 0 && settingsDict["Cat and Rat"] > 0) {
            this.CatAndRat(player, openTiles, isStartingHand);
        }

        if (settingsDict["Hidden Chicken and Centipede"] > 0 && settingsDict["Chicken and Centipede"] > 0) {
            this.ChickenAndCentipede(player, openTiles, isStartingHand);
        }

        if (settingsDict["Complete Animal Group Payout"] > 0) {
            this.CompleteAnimalGroupPayout(player);
        }

        if (settingsDict["Hidden Bonus Tile Match Seat Wind Pair"] > 0 && settingsDict["Bonus Tile Match Seat Wind Pair"] > 0) {
            this.BonusTileMatchSeatWindPair(player, playerWind, openTiles, isStartingHand);
        }

        if (settingsDict["Complete Season Group Payout"] > 0) {
            this.CompleteSeasonGroupPayout(player, openTiles);
        }

        if (settingsDict["Complete Flower Group Payout"] > 0) {
            this.CompleteFlowerGroupPayout(player, openTiles);
        }

        if (settingsDict["Concealed Kong Payout"] > 0 && settingsDict["Discard and Exposed Kong Payout"] > 0) {
            this.KongPayout(player, openTiles, numberOfTilesLeft, isFreshTile, discardPlayer);
        }
    }


    #region Instant Payouts

    /// <summary>
    /// Determine if the player has both the Cat and Rat. Runs locally.
    /// </summary>
    private void CatAndRat(Player player, List<Tile> openTiles, bool isStartingHand) {
        if (openTiles.Contains(new Tile(Tile.Suit.Animal, Tile.Rank.One)) && openTiles.Contains(new Tile(Tile.Suit.Animal, Tile.Rank.Two))) {

            if (instantPaymentDict[player].Contains("Cat and Rat")) {
                return;
            } else {
                instantPaymentDict[player].Add("Cat and Rat");
            }
        } else {
            return;
        }

        // Hidden Cat and Rat check
        if (isStartingHand) {
            Debug.LogError("Instant Payout: Hidden Cat and Rat");
            StartCoroutine(UI.Instance.GeneralUI("Instant Payout", "Hidden Cat and Rat", 
                new List<Tile>() { new Tile(Tile.Suit.Animal, Tile.Rank.One), new Tile(Tile.Suit.Animal, Tile.Rank.Two) }));

            if (player == PhotonNetwork.LocalPlayer) {
                playerManager.points += minPoint * (int) Math.Pow(2, settingsDict["Hidden Cat and Rat"]) * 3;
            } else {
                playerManager.points -= minPoint * (int)Math.Pow(2, settingsDict["Hidden Cat and Rat"]);
            }

        } else {
            Debug.LogError("Instant Payout: Cat and Rat");
            StartCoroutine(UI.Instance.GeneralUI("Instant Payout", "Cat and Rat",
                            new List<Tile>() { new Tile(Tile.Suit.Animal, Tile.Rank.One), new Tile(Tile.Suit.Animal, Tile.Rank.Two) })); 

            if (player == PhotonNetwork.LocalPlayer) {
                playerManager.points += minPoint * (int)Math.Pow(2, settingsDict["Cat and Rat"]) * 3;
            } else {
                playerManager.points -= minPoint * (int)Math.Pow(2, settingsDict["Cat and Rat"]);
            }
        }
    }


    /// <summary>
    /// Determine if the player has both the Chicken and Centipede. Runs locally.
    /// </summary>
    private void ChickenAndCentipede(Player player, List<Tile> openTiles, bool isStartingHand) {
        if (openTiles.Contains(new Tile(Tile.Suit.Animal, Tile.Rank.Three)) && openTiles.Contains(new Tile(Tile.Suit.Animal, Tile.Rank.Four))) {

            if (instantPaymentDict[player].Contains("Chicken and Centipede")) {
                return;
            } else {
                instantPaymentDict[player].Add("Chicken and Centipede");
            }
        } else {
            return;
        }

        // Hidden Chicken and Centipede check
        if (isStartingHand) {
            Debug.LogError("Instant Payout: Hidden Chicken and Centipede");
            StartCoroutine(UI.Instance.GeneralUI("Instant Payout", "Hidden Chicken and Centipede",
                            new List<Tile>() { new Tile(Tile.Suit.Animal, Tile.Rank.Three), new Tile(Tile.Suit.Animal, Tile.Rank.Four) })); 

            if (player == PhotonNetwork.LocalPlayer) {
                playerManager.points += minPoint * (int)Math.Pow(2, settingsDict["Hidden Chicken and Centipede"]) * 3;
            } else {
                playerManager.points -= minPoint * (int)Math.Pow(2, settingsDict["Hidden Chicken and Centipede"]);
            }

        } else {
            Debug.LogError("Instant Payout: Chicken and Centipede");
            StartCoroutine(UI.Instance.GeneralUI("Instant Payout", "Chicken and Centipede",
                            new List<Tile>() { new Tile(Tile.Suit.Animal, Tile.Rank.Three), new Tile(Tile.Suit.Animal, Tile.Rank.Four) })); 

            if (player == PhotonNetwork.LocalPlayer) {
                playerManager.points += minPoint * (int)Math.Pow(2, settingsDict["Chicken and Centipede"]) * 3;
            } else {
                playerManager.points -= minPoint * (int)Math.Pow(2, settingsDict["Chicken and Centipede"]);
            }
        }
    }


    /// <summary>
    /// Determine if the player has the Complete Animal Group. Runs locally.
    /// </summary>
    private void CompleteAnimalGroupPayout(Player player) {
        if (instantPaymentDict[player].Contains("Cat and Rat") && instantPaymentDict[player].Contains("Chicken and Centipede")) {
            if (instantPaymentDict[player].Contains("Complete Animal Group")) {
                return;
            } else {
                instantPaymentDict[player].Add("Complete Animal Group");
            }
        } else {
            return;
        }
        Debug.LogError("Instant Payout: Complete Animal Group Payout");
        StartCoroutine(UI.Instance.GeneralUI("Instant Payout", "Complete Animal Group Payout", new List<Tile>() {
            new Tile(Tile.Suit.Animal, Tile.Rank.One), new Tile(Tile.Suit.Animal, Tile.Rank.Two),
            new Tile(Tile.Suit.Animal, Tile.Rank.Three), new Tile(Tile.Suit.Animal, Tile.Rank.Four) }));

        if (player == PhotonNetwork.LocalPlayer) {
            playerManager.points += minPoint * (int)Math.Pow(2, settingsDict["Complete Animal Group Payout"]) * 3;
        } else {
            playerManager.points -= minPoint * (int)Math.Pow(2, settingsDict["Complete Animal Group Payout"]);
        }
    }


    /// <summary>
    /// Determine if the player has the Bonus Tile Match Seat Wind Pair. Runs locally.
    /// </summary>
    private void BonusTileMatchSeatWindPair(Player player, PlayerManager.Wind seatWind, List<Tile> openTiles, bool isStartingHand) {
        if (openTiles.Contains(DictManager.Instance.windTo3TilesDict[seatWind][0]) && openTiles.Contains(DictManager.Instance.windTo3TilesDict[seatWind][1])) {

            if (instantPaymentDict[player].Contains("Bonus Tile Match Seat Wind Pair")) {
                return;
            } else {
                instantPaymentDict[player].Add("Bonus Tile Match Seat Wind Pair");
            }
        } else {
            return;
        }

        // Hidden Bonus Tile Match Seat Wind check
        if (isStartingHand) {
            Debug.LogError("Instant Payout: Hidden Bonus Tile Match Seat Wind Pair");
            StartCoroutine(UI.Instance.GeneralUI("Instant Payout", "Hidden Bonus Tile Match Seat Wind Pair", new List<Tile>() { 
                DictManager.Instance.windTo3TilesDict[seatWind][0], DictManager.Instance.windTo3TilesDict[seatWind][1] }));

            if (player == PhotonNetwork.LocalPlayer) {
                playerManager.points += minPoint * (int)Math.Pow(2, settingsDict["Hidden Bonus Tile Match Seat Wind Pair"]) * 3;
            } else {
                playerManager.points -= minPoint * (int)Math.Pow(2, settingsDict["Hidden Bonus Tile Match Seat Wind Pair"]);
            }

        } else {
            Debug.LogError("Instant Payout: Bonus Tile Match Seat Wind Pair");
            StartCoroutine(UI.Instance.GeneralUI("Instant Payout", "Bonus Tile Match Seat Wind Pair", new List<Tile>() {
                DictManager.Instance.windTo3TilesDict[seatWind][0], DictManager.Instance.windTo3TilesDict[seatWind][1] }));

            if (player == PhotonNetwork.LocalPlayer) {
                playerManager.points += minPoint * (int)Math.Pow(2, settingsDict["Bonus Tile Match Seat Wind Pair"]) * 3;
            } else {
                playerManager.points -= minPoint * (int)Math.Pow(2, settingsDict["Bonus Tile Match Seat Wind Pair"]);
            }
        }
    }


    /// <summary>
    /// Determine if the player has the Complete Season Group. Runs locally.
    /// </summary>
    private void CompleteSeasonGroupPayout(Player player, List<Tile> openTiles) {

        if (openTiles.Contains(seasonGroupTiles[0]) && openTiles.Contains(seasonGroupTiles[1]) && openTiles.Contains(seasonGroupTiles[2]) && openTiles.Contains(seasonGroupTiles[3])) {

            if (instantPaymentDict[player].Contains("Complete Season Group Payout")) {
                return;
            } else {
                instantPaymentDict[player].Add("Complete Season Group Payout");
            }
        } else {
            return;
        }
        Debug.LogError("Instant Payout: Complete Season Group Payout");
        StartCoroutine(UI.Instance.GeneralUI("Instant Payout", "Complete Season Group Payout", new List<Tile>() {
            new Tile(Tile.Suit.Season, Tile.Rank.One), new Tile(Tile.Suit.Season, Tile.Rank.Two),
            new Tile(Tile.Suit.Season, Tile.Rank.Three), new Tile(Tile.Suit.Season, Tile.Rank.Four) }));

        if (player == PhotonNetwork.LocalPlayer) {
            playerManager.points += minPoint * (int)Math.Pow(2, settingsDict["Complete Season Group Payout"]) * 3;
        } else {
            playerManager.points -= minPoint * (int)Math.Pow(2, settingsDict["Complete Season Group Payout"]);
        }
    }


    /// <summary>
    /// Determine if the player has the Complete Flower Group. Runs locally.
    /// </summary>
    private void CompleteFlowerGroupPayout(Player player, List<Tile> openTiles) {

        if (openTiles.Contains(flowerGroupTiles[0]) && openTiles.Contains(flowerGroupTiles[1]) && openTiles.Contains(flowerGroupTiles[2]) && openTiles.Contains(flowerGroupTiles[3])) {

            if (instantPaymentDict[player].Contains("Complete Flower Group Payout")) {
                return;
            } else {
                instantPaymentDict[player].Add("Complete Flower Group Payout");
            }
        } else {
            return;
        }
        Debug.LogError("Instant Payout: Complete Flower Group Payout");
        StartCoroutine(UI.Instance.GeneralUI("Instant Payout", "Complete Flower Group Payout", new List<Tile>() {
            new Tile(Tile.Suit.Flower, Tile.Rank.One), new Tile(Tile.Suit.Flower, Tile.Rank.Two),
            new Tile(Tile.Suit.Flower, Tile.Rank.Three), new Tile(Tile.Suit.Flower, Tile.Rank.Four) }));

        if (player == PhotonNetwork.LocalPlayer) {
            playerManager.points += minPoint * (int)Math.Pow(2, settingsDict["Complete Flower Group Payout"]) * 3;
        } else {
            playerManager.points -= minPoint * (int)Math.Pow(2, settingsDict["Complete Flower Group Payout"]);
        }
    }


    /// <summary>
    /// Determine if the player has a Kong. Runs locally.
    /// </summary>
    private void KongPayout(Player player, List<Tile> openTiles, int numberOfTilesLeft, bool isFreshTile, Player discardPlayer) {
        kongTypeCount = new Dictionary<int, int>();
        Tile kongTile = null;

        for (int i = 0; i < 4; i++) {
            kongTypeCount.Add(i, 0);
        }

        foreach (Tile tile in openTiles) {
            if (tile.kongType > 0) {
                kongTile = tile;
            }
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

        // Concealed Kong, Exposed Kong and Discard Kong check
        if (kongTypeCount[3] > 0) {
            instantPaymentDict[player].Add("Concealed Kong");
            latestKongPlayer = player;
            latestKongType = "Concealed Kong";
            Debug.LogError("Instant Payout: Concealed Kong");
            StartCoroutine(UI.Instance.GeneralUI("Instant Payout", "Concealed Kong", new List<Tile>() {
                kongTile, kongTile, kongTile, kongTile }));

            if (player == PhotonNetwork.LocalPlayer) {
                playerManager.points += minPoint * (int)Math.Pow(2, settingsDict["Concealed Kong Payout"]) * 3;
            } else {
                playerManager.points -= minPoint * (int)Math.Pow(2, settingsDict["Concealed Kong Payout"]);
            }
            return;

        } else if (kongTypeCount[2] > 0) {
            instantPaymentDict[player].Add("Exposed Kong");
            latestKongPlayer = player;
            latestKongType = "Exposed Kong";
            Debug.LogError("Instant Payout: Exposed Kong");
            StartCoroutine(UI.Instance.GeneralUI("Instant Payout", "Exposed Kong", new List<Tile>() {
                kongTile, kongTile, kongTile, kongTile }));

            if (player == PhotonNetwork.LocalPlayer) {
                playerManager.points += minPoint * (int)Math.Pow(2, settingsDict["Discard and Exposed Kong Payout"]) * 3;
            } else {
                playerManager.points -= minPoint * (int)Math.Pow(2, settingsDict["Discard and Exposed Kong Payout"]);
            }
            return;

        } else {
            instantPaymentDict[player].Add("Discard Kong");
            Debug.LogError("Instant Payout: Discard Kong");
            StartCoroutine(UI.Instance.GeneralUI("Instant Payout", "Discard Kong", new List<Tile>() {
                kongTile, kongTile, kongTile, kongTile }));

            if (player == PhotonNetwork.LocalPlayer) {
                playerManager.points += minPoint * (int)Math.Pow(2, settingsDict["Discard and Exposed Kong Payout"]) * 3;
            } else {
                if (numberOfTilesLeft < 22 && isFreshTile) {
                    // Only the player that discarded the Fresh Tile pays 
                    if (discardPlayer == PhotonNetwork.LocalPlayer) {
                        playerManager.points -= minPoint * (int)Math.Pow(2, settingsDict["Discard and Exposed Kong Payout"]) * 3;
                        Debug.LogError("Instant Payout: Fresh Tile Discard Kong, Payer");
                    } else {
                        Debug.LogError("Instant Payout: Fresh Tile Discard Kong, Non-Payer");
                    }
                    return;
                }
                playerManager.points -= minPoint * (int)Math.Pow(2, settingsDict["Discard and Exposed Kong Payout"]);
            }
        }
    }

    #endregion

    /// <summary>
    /// Determine the number of points to give or remove from the local player. Runs locally after the game is won.
    /// </summary>
    public void HandPayout(Player winner, Player discardPlayer, int fan, List<string> winningCombos, int numberOfTilesLeft, bool isFreshTile) {

        if (winner == PhotonNetwork.LocalPlayer) {
            // If the local player is the winner

            if (discardPlayer == null) {
                // Self-pick
                Debug.LogError("Winner: Self-Pick");
                playerManager.points += minPoint * (int)Math.Pow(2, fan - 1) * 2 * 3;
            } else {
                Debug.LogError("Winner: Non Self-Pick");
                playerManager.points += minPoint * (int)Math.Pow(2, fan - 1) * 4;
            }

        } else {
            // If the local player is the loser
            Debug.LogError("Checkpoint 1");

            // Fresh Tile Mahjong Scenario
            if (numberOfTilesLeft < 20 && isFreshTile && discardPlayer != null) {
                // Only the player that discarded the Fresh Tile pays 
                if (discardPlayer == PhotonNetwork.LocalPlayer) {
                    Debug.LogError("Loser: Discarded Fresh Tile For Win");
                    playerManager.points -= minPoint * (int)Math.Pow(2, fan - 1) * 4;
                }
                return;
            }
            Debug.LogError("Checkpoint 2");

            // Paying for all players
            if (playerManager.payForAll == "Local") {
                if (discardPlayer == null) {
                    Debug.LogError("Loser: Paying for all players, Self-Pick");
                    playerManager.points -= minPoint * (int)Math.Pow(2, fan - 1) * 2 * 4;
                } else {
                    Debug.LogError("Loser: Paying for all players, Non Self-Pick");
                    playerManager.points -= minPoint * (int)Math.Pow(2, fan - 1) * 4;
                }
                return;
            } else if (playerManager.payForAll == "Remote") {
                return;
            }
            Debug.LogError("Checkpoint 3");

            // Shooter pay
            if (shooterPay) {
                // If local player is the shooter
                if (discardPlayer == PhotonNetwork.LocalPlayer) {
                    Debug.LogError("Loser: Shooter Pay");
                    playerManager.points -= minPoint * (int)Math.Pow(2, fan - 1) * 4;

                } else if (discardPlayer == null) {
                    // If the remote player self pick
                    Debug.LogError("Loser: Shooter Pay, but Self-Pick");
                    playerManager.points -= minPoint * (int)Math.Pow(2, fan - 1) * 2;
                }
                return;
            }

            Debug.LogError("Checkpoint 4");
            // Non-shooter pay
            if (discardPlayer == null || discardPlayer == PhotonNetwork.LocalPlayer) {
                // If the winner self-pick or if the local player discarded the winning tile
                Debug.LogError("Loser: Normal. Winner Self-Pick / Local Player discarded winning tile");
                playerManager.points -= minPoint * (int)Math.Pow(2, fan - 1) * 2;

            } else {
                // If the local player is a loser
                Debug.LogError("Loser: Normal.");
                playerManager.points -= minPoint * (int)Math.Pow(2, fan - 1);
            }
        }
    }


    /// <summary>
    /// Return payouts from Exposed and Concealed Kong if the Kong caused a player to execute Robbing the Kong
    /// </summary>
    public void RevertKongPayout() {
        if (latestKongPlayer == null) {
            return;
        }
        Debug.LogError("Instant Payout: Revert Kong");

        if (latestKongType == "Concealed Kong") {
            if (latestKongPlayer == PhotonNetwork.LocalPlayer) {
                playerManager.points -= minPoint * (int)Math.Pow(2, settingsDict["Concealed Kong Payout"]) * 3;
            } else {
                playerManager.points += minPoint * (int)Math.Pow(2, settingsDict["Concealed Kong Payout"]);
            }
        } else if (latestKongType == "Exposed Kong") {
            if (latestKongPlayer == PhotonNetwork.LocalPlayer) {
                playerManager.points -= minPoint * (int)Math.Pow(2, settingsDict["Discard and Exposed Kong Payout"]) * 3;
            } else {
                playerManager.points += minPoint * (int)Math.Pow(2, settingsDict["Discard and Exposed Kong Payout"]);
            }
        }
    }


    public void ResetVariables() {
        instantPaymentDict.Clear();
        latestKongPlayer = null;
        latestKongType = null;
        kongTypeCount.Clear();
    }
}
