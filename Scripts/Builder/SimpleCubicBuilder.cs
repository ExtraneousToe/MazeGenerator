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
        #endregion

        protected override void GetSpawnPositionAndRotation(Maze aMaze, Vector2Int aCoord, out Vector3 outPosition, out Quaternion outRotation)
        {
            outPosition = new Vector3(aCoord.x, 0, aCoord.y);
            outRotation = Quaternion.identity;
        }

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
            else if (aCell is PathCell || aCell is NullCell)
            {
                return m_pathPrefab;
            }

            return null;
        }
    }
}
