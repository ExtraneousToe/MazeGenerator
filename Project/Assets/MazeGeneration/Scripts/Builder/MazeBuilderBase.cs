using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MazeGeneration.Building
{
    [RequireComponent(typeof(MazeGenerator))]
    public abstract class MazeBuilderBase : MonoBehaviour
    {
        #region Variables
        #region Fields
        private MazeGenerator m_mazeGenerator;
        private Transform[,] m_spawnedGrid;
        #endregion

        #region Properties
        public Transform[,] SpawnedGrid
        {
            get => m_spawnedGrid;
            protected set => m_spawnedGrid = value;
        }
        #endregion
        #endregion

        #region Mono
        protected virtual void Reset()
        {
            m_mazeGenerator = GetComponent<MazeGenerator>();
            m_mazeGenerator?.RegisterBuilder(this);
        }
        #endregion

        #region MazeBuilder
        public abstract void BuildMaze(Maze aMaze);
        public abstract void UpdateConstruction(Maze aMaze, Vector2Int aUpdatedCell);

        protected abstract GameObject GetPrefab(GridCell aCell);

        public void ClearConstruction()
        {
            if (SpawnedGrid == null) return;

            for (int x = 0; x < SpawnedGrid.GetLength(0); ++x)
            {
                for (int y = 0; y < SpawnedGrid.GetLength(1); ++y)
                {
                    Destroy(SpawnedGrid[x, y].gameObject);
                }
            }

            SpawnedGrid = null;
        }
        #endregion
    }
}