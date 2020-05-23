﻿using Photon.Pun.Demo.Cockpit;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Checks how many Fan the local hand can win.
/// </summary>
// https://stackoverflow.com/questions/4937771/mahjong-winning-hand-algorithm
public class WinConditions {
    List<List<string>> listOfCombos = new List<List<string>>();

    public void CheckWin(List<Tile> hand) {
        foreach (Tile tile in hand) {
            tile.isVisited = false;
            tile.isWinning = false;
        }

        Backtracking(hand, new List<string>());
        Debug.Log(listOfCombos);
    }


    public void Backtracking(List<Tile> hand, List<string> comboListInput) {
        List<string> comboTypeList = new List<string>(comboListInput);

        // Base case
        bool allWinning = true;
        foreach (Tile tile in hand) {
            if (!tile.isWinning) {
                allWinning = false;
                break;
            }
        }

        if (allWinning) {
            listOfCombos.Add(comboTypeList);
            return;
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

            string comboType = "Pong";
            comboTypeList.Add(comboType);

            // DEBUG
            int winningTiles = 0;
            foreach (Tile tile in hand) {
                if (tile.isWinning) {
                    winningTiles += 1;
                }
            }
            Debug.LogFormat("New Pong Combo. There are now {0} winning tiles", winningTiles);

            Backtracking(hand, comboTypeList);
            comboTypeList.Remove(comboType);

            foreach (Tile tile in winningPotential) {
                tile.isVisited = false;
                tile.isWinning = false;
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

            string comboType = "Chow";
            comboTypeList.Add(comboType);

            // DEBUG
            int winningTiles = 0;
            foreach (Tile tile in hand) {
                if (tile.isWinning) {
                    winningTiles += 1;
                }
            }
            Debug.LogFormat("New Chow Combo. There are now {0} winning tiles", winningTiles);

            Backtracking(hand, comboTypeList);
            comboTypeList.Remove(comboType);

            foreach (Tile tile in winningPotential) {
                tile.isVisited = false;
                tile.isWinning = false;
            }
        }
        winningPotential.Clear();

        
        // The number of winning tiles is always divisible by 3, unless an eye is part of the winning tiles. In which case,
        // don't consider the possibility of an eye
        int numberOfWinningTiles = 0;
        foreach (Tile tile in hand) {
            if (tile.isWinning) {
                numberOfWinningTiles += 1;
            }
        }

        if (numberOfWinningTiles % 3 != 0) {
            return;
        }


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

            string comboType = "Eye";
            comboTypeList.Add(comboType);

            // DEBUG
            int winningTiles = 0;
            foreach (Tile tile in hand) {
                if (tile.isWinning) {
                    winningTiles += 1;
                }
            }
            Debug.LogFormat("New Eye Combo. There are now {0} winning tiles", winningTiles);

            Backtracking(hand, comboTypeList);
            comboTypeList.Remove(comboType);

            foreach (Tile tile in winningPotential) {
                tile.isVisited = false;
                tile.isWinning = false;
            }
            winningPotential.Clear();
        }

        return;
    }

}
