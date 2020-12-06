using UnityEngine;

namespace Assets.DataStructures
{
    public struct Cell
    {
        public int numberOfCells;
        public int cellLeft;
        public bool[] directions;
        public string name;
        public Material material;

        public Cell(Material material, string name, int numberOfCells, bool[] directions)
        {
            this.name = name;
            this.numberOfCells = numberOfCells;
            this.cellLeft = numberOfCells;
            this.directions = directions;
            this.material = material;
        }
    }
}
