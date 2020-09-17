using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MazeGeneration
{
    using Paths;
    using Rooms;
    using Building;

    /// <summary>
    /// Generates a map using a 2D grid of cells
    /// </summary>
    public class MazeGenerator : MonoBehaviour
    {
        #region Variables
        #region Fields
        [Header("Size")]
        [SerializeField] private Vector2Int m_gridSize = new Vector2Int(10, 10);

        [Header("Rooms")]
        [SerializeField] private RoomGenerator m_roomGenerator;

        [Header("Path Generation")]
        [SerializeField] private PathAlgorithm m_generationAlgorithm;

        private Maze m_maze;

        [Header("Building")]
        [SerializeField] private MazeBuilderBase m_builder;

        private Coroutine m_buildingRoutine;
        #endregion

        #region Properties
        public Vector2Int GridSize
        {
            get => m_gridSize;
            set => m_gridSize = value;
        }

        public RoomGenerator RoomGenerator
        {
            get => m_roomGenerator;
            set => m_roomGenerator = value;
        }

        public PathAlgorithm PathAlgorithm
        {
            get => m_generationAlgorithm;
            set => m_generationAlgorithm = value;
        }

        public MazeBuilderBase MazeBuilder
        {
            get => m_builder;
            set => m_builder = value;
        }
        #endregion
        #endregion

        #region Mono
        protected void OnEnable()
        {
            m_buildingRoutine = null;

            for (int i = 0; i < 2; ++i)
            {
                m_gridSize[i] = Mathf.Max(2, m_gridSize[i]);
            }

            m_generationAlgorithm.OnCellChanged += UpdateConstruction;
            Generate();
        }

        protected void OnDisable()
        {
            if (m_buildingRoutine != null)
            {
                StopCoroutine(m_buildingRoutine);
                m_buildingRoutine = null;
            }

            m_generationAlgorithm.OnCellChanged -= UpdateConstruction;
            m_builder?.ClearConstruction();
        }
        #endregion

        #region MazeGenerator
        private void UpdateConstruction(Vector2Int aCellCoord)
        {
            m_builder?.UpdateConstruction(m_maze, aCellCoord);
        }

        public void RegisterBuilder(MazeBuilderBase aBuilder)
        {
            m_builder = aBuilder;
        }

        public void Initialise()
        {
            m_maze = new Maze(m_gridSize);
        }

        public void Generate()
        {
            m_builder?.BuildMaze(m_maze);

            m_buildingRoutine = StartCoroutine(Routine());

            IEnumerator Routine()
            {
                if (m_roomGenerator) yield return StartCoroutine(m_roomGenerator?.GenerateRooms(m_maze));
                if (m_generationAlgorithm) yield return StartCoroutine(m_generationAlgorithm?.GeneratePath(m_maze, m_gridSize));

                Vector2Int[] modPoints = m_maze.MarkDeadEnds();
                foreach (Vector2Int point in modPoints)
                {
                    m_builder?.UpdateConstruction(m_maze, point);
                }

                modPoints = m_maze.CreateStartAndEnd();
                foreach (Vector2Int point in modPoints)
                {
                    m_builder?.UpdateConstruction(m_maze, point);
                }

                m_builder?.BuildMaze(m_maze);
            }
        }

        public void SetCameraProperties(Camera aCamera)
        {
            m_builder?.SetCameraProperties(m_maze, aCamera);
        }
        #endregion
    }
}
