using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTools;

namespace MazeGeneration.Paths
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

        protected List<Vector2Int> m_directionsList = new List<Vector2Int>(new Vector2Int[]
        {
            Vector2Int.up,
            Vector2Int.right,
            Vector2Int.down,
            Vector2Int.left
        });
        #endregion

        public abstract IEnumerator GeneratePath(Maze aMaze, Vector2Int aSize);

        protected void CellChanged(Vector2Int aCoord)
        {
            OnCellChanged?.Invoke(aCoord);
        }
    } 
}
