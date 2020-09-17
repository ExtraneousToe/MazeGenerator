using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MazeGeneration
{
    public abstract class AlgorithmScriptable : ScriptableObject
    {
        #region Variables
        #region Fields
        [SerializeField]
        private float m_routineDelay = 0.1f;
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
        #endregion
    }
}
