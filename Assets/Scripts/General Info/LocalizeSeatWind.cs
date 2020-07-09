using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LocalizeSeatWind : MonoBehaviour {

    private string seatWind;

    public string SeatWind {
        get {
            return seatWind;
        }
        set {
            var op = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("Info", value);
            seatWind = op.Result;
        }
    }

    #region Singleton Initialization

    private static LocalizeSeatWind _instance;

    public static LocalizeSeatWind Instance { get { return _instance; } }

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

    #endregion
}
