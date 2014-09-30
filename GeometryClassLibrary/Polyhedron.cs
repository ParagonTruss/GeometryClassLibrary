using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace GeometryClassLibrary
{
    [DebuggerDisplay("Base Point = {BasePoint}")]
    [Serializable]
    public class Polyhedron : Solid
    {
        #region Fields and Properties
        /// <summary>
        /// Accesses a point on the solid 
        /// </summary>
        public override Point PointOnSolid
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

        /// <summary>
        /// Accesses tge line segments in the polyhedron
        /// </summary>
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
                this._polygons = value.MakeCoplanarLineSegmentsIntoPolygons();
            }
        }
        #endregion

        #region Constructors
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
            _polygons = passedLineSegments.MakeCoplanarLineSegmentsIntoPolygons();
        }

        public Polyhedron(IEnumerable<Polygon> passedPolygons)
        {
            _polygons = new List<Polygon>(passedPolygons);
        }

        public Polyhedron(Polyhedron passedSolid)
        {
            _polygons = passedSolid._polygons;
        }
        #endregion

        #region Methods
        public override Point CenterPoint()
        {
            throw new NotImplementedException();
        }

        public override Line MidLine()
        {
            throw new NotImplementedException();
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
        #endregion
    }
}
