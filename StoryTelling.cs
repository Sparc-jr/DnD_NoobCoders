using System.Text;

namespace DnD_NoobCoders
{
    internal static class StoryTelling
    {
        internal static void Introduction()
        {
            StoryDrawer.TextTyping("Stay a while and listen, and I will tell you a story. A story of Dungeons and Dragons, " +
                "of Orcs and Goblins, of Ghouls and Ghosts, of Kings and Quests, but most importantly -- of Heroes and NoobCo " +
                "-- Well... A story of Heroes.",(0,0));
        }
        internal static void PlotTwist(List<Hero> heroes, List<Hero> enemies)
        {
            var text = new StringBuilder();
            text.Append("So here starts the journey of our hero");
            var heroesList = new StringBuilder();
            var enemiesList = new StringBuilder();
            if (heroes.Count == 1)
            {
                text.Append(" ");
                heroesList.Append($"{heroes[0].Name}");                
            }
            else
            {
                text.Append("es: ");
                heroesList.AppendJoin(", ", heroes.Select(p => p.Name).ToList());                
            }
            text.Append(heroesList);
            text.Append(" got order to eliminate the local ");
            if (enemies.Count == 1)
            {
                text.Append("bandit known as ");
                enemiesList.Append($"{enemies[0].Name}");
            }
            else
            {
                text.Append("gang consists of well known bandits: ");
                enemiesList.AppendJoin(", ", enemies.Select( p => p.Name).ToList());
            }
            text.Append(enemiesList);
            text.Append('.');
            var oldCursorPosition = StoryDrawer.SaveAndMoveCursorTo(0, 28);
            Console.WriteLine(text.ToString());
            if (heroes.Count > 1) Console.WriteLine($"{heroesList.ToString()} engaged the {enemiesList.ToString()}.");
            Console.SetCursorPosition(oldCursorPosition.Left, oldCursorPosition.Top);
        }
        internal static void HappyFinal()
        {
            Console.WriteLine("Congratulations!");
        }
        internal static void GameOver(bool groupOfHeroes)
        {
            Console.WriteLine($"Unfortunately our hero{(groupOfHeroes?"es were": " was")} brave, yet not enought skilled, or just lack of luck.");
        }
        internal static void Attacked(Hero hero, Hero enemy)
        {
            Console.WriteLine($"{hero.Name} attacking {enemy.Name} with {hero.Weapon.Name}.");
        }
        internal static void AttackedBySpell(Hero hero, Hero enemy, Spell spell)
        {
            Console.WriteLine($"{hero.Name} attacking {enemy.Name} with {spell.Name}.");
        }
        internal static void GotHit(Hero hero, int damage)
        {
            Console.WriteLine($"{hero.Name} get hit for {damage} hp and have {hero.Health} hp left!");
        }
        internal static void Defeated(Hero hero)
        {
            Console.WriteLine($"{hero.Name} is defeated!");
        }
        internal static void PressEnterToContinue(int x, int y)
        {
            var previousPosition = StoryDrawer.SaveAndMoveCursorTo(x, y);
            var oldTextColor = StoryDrawer.SaveOldAndSetNewTextColor(ConsoleColor.Yellow);
            Console.WriteLine($"                              ");
            StoryDrawer.ShiftCursor(x);
            Console.WriteLine($" Press \"Enter\" to continue... ");
            StoryDrawer.ShiftCursor(x);
            Console.Write($"                              ");
            Console.ReadLine();
            Console.SetCursorPosition(previousPosition.Left, previousPosition.Top);
            Console.ForegroundColor = oldTextColor;
        }
    }
}