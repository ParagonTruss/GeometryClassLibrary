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

using System;
using System.Diagnostics;
using System.Linq;
using UnitClassLibrary.AngleUnit;
using UnitClassLibrary.DistanceUnit;
using static UnitClassLibrary.AngleUnit.Angle;
using static UnitClassLibrary.DistanceUnit.Distance;

namespace GeometryClassLibrary
{
    [DebuggerDisplay("Angle to Rotate = {AngleToRotate.InDegrees.Value}, Axis of Rotation: BasePoint = {AxisToRotateAround.BasePoint.X.ValueInInches}, {AxisToRotateAround.BasePoint.Y.ValueInInches}, {AxisToRotateAround.BasePoint.Z.ValueInInches}, Direction Vector = {AxisToRotateAround.DirectionVector.XComponentOfDirection.ValueInInches}, {AxisToRotateAround.DirectionVector.YComponentOfDirection.ValueInInches}, {AxisToRotateAround.DirectionVector.ZComponentOfDirection.ValueInInches}")]
    public class Rotation
    {
        public static Rotation Identity => new Rotation(Matrix.IdentityMatrix(4));
        #region Properties and Fields
        private Angle _rotationAngle;
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
        
        public Angle RotationAngle
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
            private set { _rotationAngle = value.ProperAngle; }
        }

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
        
        public Rotation(ILinear axisOfRotation, Angle rotationAngle)
        {
            this.RotationAngle = rotationAngle;
            this.AxisOfRotation = new Line(axisOfRotation);
        }

        public Rotation(Angle rotationAngle, Line axisOfRotation = null)
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

        private Rotation(Matrix matrix)
        {
            this._matrix = matrix;
        }

        private Rotation(Line axis, Angle angle, Matrix matrix)
        {
            this._axisOfRotation = axis;
            this._rotationAngle = angle;
            this._matrix = matrix;
        }

        #region Constructor Helpers
        private Matrix _matrixOfRotationAboutOrigin()
        {
            Matrix rotationMatrix = new Matrix(4, 4);

            Direction rotationUnitVector = this.AxisOfRotation.Direction;

            double unitX = rotationUnitVector.X; //Projection onto x-axis
            double unitY = rotationUnitVector.Y;
            double unitZ = rotationUnitVector.Z;
            Angle theta = this.RotationAngle;

            double sinTheta = Angle.Sine(theta);
            double cosTheta = Angle.Cosine(theta);

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

        private void _setMatrix()
        {
            var translateInverse = new Translation(_axisOfRotation.BasePoint.Negate()).Matrix;
            var rotate = _matrixOfRotationAboutOrigin();
            var translate = new Translation(_axisOfRotation.BasePoint).Matrix;

            this._matrix = translate * (rotate * translateInverse);
        }

        private void _setAxisAndAngle()
        {
            Line axis;
            Angle angle;
            var shift = new Shift(this.Matrix);
            var rotationMatrix = Matrix.ProjectiveMatrixToRotationMatrix(Matrix);
            var singularMatrix = rotationMatrix  - Matrix.IdentityMatrix(3);

            var nullSpace = (singularMatrix).NullSpace();
            axis = new Line(nullSpace[0]);

            var row = singularMatrix.Rows().FirstOrDefault(d => d.Any(v => Math.Abs(v) > 0.5));
            if (row != null)
            {
                var rowVector = new Vector(new Point(row.Select(d => new Distance(d, Inches)).ToList()));
                Vector rotated = new Vector(rowVector.EndPoint.Rotate3D(shift.RotationAboutOrigin));

                angle = rowVector.SignedAngleBetween(rotated, axis);

                var translation = singularMatrix.SystemSolve(new Matrix(shift.Translation.Point.Negate().ToListOfCoordinates()
                    .Select(d => d.ValueInInches).ToArray())).GetColumn(0).Select(d => new Distance(d, Inches)).ToList();
                axis = axis.Translate(new Point(translation));
            }
            else
            {
                angle = new Angle(0, Degrees);
            }
            this.AxisOfRotation = axis;
            this.RotationAngle = angle;
        }
        #endregion
        
        #endregion

        #region Methods

        /// <summary>
        /// This method runs no validation. So make sure this matrix represents an actual rotation.
        /// </summary>
        public static Rotation RotationFromMatrix(Matrix matrix)
        {
            return new Rotation(matrix);
        }
        /// <summary>
        /// Returns the rotation component of the shift object.
        /// </summary>
        public static Rotation RotationAboutOrigin(Shift shift)
        {
            var copy = new Matrix(shift.Matrix);
            copy.SetColumn(3, new double[] { 0, 0, 0, 1 });
            return new Rotation(copy);
        }

        /// <summary>
        /// Returns the inverse rotation.
        /// </summary>
        public Rotation Inverse()
        {
            var inverseAngle = RotationAngle.Negate();
            
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
            if (this.RotationAngle == ZeroAngle &&
                rotation.RotationAngle == ZeroAngle)
            {
                return true;
            }
            if (this.AxisOfRotation != rotation.AxisOfRotation)
            {
                return false;
            }      
            if (this.AxisOfRotation.Direction == rotation.AxisOfRotation.Direction)
            {
                if (this.RotationAngle == rotation.RotationAngle)
                {
                    return true;
                }
            }
            else
            {
                if (this.RotationAngle == rotation.RotationAngle.Negate().ProperAngle)
                {
                    return true;
                }
            }
            return false;        
        }

        #endregion 

    }
}
