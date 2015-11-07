using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnitClassLibrary;
using System.Linq;
using MoreLinq;
using static UnitClassLibrary.Distance;

namespace GeometryClassLibrary
{
    [DebuggerDisplay("Angle to Rotate = {AngleToRotate.Degrees}, Axis of Rotation: BasePoint = {AxisToRotateAround.BasePoint.X.Inches}, {AxisToRotateAround.BasePoint.Y.Inches}, {AxisToRotateAround.BasePoint.Z.Inches}, Direction Vector = {AxisToRotateAround.DirectionVector.XComponentOfDirection.Inches}, {AxisToRotateAround.DirectionVector.YComponentOfDirection.Inches}, {AxisToRotateAround.DirectionVector.ZComponentOfDirection.Inches}")]
    [JsonObject(MemberSerialization.OptIn)]
    public class Rotation
    {
        #region Properties and Fields
        private AngularDistance _rotationAngle;
        private Line _axisOfRotation;

        private Matrix _matrix;
        public Matrix Matrix
        {
            get
            {
                if (_matrix == null)
                {
                    _setMatrix();
                }
                return _matrix;
            }
        }

        [JsonProperty]
        public AngularDistance RotationAngle
        {
            get
            {
                //When we use the matrix constructor, we will not know the rotation angle, nor will we neccesarily need it.
                //If hasn't been determined we go ahead and determine both the axis and rotation angle.
                if (_rotationAngle == null)
                {
                    _setAxisAndAngle();
                }
                return _rotationAngle;
            }
            private set { _rotationAngle = new Angle(value); }
        }

        [JsonProperty]
        public Line AxisOfRotation
        {
            get
            {
                //When we use the matrix constructor, we will not know the rotation axis, nor will we neccesarily need it.
                //If hasn't been determined we go ahead and determine both the axis and rotation angle.

                if (_axisOfRotation == null)
                {
                    _setAxisAndAngle();   
                }
                return _axisOfRotation;
            }
            private set { _axisOfRotation = new Line(value); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Null Constructor
        /// </summary>
        private Rotation() { }
       
        /// <summary>
        /// Creates a rotation about the input Axis and with the input Angle of 0 if the angle is omitted
        /// </summary>
        [JsonConstructor]
        public Rotation(Line axisOfRotation, AngularDistance rotationAngle)
        {
            this.RotationAngle = rotationAngle;
            this.AxisOfRotation = axisOfRotation;
        }

        public Rotation(AngularDistance rotationAngle, Line axisOfRotation = null)
        {
            if (axisOfRotation == null)
            {
                axisOfRotation = Line.ZAxis;
            }
            this.AxisOfRotation = axisOfRotation;
            this.RotationAngle = rotationAngle;
        }

        /// <summary>
        /// Creates a copy of the given rotation
        /// </summary>
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

            this._matrix = translate * (rotate * translateInverse);
        }

        private Matrix _matrixOfRotationAboutOrigin()
        {
            Matrix rotationMatrix = new Matrix(4, 4);

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

   
        public Rotation(Matrix matrix)
        {
            this._matrix = matrix;
        }

        private void _setAxisAndAngle()
        {
            var shift = new Shift(this.Matrix);
            var rotationMatrix = Matrix.ProjectiveMatrixToRotationMatrix(Matrix);
            var singularMatrix = rotationMatrix  - Matrix.IdentityMatrix(3);

            var nullSpace = (singularMatrix).NullSpace();
            var axis = nullSpace[0];

            var rowVector = new Vector(new Point(singularMatrix.Rows().First(d => d.Any(v => Math.Abs(v) > 0.5)).Select(d => d*Inch).ToList()));
            Vector rotated = new Vector(rowVector.EndPoint.Rotate3D(shift.RotationAboutOrigin));

            var angle = rowVector.SignedAngleBetween(rotated, axis);

            var translation = singularMatrix.SystemSolve(shift.Translation.Point.Negate().ToListOfCoordinates()
                .Select(d => d.Inches).ToArray()).Select(d => d * Inch).ToList();
            axis = axis.Translate(new Point(translation));

            this.AxisOfRotation = axis;
            this.RotationAngle = angle;
        }

        private Rotation(Line axis, AngularDistance angle, Matrix matrix)
        {
            this._axisOfRotation = axis;
            this._rotationAngle = angle;
            this._matrix = matrix;
        }

        #endregion 
             
        #region Methods

        /// <summary>
        /// Returns the inverse rotation.
        /// </summary>
        public Rotation Inverse()
        {
            var inverseAngle = RotationAngle * -1;
            
            return new Rotation(this.AxisOfRotation, inverseAngle);
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
            if (other == null || !(other is Rotation))
            {
                return false;
            }
            Rotation rotation = (Rotation)other;

            return this.Matrix == rotation.Matrix;
        }

        #endregion 

    }
}
