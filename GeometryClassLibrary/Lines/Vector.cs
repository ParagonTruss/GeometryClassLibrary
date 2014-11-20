using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using UnitClassLibrary;

namespace GeometryClassLibrary
{
    /// <summary>
    /// A vector is a line segment that has a direction
    /// </summary>
    [DebuggerDisplay("Components = {XComponentOfDirection.Millimeters}, {YComponentOfDirection.Millimeters}, {ZComponentOfDirection.Millimeters}, Magnitude = {Magnitude.Millimeters}")]
    [Serializable]
    public class Vector : Line
    {
        #region Properties and Fields
                
        /// <summary>
        /// Returns the magnitude of the vector
        /// </summary>
        public Distance Magnitude
        {
            get { return _magnitude; }
            set { _magnitude = value; }
        }
        private Distance _magnitude;

        /// <summary>
        /// Returns the x-component of this vector
        /// </summary>
        public Distance XComponent
        {
            get { return _magnitude * base.Direction.XComponentOfDirection; }
        }

        /// <summary>
        /// Returns the y-component of this vector
        /// </summary>
        public Distance YComponent
        {
            get { return _magnitude * base.Direction.YComponentOfDirection; }
        }

        /// <summary>
        /// Returns the z-component of this vector
        /// </summary>
        public Distance ZComponent
        {
            get { return _magnitude * base.Direction.ZComponentOfDirection; }
        }

        /// <summary>
        /// Returns the point that is the distance away from the Vector's current basepoint that is equal to the vector's magnitude in the vector's direction
        /// </summary>
        public Point EndPoint
        {
            get { return new Point(XComponent, YComponent, ZComponent) + BasePoint; }
        }

