using System;
using Newtonsoft.Json;
using UnitClassLibrary;
using static UnitClassLibrary.Distance;

namespace GeometryClassLibrary
{
    /// <summary>
    /// A vector is a line segment that has a direction
    /// Except it derives from Line. So that it doesn't cut the way linesegments do.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Vector : Line
    {
        #region Properties and Fields

        /// <summary>
        /// Returns the magnitude of the vector
        /// </summary>
        [JsonProperty]
        public virtual Distance Magnitude
        {
            get { return _magnitude; }
            set { _magnitude = value; }
        }
        private Distance _magnitude;

        /// <summary>
        /// Returns the x-component of this vector
        /// </summary>
        public virtual Distance XComponent
        {
            get { return _magnitude * base.Direction.XComponent; }
        }

        /// <summary>
        /// Returns the y-component of this vector
        /// </summary>
        public virtual Distance YComponent
        {
            get { return _magnitude * base.Direction.YComponent; }
        }

        /// <summary>
        /// Returns the z-component of this vector
        /// </summary>
        public virtual Distance ZComponent
        {
            get { return _magnitude * base.Direction.ZComponent; }
        }

        /// <summary>
        /// Returns the point that is the distance away from the Vector's current basepoint that is equal to the vector's magnitude in the vector's direction
        /// </summary>
        public virtual Point EndPoint
        {
            get { return new Point(XComponent, YComponent, ZComponent) + BasePoint; }
        }

        ///// <summary>
        ///// Allows the xyz components of the vector to be able to be accessed as an array
        ///// </summary>
        ///// <param name="i"></param>
        ///// <returns></returns>
        //private Distance this[int i]
        //{
        //    get
        //    {
        //        if (i == 0)
        //            return XComponent;
        //        else if (i == 1)
        //            return YComponent;
        //        else if (i == 2)
        //            return ZComponent;
        //        else
        //            throw new Exception("No item of that index!");
        //    }
        //}

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
            _magnitude = passedBasePoint.DistanceTo(passedEndPoint);
        }

        /// <summary>
        /// Creates a new vector with the given BasePoint in the same direction and magnitude as the passed Vector
        /// </summary>
        public Vector(Point basePoint, Vector vector)
            : base(vector.Direction, basePoint)
        {
            _magnitude = new Distance(vector._magnitude);
        }

        public Vector(Direction direction, Distance magnitude) : base(direction)
        {
            this._magnitude = magnitude;
        }

        public Vector(Line line, Distance magnitude) : this(line.BasePoint, line.Direction, magnitude) { }

        public Vector(Vector vector, Distance magnitude) : this(vector.BasePoint, vector.Direction, magnitude) { }

        /// <summary>
        /// Creates a new vector with the given BasePoint in the given direction with the given magnitude
        /// </summary>
        public Vector(Point passedBasePoint, Direction direction, Distance magnitude = null)
            : base(direction, passedBasePoint)
        {
            if (magnitude == null)
            {
                _magnitude = Inch;
            }
            else
            {
                _magnitude = new Distance(magnitude);
            }
        }

        /// <summary>
        /// Creates a Vector with the same basepoint and end point as this edge
        /// </summary>
        public Vector(IEdge edge) : this(edge.BasePoint, edge.EndPoint) { }

        /// <summary>
        /// Default copy constructor
        /// </summary>
        public Vector(Vector toCopy)
            : this(toCopy.BasePoint, toCopy.Direction, toCopy.Magnitude) { }

        #endregion

        #region Overloaded Operators

        public override int GetHashCode()
        {
            return this.BasePoint.GetHashCode() ^ this.EndPoint.GetHashCode();
        }

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
        /// Returns a new Vector with each component multiplied by the scalar (order of terms does not matter)
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

                bool basePointsEqual = (this.BasePoint == vec.BasePoint);
                bool endPointsEqual = (this.EndPoint == vec.EndPoint);

                return basePointsEqual && endPointsEqual;
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

        /// <summary>
        /// returns the vector as a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("BasePoint= {0}, EndPoint= {1}, Magnitude= {2}", this.BasePoint.ToString(), this.EndPoint.ToString(), this.Magnitude.ToString());
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a new vector with the same base point and direction but a different magnitude.
        /// </summary>
        public Vector Resize(Distance newMagnitude)
        {
            return new Vector(BasePoint, Direction, newMagnitude);
        }

        /// <summary>
        /// Sometimes we want to check if the vector would intersect with line if it were extended towards the line
        /// </summary>
        /// <param name="passedLine"></param>
        /// <returns></returns>
        public Point HypotheticalIntersection(Line passedLine)
        {
            return new Line(this).IntersectWithLine(passedLine);
        }
        
        /// <summary>
        /// Checks if this Vector intersects the given line and returns the point if it does or null otherwise
        /// </summary>
        /// <param name="passedLine">The line to check if this intersects with</param>
        /// <returns>returns the intersection point of the two lines or null if they do not</returns>
        public override Point IntersectWithLine(Line passedLine)
        {
            Point intersect = new Line(this).IntersectWithLine(passedLine);

            if (!ReferenceEquals(intersect, null) && intersect.IsOnVector(this))
            {
                return intersect;
            }
            return null;
        }

