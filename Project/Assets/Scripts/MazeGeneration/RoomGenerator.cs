using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MazeGeneration
{
    public abstract class RoomGenerator : ScriptableObject
    {
        public abstract void GenerateRooms();
    }
}