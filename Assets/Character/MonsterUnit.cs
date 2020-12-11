﻿
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Character
{
    class MonsterUnit : HexUnit
    {
        List<HexCell> way;

        void RemoveLastCellFromWay()
        {
            way?.RemoveAt(0);
        }

        public void SetupSelfWay()
        {
            List<List<HexCell>> possibleWays = new List<List<HexCell>>();
            bool canReachCastle = SelectPossibleWay(
                Location,
                possibleWays,
                new List<HexCell>()
            );


            for (int i = 0; i < possibleWays.Count; i++)
            {
                if (way == null || way.Count > possibleWays[i].Count)
                    way = possibleWays[i];
            }

            RemoveLastCellFromWay();
        }

        bool SelectPossibleWay(
            HexCell currentCell, 
            List<List<HexCell>> possibleWays,
            List<HexCell> visitedCells
        ) {

            // The end of field
            if (currentCell == null)
                return false;

            //Castle is reached
            if (currentCell.name.Equals(DataController.Instance.castleName))
            {
                List<HexCell> newPath = new List<HexCell>();
                newPath.Add(currentCell);
                possibleWays.Add(newPath);
                return true;
            }

            // Check if current cell was visted
            if(visitedCells.Find(visitedCell => visitedCell == currentCell) != null)
            {
                return false;
            }
            visitedCells.Add(currentCell);

            bool canReachCastle = false;
            for(int i = 0; i < currentCell.neighbors.Length; i++)
            {
                if(currentCell.IsNeighborAchievable((HexDirection)i))
                {
                    // remember currenct count to add current cell to the new pathes
                    int numberOfPathesWithoutCurrentCell = possibleWays.Count;
                    bool successfullyReachedCastle = SelectPossibleWay(
                        currentCell.GetNeighbor((HexDirection)i),
                        possibleWays,
                        visitedCells
                    );
                    if(successfullyReachedCastle)
                    {
                        canReachCastle = true;
                        // Current cell should be added on the first position
                        for (
                            int j = numberOfPathesWithoutCurrentCell;
                            j < possibleWays.Count; 
                            j++
                        ) {
                            possibleWays[j].Insert(0, currentCell);
                        }
                    }
                }
            }

            // If the castle is reachable from the current cell, 
            // we need to remove it from visited cells.
            // Perhaps, the current cell can be reached in a shorter way.
            // If the castle is not reachable, 
            // we should not be trapped on this cell again
            if (canReachCastle)
            {
                visitedCells.Remove(currentCell);
            }

            return canReachCastle;
        }

        public override void TakeTurn()
        {
            base.TakeTurn();
            while (STAMINA > 0)
            {
                HexUnit unitToAttack = GetUnitToAttack();
                if(unitToAttack != null)
                {
                    Attack(unitToAttack);
                    continue;
                }

                if(way == null)
                {
                    SetupSelfWay();
                }
                if (way == null)
                    break;

                if(way.Count == 0)
                {
                    // TODO: ATTACK CASTLE
                    break;
                }
                HexCell targetCell = way[0];
                if(targetCell.unit != null)
                {
                    Location = way[0];
                    RemoveLastCellFromWay();
                    STAMINA--;
                }
                else
                {
                    break;
                }
            }

            EndTurn();
        }

        protected HexUnit GetUnitToAttack()
        {
            foreach (HexCell cell in Location.neighbors)
            {
                if(cell?.unit?.isEnemy == false)
                {
                    Debug.Log($"Monster {name} -> Hero {cell.unit.name}");
                    return cell.unit;
                }
            }
            return null;
        }

        void Attack(HexUnit unit)
        {
            STAMINA--;
            // TODO
        }
         
        void RefreshWayIfNeeded()
        {

        }
    }
}