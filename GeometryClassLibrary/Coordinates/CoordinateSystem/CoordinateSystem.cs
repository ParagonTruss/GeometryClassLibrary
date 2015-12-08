using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnitClassLibrary;
using static UnitClassLibrary.DistanceUnit.Distance;
using UnitClassLibrary.AngleUnit;

namespace GeometryClassLibrary
{
    /// <summary>
    /// Coordinate Systems can be thought of as where you are viewing the world from and this serves as a good conceptual model for how they are implemented in this Library. 
    /// Following this model, if you shift based on a CoordinateSystem, the objects will shift in the opposite way because you are moving "yourself" and not the objects when 
    /// you are changing CoordinateSystems. To see this, just look at an object in front of you. If you move left a step, it is perceived the same as if the object moved right 
    /// and you stayed still and this is how the CoordinateSystems work. CoordinateSystems are useful because we can switch easily to a different view for a certain
    /// object to see how it relates easier without losing how it is related to the rest of the objects. These can be used to simplify calculations from 3D to 2D and make them
    /// much easier to preform by shifting to a CoordinateSystem in which the calculations done will have no component in one of the axis directions. Using CoordinateSystems this 
    /// way is the same idea of reference frames (http://en.wikipedia.org/wiki/Frame_of_reference) and is actually a limited application of reference frames if that helps with 
    /// conceptualizing them and their power.
    /// 
    /// Another concept that is used in this class is of a "WorldCoordinateSystem." This is an abitrary CoordinateSystem that is used as the basis and reference for all the
    /// CoordinateSystems. Since it is abitrary and the reference, it's components will always be "zero" no matter where it is placed. This is the same as when you are graphing
    /// a line for instance. You draw a X-Axis and an Y-Axis, which positions are abitrary, and then draw the line based on those axis.
    /// 
    /// 3D CoordinateSystems have two main parts: an origin and a set of axes. The origin is usually represented by a point in 3D space based on the WorldCoordinateSystem (the reference 
    /// CoordinateSystem), but the axes can be represented in many different ways. The way we store the axes in this class is with 3 angles to rotate around each of the 
    /// WorldCoordinateSytem's axes in X-Y-Z order, which as also refered to as euler angles(http://en.wikipedia.org/wiki/Euler_angles), but we do not limit beta to [0, pi] 
    /// (http://en.wikipedia.org/wiki/Euler_angles#Signs_and_ranges). Both of these are stored relative to the WorldCoordinates and so, in a sense, the World Coordinates are 
    /// self defining. Note that the order we rotate around the axis IS important! If you do the same angles with Z-Y-X rotation, it most likely will result in a different 
    /// orientation with respect to the WorldCoordinateSystem. Also, we rotate the angles around the WorldCoordinateSystem, meaning this is an extrinsic rotation approach to 
    /// CoordinateSystems (http://en.wikipedia.org/wiki/Euler_angles#Extrinsic_rotations). We can store the axis as angles instead of lines because we make two assumptions that
    /// are alway held in this class about the CoordinateSystems. First, it is a cartesian Coordinate System meaning that each axis is seperated from the other two by 90 degrees 
    /// and the axes are formed by the intersection of 3 planes(http://en.wikipedia.org/wiki/Coordinate_system#Cartesian_coordinate_system). Secondly, they follow the right hand 
    /// rule for determining how the axes relate to each other(http://en.wikipedia.org/wiki/Right-hand_rule#Coordinate_orientation). 
    /// 
    /// Another thing to make note of is that Euler Angle, which we use to store the CoordinateSystems axes with, do NOT represent a unique orientation. Many different 
    /// combinations of euler angle can lead to the same orientation based on the WorldCoordinateSystem. This is partly due to not limiting any of the angles from [0, pi] 
    /// instead of [-pi, pi], but there are still ambiguous cases I belive even with the limit, but it is easier to leave them unlimited and then represent the orientation in 
    /// a different form (we use Quaternions: http://en.wikipedia.org/wiki/Quaternion) to determine if they are the same orientation.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public partial class CoordinateSystem
    {
        #region Properties and Fields

        /// <summary>
        /// The world coordinate system
        /// </summary>
        public readonly static CoordinateSystem WorldCoordinateSystem = new CoordinateSystem(Point.Origin, Angle.Zero,Angle.Zero,Angle.Zero);

        [JsonProperty]
        public Shift ShiftFromThisToWorld { get; set; }
       
        public Point TranslationToOrigin
        {
            get
            {
                return Matrix.ShiftPoint(Point.Origin, ShiftFromThisToWorld.Matrix);
            }
        }

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="passedOrigin">The origin point of this coordinate system in reference to the world coordinate system</param>
        public CoordinateSystem(Plane planeContainingTwoOfTheAxes, Vector axisInPassedPlaneToUseAsBase,
            Enums.Axis whichAxisIsPassed = Enums.Axis.X, Enums.AxisPlanes whichAxisPlaneIsPassed = Enums.AxisPlanes.XYPlane)
        {

            //make sure the passed vector is in our plane
            if (!planeContainingTwoOfTheAxes.Contains(axisInPassedPlaneToUseAsBase))
            {
                throw new ArgumentOutOfRangeException("the passed axis was not in the plane");
            }

            Vector xAxis = new Vector(Point.Origin);
            Vector yAxis = new Vector(Point.Origin);
            Vector zAxis = new Vector(Point.Origin);

            //Vector normalAxis = new Vector(axisInPassedPlaneToUseAsBase.BasePoint, planeContainingTwoOfTheAxes.NormalVector.Direction, new Distance(new Inch(), 1));

            //Vector otherAxis;

            switch (whichAxisPlaneIsPassed)
            {
                case Enums.AxisPlanes.XYPlane:
                    zAxis = new Vector(axisInPassedPlaneToUseAsBase.BasePoint, planeContainingTwoOfTheAxes.NormalVector.Direction, new Distance(new Inch(), 1));
                    switch (whichAxisIsPassed)
                    {
                        //if its the x we were given the one we calculate is the y, so we want it 90 degrees (to the left)
                        case Enums.Axis.X:
                            xAxis = axisInPassedPlaneToUseAsBase;
                            yAxis = zAxis.CrossProduct(xAxis);
                            //otherAxis = axisInPassedPlaneToUseAsBase.Rotate(new Rotation(planeContainingTwoOfTheAxes.NormalVector, new Angle(new Degree(), 90)));
                            break;
                        //if its the y we were the the y axis and we need to calculate the x which will be -90 degrees (to the right)
                        case Enums.Axis.Y:
                            yAxis = axisInPassedPlaneToUseAsBase;
                            xAxis = yAxis.CrossProduct(zAxis);
                            //otherAxis = axisInPassedPlaneToUseAsBase.Rotate(new Rotation(planeContainingTwoOfTheAxes.NormalVector, new Angle(new Degree(), -90)));
                            break;
                        //the axis must be in the plane we were passed
                        case Enums.Axis.Z:
                            throw new ArgumentOutOfRangeException("the passed axis type was not in the plane type");
                    }
                    break;
                case Enums.AxisPlanes.XZPlane:
                    yAxis = new Vector(axisInPassedPlaneToUseAsBase.BasePoint, planeContainingTwoOfTheAxes.NormalVector.Direction, new Distance(new Inch(), 1));
                    switch (whichAxisIsPassed)
                    {
                        //if its the x we were passed then we need to calculate z, which will be -90 degrees (to the right)
                        case Enums.Axis.X:
                            xAxis = axisInPassedPlaneToUseAsBase;
                            zAxis = xAxis.CrossProduct(yAxis);
                            //otherAxis = axisInPassedPlaneToUseAsBase.Rotate(new Rotation(planeContainingTwoOfTheAxes.NormalVector, new Angle(new Degree(), -90)));
                            break;
                        //the axis must be in the plane we were passed
                        case Enums.Axis.Y:
                            throw new ArgumentOutOfRangeException("the passed axis type was not in the plane type");
                        //if its the z we were passed then we need to calculate x, which will be 90 degrees (to the left)
                        case Enums.Axis.Z:
                            zAxis = axisInPassedPlaneToUseAsBase;
                            xAxis = yAxis.CrossProduct(zAxis);
                            //otherAxis = axisInPassedPlaneToUseAsBase.Rotate(new Rotation(planeContainingTwoOfTheAxes.NormalVector, new Angle(new Degree(), 90)));
                            break;
                    }
                    break;
                case Enums.AxisPlanes.YZPlane:
                    xAxis = new Vector(axisInPassedPlaneToUseAsBase.BasePoint, planeContainingTwoOfTheAxes.NormalVector.Direction, new Distance(new Inch(), 1));
                    switch (whichAxisIsPassed)
                    {
                        //the axis must be in the plane we were passed
                        case Enums.Axis.X:
                            throw new ArgumentOutOfRangeException("the passed axis type was not in the plane type");
                        //if it is the Y axis then we need to find the z, which is 90 degrees (to the left)
                        case Enums.Axis.Y:
                            yAxis = axisInPassedPlaneToUseAsBase;
                            zAxis = xAxis.CrossProduct(yAxis);
                            //otherAxis = axisInPassedPlaneToUseAsBase.Rotate(new Rotation(planeContainingTwoOfTheAxes.NormalVector, new Angle(new Degree(), 90)));
                            break;
                        //if it is the Z axis then we need to find the y, which is -90 degrees (to the right)
                        case Enums.Axis.Z:
                            zAxis = axisInPassedPlaneToUseAsBase;
                            yAxis = zAxis.CrossProduct(xAxis);
                            //otherAxis = axisInPassedPlaneToUseAsBase.Rotate(new Rotation(planeContainingTwoOfTheAxes.NormalVector, new Angle(new Degree(), -90)));
                            break;
                    }
                    break;
            }

            //we found our axes, now we can determine the angles from them
            //Since we rotate in the order Z, X, then Y, we must find the angles in the reverse order
            //i.e. y first, then x then z

            //if we find line up the z axis in the YZ plane with the y rotation, then we can rotate it around the x axis to make the z axes line up
            //and then we can z rotate to make the x and y coincide with the origins

            //first make them into unitvectors to simplify the calculations
            xAxis = xAxis.Direction*Inch;
            yAxis = yAxis.Direction*Inch;
            zAxis = zAxis.Direction*Inch;

            //now first find out the amount we need to rotate around the y axis to line up z in the yz plane

            //First project the z axis onto the xz plane
            Line projectedZAxis = ((Line)zAxis).ProjectOntoPlane(new Plane(Line.XAxis, Line.ZAxis));

            //then use the projected Line to find out how far we need to rotate in the Y direction to line up the z axes in the YZplane
            Angle angleBetweenCurrentZAndYZPlane = projectedZAxis.Direction.Theta;

            //if the projection is in the negative x direction we need to rotate negitively(clockwise) instead of positivly
            if (projectedZAxis.Direction.XComponent > 0)
            {
                angleBetweenCurrentZAndYZPlane = angleBetweenCurrentZAndYZPlane.Negate();
            }

            //http://www.vitutor.com/geometry/distance/line_plane.html
            //we can simplify the equation as this since it is unit vectors
            //sin(angle to plane) = z * planeNormal (which is the x axis by definition)
            //Distance dotProductOfZAndNormal = zAxis * Line.XAxis.UnitVector(new Inch());
            //Angle angleBetweenCurrentZAndYZPlane = new Angle(new Radian(), Math.Asin(dotProductOfZAndNormal.Inches));

            //now rotate the axis (we only need to do z and x since we are done with y now)
            xAxis = xAxis.Rotate(new Rotation(Line.YAxis, angleBetweenCurrentZAndYZPlane));
            zAxis = zAxis.Rotate(new Rotation(Line.YAxis, angleBetweenCurrentZAndYZPlane));

            //now find out how much we need to rotate it in the x direction to line up z in the xz plane (meaning now z will be aligned with the world z)
            Angle angleBetweenZAndZAxis = zAxis.Direction.Theta;

            //now we need to rotate the x axis so we can line it up (the y and z we are done with)
            //if its negative we need to rotate it clockwise (negative) instead of ccw (positive)
            if (zAxis.Direction.YComponent < 0)
            {
                angleBetweenZAndZAxis = angleBetweenZAndZAxis.Negate();
            }

            //finally find out the z rotation needed to line up the x axis with the xz plane (this also forces the y to be lined up)
            xAxis = xAxis.Rotate(new Rotation(Line.XAxis, angleBetweenZAndZAxis));
            Angle angleBetweenXAndXAxis = xAxis.Direction.Phi;

            //now we know all our angles, but we have to take the negative of them because we were transforming back to
            //the origin and we store the tranform from the origin
            var _xAxisRotationAngle = angleBetweenZAndZAxis.Negate();
            var _yAxisRotationAngle = angleBetweenCurrentZAndYZPlane.Negate();
            var _zAxisRotationAngle = angleBetweenXAndXAxis.Negate();

            var rotationX = new Rotation(Line.XAxis, _xAxisRotationAngle);
            var rotationY = new Rotation(Line.YAxis, _yAxisRotationAngle);
            var rotationZ = new Rotation(Line.ZAxis, _zAxisRotationAngle);
            this.ShiftFromThisToWorld = new Shift(new List<Rotation>() { rotationX, rotationY, rotationZ }, axisInPassedPlaneToUseAsBase.BasePoint);
        }

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
