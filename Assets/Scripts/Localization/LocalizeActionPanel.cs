using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Components;
using Photon.Realtime;

public class LocalizeActionPanel : MonoBehaviour {

    [SerializeField]
    private GameObject actionPanel;

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

    private Dictionary<string, Text> textDict;

    private Dictionary<string, LocalizeStringEvent> stringDict;

    #region Singleton Initialization

    private static LocalizeActionPanel _instance;

    public static LocalizeActionPanel Instance { get { return _instance; } }

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

    #endregion

    void Start() {
        DefaultUI();

        leftPlayerString = leftPlayerText.GetComponent<LocalizeStringEvent>();
        rightPlayerString = rightPlayerText.GetComponent<LocalizeStringEvent>();
        oppositePlayerString = oppositePlayerText.GetComponent<LocalizeStringEvent>();
        localPlayerString = localPlayerText.GetComponent<LocalizeStringEvent>();

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

    public IEnumerator SetAction(Player player, string action) {
        string pos = RemotePlayer.RelativePlayerPosition(player);

        Text playerText = textDict[pos];
        LocalizeStringEvent stringEvent = stringDict[pos];

        playerText.transform.parent.gameObject.SetActive(true);
        stringEvent.StringReference.SetReference("Action", action);

        yield return new WaitForSeconds(3f);

        playerText.transform.parent.gameObject.SetActive(false);
    }

    private void DefaultUI() {
        actionPanel.SetActive(true);
        foreach (Transform child in actionPanel.transform) {
            child.gameObject.SetActive(false);
        }

    }
}
