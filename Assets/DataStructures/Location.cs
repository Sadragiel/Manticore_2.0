using UnityEngine;

namespace Assets.DataStructures
{
    public struct Location
    {
        public string name;
        public int[] cells;
        public Color color;

        public Location(string name, int[] cells, Color color)
        {
            this.name = name;
            this.cells = cells;
            this.color = color;
        }
    }
}
