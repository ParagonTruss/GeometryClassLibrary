using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeometryClassLibrary;
using UnitClassLibrary;
using System.Diagnostics;

namespace GeometryClassLibrary
{
    [DebuggerDisplay("Displacement = {this._displacement.Magnitude.Millimeters}, Angle With XZ-Plane = {this._angleWithXZPlane.Degrees} deg, Angle With Z-Axis = {this._angleWithZAxis.Degrees} deg")]
    public class Shift
    {
        private Vector _displacement;
        public Vector Displacement
        {
            get { return _displacement; }
        }

        private Angle _angleWithYZPlane; //polar angle
        public Angle AngleWithYZPlane
        {
            get { return _angleWithYZPlane; }
        }

        private Angle _angleWithXZPlane; //azimuthal angle
        public Angle AngleWithXZPlane
        {
            get { return _angleWithXZPlane; }
        }

       /// <summary>
       /// Converts from a rotation described by 2 angles to a rotation described by 1 angle and an axis
       /// </summary>
        public Line RotationAxis
        {
            //http://math.stackexchange.com/questions/513397/how-can-i-convert-an-axis-angle-representation-to-a-euler-angle-representation
            get
            {
                throw new NotImplementedException();                
                                
            }
            set
            {
                throw new NotImplementedException();
            }
        }


        public Shift(Vector passedDisplacement, Angle passedAngleWithYZPlane, Angle passedAngleWithXZPlane)
        {
            this._displacement = passedDisplacement;
            this._angleWithYZPlane = passedAngleWithYZPlane;
            this._angleWithXZPlane = passedAngleWithXZPlane;
        }

        public Shift()
        {
            this._displacement = new Vector();
            this._angleWithYZPlane = new Angle();
            this._angleWithXZPlane = new Angle();
        }

        public Shift(Vector passedDisplacement, Angle passedRotationAngle)
        {
            this._displacement = passedDisplacement;
            this._angleWithYZPlane = passedRotationAngle;
            this._angleWithXZPlane = new Angle();
        }

        public Shift(Vector passedDisplacement, Angle passedRotationAngle, Line passedRotationAxis)
        {
            this._displacement = passedDisplacement;
            //Calculate Rotation Angle based on axis and passed angle
            //this._rotationAngle = new Angle();

            //Calculate Elevation Angle based on axis and passed angle
            this._angleWithXZPlane = new Angle();
        }

        #region Overloaded Operators

        /* You may notice that we do not overload the increment and decrement operators nor do we overload multiplication and division.
         * This is because the user of this library does not know what is being internally stored and those operations will not return useful information. 
         */

        public static Shift operator +(Shift d1, Shift d2)
        {
            //add the two Shifts together
            //return a new Shift with the new value

            Vector displacement = d1._displacement + d2.Displacement;
            Angle rotationAngle = d1._angleWithYZPlane + d2._angleWithYZPlane;
            Angle elevationAngle = d1._angleWithXZPlane + d2._angleWithXZPlane;

            return new Shift(displacement, rotationAngle, elevationAngle);
        }

        public static Shift operator -(Shift d1, Shift d2)
        {
            //subtract the two Shifts
            //return a new Shift with the new value
            Vector displacement = d1._displacement - d2.Displacement;
            Angle rotationAngle = d1._angleWithYZPlane - d2._angleWithYZPlane;
            Angle elevationAngle = d1._angleWithXZPlane - d2._angleWithXZPlane;

            return new Shift(displacement, rotationAngle, elevationAngle);
        }


        /// <summary>
        /// Not a perfect equality operator, is only accurate up to Constants.AcceptedEqualityDeviationConstant 
        /// </summary>
        public static bool operator ==(Shift d1, Shift d2)
        {
            return d1.Equals(d2);
        }

        /// <summary>
        /// Not a perfect inequality operator, is only accurate up to Constants.AcceptedEqualityDeviationConstant 
        /// </summary>
        public static bool operator !=(Shift d1, Shift d2)
        {
            return !d1.Equals(d2);
        }


        /// <summary>
        /// This override determines how this object is inserted into hashtables.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            List<object> parameters = new List<object> { this.Displacement, this.AngleWithYZPlane, this.AngleWithXZPlane };

            return HashGenerator.GetHashCode(parameters);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// does the same thing as == if the passed in object is a d
        /// </summary>
        public override bool Equals(object obj)
        {
            Shift sh = (Shift)obj;

            return this.Displacement == sh.Displacement &&
                this.AngleWithXZPlane == sh.AngleWithXZPlane &&
                this.AngleWithYZPlane == sh.AngleWithYZPlane;
        }

        #endregion

        #region Methods

        /// <summary>
        /// creates a negative instance of the shift object
        /// </summary>
        /// <returns>Negative shift object</returns>
        public Shift Negate()
        {
            //get negative instances of all of the shift's fields
            Angle returnAngleWithXZPlane = _angleWithXZPlane.Negate();
            Angle returnAngleWithYZPlane = _angleWithYZPlane.Negate();
            Vector returnDisplancement = _displacement.Negate();

            //create and return new shift
            return new Shift(returnDisplancement, returnAngleWithYZPlane, returnAngleWithXZPlane);
        }
        #endregion

        public Vector Vector
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }



    }
}
