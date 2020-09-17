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
        #region Fields
        [SerializeField]
        private float m_routineDelay = 0.1f;

        protected List<Vector2Int> m_directionsList = new List<Vector2Int>(new Vector2Int[]
        {
            Vector2Int.up,
            Vector2Int.right,
            Vector2Int.down,
            Vector2Int.left
        });
        #endregion

        #region Properties
        public float RoutineDelay
        {
            get => m_routineDelay;
            set => m_routineDelay = value;
        }

        protected abstract bool ShouldContinue { get; }
        #endregion
        #endregion

        public IEnumerator GeneratePathRoutine(Maze aMaze, Vector2Int aSize)
        {
            InitialiseAlgorithm(aMaze, aSize);

            do
            {
                StepAlgorithm(aMaze, aSize);

                if (RoutineDelay > 0)
                {
                    yield return new WaitForSeconds(RoutineDelay);
                }
            } while (ShouldContinue);
        }

        public void GeneratePath(Maze aMaze, Vector2Int aSize)
        {
            InitialiseAlgorithm(aMaze, aSize);

            do
            {
                StepAlgorithm(aMaze, aSize);
            } while (ShouldContinue);
        }

        protected abstract void InitialiseAlgorithm(Maze aMaze, Vector2Int aSize);
        protected abstract void StepAlgorithm(Maze aMaze, Vector2Int aSize);

        protected void CellChanged(Vector2Int aCoord)
        {
            OnCellChanged?.Invoke(aCoord);
        }
    } 
}
