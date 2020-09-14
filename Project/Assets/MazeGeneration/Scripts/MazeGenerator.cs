using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MazeGeneration
{
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

        private GridCell[,] m_generatedGrid;

        [Header("Building")]
        [SerializeField] private GameObject m_wallPrefab;
        [SerializeField] private GameObject m_edgePrefab;
        [SerializeField] private GameObject m_pathPrefab;

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
        }
        #endregion

        #region MazeGenerator
        public void Generate()
        {
            int width = m_gridSize.x * 2 + 1;
            int height = m_gridSize.y * 2 + 1;

            m_generatedGrid = new GridCell[width, height];

            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    if (x == 0
                        || x == width - 1
                        || y == 0
                        || y == height - 1)
                    {
                        m_generatedGrid[x, y] = new EdgeCell(x, y);
                    }
                    else if (x % 2 == 1
                        && y % 2 == 1)
                    {
                        m_generatedGrid[x, y] = new PathCell(x, y);
                    }
                    else
                    {
                        m_generatedGrid[x, y] = new WallCell(x, y);
                    }
                }
            }

            //m_roomGenerator?.GenerateRooms(ref m_generatedGrid);
            m_generationAlgorithm?.GeneratePath(ref m_generatedGrid, m_gridSize);

            StringBuilder stringBuilder = new StringBuilder();

            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    stringBuilder.Append(m_generatedGrid[x, y]);
                }
                stringBuilder.Append("\n");
            }

            Debug.Log(stringBuilder.ToString());
        }

        public void Build()
        {
            _spawnedChildren.ForEach(go => Destroy(go));
            _spawnedChildren.Clear();

            for (int x = 0; x < m_generatedGrid.GetLength(0); ++x)
            {
                for (int y = 0; y < m_generatedGrid.GetLength(1); ++y)
                {
                    GridCell cell = m_generatedGrid[x, y];
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
