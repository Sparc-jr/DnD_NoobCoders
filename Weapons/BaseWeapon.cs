namespace DnD_NoobCoders
{
    internal class Weapon
    {
        private string name;
        internal string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }
        private int damage;
        internal int Damage
        {
            get
            {
                return damage;
            }
            set
            {
                damage = value;
            }
        }
        private int bonusDamage;
        internal int BonusDamage
        {
            get
            {
                return bonusDamage;
            }
            set
            {
                bonusDamage = value;
            }
        }
        private int bonusMagicDamage;
        internal int BonusMagicDamage
        {
            get
            {
                return bonusMagicDamage;
            }
            set
            {
                bonusMagicDamage = value;
            }
        }
        private Hero owner;
        internal Hero Owner
        {
            get
            {
                return owner;
            }
            set
            {
                owner = value;
            }
        }
        internal Weapon(Hero owner)
        {
            Owner = owner;
        }

    }
}