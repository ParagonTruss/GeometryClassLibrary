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
using System.Linq;
using System.Text;
using UnitClassLibrary;
using UnitClassLibrary.DistanceUnit;

namespace GeometryClassLibrary.Vectors
{
    public class MeasurementVector : IMeasurementVector, IRotate<MeasurementVector>
    {
        public static MeasurementVector Zero { get { return new MeasurementVector(0.0, 0.0, 0.0); } }

        #region Local Properties
        public Measurement X { get; }
        public Measurement Y { get; }
        public Measurement Z { get; }

        public Measurement Magnitude { get { return this.Magnitude(); } }

        #endregion

        #region Constructor
        public MeasurementVector(Measurement x, Measurement y)
        {
            this.X = x;
            this.Y = y;
            this.Z = new Measurement(0.0, 0.0);
        }
        
        public MeasurementVector(Measurement x, Measurement y, Measurement z)
        {
            if (Measurement.ErrorPropagationIsEnabled)
            {
                this.X = x;
                this.Y = y;
                this.Z = z;
            }
            else
            {
                this.X = new Measurement(x.Value);
                this.Y = new Measurement(y.Value);
                this.Z = new Measurement(z.Value);
            }
        }

        public MeasurementVector(Measurement magnitude, Direction direction)
        {
            this.X = magnitude * direction.X;
            this.Y = magnitude * direction.Y;
            this.Z = magnitude * direction.Z;
        }
        #endregion

        #region Public Methods
        public MeasurementVector Reverse()
        {
            return new MeasurementVector(X.Negate(), Y.Negate(), Z.Negate());
        }

        public MeasurementVector Rotate(Rotation rotation)
        {
            // update later, but for now:
            var point = new Point(Distance.Inches, X, Y, Z);
            point = point.Rotate3D(rotation);
            return new MeasurementVector(point.X.InInches, point.Y.InInches, point.Z.InInches);
        }


        #endregion

        #region Operator Overloads
        public static MeasurementVector operator /(MeasurementVector vector, double divisor)
        {
            return new MeasurementVector(vector.X/divisor, vector.Y/divisor, vector.Z/divisor);
        }
        #endregion
    }
}
