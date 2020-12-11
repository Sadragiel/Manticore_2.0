using UnityEngine;
using Assets.Artifacts;
using System.Collections.Generic;

namespace Assets.DataStructures
{
    public struct Location
    {
        public string name;
        public int[] cells;
        public Color color;
        public int artifactHost;
        public List<Artifact> artifactStock;

        public Location(
            string name,
            int[] cells,
            Color color,
            List<Artifact> artifactStock,
            int artifactHost
        ) {
            this.name = name;
            this.cells = cells;
            this.color = color;
            this.artifactStock = artifactStock;
            this.artifactHost = artifactHost;
        }
    }
}
