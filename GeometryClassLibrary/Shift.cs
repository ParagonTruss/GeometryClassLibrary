using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using GeometryClassLibrary;
using UnitClassLibrary;
using System.Diagnostics;

namespace GeometryClassLibrary
{
    [DebuggerDisplay("Displacement = {this._displacement.Magnitude.Millimeters}, Angle With XZ-Plane = {this._angleWithXZPlane.Degrees} deg, Angle With Z-Axis = {this._angleWithZAxis.Degrees} deg")]
    public class Shift
    {

        /************************************************************************************************************
         * The _isNegatedShift flag should only be true when .negate() is called on an existing shift
         * 
         * The reson for this flag is so that we can properly reverse previous shifts in a non-math intensive way.
         * Basically, when shift is called on an object, it goes down all the way to the point level and then
         * rotates each point individually one at a time (implemented in Point.Shift function). However, when
         * we want to undue a shift we need to not only reverse the directions of rotations and translations,
         * but we must also do them in reverse order so that the translations reverse properly since transformations
         * (both rotations and translations) are non-communitive, meaning that the order matters. Since the rotations
         * are already kept in a list, they are easy enough to reverse. The translation is different though because it
         * stored individually and serperate from the rotation list. Because of this, when we are performing an
         * original shift we want to perform the translation last, but when we are unduing a shift we must perform the
         * translation first so the object returns to its original location. If this is not done then the rotation
         * will return to its original but it will be in the wrong location. 
         * 
         * So in the Point.Shift function, it uses this flag in order to determine whether the point needs to be
         * translated at the very end or the very begining of the shifting processto ensure proper shifts and 
         * negating shifts
         * **********************************************************************************************************/
        private bool _isNegatedShift = false;
        public bool isNegatedShift
        {
            get { return _isNegatedShift; }
        }


        private Vector _displacement;
        public Vector Displacement
        {
            get { return _displacement; }
        }

        
        private List<Rotation> _rotationsToApply;
        public List<Rotation> rotationsToApply
        {
            get { return _rotationsToApply; }
        }


        /*
        private Angle _angleAboutZAxis; //polar angle (angle from z-axis)
        public Angle AngleAboutZAxis
        {
            get { return _angleAboutZAxis; }
        }

        private Angle _angleAboutXAxis; //azimuthal angle (angle from x-axis in xy-plane)
        public Angle AngleAboutXAxis
        {
            get { return _angleAboutXAxis; }
        }*/

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

        public Shift()
        {
            this._displacement = new Vector();
            this._rotationsToApply = new List<Rotation>();
            //this._angleAboutZAxis = new Angle();
            //this._angleAboutXAxis = new Angle();
        }

        public Shift(Vector passedDisplacement)
        {
            this._displacement = passedDisplacement;
            this._rotationsToApply = new List<Rotation>();
        }

        public Shift(Vector passedDisplacement, Rotation passedRotation)
        {
            this._displacement = passedDisplacement;
            this._rotationsToApply = new List<Rotation>() { passedRotation };
            //this._angleAboutZAxis = passedRotationAngle;
            //this._angleAboutXAxis = new Angle();
        }

        public Shift(Vector passedDisplacement, List<Rotation> passedRotations)
        {
            this._displacement = passedDisplacement;
            this._rotationsToApply = passedRotations;
            //this._angleAboutZAxis = passedAngleWithZAxis;
            //this._angleAboutXAxis = passedAngleWithXZPlane;
        }

        #region Overloaded Operators

        /* You may notice that we do not overload the increment and decrement operators nor do we overload multiplication and division.
         * This is because the user of this library does not know what is being internally stored and those operations will not return useful information. 
         */

        /** For now these dont really make sense
         * public static Shift operator +(Shift d1, Shift d2)
        {
            //add the two Shifts together
            //return a new Shift with the new value

            Vector displacement = d1._displacement + d2.Displacement;
            Angle rotationAngle = d1._angleAboutZAxis + d2._angleAboutZAxis;
            Angle elevationAngle = d1._angleAboutXAxis + d2._angleAboutXAxis;

            return new Shift(displacement, rotationAngle, elevationAngle);
        }

        public static Shift operator -(Shift d1, Shift d2)
        {
            //subtract the two Shifts
            //return a new Shift with the new value
            Vector displacement = d1._displacement - d2.Displacement;
            Angle rotationAngle = d1._angleAboutZAxis - d2._angleAboutZAxis;
            Angle elevationAngle = d1._angleAboutXAxis - d2._angleAboutXAxis;

            return new Shift(displacement, rotationAngle, elevationAngle);
        }*/


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
            List<object> parameters = new List<object> { this.Displacement, this._rotationsToApply };

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


                //think this will compare adressses instead of values


                this._rotationsToApply == sh._rotationsToApply;
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



            //think something has to be done with translating the axis too in order to make sure they get reversed right



            List<Rotation> returnRotations = new List<Rotation>();
            foreach (Rotation rotation in _rotationsToApply)
            {
                //switch the angle of each rotation to its opposite
                 returnRotations.Add(new Rotation(rotation.axisToRotateAround, new Angle() - rotation.angleToRotate));
            }
            //now flip the order of them
            returnRotations.Reverse();

            //now we have to do some magic to turn this back into the right spot since it will happen after the rotations again
            Vector returnDisplancement = _displacement.Negate();

            //create and return new shift
            Shift test = new Shift(returnDisplancement, returnRotations);
            test._isNegatedShift = true;
            return test;
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
