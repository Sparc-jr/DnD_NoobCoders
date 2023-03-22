namespace DnD_NoobCoders
{
    internal class Sword : Weapon
    {
        internal Sword(Hero owner) : base (owner)
        {
            Name = "sword";
            Damage = 5;
            BonusDamage = owner.Power;
        }
    }
    internal class Dagger : Weapon
    {
        internal Dagger(Hero owner) : base(owner)
        {
            Name = "dagger";
            Damage = 4;
            BonusDamage = owner.Agility;
        }
    }
    internal class Staff : Weapon
    {
        internal Staff(Hero owner) : base(owner)
        {
            Name = "staff";
            Damage = 15;
            BonusDamage = owner.Power;
            BonusMagicDamage = 10;
        }
    }
}