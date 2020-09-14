using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MazeGeneration
{
    using PathGeneration;
    using RoomGeneration;

    public abstract class GridCell : IEquatable<GridCell>
    {
        public Vector2Int Coord { get; set; }

        public GridCell(Vector2Int aCoord)
        {
            Coord = aCoord;
        }

        public bool Equals(GridCell other)
        {
            return Coord.Equals(other.Coord);
        }
    }
    public class EdgeCell : GridCell
    {
        public EdgeCell(Vector2Int aCoord) : base(aCoord) { }
        public EdgeCell(int aX, int aY) : this(new Vector2Int(aX, aY)) { }
        public override string ToString() => "0";
    }
    public class WallCell : GridCell
    {
        public WallCell(Vector2Int aCoord) : base(aCoord) { }
        public WallCell(int aX, int aY) : this(new Vector2Int(aX, aY)) { }
        public override string ToString() => "x";
    }
    public class CornerCell : GridCell
    {
        public CornerCell(Vector2Int aCoord) : base(aCoord) { }
        public CornerCell(int aX, int aY) : this(new Vector2Int(aX, aY)) { }
        public override string ToString() => "C";
    }
    public class PathCell : GridCell
    {
        public PathCell(Vector2Int aCoord) : base(aCoord) { }
        public PathCell(int aX, int aY) : this(new Vector2Int(aX, aY)) { }
        public override string ToString() => "+";
    }

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

        private Maze m_maze;

        [Header("Building")]
        [SerializeField] private GameObject m_wallPrefab;
        [SerializeField] private GameObject m_edgePrefab;
        [SerializeField] private GameObject m_pathPrefab;
        [SerializeField] private GameObject m_cornerPrefab;

        private List<GameObject> _spawnedChildren = new List<GameObject>();
        #endregion

        #region Properties
        #endregion
        #endregion

        #region Mono
        protected void Start()
        {
            m_generationAlgorithm.OnCellChanged += (a) => Build();
            Generate();
            Build();
        }
        #endregion

        #region MazeGenerator
        public void Generate()
        {
            m_maze = new Maze(m_gridSize);

            Build();

            //m_roomGenerator?.GenerateRooms(ref m_generatedGrid);
            StartCoroutine(m_generationAlgorithm?.GeneratePath(m_maze, m_gridSize));
        }

        public void Build()
        {
            _spawnedChildren.ForEach(go => Destroy(go));
            _spawnedChildren.Clear();

            for (int x = 0; x < m_maze.Grid.GetLength(0); ++x)
            {
                for (int y = 0; y < m_maze.Grid.GetLength(1); ++y)
                {
                    GridCell cell = m_maze.Grid[x, y];
                    Vector3 worldPos = new Vector3(
                        x, 0, y
                    );

                    GameObject prefab = null;

                    if (cell is PathCell)
                    {
                        prefab = m_pathPrefab;
                    }
                    else if (cell is EdgeCell)
                    {
                        prefab = m_edgePrefab;
                    }
                    else if (cell is WallCell)
                    {
                        prefab = m_wallPrefab;
                    }
                    if (cell is CornerCell)
                    {
                        prefab = m_cornerPrefab;
                    }

                    if (prefab)
                    {
                        _spawnedChildren.Add(
                            Instantiate(prefab, worldPos, Quaternion.identity, transform)
                        );
                    }
                }
            }
        }
        #endregion
    }
}
