﻿using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class UI : MonoBehaviour {

    #region SerializeField

    [SerializeField]
    private GameObject scriptManager;

    [SerializeField]
    private GameObject uiPanel;

    [SerializeField]
    private GameObject primaryText;

    [SerializeField]
    private GameObject spritesPanel;

    [SerializeField]
    private GameObject comboPanel;

    [SerializeField]
    private GameObject fanTotalText;

    [SerializeField]
    private GameObject okButtonObject;

    [SerializeField]
    private GameObject skipButtonObject;

    [SerializeField]
    private GameObject winLosePanel;

    #endregion

    #region Retrieved Components

    private WinManager winManager;

    private Text primaryTextField;

    private Text fanTotalTextField;

    private Button okButton;

    private Dictionary<string, int> settingsDict;

    private string uiType;

    #endregion

    #region Singleton Initialization

    private static UI _instance;

    public static UI Instance { get { return _instance; } }

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

    #endregion

    public void Start() {
        winManager = scriptManager.GetComponent<WinManager>();

        primaryTextField = primaryText.GetComponent<Text>();
        fanTotalTextField = fanTotalText.GetComponent<Text>();
        okButton = okButtonObject.GetComponent<Button>();

        Settings settings = scriptManager.GetComponent<Settings>();
        settingsDict = settings.settingsDict;

        DefaultUI();
    }

    #region UI Methods

    /// <summary>
    /// Wrapper coroutine for all UI Methods. Automatically queues up UI requests. 
    /// </summary>
    public IEnumerator GeneralUI(string type, params object[] objects) {
        while (uiPanel.activeSelf) {
            yield return null;
        }

        switch (type) {
            case "Instant Payout":
                InstantPayoutUI(objects);
                break;

            case "Revert Kong Payout":
                RevertKongPayoutUI(objects);
                break;

            case "Sacred Discard":
                SacredDiscardUI(objects);
                break;

            case "Missed Discard":
                MissedDiscardUI(objects);
                break;

            case "Can Win":
                CanWinUI(objects);
                break;

            case "Win Ok":
                OnWinOkUI(objects);
                break;

            case "Remote Win":
                RemoteWinUI(objects);
                break;

            case "No More Tiles":
                NoMoreTilesUI();
                break;
        }

    }

    /// <summary>
    /// Called when an Instant Payout is to be made
    /// </summary>
    private void InstantPayoutUI(params object[] objects) {
        uiPanel.SetActive(true);
        LocalizePrimaryText.Instance.SetPrimaryText("INSTANT_PAYOUT_" + (string)objects[0]);
        uiType = "Instant Payout";
        AddSpriteTiles((List<Tile>)objects[1]);
    }

    /// <summary>
    /// Called when Kong Payout is to be reverted
    /// </summary>
    private void RevertKongPayoutUI(params object[] objects) {
        uiPanel.SetActive(true);
        LocalizePrimaryText.Instance.SetPrimaryText("INSTANT_PAYOUT_REVERT_KONG");
        uiType = "Revert Kong";
        AddSpriteTiles((List<Tile>)objects[0]);
    }

    /// <summary>
    /// Called when a Sacred Discard tile has been discarded
    /// </summary>
    private void SacredDiscardUI(params object[] objects) {
        uiPanel.SetActive(true);
        LocalizePrimaryText.Instance.SetPrimaryText("INSTANT_PAYOUT_SACRED_DISCARD");
        uiType = "Sacred Discard";
        AddSpriteTiles(new List<Tile>() { (Tile)objects[0] });
    }

    /// <summary>
    /// Called when a Missed Discard tile has been discarded
    /// </summary>
    private void MissedDiscardUI(params object[] objects) {
        uiPanel.SetActive(true);
        LocalizePrimaryText.Instance.SetPrimaryText("INSTANT_PAYOUT_MISSED_DISCARD");
        uiType = "Missed Discard";
        AddSpriteTiles(new List<Tile>() { (Tile)objects[0] });
    }

    private void CanWinUI(params object[] objects) {
        uiPanel.SetActive(true);
        skipButtonObject.SetActive(true);
        LocalizePrimaryText.Instance.SetPrimaryText("CAN_WIN");
        uiType = "Can Win";
        AddSpriteTiles(new List<Tile>() { (Tile)objects[0] });

        LocalizeWinCombos.Instance.SetFanTotal((int)objects[1]);        
    }

    private void OnWinOkUI(params object[] objects) {
        uiPanel.SetActive(true);
        LocalizePrimaryText.Instance.SetPrimaryText("YOU_HAVE_WON");
        uiType = "You have won!";

        Tile winningTile = (Tile)objects[0];
        int fanTotal = (int)objects[1];
        List<string> winningCombos = (List<string>)objects[2];

        AddSpriteTiles(new List<Tile>() { winningTile });

        LocalizeWinCombos.Instance.SetWinningCombos(winningCombos);
        LocalizeWinCombos.Instance.SetFanTotal(fanTotal);
        LocalizeWinLoseType.Instance.SetWinLosePanel();
        winLosePanel.SetActive(true);
    }

    private void RemoteWinUI(params object[] objects) {
        uiPanel.SetActive(true);
        LocalizePrimaryText.Instance.SetPrimaryText("ANOTHER_PLAYER_HAS_WON");
        uiType = "Another player has won";

        Player winner = (Player)objects[0];
        Tile winningTile = (Tile)objects[1];
        int fanTotal = (int)objects[2];
        List<string> winningCombos = (List<string>)objects[3];

        AddSpriteTiles(new List<Tile>() { winningTile });

        LocalizeWinCombos.Instance.winnerName = winner.NickName;
        LocalizeWinCombos.Instance.SetFanTotal(fanTotal);
        LocalizeWinCombos.Instance.SetWinningCombos(winningCombos);
        LocalizeWinLoseType.Instance.SetWinLosePanel();
        winLosePanel.SetActive(true);
    }

    private void NoMoreTilesUI() {
        uiPanel.SetActive(true);
        LocalizePrimaryText.Instance.SetPrimaryText("THERE_ARE_NO_MORE_TILES_LEFT");
        uiType = "There are no more tiles left";
    }

    #endregion

    /// <summary>
    /// Helper function that adds sprites to either the Primary or Secondary Sprites Panel
    /// </summary>
    private void AddSpriteTiles(List<Tile> tiles) {
        for (int i = 0; i < tiles.Count; i++) {
            Transform imageTransform = spritesPanel.transform.GetChild(i);
            Image image = imageTransform.GetComponent<Image>();
            image.sprite = DictManager.Instance.spritesDict[tiles[i]];
            imageTransform.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Called upon clicking the 'Ok' button
    /// </summary>
    public void OnOk() {
        switch (uiType) {
            case "Instant Payout":
            case "Revert Kong":
            case "Sacred Discard":
            case "Missed Discard":
                ResetUI();
                break;

            case "Can Win":
                ResetUI();
                winManager.OnWinOk();
                break;

            case "You have won!":
                ResetUI();
                EventsManager.EventReadyForNewRound();
                break;

            case "Another player has won":
                ResetUI();
                EventsManager.EventReadyForNewRound();
                break;

            case "There are no more tiles left":
                ResetUI();
                EventsManager.EventReadyForNewRound();
                break;
        }

        
    }

    /// <summary>
    /// Called upon clicking the 'Skip' button
    /// </summary>
    public void OnSkip() {
        winManager.OnWinSkip();
        ResetUI();
    }

    /// <summary>
    /// The default UI configuration
    /// </summary>
    private void DefaultUI() { 
        uiPanel.SetActive(false);

        primaryText.SetActive(true);
        primaryTextField.text = "";

        comboPanel.SetActive(true);
        foreach (Transform child in comboPanel.transform) {
            child.gameObject.SetActive(false);
        }

        spritesPanel.SetActive(true);
        foreach (Transform imageTransform in spritesPanel.transform) {
            imageTransform.gameObject.SetActive(false);
            Image image = imageTransform.GetComponent<Image>();
            image.color = new Color(1f, 1f, 1f);
        }

        fanTotalText.SetActive(true);
        fanTotalTextField.text = "";

        okButtonObject.SetActive(true);
        skipButtonObject.SetActive(false);

        winLosePanel.SetActive(false);
        foreach (Transform child in winLosePanel.transform) {
            child.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Closes the UI and resets UI elements to their default state
    /// </summary>
    private void ResetUI() {
        uiPanel.SetActive(false);
        
        foreach (Transform child in comboPanel.transform) {
            child.gameObject.SetActive(false);
        }

        foreach (Transform imageTransform in spritesPanel.transform) {
            imageTransform.gameObject.SetActive(false);
            Image image = imageTransform.GetComponent<Image>();
            image.color = new Color(1f, 1f, 1f);
        }

        fanTotalTextField.text = "";

        skipButtonObject.SetActive(false);

        winLosePanel.SetActive(false);
        foreach (Transform child in winLosePanel.transform) {
            child.gameObject.SetActive(false);
        }
    }
}