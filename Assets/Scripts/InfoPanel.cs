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
    private GameObject leftPlayer;

    [SerializeField]
    private GameObject rightPlayer;

    [SerializeField]
    private GameObject oppositePlayer;

    [SerializeField]
    private GameObject localPlayer;

    [SerializeField]
    private Text seatWindText;

    [SerializeField]
    private Text prevailingWindText;

    [SerializeField]
    private Text tilesLeftText;

    #endregion

    #region Derived Components 

    private Text generalInfoText;

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

        infoPanel.SetActive(true);
    }

    #endregion

    private Dictionary<string, Text> playerText;

    private void Start() {
        playerText = new Dictionary<string, Text>() {
            ["Left"] = leftPlayer.GetComponentInChildren<Text>(),
            ["Right"] = rightPlayer.GetComponentInChildren<Text>(),
            ["Opposite"] = oppositePlayer.GetComponentInChildren<Text>(),
            ["Local"] = localPlayer.GetComponentInChildren<Text>()
        };

        DefaultConfig();
    }

    /// <summary>
    /// Called at the start of the round to display the Prevailing Wind
    /// </summary>
    public IEnumerator ShowPrevailingWind() {
        infoPanel.SetActive(true);
        infoPanel.GetComponent<Image>().enabled = false;
        prevailingWindText.gameObject.SetActive(true);

        yield return new WaitForSeconds(5f);

        infoPanel.SetActive(false);
        infoPanel.GetComponent<Image>().enabled = true;
        prevailingWindText.gameObject.SetActive(false);
    }

    /// <summary>
    /// Called upon clicking the 'Show Info' button
    /// </summary>
    public void ShowInfo() {
        if (!infoPanel.activeSelf) {
            infoPanel.SetActive(true);

            foreach (Transform child in generalInfoTextObject.transform) {
                child.gameObject.SetActive(true);
            }

            leftPlayer.SetActive(true);
            rightPlayer.SetActive(true);
            oppositePlayer.SetActive(true);
            localPlayer.SetActive(true);
        }
        closeInfoButton.SetActive(true);
    }

    /// <summary>
    /// Called upon clicking the 'Close Info' button
    /// </summary>
    public void CloseInfo() {
        if (infoPanel.activeSelf) {
            infoPanel.SetActive(false);

            foreach (Transform child in generalInfoTextObject.transform) {
                child.gameObject.SetActive(false);
            }

            leftPlayer.SetActive(false);
            rightPlayer.SetActive(false);
            oppositePlayer.SetActive(false);
            localPlayer.SetActive(false);
        }
        closeInfoButton.SetActive(false);
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
        foreach (Transform child in generalInfoTextObject.transform) {
            child.gameObject.SetActive(false);
        }

        leftPlayer.SetActive(false);
        rightPlayer.SetActive(false);
        oppositePlayer.SetActive(false);
        localPlayer.SetActive(false);
    }
}
