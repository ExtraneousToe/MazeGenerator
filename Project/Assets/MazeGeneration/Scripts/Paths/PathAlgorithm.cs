using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTools;

namespace MazeGeneration
{
    public abstract class PathAlgorithm : ScriptableObject
    {
        #region Delegates
        public delegate void CellChangedDelegate(Vector2Int aCoord);
        public event CellChangedDelegate OnCellChanged;
        #endregion

        #region Variables
        [SerializeField]
        private float m_routineDelay = 0.1f;
        public float RoutineDelay => m_routineDelay; 
        #endregion

        public abstract IEnumerator GeneratePath(ref GridCell[,] aGrid, Vector2Int aSize);
    } 
}
