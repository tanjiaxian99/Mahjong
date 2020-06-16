﻿using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PayAllDiscard {
    private HighRiskTiles highRiskTiles;
    private Dictionary<Tile, PlayerManager.Wind> tileToWindDict;

    public PayAllDiscard(Dictionary<string, int> handsToCheck) {
        highRiskTiles = new HighRiskTiles(handsToCheck);

        tileToWindDict = new Dictionary<Tile, PlayerManager.Wind>();
        tileToWindDict.Add(new Tile(Tile.Suit.Wind, Tile.Rank.One), PlayerManager.Wind.EAST);
        tileToWindDict.Add(new Tile(Tile.Suit.Wind, Tile.Rank.Two), PlayerManager.Wind.SOUTH);
        tileToWindDict.Add(new Tile(Tile.Suit.Wind, Tile.Rank.Three), PlayerManager.Wind.WEST);
        tileToWindDict.Add(new Tile(Tile.Suit.Wind, Tile.Rank.Four), PlayerManager.Wind.NORTH);
    }

    /// <summary>
    /// Returns true if the player should Pay For All, given the discard tile
    /// </summary>
    public bool shouldPayForAll(PlayerManager playerManager, PlayerManager.Wind prevailingWind, Tile discardTile, string actionType) {
        List<Tile> dangerTiles = highRiskTiles.HighRiskDiscards(playerManager.openTiles, playerManager.playerWind, prevailingWind);
        List<string> highRiskScenarios = highRiskTiles.HighRiskScenarios();

        if (!dangerTiles.Contains(discardTile)) {
            return false;
        }

        if (actionType == "Pong" || actionType == "Kong") {
            return true;
        }

        // Dragon Tile Set Scenario
        if (highRiskScenarios.Contains("Dragon Tile Set")) {
            if (discardTile.suit == Tile.Suit.Dragon) {
                if (playerManager.winningCombos.Contains("Three Lesser Scholars") || playerManager.winningCombos.Contains("Three Great Scholars")) {
                    return true;
                }
            }
        }        

        // Point Limit Scenario
        if (highRiskScenarios.Contains("Point Limit")) {
            if (discardTile.suit == Tile.Suit.Wind && tileToWindDict[discardTile] == playerManager.playerWind) {
                if (playerManager.winningCombos.Contains("Player Wind Combo")) {
                    return true;
                }
            } else if (discardTile.suit == Tile.Suit.Wind && tileToWindDict[discardTile] == prevailingWind) {
                if (playerManager.winningCombos.Contains("Prevailing Wind Combo")) {
                    return true;
                }
            } else if (discardTile.ToString() == "Dragon_One") {
                if (playerManager.winningCombos.Contains("Dragon_One")) {
                    return true;
                }
            } else if (discardTile.ToString() == "Dragon_Two") {
                if (playerManager.winningCombos.Contains("Dragon_Two")) {
                    return true;
                }
            } else if (discardTile.ToString() == "Dragon_Three") {
                if (playerManager.winningCombos.Contains("Dragon_Three")) {
                    return true;
                }
            }
        }

        // Wind Tile Set & Full Flush Scenario
        if (highRiskScenarios.Contains("Full Flush")) {

            // Wind Tile Set Scenario
            if (discardTile.suit == Tile.Suit.Wind) {
                if (playerManager.winningCombos.Contains("Four Lesser Blessings") || playerManager.winningCombos.Contains("Four Great Blessings")) {
                    return true;
                }
            }

            // Full Flush Scenario
            if (discardTile.suit == Tile.Suit.Character || discardTile.suit == Tile.Suit.Dot || discardTile.suit == Tile.Suit.Bamboo) {
                if (playerManager.winningCombos.Contains("Full Flush") ||
                    playerManager.winningCombos.Contains("Full Flush Triplets") ||
                    playerManager.winningCombos.Contains("Full Flush Full Sequence") ||
                    playerManager.winningCombos.Contains("Full Flush Lesser Sequence")) {
                    return true;
                }
            } else if (discardTile.suit == Tile.Suit.Wind || discardTile.suit == Tile.Suit.Dragon) {
                if (playerManager.winningCombos.Contains("All Honour")) {
                    return true;
                }
            }
        }

        // Pure Terminals Scenario
        if (highRiskScenarios.Contains("Pure Terminals")) {
            if (discardTile.rank == Tile.Rank.One || discardTile.rank == Tile.Rank.Nine) {
                if (discardTile.suit == Tile.Suit.Character || discardTile.suit == Tile.Suit.Dot || discardTile.suit == Tile.Suit.Bamboo) {
                    if (playerManager.winningCombos.Contains("Pure Terminals")) {
                        return true;
                    }
                }
            }
        }

        
        

        return false;
    }
}
