using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DictManager : MonoBehaviour {

    #region Tile Sprites

    [SerializeField]
    private Sprite Character_One_Sprite;

    [SerializeField]
    private Sprite Character_Two_Sprite;

    [SerializeField]
    private Sprite Character_Three_Sprite;

    [SerializeField]
    private Sprite Character_Four_Sprite;

    [SerializeField]
    private Sprite Character_Five_Sprite;

    [SerializeField]
    private Sprite Character_Six_Sprite;

    [SerializeField]
    private Sprite Character_Seven_Sprite;

    [SerializeField]
    private Sprite Character_Eight_Sprite;

    [SerializeField]
    private Sprite Character_Nine_Sprite;

    [SerializeField]
    private Sprite Dot_One_Sprite;

    [SerializeField]
    private Sprite Dot_Two_Sprite;

    [SerializeField]
    private Sprite Dot_Three_Sprite;

    [SerializeField]
    private Sprite Dot_Four_Sprite;

    [SerializeField]
    private Sprite Dot_Five_Sprite;

    [SerializeField]
    private Sprite Dot_Six_Sprite;

    [SerializeField]
    private Sprite Dot_Seven_Sprite;

    [SerializeField]
    private Sprite Dot_Eight_Sprite;

    [SerializeField]
    private Sprite Dot_Nine_Sprite;

    [SerializeField]
    private Sprite Bamboo_One_Sprite;

    [SerializeField]
    private Sprite Bamboo_Two_Sprite;

    [SerializeField]
    private Sprite Bamboo_Three_Sprite;

    [SerializeField]
    private Sprite Bamboo_Four_Sprite;

    [SerializeField]
    private Sprite Bamboo_Five_Sprite;

    [SerializeField]
    private Sprite Bamboo_Six_Sprite;

    [SerializeField]
    private Sprite Bamboo_Seven_Sprite;

    [SerializeField]
    private Sprite Bamboo_Eight_Sprite;

    [SerializeField]
    private Sprite Bamboo_Nine_Sprite;

    [SerializeField]
    private Sprite Wind_One_Sprite;

    [SerializeField]
    private Sprite Wind_Two_Sprite;

    [SerializeField]
    private Sprite Wind_Three_Sprite;

    [SerializeField]
    private Sprite Wind_Four_Sprite;

    [SerializeField]
    private Sprite Dragon_One_Sprite;

    [SerializeField]
    private Sprite Dragon_Two_Sprite;

    [SerializeField]
    private Sprite Dragon_Three_Sprite;

    [SerializeField]
    private Sprite Season_One_Sprite;

    [SerializeField]
    private Sprite Season_Two_Sprite;

    [SerializeField]
    private Sprite Season_Three_Sprite;

    [SerializeField]
    private Sprite Season_Four_Sprite;

    [SerializeField]
    private Sprite Flower_One_Sprite;

    [SerializeField]
    private Sprite Flower_Two_Sprite;

    [SerializeField]
    private Sprite Flower_Three_Sprite;

    [SerializeField]
    private Sprite Flower_Four_Sprite;

    [SerializeField]
    private Sprite Animal_One_Sprite;

    [SerializeField]
    private Sprite Animal_Two_Sprite;

    [SerializeField]
    private Sprite Animal_Three_Sprite;

    [SerializeField]
    private Sprite Animal_Four_Sprite;

    #endregion

    /// <summary>
    /// Dictionary containing Tile objects and their respective sprite
    /// </summary>
    public Dictionary<Tile, Sprite> spritesDict;

    /// <summary>
    /// Dictionary containing Tile objects and their respective winds
    /// </summary>
    public Dictionary<Tile, PlayerManager.Wind> tileToWindDict;

    public Dictionary<PlayerManager.Wind, Tile> windToTileDict;

    public Dictionary<PlayerManager.Wind, List<Tile>> windToBonusTilesDict;

    private static DictManager _instance;

    public static DictManager Instance { get { return _instance; } }

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

    private void Start() {
        this.InitializeSpritesDict();
        this.InitializeTileToWindDict();
        this.InitializeWindToTileDict();
        this.InitializeWindToBonusTilesDict();
    }

    /// <summary>
    /// Initialize the spritesDict with Tile objects and their respective sprites
    /// </summary>
    private void InitializeSpritesDict() {
        spritesDict = new Dictionary<Tile, Sprite>();

        spritesDict.Add(new Tile("Character_One"), Character_One_Sprite);
        spritesDict.Add(new Tile("Character_Two"), Character_Two_Sprite);
        spritesDict.Add(new Tile("Character_Three"), Character_Three_Sprite);
        spritesDict.Add(new Tile("Character_Four"), Character_Four_Sprite);
        spritesDict.Add(new Tile("Character_Five"), Character_Five_Sprite);
        spritesDict.Add(new Tile("Character_Six"), Character_Six_Sprite);
        spritesDict.Add(new Tile("Character_Seven"), Character_Seven_Sprite);
        spritesDict.Add(new Tile("Character_Eight"), Character_Eight_Sprite);
        spritesDict.Add(new Tile("Character_Nine"), Character_Nine_Sprite);

        spritesDict.Add(new Tile("Dot_One"), Dot_One_Sprite);
        spritesDict.Add(new Tile("Dot_Two"), Dot_Two_Sprite);
        spritesDict.Add(new Tile("Dot_Three"), Dot_Three_Sprite);
        spritesDict.Add(new Tile("Dot_Four"), Dot_Four_Sprite);
        spritesDict.Add(new Tile("Dot_Five"), Dot_Five_Sprite);
        spritesDict.Add(new Tile("Dot_Six"), Dot_Six_Sprite);
        spritesDict.Add(new Tile("Dot_Seven"), Dot_Seven_Sprite);
        spritesDict.Add(new Tile("Dot_Eight"), Dot_Eight_Sprite);
        spritesDict.Add(new Tile("Dot_Nine"), Dot_Nine_Sprite);

        spritesDict.Add(new Tile("Bamboo_One"), Bamboo_One_Sprite);
        spritesDict.Add(new Tile("Bamboo_Two"), Bamboo_Two_Sprite);
        spritesDict.Add(new Tile("Bamboo_Three"), Bamboo_Three_Sprite);
        spritesDict.Add(new Tile("Bamboo_Four"), Bamboo_Four_Sprite);
        spritesDict.Add(new Tile("Bamboo_Five"), Bamboo_Five_Sprite);
        spritesDict.Add(new Tile("Bamboo_Six"), Bamboo_Six_Sprite);
        spritesDict.Add(new Tile("Bamboo_Seven"), Bamboo_Seven_Sprite);
        spritesDict.Add(new Tile("Bamboo_Eight"), Bamboo_Eight_Sprite);
        spritesDict.Add(new Tile("Bamboo_Nine"), Bamboo_Nine_Sprite);

        spritesDict.Add(new Tile("Wind_One"), Wind_One_Sprite);
        spritesDict.Add(new Tile("Wind_Two"), Wind_Two_Sprite);
        spritesDict.Add(new Tile("Wind_Three"), Wind_Three_Sprite);
        spritesDict.Add(new Tile("Wind_Four"), Wind_Four_Sprite);

        spritesDict.Add(new Tile("Dragon_One"), Dragon_One_Sprite);
        spritesDict.Add(new Tile("Dragon_Two"), Dragon_Two_Sprite);
        spritesDict.Add(new Tile("Dragon_Three"), Dragon_Three_Sprite);

        spritesDict.Add(new Tile("Season_One"), Season_One_Sprite);
        spritesDict.Add(new Tile("Season_Two"), Season_Two_Sprite);
        spritesDict.Add(new Tile("Season_Three"), Season_Three_Sprite);
        spritesDict.Add(new Tile("Season_Four"), Season_Four_Sprite);

        spritesDict.Add(new Tile("Flower_One"), Flower_One_Sprite);
        spritesDict.Add(new Tile("Flower_Two"), Flower_Two_Sprite);
        spritesDict.Add(new Tile("Flower_Three"), Flower_Three_Sprite);
        spritesDict.Add(new Tile("Flower_Four"), Flower_Four_Sprite);

        spritesDict.Add(new Tile("Animal_One"), Animal_One_Sprite);
        spritesDict.Add(new Tile("Animal_Two"), Animal_Two_Sprite);
        spritesDict.Add(new Tile("Animal_Three"), Animal_Three_Sprite);
        spritesDict.Add(new Tile("Animal_Four"), Animal_Four_Sprite);
    }


    /// <summary>
    /// Initialize the tileToWindDict with Tile objects and their respective Winds
    /// </summary>
    private void InitializeTileToWindDict() {
        tileToWindDict = new Dictionary<Tile, PlayerManager.Wind>();

        tileToWindDict.Add(new Tile(Tile.Suit.Wind, Tile.Rank.One), PlayerManager.Wind.EAST);
        tileToWindDict.Add(new Tile(Tile.Suit.Wind, Tile.Rank.Two), PlayerManager.Wind.SOUTH);
        tileToWindDict.Add(new Tile(Tile.Suit.Wind, Tile.Rank.Three), PlayerManager.Wind.WEST);
        tileToWindDict.Add(new Tile(Tile.Suit.Wind, Tile.Rank.Four), PlayerManager.Wind.NORTH);

        tileToWindDict.Add(new Tile(Tile.Suit.Season, Tile.Rank.One), PlayerManager.Wind.EAST);
        tileToWindDict.Add(new Tile(Tile.Suit.Season, Tile.Rank.Two), PlayerManager.Wind.SOUTH);
        tileToWindDict.Add(new Tile(Tile.Suit.Season, Tile.Rank.Three), PlayerManager.Wind.WEST);
        tileToWindDict.Add(new Tile(Tile.Suit.Season, Tile.Rank.Four), PlayerManager.Wind.NORTH);

        tileToWindDict.Add(new Tile(Tile.Suit.Flower, Tile.Rank.One), PlayerManager.Wind.EAST);
        tileToWindDict.Add(new Tile(Tile.Suit.Flower, Tile.Rank.Two), PlayerManager.Wind.SOUTH);
        tileToWindDict.Add(new Tile(Tile.Suit.Flower, Tile.Rank.Three), PlayerManager.Wind.WEST);
        tileToWindDict.Add(new Tile(Tile.Suit.Flower, Tile.Rank.Four), PlayerManager.Wind.NORTH);
    }


    /// <summary>
    /// Initialize the windToTileDict with Winds and their respective Wind Tiles
    /// </summary>
    private void InitializeWindToTileDict() {
        windToTileDict = new Dictionary<PlayerManager.Wind, Tile>();

        windToTileDict.Add(PlayerManager.Wind.EAST, new Tile(Tile.Suit.Wind, Tile.Rank.One));
        windToTileDict.Add(PlayerManager.Wind.SOUTH, new Tile(Tile.Suit.Wind, Tile.Rank.Two));
        windToTileDict.Add(PlayerManager.Wind.WEST, new Tile(Tile.Suit.Wind, Tile.Rank.Three));
        windToTileDict.Add(PlayerManager.Wind.NORTH, new Tile(Tile.Suit.Wind, Tile.Rank.Four));
    }


    /// <summary>
    /// Initialize the windToBonusTilesDict with Winds and the 2 bonus tiles paired to each Wind
    /// </summary>
    private void InitializeWindToBonusTilesDict() {
        windToBonusTilesDict = new Dictionary<PlayerManager.Wind, List<Tile>>();

        windToBonusTilesDict.Add(PlayerManager.Wind.EAST, new List<Tile>() { new Tile(Tile.Suit.Season, Tile.Rank.One), new Tile(Tile.Suit.Flower, Tile.Rank.One) });
        windToBonusTilesDict.Add(PlayerManager.Wind.SOUTH, new List<Tile>() { new Tile(Tile.Suit.Season, Tile.Rank.Two), new Tile(Tile.Suit.Flower, Tile.Rank.Two) });
        windToBonusTilesDict.Add(PlayerManager.Wind.WEST, new List<Tile>() { new Tile(Tile.Suit.Season, Tile.Rank.Three), new Tile(Tile.Suit.Flower, Tile.Rank.Three) });
        windToBonusTilesDict.Add(PlayerManager.Wind.NORTH, new List<Tile>() { new Tile(Tile.Suit.Season, Tile.Rank.Four), new Tile(Tile.Suit.Flower, Tile.Rank.Four) });
    }


    public Dictionary<Tile, int> HonourTilesCountDict() {
        Dictionary<Tile, int> honourTilesCount = new Dictionary<Tile, int>();

        honourTilesCount.Add(new Tile(Tile.Suit.Wind, Tile.Rank.One), 0);
        honourTilesCount.Add(new Tile(Tile.Suit.Wind, Tile.Rank.Two), 0);
        honourTilesCount.Add(new Tile(Tile.Suit.Wind, Tile.Rank.Three), 0);
        honourTilesCount.Add(new Tile(Tile.Suit.Wind, Tile.Rank.Four), 0);

        honourTilesCount.Add(new Tile(Tile.Suit.Dragon, Tile.Rank.One), 0);
        honourTilesCount.Add(new Tile(Tile.Suit.Dragon, Tile.Rank.Two), 0);
        honourTilesCount.Add(new Tile(Tile.Suit.Dragon, Tile.Rank.Three), 0);

        return honourTilesCount;
    }


    public Dictionary<Tile, int> NineGatesDict() {
        Dictionary<Tile, int> nineGatesDict = new Dictionary<Tile, int>();

        nineGatesDict.Add(new Tile(Tile.Suit.Character, Tile.Rank.One), 3);
        nineGatesDict.Add(new Tile(Tile.Suit.Character, Tile.Rank.Two), 1);
        nineGatesDict.Add(new Tile(Tile.Suit.Character, Tile.Rank.Three), 1);
        nineGatesDict.Add(new Tile(Tile.Suit.Character, Tile.Rank.Four), 1);
        nineGatesDict.Add(new Tile(Tile.Suit.Character, Tile.Rank.Five), 1);
        nineGatesDict.Add(new Tile(Tile.Suit.Character, Tile.Rank.Six), 1);
        nineGatesDict.Add(new Tile(Tile.Suit.Character, Tile.Rank.Seven), 1);
        nineGatesDict.Add(new Tile(Tile.Suit.Character, Tile.Rank.Eight), 1);
        nineGatesDict.Add(new Tile(Tile.Suit.Character, Tile.Rank.Nine), 3);

        nineGatesDict.Add(new Tile(Tile.Suit.Dot, Tile.Rank.One), 3);
        nineGatesDict.Add(new Tile(Tile.Suit.Dot, Tile.Rank.Two), 1);
        nineGatesDict.Add(new Tile(Tile.Suit.Dot, Tile.Rank.Three), 1);
        nineGatesDict.Add(new Tile(Tile.Suit.Dot, Tile.Rank.Four), 1);
        nineGatesDict.Add(new Tile(Tile.Suit.Dot, Tile.Rank.Five), 1);
        nineGatesDict.Add(new Tile(Tile.Suit.Dot, Tile.Rank.Six), 1);
        nineGatesDict.Add(new Tile(Tile.Suit.Dot, Tile.Rank.Seven), 1);
        nineGatesDict.Add(new Tile(Tile.Suit.Dot, Tile.Rank.Eight), 1);
        nineGatesDict.Add(new Tile(Tile.Suit.Dot, Tile.Rank.Nine), 3);

        nineGatesDict.Add(new Tile(Tile.Suit.Bamboo, Tile.Rank.One), 3);
        nineGatesDict.Add(new Tile(Tile.Suit.Bamboo, Tile.Rank.Two), 1);
        nineGatesDict.Add(new Tile(Tile.Suit.Bamboo, Tile.Rank.Three), 1);
        nineGatesDict.Add(new Tile(Tile.Suit.Bamboo, Tile.Rank.Four), 1);
        nineGatesDict.Add(new Tile(Tile.Suit.Bamboo, Tile.Rank.Five), 1);
        nineGatesDict.Add(new Tile(Tile.Suit.Bamboo, Tile.Rank.Six), 1);
        nineGatesDict.Add(new Tile(Tile.Suit.Bamboo, Tile.Rank.Seven), 1);
        nineGatesDict.Add(new Tile(Tile.Suit.Bamboo, Tile.Rank.Eight), 1);
        nineGatesDict.Add(new Tile(Tile.Suit.Bamboo, Tile.Rank.Nine), 3);

        return nineGatesDict;
    }


    public Dictionary<Tile, int> ThirteenWondersDict() {
        Dictionary<Tile, int> thirteenWondersDict = new Dictionary<Tile, int>() {
            [new Tile(Tile.Suit.Character, Tile.Rank.One)] = 0,
            [new Tile(Tile.Suit.Character, Tile.Rank.Nine)] = 0,
            [new Tile(Tile.Suit.Dot, Tile.Rank.One)] = 0,
            [new Tile(Tile.Suit.Dot, Tile.Rank.Nine)] = 0,
            [new Tile(Tile.Suit.Bamboo, Tile.Rank.One)] = 0,
            [new Tile(Tile.Suit.Bamboo, Tile.Rank.Nine)] = 0,
            [new Tile(Tile.Suit.Wind, Tile.Rank.One)] = 0,
            [new Tile(Tile.Suit.Wind, Tile.Rank.Two)] = 0,
            [new Tile(Tile.Suit.Wind, Tile.Rank.Three)] = 0,
            [new Tile(Tile.Suit.Wind, Tile.Rank.Four)] = 0,
            [new Tile(Tile.Suit.Dragon, Tile.Rank.One)] = 0,
            [new Tile(Tile.Suit.Dragon, Tile.Rank.Two)] = 0,
            [new Tile(Tile.Suit.Dragon, Tile.Rank.Three)] = 0,
        };
        return thirteenWondersDict;
    }
}
