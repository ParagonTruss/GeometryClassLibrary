using System.Collections.Generic;

namespace GeometryClassLibrary
{
    public class Parallelogram : Polygon
    {
        public override Point Centroid {get { return base.CenterPoint; }}

        public Parallelogram(Vector vector1, Vector vector2, Point basePoint = null)
            : base(MakeParallelogram(vector1, vector2, basePoint)) { }

        public Parallelogram(KeyValuePair<Vector, Vector> pair, Point basePoint = null)
            : this(pair.Key, pair.Value, basePoint) { }
    }
}
