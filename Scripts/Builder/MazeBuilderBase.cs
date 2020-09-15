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
        private Transform m_buildParent;
        #endregion

        #region Properties
        public Transform[,] SpawnedGrid
        {
            get => m_spawnedGrid;
            protected set => m_spawnedGrid = value;
        }

        protected Transform BuildParent => m_buildParent ?? (m_buildParent = new GameObject("Build Parent").transform);
        #endregion
        #endregion

        #region Mono
        protected virtual void Reset()
        {
            m_mazeGenerator = GetComponent<MazeGenerator>();
            m_mazeGenerator?.RegisterBuilder(this);
        }

        protected void Awake()
        {
            BuildParent.SetParent(transform);
        }
        #endregion

        #region MazeBuilder
        public void BuildMaze(Maze aMaze)
        {
            InitialiseBuildParent(aMaze);

            ClearConstruction();

            SpawnedGrid = new Transform[aMaze.Grid.GetLength(0), aMaze.Grid.GetLength(1)];

            for (int x = 0; x < aMaze.Grid.GetLength(0); ++x)
            {
                for (int y = 0; y < aMaze.Grid.GetLength(1); ++y)
                {
                    GameObject prefab = GetPrefab(aMaze.Grid[x, y]);

                    if (prefab)
                    {
                        SpawnPiece(prefab, aMaze, x, y);
                    }
                }
            }
        }

        public void UpdateConstruction(Maze aMaze, Vector2Int aUpdatedCell)
        {
            Transform currentPoint = SpawnedGrid[aUpdatedCell.x, aUpdatedCell.y];

            if (currentPoint)
            {
                Destroy(currentPoint.gameObject);
            }

            GameObject prefab = GetPrefab(aMaze.Grid[aUpdatedCell.x, aUpdatedCell.y]);

            if (prefab)
            {
                SpawnPiece(prefab, aMaze, aUpdatedCell.x, aUpdatedCell.y);
            }
        }

        protected abstract void InitialiseBuildParent(Maze aMaze);
        protected abstract GameObject GetPrefab(GridCell aCell);
        protected abstract void GetSpawnPositionAndRotation(Maze aMaze, Vector2Int aCoord, out Vector3 outPosition, out Quaternion outRotation);

        public void ClearConstruction()
        {
            if (SpawnedGrid == null) return;

            for (int x = 0; x < SpawnedGrid.GetLength(0); ++x)
            {
                for (int y = 0; y < SpawnedGrid.GetLength(1); ++y)
                {
                    if (SpawnedGrid[x, y])
                    {
                        Destroy(SpawnedGrid[x, y].gameObject);
                    }
                }
            }

            SpawnedGrid = null;
        }

        private void SpawnPiece(GameObject aPiecePrefab, Maze aMaze, int aX, int aY)
        {
            GetSpawnPositionAndRotation(aMaze, new Vector2Int(aX, aY), out Vector3 spawnPosition, out Quaternion spawnRotation);

            var newTransform = Instantiate(
                aPiecePrefab.transform,
                BuildParent
            );
            newTransform.localPosition = spawnPosition;
            newTransform.localRotation = spawnRotation;
            newTransform.name = $"[{aX},{aY}] {aPiecePrefab.name}";

            SpawnedGrid[aX, aY] = newTransform;
        }
        #endregion
    }
}