using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;

public class LocalizeWinCombos : MonoBehaviour, IResetVariables {

    public string winnerName;

    public string winLoseType;

    [SerializeField]
    private GameObject comboPanel;

    [SerializeField]
    private Text fanTotalText;

    [SerializeField]
    private GameObject scriptManager;

    private Dictionary<string, int> settingsDict;

    #region Singleton Initialization

    private static LocalizeWinCombos _instance;

    public static LocalizeWinCombos Instance { get { return _instance; } }

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }

        var settings = scriptManager.GetComponent<Settings>();
        settingsDict = settings.settingsDict;
    }

    #endregion

    // Call when there is a change in locale
    public void SetWinningCombos(List<string> winningCombos) {
        comboPanel.SetActive(true);

        for (int i = 0; i < winningCombos.Count; i++) {
            string winningCombo = winningCombos[i];
            Transform winComboTransform = comboPanel.transform.GetChild(i);
            winComboTransform.gameObject.SetActive(true);
            
            string entry = winningCombo.ToUpper();
            entry = entry.Replace(' ', '_');
            LocalizeStringEvent comboString = winComboTransform.GetChild(0).GetChild(0).gameObject.GetComponent<LocalizeStringEvent>();
            comboString.StringReference.SetReference("Room Settings", entry);

            int fan = settingsDict[winningCombo];
            Text fanText = winComboTransform.GetChild(1).GetChild(0).gameObject.GetComponent<Text>();
            fanText.text = fan + " " + GetFanTranslation();
        }
    }

    public void SetFanTotal(int fanTotal) {
        fanTotalText.gameObject.SetActive(true);
        fanTotalText.text = fanTotal + " " + GetFanTranslation();
    }

    public void SetWinLoseType(string entry) {
        var op = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("Win Lose Type", entry);
        winLoseType = op.Result;
    }

    // https://forum.unity.com/threads/localizating-strings-on-script.847000/
    public string GetFanTranslation() {
        var op = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("Secondary Text", "FAN");
        return op.Result;
    }

    public void ResetVariables() {
        winnerName = null;
        winLoseType = null;
    }
}
