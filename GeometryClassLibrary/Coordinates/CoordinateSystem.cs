﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitClassLibrary;

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
    public class CoordinateSystem
    {
        #region Properties and Fields

        /// <summary>
        /// The world coordinate system
        /// </summary>
        public readonly static CoordinateSystem WorldCoordinateSystem = new CoordinateSystem();

        /// <summary>
        /// The Angle to rotate around the global/external x axis, which is performed first.
        /// </summary>
        public Angle XAxisRotationAngle
        {
            get { return _xAxisRotationAngle; }
        }
        private Angle _xAxisRotationAngle;

        /// <summary>
        /// The Angle to rotate around the global/external y axis, which is performed second.
        /// </summary>
        public Angle YAxisRotationAngle
        {
            get { return _yAxisRotationAngle; }
        }
        private Angle _yAxisRotationAngle;

        /// <summary>
        /// The Angle to rotate around the global/external z axis, which is performed third.
        /// </summary>
        public Angle ZAxisRotationAngle
        {
            get { return _zAxisRotationAngle; }
        }
        private Angle _zAxisRotationAngle;

        /// <summary>
        /// The translation from the world coordinate system's origin to the local coordinate system's origin
        /// </summary>
        public Point TranslationToOrigin
        {
            get { return _translationToOrigin; }
        }
        private Point _translationToOrigin;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the world coordinate system
        /// </summary>
        public CoordinateSystem()
        {
            _translationToOrigin = new Point();
            _xAxisRotationAngle = new Angle();
            _yAxisRotationAngle = new Angle();
            _zAxisRotationAngle = new Angle();
        }

        /// <summary>
        /// Creates a local coordinate system that has the same axes as the world coordinate system and has only been shifted to the given point
        /// </summary>
        /// <param name="passedTranslationToOrigin">The origin point of this coordinate system in reference to the world coordinate system</param>
        public CoordinateSystem(Point passedTranslationToOrigin)
        {
            _translationToOrigin = new Point(passedTranslationToOrigin);
            _xAxisRotationAngle = new Angle();
            _yAxisRotationAngle = new Angle();
            _zAxisRotationAngle = new Angle();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="passedOrigin">The origin point of this coordinate system in reference to the world coordinate system</param>
        public CoordinateSystem(Plane planeContainingTwoOfTheAxes, Vector axisInPassedPlaneToUseAsBase,
            Enums.Axis whichAxisIsPassed = Enums.Axis.X, Enums.AxisPlanes whichAxisPlaneIsPassed = Enums.AxisPlanes.XYPlane)
        {
            //use the base point of the passed axis as the origin point
            _translationToOrigin = new Point(axisInPassedPlaneToUseAsBase.BasePoint);

            //make sure the passed vector is in our plane
            if (!planeContainingTwoOfTheAxes.Contains(axisInPassedPlaneToUseAsBase))
            {
                throw new ArgumentOutOfRangeException("the passed axis was not in the plane");
            }

            Vector xAxis = new Vector();
            Vector yAxis = new Vector();
            Vector zAxis = new Vector();

            //Vector normalAxis = new Vector(axisInPassedPlaneToUseAsBase.BasePoint, planeContainingTwoOfTheAxes.NormalVector.Direction, new Distance(DistanceType.Inch, 1));

            //Vector otherAxis;

            switch (whichAxisPlaneIsPassed)
            {
                case Enums.AxisPlanes.XYPlane:
                    zAxis = new Vector(axisInPassedPlaneToUseAsBase.BasePoint, planeContainingTwoOfTheAxes.NormalVector.Direction, new Distance(DistanceType.Inch, 1));
                    switch (whichAxisIsPassed)
                    {
                        //if its the x we were given the one we calculate is the y, so we want it 90 degrees (to the left)
                        case Enums.Axis.X:
                            xAxis = axisInPassedPlaneToUseAsBase;
                            yAxis = zAxis.CrossProduct(xAxis);
                            //otherAxis = axisInPassedPlaneToUseAsBase.Rotate(new Rotation(planeContainingTwoOfTheAxes.NormalVector, new Angle(AngleType.Degree, 90)));
                            break;
                        //if its the y we were the the y axis and we need to calculate the x which will be -90 degrees (to the right)
                        case Enums.Axis.Y:
                            yAxis = axisInPassedPlaneToUseAsBase;
                            xAxis = yAxis.CrossProduct(zAxis);
                            //otherAxis = axisInPassedPlaneToUseAsBase.Rotate(new Rotation(planeContainingTwoOfTheAxes.NormalVector, new Angle(AngleType.Degree, -90)));
                            break;
                        //the axis must be in the plane we were passed
                        case Enums.Axis.Z:
                            throw new ArgumentOutOfRangeException("the passed axis type was not in the plane type");
                    }
                    break;
                case Enums.AxisPlanes.XZPlane:
                    yAxis = new Vector(axisInPassedPlaneToUseAsBase.BasePoint, planeContainingTwoOfTheAxes.NormalVector.Direction, new Distance(DistanceType.Inch, 1));
                    switch (whichAxisIsPassed)
                    {
                        //if its the x we were passed then we need to calculate z, which will be -90 degrees (to the right)
                        case Enums.Axis.X:
                            xAxis = axisInPassedPlaneToUseAsBase;
                            zAxis = xAxis.CrossProduct(yAxis);
                            //otherAxis = axisInPassedPlaneToUseAsBase.Rotate(new Rotation(planeContainingTwoOfTheAxes.NormalVector, new Angle(AngleType.Degree, -90)));
                            break;
                        //the axis must be in the plane we were passed
                        case Enums.Axis.Y:
                            throw new ArgumentOutOfRangeException("the passed axis type was not in the plane type");
                        //if its the z we were passed then we need to calculate x, which will be 90 degrees (to the left)
                        case Enums.Axis.Z:
                            zAxis = axisInPassedPlaneToUseAsBase;
                            xAxis = yAxis.CrossProduct(zAxis);
                            //otherAxis = axisInPassedPlaneToUseAsBase.Rotate(new Rotation(planeContainingTwoOfTheAxes.NormalVector, new Angle(AngleType.Degree, 90)));
                            break;
                    }
                    break;
                case Enums.AxisPlanes.YZPlane:
                    xAxis = new Vector(axisInPassedPlaneToUseAsBase.BasePoint, planeContainingTwoOfTheAxes.NormalVector.Direction, new Distance(DistanceType.Inch, 1));
                    switch (whichAxisIsPassed)
                    {
                        //the axis must be in the plane we were passed
                        case Enums.Axis.X:
                            throw new ArgumentOutOfRangeException("the passed axis type was not in the plane type");
                        //if it is the Y axis then we need to find the z, which is 90 degrees (to the left)
                        case Enums.Axis.Y:
                            yAxis = axisInPassedPlaneToUseAsBase;
                            zAxis = xAxis.CrossProduct(yAxis);
                            //otherAxis = axisInPassedPlaneToUseAsBase.Rotate(new Rotation(planeContainingTwoOfTheAxes.NormalVector, new Angle(AngleType.Degree, 90)));
                            break;
                        //if it is the Z axis then we need to find the y, which is -90 degrees (to the right)
                        case Enums.Axis.Z:
                            zAxis = axisInPassedPlaneToUseAsBase;
                            yAxis = zAxis.CrossProduct(xAxis);
                            //otherAxis = axisInPassedPlaneToUseAsBase.Rotate(new Rotation(planeContainingTwoOfTheAxes.NormalVector, new Angle(AngleType.Degree, -90)));
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
            xAxis = xAxis.Direction.UnitVector(DistanceType.Inch);
            yAxis = yAxis.Direction.UnitVector(DistanceType.Inch);
            zAxis = zAxis.Direction.UnitVector(DistanceType.Inch);

            //now first find out the amount we need to rotate around the y axis to line up z in the yz plane

            //First project the z axis onto the xz plane
            Line projectedZAxis = ((Line)zAxis).ProjectOntoPlane(new Plane(Line.XAxis, Line.ZAxis));

            //then use the projected Line to find out how far we need to rotate in the Y direction to line up the z axes in the YZplane
            Angle angleBetweenCurrentZAndYZPlane = projectedZAxis.Direction.Theta;

            //if the projection is in the negative x direction we need to rotate negitively(clockwise) instead of positivly
            if (projectedZAxis.Direction.XComponentOfDirection > 0)
            {
                angleBetweenCurrentZAndYZPlane = angleBetweenCurrentZAndYZPlane.Negate();
            }

            //http://www.vitutor.com/geometry/distance/line_plane.html
            //we can simplify the equation as this since it is unit vectors
            //sin(angle to plane) = z * planeNormal (which is the x axis by definition)
            //Distance dotProductOfZAndNormal = zAxis * Line.XAxis.UnitVector(DistanceType.Inch);
            //Angle angleBetweenCurrentZAndYZPlane = new Angle(AngleType.Radian, Math.Asin(dotProductOfZAndNormal.Inches));

            //now rotate the axis (we only need to do z and x since we are done with y now)
            xAxis = xAxis.Rotate(new Rotation(Line.YAxis, angleBetweenCurrentZAndYZPlane));
            zAxis = zAxis.Rotate(new Rotation(Line.YAxis, angleBetweenCurrentZAndYZPlane));

            //now find out how much we need to rotate it in the x direction to line up z in the xz plane (meaning now z will be aligned with the world z)
            Angle angleBetweenZAndZAxis = zAxis.Direction.Theta;

            //now we need to rotate the x axis so we can line it up (the y and z we are done with)
            //if its negative we need to rotate it clockwise (negative) instead of ccw (positive)
            if (zAxis.Direction.YComponentOfDirection < 0)
            {
                angleBetweenZAndZAxis = angleBetweenZAndZAxis.Negate();
            }

            //finally find out the z rotation needed to line up the x axis with the xz plane (this also forces the y to be lined up)
            xAxis = xAxis.Rotate(new Rotation(Line.XAxis, angleBetweenZAndZAxis));
            Angle angleBetweenXAndXAxis = xAxis.Direction.Phi;

            //now we know all our angles, but we have to take the negative of them because we were transforming back to
            //the origin and we store the tranform from the origin
            _xAxisRotationAngle = angleBetweenZAndZAxis.Negate();
            _yAxisRotationAngle = angleBetweenCurrentZAndYZPlane.Negate();
            _zAxisRotationAngle = angleBetweenXAndXAxis.Negate();
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
        public CoordinateSystem(Point passedTranslationToOrigin, Angle passedXAxisRotation, Angle passedYAxisRotation, Angle passedZAxisRotation)
        {
            _translationToOrigin = new Point(passedTranslationToOrigin);
            _xAxisRotationAngle = new Angle(passedXAxisRotation);
            _yAxisRotationAngle = new Angle(passedYAxisRotation);
            _zAxisRotationAngle =new Angle(passedZAxisRotation);
        }

        /// <summary>
        /// Creates a copy of the given coordinate system
        /// </summary>
        /// <param name="toCopy">the Coordinate System to copy</param>
        public CoordinateSystem(CoordinateSystem toCopy)
        {
            _translationToOrigin = new Point(toCopy.TranslationToOrigin);
            _xAxisRotationAngle = new Angle(toCopy.XAxisRotationAngle);
            _yAxisRotationAngle = new Angle(toCopy.YAxisRotationAngle);
            _zAxisRotationAngle = new Angle(toCopy.ZAxisRotationAngle);
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
            if (obj == null)
            {
                return false;
            }

            //try to cast the object to a coordinate System, if it fails then we know the user passed in the wrong type of object
            try
            {
                CoordinateSystem comparableSystem = (CoordinateSystem)obj;

                bool areOriginsEqual = _translationToOrigin == comparableSystem.TranslationToOrigin;

                return areOriginsEqual && this.AreDirectionsEquivalent(comparableSystem);
            }
            //if they are not the same type than they are not equal
            catch (InvalidCastException)
            {
                return false;
            }
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
            //note: we have to multiply "backwards" of the order we rotate in to make this work correctly because how matrix multiplication works
            return Matrix.RotationMatrixAboutZ(ZAxisRotationAngle) * Matrix.RotationMatrixAboutY(YAxisRotationAngle) * Matrix.RotationMatrixAboutX(XAxisRotationAngle);
        }

        /// <summary>
        /// The rotation matrix that describes the local axes' orientation relative to the global axes.
        /// This single rotation is the product of the three separate rotations that follow.
        /// This rotation matrix is equivalent to switching the angles using the rotateToThisFrom(WorldCoordinateSystem) function
        /// </summary>
        public Matrix RotationMatrixToThisFromWorld()
        {
            //note: we reverse the matrix by inverting it see:http://en.wikipedia.org/wiki/Rotation_matrix#Ambiguities (the part on alias or alibi)
            //non inverted is like rotating the point and inverted is like rotating the axes
            return this.RotationMatrixFromThisToWorld().Invert();
        }

        /// <summary>
        /// Determines if the orientations of the axes of two different coordinate systems are identical. 
        /// Note: a single orientation can be arrived at through several different sets of rotations.  Therefore,
        /// this method simply checks if the overall rotations are the same.
        /// </summary>
        /// <param name="toCheckIfEquivalentTo">The coordinate system we are checking against</param>
        /// <returns>Returns a bool of whether or not the two directions are equivalent</returns>
        public bool AreDirectionsEquivalent(CoordinateSystem toCheckIfEquivalentTo)
        {
            //We have to convert the Matricies we are checking into Quarternions first because their orientations are easier to determine than matrices because only
            //two Quaternion can represent the same orientation, q and -q, which is easy to check for.
            //See: http://gamedev.stackexchange.com/a/75077
            Matrix thisQuaternion = this.RotationMatrixFromThisToWorld().ConvertRotationMatrixToQuaternion();
            Matrix otherQuaternion = toCheckIfEquivalentTo.RotationMatrixFromThisToWorld().ConvertRotationMatrixToQuaternion();

            bool areSame = thisQuaternion == otherQuaternion;
            bool areOpposite = thisQuaternion == otherQuaternion * -1;
            return areSame || areOpposite; //since q == q && q == -q for Quaternion
        }

        /// <summary>
        /// Returns only the rotational Shift for this CoordinateSystem to apply to objects in order to only orient them in the passed CoordinateSystem, but not move them.
        /// If the CoordinateSystem to rotate to is left out, it rotates it to the WorldCoordinateSystem
        /// Note: Only works if this CoordinateSystem is the current shift on the object! if it is in another CoordinateSystem and you 
        /// perform this rotation it will casue incorrect results!
        /// </summary>
        /// <param name="systemToRotateTo">The CoordinateSystem to Rotate to from this CoordinateSystem. defaults to the WorldCoordinateSystem if left out</param>
        /// <returns>Returns the Shift to apply to objects oriented in this CoordinateSystem in order to orient them in the same way as the passed CoordinateSystem</returns>
        public Shift RotateFromThisTo(CoordinateSystem systemToRotateTo = null)
        {
            //find the whole shift but then make a new one with only the rotations
            Shift shiftTo = ShiftFromThisTo(systemToRotateTo);
            return new Shift(shiftTo.RotationsToApply);
        }

        /// <summary>
        /// Returns only the rotational Shift to apply to objects in order to only orient them in this CoordinateSystem when they are currently oriented in the passed CoordinateSystem, but does not move them.
        /// If the CoordinateSystem to rotate from is left out, it assumes it is currently oriented in WorldCoordinateSystem
        /// Note: Only works if the passed CoordinateSystem is the current shift on the object! if it is in another CoordinateSystem and you 
        /// perform this rotation it will casue incorrect results!
        /// </summary>
        /// <param name="systemToRotateFrom">The CoordinateSystem to Rotate from to this CoordinateSystem. defaults to the WorldCoordinateSystem if left out</param>
        /// <returns>Returns the Shift to apply to objects in order to orient them in this CoordinateSystem assuming they are oriented currently on the passed CoordinateSystem</returns>
        public Shift RotateToThisFrom(CoordinateSystem systemToRotateFrom = null)
        {
            //find the whole shift but then make a new one with only the rotations
            Shift shiftFrom = ShiftToThisFrom(systemToRotateFrom);
            return new Shift(shiftFrom.RotationsToApply);
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
            //if its the world coordinates then just make the shift directly from the coordinate system since it is stored in terms of the world coordinates
            if (systemToShiftTo == null || systemToShiftTo == CoordinateSystem.WorldCoordinateSystem)
            {
                //put the rotations on in the right order (XYZ)
                List<Rotation> rotations = new List<Rotation>();
                rotations.Add(new Rotation(Line.XAxis, this.XAxisRotationAngle));
                rotations.Add(new Rotation(Line.YAxis, this.YAxisRotationAngle));
                rotations.Add(new Rotation(Line.ZAxis, this.ZAxisRotationAngle));

                //Then put the displacement to the origin
                Point displacement = new Point(this.TranslationToOrigin);

                //make and return the shift
                return new Shift(rotations, displacement);
            }
            //If not then we need to figure out how to shift between the two coordinates
            else
            {
                //the displacements just subtract since we want the passed system realtive to this system, but this is relative to the world coordinates
                //so we have to rotate the result into this coordinates since we are shifting relative to this coordiantes
                Point combinedDisplacement = (this.TranslationToOrigin - systemToShiftTo.TranslationToOrigin).Shift(systemToShiftTo.RotateToThisFrom());

                //We need to figure out the equivalent matrix to the two shifts so we multiply them together
                //Order is Important! (the first in multiplication order is the shift performed second and vice versa!)
                //this rotation matrix is second because we have to find the equivalent of first shifting to the world from this coordinates and then again to the passed coordinates
                Matrix shiftMatrix = systemToShiftTo.RotationMatrixToThisFromWorld() * this.RotationMatrixFromThisToWorld();

                //now get the angles out of equivalent rotation matrix and put them into a rotation list for the shift
                List<Angle> shiftAngles = shiftMatrix.GetAnglesOutOfRotationMatrixForXYZRotationOrder();
                List<Rotation> shiftRotations = new List<Rotation>();
                shiftRotations.Add(new Rotation(Line.XAxis, shiftAngles[0]));
                shiftRotations.Add(new Rotation(Line.YAxis, shiftAngles[1]));
                shiftRotations.Add(new Rotation(Line.ZAxis, shiftAngles[2]));

                //and then make and return the new shift
                return new Shift(shiftRotations, combinedDisplacement);
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
            //the simple way is just to create a shift with this coordinate system and then negate it
            Shift shift = ShiftFromThisTo(systemToShiftFrom).Negate();
            return shift;
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
            //coordinate system equations
            //1 = passed (relative to world)
            //2 = this (relative to s1)
            //3 = this relative to world

            //sys3 = rot1(rot2) + rot1(origin2) + origin1

            //first find the translation, which is the passedCordinates origin + this origin rotated from the passedSystem to the world system so it is in terms of the world system
            //rot1(origin2) + origin1 [does both parts - shifts the point and then adds the origin point]
            Point newOrigin = this.TranslationToOrigin.Shift(thisRelativeTo.ShiftFromThisTo());

            //now find the resulting rotaions and then pull out the angle data from the rotation matrix
            //Note order of matrix multiplication is important! (the first in multiplication order is the shift performed second and vice versa!)
            //rot1(rot2)
            Matrix resultingMatrix = thisRelativeTo.RotationMatrixFromThisToWorld() * this.RotationMatrixFromThisToWorld();
            List<Angle> resultingAngles = resultingMatrix.GetAnglesOutOfRotationMatrixForXYZRotationOrder();

            //make and return our new system
            return new CoordinateSystem(newOrigin, resultingAngles[0], resultingAngles[1], resultingAngles[2]);
        }

        /// <summary>
        /// This shifts the coordinate system relative to the world system with the given shift
        /// </summary>
        /// <param name="passedShift">The shift to apply to the coordinate system</param>
        /// <returns>Returns a new coordinate system that is shifted by the given shift</returns>
        public CoordinateSystem Shift(Shift passedShift)
        {
            //find the relative displacement by adding the displacement to this coordinate system's translation to origin (this is relative to WoorldCoordinates though)
            Point displacementInCurrent = this.TranslationToOrigin.Shift(passedShift);

            //Get the matrix representation of the coordinate system to start with as a base
            Matrix cumulativeMatrix = this.RotationMatrixFromThisToWorld();

            //for each of our rotations we need to figure out how that affects our resulting/cumulative matrix
            foreach (Rotation rotation in passedShift.RotationsToApply)
            {
                //make the rotations into a matrix
                Matrix rotationMatrix = Matrix.RotationMatrixAboutAxis(rotation);

                //Multiply the new matrix by the cumulative one we have already
                //Order is Important! (the first in multiplication order is the shift performed second and vice versa!)
                cumulativeMatrix = rotationMatrix * cumulativeMatrix;
            }

            //get the angles out of our final matrx
            List<Angle> resultAngles = cumulativeMatrix.GetAnglesOutOfRotationMatrixForXYZRotationOrder();

            //create the coordinate system relative to the passed current one
            return new CoordinateSystem(displacementInCurrent, resultAngles[0], resultAngles[1], resultAngles[2]);
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
            //change the coordinate system so that it is in terms of the current system so we can shift it relative to that system and 
            //then we need to shift it so its back on the worldCoordinates
            CoordinateSystem inCurrent = this.Shift(CoordinateSystem.WorldCoordinateSystem.ShiftFromThisTo(systemShiftIsRelativeTo));
            CoordinateSystem shiftedInCurrent = inCurrent.Shift(passedShift);
            //now put it back in terms of the world and return it
            return shiftedInCurrent.Shift(CoordinateSystem.WorldCoordinateSystem.ShiftToThisFrom(systemShiftIsRelativeTo));
        }

        #endregion
    }
}
