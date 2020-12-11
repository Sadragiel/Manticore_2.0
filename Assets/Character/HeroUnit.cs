using System;
using UnityEngine;

namespace Assets.Character
{
    class HeroUnit : HexUnit
    {
        public void Update()
        {
            if (!isMyTurn || GameManager.Instance.IsDialogOpened)
                return;

            if (Input.GetMouseButtonUp(0))
            {
                HandleInput();
            }
        }

        protected bool CheckInteractionAvailability(HexCell targetCell)
        {
            return targetCell != null
                && targetCell != Location
                && Location.GetDirectionOfNeighbor(targetCell) != null;
        }

        protected void HandleInput()
        {
            HexCell targetCell = grid.getHexCellFromPointer();
            if (!CheckInteractionAvailability(targetCell))
            {
                return;
            }

            //Movement
            if (targetCell.unit == null)
            {
                int requiredStamina = Location.IsNeighborAchievable(targetCell) ? 1 : 3;
                if (this.STAMINA >= requiredStamina)
                {
                    this.STAMINA -= requiredStamina;
                    Location = targetCell;
                }
                return;
            }
        }
    }
}
