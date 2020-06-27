using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class UI : MonoBehaviour {
    // TODO: Revert Kong Payout, Number of tiles left, Seat Wind, Prevailing Wind, Nicknames, Points, Update points custom properties, shooter pay kong 

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
    private GameObject secondaryText;

    [SerializeField]
    private GameObject okButtonObject;

    [SerializeField]
    private GameObject skipButtonObject;

    [SerializeField]
    private GameObject showInfoButton;

    [SerializeField]
    private GameObject closeInfoButton;

    [SerializeField]
    private GameObject infoPanel;

    [SerializeField]
    private GameObject generalInfoTextObject;

    [SerializeField]
    private GameObject leftPlayerTextObject;

    [SerializeField]
    private GameObject rightPlayerTextObject;

    [SerializeField]
    private GameObject oppositePlayerTextObject;

    [SerializeField]
    private GameObject localPlayerTextObject;

    #endregion

    #region Retrieved Components

    private WinManager winManager;

    private Text primaryTextField;

    private Text secondaryTextField;

    private Button okButton;

    private Dictionary<string, int> settingsDict;

    private Text generalInfoText;

    private Text leftPlayerText;

    private Text rightPlayerText;

    private Text oppositePlayerText;

    private Text localPlayerText;

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
        secondaryTextField = secondaryText.GetComponent<Text>();
        okButton = okButtonObject.GetComponent<Button>();

        Settings settings = scriptManager.GetComponent<Settings>();
        settingsDict = settings.settingsDict;

        generalInfoText = generalInfoTextObject.GetComponent<Text>();
        leftPlayerText = leftPlayerTextObject.GetComponent<Text>();
        rightPlayerText = rightPlayerTextObject.GetComponent<Text>();
        oppositePlayerText = oppositePlayerTextObject.GetComponent<Text>();
        localPlayerText = localPlayerTextObject.GetComponent<Text>();

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

    private void CanWinUI(params object[] objects) {
        uiPanel.SetActive(true);
        skipButtonObject.SetActive(true);
        primaryTextField.text = "Can Win";
        AddSpriteTiles(new List<Tile>() { (Tile)objects[0] });
        
        secondaryTextField.text = string.Format("You can win with {0} fan.", (int)objects[1]);
    }

    private void OnWinOkUI(params object[] objects) {
        uiPanel.SetActive(true);
        primaryTextField.text = "You have won!";

        Tile winningTile = (Tile)objects[0];
        int fanTotal = (int)objects[1];
        List<string> winningCombos = (List<string>)objects[2];
        string winLoseType = (string)objects[3];

        AddSpriteTiles(new List<Tile>() { winningTile });

        string combos = "";
        foreach (string combo in winningCombos) {
            if (combo.Contains("Dragon")) {
                combos += combo + " = " + settingsDict["Dragon"] + " fan" + "\n";
            } else {
                combos += combo + " = " + settingsDict[combo] + " fan" + "\n";
            }  
        }
        combos = combos.TrimEnd('\n');
        secondaryTextField.text = string.Format("You won with {0} fan and the following combos:\n{1}\n\n{2}", fanTotal, combos, winLoseType);
    }

    private void RemoteWinUI(params object[] objects) {
        uiPanel.SetActive(true);
        primaryTextField.text = "Another player has won";

        Player winner = (Player)objects[0];
        Tile winningTile = (Tile)objects[1];
        int fanTotal = (int)objects[2];
        List<string> winningCombos = (List<string>)objects[3];
        string winLoseType = (string)objects[4];

        AddSpriteTiles(new List<Tile>() { winningTile });

        string combos = "";
        foreach (string combo in winningCombos) {
            if (combo.Contains("Dragon")) {
                combos += combo + " = " + settingsDict["Dragon"] + " fan" + "\n";
            } else {
                combos += combo + " = " + settingsDict[combo] + " fan" + "\n";
            }
        }
        combos = combos.TrimEnd('\n');
        secondaryTextField.text = string.Format(
            "{0} has won with {1} fan and the following combos:\n{2}\n\n{3}", 
            winner.NickName, fanTotal, combos, winLoseType);
    }

    #endregion

    #region General Info

    /// <summary>
    /// Called upon clicking the 'Show Info' button
    /// </summary>
    public void ShowInfo() {
        if (!infoPanel.activeSelf) {
            infoPanel.SetActive(true);
        }
        closeInfoButton.SetActive(true);
    }

    /// <summary>
    /// Called upon clicking the 'Close Info' button
    /// </summary>
    public void CloseInfo() {
        if (infoPanel.activeSelf) {
            infoPanel.SetActive(false);
        }
        closeInfoButton.SetActive(false);
    }

    /// <summary>
    /// Update the seat wind of the local player in the info panel
    /// </summary>
    public void SetSeatWind(PlayerManager.Wind seatWind) {
        string input = generalInfoText.text;
        string pattern = @"(Seat Wind: )(\w*)(\s+Prevailing Wind: )(\w*)(\s+Number Of Tiles Left: )(\d*)";
        string replacement = string.Format("$1{0}$3$4$5$6", seatWind);
        string result = Regex.Replace(input, pattern, replacement);
        generalInfoText.text = result;
    }

    /// <summary>
    /// Update the prevailing wind in the info panel
    /// </summary>
    public void SetPrevailingWind(PlayerManager.Wind prevailingWind) {
        string input = generalInfoText.text;
        string pattern = @"(Seat Wind: )(\w*)(\s+Prevailing Wind: )(\w*)(\s+Number Of Tiles Left: )(\d*)";
        string replacement = string.Format("$1$2$3{0}$5$6", prevailingWind);
        string result = Regex.Replace(input, pattern, replacement);
        generalInfoText.text = result;
    }

    /// <summary>
    /// Update the number of tiles left in the info panel
    /// </summary>
    /// <param name="tilesLeft"></param>
    public void SetNumberOfTilesLeft(int tilesLeft) {
        string input = generalInfoText.text;
        string pattern = @"(Seat Wind: )(\w*)(\s+Prevailing Wind: )(\w*)(\s+Number Of Tiles Left: )(\d*)";
        string replacement = "$1$2$3$4${5}" + tilesLeft;
        string result = Regex.Replace(input, pattern, replacement);
        generalInfoText.text = result;
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
        }
    }

    /// <summary>
    /// Called upon clicking the 'Skip' button
    /// </summary>
    public void OnSkip() {
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
            case "Can Win":
                winManager.OnWinSkip();
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

        spritesPanel.SetActive(true);
        foreach (Transform imageTransform in spritesPanel.transform) {
            imageTransform.gameObject.SetActive(false);
            Image image = imageTransform.GetComponent<Image>();
            image.color = new Color(1f, 1f, 1f);
        }

        okButtonObject.SetActive(true);
        skipButtonObject.SetActive(false);

        showInfoButton.SetActive(true);
        infoPanel.SetActive(false);
        generalInfoTextObject.SetActive(true);
        leftPlayerTextObject.SetActive(true);
        rightPlayerTextObject.SetActive(true);
        oppositePlayerTextObject.SetActive(true);
        localPlayerTextObject.SetActive(true);
    }

    /// <summary>
    /// Closes the UI and resets UI elements to their default state
    /// </summary>
    private void ResetUI() {
        uiPanel.SetActive(false);
        skipButtonObject.SetActive(false);

        foreach (Transform imageTransform in spritesPanel.transform) {
            imageTransform.gameObject.SetActive(false);
            Image image = imageTransform.GetComponent<Image>();
            image.color = new Color(1f, 1f, 1f);
        }

        secondaryTextField.text = "";
    }
}
