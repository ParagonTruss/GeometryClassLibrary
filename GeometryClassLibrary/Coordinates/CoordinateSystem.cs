using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnitClassLibrary;

namespace GeometryClassLibrary
{
    /// <summary>
    /// This represents a new coordinated system that is described using the world coordinate system. This class determines 
    /// the translation from the origin to get to the new system as well as the angles about the world's x, y and z axes to 
    /// rotate around in order to get to the desired coordinate system. (executes the rotations: x then y then z)
    /// Note: the "world" coordinate system refers to how we normally percieve the world. The world coordinates are always the same,
    /// but objects can be moved around in it and represent by different sub coordinate systems based on the world ones. This is
    /// how this class works conceptually
    /// Note: because this shift decribes the coordinate system, the effect on objects will be the opposite if those descirbed
    /// for the coordinate system
    /// </summary>
    public class CoordinateSystem
    {
        #region Properties and Fields

        //public static CoordinateSystem CurrentSystem = new CoordinateSystem();

        /// <summary>
        /// The world coordinate System
        /// </summary>
        public readonly static CoordinateSystem WorldCoordinateSystem = new CoordinateSystem();

        /// <summary>
        /// The angle to rotate around the world coordinate system's X axis to get to this coordinate system
        /// </summary>
        public Angle XAngle;

        /// <summary>
        /// The x axis of this coordinate system
        /// </summary>
        public Direction XAxisDirection
        {
            get
            {
                return Line.XAxis.Rotate(this.CoordinateSystemRotations).Direction;
            }
        }

        /// <summary>
        /// The angle to rotate around the world coordinate system's Y axis to get to this coordinate system
        /// </summary>
        public Angle YAngle;

        /// <summary>
        /// The y axis of this coordinate system
        /// </summary>
        public Direction YAxisDirection
        {
            get
            {
                return Line.YAxis.Rotate(this.CoordinateSystemRotations).Direction;
            }
        }

        /// <summary>
        /// The angle to rotate around the world coordinate system's Z axis to get to this coordinate system
        /// </summary>
        public Angle ZAngle;

        /// <summary>
        /// The z axis of this coordinate system
        /// </summary>
        public Direction ZAxisDirection
        {
            get
            {
                return Line.ZAxis.Rotate(this.CoordinateSystemRotations).Direction;
            }
        }

        /// <summary>
        /// This coordinate systems origin point relative to the world coordinate system
        /// </summary>
        public Point Origin;

