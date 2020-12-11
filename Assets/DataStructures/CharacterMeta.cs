using UnityEngine;

namespace Assets.DataStructures
{
    public struct CharacterMeta
    {
        public Material material;
        public string name;
        public string initLocation;

        public int MAX_HP;
        public int MAX_STAMINA;
        public int ATK;
        public int MAGIC_ATK;
        public int RANGE_ATK;
        public int DEF;
        public int MAGIC_DEF;

        public CharacterMeta(
            Material material, 
            string name, 
            string initLocation,
            int MAX_HP,
            int MAX_STAMINA,
            int ATK,
            int MAGIC_ATK,
            int RANGE_ATK,
            int DEF,
            int MAGIC_DEF
        ) {
            this.name = name;
            this.material = material;
            this.initLocation = initLocation;
            this.MAX_HP = MAX_HP;
            this.MAX_STAMINA = MAX_STAMINA;
            this.ATK = ATK;
            this.MAGIC_ATK = MAGIC_ATK;
            this.RANGE_ATK = RANGE_ATK;
            this.DEF = DEF;
            this.MAGIC_DEF = MAGIC_DEF;
        }
    }
}
