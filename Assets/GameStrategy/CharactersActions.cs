using Assets.Artifacts;
using Assets.DataStructures;
using System.Collections.Generic;
using UnityEngine.UI;
using Assets.MenuScripts.HeroOverviewPanel;

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
        ArtifactManagement artifactManager;

        public HexUnit currentUnit {
            get
            {
                return units[currentUnitIndex];
            }
        }

        int currentUnitIndex = -1;

        int TurnNumber = 0;

        List<HexUnit> units;

        public CharactersActions(Button skipTurnButton, ArtifactManagement artifactManager)
        {
            skipTurnButton.onClick.AddListener(EndTurn);
            this.artifactManager = artifactManager;
        }

        public override void Update()
        {
        }

        void EndTurn()
        {
            currentUnit.EndTurn();
        }

        public void NextTurn()
        {
            currentUnitIndex = (currentUnitIndex + 1) % units.Count;
            if(currentUnitIndex == 0)
            {
                TurnNumber++;
                if(TurnNumber == 10)
                {
                    DataController.Instance.ManticoreMovementAvailable = true;
                }
            }
            currentUnit.TakeTurn();
        }

        public override void Start()
        {
            base.Start();
            GameManager.Instance.ShowOverviewPanel();
            units = new List<HexUnit>();
            setCharactersOnInitiaPosition();
            NextTurn();
        }

        HexUnit CreateUnit(CharacterMeta metadata, HexCell location)
        {
            HexUnit hexUnit = DataController.Instance.GetHexUnit(metadata.unitPrefab);
            hexUnit.transform.SetParent(grid.transform, false);
            hexUnit.SetMetadata(metadata);
            if(location != null)
            {
                hexUnit.Location = location;
            }
            return hexUnit;
        }

        void setCharactersOnInitiaPosition()
        {
            CharacterMeta[] metadataArray = DataController.Instance.charactersData;

            foreach(CharacterMeta metadata in metadataArray)
            {
                List<HexCell> cellList;
                if(metadata.initLocation.Equals(DataController.Instance.castleName))
                {
                    cellList = new List<HexCell>();
                    cellList.Add(grid.GetFreeCell(metadata.initLocation));
                }
                else
                {
                    cellList = grid.GetCellsByName(metadata.initLocation);
                }
                cellList.ForEach(cellItem =>
                {
                    HexUnit unit = CreateUnit(metadata, cellItem);
                    units.Add(unit);
                });
            }
        }

        public void OpenArtifactManagementDialog(List<Artifact> targetArtifacts)
        {
            var list = targetArtifacts ?? currentUnit.Location.artifactStock;
            artifactManager.OpenDialog(currentUnit.equipped, currentUnit.bag, list, targetArtifacts != null);
        }

        public void CloseArtifactManagementDialog()
        {
            artifactManager.CloseDialog();
        }

        public void HandleDeath(HexUnit deadUnit)
        {
            DataController.Instance.ManticoreMovementAvailable = true;
            int indexOfDeadUnit = units.IndexOf(deadUnit);
            if(currentUnitIndex <= indexOfDeadUnit)
            {
                currentUnitIndex--;
            }

            units.Remove(deadUnit);
        }
    }
}
