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

using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace GeometryClassLibrary
{
    [JsonObject(MemberSerialization.OptIn)]
    public partial class Shift
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

        [JsonProperty]
        public Matrix Matrix { get { return _matrix; } }
        private Matrix _matrix = Matrix.IdentityMatrix(4);

        public Translation Translation { get { return new Translation(Point.Origin.Shift(this)); } }
        
        public Rotation RotationAboutOrigin { get { return Rotation.RotationAboutOrigin(this); } }
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
            if (displacement == null)
            {
                this._matrix = rotation.Matrix;
            }
            else
            {
                this._matrix = new Translation(displacement).Matrix * rotation.Matrix;
            }
        }

        /// <summary>
        /// Creates a Shift with multiple Rotations and a displacment, or zero translation if it is omitted
        /// </summary>
        /// <param name="rotations">The rotations that make up and are represented by this shift</param>
        /// <param name="displacement">The distance of displacement this shift represents in each direction</param>
        public Shift(List<Rotation> rotations, Point displacement = null)
        {
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

        [JsonConstructor]
        public Shift(Matrix matrix)
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

        #region Methods

        internal static bool RotationsAreEquivalent(Shift shift1, Shift shift2)
        {
            return shift1.RotationAboutOrigin == shift2.RotationAboutOrigin;
        }

        /// <summary>
        /// Right to left composition.
        /// </summary>
        public static Shift ComposeRightToLeft(Shift second, Shift first)
        {
            return new Shift(second.Matrix * first.Matrix);
        }

        /// <summary>
        /// Left to right composition.
        /// </summary>
        public static Shift ComposeLeftToRight(Shift first, Shift second)
        {
            return new Shift(second.Matrix * first.Matrix);
        }

        /// <summary>
        /// Right to left composition.
        /// </summary>
        [Obsolete("Use ComposeRightToLeft  instead.")]
        public Shift Compose(Shift shift)
        {
            return new Shift(this.Matrix * shift.Matrix);
        }

        /// <summary>
        /// Right to left composition.
        /// </summary>
        [Obsolete("Use ComposeRightToLeft instead.")]
        public static Shift Compose(Shift second, Shift first)
        {
            return new Shift(second.Matrix * first.Matrix);
        }

        /// <summary>
        /// Returns the inverse shift. 
        /// That is, it performs the opposite motion of this shift.
        /// </summary>
        public Shift Inverse()
        {
            return new Shift(Matrix.Invert());
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
            var other = (Shift)obj;
           
            Translation t1 = this.Translation;
            Translation t2 = other.Translation;
            if (t1 != t2)
            {
                return false;
            }

            return Shift.RotationsAreEquivalent(this, other);

        }

        /// <summary>
        /// This override determines how this object is inserted into hashtables.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this._matrix.GetHashCode();
        }

        #endregion

    }
}
