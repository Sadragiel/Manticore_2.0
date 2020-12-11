using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Artifacts
{
    public class Artifact
    {
        public Material material;
        public Sprite image;
        string name;
        int bonus;
        bool isConsumable;

        public Artifact(string name, int bonus, bool isConsumable, Material material, Sprite image)
        {
            this.name = name;
            this.bonus = bonus;
            this.isConsumable = isConsumable;
            this.material = material;
            this.image = image;
        }
    }
}
