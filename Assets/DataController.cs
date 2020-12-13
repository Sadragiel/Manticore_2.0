using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Assets.DataStructures;
using Assets.GameStrategy;
using Assets.Artifacts;
using UnityEngine.UI;

public class DataController : MonoBehaviour
{
    private static DataController _instance;
    public static DataController Instance
    {
        get
        {
            if (_instance == null)
                Initialize();
            return _instance;
        }
    }

    public Button skipTurnButton;
    public ArtifactManagement artifactManager;

    public HexUnit characterPrefab;
    public HexUnit monsterPrefab;
    public HexUnit manticorePrefab;
    public RoadCell roadcellPrefab;

    public static int NUMBER_OF_DIRECTIONS = 6;

    public string emptyLocationName = "EMPTY";
    public string castleName = "CASTLE";
    string necklaceName = "NECKLACE";
    string swordName = "SWORD";
    string bowName = "BOW";
    string shieldName = "SHIELD";
    string halmetName = "HALMET";
    string bootsName = "BOOTS";
    string fireballName = "FIREBALL";
    string frostboltName = "FROSTBOLT";
    string manticoreName = "MANTICORE";

    string monster_agro_road = "road_birdpaw";
    string monster_lvl3_road = "road_crossroad";
    string monster_lvl4_road = "road_starroad";

    public Location[] locations;
    public Cell[] cellsData;
    public CharacterMeta[] charactersData;

    public Material[] roadCellMaterials;
    public Material[] charactersMaterials;
    public Material[] artifactMaterials;

    public Sprite[] artifactSprites;
    public Sprite[] charecterSprites;

    ArrayList activeLocations;

    public Sprite[] WeaponTypes;

    public bool ManticoreMovementAvailable;

    private static void Initialize()
    {
        _instance = GameObject.FindObjectOfType<DataController>();

        // Initial Artifact Stoks
        List<List<Artifact>> artifactStocks = _instance.createArtifactStocks();


        // Locations have constant coordinates on the map
        _instance.locations = new Location[10] {
            new Location(_instance.castleName, new int[4]{ 75, 89, 90, 103 }, Color.green, new List<Artifact>(), 103),
            new Location(_instance.necklaceName, new int[3]{ 168, 169, 154 }, Color.blue, artifactStocks[0], 168),
            new Location(_instance.swordName, new int[4]{ 174, 175, 159, 160 }, Color.blue, artifactStocks[1], 174),
            new Location(_instance.bowName, new int[4]{ 6, 7, 20, 21 }, Color.blue, artifactStocks[2], 20),
            new Location(_instance.shieldName, new int[4]{ 82, 96, 97, 110 }, Color.blue, artifactStocks[3], 110),
            new Location(_instance.halmetName, new int[3]{ 12, 13, 26 }, Color.blue, artifactStocks[4], 26),
            new Location(_instance.bootsName, new int[4]{ 70, 84, 85, 98 }, Color.blue, artifactStocks[5], 98),
            new Location(_instance.fireballName, new int[3]{ 136, 150, 151 }, Color.blue, artifactStocks[6], 150),
            new Location(_instance.frostboltName, new int[3]{ 0, 1, 14 }, Color.blue, artifactStocks[7], 14),
            new Location(_instance.manticoreName, new int[1]{ 181 }, Color.blue, new List<Artifact>(), 181),
        };

        _instance.cellsData = new Cell[7] {
            new Cell(_instance.roadCellMaterials[0], _instance.monster_agro_road, 1, new bool[6] { false, true, true, true, false, true }),
            new Cell(_instance.roadCellMaterials[1], _instance.monster_lvl3_road, 1, new bool[6] { true, false, true, true, false, true }),
            new Cell(_instance.roadCellMaterials[2], "road_fork", 1, new bool[6] { false, true, true, false, false, true }),
            new Cell(_instance.roadCellMaterials[3], _instance.monster_lvl4_road, 1, new bool[6] { true, true, true, true, true, true }),
            new Cell(_instance.roadCellMaterials[4], "road_turnroad", 1, new bool[6] { false, true, false, true, false, false }),
            new Cell(_instance.roadCellMaterials[5], "road_y_type", 1, new bool[6] { false, true, false, true, false, true }),
            new Cell(_instance.roadCellMaterials[6], "road_straightroad", 1, new bool[6] { false, false, true, false, false, true }),
        };

        _instance.activeLocations = new ArrayList();
        _instance.activeLocations.Add(_instance.castleName);

        _instance.charactersData = new CharacterMeta[8]
        {
            new CharacterMeta(_instance.charactersMaterials[0], _instance.charecterSprites[0], "Archer", _instance.castleName, 1, 100, 1, 3, 1, 1, 3, false, _instance.characterPrefab),
            new CharacterMeta(_instance.charactersMaterials[1], _instance.charecterSprites[1], "Mage", _instance.castleName, 10, 2, 1, 4, 3, 2, 4, false, _instance.characterPrefab),
            new CharacterMeta(_instance.charactersMaterials[2], _instance.charecterSprites[2], "Tank", _instance.castleName, 10, 3, 2, 3, 1, 3, 3, false, _instance.characterPrefab),
            new CharacterMeta(_instance.charactersMaterials[3], _instance.charecterSprites[3], "Knight", _instance.castleName, 10, 4, 3, 2, 1, 2, 2, false, _instance.characterPrefab),
            new CharacterMeta(_instance.charactersMaterials[4], _instance.charecterSprites[4], "MonsterLvl4", _instance.monster_lvl4_road, 4, 1, 4, 4, 4, 4, 4, true, _instance.monsterPrefab),
            new CharacterMeta(_instance.charactersMaterials[5], _instance.charecterSprites[5], "MonsterLvl3", _instance.monster_lvl3_road, 3, 1, 3, 3, 3, 3, 3, true, _instance.monsterPrefab),
            new CharacterMeta(_instance.charactersMaterials[6], _instance.charecterSprites[6], "MonsterAgro", _instance.monster_agro_road, 3, 1, 3, 3, 3, 3, 3, true, _instance.monsterPrefab),
            new CharacterMeta(_instance.charactersMaterials[7], _instance.charecterSprites[7], "Manticore", _instance.manticoreName, 3, 1, 3, 3, 3, 3, 3, true, _instance.manticorePrefab),
        };
    }

