using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEngine;

namespace MazeGeneration.Demo
{
    using Paths;

    public class MazeDemoController : MonoBehaviour
    {
        #region Variables
        #region Fields
        [Header("UI")]
        [SerializeField] private GameObject m_setupUIPanel;
        [SerializeField] private GameObject m_generatingPanel;

        [Header("Generation")]
        [SerializeField] private List<PathAlgorithm> m_pathAlgorithms;
        [SerializeField] private List<MazeGenerator> m_mazeGenerators;

        [Header("Size")] 
        [SerializeField] private int m_mazeGridWidth;
        [SerializeField] private int m_mazeGridHeight;
        #endregion

        #region Properties
        public int AlgorithmIndex
        {
            get; 
            private set;
        }

        public int BuilderIndex
        {
            get;
            private set;
        }
        #endregion
        #endregion

        public void SetGenerationAlgorithm(int aIndex)
        {
            AlgorithmIndex = aIndex;
        }

        public void SetGenerationBuilder(int aIndex)
        {
            BuilderIndex = aIndex;
        }

        public void SetMazeWidth(int aWidth)
        {
            m_mazeGridWidth = aWidth;
        }

        public void SetMazeHeight(int aHeight)
        {
            m_mazeGridHeight = aHeight;
        }

        public void BuildMaze()
        {
            // get algorithm scriptable
            // set timing

            // get builder
            // set sizings
            // set algorithm
            // set the camera position
            // trigger the build
        }

        public void ResetState()
        {
            // clear the current builder
        }
    } 
}
