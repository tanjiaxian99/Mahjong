using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using System.Linq;

public class EndRound : MonoBehaviour {

    [SerializeField]
    private GameObject scriptManager;

    private PlayerManager playerManager;

    private TilesManager tilesManager;

    private Payment payment;

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
        playerManager = scriptManager.GetComponent<PlayerManager>();
        payment = scriptManager.GetComponent<Payment>();
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
        ResetAllVariables();
    }

    /// <summary>
    /// Inform other players of the local player's hand tiles
    /// </summary>
    public void UpdateHandTiles() {
        // TODO: Update player's hand after Robbing the Kong
        PropertiesManager.SetOpenHand(tilesManager.hand);
    }

    public void DisplayWinningCombo(Player winner, int fanTotal, List<string> winningCombos) {
        // TODO: Display winning combos / fan
    }

    /// <summary>
    /// Reset variables which do not persist between rounds
    /// </summary>
    public void ResetAllVariables() {
        // TODO: Any way to make use of IResetVariables?
        EventsManager.ResetVariables();
        scriptManager.GetComponent<PlayerManager>().ResetVariables();
        scriptManager.GetComponent<TilesManager>().ResetVariables();
        scriptManager.GetComponent<SacredDiscardManager>().ResetVariables();
        scriptManager.GetComponent<MissedDiscardManager>().ResetVariables();
        scriptManager.GetComponent<FanCalculator>().ResetVariables();
        scriptManager.GetComponent<Payment>().ResetVariables();
    }

    // TODO: Clear gameTable

    // TODO: Recalculate playOrder & prevailing


    // TODO: Call InitializeRound to start the new round
    public static void StartNewRound() {
        // TODO: Every player press ok, masterclient then starts the new round
        // TODO: DO NOT CALL assignPlayerWind
    }
    

    // TODO: If next round is East/East, end the game
}
