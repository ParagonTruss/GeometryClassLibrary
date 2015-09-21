using System.Collections.Generic;

namespace GeometryClassLibrary
{

    public class Rhombus : Parallelogram
    {
        public Rhombus() : base(_makeVectors())
        {
                
        }

        private static KeyValuePair<Vector, Vector> _makeVectors()
        {
            return new KeyValuePair<Vector, Vector>(new Vector(Point.MakePointWithInches(1, 0)), new Vector(Point.MakePointWithInches(0, 1)));
        }
    }
}
