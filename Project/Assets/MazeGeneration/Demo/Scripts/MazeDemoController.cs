using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEngine;

namespace MazeGeneration.Demo
{
    using Microsoft.Win32;
    using Paths;
    using UnityEngine.UI;

    public class MazeDemoController : MonoBehaviour
    {
        #region Variables
        #region Fields
        [Header("UI")]
        [SerializeField] private GameObject m_setupUIPanel;
        [SerializeField] private GameObject m_generatingPanel;
        [SerializeField] private Slider m_widthSlider;
        [SerializeField] private Slider m_heightSlider;
        [SerializeField] private Slider m_timeSlider;

        [Header("Generation")]
        [SerializeField] private List<PathAlgorithm> m_pathAlgorithms;
        [SerializeField] private List<MazeGenerator> m_mazeGenerators;
        [SerializeField] private int m_algorithmIndex;
        [SerializeField] private int m_builderIndex;
        [SerializeField] private float m_delayTime;

        [Header("Size")]
        [SerializeField] private int m_mazeGridWidth;
        [SerializeField] private int m_mazeGridHeight;
        #endregion

        #region Properties
        public int AlgorithmIndex
        {
            get => m_algorithmIndex;
            private set => m_algorithmIndex = value;
        }

        public int BuilderIndex
        {
            get => m_builderIndex;
            private set => m_builderIndex = value;
        }

        public float DelayTime
        {
            get => m_delayTime;
            private set => m_delayTime = value;
        }

        public int GridWidth
        {
            get => m_mazeGridWidth;
            private set => m_mazeGridWidth = value;
        }

        public int GridHeight
        {
            get => m_mazeGridHeight;
            private set => m_mazeGridHeight = value;
        }
        #endregion
        #endregion

        #region Mono
        protected void Start()
        {
            m_widthSlider.value = GridWidth = 5;
            m_widthSlider.onValueChanged?.Invoke(m_widthSlider.value);

            m_heightSlider.value = GridHeight = 5;
            m_heightSlider.onValueChanged?.Invoke(m_heightSlider.value);

            m_timeSlider.value = DelayTime = 0.05f;
            m_timeSlider.onValueChanged?.Invoke(m_timeSlider.value);

            DisplayUI(true);
        }
        #endregion

        public void DisplayUI(bool asSetup)
        {
            m_setupUIPanel.SetActive(asSetup);
            m_generatingPanel.SetActive(!asSetup);
        }

        public void SetGenerationAlgorithm(int aIndex)
        {
            AlgorithmIndex = aIndex;
        }

        public void SetGenerationBuilder(int aIndex)
        {
            BuilderIndex = aIndex;
        }

        public void SetMazeWidth(float aWidth)
        {
            SetMazeWidth(Mathf.RoundToInt(aWidth));
        }
        public void SetMazeWidth(int aWidth)
        {
            GridWidth = aWidth;
        }

        public void SetMazeHeight(float aHeight)
        {
            SetMazeHeight(Mathf.RoundToInt(aHeight));
        }
        public void SetMazeHeight(int aHeight)
        {
            GridHeight = aHeight;
        }

        public void SetDelayTime(float aTime)
        {
            DelayTime = aTime;
        }

        public void BuildMaze()
        {
            // get algorithm scriptable
            PathAlgorithm path = m_pathAlgorithms[AlgorithmIndex];
            // set timing
            path.RoutineDelay = DelayTime;

            // get builder
            MazeGenerator builder = m_mazeGenerators[BuilderIndex];
            // set sizings
            builder.GridSize = new Vector2Int(
                GridWidth,
                GridHeight
            );
            // set algorithm
            builder.PathAlgorithm = path;
            builder.Initialise();
            // set the camera position
            builder.SetCameraProperties(Camera.main);

            // trigger the build
            builder.gameObject.SetActive(true);

            DisplayUI(false);
        }

        public void ResetState()
        {
            // clear the current builder
            m_mazeGenerators.ForEach(mg => mg.gameObject.SetActive(false));

            DisplayUI(true);
        }
    }
}
