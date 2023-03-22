namespace DnD_NoobCoders
{
    internal class Knight : Hero 
    {
        public Knight(string name, int power, int agility, int vitality, int intellect) : base (name, power, agility ,vitality, intellect)
        {
            Health += 15;
            Power += 2;
            Armor += 2;
            Weapon = new Sword(this);
        }
    }
    internal class Thief : Hero
    {
        public Thief(string name, int power, int agility, int vitality, int intellect) : base(name, power, agility, vitality, intellect)
        {
            Agility += 3;
            Weapon = new Dagger(this);
        }
    }
    internal class Mage : Hero
    {
        public Mage(string name, int power, int agility, int vitality, int intellect) : base(name, power, agility, vitality, intellect)
        {
            Intellect += 5;
            Mana += 25;
            MagicArmor += 2;
            Weapon = new Staff(this);
            Spell = new ChainLightning(this);
        }
    }
}