        /// <summary>
        /// Allows the xyz components of the vector to be able to be accessed as an array
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private Distance this[int i]
        {
            get
            {
                if (i == 0)
                    return XComponent;
                else if (i == 1)
                    return YComponent;
                else if (i == 2)
                    return ZComponent;
                else
                    throw new Exception("No item of that index!");
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Empty Constructor (A "Zero Vector")
        /// </summary>   
        public Vector()
            : base() 
        {
            _magnitude = new Distance();
        }

        /// <summary>
        /// Creates a vector that extends from the Origin to the passed reference point  
        /// </summary>
        /// <param name="passedEndPoint">The point at which the vector goes to / ends at</param>
        public Vector(Point passedEndPoint)
            : base(passedEndPoint)
        {
            _magnitude = passedEndPoint.DistanceTo(new Point());
        }

        /// <summary>
        /// Creates a vector that starts at the given base point and goes to the given end point
        /// </summary>
        /// <param name="passedBasePoint">The point at which the vector starts</param>
        /// <param name="passedEndPoint">The point at which the vector goes to / ends at</param>
        public Vector(Point passedBasePoint, Point passedEndPoint)
            : base(passedBasePoint, passedEndPoint) 
        {
            Distance xLength = passedBasePoint.X - passedEndPoint.X;
            Distance yLength = passedBasePoint.Y - passedEndPoint.Y;
            Distance zLength = passedBasePoint.Z - passedEndPoint.Z;

            _magnitude = new Point(xLength, yLength, zLength).DistanceTo(new Point());
        }

        /// <summary>
        /// Creates a new vector with the given BasePoint in the same direction and magnitude as the passed Vector
        /// </summary>
        /// <param name="passedBasePoint">The point at which the vector starts</param>
        /// <param name="passedVector">The vector with the direction and magnitude to use for this vector</param>
        public Vector(Point passedBasePoint, Vector passedVector)
            : base(passedVector.Direction, passedBasePoint)
        {
            _magnitude = new Distance(passedVector._magnitude);
        }

        /// <summary>
        /// Creates a new vector with the given BasePoint in the given direction with the given magnitude
        /// </summary>
        /// <param name="passedBasePoint">The point at which the vector starts</param>
        /// <param name="passedDirection">The direction the vector points</param>
        /// <param name="passedMagnitude">The length of the Vector</param>
        public Vector(Point passedBasePoint, Direction passedDirection, Distance passedMagnitude = null)
            : base(passedDirection, passedBasePoint)
        {
            if (passedMagnitude == null)
            {
                _magnitude = new Distance();
            }
            else
            {
                _magnitude = new Distance(passedMagnitude);
            }
        }

        /// <summary>
        /// Default copy constructor
        /// </summary>
        /// <param name="toCopy">The vector to copy</param>
        public Vector(Vector toCopy)
            : this(toCopy.BasePoint, toCopy.Direction, toCopy.Magnitude) { }

        #endregion

        #region Overloaded Operators

        /// <summary>
        /// Adds the two vectors and returns the resultant vector
        /// </summary>
        /// <param name="passedVector1"></param>
        /// <param name="passedVector2"></param>
        /// <returns></returns>
        public static Vector operator +(Vector passedVector1, Vector passedVector2)
        {
            //Recreates Vector 2 with its base point at the end point of Vector 1
            Vector relocatedVector2 = new Vector(passedVector1.EndPoint, passedVector2);
            
            Point newBasePoint = passedVector1.BasePoint; //The new vector has the same base point as Vector 1
            Point newEndPoint = relocatedVector2.EndPoint; //The new vector has the same end point as the relocated Vector 2

            return new Vector(newBasePoint, newEndPoint);
        }

        /// <summary>
        /// Subtracts the two vectors and returns the resultant vector
        /// </summary>
        /// <param name="passedVector1"></param>
        /// <param name="passedVector2"></param>
        /// <returns></returns>
        public static Vector operator -(Vector passedVector1, Vector passedVector2)
        {
            Vector negatedVector2 = -1 * passedVector2;
            return passedVector1 + negatedVector2;
        } 

        /// <summary>
        /// Returns the dot product of the two vectors
        /// </summary>
        /// <param name="passedVector1"></param>
        /// <param name="passedVector2"></param>
        /// <returns></returns>
        public static Distance operator *(Vector passedVector1, Vector passedVector2)
        {
            //returns a new Distance with the dot product of the two vectors
            double xComponent = passedVector1.XComponent.Inches * passedVector2.XComponent.Inches;
            double yComponent = passedVector1.YComponent.Inches * passedVector2.YComponent.Inches;
            double zComponent = passedVector1.ZComponent.Inches * passedVector2.ZComponent.Inches;

            return new Distance(DistanceType.Inch, xComponent + yComponent + zComponent);
        }

        /// <summary>
        /// Returns a new Vector with each component multiplied by the acalar (order of terms does not matter)
        /// </summary>
        /// <param name="passedVector"></param>
        /// <param name="scalar"></param>
        /// <returns></returns>
        public static Vector operator *(Vector passedVector, double scalar)
        {
            return new Vector(passedVector.BasePoint, passedVector.Direction, passedVector.Magnitude * scalar);
        }

        /// <summary>
        /// Returns a new Vector with each component multiplied by the scalar (order of terms does not matter)
        /// </summary>
        /// <param name="scalar"></param>
        /// <param name="passedVector"></param>
        /// <returns></returns>
        public static Vector operator *(double scalar, Vector passedVector)
        {
            return passedVector * scalar;
        }

        /// <summary>
        /// Returns a new Vector with each component divided by the divisor
        /// </summary>
        /// <param name="passedVector1"></param>
        /// <param name="divisor"></param>
        /// <returns></returns>
        public static Vector operator /(Vector passedVector, double divisor)
        {
            return new Vector(passedVector.BasePoint, passedVector.Direction, passedVector.Magnitude / divisor);
        }

        public static bool operator ==(Vector vector1, Vector vector2)
        {
            if ((object)vector1 == null)
            {
                if ((object)vector2 == null)
                {
                    return true;
                }
                return false;
            }
            return vector1.Equals(vector2);
        }

        public static bool operator !=(Vector vector1, Vector vector2)
        {
            if ((object)vector1 == null)
            {
                if ((object)vector2 == null)
                {
                    return false;
                }
                return true;
            }
            return !vector1.Equals(vector2);
        }

        public override bool Equals(object obj)
        {
            //check for null
            if (obj == null)
            {
                return false;
            }

            //try casting and comparing them
            try
            {
                Vector vec = (Vector)obj;

                bool areInSameDirection = PointInSameDirection(vec);
                bool magnitudesEqual = Magnitude == vec.Magnitude;

                return areInSameDirection && magnitudesEqual;
            }
            //if it was not a vector than it cant be equal
            catch (InvalidCastException)
            {
                return false;
            }
        }
        
        /// <summary>
        /// returns the comparison integer of -1 if less than, 0 if equal to, and 1 if greater than the other segment
        /// NOTE: BASED SOLELY ON LENGTH.  MAY WANT TO CHANGE LATER
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(Vector other)
        {
            if (this._magnitude.Equals(other._magnitude))
                return 0;
            else
                return this._magnitude.CompareTo(other._magnitude);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sometimes we want to check if the vector would intersect with line if it were extended towards the line
        /// </summary>
        /// <param name="passedLine"></param>
        /// <returns></returns>
        public Point HypotheticalIntersection(Line passedLine)
        {
            return new Line(this).Intersection(passedLine);
        }

        /// <summary>
        /// Checks if this Vector intersects the given line and returns the point if it does or null otherwise
        /// </summary>
        /// <param name="passedLine">The line to check if this intersects with</param>
        /// <returns>returns the intersection point of the two lines or null if they do not</returns>
        public override Point Intersection(Line passedLine)
        {
            Point intersect = this.HypotheticalIntersection(passedLine);

            if (!ReferenceEquals(intersect, null) && intersect.IsOnVector(this))
                return intersect;
            else
                return null;
        }

        /// <summary>
        /// Checks if this LineSegment intersects with the given LineSegment and returns the point of intersection
        /// </summary>
        /// <param name="passedLineSegment">The LineSegment to check for intersection with</param>
        /// <returns>Returns the Point of intersection or null if they do not intersect</returns>
        public Point Intersection(Vector passedVector)
        {
            Point potentialIntersect = this.Intersection((Line)passedVector);

            if (potentialIntersect != null && potentialIntersect.IsOnVector(passedVector))
                return potentialIntersect;
            else
                return null;
        }

        /// <summary>
        /// Returns true if the vector shares a base point or endpoint with the passed vector
        /// </summary>
        /// <param name="passedVector"></param>
        /// <returns></returns>
        public bool DoesSharesABaseOrEndPointWith(Vector passedVector)
        {
            return (this.BasePoint == passedVector.EndPoint
                || this.BasePoint == passedVector.BasePoint
                || this.EndPoint == passedVector.EndPoint
                || this.EndPoint == passedVector.BasePoint);
        }

        /// <summary>
        /// Checks to see if a vector contains another vector.  Useful for checking if members touch
        /// </summary>
        /// <param name="passedLine"></param>
        /// <returns></returns>
        public bool Contains(Vector passedVector)
        {
            if (this.Magnitude >= passedVector.Magnitude)
            {
                if (this.IsCoplanarWith(passedVector) && this.IsParallelTo(passedVector))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Projects this LineSegment onto the given Line, which is the projected length of this LineSegment in the direction of the Line projected onto
        /// </summary>
        /// <param name="projectOnto">the Line on which to project the LineSegment</param>
        /// <returns></returns>
        public Vector ProjectOntoLine(Line projectOnto)
        {
            //we can do this by making the points into vectors with the basepoint of the line as the basepoint and the endpoint of the 
            //point we want to project then using the dot product to find its relative vector and then position it 
            //relative to the clipping basepoint. We have to make the basepoint to the divisionLine's so that the vectors
            //intersect, allowing us to project the one onto the other easily
            Vector basePointVector = new Vector(projectOnto.BasePoint, BasePoint);

            //now we can project the basepointvector onto the division line's unit vector to find the scalar length of the projection
            //  note: we use the unit vector because it has a length of 1 and will not affect the scaling of the projected vector
            Distance basePointProjectedLength = basePointVector * projectOnto.UnitVector(DistanceType.Inch);

            //now make the projected vector using divisionLine's basePoint (our reference), the divisionLines unitVector (since we
            //  are projecting onto it we know that it is the direction of the resulting vector), and then the length we
            //  found for the projection
            Vector basePointProjection = new Vector(projectOnto.BasePoint, projectOnto.Direction, basePointProjectedLength);

            //now the basepointProjection starts at the divisionLine's endpoint and ends at the projected point on itself
            Point newBasePoint = basePointProjection.EndPoint;

            //now we just need to do it all again to find the newEndpoint's projection
            Vector endPointVector = new Vector(projectOnto.BasePoint, this.EndPoint);
            Distance endPointProjectedLength = endPointVector * projectOnto.UnitVector(DistanceType.Inch);
            Vector endPointProjection = new Vector(projectOnto.BasePoint, projectOnto.Direction, endPointProjectedLength);
            Point newEndPoint = endPointProjection.EndPoint;

            //now we have the two projected points and we can make a line segment which represents the porjected line
            return new Vector(newBasePoint, newEndPoint);
        }

        /// <summary>
        /// Projects this vector on the given plane
        /// </summary>
        /// <param name="projectOnto">The Plane to project this Vector onto</param>
        /// <returns>Returns a new Vector that is this Vector projected onto the Plane</returns>
        public Vector ProjectOntoPlane(Plane projectOnto)
        {
            //find the line in the plane and then project this line onto it
            Line projectedLine = ((Line)this).ProjectOntoPlane(projectOnto);
            return this.ProjectOntoLine(projectedLine);
        }

        /// <summary>
        /// Returns the cross product of the 2 vectors(i.e. a vector that is perpendicular to both this vector and the passed Vector)
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public Vector CrossProduct(Vector passedVector)
        {
            Point originPoint = new Point();
            Vector v1 = this;
            Vector v2 = new Vector(passedVector);

            if(this.BasePoint != originPoint || passedVector.BasePoint != originPoint)
            {
                v1.BasePoint = originPoint;
                v2.BasePoint = originPoint;
            }

            //Find each component 

            double xProduct1 = v1.YComponent.Inches * v2.ZComponent.Inches;
            double xProduct2 = v1.ZComponent.Inches * v2.YComponent.Inches;

            double yProduct1 = v1.ZComponent.Inches * v2.XComponent.Inches;
            double yProduct2 = v1.XComponent.Inches * v2.ZComponent.Inches;

            double zProduct1 = v1.XComponent.Inches * v2.YComponent.Inches;
            double zProduct2 = v1.YComponent.Inches * v2.XComponent.Inches;

            Distance newX = Distance.MakeDistanceWithInches((xProduct1) - (xProduct2));
            Distance newY = Distance.MakeDistanceWithInches((yProduct1) - (yProduct2));
            Distance newZ = Distance.MakeDistanceWithInches((zProduct1) - (zProduct2));

            return new Vector(PointGenerator.MakePointWithInches(newX.Inches, newY.Inches, newZ.Inches));
        }

        /// <summary>
        /// determines whether two vectors point in the same direction using comparisons of x, y, and z components
        /// </summary>
        /// <param name="v1">vector to compare against</param>
        /// <returns></returns>
        public bool PointInSameDirection(Vector passedVector)
        {
            // checks to see if there's more than 1 nonzero component
            int componentIndex1 = -1;
            int componentIndex2 = -1;
            int numberOfZeroComponents1 = 0;
            int numberOfZeroComponents2 = 0;

            // checks the first Vector
            for (int i = 0; i < 3; i++)
            {
                if (this[i] == new Distance())
                    numberOfZeroComponents1++;
                else
                    componentIndex1 = i;
            }

            // checks the second Vector
            for (int j = 0; j < 3; j++)
            {
                if (passedVector[j] == new Distance())
                    numberOfZeroComponents2++;
                else
                    componentIndex2 = j;
            }

            // if there's only one, our job becomes a lot simpler
            // make sure it's the same component, and then check to see if multiplying them creates a number greater than 0, which means they have different signs
            if (numberOfZeroComponents1 == 2 && numberOfZeroComponents2 == 2 && componentIndex1 == componentIndex2)
                return this[componentIndex1] * passedVector[componentIndex2] > new Area();

            // uses compareTo as a factor and compares it to the other quotients, determining whether the
            // component is a multiple of the other component.  Negate() is used for opposite directions
            double compareTo;

            // determines a component that does not equal 0 to use as a probable factor
            if (this.XComponent != new Distance())
                compareTo = Math.Abs(this.XComponent / passedVector.XComponent);
            else if (this.YComponent != new Distance())
                compareTo = Math.Abs(this.YComponent / passedVector.YComponent);
            else if (this.ZComponent != new Distance())
                compareTo = Math.Abs(this.ZComponent / passedVector.ZComponent);
            else
                compareTo = 0;

            return passedVector.XComponent * compareTo == this.XComponent &&
                passedVector.YComponent * compareTo == this.YComponent &&
                passedVector.ZComponent * compareTo == this.ZComponent;
        }

        /// <summary>
        /// determines whether two vectors point in opposite directions using comparisons of x, y, and z components
        /// </summary>
        /// <param name="v1">vector to compare against</param>
        /// <returns></returns>
        public bool PointInOppositeDirections(Vector passedVector)
        {
            //flip one of the vectors
            Vector passedVectorInOppositeDirection = passedVector.Negate();

            //then check if they are in the same direction
            return this.PointInSameDirection(passedVectorInOppositeDirection);
        }

        /// <summary>
        /// determines whether two vectors point in opposite directions using comparisons of x, y, and z components
        /// </summary>
        /// <param name="v1">vector to compare against</param>
        /// <returns></returns>
        public bool PointInSameOrOppositeDirections(Vector v1)
        {
            bool sameDirection = this.PointInSameDirection(v1);
            bool oppositeDirections = this.PointInOppositeDirections(v1);

            return sameDirection || oppositeDirections;
        }

        /// <summary>
        /// creates a new vector towards acting on the same point but from the opposite side. 
        /// 
        /// In other words does not do this  <-------|------->
        /// 
        /// It does this: ------>|<-------
        /// 
        /// 
        /// </summary>
        /// <returns>a negative instance of this Vector</returns>
        public Vector Negate()
        {
            //first flip the vector about itself
            Vector flipped = new Vector(this.EndPoint, this.BasePoint);

            //next translate the vector to the correct position
            return flipped.Translate(flipped.EndPoint);
        }

        /// <summary>
        /// Converts a vector into a matrix with one column
        /// </summary>
        /// <returns></returns>
        public Matrix ConvertToMatrixColumn()
        {
            Matrix returnMatrix = new Matrix(3, 1);
            double[] vectorArray = { XComponent.Inches, YComponent.Inches, ZComponent.Inches };
            returnMatrix.SetColumn(0, vectorArray);
            return returnMatrix;
        }

        /// <summary>
        /// Rotates the vector about the given axis by the passed angle
        /// </summary>
        /// <param name="rotationToApply">The Rotation to apply to the Vector that stores the axis to rotate around and the angle to rotate</param>
        /// <returns></returns>
        public new Vector Rotate(Rotation rotationToApply)
        {
            Point rotatedBasePoint = this.BasePoint.Rotate3D(rotationToApply);
            Point rotatedEndPoint = this.EndPoint.Rotate3D(rotationToApply);
            return new Vector(rotatedBasePoint, rotatedEndPoint);
        }

        /// <summary>
        /// Performs the Shift on this vector
        /// </summary>
        /// <param name="passedShift"></param>
        /// <returns></returns>
        public Vector Shift(Shift passedShift)
        {
            return new Vector(BasePoint.Shift(passedShift), EndPoint.Shift(passedShift));
        }

        /// <summary>
        /// Translates the vector the given distance in the given direction
        /// </summary>
        /// <param name="passedDirection"></param>
        /// <param name="passedDisplacement"></param>
        /// <returns></returns>
        public new Vector Translate(Point translation)
        {
            Point newBasePoint = this.BasePoint.Translate(translation);
            Point newEndPoint = this.EndPoint.Translate(translation);

            return new Vector(newBasePoint, newEndPoint);
        }


        /// <summary>
        /// Returns a unit vector with a length of 1 in with the given Distance that is equivalent to this direction
        /// Note: if you want a generic unitvector, you must call each of the components individually and keep track of them
        /// Note: if it is a zero vecotor and you call the unitVector it will return a zero vector
        /// </summary>
        /// <param name="passedType">Dimesnion Type that will be used. The vector will have a length of 1 in this unit type</param>
        /// <returns></returns>
        public Vector UnitVector(DistanceType passedType)
        {
            if (Magnitude == new Distance())
            {
                return new Vector();
            }
            else return Direction.UnitVector(passedType);
        }

        /// <summary>
        /// Returns whether or not the dot product between the two vectors is approximately equal to 0
        /// This takes into account the magnitude of the vectors and scales them in a way in which you
        /// should always get a result with a small fractional error
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public bool DotProductIsEqualToZero(Vector other)
        {
            //find out how to scale them so their multiplied magnitudes are 25
            double scale = 25 / (other.Magnitude.Inches * this.Magnitude.Inches);
            scale = Math.Sqrt(scale);

            Vector scaledVector1 = new Vector(new Point(), this.Direction, new Distance(DistanceType.Inch, scale * this.Magnitude.Inches));
            Vector scaledVector2 = new Vector(new Point(), other.Direction, new Distance(DistanceType.Inch, scale * other.Magnitude.Inches));

            //now get the result
            Distance dotResult = scaledVector1 * scaledVector2;

            //and return if its close to zero
            return dotResult == new Distance();
        }

        #endregion
    }
}
