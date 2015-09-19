using System;
using System.Collections.Generic;

namespace GeometryClassLibrary
{
    public class Shift
    {
        #region Implicit Conversions

        public static implicit operator Shift(Rotation r)
        {
            return new Shift(r.Matrix);
        }
        public static implicit operator Shift(Translation t)
        {
            return new Shift(t.Matrix);
        }
        public static implicit operator Shift(Point p)
        {
            return new Shift(p);
        }

        public static implicit operator Shift(Vector v)
        {
            return new Shift(v);
        }

        #endregion

        #region Properties and Fields

        /// <summary>
        /// The _isNegatedShift flag should only be true when .negate() is called on an existing shift 
        /// The reson for this flag is so that we can properly reverse previous shifts in a non-math intensive way.
        /// Basically, when shift is called on an object, it goes down all the way to the point level and then
        /// rotates each point individually one at a time (implemented in Point.Shift function). However, when
        /// we want to undue a shift we need to not only reverse the directions of rotations and translations,
        /// but we must also do them in reverse order so that the translations reverse properly since transformations
        /// (both rotations and translations) are non-communitive, meaning that the order matters. Since the rotations
        /// are already kept in a list, they are easy enough to reverse. The translation is different though because it
        /// stored individually and serperate from the rotation list. Because of this, when we are performing an
        /// original shift we want to perform the translation last, but when we are unduing a shift we must perform the
        /// translation first so the object returns to its original location. If this is not done then the rotation
        /// will return to its original but it will be in the wrong location. 
        /// 
        /// So in the Point.Shift function, it uses this flag in order to determine whether the point needs to be
        /// translated at the very end or the very begining of the shifting processto ensure proper shifts and 
        /// negating shifts
        /// </summary>

        private Matrix _matrix = Matrix.IdentityMatrix(4);
        internal Matrix Matrix { get { return _matrix; } }

       public List<Rotation> RotationsToApply = new List<Rotation>();
        #endregion

        #region Constructors

        /// <summary>
        /// Null Constructor
        /// </summary>
        protected Shift() { }

        /// <summary>
        /// Creates a Shift with the given displacement and no rotations
        /// </summary>
        public Shift(Point displacement)
        {
            _matrix = new Translation(displacement).Matrix;
        }

        public Shift(Vector vector)
            : this(vector.EndPoint-vector.BasePoint) { }

        /// <summary>
        /// Creates a Shift with the given rotation and translation, or zero translation if it is omitted
        /// </summary>
        public Shift(Rotation rotation, Point displacement = null)
        {
            this.RotationsToApply.Add(rotation);
            if (displacement == null)
            {
                this._matrix = rotation.Matrix;
            }
            else
            {
                this._matrix = new Translation(displacement).Matrix
                    * rotation.Matrix;
            }
        }

        /// <summary>
        /// Creates a Shift with multiple Rotations and a displacment, or zero translation if it is omitted
        /// </summary>
        /// <param name="rotations">The rotations that make up and are represented by this shift</param>
        /// <param name="displacement">The distance of displacement this shift represents in each direction</param>
        public Shift(List<Rotation> rotations, Point displacement = null)
        {
            this.RotationsToApply.AddRange(rotations);
            if (displacement == null)
            {
                displacement = Point.Origin;
            }
           
            foreach(var rotation in rotations)
            {
                _matrix = rotation.Matrix * _matrix;
            }
            var translationMatrix = new Translation(displacement).Matrix;
            _matrix = translationMatrix * _matrix;
        }
        private Shift(Matrix matrix)
        {
            this._matrix = matrix;
        }
        /// <summary>
        /// Creates a copy of the given Shift
        /// </summary>
        public Shift(Shift toCopy)
        {
            _matrix = new Matrix(toCopy._matrix);
        }

        #endregion

        #region Overloaded Operators


        /// <summary>
        /// Compose two shift operations.
        ///  Note this is not generally commutative.
        ///The shift on the left is applied first.
        /// </summary>
        public static Shift operator *(Shift s1, Shift s2)
        {
            //Matrix multiplication goes right to left.
            return new Shift(s2.Matrix * s1.Matrix);
        }

        /// <summary>
        /// Not a perfect equality operator, is only accurate up to Constants.AcceptedEqualityDeviationConstant 
        /// </summary>
        public static bool operator ==(Shift shift1, Shift shift2)
        {
            if ((object)shift1 == null)
            {
                if ((object)shift2 == null)
                {
                    return true;
                }
                return false;
            }
            return shift1.Equals(shift2);
        }

        /// <summary>
        /// Not a perfect inequality operator, is only accurate up to Constants.AcceptedEqualityDeviationConstant 
        /// </summary>
        public static bool operator !=(Shift shift1, Shift shift2)
        {
            if ((object)shift1 == null)
            {
                if ((object)shift2 == null)
                {
                    return false;
                }
                return true;
            }
            return !shift1.Equals(shift2);
        }

        /// <summary>
        /// does the same thing as == if the passed in object is a d
        /// </summary>
        public override bool Equals(object obj)
        {
            //make sure the obj is not null
            if (obj == null || !(obj is Shift))
            {
                return false;
            }

            return this._matrix.Equals(((Shift)obj)._matrix);
        }

        /// <summary>
        /// This override determines how this object is inserted into hashtables.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this._matrix.GetHashCode();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            throw new NotImplementedException();
        }


        #endregion

        #region Methods

        public Shift Compose(Shift shift)
        {
            return new Shift(this.Matrix * shift.Matrix);
        }

        /// <summary>
        /// creates a negative instance of the shift object
        /// </summary>
        /// <returns>A new shift object that is negative of this one</returns>
        public Shift Negate()
        {
            return new Shift(_matrix.Invert());
        }
        #endregion
    }
}
