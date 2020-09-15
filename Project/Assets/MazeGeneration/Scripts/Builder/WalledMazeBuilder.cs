using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTools;

namespace MazeGeneration.Building
{
    public class WalledMazeBuilder : MazeBuilderBase
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
        [SerializeField] private GameObject m_edgeWallPrefab;
        [SerializeField] private GameObject m_edgeCornerPrefab;

        [SerializeField] private float m_pathCellSize = 1f;

        #endregion

        #region Properties

        #endregion
        #endregion

        #region MazeBuilderBase
        protected override void InitialiseBuildParent(Maze aMaze)
        {
            BuildParent.localPosition = -new Vector3(
                aMaze.Grid.GetLength(0) - 1,
                0,
                aMaze.Grid.GetLength(1) - 1) / 4;
        }

        protected override void GetSpawnPositionAndRotation(Maze aMaze, Vector2Int aCoord, out Vector3 outPosition, out Quaternion outRotation)
        {
            outPosition = new Vector3(aCoord.x, 0, aCoord.y) / 2 * m_pathCellSize;
            outRotation = Quaternion.identity;

            GridCell cell = aMaze.Grid[aCoord.x, aCoord.y];

            if (cell is PathCell path)
            {
                // do nothing special
            }
            else if (cell is CornerCell corner)
            {
                // do nothing special
            }
            else if (cell is EdgeCell edge)
            {
                if (cell is EdgeWallCell edgeWall)
                {
                    // determine vertical/horizontal
                    bool isVertical = edgeWall.Coord.x == 0 || edgeWall.Coord.x == aMaze.Grid.GetLength(0) - 1;

                    if (!isVertical)
                    {
                        outRotation = Quaternion.Euler(0, 90, 0);
                    }
                }
                else if (cell is EdgeCornerCell edgeCorner)
                {
                    // do nothing special
                }
            }
            else if (cell is WallCell wall)
            {
                // determine vertical/horizontal
                bool isVertical = wall.Coord.x % 2 == 0;

                if (!isVertical)
                {
                    outRotation = Quaternion.Euler(0, 90, 0);
                }
            }
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
                if (aCell is EdgeWallCell)
                {
                    return m_edgeWallPrefab;
                }
                else if (aCell is EdgeCornerCell)
                {
                    return m_edgeCornerPrefab;
                }

                return m_edgeCornerPrefab;
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
                return null;
            }

            return null;
        }
        #endregion
    }
}
