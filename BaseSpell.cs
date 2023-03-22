namespace DnD_NoobCoders
{
    internal class Spell
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
        private int manaCost;
        internal int ManaCost
        {
            get
            {
                return manaCost;
            }
            set
            {
                manaCost = value;
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
        internal Spell(Hero owner)
        {
            Owner = owner;
        }
    }
}