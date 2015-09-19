using System;
using System.Diagnostics;
using UnitClassLibrary;

namespace GeometryClassLibrary
{
    [DebuggerDisplay("Angle to Rotate = {AngleToRotate.Degrees}, Axis of Rotation: BasePoint = {AxisToRotateAround.BasePoint.X.Inches}, {AxisToRotateAround.BasePoint.Y.Inches}, {AxisToRotateAround.BasePoint.Z.Inches}, Direction Vector = {AxisToRotateAround.DirectionVector.XComponentOfDirection.Inches}, {AxisToRotateAround.DirectionVector.YComponentOfDirection.Inches}, {AxisToRotateAround.DirectionVector.ZComponentOfDirection.Inches}")]
    public class Rotation
    {
        #region Properties and Fields
        private readonly AngularDistance _rotationAngle;
        private readonly Line _axisOfRotation;
        private Matrix _matrix;
        public Matrix Matrix { get { return _matrix; } }
        public AngularDistance RotationAngle
        {
            get
            {
                return _rotationAngle;
            }
        }

        public Line AxisOfRotation
        {
            get
            {
                return _axisOfRotation;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Null Constructor
        /// </summary>
        public Rotation() { }
       
        /// <summary>
        /// Creates a rotation about the input Axis and with the input Angle of 0 if the angle is omitted
        /// </summary>
        /// <param name="axisOfRotation">The Axis to rotate around</param>
        /// <param name="rotationAngle">The Angle that specifies how far to rotate</param>
        public Rotation(Line axisOfRotation, AngularDistance rotationAngle)
        {
            this._rotationAngle = rotationAngle;
            this._axisOfRotation = axisOfRotation;
            _setMatrix();
        }

        public Rotation(AngularDistance rotationAngle, Line axisOfRotation = null)
        {
            if (axisOfRotation == null)
            {
                axisOfRotation = Line.ZAxis;
            }
            this._axisOfRotation = axisOfRotation;
            this._rotationAngle = rotationAngle;
            _setMatrix();
        }

        /// <summary>
        /// Creates a copy of the given rotation
        /// </summary>
        /// <param name="toCopy">the Rotation to copy</param>
        public Rotation(Rotation toCopy)
        {
            this._rotationAngle = toCopy._rotationAngle;
            this._axisOfRotation = toCopy._axisOfRotation;
            this._matrix = new Matrix(toCopy._matrix);
        }

        private void _setMatrix()
        {
            var translateInverse = new Translation(_axisOfRotation.BasePoint.Negate()).Matrix;
            var rotate = _matrixOfRotationAboutOrigin();
            var translate = new Translation(_axisOfRotation.BasePoint).Matrix;
            this._matrix = translate * rotate * translateInverse;
        }
        /// <summary>
        /// Returns a matrix that can be multiplied by another matrix to represent a rotation of that matrix about the passed axis line by the specified angle
        /// </summary>>
        private Matrix _matrixOfRotationAboutOrigin()
        {
            Matrix rotationMatrix = new Matrix(4,4);

            Direction rotationUnitVector = this.AxisOfRotation.Direction;

            double unitX = rotationUnitVector.XComponent; //Projection onto x-axis
            double unitY = rotationUnitVector.YComponent;
            double unitZ = rotationUnitVector.ZComponent;
            double theta = this.RotationAngle.Radians;

            double sinTheta = Math.Sin(theta);
            double cosTheta = Math.Cos(theta);

            double row0column0 = cosTheta + unitX * unitX * (1 - cosTheta);
            double row0column1 = unitX * unitY * (1 - cosTheta) - unitZ * sinTheta;
            double row0column2 = unitX * unitZ * (1 - cosTheta) + unitY * sinTheta;
            double row1column0 = unitY * unitX * (1 - cosTheta) + unitZ * sinTheta;
            double row1column1 = cosTheta + unitY * unitY * (1 - cosTheta);
            double row1column2 = unitY * unitZ * (1 - cosTheta) - unitX * sinTheta;
            double row2column0 = unitZ * unitX * (1 - cosTheta) - unitY * sinTheta;
            double row2column1 = unitZ * unitY * (1 - cosTheta) + unitX * sinTheta;
            double row2column2 = cosTheta + unitZ * unitZ * (1 - cosTheta);

            rotationMatrix.SetElement(0, 0, row0column0);
            rotationMatrix.SetElement(0, 1, row0column1);
            rotationMatrix.SetElement(0, 2, row0column2);
            rotationMatrix.SetElement(1, 0, row1column0);
            rotationMatrix.SetElement(1, 1, row1column1);
            rotationMatrix.SetElement(1, 2, row1column2);
            rotationMatrix.SetElement(2, 0, row2column0);
            rotationMatrix.SetElement(2, 1, row2column1);
            rotationMatrix.SetElement(2, 2, row2column2);

            rotationMatrix.SetElement(3, 3, 1.0);
            return rotationMatrix;
        }
        #endregion 

        #region Overloaded Operators

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }


        /// <summary>
        /// Not a perfect equality operator, is only accurate up to Constants.AcceptedEqualityDeviationConstant 
        /// </summary>
        public static bool operator ==(Rotation rotation1, Rotation rotation2)
        {
            if ((object)rotation1 == null)
            {
                if ((object)rotation2 == null)
                {
                    return true;
                }
                return false;
            }
            return rotation1.Equals(rotation2);
        }

        /// <summary>
        /// Not a perfect inequality operator, is only accurate up to Constants.AcceptedEqualityDeviationConstant 
        /// </summary>
        public static bool operator !=(Rotation rotation1, Rotation rotation2)
        {
            if ((object)rotation1 == null)
            {
                if ((object)rotation2 == null)
                {
                    return false;
                }
                return true;
            }
            return !rotation1.Equals(rotation2);
        }

        /// <summary>
        /// does the same thing as == if the passed in object is a Rotation
        /// </summary>
        public override bool Equals(object other)
        {
            //make sure the obj is not null
            if (other == null)
            {
                return false;
            }

            //try casting it and then comparing it
            try
            {
                Rotation rotation = (Rotation)other;
                if (this.AxisOfRotation != rotation.AxisOfRotation)
                {
                    return false;
                }
                return (this.AxisOfRotation.Direction == rotation.AxisOfRotation.Direction &&
                    this.RotationAngle == rotation.RotationAngle) ||
                    (this.AxisOfRotation.Direction == rotation.AxisOfRotation.Direction.Reverse() &&
                    this.RotationAngle == rotation.RotationAngle.Negate());

            }
            //if it wasn't a rotation than it's not equal
            catch (InvalidCastException)
            {
                return false;
            }
        }

        #endregion 

        #region Methods

        /// <summary>
        /// Returns the inverse rotation.
        /// </summary>
        public Rotation Inverse()
        {
            Angle inverseAngle = new Angle(AngleType.Radian, this.RotationAngle.Radians * -1);
            return new Rotation(this.AxisOfRotation, inverseAngle);
        }

        #endregion
    }
}
