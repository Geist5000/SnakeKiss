using SnakeKISS.SnakeGame;
using System;

namespace SnakeKISS
{
    class Program
    {
        static void Main(string[] args)
        {
            Game snakeGame = new Game(10, 10, 3);

            while (!snakeGame.IsDead)
            {
                snakeGame.GameLoop();
                snakeGame.PrintGameState();
                string dir = Console.ReadLine();
                dir = dir.ToUpper();
                switch (dir)
                {
                    case "W":
                        snakeGame.NextDirection = Game.DIRECTION_UP;
                        break;
                    case "A":
                        snakeGame.NextDirection = Game.DIRECTION_LEFT;
                        break;
                    case "S":
                        snakeGame.NextDirection = Game.DIRECTION_DOWN;
                        break;
                    case "D":
                        snakeGame.NextDirection = Game.DIRECTION_RIGTH;
                        break;
                }
            }
        }
    }
}
