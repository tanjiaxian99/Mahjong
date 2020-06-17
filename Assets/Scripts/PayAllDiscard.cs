using System.Collections;
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
    public bool shouldPayForAll(PlayerManager playerManager, TilesManager tileManager, PlayerManager.Wind prevailingWind, Tile discardTile, string actionType) {
        List<Tile> dangerTiles = highRiskTiles.HighRiskDiscards(tileManager.openTiles, playerManager.seatWind, prevailingWind);
        List<string> highRiskScenarios = highRiskTiles.HighRiskScenarios();

        if (!dangerTiles.Contains(discardTile)) {
            return false;
        }

        if (actionType == "Pong" || actionType == "Kong") {
            return true;
        }

        if (highRiskScenarios.Contains("Dragon Tile Set")) {
            if (this.IsDragonTileSet(playerManager, discardTile)) {
                return true;
            }
        }        

        if (highRiskScenarios.Contains("Point Limit")) {
            if (this.IsPointLimit(playerManager, discardTile, prevailingWind)) {
                return true;
            }
        }

        if (highRiskScenarios.Contains("Full Flush")) {
            if (this.IsWindTileSet(playerManager, discardTile)) {
                return true;
            }

            if (this.IsFullFlush(playerManager, discardTile)) {
                return true;
            }
        }

        if (highRiskScenarios.Contains("Pure Terminals")) {
            if (this.IsPureTerminals(playerManager, discardTile)) {
                return true;
            }
        }

        return false;
    }

    public bool IsDragonTileSet(PlayerManager playerManager, Tile discardTile) {
        if (discardTile.suit == Tile.Suit.Dragon) {
            if (playerManager.winningCombos.Contains("Three Lesser Scholars") || playerManager.winningCombos.Contains("Three Great Scholars")) {
                return true;
            }
        }
        return false;
    }

    public bool IsPointLimit(PlayerManager playerManager, Tile discardTile, PlayerManager.Wind prevailingWind) {
        if (discardTile.suit == Tile.Suit.Wind && tileToWindDict[discardTile] == playerManager.seatWind) {
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
        return false;
    }

    public bool IsWindTileSet(PlayerManager playerManager, Tile discardTile) {
        if (discardTile.suit == Tile.Suit.Wind) {
            if (playerManager.winningCombos.Contains("Four Lesser Blessings") || playerManager.winningCombos.Contains("Four Great Blessings")) {
                return true;
            }
        }
        return false;
    }

    public bool IsFullFlush(PlayerManager playerManager, Tile discardTile) {
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
        return false;
    }

    public bool IsPureTerminals(PlayerManager playerManager, Tile discardTile) {
        if (discardTile.rank == Tile.Rank.One || discardTile.rank == Tile.Rank.Nine) {
            if (discardTile.suit == Tile.Suit.Character || discardTile.suit == Tile.Suit.Dot || discardTile.suit == Tile.Suit.Bamboo) {
                if (playerManager.winningCombos.Contains("Pure Terminals")) {
                    return true;
                }
            }
        }
        return false;
    }
}
