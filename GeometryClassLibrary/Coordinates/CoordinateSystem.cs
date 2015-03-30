using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitClassLibrary;

namespace GeometryClassLibrary
{
    /// <summary>
    /// This represents a new coordinate system  (local) that is described using the world coordinate system. This class determines 
    /// the transformation of the original coordinate system which results in the new system.
    /// The transformation has two parts: a translation and a rotation.  The translation moves from the world origin to the local origin.
    /// The rotation is a series of three separate rotations in order, about the x-axis, then the y-axis, then the z-axis.
    /// The rotations are "extrinsic," which means that the are performed about the global axes, not the local axes.
    /// This link is helpful when trying to understand the rotations: http://en.wikipedia.org/wiki/Euler_angles#Intrinsic_rotations
    /// Note: the "world" coordinate system refers to how we normally perceive the world. The world coordinate system is static,
    /// but objects can be moved around in it and represented in different local coordinate systems based on the world system.
    /// </summary>
    public class CoordinateSystem
    {
        #region Properties and Fields

        /// <summary>
        /// The world coordinate system
        /// </summary>
        public readonly static CoordinateSystem WorldCoordinateSystem = new CoordinateSystem();

        /// <summary>
        /// The rotation matrix that describes the local axes' orientation relative to the global axes.
        /// This single rotation is the product of the three separate rotations that follow.
        /// This rotation matrix is equivalent to switching the angles using the shiftFromThisToWorld function
        /// </summary>
        public Matrix RotationMatrix
        {
            get
            {
                //note: we have to multiply "backwards" of the order we rotate in to make this work correctly because how matrix multiplication works
                return Matrix.RotationMatrixAboutZ(ZAxisRotationAngle) * Matrix.RotationMatrixAboutY(YAxisRotationAngle) * Matrix.RotationMatrixAboutX(XAxisRotationAngle);
            }
        }

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
        /// <param name="passedOrigin">The origin point of this coordinate system in reference to the world coordinate system</param>
        public CoordinateSystem(Point passedOrigin)
        {
            _translationToOrigin = new Point(passedOrigin);
            _yAxisRotationAngle = new Angle();
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
        /// <param name="passedOrigin">The origin point of this coordinate system in reference to the world coordinate system</param>
        /// <param name="passedXRotation">The rotation around the world coordinate system's X axis to rotate around to get to this
        /// coordinate system</param>
        /// <param name="passedYRotation">The rotation around the world coordinate system's Y axis to rotate around to get to this
        /// coordinate system</param>
        /// <param name="passedZRotation">The rotation around the world coordinate system's Z axis to rotate around to get to this
        /// coordinate system</param>
        public CoordinateSystem(Point passedOrigin, Angle passedXRotation, Angle passedYRotation, Angle passedZRotation)
        {
            _translationToOrigin = new Point(passedOrigin);
            _xAxisRotationAngle = passedXRotation;
            _yAxisRotationAngle = passedYRotation;
            _zAxisRotationAngle = passedZRotation;
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
        /// Determines if the orientations of the axes of two different coordinate systems are identical. 
        /// Note: a single orientation can be arrived at through several different sets of rotations.  Therefore,
        /// this method simply checks if the overall rotations are the same.
        /// </summary>
        /// <param name="toCheckIfEquivalentTo">The coordinate system we are checking against</param>
        /// <returns>Returns a bool of whether or not the two directions are equivalent</returns>
        public bool AreDirectionsEquivalent(CoordinateSystem toCheckIfEquivalentTo)
        {
            //We have to convert the Matricies we are checking into Quarternions first because they are easier to determine thatn matrices because only
            //two can represent the same orientation, q and -q, which is easy to check for.
            //See: http://gamedev.stackexchange.com/a/75077
            Matrix thisQuaternion = this.RotationMatrix.ConvertRotationMatrixToQuaternion();
            Matrix otherQuaternion = toCheckIfEquivalentTo.RotationMatrix.ConvertRotationMatrixToQuaternion();
            bool areSame = thisQuaternion == otherQuaternion;
            bool areOpposite = thisQuaternion == otherQuaternion * -1;
            return areSame || areOpposite;
        }

        /// <summary>
        /// Returns only the rotation shift for this coordinate system to apply to objects in order to only orient them in the passed system, but not move them.
        /// If the system to rotate to is left out, it rotates it back to the world coordinate System
        /// Note: Only works if this is the current shift on the object! if it is in other coordinates and you 
        /// perform this rotation it will casue incorrect results!
        /// </summary>
        /// <param name="systemToRotateTo">The coordinate system to Rotate from into this coordinate system. defaults to the World Coordinate System if left out</param>
        /// <returns>Returns the shift to apply to Objects in order to Orient them in the same way as the passed system</returns>
        public Shift RotateFromThisTo(CoordinateSystem systemToRotateTo = null)
        {
            Shift shiftTo = ShiftFromThisTo(systemToRotateTo);
            return new Shift(shiftTo.RotationsToApply);
        }

        /// <summary>
        /// Returns only the rotation shift to apply to objects in order to only orient them in this coordinate system when they are currently oriented in the passed system, but does not move them.
        /// If the system to rotate from is left out, it assumes it is oriented in world coordinate System
        /// Note: Only works if the passed coordinate system is the current shift on the object! if it is in other coordinates and you 
        /// perform this rotation it will casue incorrect results!
        /// </summary>
        /// <param name="systemToRotateTo">The coordinate system to Rotate to from this coordinate system. defaults to the World Coordinate System if left out</param>
        /// <returns>Returns the shift to apply to Objects in order to Orient them in this system assuming they are based on the passed system</returns>
        public Shift RotateToThisFrom(CoordinateSystem rotateFrom = null)
        {
            Shift shiftFrom = ShiftToThisFrom(rotateFrom);
            return new Shift(shiftFrom.RotationsToApply);
        }

        /// <summary>
        /// Returns the shift for this coordinate system to apply to objects in order to postition and orient them in the passed system.
        /// If the system to shift to is left out, it shifts it back to the world coordinate System
        /// Note: Only works if this is the current shift on the object! if it is in other coordinates and you 
        /// perform this shift it will give incorrect results!
        /// </summary>
        /// <param name="systemToShiftFrom">The coordinate system to shift to from this coordinate system. defaults to the World Coordinate System if left out</param>
        /// <returns>Returns the shift to apply to Objects in order to shift them from this coordinate system to the passed coordinate system</returns>
        public Shift ShiftFromThisTo(CoordinateSystem systemToShiftTo = null)
        {
            if (systemToShiftTo == null || systemToShiftTo == WorldCoordinateSystem)
            {
                //put the rotations on in the right order (XYZ)
                List<Rotation> rotations = new List<Rotation>();
                rotations.Add(new Rotation(Line.XAxis, this.XAxisRotationAngle));
                rotations.Add(new Rotation(Line.YAxis, this.YAxisRotationAngle));
                rotations.Add(new Rotation(Line.ZAxis, this.ZAxisRotationAngle));

                //Then put the displacement to the origin
                Point displacement = new Point(this.TranslationToOrigin);

                return new Shift(rotations, displacement);
            }
            else
            {
                //the displacements just add(but we do the opposite of shiftTo hence the negative)
                //also we need to the rotate the displacement so that it is in terms of the one we are going to
                Point combinedDisplacement = (this.TranslationToOrigin - systemToShiftTo.TranslationToOrigin).Shift(systemToShiftTo.RotateToThisFrom());
            
                //We need to figure out the equivalent matrix to the two shifts so we multiply them together
                //Note: Order is Improtant!!!
                //Note: the shiftTo is inverted because it normally is how to get to the world coordinate from its coordinates, 
                //      but we want how to get to it from the world coodinates
                Matrix shiftMatrix = (systemToShiftTo.RotationMatrix.Invert()) * this.RotationMatrix;

                //now get the angles out of equivalent rotation matrix
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
        /// Creates a shift that represents going to this coordinate system fromt the passed coordinate system
        /// If the system to shift from is left out, it shifts it back to the world coordinate System
        /// Note: Only works if the passed coordinate system is the current system of the object! if it is in other coordinates and you 
        /// perform this shift it will give incorrect results!
        /// </summary>
        /// <param name="systemToShiftFrom">The coordinate system to shift to this one from. defaults to the World Coordinate System if left out</param>
        /// <returns>Returns a shft to be applied to an obect to put it back to the world coordinate system from this one</returns>
        public Shift ShiftToThisFrom(CoordinateSystem systemToShiftFrom = null)
        {
            //the simple way is just to create a shift with this coordinate system and then negate it
            Shift shift =  ShiftFromThisTo(systemToShiftFrom).Negate();
            return shift;
        }


        //CONTINUE WORKING ON COMMENT CLARITY AND COOMETNING METHODS
        //ALSO MAKE SURE THAT THER IS NO SHIFT CONSTRUCTOR FOR SYSTEMS



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
            //s1 = passed (relative to world)
            //s2 = this (relative to s1)
            //s3 = this relative to world
            //s3 = s2(s1(r1)) + s1(t2) + t1

            //first find the translation
            //r1(t2) + t1 [does both parts - shifts the point and then adds the origin point
            Point newOrigin = this.TranslationToOrigin.Shift(thisRelativeTo.ShiftFromThisTo());

            //now find the resulting rotaions
            //r1(r2)
            //then pull out the angle data from the rotation matrix
            Matrix resultingMatrix = thisRelativeTo.RotationMatrix * this.RotationMatrix;
            List<Angle> resultingAngles = resultingMatrix.GetAnglesOutOfRotationMatrixForXYZRotationOrder();

            //make and return our new system
            return new CoordinateSystem(newOrigin, resultingAngles[0], resultingAngles[1], resultingAngles[2]);
        }

        /*
                /// <summary>
                /// Makes a shift that can be used to move coordinate systems around
                /// </summary>
                /// <returns>Returns a shift to be used on a coordinate systems origin to move it to the corresponding spot</returns>
                public Shift MakeIntoShiftForAnotherCoordinateSystemsOrigin()
                {
                    //we have to make the shift from scratch because
                    Shift systemShift = new Shift();
                    systemShift.RotationsToApply.Add(new Rotation(Line.XAxis, this.XRotation));
                    systemShift.RotationsToApply.Add(new Rotation(Line.YAxis, this.YRotation));
                    systemShift.RotationsToApply.Add(new Rotation(Line.ZAxis, this.ZRotation));

                    systemShift.Displacement = this.Origin;

                    return systemShift;
                }*/


        /// <summary>
        /// This shifts the coordinate system using the given shift
        /// </summary>
        /// <param name="passedShift">The shift to apply to the coordinate system</param>
        /// <returns>Returns a new coordinate system that is shifted by the given shift</returns>
        public CoordinateSystem Shift(Shift passedShift)
        {
            CoordinateSystem toReturn = new CoordinateSystem(this);

            toReturn._translationToOrigin = _translationToOrigin.Shift(passedShift);

            //now we need to shift the angles
            Matrix coordinateMatrix = this.RotationMatrix;

            foreach (Rotation rotation in passedShift.RotationsToApply)
            {
                //make the rotations into a matrix
                Matrix rotationMatrix = Matrix.RotationMatrixAboutAxis(rotation);

                //we need to figure out how to add the angles to get the resulting one
                throw new NotImplementedException();
                //coordinateMatrix = coordinateMatrix.MultiplyBy(rotationMatrix);
            }

            List<Angle> resultAngles = coordinateMatrix.GetAnglesOutOfRotationMatrixForXYZRotationOrder();

            return toReturn;
        }

        #endregion
    }
}
