using System;
using System.Collections.Generic;
using System.Text;

namespace SnakeKISS.SnakeGame
{
    class Game
    {

        public static (int,int) DIRECTION_UP = (0,-1);
        public static (int,int) DIRECTION_LEFT = (-1,0);
        public static (int, int) DIRECTION_DOWN = (0,1);
        public static (int, int) DIRECTION_RIGTH = (1,0);

        (int, int) currentDirection = DIRECTION_UP;
        (int, int) nextDirection = DIRECTION_UP;
        private int startLength;

        /// <summary>
        /// every value greater than zero is the snake. The value of the snake part describes the age, if age is greater than the lenght, the body part dies)
        /// 
        /// value -1 is the fruit
        /// </summary>
        int[,] gameField;

        Random r;

        int age;

        bool dead;
        /// <summary>
        /// possion of the head inside the gameField, only for performance reasons (no scan of array needed)
        /// </summary>
        (int, int) headPos;

        public Game(int width, int height, int startLength)
        {
            this.startLength = startLength;
            this.gameField = new int[width, height];
            this.age = startLength;
            dead = false;
            r = new Random();
            PlaceSnakeRandom();
            PlaceFruitRandom();
        }

        public (int,int) NextDirection { 
            get => nextDirection;
            set
            {
                if (currentDirection.Item1 + value.Item1 != 0 && currentDirection.Item2 + value.Item2 != 0)
                {
                    nextDirection = value;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reason">true if the snake went out of bounds, false if the snake hit its own part</param>
        private void GameOver(bool reason)
        {
            dead = true;
        }

        /// <summary>
        /// function which is called every game loop
        /// </summary>
        public void GameLoop()
        {
            if (! this.IsDead)
            {
                this.MakeSnakeAge();
                this.MoveSnake();
            }
                
        }

        public bool IsDead
        {
            get{ return dead; }
        }

        public int Score { get { return age - startLength; } }

        private void MoveSnake()
        {
            this.currentDirection = NextDirection;
            this.MoveSnakeInDirection(this.currentDirection);
        }

        private void MoveSnakeInDirection((int,int) direction)
        {
            headPos.Item1 += direction.Item1;
            headPos.Item2 += direction.Item2;

            try
            {
                var prevValue = gameField[headPos.Item1, headPos.Item2];
                if(prevValue != 0)
                {
                    if(prevValue == -1)
                    {
                        // did eat fruit
                        this.age++;
                        PlaceFruitRandom();
                    }
                    else
                    {
                        GameOver(false);
                        return;
                    }
                }
                gameField[headPos.Item1, headPos.Item2] = 1;
            }catch(IndexOutOfRangeException)
            {
                // the snake walked out of the gameField

                GameOver(true);
            }
        }


        private void PlaceSnakeRandom()
        {
            headPos = GetRandomCoordinate();
            
            gameField[headPos.Item1, headPos.Item2] = 1;
        }


        private void PlaceFruitRandom()
        {
            (int,int) coord;
            do
            {
                coord = GetRandomCoordinate();
            } while (gameField[coord.Item1, coord.Item2] != 0);

            gameField[coord.Item1, coord.Item2] = -1;
        }

        /// <summary>
        /// craetes a random coordinate inside the gameField dimensions (doesn't check the value of the gameField at the returned Coordinate)
        /// </summary>
        /// <returns>a random coordinate inside the gameField dimensions</returns>
        private (int,int) GetRandomCoordinate()
        {
            return (r.Next(gameField.GetLength(0)),r.Next(gameField.GetLength(1)));
        }

        /// <summary>
        /// does increment the age of each body part inside the gameField array
        /// 
        /// could be made more efficient if the coordinates of the snake are saved in a list or the snake is searched by the position of the head
        /// </summary>
        private void MakeSnakeAge()
        {
            for (int x = 0; x < gameField.GetLength(0); x++)
            {
                for (int y = 0; y < gameField.GetLength(1); y++)
                {
                    var item = gameField[x, y];
                    if(item > 0)
                    {
                        if (item >= age)
                        {
                            gameField[x, y] = 0;
                        }
                        else
                        {
                            gameField[x, y] += 1;
                        }
                        
                    }   
                }
            }
        }

        public void PrintGameState()
        {
            for (int y = 0; y < gameField.GetLength(1); y++)
            {
                String line = "";
                for (int x = 0; x < gameField.GetLength(0); x++)
                {
                    line += gameField[x, y];
                    line += " ";
                }
                Console.WriteLine(line);
            }
        }

        private char GetCharRep(int value)
        {
            var charRep = '.';
            switch (value)
            {
                case int n when (n > 0):
                    charRep = '#';
                    break;
                case -1:
                    charRep = '@';
                    break;
            }

            return charRep;
        }

        public char[,] ToCharArray()
        {
            // multiply width by 2 to place a space between columns
            char[,] result = new char[gameField.GetLength(0) * 2, gameField.GetLength(1)];
            for (int x = 0; x < gameField.GetLength(0); x++)
            {
                for (int y = 0; y < gameField.GetLength(0); y++)
                {
                    var current = gameField[x, y];
                    

                    result[x * 2, y] = GetCharRep(current);
                }
            }
            return result;
        }

        public override String ToString()
        {
            string result = "";
            for (int y = 0; y < gameField.GetLength(1); y++)
            {
                for (int x = 0; x < gameField.GetLength(0); x++)
                {
                    var current = gameField[x, y];


                    result += GetCharRep(current)+" ";
                }
            }
            return result;
        }

        public string GetDeathString()
        {
            return "Du bist gestorben\n\nScore:" + Score;
        }
    }
}
