﻿using System.Collections;
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

    #region Tile 3D Prefabs
    [SerializeField]
    private GameObject Character_One;

    [SerializeField]
    private GameObject Character_Two;

    [SerializeField]
    private GameObject Character_Three;

    [SerializeField]
    private GameObject Character_Four;

    [SerializeField]
    private GameObject Character_Five;

    [SerializeField]
    private GameObject Character_Six;

    [SerializeField]
    private GameObject Character_Seven;

    [SerializeField]
    private GameObject Character_Eight;

    [SerializeField]
    private GameObject Character_Nine;

    [SerializeField]
    private GameObject Dot_One;

    [SerializeField]
    private GameObject Dot_Two;

    [SerializeField]
    private GameObject Dot_Three;

    [SerializeField]
    private GameObject Dot_Four;

    [SerializeField]
    private GameObject Dot_Five;

    [SerializeField]
    private GameObject Dot_Six;

    [SerializeField]
    private GameObject Dot_Seven;

    [SerializeField]
    private GameObject Dot_Eight;

    [SerializeField]
    private GameObject Dot_Nine;

    [SerializeField]
    private GameObject Bamboo_One;

    [SerializeField]
    private GameObject Bamboo_Two;

    [SerializeField]
    private GameObject Bamboo_Three;

    [SerializeField]
    private GameObject Bamboo_Four;

    [SerializeField]
    private GameObject Bamboo_Five;

    [SerializeField]
    private GameObject Bamboo_Six;

    [SerializeField]
    private GameObject Bamboo_Seven;

    [SerializeField]
    private GameObject Bamboo_Eight;

    [SerializeField]
    private GameObject Bamboo_Nine;

    [SerializeField]
    private GameObject Wind_One;

    [SerializeField]
    private GameObject Wind_Two;

    [SerializeField]
    private GameObject Wind_Three;

    [SerializeField]
    private GameObject Wind_Four;

    [SerializeField]
    private GameObject Dragon_One;

    [SerializeField]
    private GameObject Dragon_Two;

    [SerializeField]
    private GameObject Dragon_Three;

    [SerializeField]
    private GameObject Season_One;

    [SerializeField]
    private GameObject Season_Two;

    [SerializeField]
    private GameObject Season_Three;

    [SerializeField]
    private GameObject Season_Four;

    [SerializeField]
    private GameObject Flower_One;

    [SerializeField]
    private GameObject Flower_Two;

    [SerializeField]
    private GameObject Flower_Three;

    [SerializeField]
    private GameObject Flower_Four;

    [SerializeField]
    private GameObject Animal_One;

    [SerializeField]
    private GameObject Animal_Two;

    [SerializeField]
    private GameObject Animal_Three;

    [SerializeField]
    private GameObject Animal_Four;

    #endregion

    #region Dictionaries

    /// <summary>
    /// Dictionary containing Tile objects and their respective sprite
    /// </summary>
    public Dictionary<Tile, Sprite> spritesDict;

    /// <summary>
    /// Dictionary containing Tile objects and their respective prefab
    /// </summary>
    public Dictionary<Tile, GameObject> tilesDict;

    /// <summary>
    /// Dictionary containing Tile objects and their respective winds
    /// </summary>
    public Dictionary<Tile, PlayerManager.Wind> tileToWindDict;

    public Dictionary<PlayerManager.Wind, Tile> windToTileDict;

    public Dictionary<PlayerManager.Wind, List<Tile>> windTo3TilesDict;

    public Dictionary<Tile.Suit?, List<Tile>> fullFlushDict;

    /// <summary>
    /// A cached copy of allocated Winds.
    /// </summary>
    public Dictionary<int, int> windsAllocation;

    #endregion

    #region Singleton Initialization

    private static DictManager _instance;

    public static DictManager Instance { get { return _instance; } }

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

    #endregion

    private void Start() {
        this.InitializeSpritesDict();
        this.InitializeTilesDict();
        this.InitializeWindTo3TilesDict();
        this.InitializeTileToWindDict();
        this.InitializeWindToTileDict();
        this.InitializeFullFlushDict();
    }


    /// <summary>
    /// For Unit Testing
    /// </summary>
    public DictManager() {
        _instance = this;

        this.InitializeSpritesDict();
        this.InitializeTilesDict();
        this.InitializeWindTo3TilesDict();
        this.InitializeTileToWindDict();
        this.InitializeWindToTileDict();
        this.InitializeFullFlushDict();
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
    /// Fill up the tilesDict with Tile objects and their respective prefabs
    /// </summary>
    private void InitializeTilesDict() {
        tilesDict = new Dictionary<Tile, GameObject>();

        tilesDict.Add(new Tile("Character_One"), Character_One);
        tilesDict.Add(new Tile("Character_Two"), Character_Two);
        tilesDict.Add(new Tile("Character_Three"), Character_Three);
        tilesDict.Add(new Tile("Character_Four"), Character_Four);
        tilesDict.Add(new Tile("Character_Five"), Character_Five);
        tilesDict.Add(new Tile("Character_Six"), Character_Six);
        tilesDict.Add(new Tile("Character_Seven"), Character_Seven);
        tilesDict.Add(new Tile("Character_Eight"), Character_Eight);
        tilesDict.Add(new Tile("Character_Nine"), Character_Nine);

        tilesDict.Add(new Tile("Dot_One"), Dot_One);
        tilesDict.Add(new Tile("Dot_Two"), Dot_Two);
        tilesDict.Add(new Tile("Dot_Three"), Dot_Three);
        tilesDict.Add(new Tile("Dot_Four"), Dot_Four);
        tilesDict.Add(new Tile("Dot_Five"), Dot_Five);
        tilesDict.Add(new Tile("Dot_Six"), Dot_Six);
        tilesDict.Add(new Tile("Dot_Seven"), Dot_Seven);
        tilesDict.Add(new Tile("Dot_Eight"), Dot_Eight);
        tilesDict.Add(new Tile("Dot_Nine"), Dot_Nine);

        tilesDict.Add(new Tile("Bamboo_One"), Bamboo_One);
        tilesDict.Add(new Tile("Bamboo_Two"), Bamboo_Two);
        tilesDict.Add(new Tile("Bamboo_Three"), Bamboo_Three);
        tilesDict.Add(new Tile("Bamboo_Four"), Bamboo_Four);
        tilesDict.Add(new Tile("Bamboo_Five"), Bamboo_Five);
        tilesDict.Add(new Tile("Bamboo_Six"), Bamboo_Six);
        tilesDict.Add(new Tile("Bamboo_Seven"), Bamboo_Seven);
        tilesDict.Add(new Tile("Bamboo_Eight"), Bamboo_Eight);
        tilesDict.Add(new Tile("Bamboo_Nine"), Bamboo_Nine);

        tilesDict.Add(new Tile("Wind_One"), Wind_One);
        tilesDict.Add(new Tile("Wind_Two"), Wind_Two);
        tilesDict.Add(new Tile("Wind_Three"), Wind_Three);
        tilesDict.Add(new Tile("Wind_Four"), Wind_Four);

        tilesDict.Add(new Tile("Dragon_One"), Dragon_One);
        tilesDict.Add(new Tile("Dragon_Two"), Dragon_Two);
        tilesDict.Add(new Tile("Dragon_Three"), Dragon_Three);

        tilesDict.Add(new Tile("Season_One"), Season_One);
        tilesDict.Add(new Tile("Season_Two"), Season_Two);
        tilesDict.Add(new Tile("Season_Three"), Season_Three);
        tilesDict.Add(new Tile("Season_Four"), Season_Four);

        tilesDict.Add(new Tile("Flower_One"), Flower_One);
        tilesDict.Add(new Tile("Flower_Two"), Flower_Two);
        tilesDict.Add(new Tile("Flower_Three"), Flower_Three);
        tilesDict.Add(new Tile("Flower_Four"), Flower_Four);

        tilesDict.Add(new Tile("Animal_One"), Animal_One);
        tilesDict.Add(new Tile("Animal_Two"), Animal_Two);
        tilesDict.Add(new Tile("Animal_Three"), Animal_Three);
        tilesDict.Add(new Tile("Animal_Four"), Animal_Four);
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
    /// Initialize the windTo3TilesDict with Winds and the Wind, Season and Flower tile paired to each wind.
    /// </summary>
    private void InitializeWindTo3TilesDict() {
        this.windTo3TilesDict = new Dictionary<PlayerManager.Wind, List<Tile>>();

        this.windTo3TilesDict.Add(PlayerManager.Wind.EAST, new List<Tile>() {
            new Tile(Tile.Suit.Season, Tile.Rank.One),
            new Tile(Tile.Suit.Flower, Tile.Rank.One),
            new Tile(Tile.Suit.Wind, Tile.Rank.One) });
        this.windTo3TilesDict.Add(PlayerManager.Wind.SOUTH, new List<Tile>() {
            new Tile(Tile.Suit.Season, Tile.Rank.Two),
            new Tile(Tile.Suit.Flower, Tile.Rank.Two),
            new Tile(Tile.Suit.Wind, Tile.Rank.Two)});
        this.windTo3TilesDict.Add(PlayerManager.Wind.WEST, new List<Tile>() {
            new Tile(Tile.Suit.Season, Tile.Rank.Three),
            new Tile(Tile.Suit.Flower, Tile.Rank.Three),
            new Tile(Tile.Suit.Wind, Tile.Rank.Three)});
        this.windTo3TilesDict.Add(PlayerManager.Wind.NORTH, new List<Tile>() {
            new Tile(Tile.Suit.Season, Tile.Rank.Four),
            new Tile(Tile.Suit.Flower, Tile.Rank.Four),
            new Tile(Tile.Suit.Wind, Tile.Rank.Four)});
    }


    /// <summary>
    /// Initializes FullFlushDict
    /// </summary>
    private void InitializeFullFlushDict() {
        this.fullFlushDict = new Dictionary<Tile.Suit?, List<Tile>>();

        fullFlushDict.Add(Tile.Suit.Character, new List<Tile>() {
            new Tile(Tile.Suit.Character, Tile.Rank.One),
            new Tile(Tile.Suit.Character, Tile.Rank.Two),
            new Tile(Tile.Suit.Character, Tile.Rank.Three),
            new Tile(Tile.Suit.Character, Tile.Rank.Four),
            new Tile(Tile.Suit.Character, Tile.Rank.Five),
            new Tile(Tile.Suit.Character, Tile.Rank.Six),
            new Tile(Tile.Suit.Character, Tile.Rank.Seven),
            new Tile(Tile.Suit.Character, Tile.Rank.Eight),
            new Tile(Tile.Suit.Character, Tile.Rank.Nine)
        });

        fullFlushDict.Add(Tile.Suit.Dot, new List<Tile>() {
            new Tile(Tile.Suit.Dot, Tile.Rank.One),
            new Tile(Tile.Suit.Dot, Tile.Rank.Two),
            new Tile(Tile.Suit.Dot, Tile.Rank.Three),
            new Tile(Tile.Suit.Dot, Tile.Rank.Four),
            new Tile(Tile.Suit.Dot, Tile.Rank.Five),
            new Tile(Tile.Suit.Dot, Tile.Rank.Six),
            new Tile(Tile.Suit.Dot, Tile.Rank.Seven),
            new Tile(Tile.Suit.Dot, Tile.Rank.Eight),
            new Tile(Tile.Suit.Dot, Tile.Rank.Nine)
        });

        fullFlushDict.Add(Tile.Suit.Bamboo, new List<Tile>() {
            new Tile(Tile.Suit.Bamboo, Tile.Rank.One),
            new Tile(Tile.Suit.Bamboo, Tile.Rank.Two),
            new Tile(Tile.Suit.Bamboo, Tile.Rank.Three),
            new Tile(Tile.Suit.Bamboo, Tile.Rank.Four),
            new Tile(Tile.Suit.Bamboo, Tile.Rank.Five),
            new Tile(Tile.Suit.Bamboo, Tile.Rank.Six),
            new Tile(Tile.Suit.Bamboo, Tile.Rank.Seven),
            new Tile(Tile.Suit.Bamboo, Tile.Rank.Eight),
            new Tile(Tile.Suit.Bamboo, Tile.Rank.Nine)
        });

        fullFlushDict.Add(Tile.Suit.Wind, new List<Tile>() {
            new Tile(Tile.Suit.Wind, Tile.Rank.One),
            new Tile(Tile.Suit.Wind, Tile.Rank.Two),
            new Tile(Tile.Suit.Wind, Tile.Rank.Three),
            new Tile(Tile.Suit.Wind, Tile.Rank.Four),
            new Tile(Tile.Suit.Dragon, Tile.Rank.One),
            new Tile(Tile.Suit.Dragon, Tile.Rank.Two),
            new Tile(Tile.Suit.Dragon, Tile.Rank.Three),
        });

        fullFlushDict.Add(Tile.Suit.Dragon, new List<Tile>() {
            new Tile(Tile.Suit.Wind, Tile.Rank.One),
            new Tile(Tile.Suit.Wind, Tile.Rank.Two),
            new Tile(Tile.Suit.Wind, Tile.Rank.Three),
            new Tile(Tile.Suit.Wind, Tile.Rank.Four),
            new Tile(Tile.Suit.Dragon, Tile.Rank.One),
            new Tile(Tile.Suit.Dragon, Tile.Rank.Two),
            new Tile(Tile.Suit.Dragon, Tile.Rank.Three),
        });
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
