using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UIElements;

public class LocalizeWinLoseType : MonoBehaviour, IResetVariables {

    [SerializeField]
    private Text leftPlayerText;

    [SerializeField]
    private Text rightPlayerText;

    [SerializeField]
    private Text oppositePlayerText;

    [SerializeField]
    private Text localPlayerText;

    private LocalizeStringEvent leftPlayerString;

    private LocalizeStringEvent rightPlayerString;

    private LocalizeStringEvent oppositePlayerString;

    private LocalizeStringEvent localPlayerString;

    [SerializeField]
    private GameObject scriptManager;

    private GameManager gameManager;

    private Dictionary<string, Player> winLoseDict;

    private bool shooterPay;

    private Dictionary<string, Text> textDict;

    private Dictionary<string, LocalizeStringEvent> stringDict;

    #region Singleton Initialization

    private static LocalizeWinLoseType _instance;

    public static LocalizeWinLoseType Instance { get { return _instance; } }

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

    #endregion

    void Start() {
        leftPlayerString = leftPlayerText.GetComponent<LocalizeStringEvent>();
        rightPlayerString = rightPlayerText.GetComponent<LocalizeStringEvent>();
        oppositePlayerString = oppositePlayerText.GetComponent<LocalizeStringEvent>();
        localPlayerString = localPlayerText.GetComponent<LocalizeStringEvent>();

        gameManager = scriptManager.GetComponent<GameManager>();
        shooterPay = scriptManager.GetComponent<Settings>().settingsDict["Shooter Pay"] != 0;

        textDict = new Dictionary<string, Text>() {
            ["Left"] = leftPlayerText,
            ["Right"] = rightPlayerText,
            ["Opposite"] = oppositePlayerText,
            ["Local"] = localPlayerText
        };

        stringDict = new Dictionary<string, LocalizeStringEvent>() {
            ["Left"] = leftPlayerString,
            ["Right"] = rightPlayerString,
            ["Opposite"] = oppositePlayerString,
            ["Local"] = localPlayerString
        };
    }

    public void WinLoseType(Player winner, Player discardPlayer, int numberOfTilesLeft, bool isFreshTile) {
        winLoseDict = new Dictionary<string, Player>();

        if (discardPlayer == null) {
            winLoseDict.Add("SELF_PICK", winner);
        } else {
            winLoseDict.Add("NORMAL_WIN", winner);
        }

        // Fresh Tile Mahjong Scenario
        if (numberOfTilesLeft < 20 && isFreshTile && discardPlayer != null) {
            winLoseDict.Add("FRESH_TILE_DISCARD", discardPlayer);
            return;
        }

        // Paying for all players
        if (gameManager.payAllPlayer != null) {
            winLoseDict.Add("PAY_FOR_ALL", gameManager.payAllPlayer);
            return;
        }

        // Shooter pay
        if (shooterPay && discardPlayer != null) {
            winLoseDict.Add("SHOOTER", discardPlayer);
            return;
        }

        // Non-shooter pay
        if (discardPlayer != null) {
            winLoseDict.Add("DISCARD", discardPlayer);
        }
    }

    public void SetWinLosePanel() {
        foreach (string entry in winLoseDict.Keys) {
            Player player = winLoseDict[entry];
            string remotePos = RemotePlayer.RelativePlayerPosition(player);
            LocalizeStringEvent stringEvent = stringDict[remotePos];
            stringEvent.StringReference.SetReference("Win Lose Type", entry);

            Text playerText = textDict[remotePos];
            playerText.transform.parent.gameObject.SetActive(true);
        }
    }

    public void ResetVariables() {
        leftPlayerString.StringReference.SetReference("", "");
        rightPlayerString.StringReference.SetReference("", "");
        oppositePlayerString.StringReference.SetReference("", "");
        localPlayerString.StringReference.SetReference("", "");
        winLoseDict.Clear();
    }
}
