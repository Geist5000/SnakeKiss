using SnakeKISS.SnakeGame;
using System;
using System.Threading;

namespace SnakeKISS
{
    class Program
    {
        static void Main(string[] args)
        {
            int gameWidth = 30;
            int gameHeight = 30;
            Game snakeGame = new Game(gameWidth,gameHeight, 3);
            Console.Title = "Snake";
            Console.CursorVisible = false;


            Console.WindowWidth = gameWidth*2;
            Console.WindowHeight = gameHeight;
            Console.SetBufferSize(gameWidth*2, gameHeight);

            while (!snakeGame.IsDead)
            {
                

                
                ConsoleKeyInfo key = new ConsoleKeyInfo();
                // flush stream to prevent issues while holding a key down
                while (Console.KeyAvailable)
                {
                    key = Console.ReadKey(true);
                }
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.W:
                        snakeGame.NextDirection = Game.DIRECTION_UP;
                        break;
                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.A:
                        snakeGame.NextDirection = Game.DIRECTION_LEFT;
                        break;
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.S:
                        snakeGame.NextDirection = Game.DIRECTION_DOWN;
                        break;
                    case ConsoleKey.RightArrow:
                    case ConsoleKey.D:
                        snakeGame.NextDirection = Game.DIRECTION_RIGHT;
                        break;
                }
            
                
                snakeGame.GameLoop();
                
                if (snakeGame.IsDead)
                {
                    Console.Write(snakeGame.GetDeathString());
                }
                else
                {
                    Console.SetCursorPosition(0, 0);
                    Console.Write(snakeGame.ToString());
                    Thread.Sleep(150);
                }
            }

            Console.Write("\nDrücke irgendeine Taste zum schließen...");
            Console.ReadKey(false);
        }
    }
}
