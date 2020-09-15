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
        #region Variables
        #region Fields
        private GridCell[,] m_grid;
        private bool m_uniformCells;
        #endregion

        #region Properties
        public GridCell[,] Grid => m_grid;
        public bool UniformCells => m_uniformCells;
        #endregion
        #endregion

        #region Constructors
        public Maze(Vector2Int aSize, bool aUniformCells)
        {
            m_uniformCells = aUniformCells;

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
                        if (x % 2 == 0 && y % 2 == 0)
                        {
                            m_grid[x, y] = new EdgeCornerCell(x, y);
                        }
                        else
                        {
                            m_grid[x, y] = new EdgeWallCell(x, y);
                        }
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
        #endregion

        #region Mutators
        public void ReplaceWall(Vector2Int aCoord)
        {
            if (UniformCells)
            {
                m_grid[aCoord.x, aCoord.y] = new PathCell(aCoord);
            }
            else
            {
                m_grid[aCoord.x, aCoord.y] = new NullCell(aCoord);
            }
        }

        public void CreateStartAndEnd()
        {
            throw new NotImplementedException();

            // find 'corridor ends'
            // path cells that have only one un-blocked connection
        }

        private GridCell[] GetConnectedCells(Vector2Int aCoord)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
