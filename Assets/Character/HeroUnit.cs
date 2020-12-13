using Assets.MenuScripts.HeroOverviewPanel;
using System;
using UnityEngine;

namespace Assets.Character
{
    class HeroUnit : HexUnit
    {
        bool canChooseWeapon;
        bool shouldUpdate;

        public override void TakeTurn()
        {
            base.TakeTurn();
            UpdateInfo();
            canChooseWeapon = true;
        }

        void UpdateInfo()
        {
            GameManager.Instance.SetCharacterInfo(this);
        }

        public void Update()
        {
            if (!isMyTurn)
                return;

            if(isMyTurn)
            {
                if(GameManager.Instance.IsDialogOpened)
                {
                    shouldUpdate = true;
                    return;
                }
            }

            if (shouldUpdate)
            {
                shouldUpdate = false;
                UpdateInfo();
            }

            if (Input.GetMouseButtonUp(0))
            {
                HandleInput();
            }
        }

        protected bool CheckInteractionAvailability(HexCell targetCell)
        {
            return targetCell != null
                && Location.GetDirectionOfNeighbor(targetCell) != null;
        }

        public void Heal()
        {
            if (STAMINA < 1 || HP >= MAX_HP)
                return;
            HP++;
            STAMINA--;
            UpdateInfo();
        }

        protected void HandleInput()
        {
            if (STAMINA == 0)
                return;

            HexCell targetCell = grid.getHexCellFromPointer();

            if(targetCell == Location)
            {
                Heal();
                return;
            }

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
                UpdateInfo();
                return;
            }

            if(targetCell.unit.isEnemy)
            {
                Attack(targetCell.unit, canChooseWeapon);
                canChooseWeapon = false;
            }
            else
            {
                GameManager.Instance.OpenArtifactManagementDialog(targetCell.unit);
            }
        }
    }
}