        /// <summary>
        /// Checks if this LineSegment intersects with the given LineSegment and returns the point of intersection
        /// </summary>
        /// <param name="passedLineSegment">The LineSegment to check for intersection with</param>
        /// <returns>Returns the Point of intersection or null if they do not intersect</returns>
        public Point Intersection(Vector passedVector)
        {
            Point potentialIntersect = base.IntersectWithLine((Line)passedVector);

            if (potentialIntersect != null && potentialIntersect.IsOnVector(passedVector) && potentialIntersect.IsOnVector(this))
            {
                return potentialIntersect;
            }
            return null;
        }

        /// <summary>
        /// Returns true if the vector shares a base point or endpoint with the passed vector
        /// </summary>
        /// <param name="passedVector"></param>
        /// <returns></returns>
        public bool SharesABaseOrEndPointWith(Vector passedVector)
        {
            return (this.BasePoint == passedVector.EndPoint
                || this.BasePoint == passedVector.BasePoint
                || this.EndPoint == passedVector.EndPoint
                || this.EndPoint == passedVector.BasePoint);
        }

        /// <summary>
        /// Checks to see if a vector contains another vector.  Useful for checking if members touch
        /// </summary>
        /// <param name="passedVector">The Vector to see if is contained in this one</param>
        /// <returns>Returns a bool of whether or not the Vector is contained</returns>
        public bool Contains(Vector vector)
        {
            bool containsBasePoint = (this.Contains(vector.BasePoint));
            bool containsEndPoint = (this.Contains(vector.EndPoint));

            return containsBasePoint && containsEndPoint;
        }

        /// <summary>
        /// Determines whether or not the point is along/contained by this vector
        /// </summary>
        public new bool Contains(Point point)
        {
            //This method can make or break the Slice method. Handle very carefully.
            //This checks for a null point, and checks the distance from the point to the line.
            if (!new Line(this).Contains(point))
            {
                return false;
            }
            //We need this check before we check directions, because there is no direction if the point is the vector's basepoint
            if (point.IsBaseOrEndPointOf(this))
            {
                return true;
            }
            Vector pointVector = new Vector(this.BasePoint, point);
            bool sameDirection = this.HasSameDirectionAs(pointVector);
            bool greaterMagnitude = (this.Magnitude >= pointVector.Magnitude);

            return sameDirection && greaterMagnitude;
        }

        /// <summary>
        /// Determines whether or not the two Vectors in the same direction overlap at all partially or completely 
        /// </summary>
        /// <param name="potentiallyOverlappingVector">The vector to see if we overlap with</param>
        /// <returns>Returns true if the vectors overlap at all or one contains the other</returns>
        public bool DoesOverlapInSameDirection(Vector potentiallyOverlappingVector)
        {
            //see if we partially overlap
            bool doesSharePoint = this.Contains(potentiallyOverlappingVector.EndPoint) || this.Contains(potentiallyOverlappingVector.BasePoint);
            bool partiallyOverlap = this.HasSameDirectionAs(potentiallyOverlappingVector) && doesSharePoint;

            //or completely contain the other
            bool doesContainOneOrOther = this.Contains(potentiallyOverlappingVector) || potentiallyOverlappingVector.Contains(this);

            return partiallyOverlap || doesContainOneOrOther;
        }

        /// <summary>
        /// Projects this LineSegment onto the given Line, which is the projected length of this LineSegment in the direction of the Line projected onto
        /// </summary>
        /// <param name="projectOnto">the Line on which to project the LineSegment</param>
        /// <returns></returns>
        public Vector ProjectOntoLine(Line projectOnto)
        {
            if (this.IsCoplanarWith(projectOnto))
            {
                Point basePoint = this.BasePoint.ProjectOntoLine(projectOnto);
                Point endPoint = this.EndPoint.ProjectOntoLine(projectOnto);
                return new Vector(basePoint, endPoint);
            }
            throw new Exception("Cannot project a vector onto a line that its not coplanar with!");
        }

        /// <summary>
        /// Projects this vector on the given plane
        /// </summary>
        /// <param name="projectOnto">The Plane to project this Vector onto</param>
        /// <returns>Returns a new Vector that is this Vector projected onto the Plane</returns>
        public new Vector ProjectOntoPlane(Plane plane)
        {
            //find the line in the plane and then project this line onto it
            Point newBasePoint = BasePoint.ProjectOntoPlane(plane);
            Point newEndPoint = EndPoint.ProjectOntoPlane(plane);
            return new Vector(newBasePoint, newEndPoint);
        }

