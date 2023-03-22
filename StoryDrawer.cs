using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using static DnD_NoobCoders.Hero;

namespace DnD_NoobCoders
{
    internal class StoryDrawer
    {
        private const int FrameDelay = 150;
        private const int TypingDelay = 3;
        private const int LeftSpacer = 5;
        internal const int LinesInImage = 10;
        private const int ImageWidth = 33;
        internal const int HeroesAtLine = 18;
        private const int EnemiesAtLine = 2;


        internal static void ShiftCursor(int x)
        {
            Console.SetCursorPosition(x, Console.GetCursorPosition().Top);
        }

        internal static void DrawFromFile(int positionX, String fileName, string section, bool alpha, int lines)
        {
            using (IniFile imageFile = new IniFile(fileName))
            {
                for (int i = 0; i < lines; i++)
                {
                    ShiftCursor(positionX);
                    var key = $"line{i}";
                    if (imageFile.KeyExists(key, section))
                    {
                        var line = imageFile.Read(section, key);
                        if(alpha)
                        {
                            foreach (char c in line)
                            {
                                if (c != ' ') Console.Write(c);
                                else Console.SetCursorPosition(Console.GetCursorPosition().Left + 1, Console.GetCursorPosition().Top);
                            }
                            Console.WriteLine();
                        }
                        else
                        {
                            Console.WriteLine($"{line}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("image not found");
                    }
                }
            }
        }

        internal static void DrawHero((int x, int y) position, Hero hero, Hero.Action action)
        {
            var previousPosition = SaveAndMoveCursorTo(position.x, position.y);
            string imageFileName = hero.GetType().Name.ToLower() + ".ini";
            DrawFromFile(position.x, imageFileName, action.ToString(), false, LinesInImage);
            ShiftCursor(position.x);
            Console.SetCursorPosition(previousPosition.Left, previousPosition.Top);
        }

        internal static void DrawSpellAttack((int x, int y) position, Hero hero) // ,Spell spell)
        {
            var previousPosition = SaveAndMoveCursorTo(position.x, position.y);
            var oldTextColor = SaveOldAndSetNewTextColor(ConsoleColor.White);
            string imageFileName = "spell.ini";
            DrawFromFile(position.x, imageFileName, "ChainLightning", true, LinesInImage);
            ShiftCursor(position.x);
            Console.ForegroundColor = oldTextColor;
            Console.SetCursorPosition(previousPosition.Left, previousPosition.Top);
        }

        internal static void Defeated((int x, int y) position, Hero hero)
        {
            var previousPosition = SaveAndMoveCursorTo(position.x, position.y);
            var oldTextColor = SaveOldAndSetNewTextColor(ConsoleColor.DarkGray);
            string imageFileName = "defeated.ini";
            DrawFromFile(position.x, imageFileName, "Tomb", false, LinesInImage);
            ShiftCursor(position.x);
            Console.Write($"{hero.Name}");
            Console.SetCursorPosition(previousPosition.Left, previousPosition.Top);
            Console.ForegroundColor = oldTextColor;
        }

        internal static (int Left, int Top) SaveAndMoveCursorTo(int left, int top)
        {
            (int oldLeft, int oldTop) = Console.GetCursorPosition();
            Console.SetCursorPosition(left, top);
            return (oldLeft, oldTop);
        }
        internal static ConsoleColor SaveOldAndSetNewTextColor(ConsoleColor newColor)
        {
            var oldTextColor = Console.ForegroundColor;
            Console.ForegroundColor = newColor;
            return oldTextColor;
        }


        internal static void DrawTeam(List<Hero> heroes, bool isMyHeroes)
        {
            int positionX = LeftSpacer;
            int positionY = isMyHeroes ? HeroesAtLine: EnemiesAtLine;
            var oldTextColor = SaveOldAndSetNewTextColor(isMyHeroes ? ConsoleColor.Blue : ConsoleColor.DarkYellow);
            foreach (var hero in heroes)
            {
                if (hero.Health == 0) Defeated((positionX, positionY), hero);
                else 
                {
                    DrawHero((positionX, positionY), hero, Hero.Action.Stay);
                    Console.SetCursorPosition(positionX, positionY+LinesInImage);
                    Console.Write($"{hero.Name} ({hero.Health})");
                }
                positionX+= ImageWidth;
            }
            Console.ForegroundColor = oldTextColor;
        }

        internal static void DrawAttack(Hero hero, int index, bool isMyHeroes, int hitPoints)
        {
            int positionX = LeftSpacer + ImageWidth * index;
            int positionY = isMyHeroes ? HeroesAtLine : EnemiesAtLine;
            var previousPosition = SaveAndMoveCursorTo(positionX, positionY - 2);
            Console.WriteLine($"Hit: {hitPoints}");
            Console.SetCursorPosition(previousPosition.Left, previousPosition.Top);
            var oldTextColor = SaveOldAndSetNewTextColor(isMyHeroes ? ConsoleColor.Green: ConsoleColor.Red);
            for (int i = 0; i < 3; i++)
            {
                DrawHero((positionX, positionY), hero, Hero.Action.Attack);
                Thread.Sleep(FrameDelay);
                DrawHero((positionX, positionY), hero, Hero.Action.Stay);
                if (i < 2) Thread.Sleep(FrameDelay);
            }
            Console.ForegroundColor = isMyHeroes ? ConsoleColor.Blue : ConsoleColor.DarkYellow;
            DrawHero((positionX, positionY), hero, Hero.Action.Stay);
            Console.ForegroundColor = oldTextColor;
        }

        internal static void DrawSpell(Hero hero, int index, bool isMyHeroes, int hitPoints)
        {
            int positionX = LeftSpacer + ImageWidth * index;
            int positionY = isMyHeroes ? HeroesAtLine : EnemiesAtLine;
            var previousPosition = SaveAndMoveCursorTo(positionX, positionY - 2);
            Console.WriteLine($"Hit: {hitPoints}");
            Console.SetCursorPosition(previousPosition.Left, previousPosition.Top);
            var oldTextColor = Console.ForegroundColor;
            Console.ForegroundColor = isMyHeroes ? ConsoleColor.Green : ConsoleColor.Red;
            for (int i = 0; i < 3; i++)
            {
                DrawSpellAttack((positionX, positionY), hero);
                Thread.Sleep(FrameDelay);
                DrawHero((positionX, positionY), hero, Hero.Action.Stay);
                if (i < 2) Thread.Sleep(FrameDelay);
            }
            Console.ForegroundColor = isMyHeroes ? ConsoleColor.Blue : ConsoleColor.DarkYellow;
            DrawHero((positionX, positionY), hero, Hero.Action.Stay);
            Console.ForegroundColor = oldTextColor;
        }

        internal static void DrawGotHit(Hero hero, int index, bool isMyHeroes, int hitPoints)
        {
            int positionX = LeftSpacer + ImageWidth * index;
            int positionY = isMyHeroes ? HeroesAtLine : EnemiesAtLine;
            var previousPosition = Console.GetCursorPosition();
            var oldTextColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(positionX, positionY - 1);
            if (hitPoints > 0)
            {
                Console.WriteLine($"HP: -{hitPoints}");
            }
            else
            {
                Console.WriteLine("Hit blocked");
            }
            Console.SetCursorPosition(previousPosition.Left, previousPosition.Top);
            Console.ForegroundColor = oldTextColor;
        }

        internal static void TextTyping(String text,(int x, int y) position)
        {
            var previousPosition = SaveAndMoveCursorTo(position.x, position.y);
            foreach (char c in text) 
            {
                Console.Write(c);
                Thread.Sleep(TypingDelay);
            }
            Console.SetCursorPosition(previousPosition.Left, previousPosition.Top);
        }


    }
}
