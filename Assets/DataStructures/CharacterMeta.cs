using UnityEngine;

namespace Assets.DataStructures
{
    public struct CharacterMeta
    {
        public Material material;
        public string name;
        public string initLocation;

        public CharacterMeta(Material material, string name, string initLocation)
        {
            this.name = name;
            this.material = material;
            this.initLocation = initLocation;
        }
    }
}
