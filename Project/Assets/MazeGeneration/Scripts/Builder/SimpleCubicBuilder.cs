using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MazeGeneration.Building
{
    public class SimpleCubicBuilder : MazeBuilderBase
    {
        #region Variables
        #region Fields
        [Header("Paths")]
        [SerializeField] private GameObject m_pathPrefab;
        [SerializeField] private GameObject m_deadEndPathPrefab;
        [SerializeField] private GameObject m_startPathPrefab;
        [SerializeField] private GameObject m_endPathPrefab;

        [Header("Inner Edges")]
        [SerializeField] private GameObject m_innerWallPrefab;
        [SerializeField] private GameObject m_innerCornerPrefab;

        [Header("Outer Edges")]
        [SerializeField] private GameObject m_edgePrefab;
        #endregion

        #region Properties

        #endregion
        #endregion

        #region Mono
        #endregion

        #region MazeBuilderBase
        protected override void InitialiseBuildParent(Maze aMaze)
        {
            BuildParent.localPosition = -new Vector3(
                aMaze.Grid.GetLength(0) - 1,
                0,
                aMaze.Grid.GetLength(1) - 1) / 2;
        }

        protected override void GetSpawnPositionAndRotation(Maze aMaze, Vector2Int aCoord, out Vector3 outPosition, out Quaternion outRotation)
        {
            outPosition = new Vector3(aCoord.x, 0, aCoord.y);
            outRotation = Quaternion.identity;
        }

        protected override GameObject GetPrefab(GridCell aCell)
        {
            if (aCell == null) return null;

            if (aCell is PathCell)
            {
                if (aCell is DeadEndCell)
                {
                    if (aCell is StartCell)
                    {
                        return m_startPathPrefab;
                    }
                    else if (aCell is EndCell)
                    {
                        return m_endPathPrefab;
                    }

                    return m_deadEndPathPrefab;
                }

                return m_pathPrefab;
            }
            else if (aCell is EdgeCell)
            {
                return m_edgePrefab;
            }
            else if (aCell is WallCell)
            {
                return m_innerWallPrefab;
            }
            else if (aCell is CornerCell)
            {
                return m_innerCornerPrefab;
            }
            else if (aCell is NullCell)
            {
                return m_pathPrefab;
            }

            return null;
        }

        public override void SetCameraProperties(Maze aMaze, Camera aCamera)
        {
            aCamera.orthographic = true;

            // 1024 / 768 == 1.33333
            float screenAspect = Screen.width / (float)Screen.height;
            // 1/1.3333 == 0.75
            float invScreenAspect = 1 / screenAspect;

            float unityScaleWidth = aMaze.Size.x;
            float unityScaleHeight = aMaze.Size.y;

            // varies
            float mazeAspect = unityScaleWidth / unityScaleHeight;
            float invMazeAspect = 1 / mazeAspect;

            float gridWidth = aMaze.Grid.GetLength(0);
            float gridHeight = aMaze.Grid.GetLength(1);

            float aspectTarget = mazeAspect / screenAspect;
            aCamera.orthographicSize = gridHeight * Mathf.Max(1, aspectTarget);

            aCamera.orthographicSize /= 2;
            aCamera.orthographicSize++;
        }
        #endregion
    }
}
