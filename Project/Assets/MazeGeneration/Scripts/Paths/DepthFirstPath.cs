using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTools;

namespace MazeGeneration.Paths
{
    public class SelectedCell : GridCell
    {
        public SelectedCell(Vector2Int aCoord) : base(aCoord) { }
        public SelectedCell(int aX, int aY) : this(new Vector2Int(aX, aY)) { }
        public override string ToString() => "S";
    }

    [CreateAssetMenu(menuName = "Maze/Path/DFS", fileName = "DFS")]
    public class DepthFirstPath : PathAlgorithm
    {
        private Stack<KeyValuePair<GridCell,GridCell>> FrontierStack { get; set; }
        private List<GridCell> UnvisitedCells { get; set; }
        private List<GridCell> VisitedCells { get; set; }

        private List<Vector2Int> DirectionsList = new List<Vector2Int>(new Vector2Int[]
        {
            Vector2Int.up,
            Vector2Int.right,
            Vector2Int.down,
            Vector2Int.left
        });

        public override IEnumerator GeneratePath(Maze aMaze, Vector2Int aSize)
        {
            int maxX, maxY;
            maxX = aMaze.Grid.GetLength(0);
            maxY = aMaze.Grid.GetLength(1);

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

            GridCell currentCell = aMaze.Grid[fX, fY];

            FrontierStack = new Stack<KeyValuePair<GridCell, GridCell>>();

            do
            {
                // if not yet visited
                if (UnvisitedCells.Contains(currentCell))
                {
                    // shuffle the directions and try stacking the connecting cells
                    DirectionsList = new List<Vector2Int>(DirectionsList.Shuffle());
                    foreach (Vector2Int direction in DirectionsList)
                    {
                        int pX, pY;
                        pX = currentCell.Coord.x + direction.x * 2;
                        pY = currentCell.Coord.y + direction.y * 2;

                        if (pX < 0 || maxX <= pX ||
                            pY < 0 || maxY <= pY)
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
                                currentCell // from
                                )
                            );
                        }
                    }

                    UnvisitedCells.Remove(currentCell);
                    VisitedCells.Add(currentCell);
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
                        currentCell = nextCell;

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
                    break;
                }

                if (RoutineDelay > 0)
                {
                    yield return new WaitForSeconds(RoutineDelay);
                }
            }
            while (UnvisitedCells.Count > 0);
        }
    }
}
