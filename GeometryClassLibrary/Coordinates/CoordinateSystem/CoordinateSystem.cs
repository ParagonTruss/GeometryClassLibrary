/*
    This file is part of Geometry Class Library.
    Copyright (C) 2016 Paragon Component Systems, LLC.

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
using System.Collections.Generic;
using Newtonsoft.Json;
using UnitClassLibrary;
using static UnitClassLibrary.DistanceUnit.Distance;
using UnitClassLibrary.AngleUnit;
using UnitClassLibrary.DistanceUnit;
using UnitClassLibrary.DistanceUnit.DistanceTypes.Imperial.InchUnit;

namespace GeometryClassLibrary
{
    /// <summary>
    /// 
    /// </summary>
    public class CoordinateSystem
    {
        #region Properties and Fields

        /// <summary>
        /// The world coordinate system
        /// </summary>
        public static CoordinateSystem WorldCoordinateSystem { get; } = new CoordinateSystem(Point.Origin, Angle.ZeroAngle, Angle.ZeroAngle, Angle.ZeroAngle);

        public Shift ShiftFromThisToWorld { get;  }
       
        public Point TranslationToOrigin => Matrix.ShiftPoint(Point.Origin, ShiftFromThisToWorld.Matrix);

        #endregion

        #region Constructors

        /// <summary>
        /// Null Constuctor
        /// </summary>
        private CoordinateSystem() { }

        [JsonConstructor]
        public CoordinateSystem(Shift shift)
        {
            this.ShiftFromThisToWorld = shift;
        }

        /// <summary>
        /// Creates a local coordinate system that is only translated from the World Coordinates.
        /// </summary>
        public CoordinateSystem(Point passedTranslationToOrigin)
        {
            this.ShiftFromThisToWorld = new Shift(passedTranslationToOrigin);
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="passedOrigin">The origin point of this coordinate system in reference to the world coordinate system</param>
        //public CoordinateSystem(Plane planeContainingTwoOfTheAxes, Vector axisInPassedPlaneToUseAsBase,
        //    Enums.Axis whichAxisIsPassed = Enums.Axis.X, Enums.AxisPlanes whichAxisPlaneIsPassed = Enums.AxisPlanes.XYPlane)
        //{

        //    //make sure the passed vector is in our plane
        //    if (!planeContainingTwoOfTheAxes.Contains(axisInPassedPlaneToUseAsBase))
        //    {
        //        throw new ArgumentOutOfRangeException("the passed axis was not in the plane");
        //    }

        //    Vector xAxis = new Vector(Point.Origin);
        //    Vector yAxis = new Vector(Point.Origin);
        //    Vector zAxis = new Vector(Point.Origin);

        //    //Vector normalAxis = new Vector(axisInPassedPlaneToUseAsBase.BasePoint, planeContainingTwoOfTheAxes.NormalVector.Direction, new Distance(new Inch(), 1));

        //    //Vector otherAxis;

        //    switch (whichAxisPlaneIsPassed)
        //    {
        //        case Enums.AxisPlanes.XYPlane:
        //            zAxis = new Vector(axisInPassedPlaneToUseAsBase.BasePoint, planeContainingTwoOfTheAxes.NormalVector.Direction, new Distance(new Inch(), 1));
        //            switch (whichAxisIsPassed)
        //            {
        //                //if its the x we were given the one we calculate is the y, so we want it 90 degrees (to the left)
        //                case Enums.Axis.X:
        //                    xAxis = axisInPassedPlaneToUseAsBase;
        //                    yAxis = zAxis.CrossProduct(xAxis);
        //                    //otherAxis = axisInPassedPlaneToUseAsBase.Rotate(new Rotation(planeContainingTwoOfTheAxes.NormalVector, Angle.RightAngle);
        //                    break;
        //                //if its the y we were the the y axis and we need to calculate the x which will be -90 degrees (to the right)
        //                case Enums.Axis.Y:
        //                    yAxis = axisInPassedPlaneToUseAsBase;
        //                    xAxis = yAxis.CrossProduct(zAxis);
        //                    //otherAxis = axisInPassedPlaneToUseAsBase.Rotate(new Rotation(planeContainingTwoOfTheAxes.NormalVector, -1 * Angle.RightAngle);
        //                    break;
        //                //the axis must be in the plane we were passed
        //                case Axis.Z:
        //                    throw new ArgumentOutOfRangeException("the passed axis type was not in the plane type");
        //            }
        //            break;
        //        case Enums.AxisPlanes.XZPlane:
        //            yAxis = new Vector(axisInPassedPlaneToUseAsBase.BasePoint, planeContainingTwoOfTheAxes.NormalVector.Direction, new Distance(new Inch(), 1));
        //            switch (whichAxisIsPassed)
        //            {
        //                //if its the x we were passed then we need to calculate z, which will be -90 degrees (to the right)
        //                case Enums.Axis.X:
        //                    xAxis = axisInPassedPlaneToUseAsBase;
        //                    zAxis = xAxis.CrossProduct(yAxis);
        //                    //otherAxis = axisInPassedPlaneToUseAsBase.Rotate(new Rotation(planeContainingTwoOfTheAxes.NormalVector, -1 * Angle.RightAngle);
        //                    break;
        //                //the axis must be in the plane we were passed
        //                case Enums.Axis.Y:
        //                    throw new ArgumentOutOfRangeException("the passed axis type was not in the plane type");
        //                //if its the z we were passed then we need to calculate x, which will be 90 degrees (to the left)
        //                case Axis.Z:
        //                    zAxis = axisInPassedPlaneToUseAsBase;
        //                    xAxis = yAxis.CrossProduct(zAxis);
        //                    //otherAxis = axisInPassedPlaneToUseAsBase.Rotate(new Rotation(planeContainingTwoOfTheAxes.NormalVector, Angle.RightAngle);
        //                    break;
        //            }
        //            break;
        //        case Enums.AxisPlanes.YZPlane:
        //            xAxis = new Vector(axisInPassedPlaneToUseAsBase.BasePoint, planeContainingTwoOfTheAxes.NormalVector.Direction, new Distance(new Inch(), 1));
        //            switch (whichAxisIsPassed)
        //            {
        //                //the axis must be in the plane we were passed
        //                case Enums.Axis.X:
        //                    throw new ArgumentOutOfRangeException("the passed axis type was not in the plane type");
        //                //if it is the Y axis then we need to find the z, which is 90 degrees (to the left)
        //                case Enums.Axis.Y:
        //                    yAxis = axisInPassedPlaneToUseAsBase;
        //                    zAxis = xAxis.CrossProduct(yAxis);
        //                    //otherAxis = axisInPassedPlaneToUseAsBase.Rotate(new Rotation(planeContainingTwoOfTheAxes.NormalVector, Angle.RightAngle);
        //                    break;
        //                //if it is the Z axis then we need to find the y, which is -90 degrees (to the right)
        //                case Axis.Z:
        //                    zAxis = axisInPassedPlaneToUseAsBase;
        //                    yAxis = zAxis.CrossProduct(xAxis);
        //                    //otherAxis = axisInPassedPlaneToUseAsBase.Rotate(new Rotation(planeContainingTwoOfTheAxes.NormalVector, -1 * Angle.RightAngle);
        //                    break;
        //            }
        //            break;
        //    }

        //    //we found our axes, now we can determine the angles from them
        //    //Since we rotate in the order Z, X, then Y, we must find the angles in the reverse order
        //    //i.e. y first, then x then z

        //    //if we find line up the z axis in the YZ plane with the y rotation, then we can rotate it around the x axis to make the z axes line up
        //    //and then we can z rotate to make the x and y coincide with the origins

        //    //first make them into unitvectors to simplify the calculations
        //    xAxis = xAxis.Direction*Inch;
        //    yAxis = yAxis.Direction*Inch;
        //    zAxis = zAxis.Direction*Inch;

        //    //now first find out the amount we need to rotate around the y axis to line up z in the yz plane

        //    //First project the z axis onto the xz plane
        //    Line projectedZAxis = ((Line)zAxis).ProjectOntoPlane(new Plane(Line.XAxis, Line.ZAxis));

        //    //then use the projected Line to find out how far we need to rotate in the Y direction to line up the z axes in the YZplane
        //    Angle angleBetweenCurrentZAndYZPlane = projectedZAxis.Direction.Theta;

        //    //if the projection is in the negative x direction we need to rotate negitively(clockwise) instead of positivly
        //    if (projectedZAxis.Direction.XComponent > 0)
        //    {
        //        angleBetweenCurrentZAndYZPlane = angleBetweenCurrentZAndYZPlane.Negate();
        //    }

        //    //http://www.vitutor.com/geometry/distance/line_plane.html
        //    //we can simplify the equation as this since it is unit vectors
        //    //sin(angle to plane) = z * planeNormal (which is the x axis by definition)
        //    //Distance dotProductOfZAndNormal = zAxis * Line.XAxis.UnitVector(new Inch());
        //    //Angle angleBetweenCurrentZAndYZPlane = new Angle(new Radian(), Math.Asin(dotProductOfZAndNormal.ValueInInches));

        //    //now rotate the axis (we only need to do z and x since we are done with y now)
        //    xAxis = xAxis.Rotate(new Rotation(Line.YAxis, angleBetweenCurrentZAndYZPlane));
        //    zAxis = zAxis.Rotate(new Rotation(Line.YAxis, angleBetweenCurrentZAndYZPlane));

        //    //now find out how much we need to rotate it in the x direction to line up z in the xz plane (meaning now z will be aligned with the world z)
        //    Angle angleBetweenZAndZAxis = zAxis.Direction.Theta;

        //    //now we need to rotate the x axis so we can line it up (the y and z we are done with)
        //    //if its negative we need to rotate it clockwise (negative) instead of ccw (positive)
        //    if (zAxis.Direction.YComponent < 0)
        //    {
        //        angleBetweenZAndZAxis = angleBetweenZAndZAxis.Negate();
        //    }

        //    //finally find out the z rotation needed to line up the x axis with the xz plane (this also forces the y to be lined up)
        //    xAxis = xAxis.Rotate(new Rotation(Line.XAxis, angleBetweenZAndZAxis));
        //    Angle angleBetweenXAndXAxis = xAxis.Direction.Phi;

        //    //now we know all our angles, but we have to take the negative of them because we were transforming back to
        //    //the origin and we store the tranform from the origin
        //    var _xAxisRotationAngle = angleBetweenZAndZAxis.Negate();
        //    var _yAxisRotationAngle = angleBetweenCurrentZAndYZPlane.Negate();
        //    var _zAxisRotationAngle = angleBetweenXAndXAxis.Negate();

        //    var rotationX = new Rotation(Line.XAxis, _xAxisRotationAngle);
        //    var rotationY = new Rotation(Line.YAxis, _yAxisRotationAngle);
        //    var rotationZ = new Rotation(Line.ZAxis, _zAxisRotationAngle);
        //    this.ShiftFromThisToWorld = new Shift(new List<Rotation>() { rotationX, rotationY, rotationZ }, axisInPassedPlaneToUseAsBase.BasePoint);
        //}

        /// <summary>
        /// Creates a new coordinate system with the given origin point and with the given rotations.
        /// The inputs are extrinsic angle, i.e. about the global axes
        /// </summary>
        /// <param name="passedTranslationToOrigin">The origin point of this coordinate system in reference to the world coordinate system</param>
        /// <param name="passedXAxisRotation">The rotation around the world coordinate system's X axis to rotate around to get to this
        /// coordinate system</param>
        /// <param name="passedYAxisRotation">The rotation around the world coordinate system's Y axis to rotate around to get to this
        /// coordinate system</param>
        /// <param name="passedZAxisRotation">The rotation around the world coordinate system's Z axis to rotate around to get to this
        /// coordinate system</param>
        public CoordinateSystem(Point translationToOrigin, Angle xAxisRotationAngle,
            Angle yAxisRotationAngle, Angle zAxisRotationAngle)
        {
            var rotationX = new Rotation(Line.XAxis, xAxisRotationAngle);
            var rotationY = new Rotation(Line.YAxis, yAxisRotationAngle);
            var rotationZ = new Rotation(Line.ZAxis, zAxisRotationAngle);
            this.ShiftFromThisToWorld = new Shift(new List<Rotation>() { rotationX, rotationY, rotationZ }, translationToOrigin);
        }

        /// <summary>
        /// Creates a copy of the given coordinate system
        /// </summary>
        /// <param name="toCopy">the Coordinate System to copy</param>
        public CoordinateSystem(CoordinateSystem toCopy)
        {
            this.ShiftFromThisToWorld = new Shift(toCopy.ShiftFromThisToWorld);
        }

        #endregion

        #region Overloaded Operators

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Not a perfect equality operator, is only accurate up to the Distance Class's accuracy
        /// </summary>
        public static bool operator ==(CoordinateSystem system1, CoordinateSystem system2)
        {
            // covers null reference checks
            if ((object)system1 == null)
            {
                if ((object)system2 == null)
                {
                    return true;
                }
                return false;
            }
            return system1.Equals(system2);
        }

        /// <summary>
        /// Not a perfect inequality operator, is only accurate up to the Distance Class's accuracy
        /// </summary>
        public static bool operator !=(CoordinateSystem system1, CoordinateSystem system2)
        {
            // if the two points' x and y are equal, returns false
            if ((object)system1 == null)
            {
                if ((object)system2 == null)
                {
                    return false;
                }
                return true;
            }
            return !system1.Equals(system2);
        }

        /// <summary>
        /// does the same thing as ==
        /// </summary>
        public override bool Equals(object obj)
        {
            //check for null (wont throw a castexception)
            if (obj == null || !(obj is CoordinateSystem))
            {
                return false;
            }
            var other = (CoordinateSystem)obj;
            return this.ShiftFromThisToWorld.Equals(other.ShiftFromThisToWorld);
        }

        #endregion

        #region Methods

        /// <summary>
        /// The rotation matrix that describes the local axes' orientation relative to the global axes.
        /// This single rotation is the product of the three separate rotations that follow.
        /// This rotation matrix is equivalent to switching the angles using the rotateFromThisTo(WorldCoordinateSystem) function
        /// </summary>
        public Matrix RotationMatrixFromThisToWorld()
        {
            return ShiftFromThisToWorld.RotationAboutOrigin.Matrix;
         }

        /// <summary>
        /// The rotation matrix that describes the local axes' orientation relative to the global axes.
        /// This single rotation is the product of the three separate rotations that follow.
        /// This rotation matrix is equivalent to switching the angles using the rotateToThisFrom(WorldCoordinateSystem) function
        /// </summary>
        public Matrix RotationMatrixToThisFromWorld()
        {
            // Inverse of the last method.
            // Since we're dealing with a rotation about the origin,
            // our matrix is necessarily orthogonal.
            // So its inverse is always just the transpose.
           return this.RotationMatrixFromThisToWorld().Transpose();
        }

        /// <summary>
        /// Determines if the orientations of the axes of two different coordinate systems are identical. 
        /// Note: a single orientation can be arrived at through several different sets of rotations.  Therefore,
        /// this method simply checks if the overall rotations are the same.
        /// </summary>
        /// <param name="other">The coordinate system we are checking against</param>
        /// <returns>Returns a bool of whether or not the two directions are equivalent</returns>
        public bool DirectionsAreEquivalent(CoordinateSystem other)
        {
            return GeometryClassLibrary.Shift.
                RotationsAreEquivalent(this.ShiftFromThisToWorld, other.ShiftFromThisToWorld);
            }

        /// <summary>
        /// Returns only the rotational Shift for this CoordinateSystem to apply to objects in order to only orient them in the passed CoordinateSystem, but not move them.
        /// If the CoordinateSystem to rotate to is left out, it rotates it to the WorldCoordinateSystem
        /// Note: Only works if this CoordinateSystem is the current shift on the object! if it is in another CoordinateSystem and you 
        /// perform this rotation it will cause incorrect results!
        /// </summary>
        public Rotation RotationFromThisTo(CoordinateSystem systemToRotateTo = null)
        {
            return RotationToThisFrom(systemToRotateTo).Inverse();
        }

        /// <summary>
        /// Returns only the rotational Shift to apply to objects in order to only orient them in this CoordinateSystem when they are currently oriented in the passed CoordinateSystem, but does not move them.
        /// If the CoordinateSystem to rotate from is left out, it assumes it is currently oriented in WorldCoordinateSystem
        /// Note: Only works if the passed CoordinateSystem is the current shift on the object! if it is in another CoordinateSystem and you 
        /// perform this rotation it will casue incorrect results!
        /// </summary>
        public Rotation RotationToThisFrom(CoordinateSystem systemToRotateFrom = null)
        {
            //find the whole shift but then make a new one with only the rotations
            Shift shiftFrom = ShiftToThisFrom(systemToRotateFrom);
            return shiftFrom.RotationAboutOrigin;
        }

        /// <summary>
        /// Returns the Shift to apply to objects in order to postition and orient them in the passed CoordinateSystem when they are currently postioned in this CoordinateSystem.
        /// If the system to shift to is left out, it defaults to the world coordinate System
        /// Note: Only works if this CoordinateSystem is the current shift on the object! if it is in another CoordinateSystem and you 
        /// perform this shift it will give incorrect results!
        /// </summary>
        /// <param name="systemToShiftTo">The CoordinateSystem to shift to from this CoordinateSystem. Defaults to the WorldCoordinateSystem if left out</param>
        /// <returns>Returns the Shift to apply to Objects in order to shift them from this CoordinateSystem to the passed CoordinateSystem</returns>
        public Shift ShiftFromThisTo(CoordinateSystem systemToShiftTo = null)
        {
            if (systemToShiftTo == null)
            {
                return this.ShiftFromThisToWorld;
            }
            //If not then we need to figure out how to shift between the two coordinate systems
            else
            {
                return this.ShiftFromThisToWorld * systemToShiftTo.ShiftFromThisToWorld.Inverse();
            }
        }

        /// <summary>
        /// Makes the Shift to apply to objects in order to postition and orient them in this CoordinateSystem when they are currently postioned in the passed CoordinateSystem.
        /// If the system to shift from is left out, it defaults to the world coordinate System
        /// Note: Only works if the passed CoordinateSystem is the current shift of the object! if it is in another CoordinateSystem and you 
        /// perform this shift it will give incorrect results!
        /// </summary>
        /// <param name="systemToShiftFrom">The CoordinateSystem to shift from to this CoordinateSystem. Defaults to the WorldCoordinateSystem if left out</param>
        /// <returns>Returns a Shft to be applied to an object to shift them from the passsed CoordinateSystem to this CoordinateSystem</returns>
        public Shift ShiftToThisFrom(CoordinateSystem systemToShiftFrom = null)
        {

            if (systemToShiftFrom == null)
            {
                return this.ShiftFromThisToWorld.Inverse();
            }
            //If not then we need to figure out how to shift between the two coordinate systems
            else
            {
                return systemToShiftFrom.ShiftFromThisToWorld * this.ShiftFromThisToWorld.Inverse();
            }
        }

        /// <summary>
        /// Find this coordinate system's (which is currently based on the passed system) shifts relative to the world coordinate 
        /// system instead of the passed coordinate system
        /// </summary>
        /// <param name="thisRelativeTo">The coordinate System this coordinate system is currently based on</param>
        /// <returns>Returns a new Coordinate System that reflects this coordinate system based on the world coordinate system 
        /// instead of the passed one</returns>
        public CoordinateSystem FindThisSystemRelativeToWorldSystemCurrentlyRelativeToPassedSystem(CoordinateSystem thisRelativeTo)
        {
            return new CoordinateSystem(thisRelativeTo.ShiftFromThisToWorld.Compose(this.ShiftFromThisToWorld));
        }

        /// <summary>
        /// This shifts the coordinate system relative to the world system with the given shift
        /// </summary>
        public CoordinateSystem Shift(Shift passedShift)
        {
           return new CoordinateSystem(passedShift.Compose(this.ShiftFromThisToWorld));
        }

        /// <summary>
        /// Shifts this system which is based on the world system with a shift that is relative to (in terms of) the passed system.
        /// This shifts this system relative to the world system with the equivalent of the shift in terms of the world system.
        /// ".Shift()" can be viewed as a special case of this relative shift where the system the shift is in terms of happens to be the world system
        /// </summary>
        /// <param name="passedShift">The shift that is based on the passed system to apply to this system</param>
        /// <param name="systemShiftIsRelativeTo">The system the shift is based in/ in terms of/ relative to</param>
        /// <returns>Returns a new CoordinateSystem that is based on the world system and has been shifted with the equivalent of the passed shift put in terms of world system</returns>
        public CoordinateSystem RelativeShift(Shift passedShift, CoordinateSystem systemShiftIsRelativeTo)
        {
            if (systemShiftIsRelativeTo == null)
            {
                systemShiftIsRelativeTo = WorldCoordinateSystem;
            }
            //change the coordinate system so that it is in terms of the current system so we can shift it relative to that system and 
            //then we need to shift it so its back on the worldCoordinates
            CoordinateSystem inCurrent = this.Shift(systemShiftIsRelativeTo.ShiftFromThisToWorld.Inverse());
            CoordinateSystem shiftedInCurrent = inCurrent.Shift(passedShift);
            //now put it back in terms of the world and return it
            return shiftedInCurrent.Shift(WorldCoordinateSystem.ShiftToThisFrom(systemShiftIsRelativeTo));
        }

        #endregion
    }
}
