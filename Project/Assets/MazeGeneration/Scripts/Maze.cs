using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityTools;

namespace MazeGeneration
{
    public class Maze
    {
        #region Variables
        #region Fields
        private GridCell[,] m_grid;
        private Vector2Int m_size;

        private List<DeadEndCell> m_deadEndsList;
        private List<Vector2Int> m_modifiedPoints;
        #endregion

        #region Properties
        public GridCell[,] Grid => m_grid;
        public Vector2Int Size => m_size;
        #endregion
        #endregion

        #region Constructors
        public Maze(Vector2Int aSize)
        {
            m_deadEndsList = new List<DeadEndCell>();
            m_modifiedPoints = new List<Vector2Int>();

            m_size = aSize;
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
                            Grid[x, y] = new EdgeCornerCell(x, y);
                        }
                        else
                        {
                            Grid[x, y] = new EdgeWallCell(x, y);
                        }
                    }
                    else if (x % 2 == 1
                        && y % 2 == 1)
                    {
                        Grid[x, y] = new PathCell(x, y);
                    }
                    else if (x % 2 == 0
                        && y % 2 == 0)
                    {
                        Grid[x, y] = new CornerCell(x, y);
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
        public void CarveOutWall(Vector2Int aCoord)
        {
            Grid[aCoord.x, aCoord.y] = new NullCell(aCoord);
        }

        public Vector2Int[] MarkDeadEnds()
        {
            m_modifiedPoints.Clear();

            int maxX = Grid.GetLength(0);
            int maxY = Grid.GetLength(1);

            for (int x = 0; x < maxX; ++x)
            {
                for (int y = 0; y < maxY; ++y)
                {
                    PathCell cell = Grid[x, y] as PathCell;

                    if (cell == null) continue;

                    int validPaths = 4;

                    foreach (var direction in new Vector2Int[] { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left })
                    {
                        int cX, cY;
                        cX = x + direction.x;
                        cY = y + direction.y;

                        if (0 <= cX && cX < maxX &&
                            0 <= cY && cY < maxY)
                        {
                            WallCell wallCheck = Grid[cX, cY] as WallCell;
                            EdgeWallCell edgeCheck = Grid[cX, cY] as EdgeWallCell;

                            if (wallCheck != null || edgeCheck != null)
                            {
                                --validPaths;
                            }
                        }
                    }

                    if (validPaths == 1)
                    {
                        m_modifiedPoints.Add(new Vector2Int(x, y));
                        Grid[x, y] = new DeadEndCell(x, y);
                        m_deadEndsList.Add(Grid[x, y] as DeadEndCell);
                    }
                }
            }

            return m_modifiedPoints.ToArray();
        }

        public Vector2Int[] CreateStartAndEnd()
        {
            m_modifiedPoints.Clear();

            // find 'corridor ends'
            // path cells that have only one un-blocked connection
            int index = MOARandom.Instance.GetRange(0, m_deadEndsList.Count - 1);

            GridCell startCell = m_deadEndsList[index];
            m_deadEndsList.RemoveAt(index);
            startCell = Grid[startCell.Coord.x, startCell.Coord.y] = new StartCell(startCell.Coord);
            m_deadEndsList.Add(startCell as StartCell);

            index = MOARandom.Instance.GetRange(0, m_deadEndsList.Count - 1);
            GridCell endCell = m_deadEndsList[index];
            m_deadEndsList.RemoveAt(index);
            endCell = Grid[endCell.Coord.x, endCell.Coord.y] = new EndCell(endCell.Coord);
            m_deadEndsList.Add(endCell as EndCell);

            return m_modifiedPoints.ToArray();
        }
        #endregion
    }
}
