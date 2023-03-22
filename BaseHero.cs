namespace DnD_NoobCoders
{
    internal class Hero
    {
        //internal enum string role { }
        internal enum Action { Stay, Attack, CastSpell, Die};
        
        private Weapon weapon;
        public Weapon Weapon
        {
            get
            {
                return weapon;
            }
            set
            {
                weapon = value;
            }
        }        
        private Spell spell;
        public Spell Spell
        {
            get
            {
                return spell;
            }
            set
            {
                spell = value;
            }
        }   
        private int power;
        internal int Power
        {
            get
            {
                return power;
            }
            set
            {
                power = value;
            }
        }
        private int agility;
        internal int Agility 
        {
            get
            {
                return agility;
            }
            set
            {
                agility = value; Armor = Agility / 2;
            } 
        }
        private int vitality;
        internal int Vitality
        {
            get
            {
                return vitality;
            }
            set
            {
                vitality = value; Health = Vitality * 4;
            }
        }
        private int intellect;
        internal int Intellect
        {
            get
            {
                return intellect;
            }
            set
            {
                intellect = value; Mana = Intellect * 4; MagicArmor = Intellect / 2;
            }
        }
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
        private int health;
        internal int Health 
        {
            get
            {
                return health;
            }
            set
            {
                health = value < 0 ? 0 : value;
            }
        }
        private int mana;
        internal int Mana 
        { 
            get
            {
                return mana;
            }
            set
            {
                mana = value; 
            }
        }
        private int armor;
        internal int Armor
        {
            get
            {
                return armor;
            }
            set
            {
                armor = value;
            }
        }
        private int magicArmor;
        internal int MagicArmor 
        {
            get
            {
                return magicArmor;
            }
            set
            {
                magicArmor = value;
            }
        }
        
        public Hero(string name, int power, int agility, int vitality, int intellect)
        { 
            Name = name;
            Power = power;
            Agility = agility;
            Vitality = vitality;
            Intellect = intellect;
        }
        public virtual (int hitPoints, int damage) Attack (Hero target)
        {
            var hitPoints = this.DealDamage();
            var madeDamage = target.GotDamage(hitPoints);
            target.Health -= madeDamage;
            StoryTelling.Attacked(this, target);
            StoryTelling.GotHit(target, madeDamage);
            return (hitPoints, madeDamage);
        }
        public virtual int DealDamage()
        {
            return this.Weapon.Damage + this.Weapon.BonusDamage;
        }
        public virtual int GotDamage(int damage)
        {
            var gotDamage = damage - this.Armor - this.Agility;
            return gotDamage > 0 ? gotDamage : 0;
        }
        public virtual int CastSpell(List<Hero> targets, Spell spell, bool isMyHeroes)
        {
            this.Mana -= spell.ManaCost;
            var dealtDamage = this.DealMagicDamage();
            var targetsCount = targets.Count;
            for (int i=0;i< targetsCount;i++)
            {
                if (targets[i].Health == 0) continue;
                var gotDamage = targets[i].GotMagicDamage(dealtDamage);
                targets[i].Health -= gotDamage;
                StoryTelling.AttackedBySpell(this, targets[i],spell);
                StoryTelling.GotHit(targets[i], gotDamage);
                StoryDrawer.DrawGotHit(targets[i], i, !isMyHeroes, gotDamage);
                if (targets[i].Health <= 0) targets[i].Defeated();
            }
            return dealtDamage;
        }
        public virtual int DealMagicDamage()
        {
            return this.Intellect + this.Weapon.BonusMagicDamage;
        }        
        public virtual int GotMagicDamage(int damage)
        {
            var gotDamage = damage - this.magicArmor - this.Intellect;
            return gotDamage > 0 ? gotDamage : 0;
        }
        public virtual void Defeated()
        {
            this.Health = 0;
            StoryTelling.Defeated(this);
        }
    }
}