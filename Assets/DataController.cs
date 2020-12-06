using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using Assets.DataStructures;
using Assets.GameStrategy;


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

    public HexUnit unitPrefab;
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

    public Location[] locations;
    public Cell[] cellsData;
    public Material[] roadCellMaterials;

    ArrayList activeLocations;

    private static void Initialize()
    {
        _instance = GameObject.FindObjectOfType<DataController>();
        
        // Locations have constant coordinates on the map
        _instance.locations = new Location[9] {
            new Location(_instance.castleName, new int[4]{ 75, 89, 90, 103 }, Color.green),
            new Location(_instance.necklaceName, new int[3]{ 168, 169, 154 }, Color.blue),
            new Location(_instance.swordName, new int[4]{ 174, 175, 159, 160 }, Color.blue),
            new Location(_instance.bowName, new int[4]{ 6, 7, 20, 21 }, Color.blue),
            new Location(_instance.shieldName, new int[4]{ 82, 96, 97, 110 }, Color.blue),
            new Location(_instance.halmetName, new int[3]{ 12, 13, 26 }, Color.blue),
            new Location(_instance.bootsName, new int[4]{ 70, 84, 85, 98 }, Color.blue),
            new Location(_instance.fireballName, new int[3]{ 136, 150, 151 }, Color.blue),
            new Location(_instance.frostboltName, new int[3]{ 0, 1, 14 }, Color.blue),
        };

        _instance.cellsData = new Cell[7] {
            new Cell(_instance.roadCellMaterials[0], "birdpaw", 3, new bool[6] { false, true, true, true, false, true }),
            new Cell(_instance.roadCellMaterials[1], "crossroad", 3, new bool[6] { true, false, true, true, false, true }),
            new Cell(_instance.roadCellMaterials[2], "fork", 7, new bool[6] { false, true, true, false, false, true }),
            new Cell(_instance.roadCellMaterials[3], "starroad", 1, new bool[6] { true, true, true, true, true, true }),
            new Cell(_instance.roadCellMaterials[4], "turnroad", 10, new bool[6] { false, true, false, true, false, false }),
            new Cell(_instance.roadCellMaterials[5], "y_type", 7, new bool[6] { false, true, false, true, false, true }),
            new Cell(_instance.roadCellMaterials[6], "straightroad", 10, new bool[6] { false, false, true, false, false, true }),
        };

        _instance.activeLocations = new ArrayList();
        _instance.activeLocations.Add(_instance.castleName);
    }

    public GameStrategy[] GetStrategies()
    {
        return new GameStrategy[2]
        {
            new RoadBuilding(),
            new CharactersActions(),
        };  
    }

    public RoadCell GetRoadCell()
    {
        return Instantiate(DataController.Instance.roadcellPrefab);
    }
    
    public ArrayList GetActiveLocationsList()
    {
        return activeLocations;
    }

    public void ActivateLocation(string locationName)
    {
        activeLocations.Add(locationName);
    }
}
