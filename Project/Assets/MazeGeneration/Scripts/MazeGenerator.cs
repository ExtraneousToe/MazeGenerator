﻿using System;
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
        [SerializeField]
        private Vector2Int m_gridSize = new Vector2Int(10, 10);

        [Header("Rooms")]
        [SerializeField] private RoomGenerator m_roomGenerator;

        [Header("Path Generation")]
        [SerializeField] private PathAlgorithm m_generationAlgorithm;

        private Maze m_maze;

        [Header("Building")]
        [SerializeField] private MazeBuilderBase m_builder;

        private List<GameObject> _spawnedChildren = new List<GameObject>();
        #endregion

        #region Properties
        #endregion
        #endregion

        #region Mono
        protected void Start()
        {
            m_generationAlgorithm.OnCellChanged += (a) => m_builder?.UpdateConstruction(m_maze, a);
            Generate();
        }
        #endregion

        #region MazeGenerator
        public void RegisterBuilder(MazeBuilderBase aBuilder)
        {
            m_builder = aBuilder;
        }

        public void Generate()
        {
            m_maze = new Maze(m_gridSize);

            m_builder?.BuildMaze(m_maze);

            //m_roomGenerator?.GenerateRooms(ref m_generatedGrid);
            StartCoroutine(Routine());

            IEnumerator Routine()
            {
                yield return StartCoroutine(m_generationAlgorithm?.GeneratePath(m_maze, m_gridSize));
                m_builder?.BuildMaze(m_maze);
            }
        }
        #endregion
    }
}
