﻿namespace GeometryClassLibrary
{
    public partial class LineSegment
    {
        public int? Polygon_DatabaseId { get; set; }

        public Polygon RelatedPolygon { get; set; }
    }
}