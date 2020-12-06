using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.GameStrategy
{
    public abstract class GameStrategy
    {
        private bool mouseDownHappened;
        private bool mouseUpHappened;

        protected HexGrid grid
        {
            get
            {
                return GameManager.Instance.hexGrid;
            }
        }

        public abstract void Update();

        public virtual void Start()
        {
            mouseDownHappened = false;
            mouseUpHappened = false;
        }

        protected void HandleClick(Action handler)
        {
            if (Input.GetMouseButtonDown(0) && !mouseDownHappened)
            {
                mouseDownHappened = true;
            }

            if (Input.GetMouseButtonUp(0) && mouseDownHappened)
            {
                mouseUpHappened = true;
            }

            if (mouseUpHappened)
            {
                mouseDownHappened = false;
                mouseUpHappened = false;
                handler();
            }
        }
    }
}
