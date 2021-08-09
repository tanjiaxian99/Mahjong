using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

/// <summary>
/// Player name input field. For use in lobby.
/// </summary>
[RequireComponent(typeof(InputField))]
public class PlayerNameInputField : MonoBehaviour {
    #region Private Fields

    // Name key for PlayerPref
    const string playerNamePrefKey = "PlayerName";

    #endregion

    #region Monobehaviour Callbacks

    void Start() {
        string defaultName = string.Empty;
        InputField _inputField = this.GetComponent<InputField>();

        if (_inputField != null) {

            // If the player has entered a name before, retrieve that name
            if (PlayerPrefs.HasKey(playerNamePrefKey)) {
                defaultName = PlayerPrefs.GetString(playerNamePrefKey);
                // Show stored name in input textfield
                _inputField.text = defaultName;
            }
        }

        PhotonNetwork.NickName = defaultName;
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Sets the name of the player, and save it in the PlayerPrefs for future sessions.
    /// </summary>
    public void SetPlayerName(string inputName) {
        if (string.IsNullOrEmpty(inputName)) {
            Debug.LogError("Player Name is null or empty");
            return;
        }
        PhotonNetwork.NickName = inputName;
        // Store the inputName in PlayerPrefs
        PlayerPrefs.SetString(playerNamePrefKey, inputName);
    }

    #endregion
}
