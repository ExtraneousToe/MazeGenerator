using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public class EdgeWallCell : EdgeCell
    {
        public EdgeWallCell(Vector2Int aCoord) : base(aCoord) { }
        public EdgeWallCell(int aX, int aY) : this(new Vector2Int(aX, aY)) { }
        public override string ToString() => "0";
    }
    public class EdgeCornerCell : EdgeCell
    {
        public EdgeCornerCell(Vector2Int aCoord) : base(aCoord) { }
        public EdgeCornerCell(int aX, int aY) : this(new Vector2Int(aX, aY)) { }
        public override string ToString() => "0";
    }
    public class WallCell : GridCell
    {
        public WallCell(Vector2Int aCoord) : base(aCoord) { }
        public WallCell(int aX, int aY) : this(new Vector2Int(aX, aY)) { }
        public override string ToString() => "x";
    }
    public class NullCell : GridCell
    {
        public NullCell(Vector2Int aCoord) : base(aCoord) { }
        public NullCell(int aX, int aY) : this(new Vector2Int(aX, aY)) { }
        public override string ToString() => " ";
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
    public class DeadEndCell : PathCell
    {
        public DeadEndCell(Vector2Int aCoord) : base(aCoord) { }
        public DeadEndCell(int aX, int aY) : this(new Vector2Int(aX, aY)) { }
        public override string ToString() => "+";
    }
    public class StartCell : DeadEndCell
    {
        public StartCell(Vector2Int aCoord) : base(aCoord) { }
        public StartCell(int aX, int aY) : this(new Vector2Int(aX, aY)) { }
        public override string ToString() => "S";
    }
    public class EndCell : DeadEndCell
    {
        public EndCell(Vector2Int aCoord) : base(aCoord) { }
        public EndCell(int aX, int aY) : this(new Vector2Int(aX, aY)) { }
        public override string ToString() => "E";
    }
}
