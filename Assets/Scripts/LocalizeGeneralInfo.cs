using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class LocalizeGeneralInfo : MonoBehaviour {

    private string seatWind;

    public string SeatWind {
        get { return seatWind; }
        set {
            var op = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("Info", value);
            seatWind = op.Result;
        }
    }

    private string prevailingWind;

    public string PrevailingWind {
        get { return prevailingWind; }
        set {
            var op = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("Info", value + "");
            prevailingWind = op.Result;
        }
    }

    private int numberOfTilesLeft;

    public int NumberOfTilesLeft { get; set; }

    #region Singleton Initialization

    private static LocalizeGeneralInfo _instance;

    public static LocalizeGeneralInfo Instance { get { return _instance; } }

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

    #endregion

}
