using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnitClassLibrary;

namespace GeometryClassLibrary.Vectors
{
    [JsonObject(MemberSerialization.OptIn)]
    public class MeasurementVector : IMeasurementVector
    {
        [JsonProperty]
        public Measurement X { get; private set; }
        [JsonProperty]
        public Measurement Y { get; private set; }
        [JsonProperty]
        public Measurement Z { get; private set; }

        public Measurement Magnitude { get { return this.Magnitude(); } }

        public static MeasurementVector Zero { get { return new MeasurementVector(0.0, 0.0, 0.0); } }

        public MeasurementVector(Measurement x, Measurement y)
        {
            this.X = x;
            this.Y = y;
            this.Z = new Measurement(0.0, 0.0);
        }

        [JsonConstructor]
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

        public MeasurementVector Reverse()
        {
            return new MeasurementVector(X.Negate(), Y.Negate(), Z.Negate());
        }

        public static MeasurementVector operator /(MeasurementVector vector, double divisor)
        {
            return new MeasurementVector(vector.X/divisor, vector.Y/divisor, vector.Z/divisor);
        }
    }
}
