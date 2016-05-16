using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeometryClassLibrary
{
    /// <summary>
    /// Exception for any scenario wherein a geometric object is to be created but the obhect would have an invalid state.
    /// </summary>
    public class GeometricException : Exception
    {
        public GeometricException()
        {

        }
        public GeometricException(string reason) : base(reason)
        {

        }
    }
}
