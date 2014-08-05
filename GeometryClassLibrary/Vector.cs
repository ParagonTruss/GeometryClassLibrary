using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using UnitClassLibrary;

namespace GeometryClassLibrary
{
    [DebuggerDisplay("Components = {XComponentOfDirection.Millimeters}, {YComponentOfDirection.Millimeters}, {ZComponentOfDirection.Millimeters}, Magnitude = {Magnitude.Millimeters}")]
    public class Vector : LineSegment
    {
        #region Properties and Fields
                

        /// <summary>
        /// Returns the magnitude of the vector (equals the length of the line segment parent)
        /// </summary>
        public Dimension Magnitude
        {
            get { return base.Length; }
            set { base.Length = value; }
        }

        private Dimension this[int i]
        {
            get
            {
                if (i == 0)
                    return XComponentOfDirection;
                else if (i == 1)
                    return YComponentOfDirection;
                else if (i == 2)
                    return ZComponentOfDirection;
                else
                    throw new Exception("No item of that index!");
            }
        }

        /// <summary>
        /// Returns a point that is a distance away from the Vector's current basepoint that is equal to the vector's magnitude in the same direction as the vector
        /// </summary>
        public Point DirectionPoint
        {
            get { return base.EndPoint; }
        }

        /// <summary>
        /// Returns the x-component of this direction vector
        /// </summary>
        public Dimension XComponentOfDirection
        {
            get { return base.XComponentOfDirection; }
        }

        /// <summary>
        /// Returns the y-component of this direction vector
        /// </summary>
        public Dimension YComponentOfDirection
        {
            get { return base.YComponentOfDirection; }
        }

        /// <summary>
        /// Returns the z-component of this direction vector
        /// </summary>
        public Dimension ZComponentOfDirection
        {
            get { return base.ZComponentOfDirection; }
        }

        #endregion

        #region Constructors
        /// <summary>
        /// Empty Constructor (A "Zero Vector")
        /// </summary>   
        public Vector():
        base(new Point()){}

        public Vector(Point passedBasePoint, Point passedEndPoint)
            : base(passedBasePoint, passedEndPoint) 
        {
            Magnitude = base.Length;
        }

        /// <summary>
        /// Constructs a unit vector (magnitude = 1) from the passed base point in the given direction
        /// </summary>
        /// <param name="passedBasePoint"></param>
        /// <param name="passedDirection"></param>
        public Vector(Point passedBasePoint, Vector passedDirection)
            : base(passedBasePoint, passedDirection) 
        {
            Magnitude = new Dimension(DimensionType.Millimeter, 1);
        }

        public Vector(Point passedBasePoint, Vector passedDirection, Dimension passedMagnitude)
            : base(passedBasePoint, passedDirection, passedMagnitude)
        {
            Magnitude = passedMagnitude;
        }

        /// <summary>
        /// Creates a vector that extends from the Origin to the passed reference point  
        /// </summary>
        /// <param name="passedX"></param>
        /// <param name="passedY"></param>
        /// <param name="passedZ"></param>
        public Vector(Point passedEndPoint) 
            : base(passedEndPoint)
        {}

        public Vector(Vector passedVector)
            : this(passedVector.BasePoint, passedVector, passedVector.Magnitude)
        { }
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
            Vector relocatedVector2 = new Vector(passedVector1.EndPoint, passedVector2.DirectionVector, passedVector2.Magnitude);
            
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
        public static Dimension operator *(Vector passedVector1, Vector passedVector2)
        {
            //returns a new dimension with the dot product of the two vectors
            double xComponent = passedVector1.XComponentOfDirection.Millimeters * passedVector2.XComponentOfDirection.Millimeters;
            double yComponent = passedVector1.YComponentOfDirection.Millimeters * passedVector2.YComponentOfDirection.Millimeters;
            double zComponent = passedVector1.ZComponentOfDirection.Millimeters * passedVector2.ZComponentOfDirection.Millimeters;

            return new Dimension(DimensionType.Millimeter, xComponent + yComponent + zComponent);
        }

        /// <summary>
        /// Returns a new Vector with each component multiplied by the multiplier (order of terms does not matter)
        /// </summary>
        /// <param name="passedVector"></param>
        /// <param name="multiplier"></param>
        /// <returns></returns>
        public static Vector operator *(Vector passedVector, double multiplier)
        {

            double xComponent = passedVector.XComponentOfDirection.Millimeters * multiplier;
            double yComponent = passedVector.YComponentOfDirection.Millimeters * multiplier;
            double zComponent = passedVector.ZComponentOfDirection.Millimeters * multiplier;

            Point newPoint = PointGenerator.MakePointWithMillimeters(xComponent, yComponent, zComponent);
            
            return new Vector(newPoint);
        }

