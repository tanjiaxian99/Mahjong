﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using System.Linq;
using System;
using Photon.Pun;

public class EndRound : MonoBehaviour {

    [SerializeField]
    private GameObject scriptManager;


    private TilesManager tilesManager;

    #region Singleton Initialization

    private static EndRound _instance;

    public static EndRound Instance { get { return _instance; } }

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

    #endregion

    private void Start() {
        tilesManager = scriptManager.GetComponent<TilesManager>();
    }

    /// <summary>
    /// Wrapper function which when called, ends the round
    /// </summary>
    /// <param name="winner"></param>
    /// <param name="fanTotal"></param>
    /// <param name="winningCombos"></param>
    public void EndGame(Player winner, int fanTotal, List<string> winningCombos) {
        UpdateHandTiles();
        DisplayWinningCombo(winner, fanTotal, winningCombos);
        NewPlayOrder(winner);
    }

    /// <summary>
    /// Inform other players of the local player's hand tiles
    /// </summary>
    private void UpdateHandTiles() {
        PropertiesManager.SetOpenHand(tilesManager.hand);
    }

    private void DisplayWinningCombo(Player winner, int fanTotal, List<string> winningCombos) {
        // TODO: Display winning combos / fan
    }

    /// <summary>
    /// UI button that prompts the start of a new round
    /// </summary>
    private void PromptNewRound() {
        // TODO
    }

    /// <summary>
    /// Called upon clicking the "Start new round" button
    /// </summary>
    public void ReadyToStart() {
        EventsManager.EventNewRound();
    }

    /// <summary>
    /// Called when all players are ready for a new round
    /// </summary>
    public void OnStartNewRound() {
        ResetAllVariables();
        ClearGameTable();
        NewPrevailingWind();
        StartNewRound();
    }

    /// <summary>
    /// Reset variables which do not persist between rounds
    /// </summary>
    private void ResetAllVariables() {
        // TODO: Any way to make use of IResetVariables?
        scriptManager.GetComponent<GameManager>().ResetVariables();
        EventsManager.ResetVariables();
        scriptManager.GetComponent<PlayerManager>().ResetVariables();
        scriptManager.GetComponent<TilesManager>().ResetVariables();
        scriptManager.GetComponent<SacredDiscardManager>().ResetVariables();
        scriptManager.GetComponent<MissedDiscardManager>().ResetVariables();
        scriptManager.GetComponent<FanCalculator>().ResetVariables();
        scriptManager.GetComponent<Payment>().ResetVariables();
    }

    /// <summary>
    /// Destroy all tiles on the game table. 
    /// </summary>
    private void ClearGameTable() {
        GameObject[] allGameObjects = FindObjectsOfType<GameObject>();
        for (int i = 0; i < allGameObjects.Length; i++) {
            if (allGameObjects[i].transform.name.EndsWith("(Clone)")) {
                Destroy(allGameObjects[i]);
            }
        }
    }

    /// <summary>
    /// Determine the new play order
    /// </summary>
    private void NewPlayOrder(Player winner) {
        if (!PhotonNetwork.IsMasterClient) {
            return;
        }

        Player[] currentPlayOrder = PropertiesManager.GetPlayOrder();
        if (winner == currentPlayOrder[0]) {
            return;
        }

        Player[] newPlayOrder = new Player[4];
        Array.Copy(currentPlayOrder, 1, newPlayOrder, 0, 3);
        newPlayOrder[3] = currentPlayOrder[0];

        PropertiesManager.SetPlayOrder(newPlayOrder);

        if (newPlayOrder == PropertiesManager.GetInitialPlayOrder()) {
            NewPrevailingWind();
        }
    }

    /// <summary>
    /// Determine the new prevailing wind
    /// </summary>
    private void NewPrevailingWind() {
        PlayerManager.Wind currentPrevailingWind = PropertiesManager.GetPrevailingWind();
        PlayerManager.Wind newPrevailingWind = currentPrevailingWind + 1;

        if (newPrevailingWind == PlayerManager.Wind.EAST) {
            EndGame();
            return;
        }

        PropertiesManager.SetPrevailingWind(newPrevailingWind);
    }


    // TODO: Call InitializeRound to start the new round
    public static void StartNewRound() {
        // TODO: Every player press ok, masterclient then starts the new round
        // TODO: DO NOT CALL assignPlayerWind
    }
    

    /// <summary>
    /// Called when the game ends
    /// </summary>
    public void EndGame() {
        Debug.LogError("The game has ended");
        // TODO: Prompt to start a new game
    }
}
