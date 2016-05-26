using UnitClassLibrary;
using UnitClassLibrary.DistanceUnit;

namespace GeometryClassLibrary
{

    public class Rectangle : Polygon
    {
        public Distance Length { get; }
        public Distance Width { get; }

        /// <summary>
        /// Creates a Rectangle in the XY Plane, with one corner at the origin, and the specified length, width.
        /// </summary>
        /// <param name="length"></param>
        /// <param name="width"></param>
        public Rectangle(Distance length, Distance width, Point basePoint = null) 
            : base(_makeRectangle(length, width))
        {
            this.Length = length.AbsoluteValue();
            this.Width = width.AbsoluteValue();
        }

        private static Polygon _makeRectangle(Distance length, Distance width, Point basePoint = null)
        {
            var vector1 = new Vector(Direction.Right, length);
            var vector2 = new Vector(Direction.Up, width);
            
            var rectangle = Parallelogram(vector1, vector2, basePoint);
            return rectangle;
        }
        

        public Rectangle(Vector baseSegment, Distance height, Direction referencePlaneNormal = null)
            : base(_makeRectangle(baseSegment, height, referencePlaneNormal)) { }

        private static Polygon _makeRectangle(Vector baseSegment, Distance height, Direction referencePlaneNormal)
        {
            if (referencePlaneNormal == null)
            {
                referencePlaneNormal = Direction.Out;
            }

            Direction heightDirection = referencePlaneNormal.CrossProduct(baseSegment.Direction);
            var heightVector = new Vector(heightDirection, height);
            
            Polygon polygon = Parallelogram(baseSegment, heightVector);

            return polygon;
        }
    }
}
