using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour {

    public Dictionary<string, int> settingsDict;

    public static Dictionary<string, int> settings;

    private void Awake() {
        settingsDict = new Dictionary<string, int>();
        this.InitialSettings();
    }

    private void InitialSettings() {
        settingsDict.Add("Fan Limit", 5);

        settingsDict.Add("Heavenly Hand", 10);
        settingsDict.Add("Earthly Hand", 10);
        settingsDict.Add("Humanly Hand", 10);

        settingsDict.Add("Bonus Tile Match Seat Wind", 1);
        settingsDict.Add("Animal", 1);
        settingsDict.Add("Complete Animal Group", 5);
        settingsDict.Add("Complete Season Group", 2);
        settingsDict.Add("Complete Flower Group", 2);
        settingsDict.Add("Robbing the Eighth", 10);
        settingsDict.Add("All Flowers and Seasons", 10);

        settingsDict.Add("Seat Wind Combo", 1);
        settingsDict.Add("Prevailing Wind Combo", 1);
        settingsDict.Add("Dragon", 1);

        settingsDict.Add("Fully Concealed", 1);
        settingsDict.Add("Triplets", 2);
        settingsDict.Add("Half Flush", 2);
        settingsDict.Add("Full Flush", 4);
        settingsDict.Add("Lesser Sequence", 1);
        settingsDict.Add("Full Sequence", 4);
        settingsDict.Add("Mixed Terminals", 4);
        settingsDict.Add("Pure Terminals", 10);
        settingsDict.Add("All Honour", 10);
        settingsDict.Add("Hidden Treasure", 10);
        settingsDict.Add("Full Flush Triplets", 10);
        settingsDict.Add("Full Flush Full Sequence", 10);
        settingsDict.Add("Full Flush Lesser Sequence", 5);
        settingsDict.Add("Nine Gates", 10);
        settingsDict.Add("Four Lesser Blessings", 2);
        settingsDict.Add("Four Great Blessings", 10);
        settingsDict.Add("Pure Green Suit", 4);
        settingsDict.Add("Three Lesser Scholars", 3);
        settingsDict.Add("Three Great Scholars", 10);
        settingsDict.Add("Eighteen Arhats", 10);
        settingsDict.Add("Thirteen Wonders", 10);

        settingsDict.Add("Replacement Tile for Flower", 1);
        settingsDict.Add("Replacement Tile for Kong", 1);
        settingsDict.Add("Kong on Kong", 10);

        settingsDict.Add("Robbing the Kong", 1);
        settingsDict.Add("Winning on Last Available Tile", 1);

        settingsDict.Add("Dragon Tile Set Pay All", 1);
        settingsDict.Add("Wind Tile Set Pay All", 1);
        settingsDict.Add("Point Limit Pay All", 1);
        settingsDict.Add("Full Flush Pay All", 1);
        settingsDict.Add("Pure Terminals Pay All", 1);

        settingsDict.Add("Min Point", 1);
        settingsDict.Add("Shooter Pay", 1);

        settingsDict.Add("Hidden Cat and Rat", 2);
        settingsDict.Add("Cat and Rat", 1);
        settingsDict.Add("Hidden Chicken and Centipede", 2);
        settingsDict.Add("Chicken and Centipede", 1);
        settingsDict.Add("Complete Animal Group Payout", 2);
        settingsDict.Add("Hidden Bonus Tile Match Seat Wind Pair", 2);
        settingsDict.Add("Bonus Tile Match Seat Wind Pair", 1);
        settingsDict.Add("Complete Season Group Payout", 2);
        settingsDict.Add("Complete Flower Group Payout", 2);
        settingsDict.Add("Concealed Kong Payout", 2);
        settingsDict.Add("Discard and Exposed Kong Payout", 1);
    }

    
}
