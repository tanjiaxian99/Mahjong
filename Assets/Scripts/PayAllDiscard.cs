using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PayAllDiscard : MonoBehaviour {
    [SerializeField]
    private GameObject scriptManager;

    private HighRiskTiles highRiskTiles;

    private void Start() {
        highRiskTiles = scriptManager.GetComponent<HighRiskTiles>();
    }

    /// <summary>
    /// For Unit Testing. 
    /// </summary>
    public PayAllDiscard(Dictionary<string, int> settingsDict) {
        highRiskTiles = new HighRiskTiles(settingsDict);
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

    private bool IsDragonTileSet(PlayerManager playerManager, Tile discardTile) {
        if (discardTile.suit == Tile.Suit.Dragon) {
            if (playerManager.winningCombos.Contains("Three Lesser Scholars") || playerManager.winningCombos.Contains("Three Great Scholars")) {
                return true;
            }
        }
        return false;
    }

    private bool IsPointLimit(PlayerManager playerManager, Tile discardTile, PlayerManager.Wind prevailingWind) {
        if (discardTile.suit == Tile.Suit.Wind && DictManager.Instance.tileToWindDict[discardTile] == playerManager.seatWind) {
            if (playerManager.winningCombos.Contains("Player Wind Combo")) {
                return true;
            }
        } else if (discardTile.suit == Tile.Suit.Wind && DictManager.Instance.tileToWindDict[discardTile] == prevailingWind) {
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

    private bool IsWindTileSet(PlayerManager playerManager, Tile discardTile) {
        if (discardTile.suit == Tile.Suit.Wind) {
            if (playerManager.winningCombos.Contains("Four Lesser Blessings") || playerManager.winningCombos.Contains("Four Great Blessings")) {
                return true;
            }
        }
        return false;
    }

    private bool IsFullFlush(PlayerManager playerManager, Tile discardTile) {
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

    private bool IsPureTerminals(PlayerManager playerManager, Tile discardTile) {
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
