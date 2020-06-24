using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {
    // TODO: Instant Payout, OnWinOk, Winning, EndGame, Number of tiles left, Start new round, Seat Wind, Prevailing Wind, Nicknames, Points

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
    private GameObject okButton;

    [SerializeField]
    private GameObject skipButton;

    #endregion

    #region Retrieved Components

    private Text primaryTextField;

    private Text secondaryTextField;

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
    }

    /// <summary>
    /// Called when a Sacred Discard tile has been discarded
    /// </summary>
    public void SacredDiscardUI(Tile sacredDiscard) {
        uiPanel.SetActive(true);

        primaryTextField.text = "Sacred Discard";

        Transform imageTransform = primarySpritesPanel.transform.GetChild(0);
        Image image = imageTransform.GetComponent<Image>();
        image.sprite = DictManager.Instance.spritesDict[sacredDiscard];
        imageTransform.gameObject.SetActive(true);
    }

    /// <summary>
    /// Called when a Missed Discard tile has been discarded
    /// </summary>
    public void MissedDiscardUI(Tile missedDiscard) {
        uiPanel.SetActive(true);

        primaryTextField.text = "Missed Discard";

        Transform imageTransform = primarySpritesPanel.transform.GetChild(0);
        Image image = imageTransform.GetComponent<Image>();
        image.sprite = DictManager.Instance.spritesDict[missedDiscard];
        imageTransform.gameObject.SetActive(true);
    }

    /// <summary>
    /// Called upon clicking the 'Ok' button
    /// </summary>
    public void OnOk() {
        switch (primaryTextField.text) {
            case "Sacred Discard":
            case "Missed Discard":
                ResetUI();
                break;
        }
        
    }

    /// <summary>
    /// Closes the UI and resets UI elements to their default state
    /// </summary>
    public void ResetUI() {
        uiPanel.SetActive(false);
        skipButton.SetActive(false);

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
