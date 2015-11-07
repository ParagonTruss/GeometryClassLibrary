namespace GeometryClassLibrary
{
    public class Parallelepiped : Polyhedron
    {
        /// <summary>
        /// Private null constructor for the use of data frameworks like Entity Framework and Json.NET
        /// </summary>
        private Parallelepiped() { }

        public Parallelepiped(Vector vector1, Vector vector2, Vector vector3, Point basePoint = null)
            : base(MakeParallelepiped(vector1, vector2, vector3)) { }

        protected Parallelepiped(Polyhedron isParallelepiped) : base(isParallelepiped) { }
       
    }
}