        /// <summary>
        /// Returns the cross product of the 2 vectors
        /// which is always perpendicular to both vectors
        /// and whose magnitude is the area of the parellelogram spanned by those two vectors
        /// Consequently if they point in the same (or opposite) direction, than the cross product is zero
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public Vector CrossProduct(Vector passedVector)
        {
            Vector v1 = new Vector(this);
            Vector v2 = new Vector(passedVector);

            //Find each component 

            double xProduct1 = v1.YComponent.Inches * v2.ZComponent.Inches;
            double xProduct2 = v1.ZComponent.Inches * v2.YComponent.Inches;

            double yProduct1 = v1.ZComponent.Inches * v2.XComponent.Inches;
            double yProduct2 = v1.XComponent.Inches * v2.ZComponent.Inches;

            double zProduct1 = v1.XComponent.Inches * v2.YComponent.Inches;
            double zProduct2 = v1.YComponent.Inches * v2.XComponent.Inches;

            double newX = (xProduct1) - (xProduct2);
            double newY = (yProduct1) - (yProduct2);
            double newZ = (zProduct1) - (zProduct2);

            return new Vector(Point.MakePointWithInches(newX, newY, newZ));
        }

        /// <summary>
        /// determines whether two vectors point in the same direction
        /// </summary>
        /// <param name="v1">vector to compare against</param>
        /// <returns></returns>
        public bool HasSameDirectionAs(Vector passedVector)
        {
            Vector vector1 = this;
            Vector vector2 = passedVector;

            if (vector2 == null)
            {
                return false; //Doesn't have a direction. Should be false
            }
            if (vector1.Magnitude == new Distance() || vector2.Magnitude == new Distance())
            {
                return true;
            }
            return vector1.Direction == vector2.Direction;
            
            
        }

        /// <summary>
        /// determines whether two vectors point in opposite directions 
        /// </summary>
        /// <param name="v1">vector to compare against</param>
        /// <returns></returns>
        public bool HasOppositeDirectionOf(Vector passedVector)
        {
            Vector vector1 = this;
            Vector vector2 = passedVector;
            
            //return (vector1.Direction == vector2.Direction.Reverse());
            return vector1.AngleBetween(vector2) == new Angle(AngleType.Degree, 180);
        }

        /// <summary>
        /// determines whether two vectors point in the same or opposite directions.
        /// </summary>
        public bool HasSameOrOppositeDirectionAs(Vector v1)
        {
            bool sameDirection = this.HasSameDirectionAs(v1);
            bool oppositeDirections = this.HasOppositeDirectionOf(v1);

            return sameDirection || oppositeDirections;
        }

        /// <summary>
        /// Switches the a vectors head with its tail.
        /// </summary>
        /// <returns></returns>
        public Vector Reverse()
        {
            return new Vector(this.EndPoint, this.BasePoint);
        }
        
       /// <summary>
        /// Flips the vector about its tail.
        /// Like this:  <-------|------->
       /// </summary>
       /// <returns></returns>
        public Vector FlipAboutTail()
        {
            Vector reversed = this.Reverse();
            return new Vector(this.BasePoint, (reversed + reversed).EndPoint);
        }

        /// <summary>
        /// Flips the vector about its head.
        /// Like this: ------>|<-------
        /// </summary><
        public Vector FlipAboutHead()
        {
            return new Vector((this + this).EndPoint, this.BasePoint);
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
        public new Vector Shift(Shift passedShift)
        {
            return new Vector(BasePoint.Shift(passedShift), EndPoint.Shift(passedShift));
        }

        /// <summary>
        /// Translates the vector the given distance in the given direction
        /// </summary>
        /// <param name="passedDirection"></param>
        /// <param name="passedDisplacement"></param>
        /// <returns></returns>
        public Vector Translate(Point translation)
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
        public new Vector UnitVector(DistanceType passedType)
        {
            if (Magnitude == new Distance())
            {
                return new Vector();
            }
            else return Direction.UnitVector(passedType);
        }
        /// <summary>
        /// Returns the DotProduct between two Vectors as a distance
        /// </summary>
        public Area DotProduct(Vector vector)
        {
            Vector vector1 = this;
            Vector vector2 = vector;

            var xTerm = vector1.XComponent * vector2.XComponent;
            var yTerm = vector1.YComponent * vector2.YComponent;
            var zTerm = vector1.ZComponent * vector2.ZComponent;
            
            var sum = xTerm + yTerm + zTerm;
            return sum;
        }
       
        /// <summary>
        /// Determines if the Dotproduct is zero. This is used to determine when vectors are perpendicular
        /// May be unnecessary as a method
        /// </summary>
        public bool IsPerpendicularTo(Vector other)
        {
            Area dotResult = this.DotProduct(other);

            //and return if its close to zero
            return (dotResult == new Area());
        }

        public bool IsParallelTo(Vector vector)
        {
            return this.HasSameOrOppositeDirectionAs(vector);
            //return this.CrossProduct(vector).Magnitude == new Distance();
            //checking the crossproduct is too precise for our library's purposes. It 
            //this might change when we implement error propagation.
        }

     

       


        #endregion

    }


}
