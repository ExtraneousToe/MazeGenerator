using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTools;

namespace MazeGeneration.Paths
{
    [CreateAssetMenu(menuName = "Maze/Path/Krustal", fileName = "Krustal")]
    public class RandomKruskalPath : PathAlgorithm
    {
        private Dictionary<GridCell, HashSet<GridCell>> Sets { get; set; }
        private List<KeyValuePair<GridCell, GridCell>> Pairs { get; set; }

        public override IEnumerator GeneratePath(Maze aMaze, Vector2Int aSize)
        {
            int maxX, maxY;
            maxX = aMaze.Grid.GetLength(0);
            maxY = aMaze.Grid.GetLength(1);

            Sets = new Dictionary<GridCell, HashSet<GridCell>>();
            Pairs = new List<KeyValuePair<GridCell, GridCell>>();

            GridCell cellA, cellB, cellC;
            HashSet<GridCell> setA, setB, setC;

            for (int x = 0; x < aSize.x; ++x)
            {
                for (int y = 0; y < aSize.y; ++y)
                {
                    int aX, aY, bX, cY;

                    aX = 1 + x * 2;
                    aY = 1 + y * 2;
                    bX = 1 + (x + 1) * 2;
                    cY = 1 + (y + 1) * 2;

                    cellA = aMaze.Grid[
                       aX,
                       aY
                    ];

                    if (!Sets.ContainsKey(cellA))
                    {
                        setA = new HashSet<GridCell>
                        {
                            cellA
                        };
                        Sets.Add(cellA, setA);
                    }

                    if (bX < maxX)
                    {
                        cellB = aMaze.Grid[
                            bX,
                            aY
                        ];

                        Pairs.Add(new KeyValuePair<GridCell, GridCell>(cellA, cellB));
                    }

                    if (cY < maxY)
                    {
                        cellC = aMaze.Grid[
                            aX,
                            cY
                        ];

                        Pairs.Add(new KeyValuePair<GridCell, GridCell>(cellA, cellC));
                    }
                }
            }

            int totalSets = Sets.Count;

            do
            {
                // get a random pair
                int randomIndex = MOARandom.Instance.GetRange(0, Pairs.Count - 1);
                KeyValuePair<GridCell, GridCell> pair = Pairs[randomIndex];
                Pairs.RemoveAt(randomIndex);

                cellA = pair.Key;
                setA = Sets[cellA];
                cellB = pair.Value;
                setB = Sets[cellB];

                // if the elements aren't present in the opposing pairs
                if (!setA.Contains(cellB) && !setB.Contains(cellA))
                {
                    // combine them, removing the wall element
                    foreach (GridCell cell in setB)
                    {
                        setA.Add(cell);
                        Sets[cell] = setA;
                    }

                    // remove the wall
                    Vector2Int wallIndex = cellB.Coord - cellA.Coord;
                    wallIndex /= 2;
                    wallIndex = cellA.Coord + wallIndex;

                    aMaze.CarveOutWall(wallIndex);

                    --totalSets;

                    CellChanged(wallIndex);
                }

                if (RoutineDelay > 0)
                {
                    yield return new WaitForSeconds(RoutineDelay);
                }
            }
            while (totalSets > 1 && Pairs.Count > 0);
        }
    }
}
