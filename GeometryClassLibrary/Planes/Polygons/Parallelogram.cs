using System.Collections.Generic;

namespace GeometryClassLibrary
{
    public class Parallelogram : Polygon
    {
        public override Point Centroid {get { return base.CenterPoint; }}

        /// <summary>
        /// Null constructor for the benefit of Entity Framework
        /// </summary>
        protected Parallelogram()
            : base() { }

        public Parallelogram(Vector vector1, Vector vector2, Point basePoint = null)
            : base(MakeParallelogram(vector1, vector2, basePoint)) { }

        public Parallelogram(KeyValuePair<Vector, Vector> pair, Point basePoint = null)
            : this(pair.Key, pair.Value, basePoint) { }
    }
}
