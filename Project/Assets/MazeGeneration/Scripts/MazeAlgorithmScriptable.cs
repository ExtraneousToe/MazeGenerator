using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTools;

namespace MazeGeneration
{
    public abstract class MazeAlgorithmScriptable : ScriptableObject
    {
        #region Delegates
        public delegate void CellChangedDelegate(Vector2Int aCoord);
        public event CellChangedDelegate OnCellChanged;
        #endregion

        #region Variables
        #region Fields
        [SerializeField]
        private float m_routineDelay = 0.1f;

        protected readonly Vector2Int[] m_directions = new Vector2Int[]
        {
            Vector2Int.up,
            Vector2Int.right,
            Vector2Int.down,
            Vector2Int.left
        };
        private List<Vector2Int> m_directionsList = null;
        #endregion

        #region Properties
        public float RoutineDelay
        {
            get => m_routineDelay;
            set => m_routineDelay = value;
        }

        protected List<Vector2Int> DirectionsList
        {
            get
            {
                if (m_directionsList == null)
                {
                    m_directionsList = new List<Vector2Int>(m_directions);
                }

                return m_directionsList;
            }
        }

        protected abstract bool ShouldContinue { get; }
        #endregion
        #endregion

        #region RoomGenerator
        public IEnumerator RunAlgorithmRoutine(Maze aMaze, Vector2Int aSize)
        {
            Initialise(aMaze, aSize);

            do
            {
                Step(aMaze, aSize);

                if (RoutineDelay > 0)
                {
                    yield return new WaitForSeconds(RoutineDelay);
                }
            } while (ShouldContinue);

            Finalise(aMaze, aSize);
        }


        public void RunAlgorithm(Maze aMaze, Vector2Int aSize)
        {
            Initialise(aMaze, aSize);

            do
            {
                Step(aMaze, aSize);
            } while (ShouldContinue);

            Finalise(aMaze, aSize);
        }

        protected abstract void Initialise(Maze aMaze, Vector2Int aSize);
        protected abstract void Step(Maze aMaze, Vector2Int aSize);
        protected abstract void Finalise(Maze aMaze, Vector2Int aSize);

        protected void ShuffleDirections()
        {
            DirectionsList.Clear();
            DirectionsList.AddRange(m_directions.Shuffle());
        }

        protected void CellChanged(Vector2Int aCoord)
        {
            OnCellChanged?.Invoke(aCoord);
        }
        #endregion
    }
}