        /// <summary>
        /// Returns a new Vector with each component multiplied by the multiplier (order of terms does not matter)
        /// </summary>
        /// <param name="multiplier"></param>
        /// <param name="passedVector"></param>
        /// <returns></returns>
        public static Vector operator *(double multiplier, Vector passedVector)
        {
            //returns a new Vector with each component multiplied by the multiplier

            double xComponent = passedVector.XComponentOfDirection.Millimeters * multiplier;
            double yComponent = passedVector.YComponentOfDirection.Millimeters * multiplier;
            double zComponent = passedVector.ZComponentOfDirection.Millimeters * multiplier;

            Point newPoint = PointGenerator.MakePointWithMillimeters(xComponent, yComponent, zComponent);

            return new Vector(newPoint);
        }

        /// <summary>
        /// Returns a new Vector with each component divided by the divisor
        /// </summary>
        /// <param name="passedVector1"></param>
        /// <param name="divisor"></param>
        /// <returns></returns>
        public static Vector operator /(Vector passedVector1, double divisor)
        {
            double multiplier = 1 / divisor;
            return passedVector1 * multiplier;

        }

        public static bool operator ==(Vector v1, Vector v2)
        {
            return v1.Equals(v2);
        }

        public static bool operator !=(Vector v1, Vector v2)
        {
            return !v1.Equals(v2);
        }

        public override bool Equals(object obj)
        {
            try
            {
                Vector vec = (Vector)obj;

                return PointInSameDirection(vec) &&
                    Magnitude == vec.Magnitude;
            }
            catch (InvalidCastException)
            {
                return false;
            }
        }
        #endregion

        #region Methods

        /// <summary>
        /// Returns the cross product of the 2 vectors(i.e. a vector that is perpendicular to both this vector and the passed Vector)
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public Vector CrossProduct(Vector passedVector)
        {
            Point originPoint = PointGenerator.MakePointWithMillimeters(0,0,0);
            Vector v1 = this;
            Vector v2 = passedVector;

            if(this.BasePoint != originPoint || passedVector.BasePoint != originPoint)
            {
                v1.BasePoint = originPoint;
                v2.BasePoint = originPoint;
            }

            //Find each component 

            double xProduct1 = v1.YComponentOfDirection.Millimeters * v2.ZComponentOfDirection.Millimeters;
            double xProduct2 = v1.ZComponentOfDirection.Millimeters * v2.YComponentOfDirection.Millimeters;

            double yProduct1 = v1.ZComponentOfDirection.Millimeters * v2.XComponentOfDirection.Millimeters;
            double yProduct2 = v1.XComponentOfDirection.Millimeters * v2.ZComponentOfDirection.Millimeters;

            double zProduct1 = v1.XComponentOfDirection.Millimeters * v2.YComponentOfDirection.Millimeters;
            double zProduct2 = v1.YComponentOfDirection.Millimeters * v2.XComponentOfDirection.Millimeters;

            Dimension newX = DimensionGenerator.MakeDimensionWithMillimeters((xProduct1) - (xProduct2));
            Dimension newY = DimensionGenerator.MakeDimensionWithMillimeters((yProduct1) - (yProduct2));
            Dimension newZ = DimensionGenerator.MakeDimensionWithMillimeters((zProduct1) - (zProduct2));

            return new Vector(PointGenerator.MakePointWithMillimeters(newX.Millimeters, newY.Millimeters, newZ.Millimeters));
        }

        /// <summary>
        /// Converts a vector into a matrix with one column
        /// </summary>
        /// <returns></returns>
        public Matrix ConvertToMatrixColumn()
        {
            Matrix returnMatrix = new Matrix(3, 1);
            double[] vectorArray = { XComponentOfDirection.Millimeters, YComponentOfDirection.Millimeters, ZComponentOfDirection.Millimeters };
            returnMatrix.SetColumnOfMatrix(0, vectorArray);
            return returnMatrix;
        }

        /// <summary>
        /// Divides the vector by its length to produce a unit vector (magnitude = 1)
        /// </summary>
        /// <returns></returns>
        public Vector ConvertToUnitVector()
        {
            Vector unitVector = this / this.Magnitude.Millimeters;   

            return unitVector;
        }

        /// <summary>
        /// Rotates the vector about the given axis by the passed angle
        /// </summary>
        /// <param name="passedRotationAxis"></param>
        /// <param name="passedRotationAngle"></param>
        /// <returns></returns>
        public Vector Rotate(Line passedRotationAxis, Angle passedRotationAngle)
        {
            Point rotatedBasePoint = this.BasePoint.Rotate3D(passedRotationAxis, passedRotationAngle);
            Point rotatedDirectionPoint =  this.DirectionPoint.Rotate3D(passedRotationAxis, passedRotationAngle);
            return new Vector(rotatedBasePoint, rotatedDirectionPoint);
        }

