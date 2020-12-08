using Assets.DataStructures;
using System.Collections.Generic;

namespace Assets.GameStrategy
{

    /*
     * Class should create all needed units on Start event: 
     *      - Heroes in the Castle
     *      - Manticore in her Cave
     *      - Monsters on their road cases
     *     
     * On Update event it should run the turn method of the characters one by one.
     *      - 
     */
    class CharactersActions : GameStrategy
    {
        HexUnit currentUnit {
            get
            {
                return units[currentUnitIndex];
            }
        }

        int currentUnitIndex = -1;

        List<HexUnit> units;

        public override void Update()
        {
            HandleClick(nextTurn);
        }

        void nextTurn()
        {
            if(currentUnitIndex != -1)
            {
                currentUnit.Deactivate();
            }

            currentUnitIndex = (currentUnitIndex + 1) % units.Count;

            currentUnit.Activate();

        }

        public override void Start()
        {
            base.Start();
            units = new List<HexUnit>();
            setCharactersOnInitiaPosition();
            nextTurn();
        }

        HexUnit CreateUnit(CharacterMeta metadata)
        {
            HexUnit hexUnit = DataController.Instance.GetHexUnit();
            hexUnit.transform.SetParent(grid.transform, false);
            hexUnit.setMaterial(metadata.material);
            return hexUnit;
        }

        void setCharactersOnInitiaPosition()
        {
            CharacterMeta[] metadataArray = DataController.Instance.charactersData;

            foreach(CharacterMeta metadata in metadataArray)
            {
                HexCell cell = grid.GetFreeCell(metadata.initLocation);
                HexUnit unit = CreateUnit(metadata);
                if(cell != null && unit != null)
                {
                    unit.Location = cell;
                    units.Add(unit);
                }
            }
        }

        //HexUnit CreateUnit(HexCell cell)
        //{
        //    HexUnit unit = Instantiate(DataController.Instance.unitPrefab);
        //    unit.transform.SetParent(hexGrid.transform, false);
        //    unit.Location = cell;
        //    return unit;
        //}
    }
}
