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
            tile.isVisted = false;
            tile.isWinning = false;
        }

        Backtracking(hand);
    }


    public string Backtracking(List<Tile> hand) {

        FindCombos(hand);

        foreach (Tile tile in hand) {
            if (!tile.isWinning) {
                return "NONWINNING";
            }
        }
        return "WINNING";
    }


    /// <summary>
    /// Helper function that returns one Chow/Pong/Eye combo, if found
    /// </summary>
    private List<Tile> FindCombos(List<Tile> hand) {
        // Retrieve the first unvisited tile
        Tile firstTile = null;
        List<Tile> winningPotential = new List<Tile>();

        foreach (Tile tile in hand) {
            if (!tile.isVisted) {
                firstTile = tile;
                tile.isVisted = true;
                break;
            }
        }

        if (firstTile == null) {
            return null;
        }

        // Check for Pong
        foreach (Tile tile in hand) {
            if (tile.isVisted) {
                continue;
            }

            if (tile == firstTile) {
                winningPotential.Add(tile);
            }
        }
        
        if (winningPotential.Count == 2) {
            firstTile.isWinning = true;
            foreach (Tile tile in winningPotential) {
                tile.isVisted = true;
                tile.isWinning = true;
            }
            
            winningPotential.Clear();
            Backtracking(hand);
        }

        // Check for Chow
        Tile tileMinusTwo = new Tile(firstTile.suit, firstTile.rank - 2);
        Tile tileMinusOne = new Tile(firstTile.suit, firstTile.rank - 1);
        Tile tilePlusOne = new Tile(firstTile.suit, firstTile.rank + 1);
        Tile tilePlusTwo = new Tile(firstTile.suit, firstTile.rank + 2);

        if (hand.Contains(tileMinusTwo) && hand.Contains(tileMinusOne)) {

        }


        // Check for Eye
        foreach (Tile tile in hand) {
            if (tile.isVisted) {
                continue;
            }

            if (tile == firstTile) {
                winningPotential.Add(tile);
            }
        }

        if (winningPotential.Count == 1) {
            firstTile.isWinning = true;
            foreach (Tile tile in winningPotential) {
                tile.isVisted = true;
                tile.isWinning = true;
            }

            winningPotential.Clear();
            Backtracking(hand);
        }

        return null;
    }
}
