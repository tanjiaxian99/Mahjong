using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {
    // TODO: Revert Kong Payout, OnWinOk, Winning, EndGame, Number of tiles left, Start new round, Seat Wind, Prevailing Wind, Nicknames, Points

    #region SerializeField

    [SerializeField]
    private GameObject uiPanel;

    [SerializeField]
    private GameObject primaryText;

    [SerializeField]
    private GameObject secondarySpritesPanel;

    [SerializeField]
    private GameObject primarySpritesPanel;

    [SerializeField]
    private GameObject secondaryText;

    [SerializeField]
    private GameObject okButtonObject;

    [SerializeField]
    private GameObject skipButtonObject;

    #endregion

    #region Retrieved Components

    private Text primaryTextField;

    private Text secondaryTextField;

    private Button okButton;

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
        primaryTextField = primaryText.GetComponent<Text>();
        secondaryTextField = secondaryText.GetComponent<Text>();
        okButton = okButtonObject.GetComponent<Button>();

        DefaultUI();
    }

    #region UI Methods

    /// <summary>
    /// Wrapper coroutine for all UI Methods. Automatically queues up UI requests. 
    /// </summary>
    // TODO: Test 3 cases of different types (e.g. kong, bonus pair, win)
    public IEnumerator GeneralUI(string type, params object[] objects) {
        while (uiPanel.activeSelf) {
            yield return null;
        }

        switch (type) {
            case "Instant Payout":
                InstantPayoutUI(objects);
                break;
            case "Sacred Discard":
                SacredDiscardUI(objects);
                Debug.LogError("called");
                break;
            case "Missed Discard":
                MissedDiscardUI(objects);
                break;
        }
        
    }

    /// <summary>
    /// Called when an Instant Payout is to be made
    /// </summary>
    private void InstantPayoutUI(params object[] objects) {
        uiPanel.SetActive(true);
        primaryTextField.text = "Instant Payout: " + (string)objects[0];
        AddSpriteTiles((List<Tile>)objects[1]);
    }

    /// <summary>
    /// Called when a Sacred Discard tile has been discarded
    /// </summary>
    private void SacredDiscardUI(params object[] objects) {
        uiPanel.SetActive(true);
        primaryTextField.text = "Sacred Discard";
        AddSpriteTiles(new List<Tile>() { (Tile)objects[0] });
    }

    /// <summary>
    /// Called when a Missed Discard tile has been discarded
    /// </summary>
    private void MissedDiscardUI(params object[] objects) {
        uiPanel.SetActive(true);
        primaryTextField.text = "Missed Discard";
        AddSpriteTiles(new List<Tile>() { (Tile)objects[0] });
    }

    #endregion

    /// <summary>
    /// Helper function that adds sprites to either the Primary or Secondary Sprites Panel
    /// </summary>
    private void AddSpriteTiles(List<Tile> tiles, string type = "Primary") {
        GameObject spritesPanel = (type == "Primary") ? primarySpritesPanel : secondarySpritesPanel;

        for (int i = 0; i < tiles.Count; i++) {
            Transform imageTransform = primarySpritesPanel.transform.GetChild(i);
            Image image = imageTransform.GetComponent<Image>();
            image.sprite = DictManager.Instance.spritesDict[tiles[i]];
            imageTransform.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Called upon clicking the 'Ok' button
    /// </summary>
    public void OnOk() {
        switch (primaryTextField.text) {
            case "Sacred Discard":
            case "Missed Discard":
            case "Instant Payout: Hidden Cat and Rat":
            case "Instant Payout: Cat and Rat":
            case "Instant Payout: Hidden Chicken and Centipede":
            case "Instant Payout: Chicken and Centipede":
            case "Instant Payout: Complete Animal Group Payout":
            case "Instant Payout: Hidden Bonus Tile Match Seat Wind Pair":
            case "Instant Payout: Bonus Tile Match Seat Wind Pair":
            case "Instant Payout: Complete Season Group Payout":
            case "Instant Payout: Complete Flower Group Payout":
            case "Instant Payout: Concealed Kong":
            case "Instant Payout: Exposed Kong":
            case "Instant Payout: Discard Kong":
                ResetUI();
                break;
        }
    }

    /// <summary>
    /// The default UI configuration
    /// </summary>
    private void DefaultUI() {
        uiPanel.SetActive(false);

        primaryText.SetActive(true);
        primaryTextField.text = "";

        secondaryText.SetActive(true);
        secondaryTextField.text = "";

        primarySpritesPanel.SetActive(true);
        foreach (Transform imageTransform in primarySpritesPanel.transform) {
            imageTransform.gameObject.SetActive(false);
            Image image = imageTransform.GetComponent<Image>();
            image.color = new Color(1f, 1f, 1f);
        }

        secondarySpritesPanel.SetActive(false);
        foreach (Transform imageTransform in secondarySpritesPanel.transform) {
            imageTransform.gameObject.SetActive(false);
            Image image = imageTransform.GetComponent<Image>();
            image.color = new Color(1f, 1f, 1f);
        }

        okButtonObject.SetActive(true);
        skipButtonObject.SetActive(false);
    }

    /// <summary>
    /// Closes the UI and resets UI elements to their default state
    /// </summary>
    private void ResetUI() {
        uiPanel.SetActive(false);
        skipButtonObject.SetActive(false);

        foreach (Transform imageTransform in primarySpritesPanel.transform) {
            imageTransform.gameObject.SetActive(false);
            Image image = imageTransform.GetComponent<Image>();
            image.color = new Color(1f, 1f, 1f);
        }
        foreach (Transform imageTransform in secondarySpritesPanel.transform) {
            imageTransform.gameObject.SetActive(false);
            Image image = imageTransform.GetComponent<Image>();
            image.color = new Color(1f, 1f, 1f);
        }
        secondaryTextField.text = "";
    }
}
