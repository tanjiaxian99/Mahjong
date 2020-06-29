using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour {

    #region SerializeField

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

    #region Derived Components 

    private Text generalInfoText;

    private Text leftPlayerText;

    private Text rightPlayerText;

    private Text oppositePlayerText;

    private Text localPlayerText;

    #endregion

    #region Singleton Initialization

    private static InfoPanel _instance;

    public static InfoPanel Instance { get { return _instance; } }

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

    #endregion

    private Dictionary<string, Text> playerText;

    private void Start() {
        generalInfoText = generalInfoTextObject.GetComponent<Text>();
        leftPlayerText = leftPlayerTextObject.GetComponent<Text>();
        rightPlayerText = rightPlayerTextObject.GetComponent<Text>();
        oppositePlayerText = oppositePlayerTextObject.GetComponent<Text>();
        localPlayerText = localPlayerTextObject.GetComponent<Text>();

        playerText = new Dictionary<string, Text>() {
            ["Left"] = leftPlayerText,
            ["Right"] = rightPlayerText,
            ["Opposite"] = oppositePlayerText,
            ["Local"] = localPlayerText
        };
        

        DefaultConfig();
    }

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

    /// <summary>
    /// Sets the local/remote player's points tally
    /// </summary>
    public void SetPlayerPoints(Player player, int points) {
        string position = RemotePlayer.RelativePlayerPosition(player);
        Text textField = playerText[position];

        string input = textField.text;
        string pattern = @"(\w*)(\s+)(-*\d*)";
        string replacement = player.NickName + "${2}" + points;
        string result = Regex.Replace(input, pattern, replacement);
        textField.text = result;
    }

    private void DefaultConfig() {
        showInfoButton.SetActive(true);
        infoPanel.SetActive(false);
        generalInfoTextObject.SetActive(true);
        leftPlayerTextObject.SetActive(true);
        rightPlayerTextObject.SetActive(true);
        oppositePlayerTextObject.SetActive(true);
        localPlayerTextObject.SetActive(true);
    }
}
