using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;

public class LocalizeSecondaryText : MonoBehaviour, IResetVariables {

    public string winnerName;

    public int fanTotal;

    public string combos;

    public string winLoseType;

    [SerializeField]
    private GameObject scriptManager;

    private Dictionary<string, int> settingsDict;

    #region Singleton Initialization

    private static LocalizeSecondaryText _instance;

    public static LocalizeSecondaryText Instance { get { return _instance; } }

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

    // https://forum.unity.com/threads/localizating-strings-on-script.847000/
    public void SetWinningCombos(List<string> winningCombos) {
        combos = "";

        foreach (string combo in winningCombos) {
            string entry = combo.ToUpper();
            entry = entry.Replace(' ', '_');

            var op = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("Winning Combos", entry);
            var o = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("Secondary Text", "FAN");
            string translation = op.Result;
            string fanTranslation = o.Result;

            if (combo.Contains("Dragon")) {
                combos += translation + " = " + settingsDict["Dragon"] + " " + fanTranslation + "\n";
            } else {
                combos += translation + " = " + settingsDict[combo] + " " + fanTranslation + "\n";
            }
        }
        combos = combos.TrimEnd('\n');
    }

    public void SetWinLoseType(string entry) {
        var op = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("Win Lose Type", entry);
        winLoseType = op.Result;
    }

    // TODO: Call whenever there is a change in locale
    public string FanTranslation() {
        var op = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("Secondary Text", "FAN");
        string translation = "";

        if (op.IsDone)
            translation = op.Result;
        else
            op.Completed += (x) => translation = x.Result;

        return translation;
    }

    public void ResetVariables() {
        winnerName = null;
        fanTotal = 0;
        combos = "";
        winLoseType = null;
    }
}
