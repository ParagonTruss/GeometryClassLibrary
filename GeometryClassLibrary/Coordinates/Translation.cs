using UnitClassLibrary;
using System;
using Newtonsoft.Json;

namespace GeometryClassLibrary
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Translation
    {
        public static implicit operator Translation(Point p)
        {
            return new Translation(p);
        }

        public static implicit operator Translation(Vector v)
        {
            return new Translation(v);
        }

        #region _fields & Properties

        [JsonProperty]
        public Point Point { get; set; }

        
        public Matrix Matrix
        {
            get
            {
                var matrix = GeometryClassLibrary.Matrix.IdentityMatrix(4);
                var array = new double[] { Point.X.Inches, Point.Y.Inches, Point.Z.Inches, 1 };
                matrix.SetColumn(3, array);
                return matrix;
            }
        }
        #endregion
        #region Constructors

        /// <summary>
        ///Null Constructor
        /// </summary>
        protected Translation() { }

        /// <summary>
        /// Creates a Translation with offsets in each direction represented by the point
        /// </summary>param>
        [JsonConstructor]
        public Translation(Point point)
        {
            this.Point = point;
        }

        /// <summary>
        /// Creates a Translation in the direction of the given vector.
        /// </summary>
        /// <param name="vector"></param>
        public Translation(Vector vector)
            : this(vector.EndPoint - vector.BasePoint) { }

        /// <summary>
        /// Creates a translation with the given translation Dimesnions in each direction
        /// </summary>
       public Translation(Distance xTranslation, Distance yTranslation, Distance zTranslation)
            : this(new Point(xTranslation, yTranslation, zTranslation)) { }


        /// <summary>
        /// Creates a copy of the given translation
        /// </summary>
        /// <param name="toCopy">The translation to copy</param>
        public Translation(Translation toCopy)
        {
            this.Point = toCopy.Point;
        }

        #endregion

        #region Overloaded Operators

        public override int GetHashCode()
        {
            return Matrix.GetHashCode();
        }

        /// <summary>
        /// Not a perfect equality operator, is only accurate up to the Distance Class's accuracy
        /// </summary>
        public static bool operator ==(Translation translation1, Translation translation2)
        {
            if ((object)translation1 == null)
            {
                if ((object)translation2 == null)
                {
                    return true;
                }
                return false;
            }
            return translation1.Equals(translation2);
        }

        /// <summary>
        /// Not a perfect equality operator, is only accurate up to the Distance Class's accuracy
        /// </summary>
        public static bool operator !=(Translation translation1, Translation translation2)
        {
            if ((object)translation1 == null)
            {
                if ((object)translation2 == null)
                {
                    return false;
                }
                return true;
            }
            return !translation1.Equals(translation2);
        }

        /// <summary>
        /// does the same thing as ==
        /// </summary>
        public override bool Equals(object obj)
        {
            //make sure we didnt get a null
            if (obj == null || !(obj is Translation))
            {
                return false;
            }

          
            Translation comparableTranslation = (Translation)obj;

            return this.Point.Equals(comparableTranslation.Point);
        
        }

        #endregion

        public Translation Inverse()
        {
            return new Translation(this.Point.Negate());
        }
    }
}