    private List<List<Artifact>> createArtifactStocks()
    {
        List<List<Artifact>> list = new List<List<Artifact>>();

        // NECKLACE
        list.Add(new List<Artifact>());
        list[0].Add(new Artifact(_instance.necklaceName, WeaponType.MAGIC, 2, false, _instance.artifactMaterials[0], _instance.artifactSprites[0]));
        list[0].Add(new Artifact(_instance.necklaceName, WeaponType.MAGIC, 1, false, _instance.artifactMaterials[0], _instance.artifactSprites[0]));
        list[0].Add(new Artifact(_instance.necklaceName, WeaponType.MAGIC, 1, false, _instance.artifactMaterials[0], _instance.artifactSprites[0]));

        // SWORD
        list.Add(new List<Artifact>());
        list[1].Add(new Artifact(_instance.swordName, WeaponType.SWORD, 3, false, _instance.artifactMaterials[1], _instance.artifactSprites[1]));
        list[1].Add(new Artifact(_instance.swordName, WeaponType.SWORD, 2, false, _instance.artifactMaterials[1], _instance.artifactSprites[1]));
        list[1].Add(new Artifact(_instance.swordName, WeaponType.SWORD, 1, false, _instance.artifactMaterials[1], _instance.artifactSprites[1]));
        list[1].Add(new Artifact(_instance.swordName, WeaponType.SWORD, 1, false, _instance.artifactMaterials[1], _instance.artifactSprites[1]));

        // BOW
        list.Add(new List<Artifact>());
        list[2].Add(new Artifact(_instance.bowName, WeaponType.BOW, 3, false, _instance.artifactMaterials[2], _instance.artifactSprites[2]));
        list[2].Add(new Artifact(_instance.bowName, WeaponType.BOW, 2, false, _instance.artifactMaterials[2], _instance.artifactSprites[2]));
        list[2].Add(new Artifact(_instance.bowName, WeaponType.BOW, 1, false, _instance.artifactMaterials[2], _instance.artifactSprites[2]));
        list[2].Add(new Artifact(_instance.bowName, WeaponType.BOW, 1, false, _instance.artifactMaterials[2], _instance.artifactSprites[2]));

        // SHIELD
        list.Add(new List<Artifact>());
        list[3].Add(new Artifact(_instance.shieldName, WeaponType.SHIELD, 3, false, _instance.artifactMaterials[3], _instance.artifactSprites[3]));
        list[3].Add(new Artifact(_instance.shieldName, WeaponType.SHIELD, 2, false, _instance.artifactMaterials[3], _instance.artifactSprites[3]));
        list[3].Add(new Artifact(_instance.shieldName, WeaponType.SHIELD, 1, false, _instance.artifactMaterials[3], _instance.artifactSprites[3]));
        list[3].Add(new Artifact(_instance.shieldName, WeaponType.SHIELD, 1, false, _instance.artifactMaterials[3], _instance.artifactSprites[3]));

        // HALMET
        list.Add(new List<Artifact>());
        list[4].Add(new Artifact(_instance.halmetName, WeaponType.MAGIC_SHIELD, 2, false, _instance.artifactMaterials[4], _instance.artifactSprites[4]));
        list[4].Add(new Artifact(_instance.halmetName, WeaponType.MAGIC_SHIELD, 1, false, _instance.artifactMaterials[4], _instance.artifactSprites[4]));
        list[4].Add(new Artifact(_instance.halmetName, WeaponType.MAGIC_SHIELD, 1, false, _instance.artifactMaterials[4], _instance.artifactSprites[4]));

        // BOOTS
        list.Add(new List<Artifact>());
        list[5].Add(new Artifact(_instance.bootsName, WeaponType.STAMINA, 3, false, _instance.artifactMaterials[5], _instance.artifactSprites[5]));
        list[5].Add(new Artifact(_instance.bootsName, WeaponType.STAMINA, 2, false, _instance.artifactMaterials[5], _instance.artifactSprites[5]));
        list[5].Add(new Artifact(_instance.bootsName, WeaponType.STAMINA, 1, false, _instance.artifactMaterials[5], _instance.artifactSprites[5]));
        list[5].Add(new Artifact(_instance.bootsName, WeaponType.STAMINA, 1, false, _instance.artifactMaterials[5], _instance.artifactSprites[5]));

        // FIREBALL
        list.Add(new List<Artifact>());
        list[6].Add(new Artifact(_instance.fireballName, WeaponType.MAGIC, 2, true, _instance.artifactMaterials[6], _instance.artifactSprites[6]));
        list[6].Add(new Artifact(_instance.fireballName, WeaponType.MAGIC, 1, true, _instance.artifactMaterials[6], _instance.artifactSprites[6]));
        list[6].Add(new Artifact(_instance.fireballName, WeaponType.MAGIC, 1, true, _instance.artifactMaterials[6], _instance.artifactSprites[6]));

        // FROSTBOLT
        list.Add(new List<Artifact>());
        list[7].Add(new Artifact(_instance.frostboltName, WeaponType.MAGIC, 2, true, _instance.artifactMaterials[7], _instance.artifactSprites[7]));
        list[7].Add(new Artifact(_instance.frostboltName, WeaponType.MAGIC, 1, true, _instance.artifactMaterials[7], _instance.artifactSprites[7]));
        list[7].Add(new Artifact(_instance.frostboltName, WeaponType.MAGIC, 1, true, _instance.artifactMaterials[7], _instance.artifactSprites[7]));

        return list;
    }

