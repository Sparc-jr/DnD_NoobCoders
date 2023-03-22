using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Security.Cryptography;
using static System.Collections.Specialized.BitVector32;

namespace DnD_NoobCoders
{
    internal class Program
    {
        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();
        private static IntPtr ThisConsole = GetConsoleWindow();
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        private const int HIDE = 0;
        private const int MAXIMIZE = 3;
        private const int MINIMIZE = 6;
        private const int RESTORE = 9;
        static void Main(string[] args)
        {
            ShowWindow(ThisConsole, MAXIMIZE);
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            List<Hero> myHeroes = new List<Hero>();
            List<Hero> enemies = new List<Hero>();
            //Console.SetWindowSize(230, 65);

            StoryDrawer.DrawFromFile(0, "screens.ini", "StartGame", false, 65);
            StoryTelling.PressEnterToContinue(30, 50);
            Console.Clear();
            Console.WriteLine("\r\n   Сгенерировать команды случайным образом? y/n");
            var userAnswer = Console.ReadLine();
            switch (userAnswer)
            {
                case "y": GenerateTeams(out myHeroes, out enemies); break;
                default: UserInputTeams(out myHeroes, out enemies); break;
            }
            
            Console.Clear();
            StoryTelling.Introduction();
            StoryTelling.PressEnterToContinue(20, 13);
            Console.Clear();
            StoryTelling.PlotTwist(myHeroes, enemies);
            StoryDrawer.DrawTeam(myHeroes, true);
            StoryDrawer.DrawTeam(enemies, false);
            StoryTelling.PressEnterToContinue(20, 13);
            Console.Clear();
            bool allDiedInTeam = false;
            while (!allDiedInTeam)
            {
                StoryDrawer.DrawTeam(myHeroes, true);
                StoryDrawer.DrawTeam(enemies, false);
                var oldCursorPosition = StoryDrawer.SaveAndMoveCursorTo(0, 28);
                bool enemiesIsDefeated = MakeTurn(myHeroes, enemies, true);
                bool heroesIsDefeated = MakeTurn(enemies, myHeroes, false);

                if (enemiesIsDefeated)
                {
                    StoryTelling.HappyFinal();
                }
                if (heroesIsDefeated)
                {
                    StoryTelling.GameOver(myHeroes.Count > 1);
                }
                Console.SetCursorPosition(oldCursorPosition.Left, oldCursorPosition.Top);
                allDiedInTeam = enemiesIsDefeated || heroesIsDefeated;
                //Console.Clear();
                StoryDrawer.DrawTeam(myHeroes, true);
                StoryDrawer.DrawTeam(enemies, false);
                StoryTelling.PressEnterToContinue(20, 13);
                Console.Clear();
            }
        }

        private static void UserInputTeams(out List<Hero> myHeroes, out List<Hero> enemies)
        {
            enemies= new List<Hero>();
            myHeroes= new List<Hero>();
            bool endInput = false;
            bool isEnemy = false;
            string nextInputLine;
            Console.WriteLine("\r\n   Введите героев и противников в формате:" +
                            "\r\n   hero" +
                            "\r\n   <class> <power> <agility> <vitality> <intellect> <name>" +
                            "\r\n   enemy" +
                            "\r\n   <class> <power> <agility> <vitality> <intellect> <name>" +
                            "\r\n   end");
            Console.WriteLine("\r\n   Количество героев и противников 1-7. Доступные классы: knight, thief, mage");
            while (!endInput)
            {
                nextInputLine = Console.ReadLine();
                switch (nextInputLine)
                {
                    case "hero": isEnemy = false; break;
                    case "enemy": isEnemy = true; break;
                    case "end": endInput = true; break;
                    default: if (isEnemy) enemies.Add(GetNewHero(nextInputLine)); else myHeroes.Add(GetNewHero(nextInputLine)); break;
                }
            }            
        }
        private static void GenerateTeams(out List<Hero> myHeroes, out List<Hero> enemies)
        {
            var rnd = new Random();
            enemies = new List<Hero>();
            myHeroes = new List<Hero>();
            bool isEnemy = false;
            Type hero = typeof(Hero); // Базовый тип
            var listOfHeroClasses = Assembly.GetAssembly(hero).GetTypes().Where(type => type.IsSubclassOf(hero)).ToList();  // using System.Linq
            FillTeamWithHeroes(out myHeroes, rnd, listOfHeroClasses);
            FillTeamWithHeroes(out enemies, rnd, listOfHeroClasses);
        }

