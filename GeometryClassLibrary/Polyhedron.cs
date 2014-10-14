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
        public virtual List<Polygon> Polygons
        {
            get { return _polygons; }
            set { _polygons = value; }
        }
        private List<Polygon> _polygons;

        public Polyhedron()
        {
            _polygons = new List<Polygon>();
        }

        public Polyhedron(List<LineSegment> passedLineSegments)
        {
            _polygons = passedLineSegments.MakeCoplanarLineSegmentsIntoPolygons();
        }

        public Polyhedron(List<Polygon> passedPolygons)
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
        /// Returns the two Polyhedrons created by a slice. In order of size.
        /// </summary>
        /// <param name="passedPlane"></param>
        /// <returns></returns>
        public List<Polyhedron> Slice(Plane passedPlane)
        {
            List<Polygon> biggerPolyhedron = new List<Polygon>();
            List<Polygon> smallerPolyhedron = new List<Polygon>();

            foreach (var polygon in this._polygons)
            {
                List<Polygon> slicedPolygons = polygon.Slice(passedPlane);
                biggerPolyhedron.Add(slicedPolygons[0]);
                smallerPolyhedron.Add(slicedPolygons[1]);
            }

            Polyhedron bigger = new Polyhedron(biggerPolyhedron);
            Polyhedron smaller = new Polyhedron(smallerPolyhedron);

            return new List<Polyhedron> { bigger, smaller };
        }

        /// <summary>
        /// Returns all the Polyhedrons created by slices. In order of size.
        /// </summary>
        /// <param name="passedPlanes"></param>
        /// <returns></returns>
        public List<Polyhedron> Slice(List<Plane> passedPlanes)
        {
            //This list will be modified every time a slice happens. 
            List<Polyhedron> returnPolyhedrons = new List<Polyhedron>{this};

            //Do this for every passed slice
            for (int i = 0; i < passedPlanes.Count; i++)
			{
                //the slice method will return 1 or more polyhedrons
                List<Polygon> biggerPolyhedron = new List<Polygon>();
                List<Polygon> smallerPolyhedron = new List<Polygon>();

                for (int j = 0; j < returnPolyhedrons.Count; i++)
                {
		                foreach (var polygon in returnPolyhedrons[0].Polygons)
                        {
                            List<Polygon> slicedPolygons = polygon.Slice(passedPlanes[i]);
                            biggerPolyhedron.Add(slicedPolygons[0]);
                            if (slicedPolygons.Count > 1)
                            {
                                smallerPolyhedron.Add(slicedPolygons[1]);
                            }
                        }

                    returnPolyhedrons.Add(new Polyhedron(biggerPolyhedron));
                    if (smallerPolyhedron != null )
	                {
		                     returnPolyhedrons.Add(new Polyhedron(smallerPolyhedron));
	                }

                    returnPolyhedrons.Remove(returnPolyhedrons[0]);
	            }
			}

            return returnPolyhedrons;
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