        /// <summary>
        /// determines whether two vectors point in the same direction using comparisons of x, y, and z components
        /// </summary>
        /// <param name="v1">vector to compare against</param>
        /// <returns></returns>
        public bool PointInSameDirection(Vector v1)
        {
            // checks to see if there's more than 1 nonzero component
            int componentIndex1 = -1;
            int componentIndex2 = -1;
            int numberOfZeroComponents1 = 0;
            int numberOfZeroComponents2 = 0;

            // checks the first Vector
            for (int i = 0; i < 3; i++)
            {
                if (this[i].Millimeters == 0)
                    numberOfZeroComponents1++;
                else
                    componentIndex1 = i;
            }

            // checks the second Vector
            for (int j = 0; j < 3; j++)
            {
                if (v1[j].Millimeters == 0)
                    numberOfZeroComponents2++;
                else
                    componentIndex2 = j;
            }

            // if there's only one, our job becomes a lot simpler
            // make sure it's the same component, and then check to see if multiplying them creates a number greater than 0, which means they have different signs
            if (numberOfZeroComponents1 == 2 && numberOfZeroComponents2 == 2 && componentIndex1 == componentIndex2)
                return this[componentIndex1] * v1[componentIndex2] > new Dimension();

            // uses compareTo as a factor and compares it to the other quotients, determining whether the
            // component is a multiple of the other component.  Negate() is used for opposite directions
            Dimension compareTo;

            // determines a component that does not equal 0 to use as a probable factor
            if (this.XComponentOfDirection.Millimeters != 0)
                compareTo = (this.XComponentOfDirection / v1.XComponentOfDirection).AbsoluteValue();
            else if (this.YComponentOfDirection.Millimeters != 0)
                compareTo = (this.YComponentOfDirection / v1.YComponentOfDirection).AbsoluteValue();
            else if (this.ZComponentOfDirection.Millimeters != 0)
                compareTo = (this.YComponentOfDirection / v1.YComponentOfDirection).AbsoluteValue();
            else
                compareTo = new Dimension();

            return v1.XComponentOfDirection * compareTo == this.XComponentOfDirection &&
                v1.YComponentOfDirection * compareTo == this.YComponentOfDirection &&
                v1.ZComponentOfDirection * compareTo == this.ZComponentOfDirection;
        }

        /// <summary>
        /// determines whether two vectors point in opposite directions using comparisons of x, y, and z components
        /// </summary>
        /// <param name="v1">vector to compare against</param>
        /// <returns></returns>
        public bool PointInOppositeDirections(Vector v1)
        {
            // checks to see if there's more than 1 nonzero component
            int componentIndex1 = -1;
            int componentIndex2 = -1;
            int numberOfZeroComponents1 = 0;
            int numberOfZeroComponents2 = 0;

            // checks the first Vector
            for (int i = 0; i < 3; i++)
            {
                if (this[i].Millimeters == 0)
                    numberOfZeroComponents1++;
                else
                    componentIndex1 = i;
            }

            // checks the second Vector
            for (int j = 0; j < 3; j++) 
            {
                if (v1[j].Millimeters == 0) 
                    numberOfZeroComponents2++;
                else
                    componentIndex2 = j;
            }

            // if there's only one, our job becomes a lot simpler
            // make sure it's the same component, and then check to see if multiplying them creates a number less than 0, which means only 1 is negative
            if (numberOfZeroComponents1 == 2 && numberOfZeroComponents2 == 2 && componentIndex1 == componentIndex2)
                return this[componentIndex1] * v1[componentIndex2] < new Dimension();

            // uses compareTo as a factor and compares it to the other quotients, determining whether the
            // component is a multiple of the other component.  Negate() is used for opposite directions
            Dimension compareTo;

            // determines a component that does not equal 0 to use as a probable factor
            if (this.XComponentOfDirection.Millimeters != 0)
                compareTo = (this.XComponentOfDirection / v1.XComponentOfDirection).AbsoluteValue();
            else if (this.YComponentOfDirection.Millimeters != 0)
                compareTo = (this.YComponentOfDirection / v1.YComponentOfDirection).AbsoluteValue();
            else if (this.ZComponentOfDirection.Millimeters != 0)
                compareTo = (this.YComponentOfDirection / v1.YComponentOfDirection).AbsoluteValue();
            else
                compareTo = new Dimension();

            return v1.XComponentOfDirection.Negate() * compareTo == this.XComponentOfDirection &&
                v1.YComponentOfDirection.Negate() * compareTo == this.YComponentOfDirection &&
                v1.ZComponentOfDirection.Negate() * compareTo == this.ZComponentOfDirection;
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
        #endregion

        /// <summary>
        /// creates a new vector towards acting on the same point but from the opposite side. 
        /// 
        /// In other words does not do this  <-------|------->
        /// 
        /// It does this ------>|<-------
        /// 
        /// 
        /// </summary>
        /// <returns>a negative instance of this Vector</returns>
        public Vector Negate()
        {
            //first flip the vector about itself
            Vector flipped = new Vector(this.EndPoint, this.BasePoint);

            //next translate the vector to the correct position
            return flipped.Translate(this.DirectionVector, this.Magnitude);
        }

        public Vector Translate(Vector passedDirectionVector, Dimension passedDisplacement)
        {
            Point newBasePoint = this.BasePoint.Translate(passedDirectionVector, passedDisplacement);
            Point newDirectionPoint = this.DirectionPoint.Translate(passedDirectionVector, passedDisplacement);

            return new Vector(newBasePoint, newDirectionPoint);
        }
    }
}
