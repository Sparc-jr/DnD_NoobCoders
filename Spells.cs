namespace DnD_NoobCoders
{
    internal class ChainLightning : Spell
    {
        internal ChainLightning(Hero owner) : base(owner)
        {
            Name = "chain lightning";
            ManaCost = 40;
        }
    }
}