    public GameStrategy[] GetStrategies()
    {
        return new GameStrategy[2]
        {
            new RoadBuilding(),
            new CharactersActions(skipTurnButton, artifactManager),
        };
    }

    public ArrayList GetActiveLocationsList()
    {
        return activeLocations;
    }

    public void ActivateLocation(string locationName)
    {
        activeLocations.Add(locationName);
    }

    public HexUnit GetHexUnit(HexUnit prefab)
    {
        return Instantiate(prefab);
    }

    public RoadCell GetRoadCell()
    {
        return Instantiate(Instance.roadcellPrefab);
    }

    public RoadCell GetArtifact()
    {
        return Instantiate(Instance.roadcellPrefab);
    }

    public Sprite GetWeaponSprite(WeaponType type)
    {
        return WeaponTypes[(int)type];
    }

    public WeaponType GetDefenciveWeapon(WeaponType attackType, bool aggressiveDefense)
    {
        if (attackType == WeaponType.BOW)
            return WeaponType.SHIELD;
        if (attackType == WeaponType.SWORD)
            return aggressiveDefense ? WeaponType.SWORD : WeaponType.SHIELD;
        if (attackType == WeaponType.MAGIC)
            return WeaponType.MAGIC_SHIELD;
        return attackType;
    }

    public int GetWeaponStatValue(HexUnit unit, WeaponType weaponType)
    {
        switch(weaponType)
        {
            case WeaponType.SWORD:
                return unit.ATK;
            case WeaponType.SHIELD:
                return unit.DEF;
            case WeaponType.BOW:
                return unit.RANGE_ATK;
            case WeaponType.MAGIC:
                return unit.MAGIC_ATK;
            case WeaponType.MAGIC_SHIELD:
                return unit.MAGIC_DEF;
        }
        return 0;
    }
}
