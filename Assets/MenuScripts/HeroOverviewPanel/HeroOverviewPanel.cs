
using UnityEngine;
using UnityEngine.UI;

namespace Assets.MenuScripts.HeroOverviewPanel
{
    public class HeroOverviewPanel : MonoBehaviour
    {
        public GameObject self;

        public Image preview;
        public Text Name;
        public Text HP;
        public Text STAMINA;
        public Text ATK;
        public Text MAGIC_ATK;
        public Text RANGE_ATK;
        public Text DEF;
        public Text MAGIC_DEF;
        public Text BAG;

        public void SetCharacterInfo(HexUnit unit)
        {
            preview.sprite = unit.previewImage;
            Name.text = unit.name;
            HP.text = $"{unit.HP}/{unit.MAX_HP}";
            STAMINA.text = $"{unit.STAMINA}/{unit.MAX_STAMINA}";
            ATK.text = $"{unit.ATK}";
            MAGIC_ATK.text = $"{unit.MAGIC_ATK}";
            RANGE_ATK.text = $"{unit.RANGE_ATK}";
            DEF.text = $"{unit.DEF}";
            MAGIC_DEF.text = $"{unit.MAGIC_DEF}";
            BAG.text = $"{unit.bag.Count}";
        }

    }
}
