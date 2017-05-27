/*
    This file is part of Geometry Class Library.
    Copyright (C) 2017 Paragon Component Systems, LLC.

    This library is free software; you can redistribute it and/or
    modify it under the terms of the GNU Lesser General Public
    License as published by the Free Software Foundation; either
    version 2.1 of the License, or (at your option) any later version.

    This library is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
    Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public
    License along with this library; if not, write to the Free Software
    Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
*/

using UnitClassLibrary.DistanceUnit;

namespace GeometryClassLibrary
{
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

        public Point Point { get; set; }
        
        public Matrix Matrix
        {
            get
            {
                var matrix = Matrix.IdentityMatrix(4);
                var array = new double[] { Point.X.ValueInInches, Point.Y.ValueInInches, Point.Z.ValueInInches, 1 };
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
        /// Creates a Translation in the direction of the given vector.
        /// </summary>
        /// <param name="vector"></param>
        public Translation(IVector vector)
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
           return !(translation1 == translation2);
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
