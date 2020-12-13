
using UnityEngine;
using UnityEngine.UI;

namespace Assets.MenuScripts.Fight
{
    public class FightDialog : MonoBehaviour
    {
        public GameObject self;

        public GameObject selectTypeOfAttackDialog;
        public Text AtkLabel;
        public Text RangeAtkLabel;
        public Text MagicAtkLabel;

        public GameObject RunLuckPhaseButton;
        public GameObject CloseDialogButton;

        HexUnit attacker;
        HexUnit defender;

        WeaponType attackerWeaponType;
        WeaponType defenderWeaponType;

        public Image attackerPreview;
        public Image defenderPreview;

        public Text attackerName;
        public Text defenderName;

        public Text attackerHealth;
        public Text defenderHealth;

        public Image attackTypePreview;
        public Image defenceTypePreview;

        public Text attackBaseText;
        public Text attackLuckText;
        public Text attackSumText;

        public Text defenceBaseText;
        public Text defenceLuckText;
        public Text defenceSumText;

        public Text message;

        bool firstFight;
        bool BaskIsKil;

        bool isSelectingTypeOfAttack;

        int GetLuckBonus()
        {
            return Random.Range(1, 7);
        }

        public void OpenDialog(HexUnit attacker, HexUnit defender, bool canChooseWeapon)
        {
            GameManager.Instance.SetDialogState(true);
            BaskIsKil = false;
            firstFight = true;
            this.attacker = attacker;
            this.defender = defender;

            SetupDialogState(canChooseWeapon);
        }

        void SetRandomWeaponTypes()
        {
            attackerWeaponType = (WeaponType)Random.Range(0, 3);
            defenderWeaponType = DataController.Instance.GetDefenciveWeapon(attackerWeaponType, false); // may be implemented in future
        }

        public void SetWeaponType(int atkWeapon)
        {
            attackerWeaponType = (WeaponType)atkWeapon;
            defenderWeaponType = DataController.Instance.GetDefenciveWeapon(attackerWeaponType, false); // may be implemented in future
            
            selectTypeOfAttackDialog.SetActive(false);
            SetupHelper();
        }

        void AskForAtkType()
        {
            AtkLabel.text = attacker.ATK.ToString();
            RangeAtkLabel.text = attacker.RANGE_ATK.ToString();
            MagicAtkLabel.text = attacker.MAGIC_ATK.ToString();

            selectTypeOfAttackDialog.SetActive(true);
        }

        void SetupDialogState(bool canChooseWeapon)
        {
            if (canChooseWeapon)
            {
                AskForAtkType(); 
            }
            else
            {
                SetRandomWeaponTypes();
                SetupHelper();
            }
        }

        public void SetupHelper()
        {
            attackLuckText.text = "";
            defenceLuckText.text = "";
            attackSumText.text = "";
            defenceSumText.text = "";

            attackerHealth.text = attacker.HP.ToString();
            defenderHealth.text = defender.HP.ToString();

            attackerPreview.sprite = attacker.previewImage;
            defenderPreview.sprite = defender.previewImage;

            attackerName.text = attacker.name;
            defenderName.text = defender.name;

            attackTypePreview.sprite = DataController.Instance.GetWeaponSprite(attackerWeaponType);
            defenceTypePreview.sprite = DataController.Instance.GetWeaponSprite(defenderWeaponType);

            attackBaseText.text = DataController.Instance.GetWeaponStatValue(attacker, attackerWeaponType).ToString();
            defenceBaseText.text = DataController.Instance.GetWeaponStatValue(defender, defenderWeaponType).ToString();

            message.text = "Press the button to attack";

            UpdateButtonState(false);
            self.SetActive(true);
        }

        void UpdateButtonState(bool isLuckPhaseActive)
        {
            CloseDialogButton.SetActive(isLuckPhaseActive);
            RunLuckPhaseButton.SetActive(!isLuckPhaseActive);
        }

        public void RunLuckPhase()
        {
            UpdateButtonState(true);

            int attackerBonus = GetLuckBonus();
            int defenderBonus = GetLuckBonus();
            int attackerSum = attackerBonus + DataController.Instance.GetWeaponStatValue(attacker, attackerWeaponType);
            int defenderSum = defenderBonus + DataController.Instance.GetWeaponStatValue(defender, defenderWeaponType);

            int delta = attackerSum - defenderSum;
            string BaskName = "";
            if (delta > 0)
            {
                BaskName = defender.name;
                BaskIsKil = defender.TakeDamage(delta);
                if (BaskIsKil)
                {
                    attacker.LevelUp();
                }
            }
            else if (delta < 0 && attackerWeaponType == WeaponType.MAGIC)
            {
                BaskName = attacker.name;
                BaskIsKil = attacker.TakeDamage(-delta);
                if (BaskIsKil)
                {
                    defender.LevelUp();
                }
            }

            attackLuckText.text = attackerBonus.ToString();
            defenceLuckText.text = defenderBonus.ToString();
            attackSumText.text = attackerSum.ToString();
            defenceSumText.text = defenderSum.ToString();

            if (BaskName.Equals(""))
            {
                message.text = "Nobody got hurt";
            }
            else
            {
                string lethal = BaskIsKil ? "lethal" : "";
                message.text = $"{BaskName} takes {Mathf.Abs(delta)} damage{lethal}";
            }

            attackerHealth.text = attacker.HP.ToString();
            defenderHealth.text = defender.HP.ToString();
        }

        public void CloseDialog()
        {
            if(firstFight && !BaskIsKil)
            {
                firstFight = false;
                HexUnit attacketTmp = attacker;
                attacker = defender;
                defender = attacketTmp;
                SetupDialogState(false);
            }
            else
            {
                firstFight = true;
                self.SetActive(false);
                GameManager.Instance.SetDialogState(false);
                if (GameManager.Instance.isManticoreDead)
                {
                    GameManager.Instance.Win();
                }
            }
        }
    }
}
