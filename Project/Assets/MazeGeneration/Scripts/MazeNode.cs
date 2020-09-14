using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MazeGeneration
{
    public class MazeNode
    {
        #region Variables
        public readonly int North = 0;
        public readonly int East = 1;
        public readonly int South = 2;
        public readonly int West = 3;

        private MazeNode[] m_nodeConnections;

        public MazeNode this[int direction]
        {
            get => m_nodeConnections[direction];
            set
            {
                m_nodeConnections[direction] = value;
            }
        }
        #endregion

        #region Constructors
        public MazeNode()
        {
            m_nodeConnections = new MazeNode[4];
        }
        #endregion
    }
}
