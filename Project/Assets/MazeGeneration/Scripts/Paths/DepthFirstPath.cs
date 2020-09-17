using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTools;

namespace MazeGeneration.Paths
{
    [CreateAssetMenu(menuName = "Maze/Path/DFS", fileName = "DFS")]
    public class DepthFirstPath : PathAlgorithm
    {
        #region Variables
        #region Fields
        private GridCell m_currentCell;
        private int m_maxX;
        private int m_maxY;
        #endregion

        #region Properties
        private Stack<KeyValuePair<GridCell, GridCell>> FrontierStack { get; set; }
        private List<GridCell> UnvisitedCells { get; set; }
        private List<GridCell> VisitedCells { get; set; }

        private List<Vector2Int> DirectionsList = new List<Vector2Int>(new Vector2Int[]
        {
            Vector2Int.up,
            Vector2Int.right,
            Vector2Int.down,
            Vector2Int.left
        });

        protected override bool ShouldContinue => UnvisitedCells != null && UnvisitedCells.Count > 0;
        #endregion
        #endregion

        #region PathAlgorithm
        protected override void InitialiseAlgorithm(Maze aMaze, Vector2Int aSize)
        {
            m_maxX = aMaze.Grid.GetLength(0);
            m_maxY = aMaze.Grid.GetLength(1);

            int fX, fY;

            fX = fY = 1;
            fX += MOARandom.Instance.GetRange(0, aSize.x - 1) * 2;
            fY += MOARandom.Instance.GetRange(0, aSize.y - 1) * 2;

            UnvisitedCells = new List<GridCell>();
            VisitedCells = new List<GridCell>();

            for (int x = 0; x < aSize.x; ++x)
            {
                for (int y = 0; y < aSize.y; ++y)
                {
                    UnvisitedCells.Add(
                         aMaze.Grid[
                            1 + x * 2,
                            1 + y * 2
                        ]
                    );
                }
            }

            m_currentCell = aMaze.Grid[fX, fY];

            FrontierStack = new Stack<KeyValuePair<GridCell, GridCell>>();
        }

        protected override void StepAlgorithm(Maze aMaze, Vector2Int aSize)
        {
            // if not yet visited
            if (UnvisitedCells.Contains(m_currentCell))
            {
                // shuffle the directions and try stacking the connecting cells
                DirectionsList = new List<Vector2Int>(DirectionsList.Shuffle());
                foreach (Vector2Int direction in DirectionsList)
                {
                    int pX, pY;
                    pX = m_currentCell.Coord.x + direction.x * 2;
                    pY = m_currentCell.Coord.y + direction.y * 2;

                    if (pX < 0 || m_maxX <= pX ||
                        pY < 0 || m_maxY <= pY)
                    {
                        // is out of range
                        continue;
                    }

                    GridCell possibleCell = aMaze.Grid[pX, pY];

                    if (UnvisitedCells.Contains(possibleCell))
                    {
                        FrontierStack.Push(
                            new KeyValuePair<GridCell, GridCell>(
                            possibleCell, // to
                            m_currentCell // from
                            )
                        );
                    }
                }

                UnvisitedCells.Remove(m_currentCell);
                VisitedCells.Add(m_currentCell);
            }
            else
            {
                // has been visited, do nothing?
            }

            if (FrontierStack.Count > 0)
            {
                // select the next cell in the frontier stack
                KeyValuePair<GridCell, GridCell> nextCellPair = FrontierStack.Pop();
                GridCell nextCell = nextCellPair.Key;

                // some cells might've been connected previously
                if (!VisitedCells.Contains(nextCell))
                {
                    GridCell fromCell = nextCellPair.Value;

                    // carve away the wall between the cells
                    Vector2Int betweenCoord = nextCell.Coord - fromCell.Coord;
                    betweenCoord /= 2;
                    betweenCoord = fromCell.Coord + betweenCoord;

                    aMaze.CarveOutWall(betweenCoord);

                    // select the new cell
                    m_currentCell = nextCell;

                    if (RoutineDelay > 0)
                    {
                        CellChanged(betweenCoord);
                    }
                }
            }
            else
            {
                UnityTools.Logger.Log("DFS", $"Broke from empty Frontier.\n" +
                    $"UnvisitedCells.Count: {UnvisitedCells.Count}\n" +
                    $"VisitedCells.Count: {VisitedCells.Count}");
            }

        }
        #endregion
    }
}
