using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using UnityEngine;

/// <summary>
/// Solves the Backtracking Enumeration Problem for Mahjong solutions and returns a list of possible solutions.
/// Each solution consists of a list of combo types.
/// </summary>
// https://stackoverflow.com/questions/4937771/mahjong-winning-hand-algorithm
public class WinCombos {
    private List<List<string>> listOfCombos;
    private List<List<string>> confirmedListOfCombos;

    /// <summary>
    /// Returns a list of solutions which the hand allows. Each solution consists of a list of combo types. 
    /// </summary>
    public List<List<string>> CheckWin(List<Tile> hand) {
        if (hand.Count != 14) {
            Debug.LogErrorFormat("The player's hand contains {0} tiles instead of 14", hand.Count);
            return null;
        }

        listOfCombos = new List<List<string>>();
        confirmedListOfCombos = new List<List<string>>();

        foreach (Tile tile in hand) {
            tile.isWinning = false;
        }

        Backtracking(hand, new List<string>());

        if (listOfCombos.Count == 0) {
            return listOfCombos;
        }

        confirmedListOfCombos.Add(listOfCombos[0].OrderBy(x => x).ToList());        
        if (listOfCombos.Count == 1) {
            return confirmedListOfCombos;
        }

        // Remove solutions which contain the same combo types
        foreach (List<string> pendingSolution in listOfCombos.Skip(1)) {

            List<string> pendingBecomeConfirmedSolution = pendingSolution.OrderBy(x => x).ToList();
            foreach (List<string> confirmedSolution in confirmedListOfCombos) {

                if (Enumerable.SequenceEqual(confirmedSolution, pendingSolution.OrderBy(x => x).ToList())) {
                    pendingBecomeConfirmedSolution = null;
                    break;
                }
            }

            if (pendingBecomeConfirmedSolution != null) {
                confirmedListOfCombos.Add(pendingBecomeConfirmedSolution);
            }
        }

        return confirmedListOfCombos;
    }


    /// <summary>
    /// When the function finds a combo amongst unmarked tiles, those tiles get marked. The function is then called recursively. 
    /// If no combo is found, backtrack.
    /// </summary>
    private void Backtracking(List<Tile> hand, List<string> comboListInput) {
        // Avoid removing comboTypes from listOfCombos
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
        Tile firstNonWinningTile = null;
        List<Tile> winningPotential = new List<Tile>();
        foreach (Tile tile in hand) {
            if (!tile.isWinning) {
                firstNonWinningTile = tile;
                break;
            }
        }


        // Check for Pong
        foreach (Tile tile in hand) {
            if (!tile.isWinning && tile.Equals(firstNonWinningTile)) {
                winningPotential.Add(tile);
            } 
        }

        if (winningPotential.Count >= 3) {
            for (int i = 0; i < 3; i++) {
                winningPotential[i].isWinning = true;
            }

            string comboType = "Pong";
            comboTypeList.Add(comboType);

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
                tile.isWinning = false;
            }
        }
        winningPotential.Clear();


        // Check for Chow
        if (firstNonWinningTile.suit == Tile.Suit.Character || firstNonWinningTile.suit == Tile.Suit.Dot || firstNonWinningTile.suit == Tile.Suit.Bamboo) {
            Tile tilePlusOne = new Tile(firstNonWinningTile.suit, firstNonWinningTile.rank + 1);
            Tile tilePlusTwo = new Tile(firstNonWinningTile.suit, firstNonWinningTile.rank + 2);

            foreach (Tile tile in hand) {
                if (!tile.isWinning && tile.Equals(firstNonWinningTile) && !winningPotential.Contains(firstNonWinningTile)) {
                    winningPotential.Add(tile);
                }

                if (!tile.isWinning && tile.Equals(tilePlusOne) && !winningPotential.Contains(tile)) {
                    winningPotential.Add(tile);
                }

                if (!tile.isWinning && tile.Equals(tilePlusTwo) && !winningPotential.Contains(tile)) {
                    winningPotential.Add(tile);
                }
            }

            if (winningPotential.Count == 3) {
                foreach (Tile tile in winningPotential) {
                    tile.isWinning = true;
                }

                string comboType = "Chow";
                comboTypeList.Add(comboType);

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
                    tile.isWinning = false;
                }
            }
            winningPotential.Clear();
        }
        

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
            if (!tile.isWinning && tile.Equals(firstNonWinningTile)) {
                winningPotential.Add(tile);
            }
        }

        if (winningPotential.Count >= 2) {
            for (int i = 0; i < 2; i++) {
                winningPotential[i].isWinning = true;
            }

            string comboType = "Eye";
            comboTypeList.Add(comboType);

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
                tile.isWinning = false;
            }
            winningPotential.Clear();
        }

        return;
    }

}
