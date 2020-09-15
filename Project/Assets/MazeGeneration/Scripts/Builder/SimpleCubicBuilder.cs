using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MazeGeneration.Building
{
    public class SimpleCubicBuilder : MazeBuilderBase
    {
        #region Variables
        #region Fields
        [SerializeField] private GameObject m_wallPrefab;
        [SerializeField] private GameObject m_edgePrefab;
        [SerializeField] private GameObject m_pathPrefab;
        [SerializeField] private GameObject m_cornerPrefab;
        #endregion

        #region Properties

        #endregion
        #endregion

        #region MazeBuilderBase
        public override void BuildMaze(Maze aMaze)
        {
            ClearConstruction();

            SpawnedGrid = new Transform[aMaze.Grid.GetLength(0), aMaze.Grid.GetLength(1)];

            for (int x = 0; x < aMaze.Grid.GetLength(0); ++x)
            {
                for (int y = 0; y < aMaze.Grid.GetLength(1); ++y)
                {
                    GameObject prefab = GetPrefab(aMaze.Grid[x, y]);

                    if (prefab)
                    {
                        SpawnedGrid[x, y] = Instantiate(
                            prefab.transform,
                            new Vector3(x, 0, y),
                            Quaternion.identity,
                            transform
                        );
                    }
                }
            }
        }

        public override void UpdateConstruction(Maze aMaze, Vector2Int aUpdatedCell)
        {
            Transform currentPoint = SpawnedGrid[aUpdatedCell.x, aUpdatedCell.y];

            if (currentPoint)
            {
                Destroy(currentPoint.gameObject);
            }

            GameObject prefab = GetPrefab(aMaze.Grid[aUpdatedCell.x, aUpdatedCell.y]);

            if (prefab)
            {
                SpawnedGrid[aUpdatedCell.x, aUpdatedCell.y] = Instantiate(
                    prefab.transform,
                    new Vector3(aUpdatedCell.x, 0, aUpdatedCell.y),
                    Quaternion.identity,
                    transform
                );
            }
        }
        #endregion

        protected override GameObject GetPrefab(GridCell aCell)
        {
            if (aCell == null) return null;

            if (aCell is EdgeCell)
            {
                return m_edgePrefab;
            }
            else if (aCell is WallCell)
            {
                return m_wallPrefab;
            }
            else if (aCell is CornerCell)
            {
                return m_cornerPrefab;
            }
            else if (aCell is PathCell)
            {
                return m_pathPrefab;
            }

            return null;
        }
    }
}
