using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeometryClassLibrary;
using System.Diagnostics;

namespace GeometryClassLibrary
{
    [DebuggerDisplay("Base Point = {BasePoint}")]
    public class Polyhedron : Solid
    {
        /// <summary>
        /// Accesses a point on the solid 
        /// </summary>
        public Point PointOnSolid
        {
            get
            {
                Point returnPoint = null;
                if (_polygons.Count > 0)
                {
                    returnPoint = _polygons[0].PlaneBoundaries[0].BasePoint;
                }
                return returnPoint;
            }
        }


        public Point CenterPoint()
        {
            throw new NotImplementedException();
        }

        public Line MidLine()
        {
            throw new NotImplementedException();
        }

        public List<Polygon> Polygons
        {
            get { return _polygons; }
            set { _polygons = value; }
        }
        private List<Polygon> _polygons;

        public Polyhedron()
        {
            _polygons = new List<Polygon>();
        }

        public Polyhedron(IEnumerable<LineSegment> passedLineSegments)
        {
            _polygons = passedLineSegments.MakeCoplanarLineSegmentsIntoRegions();
        }

        public Polyhedron(IEnumerable<Polygon> passedPolygons)
        {
            _polygons = new List<Polygon>(passedPolygons);
        }

        public Polyhedron(Polyhedron passedSolid)
        {
            _polygons = passedSolid._polygons;
        }

        public List<LineSegment> LineSegments
        {
            get
            {
                List<LineSegment> returnList = new List<LineSegment>();

                foreach (Polygon region in _polygons)
                {
                    if (region.PlaneBoundaries != null)
                        returnList.AddRange(region.PlaneBoundaries);
                }

                return returnList;
            }
            set
            {
                this._polygons = value.MakeCoplanarLineSegmentsIntoRegions();
            }
        }

        /// <summary>
        /// Shifts the solid to another location and orientation
        /// </summary>
        /// <param name="passedShift"></param>
        /// <param name="passedRotationAxis"></param>
        public Polyhedron Shift(Shift passedShift)
        {
            List<Polygon> shiftedRegions = new List<Polygon>(_polygons.Shift(passedShift));

            return new Polyhedron(shiftedRegions);
        }
    }

    public class Solid
    {
    }
}
