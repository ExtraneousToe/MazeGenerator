using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MazeGeneration
{
    public class GridCell { }
    public class EdgeCell : GridCell { }
    public class WallCell : GridCell { }

    /// <summary>
    /// Generates a map using a 2D grid of cells
    /// 
    /// x: Edge
    /// 0: Pathable cell
    /// |+-: Walls
    /// 
    /// XXXXXXXXXXXXX
    /// X0|0|0|0|0|0X
    /// x-+-+-+-+-+-X
    /// X0|0|0|0|0|0X
    /// x-+-+-+-+-+-X
    /// X0|0|0|0|0|0X
    /// x-+-+-+-+-+-X
    /// X0|0|0|0|0|0X
    /// XXXXXXXXXXXXX
    /// 
    /// </summary>
    public class MazeGenerator : MonoBehaviour
    {
        #region Variables
        #region Fields
        [Header("Size")]
        [SerializeField]
        private Vector2Int m_gridSize = new Vector2Int(10, 10);

        [Header("Rooms")]
        [SerializeField] private RoomGenerator m_roomGenerator;

        [Header("Path Generation")]
        [SerializeField] private PathAlgorithm m_generationAlgorithm;
        #endregion

        #region Properties

        #endregion
        #endregion

        #region Mono

        #endregion

        #region MazeGenerator

        #endregion
    }
}
