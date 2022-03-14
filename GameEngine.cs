using System;

namespace GameOfLife
{
    public class GameEngine
    {
        private bool[,] field;
        private readonly int cols;
        private readonly int rows;
        public uint currentGeneration { get; private set; }
        private int minNeighbours;
        private int maxNeighbours;


        public GameEngine(int cols, int rows, int density, int minNeighbours_ = 2, int maxNeighbours_ = 3)
        {
            this.cols = cols;   
            this.rows = rows;
            minNeighbours = minNeighbours_;
            maxNeighbours = maxNeighbours_;
            field = new bool[cols, rows];
            Random random = new Random();

            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    field[x, y] = random.Next((int)density) == 0;
                }
            }
        }

        public void NextGeneration()
        {
            var newField = new bool[cols, rows];

            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    var neighboursCount = CountNeighbours(x, y);
                    var hasLife = field[x, y];

                    if (!hasLife && neighboursCount <= maxNeighbours && neighboursCount > minNeighbours)
                    {
                        newField[x, y] = true;
                    }
                    else if (hasLife && (neighboursCount < minNeighbours || neighboursCount > maxNeighbours))
                    {
                        newField[x, y] = false;
                    }
                    else
                    {
                        newField[x, y] = field[x, y];
                    }
                }
            }
            field = newField;
        }

        public bool[,] GetCurrentGeneration()
        {
            var result = new bool[cols, rows];
            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    result[x, y] = field[x, y];
                }
            }
            return result;
        }

        private int CountNeighbours(int x, int y)
        {
            int count = 0;

            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    var col = (x + i + cols) % cols;
                    var row = (y + j + rows) % rows;

                    var isSelfCheking = x == col && y == row;
                    var hasLife = field[col, row];

                    if (hasLife && !isSelfCheking)
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        private bool ValidateCellPosition(int x, int y)
        {
            return (x > 0 && y > 0 && x < cols && y < rows);
        }

        private void UpdateCell(int x, int y, bool state)
        {
            if (ValidateCellPosition(x, y))
                field[x, y] = state;
        }

        public void AddCell(int x, int y)
        {
            UpdateCell(x, y, true);
        }

        public void RemoveCell(int x, int y)
        {
            UpdateCell(x, y, false);
        }
    }
}
