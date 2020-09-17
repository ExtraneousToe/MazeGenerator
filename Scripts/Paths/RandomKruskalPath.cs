using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTools;

namespace MazeGeneration.Paths
{
    [CreateAssetMenu(menuName = "Maze/Path/Krustal", fileName = "Krustal")]
    public class RandomKruskalPath : PathAlgorithm
    {
        #region Variables
        #region Fields
        private int m_maxX;
        private int m_maxY;
        private int m_totalSets;
        private HashSet<GridCell> m_setA, m_setB, m_setC;
        private GridCell m_cellA, m_cellB, m_cellC;
        #endregion

        #region Properties
        private Dictionary<GridCell, HashSet<GridCell>> Sets { get; set; }
        private List<KeyValuePair<GridCell, GridCell>> Pairs { get; set; }
        protected override bool ShouldContinue => m_totalSets > 1 && Pairs != null && Pairs.Count > 0;
        #endregion
        #endregion

        #region PathAlgorithm
        protected override void InitialiseAlgorithm(Maze aMaze, Vector2Int aSize)
        {
            m_maxX = aMaze.Grid.GetLength(0);
            m_maxY = aMaze.Grid.GetLength(1);

            Sets = new Dictionary<GridCell, HashSet<GridCell>>();
            Pairs = new List<KeyValuePair<GridCell, GridCell>>();


            for (int x = 0; x < aSize.x; ++x)
            {
                for (int y = 0; y < aSize.y; ++y)
                {
                    int aX, aY, bX, cY;

                    aX = 1 + x * 2;
                    aY = 1 + y * 2;
                    bX = 1 + (x + 1) * 2;
                    cY = 1 + (y + 1) * 2;

                    m_cellA = aMaze.Grid[
                       aX,
                       aY
                    ];

                    if (!Sets.ContainsKey(m_cellA))
                    {
                        m_setA = new HashSet<GridCell>
                        {
                            m_cellA
                        };
                        Sets.Add(m_cellA, m_setA);
                    }

                    if (bX < m_maxX)
                    {
                        m_cellB = aMaze.Grid[
                            bX,
                            aY
                        ];

                        Pairs.Add(new KeyValuePair<GridCell, GridCell>(m_cellA, m_cellB));
                    }

                    if (cY < m_maxY)
                    {
                        m_cellC = aMaze.Grid[
                            aX,
                            cY
                        ];

                        Pairs.Add(new KeyValuePair<GridCell, GridCell>(m_cellA, m_cellC));
                    }
                }
            }

            m_totalSets = Sets.Count;
        }

        protected override void StepAlgorithm(Maze aMaze, Vector2Int aSize)
        {
            // get a random pair
            int randomIndex = MOARandom.Instance.GetRange(0, Pairs.Count - 1);
            KeyValuePair<GridCell, GridCell> pair = Pairs[randomIndex];
            Pairs.RemoveAt(randomIndex);

            m_cellA = pair.Key;
            m_setA = Sets[m_cellA];
            m_cellB = pair.Value;
            m_setB = Sets[m_cellB];

            // if the elements aren't present in the opposing pairs
            if (!m_setA.Contains(m_cellB) && !m_setB.Contains(m_cellA))
            {
                // combine them, removing the wall element
                foreach (GridCell cell in m_setB)
                {
                    m_setA.Add(cell);
                    Sets[cell] = m_setA;
                }

                // remove the wall
                Vector2Int wallIndex = m_cellB.Coord - m_cellA.Coord;
                wallIndex /= 2;
                wallIndex = m_cellA.Coord + wallIndex;

                aMaze.CarveOutWall(wallIndex);

                --m_totalSets;

                CellChanged(wallIndex);
            }
        }
        #endregion
    }
}
