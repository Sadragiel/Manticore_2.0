using System;
using UnityEngine;
using Assets.DataStructures;

namespace Assets.GameStrategy
{
    public class RoadBuilding : GameStrategy
    {
        RoadCell currentCell;
        [SerializeField]
        public bool[] roadDirections;

        bool isAvailableToSet;

        public override void Start()
        {
            base.Start();
            currentCell = GetRandomCell();
        }

        public override void Update()
        {
            HandleClick(SetCellToTheGrid);

            if (Input.GetAxis("Mouse ScrollWheel") != 0f)
            {
                PerformRotation(Input.GetAxis("Mouse ScrollWheel"));
            }

            MoveCellToPointer();
        }

        

        RoadCell CreateCell(Cell cellData)
        {
            RoadCell roadcell = DataController.Instance.GetRoadCell();
            roadcell.transform.SetParent(grid.transform, false);
            roadcell.setMaterial(cellData.material);
            roadcell.name = cellData.name;

            roadDirections = new bool[cellData.directions.Length];
            Array.Copy(cellData.directions, roadDirections, roadDirections.Length);
            
            return roadcell;
        }

        RoadCell GetRandomCell()
        {
            var cellList = DataController.Instance.cellsData;

            int amoungOfCells = 0;
            foreach(var cellData in cellList)
            {
                amoungOfCells += cellData.cellLeft;
            }

            int randomNumber = UnityEngine.Random.Range(0, amoungOfCells);

            for (int i = 0; i < cellList.Length; i++)
            {
                if(randomNumber >= cellList[i].cellLeft)
                {
                    randomNumber -= cellList[i].cellLeft;
                }
                else
                {
                    cellList[i].cellLeft--;
                    return CreateCell(cellList[i]);
                }
            }
            return null;
        }

        void PerformRotation(float shift)
        {
            currentCell.Rotate(shift);

            if(Math.Sign(shift) == 1)
            {
                for (int i = 0; i < roadDirections.Length - 1; i++)
                {
                    int nextIndex = (i + 1) % roadDirections.Length;

                    bool tmp = roadDirections[nextIndex];
                    roadDirections[nextIndex] = roadDirections[i];
                    roadDirections[i] = tmp;
                }
            }
            else
            {
                for (int i = roadDirections.Length - 1; i > 0; i--)
                {
                    int nextIndex = (i - 1 + roadDirections.Length) % roadDirections.Length;

                    bool tmp = roadDirections[nextIndex];
                    roadDirections[nextIndex] = roadDirections[i];
                    roadDirections[i] = tmp;
                }
            }

            checkAvailabilityToSet();
        }

        void checkAvailabilityToSet()
        {
            HexCell cellForPlacing = currentCell.Location;

            if (cellForPlacing.isBusy())
            {
                MarkUnavailable();
                return;
            }

            /*
                collisionState
                    true - set is available, there are no collisions
                    false - set is not available, there are some collisions
                    null - set is not available, there are no collisions, but no connection to other roads
             */
            bool? collisionState = null; 

            for(int i = 0; i < roadDirections.Length && collisionState != false; i++)
            {
                HexDirection direction = (HexDirection)i;
                HexCell directedCell = cellForPlacing.GetNeighbor(direction);

                if(directedCell == null || !directedCell.isBusy())
                    continue;

                if(!roadDirections[i]) {
                    if(directedCell.IsNeighborAchievable(direction.Opposite()))
                        collisionState = false;
                    continue;
                }

                if (directedCell.IsLocation())
                {
                    if (directedCell.IsActiveLocation())
                    {
                        collisionState = true;
                    }
                }
                else {
                    collisionState = directedCell.IsNeighborAchievable(direction.Opposite());
                }
            }

            if(collisionState != true)
            {
                MarkUnavailable();
            }
            else
            {
                MarkAvailable();
            }
        }

        void MarkUnavailable()
        {
            currentCell.Highlight.SetActive(true);
            isAvailableToSet = false;
        }

        void MarkAvailable()
        {
            if (currentCell.Location == null)
                return;
            currentCell.Highlight.SetActive(false);
            isAvailableToSet = true;
        }

        public void ActivateLocationsIfNeeded()
        {
            for (int i = 0; i < roadDirections.Length ; i++)
            {
                HexDirection direction = (HexDirection)i;
                HexCell directedCell = currentCell.Location.GetNeighbor(direction);
                if(
                    roadDirections[i]
                    && directedCell != null 
                    && directedCell.IsLocation()
                    && !directedCell.IsActiveLocation())
                {
                    DataController.Instance.ActivateLocation(directedCell.locationName);
                }
            }
        }

        void SetCellToTheGrid()
        {
            if (isAvailableToSet)
            {
                //set up previous cell
                currentCell.Location.name = currentCell.name;
                currentCell.Location.SetAchievableNeighbors(roadDirections);
                ActivateLocationsIfNeeded();

                //get new cell
                currentCell = GetRandomCell();

                if(currentCell == null)
                {
                    GameManager.Instance.RunNextStrategy();
                }
            }
        }

        void MoveCellToPointer()
        {
            HexCell cell = grid.getHexCellFromPointer();

            if (currentCell != null && cell != null && currentCell.Location != cell)
            {
                MarkAvailable();
                currentCell.Location = cell;
                checkAvailabilityToSet();
            }
        }
    }
}
