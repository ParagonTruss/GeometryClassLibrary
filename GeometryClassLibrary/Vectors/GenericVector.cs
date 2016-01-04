using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnitClassLibrary;

namespace GeometryClassLibrary.Vectors
{
    [JsonObject(MemberSerialization.OptIn)]
    public class GenericVector : IUnitLessVector
    {
        [JsonProperty]
        public Measurement X { get; private set; }
        [JsonProperty]
        public Measurement Y { get; private set; }
        [JsonProperty]
        public Measurement Z { get; private set; }

        public Measurement Magnitude { get { return this.Magnitude(); } }

        public static GenericVector Zero { get { return new GenericVector(0.0, 0.0, 0.0); } }

        public GenericVector(Measurement x, Measurement y)
        {
            this.X = x;
            this.Y = y;
            this.Z = new Measurement(0.0, 0.0);
        }

        [JsonConstructor]
        public GenericVector(Measurement x, Measurement y, Measurement z)
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

        public GenericVector(Measurement magnitude, Direction direction)
        {
            this.X = magnitude * direction.X;
            this.Y = magnitude * direction.Y;
            this.Z = magnitude * direction.Z;
        }

        public GenericVector Reverse()
        {
            return new GenericVector(X.Negate(), Y.Negate(), Z.Negate());
        }
    }
}
