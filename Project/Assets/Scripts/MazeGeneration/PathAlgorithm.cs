using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MazeGeneration
{
    public abstract class PathAlgorithm : ScriptableObject
    {
        public abstract void GeneratePath();
    } 
}
