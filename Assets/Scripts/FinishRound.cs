using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using System.Linq;
using System;
using Photon.Pun;

public class FinishRound : MonoBehaviour {

    [SerializeField]
    private GameObject scriptManager;

    private GameManager gameManager;

    private PlayerManager playerManager;

    private TilesManager tilesManager;

    #region Singleton Initialization

    private static FinishRound _instance;

    public static FinishRound Instance { get { return _instance; } }

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

    #endregion

    private void Start() {
        gameManager = scriptManager.GetComponent<GameManager>();
        playerManager = scriptManager.GetComponent<PlayerManager>();
        tilesManager = scriptManager.GetComponent<TilesManager>();
    }

    /// <summary>
    /// Wrapper function which when called, ends the round
    /// </summary>
    /// <param name="winner"></param>
    /// <param name="fanTotal"></param>
    /// <param name="winningCombos"></param>
    public void EndRound(Player winner) {
        playerManager.InstantiateLocalHand();
        playerManager.InstantiateLocalOpenTiles();
        UpdateHandTiles();
        NewPlayOrder(winner);

        if (winner == null) {
            StartCoroutine(UI.Instance.GeneralUI("No More Tiles"));
        }
    }

    /// <summary>
    /// Inform other players of the local player's hand tiles
    /// </summary>
    private void UpdateHandTiles() {
        PropertiesManager.SetOpenHand(tilesManager.hand);
    }

    /// <summary>
    /// Determine the new play order
    /// </summary>
    private void NewPlayOrder(Player winner) {
        if (!PhotonNetwork.IsMasterClient) {
            return;
        }

        if (winner == null) {
            return;
        }

        Player[] currentPlayOrder = PropertiesManager.GetPlayOrder();
        if (winner == null || winner == currentPlayOrder[0]) {
            return;
        }

        Player[] newPlayOrder = new Player[4];
        Array.Copy(currentPlayOrder, 1, newPlayOrder, 0, 3);
        newPlayOrder[3] = currentPlayOrder[0];

        PropertiesManager.SetPlayOrder(newPlayOrder);

        if (Enumerable.SequenceEqual(newPlayOrder, PropertiesManager.GetInitialPlayOrder())) {
            NewPrevailingWind();
        }
    }

    /// <summary>
    /// Determine the new prevailing wind
    /// </summary>
    private void NewPrevailingWind() {
        if (!PhotonNetwork.IsMasterClient) {
            return;
        }

        PlayerManager.Wind currentPrevailingWind = PropertiesManager.GetPrevailingWind();
        PlayerManager.Wind newPrevailingWind = currentPrevailingWind + 1;

        if ((int)newPrevailingWind == 4) {
            EventsManager.EventEndGame();
            return;
        }

        PropertiesManager.SetPrevailingWind(newPrevailingWind);
    }

    /// <summary>
    /// Called when all players are ready for a new round
    /// </summary>
    public void StartNewRound() {
        ResetAllVariables();
        ClearGameTable();
        InitializeNewRound();
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
        scriptManager.GetComponent<Payment>().ResetVariables();
        LocalizeWinCombos.Instance.ResetVariables();
        LocalizeWinLoseType.Instance.ResetVariables();
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
    /// The MasterClient will start a new round
    /// </summary>
    public void InitializeNewRound() {
        if (!PhotonNetwork.IsMasterClient) {
            return;
        }
        StartCoroutine(InitializeRound.InitializeGame(gameManager, gameManager.numberOfTilesLeft, "New Round"));
    }

    /// <summary>
    /// Called when the game ends
    /// </summary>
    public void EndGame() {
        Debug.LogError("The game has ended");
        // TODO: Prompt to start a new game
    }
}
