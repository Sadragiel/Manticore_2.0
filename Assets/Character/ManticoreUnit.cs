using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Character
{
    public class ManticoreUnit : MonsterUnit
    {

        public override void TakeTurn()
        {
            if (DataController.Instance.ManticoreMovementAvailable)
            {
                base.TakeTurn();
            }
            else
            {
                EndTurn();
            }
        }

        public override void SetupSelfWay()
        {
            way = new List<HexCell>();
            way.Add(grid.GetCell(166));
            way.Add(grid.GetCell(180));
            way.Add(grid.GetCell(179));
            way.Add(grid.GetCell(178));
            way.Add(grid.GetCell(163));
            way.Add(grid.GetCell(162));
            way.Add(grid.GetCell(148));
            way.Add(grid.GetCell(134));
            way.Add(grid.GetCell(121));
            way.Add(grid.GetCell(107));
            way.Add(grid.GetCell(106));
            way.Add(grid.GetCell(105));
            way.Add(grid.GetCell(91));
            way.Add(grid.GetCell(90)); // Castle
        }


        public override void AttackCastle()
        {
            GameManager.Instance.Defeat(true);
        }

        protected override void Die()
        {
            GameManager.Instance.isManticoreDead = true;
        }
    }
}

