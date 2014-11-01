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

        public virtual List<Polygon> Polygons
        {
            get { return this.Planes as List<Polygon>; }
            set { this.Planes = value; }
        }

        /// <summary>
        /// Accesses a point on the solid 
        /// </summary>
        public override Point PointOnSolid
        {
            get
            {
                Point returnPoint = null;
                if (this.Polygons.Count > 0)
                {
                    returnPoint = this.Polygons[0].PlaneBoundaries[0].BasePoint;
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

                foreach (Polygon region in this.Polygons)
                {
                    if (region.PlaneBoundaries != null)
                        returnList.AddRange(region.PlaneBoundaries);
                }

                return returnList;
            }
            set
            {
                this.Polygons = value.MakeCoplanarLineSegmentsIntoPolygons();
            }
        }

        #endregion

        #region Constructors

        public Polyhedron()
            : base()
        {
            this.Polygons = new List<Polygon>();
        }

        public Polyhedron(List<LineSegment> passedLineSegments)
            : base()
        {
            this.Polygons = passedLineSegments.MakeCoplanarLineSegmentsIntoPolygons();
        }

        public Polyhedron(List<Polygon> passedPolygons)
            : base()
        {
            this.Polygons = new List<Polygon>(passedPolygons);
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="passedSolid"></param>
        public Polyhedron(Polyhedron toCopy)
            : this(toCopy.Polygons) { }

        #endregion

        #region Overloaded Operators

        /// <summary>
        /// Not a perfect equality operator, is only accurate up to the Dimension Class's accuracy
        /// </summary>
        public static bool operator ==(Polyhedron polyhedron1, Polyhedron polyhedron2)
        {
            if ((object)polyhedron1 == null || (object)polyhedron2 == null)
            {
                if ((object)polyhedron1 == null && (object)polyhedron2 == null)
                {
                    return true;
                }
                return false;
            }
            return polyhedron1.Equals(polyhedron2);
        }

        /// <summary>
        /// Not a perfect equality operator, is only accurate up to the Dimension Class's accuracy
        /// </summary>
        public static bool operator !=(Polyhedron polyhedron1, Polyhedron polyhedron2)
        {
            if ((object)polyhedron1 == null || (object)polyhedron2 == null)
            {
                if ((object)polyhedron1 == null && (object)polyhedron2 == null)
                {
                    return false;
                }
                return true;
            }
            return !polyhedron1.Equals(polyhedron2);
        }

        /// <summary>
        /// does the same thing as ==
        /// </summary>
        public override bool Equals(object polyhedron)
        {
            if (polyhedron == null)
            {
                return false;
            }

            //try to cast the object to a Polygon, if it fails then we know the user passed in the wrong type of object
            try
            {
                Polyhedron passedPolyhedron = (Polyhedron)polyhedron;

                if (this.Polygons.Count == passedPolyhedron.Polygons.Count)
                {
                    foreach (Polygon polygon in this.Polygons)
                    {
                        int timesUsed = 0;
                        foreach (Polygon polygonOther in passedPolyhedron.Polygons)
                        {
                            if (polygon == polygonOther)
                            {
                                timesUsed++;
                            }
                        }

                        if (timesUsed != 1)
                        {
                            return false;
                        }
                    }

                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
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
        /// Returns whether or not the given plane intersects with the polyhedron
        /// </summary>
        /// <param name="passedPolyhedron"></param>
        /// <returns></returns>
        public virtual bool DoesIntersectNotCoplanar(Plane passedPlane)
        {
            foreach (Polygon polygon in this.Polygons)
            {
                if (this.DoesIntersectNotCoplanar(polygon))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns whether or not the given plane intersects with the polyhedron
        /// </summary>
        /// <param name="passedPolyhedron"></param>
        /// <returns></returns>
        public virtual bool DoesIntersect(Plane passedPlane)
        {
            foreach (Polygon polygon in this.Polygons)
            {
                if (passedPlane.DoesIntersect(polygon))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns the two Polyhedrons created by a slice. In order of size.
        /// </summary>
        /// <param name="passedPlane"></param>
        /// <returns></returns>
        public List<Polyhedron> Slice(Plane passedPlane)
        {
            if (this.DoesIntersect(passedPlane))
            {
                //make a list to keep track of all the points we sliced at
                List<Line> slicingPlaneLines = new List<Line>();

                List<Polygon> unknownPolygons = new List<Polygon>();
                List<Polygon> unknownPolygonsOther = new List<Polygon>();

                //the slice method will return 1 or more polyhedrons
                Polyhedron insidePolyhedron = new Polyhedron();
                Polyhedron outsidePolyhedron = new Polyhedron();

                //slice each polygon
                foreach (var polygon in this.Polygons)
                {
                    List<Polygon> slicedPolygons = polygon.Slice(passedPlane);

                    bool wasAdded = _addPolygonToCorrectPolyhedron(slicedPolygons, insidePolyhedron, outsidePolyhedron);

                    //if we failed to find which one to add it too hang on to it and take care of it after we have found the rest
                    if (!wasAdded)
                    {
                        if (slicedPolygons.Count == 1)
                        {
                            unknownPolygons.Add(slicedPolygons[0]);
                            unknownPolygonsOther.Add(null); //so the spacing stays the same
                        }
                        else
                        {
                            unknownPolygons.Add(slicedPolygons[0]);
                            unknownPolygonsOther.Add(slicedPolygons[1]);
                        }
                    }

                    //keep track of the lines we sliced on so we can easily make a new plane to cover the face
                    _addSlicingLine(passedPlane, polygon, slicingPlaneLines);
                }

                //figure out where our undetermined polygons go
                _addUndeterminedPolygons(insidePolyhedron, outsidePolyhedron, unknownPolygons, unknownPolygonsOther);

                //now make the joining sides based on where the intersection plane was
                //create a new plane based on the intersections
                Polygon slicingPlanePolygon = new Polygon(slicingPlaneLines);
                Polygon slicingPlanePolygon2 = new Polygon(slicingPlaneLines);

                if (slicingPlanePolygon.isValidPolygon())
                {
                    insidePolyhedron.Polygons.Add(slicingPlanePolygon);
                    outsidePolyhedron.Polygons.Add(slicingPlanePolygon2);
                }

                List<Polyhedron> toReturn = new List<Polyhedron>() { new Polyhedron(insidePolyhedron), new Polyhedron(outsidePolyhedron) };
                //toReturn.Sort();
                //toReturn.Reverse();
                return toReturn;
            }
            return new List<Polyhedron>() { new Polyhedron(this) };
        }

        private void _addUndeterminedPolygons(Polyhedron insidePolyhedron, Polyhedron outsidePolyhedron, List<Polygon> unknownPolygons, List<Polygon> unknownPolygonsOther)
        {
            List<Polygon> toAddInsides = new List<Polygon>();
            List<Polygon> toAddOutsides = new List<Polygon>();
            for (int i = 0; i < unknownPolygons.Count; i++)
            {
                Polygon polygon = unknownPolygons.ElementAt(i);
                Polygon otherPolygon = unknownPolygonsOther.ElementAt(i);

                if (insidePolyhedron.DoesShareSide(polygon))
                {
                    toAddInsides.Add(polygon);
                    unknownPolygons.Remove(polygon);

                    if (otherPolygon != null)
                    {
                        toAddOutsides.Add(otherPolygon);
                        unknownPolygons.Remove(otherPolygon);
                    }
                    i = -1; //start our loop over
                }
                else
                {
                    if (outsidePolyhedron.DoesShareSide(polygon))
                    {
                        toAddOutsides.Add(polygon);
                        unknownPolygons.Remove(polygon);

                        if (otherPolygon != null)
                        {
                            toAddInsides.Add(otherPolygon);
                            unknownPolygons.Remove(otherPolygon);
                        }
                        i = -1; //start our loop over
                    }

                    //if its not in either just leave it and it will get iterated through again after more have been placed
                }
            }

            insidePolyhedron.Polygons.AddRange(toAddInsides);
            outsidePolyhedron.Polygons.AddRange(toAddOutsides);
        }

        private bool _addPolygonToCorrectPolyhedron(List<Polygon> slicedPolygons, Polyhedron insidePolyhedron, Polyhedron outsidePolyhedron)
        {
            if (insidePolyhedron.Polygons.Count == 0)
            {
                //make the larger one the basis for our "inside" polygon
                insidePolyhedron.Polygons.Add(slicedPolygons[0]);

                //and if it was split then add the other part to the "outside"
                if (slicedPolygons.Count > 1)
                {
                    outsidePolyhedron.Polygons.Add(slicedPolygons[1]);
                }
                return true;
            }
            else
            {
                Polygon toAddInside = null;

                if (insidePolyhedron.DoesShareSide(slicedPolygons[0]))
                {
                    toAddInside = slicedPolygons[0];

                    if (slicedPolygons.Count > 1)
                    {
                        outsidePolyhedron.Polygons.Add(slicedPolygons[1]);
                    }
                }
                else if (slicedPolygons.Count > 1 && insidePolyhedron.DoesShareSide(slicedPolygons[1]))
                {
                    outsidePolyhedron.Polygons.Add(slicedPolygons[0]);
                    toAddInside = slicedPolygons[1];
                }

                if (toAddInside != null)
                {
                    insidePolyhedron.Polygons.Add(toAddInside);
                    return true;
                }
                //if we didnt find it inside see if it belongs outside
                else if (slicedPolygons.Count == 1 && outsidePolyhedron.DoesShareSide(slicedPolygons[0]))
                {
                    outsidePolyhedron.Polygons.Add(slicedPolygons[0]);
                    return true;
                }

                return false;
            }
        }

        private void _addSlicingLine(Plane passedPlane, Polygon polygon, List<Line> slicingPlaneLines)
        {
            Line slicingLine = polygon.Intersection(passedPlane);
            if (slicingLine != null && polygon.DoesIntersect(slicingLine))
            {
                slicingPlaneLines.Add(slicingLine);
            }
        }

        /// <summary>
        /// Returns all the Polyhedrons created by slices. In order of size.
        /// </summary>
        /// <param name="passedPlanes"></param>
        /// <returns></returns>
        public List<Polyhedron> Slice(List<Plane> passedPlanes)
        {
            //This list will be modified every time a slice happens. 
            List<Polyhedron> toSlicePolyhedrons = new List<Polyhedron> { this };
            List<Polyhedron> returnPolyhedrons = new List<Polyhedron>();

            //Do this for every passed slice
            foreach (Plane slicingPlane in passedPlanes)
            {
                //reset our return list
                returnPolyhedrons = new List<Polyhedron>(); 

                foreach (Polyhedron polyhedron in toSlicePolyhedrons)
                {
                    List<Polyhedron> slicedPolyhedrons = polyhedron.Slice(slicingPlane);

                    returnPolyhedrons.AddRange(slicedPolyhedrons);
                }

                //make our to slice list the smae as the return in case we still need to cut over them
                toSlicePolyhedrons = returnPolyhedrons;
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
            List<Polygon> shiftedRegions = this.Polygons.Shift(passedShift);

            return new Polyhedron(shiftedRegions);
        }

        /// <summary>
        /// Returns whether or not the polygan has a commn side with the polyhedron
        /// </summary>
        /// <param name="polygon"></param>
        /// <returns></returns>
        public bool DoesShareSide(Polygon polygon)
        {
            foreach (Polygon polygonReference in this.Polygons)
            {
                if (polygon.DoesShareSide(polygonReference))
                {
                    return true;
                }
            }
            return false;
        }

        #endregion
    }
}
