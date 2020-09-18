using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTools;

namespace MazeGeneration.Paths
{
    public abstract class PathAlgorithm : MazeAlgorithmScriptable
    {
        #region Variables
        #region Fields
        #endregion

        #region Properties
        #endregion
        #endregion

        #region MazeAlgorithmScriptable
        protected override void Finalise(Maze aMaze, Vector2Int aSize)
        {
            Vector2Int[] modPoints = aMaze.MarkDeadEnds();
            foreach (Vector2Int point in modPoints)
            {
                CellChanged(point);
            }

            modPoints = aMaze.CreateStartAndEnd();
            foreach (Vector2Int point in modPoints)
            {
                CellChanged(point);
            }
        }
        #endregion
    } 
}
