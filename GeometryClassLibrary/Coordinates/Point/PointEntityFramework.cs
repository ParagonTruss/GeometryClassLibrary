using System;

namespace GeometryClassLibrary
{
    public partial class Point
    {
        public int? DatabaseId { get; set; }

        public int? X_DatabaseId { get; set; }

        public int? Y_DatabaseId { get; set; }

        public int? Z_DatabaseId { get; set; }

        internal Point Translate(object point)
        {
            throw new NotImplementedException();
        }
    }
}