        private static void FillTeamWithHeroes(out List<Hero> heroes, Random rnd, List<Type> listOfHeroClasses)
        {
            int heroesCount = rnd.Next(1, 8);
            heroes = new List<Hero>();
            int numberOfHeroClasses = listOfHeroClasses.Count();
            for (int i = 0; i < heroesCount; i++)
            {
                var heroClass = listOfHeroClasses[rnd.Next(numberOfHeroClasses)].Name.ToLower();
                string heroName;
                using (IniFile imageFile = new IniFile("names.ini"))
                {
                    int variantsCount;
                    if (imageFile.KeyExists("num", heroClass))
                    {
                        variantsCount = int.Parse(imageFile.Read(heroClass, "num"));
                    }
                    else
                    {
                        variantsCount = 0;
                    }
                    var key = $"{rnd.Next(variantsCount)}";
                    if (imageFile.KeyExists(key, heroClass))
                    {
                        heroName = imageFile.Read(heroClass, key);
                    }
                    else
                    {
                        heroName = "player unknown";
                    }
                }
                heroes.Add(GetNewHero($"{heroClass} {rnd.Next(1, 10)} {rnd.Next(1, 10)} {rnd.Next(1, 10)} {rnd.Next(1, 10)} {heroName}"));
            }
        }

        public static bool MakeTurn (List<Hero> attackers, List<Hero> defenders, bool isMyHeroes)
        {
            var attackersCount = attackers.Count;
            for (int i = 0; i < attackersCount; i++)
            {
                if (attackers[i].Health == 0) continue;
                if (attackers[i] is Mage)
                {
                    if (attackers[i].Mana >= attackers[i].Spell.ManaCost)
                    {
                        var spellDamage = attackers[i].CastSpell(defenders, attackers[i].Spell, isMyHeroes);
                        StoryDrawer.DrawSpell(attackers[i], i, isMyHeroes, spellDamage);
                        continue;
                    }
                }
                var target = FindMinHealth(defenders);
                if (target.Health != 0)
                {
                    var (hitpoints, damage) = attackers[i].Attack(target);
                    StoryDrawer.DrawAttack(attackers[i],i,isMyHeroes, hitpoints);
                    var j = defenders.IndexOf(target);
                    StoryDrawer.DrawGotHit(target, j, !isMyHeroes, damage);
                    if (target.Health == 0) target.Defeated();
                }
            }
            return IsDefeated(defenders);
        }
        public static bool IsDefeated(List<Hero> heroes)
        {
            foreach(var hero in heroes)
            {
                if (hero.Health > 0) return false;
            }
            return true;
        }
        
        public static Hero GetNewHero(string InputLine) 
        { 
            var parameters = InputLine.Split(' ');
            var name = new StringBuilder();
            name.Append(Char.ToUpper(parameters[0][0]) + parameters[0].Substring(1) +' ');
            name.AppendJoin(' ', parameters.Skip(5));
            switch (parameters[0])
            {
                case "knight": return new Knight(name.ToString(), int.Parse(parameters[1]), int.Parse(parameters[2]), 
                    int.Parse(parameters[3]), int.Parse(parameters[4]));
                case "thief":
                    return new Thief(name.ToString(), int.Parse(parameters[1]), int.Parse(parameters[2]),
                    int.Parse(parameters[3]), int.Parse(parameters[4]));
                case "mage":
                    return new Mage (name.ToString(), int.Parse(parameters[1]), int.Parse(parameters[2]),
                    int.Parse(parameters[3]), int.Parse(parameters[4]));
                default: return new Hero("SuperHero", 0, 0, 0, 0);
            }
        }

        public static Hero FindMinHealth(List<Hero> heroesList)
        {
            var heroWithMinHealth = heroesList[0];
            foreach (var hero in heroesList)
            {
                if ((hero.Health < heroWithMinHealth.Health && hero.Health > 0) || heroWithMinHealth.Health == 0) heroWithMinHealth = hero;
            }
            return heroWithMinHealth;
        }
    }
}