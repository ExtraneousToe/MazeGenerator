using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MazeGeneration
{
    public class Maze
    {
        private GridCell[,] m_grid;

        public GridCell[,] Grid
        {
            get => m_grid;
        }

        public Maze(Vector2Int aSize)
        {
            int width = aSize.x * 2 + 1;
            int height = aSize.y * 2 + 1;

            m_grid = new GridCell[width, height];

            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    if (x == 0
                        || x == width - 1
                        || y == 0
                        || y == height - 1)
                    {
                        m_grid[x, y] = new EdgeCell(x, y);
                    }
                    else if (x % 2 == 1
                        && y % 2 == 1)
                    {
                        m_grid[x, y] = new PathCell(x, y);
                    }
                    else if (x % 2 == 0
                        && y % 2 == 0)
                    {
                        m_grid[x, y] = new CornerCell(x, y);
                    }
                    else
                    {
                        m_grid[x, y] = new WallCell(x, y);
                    }
                }
            }
        }
    }
}
