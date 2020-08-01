using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using ExitGames.Client.Photon;
using System.Linq;
using UnityEngine.UI;
using System.Xml.Serialization;
using UnityEngineInternal;
using Photon.Realtime;

public class RoomSettingsPanel : MonoBehaviourPunCallbacks, IOnEventCallback {

    [SerializeField]
    private GameObject roomPanel;

    [SerializeField]
    private GameObject roomSettingsPanel;

    [SerializeField]
    private Transform content;

    private Dictionary<string, int> defaultSettings;

    private Dictionary<string, int> roomSettings;

    private List<string> maxValueSettings;

    private void Awake() {
        InitializeMaxValueSettings();
        InitializeDefaultSettings();
    }

    public override void OnEnable() {
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
        roomSettings = PropertiesManager.GetRoomSettings();
        int maxValue = roomSettings["Fan Limit"];

        foreach (Transform child in content) {            
            Slider slider = child.GetComponentInChildren<Slider>();
            if (PhotonNetwork.IsMasterClient) {
                slider.interactable = true;
            } else {
                slider.interactable = false;
            }

            string settingsName = child.name;
            child.GetComponent<RoomSetting>().settingsName = settingsName;

            if (child.name != "Fan Limit") {
                slider.maxValue = maxValue;
            }

            slider.value = roomSettings[settingsName];            
        }
    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged) {

        if (propertiesThatChanged.ContainsKey(PropertiesManager.ChangedRoomSettingPropKey)) {
            Dictionary<string, int> changedRoomSetting = PropertiesManager.GetChangedRoomSetting();
            string settingsName = changedRoomSetting.Keys.First();
            int value = changedRoomSetting.Values.First();

            if (settingsName == "Fan Limit") {
                foreach (Transform child in content) {
                    UpdateValues(child, value);
                }
            }

            if (PhotonNetwork.IsMasterClient) {
                roomSettings[settingsName] = value;
                PropertiesManager.UpdateRoomSettings(roomSettings);
                return;
            }

            Transform roomSetting = content.Find(settingsName);
            Slider slider = roomSetting.GetComponentInChildren<Slider>();
            slider.value = value;

            //Debug.LogErrorFormat("Changed {0} to {1}", settingsName, value);
        }
    }

    public void OnEvent(EventData photonEvent) {
        byte eventCode = photonEvent.Code;

        if (eventCode == EventsManager.EvResetRoomSettings) {
            OnEnable();
        }
    }

    public override void OnDisable() {
        base.OnDisable();
    }

    #region Buttons

    public void OnClickResetSettings() {
        PropertiesManager.SetRoomSettings(defaultSettings);
        EventsManager.EventResetRoomSettings();
    }

    public void OnClickBackToRoom() {
        roomSettingsPanel.SetActive(false);
        roomPanel.SetActive(true);
    }

    #endregion   

    #region Private Methods

    private void InitializeMaxValueSettings() {
        maxValueSettings = new List<string>() {
            "Heavenly Hand",
            "Earthly Hand",
            "Humanly Hand",
            "Robbing the Eighth",
            "All Flowers and Seasons",
            "Pure Terminals",
            "All Honour",
            "Hidden Treasure",
            "Full Flush Triplets",
            "Full Flush Full Sequence",
            "Nine Gates",
            "Four Great Blessings",
            "Three Great Scholars",
            "Eighteen Arhats",
            "Thirteen Wonders",
            "Kong on Kong"
        };
    }

    private void InitializeDefaultSettings() {
        defaultSettings = new Dictionary<string, int>() {
            ["Fan Limit"] = 5,

            ["Heavenly Hand"] = 10,
            ["Earthly Hand"] = 10,
            ["Humanly Hand"] = 10,

            ["Bonus Tile Match Seat Wind"] = 1,
            ["Animal"] = 1,
            ["Complete Animal Group"] = 5,
            ["Complete Season Group"] = 2,
            ["Complete Flower Group"] = 2,
            ["Robbing the Eighth"] = 10,
            ["All Flowers and Seasons"] = 10,

            ["Seat Wind Combo"] = 1,
            ["Prevailing Wind Combo"] = 1,
            ["Dragon"] = 1,

            ["Fully Concealed"] = 1,
            ["Triplets"] = 2,
            ["Half Flush"] = 2,
            ["Full Flush"] = 4,
            ["Lesser Sequence"] = 1,
            ["Full Sequence"] = 4,
            ["Mixed Terminals"] = 4,
            ["Pure Terminals"] = 10,
            ["All Honour"] = 10,
            ["Hidden Treasure"] = 10,
            ["Full Flush Triplets"] = 10,
            ["Full Flush Full Sequence"] = 10,
            ["Full Flush Lesser Sequence"] = 5,
            ["Nine Gates"] = 10,
            ["Four Lesser Blessings"] = 2,
            ["Four Great Blessings"] = 10,
            ["Pure Green Suit"] = 4,
            ["Three Lesser Scholars"] = 3,
            ["Three Great Scholars"] = 10,
            ["Eighteen Arhats"] = 10,
            ["Thirteen Wonders"] = 10,

            ["Replacement Tile for Flower"] = 1,
            ["Replacement Tile for Kong"] = 1,
            ["Kong on Kong"] = 10,

            ["Robbing the Kong"] = 1,
            ["Winning on Last Available Tile"] = 1,

            ["Dragon Tile Set Pay All"] = 1,
            ["Wind Tile Set Pay All"] = 1,
            ["Point Limit Pay All"] = 1,
            ["Full Flush Pay All"] = 1,
            ["Pure Terminals Pay All"] = 1,

            ["Min Point"] = 1,
            ["Shooter Pay"] = 1,

            ["Hidden Cat and Rat"] = 2,
            ["Cat and Rat"] = 1,
            ["Hidden Chicken and Centipede"] = 2,
            ["Chicken and Centipede"] = 1,
            ["Complete Animal Group Payout"] = 2,
            ["Hidden Bonus Tile Match Seat Wind Pair"] = 2,
            ["Bonus Tile Match Seat Wind Pair"] = 1,
            ["Complete Season Group Payout"] = 2,
            ["Complete Flower Group Payout"] = 2,
            ["Concealed Kong Payout"] = 2,
            ["Discard and Exposed Kong Payout"] = 1,
        };
    }

    private void UpdateValues(Transform child, int value) {
        if (child.name == "Fan Limit") {
            return;
        }

        Slider slider = child.GetComponentInChildren<Slider>();
        slider.maxValue = value;
        if (maxValueSettings.Contains(child.name)) {
            slider.value = value;
            roomSettings[child.name] = value;
        }
    }

    #endregion
}