        /// <summary>
        /// The list of rotations that are applied to the world coordinate system to get the coordinate system to this one
        /// </summary>
        public List<Rotation> CoordinateSystemRotations
        {
            get
            {
                List<Rotation> rotations = new List<Rotation>();

                rotations.Add(new Rotation(Line.ZAxis, this.ZAngle));
                rotations.Add(new Rotation(Line.XAxis, this.XAngle));
                rotations.Add(new Rotation(Line.YAxis, this.YAngle));

                return rotations;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new intance of the world coordinate system
        /// </summary>
        public CoordinateSystem()
        {
            this.Origin = new Point();
            this.XAngle = new Angle();
            this.YAngle = new Angle();
            this.ZAngle = new Angle();
        }

        /// <summary>
        /// Creates a new coordinate system that has the same axis as the world one and is only shifted to the given point
        /// </summary>
        /// <param name="passedOrigin">The origin point of this coordinate system in reference to the world coordinate system</param>
        public CoordinateSystem(Point passedOrigin)
        {
            Origin = new Point(passedOrigin);
            this.XAngle = new Angle();
            this.YAngle = new Angle();
            this.ZAngle = new Angle();
        }

        /// <summary>
        /// Creates a new coordinate system that has the same axis as the world one and is only shifted to the given point
        /// </summary>
        /// <param name="passedOrigin">The origin point of this coordinate system in reference to the world coordinate system</param>
        public CoordinateSystem(Plane planeContainingTwoOfTheAxes, Vector axisInPassedPlaneToUseAsBase, 
            Enums.Axis whichAxisIsPassed = Enums.Axis.X, Enums.AxisPlanes whichAxisPlaneIsPassed = Enums.AxisPlanes.XYPlane)
        {
            //use the base point of the passed axis as the origin point
            Origin = new Point(axisInPassedPlaneToUseAsBase.BasePoint);

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
                angleBetweenCurrentZAndYZPlane = new Angle() - angleBetweenCurrentZAndYZPlane;
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
                angleBetweenZAndZAxis = new Angle() - angleBetweenZAndZAxis;
            }

            //finally find out the z rotation needed to line up the x axis with the xz plane (this also forces the y to be lined up)
            xAxis = xAxis.Rotate(new Rotation(Line.XAxis, angleBetweenZAndZAxis));
            Angle angleBetweenXAndXAxis = xAxis.Direction.Phi;

            //now we know all our angles, but we have to take the negative of them because we were transforming back to
            //the origin and we store the tranform from the origin
            this.ZAngle = new Angle() - angleBetweenXAndXAxis;
            this.XAngle = new Angle() - angleBetweenZAndZAxis;
            this.YAngle = new Angle() - angleBetweenCurrentZAndYZPlane;
        }

        /// <summary>
        /// Creates a new coordinate system with the given origin point and with the given rotations, all in repect to the
        /// world coordinate system
        /// </summary>
        /// <param name="passedOrigin">The origin point of this coordinate system in reference to the world coordinate system</param>
        /// <param name="passedXRotation">The rotation around the world coordinate systems X axis to rotate around to get to this
        /// coordinate system</param>
        /// <param name="passedYRotation">The rotation around the world coordinate systems Y axis to rotate around to get to this
        /// coordinate system</param>
        /// <param name="passedZRotation">The rotation around the world coordinate systems Z axis to rotate around to get to this
        /// coordinate system</param>
        public CoordinateSystem(Point passedOrigin, Angle passedZRotation, Angle passedXRotation, Angle passedYRotation)
        {
            Origin = new Point(passedOrigin);
            this.XAngle = new Angle(passedXRotation);
            this.YAngle = new Angle(passedYRotation);
            this.ZAngle = new Angle(passedZRotation);
        }

        /// <summary>
        /// Creates a copy of the given coordinate system
        /// </summary>
        /// <param name="toCopy">the Coordinate System to copy</param>
        public CoordinateSystem(CoordinateSystem toCopy)
        {
            Origin = new Point(toCopy.Origin);
            this.XAngle = new Angle(toCopy.XAngle);
            this.YAngle = new Angle(toCopy.YAngle);
            this.ZAngle = new Angle(toCopy.ZAngle);
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
            // if the two points' x and y and z are equal, returns true
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

                bool areOriginsEqual = this.Origin == comparableSystem.Origin;

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
        /// Returns this Coordinate System's axes rotations as a single rotation matrix
        /// </summary>
        /// <returns></returns>
        public Matrix GetRotationMatrix()
        {
            return Matrix.RotationMatrixAboutZ(this.ZAngle) * Matrix.RotationMatrixAboutX(this.XAngle) * Matrix.RotationMatrixAboutY(this.YAngle);
        }

        /// <summary>
        /// Determines if the two directions represented by the euler triple (x,y, and z angles) are equivalent 
        /// Note: multiple combinations of euler triples can represent the same orientation
        /// </summary>
        /// <param name="toCheckIfEquivalentTo">The Coordinate System to see if this one is equivalent in direction</param>
        /// <returns>Returns a bool of whether or not the two directions are equivalent</returns>
        public bool AreDirectionsEquivalent(CoordinateSystem toCheckIfEquivalentTo)
        {
            //we can check this by creating the axis and then seeing if they are the same (we actually only need to test two since we
            //always follow the right hand rule)
            bool sameXAxes = this.XAxisDirection == toCheckIfEquivalentTo.XAxisDirection;
            bool sameYAxes = this.YAxisDirection == toCheckIfEquivalentTo.YAxisDirection;

            return sameXAxes && sameYAxes;
        }


        /// <summary>
        /// Returns the shift for this coordinate system to apply to OBJECTS in order to orient them back to the origin
        /// Note: Only works if this is the current shift on the object! if it is already in world coordinates and you 
        /// perform this shift it will move it from the world coordinates to coordinates that are opposite to this one!
        /// </summary>
        /// <returns>Returns the shift to apply to Objects in order to return them from this coordinate system to the world coordinate system</returns>
        public Shift ShiftThatReturnsThisToWorldCoordinateSystem()
        {
            //the simple way is just to create a shift with this coordinate system and then negate it
            return new Shift(this).Negate();
        }

        /// <summary>
        /// Find this coordinate system's (which is currently based on the passed system) shifts relative to the world coordinate 
        /// system instead of the passed coordinate system
        /// </summary>
        /// <param name="passedCoordinateSystem">The coordinate System this coordinate system is currently based on</param>
        /// <returns>Returns a new Coordinate System that reflects this coordinate system based on the world coordinate system 
        /// instead of the passed one</returns>
        public CoordinateSystem FindThisSystemRelativeToWorldSystemCurrentlyRelativeToPassedSystem(CoordinateSystem passedCoordinateSystem)
        {
            CoordinateSystem toReturn = new CoordinateSystem(this);

            //we have to shift our origin point first
            //we need to manually make the shift so that it wont be negated and then translate before rotating
            toReturn.Origin = this.Origin.Shift(new Shift(this.CoordinateSystemRotations, passedCoordinateSystem.Origin));

            //we can just add the rotations
            //convert them to matricies

            //this one - in terms of the passed one
            //we need to invert this one
            Matrix[] thisAnglesMatricies = new Matrix[] {
                Matrix.RotationMatrixAboutZ(this.ZAngle).Invert(),
                Matrix.RotationMatrixAboutX(this.XAngle).Invert(),
                Matrix.RotationMatrixAboutY(this.YAngle).Invert()
            };

            //the passed one - in terms of the world
            Matrix[] passedAnglesMatricies = new Matrix[] {
                Matrix.RotationMatrixAboutZ(passedCoordinateSystem.ZAngle),
                Matrix.RotationMatrixAboutX(passedCoordinateSystem.XAngle),
                Matrix.RotationMatrixAboutY(passedCoordinateSystem.YAngle)
            };

            //multiply them (order is important!)
            Matrix resultingSystem = (thisAnglesMatricies[0] * thisAnglesMatricies[1] * thisAnglesMatricies[2]) * (passedAnglesMatricies[0] *
                passedAnglesMatricies[1] * passedAnglesMatricies[2]);
            //Matrix resultingSystem = (thisAnglesMatricies[0] * passedAnglesMatricies[0]) * (thisAnglesMatricies[1] * passedAnglesMatricies[1]) *
            //    (thisAnglesMatricies[2] * passedAnglesMatricies[2]);

            //then pull out the data
            List<Angle> resultingAngles = resultingSystem.GetAnglesOutOfRotationMatrix();

            toReturn.ZAngle = resultingAngles[0];
            toReturn.XAngle = resultingAngles[1];
            toReturn.YAngle = resultingAngles[2];

            return toReturn;
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

        #endregion
    }
}
