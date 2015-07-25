using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeometryClassLibrary
{
    public class Parallelepiped : Polyhedron
    {
        public Parallelepiped(Vector vector1, Vector vector2, Vector vector3, Point basePoint = null)
            : base(MakeParallelepiped(vector1, vector2, vector3)) { }

        protected Parallelepiped(Polyhedron isParallelepiped) : base(isParallelepiped) { }
       
    }
}
