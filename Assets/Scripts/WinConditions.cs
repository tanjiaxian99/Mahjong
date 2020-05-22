using Photon.Pun.Demo.Cockpit;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Checks how many Fan the local hand can win.
/// </summary>
// https://stackoverflow.com/questions/4937771/mahjong-winning-hand-algorithm
public class WinConditions : MonoBehaviour {
    public void CheckWin(List<Tile> hand) {
        foreach (Tile tile in hand) {
            tile.isVisited = false;
            tile.isWinning = false;
        }

        UnityEngine.Debug.Log(Backtracking(hand));
    }


    public string Backtracking(List<Tile> hand) {
        // Base case
        bool allWinning = true;
        foreach (Tile tile in hand) {
            if (!tile.isWinning) {
                allWinning = false;
                break;
            }
        }

        if (allWinning) {
            return "Winning";
        }


        // Retrieve the first unvisited tile
        Tile firstUnvisitedTile = null;
        List<Tile> winningPotential = new List<Tile>();
        foreach (Tile tile in hand) {
            if (!tile.isVisited) {
                firstUnvisitedTile = tile;
                break;
            }
        }


        // Check for Pong
        foreach (Tile tile in hand) {
            if (!tile.isVisited && tile.Equals(firstUnvisitedTile)) {
                winningPotential.Add(tile);
            } 
        }

        if (winningPotential.Count >= 3) {
            for (int i = 0; i < 3; i++) {
                winningPotential[i].isVisited = true;
                winningPotential[i].isWinning = true;
            }

            // DEBUG
            int winningTiles = 0;
            foreach (Tile tile in hand) {
                if (tile.isWinning) {
                    winningTiles += 1;
                }
            }
            Debug.LogFormat("New Pong Combo. There are now {0} winning tiles", winningTiles);

            if (Backtracking(hand) == "Winning") {
                return "Winning";
            } else {
                foreach (Tile tile in winningPotential) {
                    tile.isVisited = false;
                    tile.isWinning = false;
                }
            }
        }
        winningPotential.Clear();


        // Check for Chow
        Tile tilePlusOne = new Tile(firstUnvisitedTile.suit, firstUnvisitedTile.rank + 1);
        Tile tilePlusTwo = new Tile(firstUnvisitedTile.suit, firstUnvisitedTile.rank + 2);

        foreach (Tile tile in hand) {
            if (!tile.isVisited && tile.Equals(firstUnvisitedTile) && !winningPotential.Contains(firstUnvisitedTile)) {
                winningPotential.Add(tile);
            }

            if (!tile.isVisited && tile.Equals(tilePlusOne) && !winningPotential.Contains(tile)) {
                winningPotential.Add(tile);
            }

            if (!tile.isVisited && tile.Equals(tilePlusTwo) && !winningPotential.Contains(tile)) {
                winningPotential.Add(tile);
            }
        }

        if (winningPotential.Count == 3) {
            foreach (Tile tile in winningPotential) {
                tile.isVisited = true;
                tile.isWinning = true;
            }

            // DEBUG
            int winningTiles = 0;
            foreach (Tile tile in hand) {
                if (tile.isWinning) {
                    winningTiles += 1;
                }
            }
            Debug.LogFormat("New Chow Combo. There are now {0} winning tiles", winningTiles);

            if (Backtracking(hand) == "Winning") {
                return "Winning";
            } else {
                foreach (Tile tile in winningPotential) {
                    tile.isVisited = false;
                    tile.isWinning = false;
                }
            }
        }
        winningPotential.Clear();


        // Check for Eye
        foreach (Tile tile in hand) {
            if (!tile.isVisited && tile.Equals(firstUnvisitedTile)) {
                winningPotential.Add(tile);
            }
        }

        if (winningPotential.Count >= 2) {
            for (int i = 0; i < 2; i++) {
                winningPotential[i].isVisited = true;
                winningPotential[i].isWinning = true;
            }

            // DEBUG
            int winningTiles = 0;
            foreach (Tile tile in hand) {
                if (tile.isWinning) {
                    winningTiles += 1;
                }
            }
            Debug.LogFormat("New Eye Combo. There are now {0} winning tiles", winningTiles);

            if (Backtracking(hand) == "Winning") {
                return "Winning";
            } else {
                foreach (Tile tile in winningPotential) {
                    tile.isVisited = false;
                    tile.isWinning = false;
                }
            }
            winningPotential.Clear();
        }

        return "Nonwinning";
    }

}
