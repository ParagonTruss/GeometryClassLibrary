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

        public List<Polygon> Polygons
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
        public override bool Equals(object obj)
        {
            Polyhedron comparablePolyhedron = null;

            //try to cast the object to a Polygon, if it fails then we know the user passed in the wrong type of object
            try
            {
                comparablePolyhedron = (Polyhedron)obj;

                if (this.Polygons.Count == comparablePolyhedron.Polygons.Count)
                {
                    foreach (Polygon polygon in this.Polygons)
                    {
                        int timesUsed = 0;
                        foreach (Polygon polygonOther in comparablePolyhedron.Polygons)
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
        /// Returns the two Polyhedrons created by a slice. In order of size.
        /// </summary>
        /// <param name="passedPlane"></param>
        /// <returns></returns>
        public List<Polyhedron> Slice(Plane passedPlane)
        {
            //make a list to keep track of all the points we sliced at
            List<Line> slicingPlaneLines = new List<Line>();

            List<Polygon> unknownPolygons = new List<Polygon>();
            List<Polygon> unknownPolygonsOther = new List<Polygon>();


            //the slice method will return 1 or more polyhedrons
            List<Polygon> insidePolyhedron = new List<Polygon>();
            List<Polygon> outsidePolyhedron = new List<Polygon>();

            foreach (var polygon in this.Polygons)
            {
                List<Polygon> slicedPolygons = polygon.Slice(passedPlane);

                if (insidePolyhedron.Count == 0)
                {
                    //make the larger one the basis for our "inside" polygon
                    insidePolyhedron.Add(slicedPolygons[0]);

                    //and if it was split then add the other part to the "outside"
                    if (slicedPolygons.Count > 1)
                    {
                        outsidePolyhedron.Add(slicedPolygons[1]);

                        //add a point to our slicing line points
                        Line slicingLine = polygon.IntersectionLineWithPlane(passedPlane);
                        slicingPlaneLines.Add(slicingLine);
                    }
                }
                else
                {
                    bool hadInsidePart = false;
                    Polygon toAddInside = null;

                    foreach (Polygon inside in insidePolyhedron)
                    {
                        if (inside.DoesShareSide(slicedPolygons[0]))
                        {
                            toAddInside = slicedPolygons[0];
                            hadInsidePart = true;

                            if (slicedPolygons.Count > 1)
                            {
                                outsidePolyhedron.Add(slicedPolygons[1]);

                                //add a point to our slicing line points                        
                                Line slicingLine = polygon.IntersectionLineWithPlane(passedPlane);
                                slicingPlaneLines.Add(slicingLine);
                            }
                        }
                        else if (slicedPolygons.Count > 1 && inside.DoesShareSide(slicedPolygons[1]))
                        {
                            outsidePolyhedron.Add(slicedPolygons[0]);
                            toAddInside = slicedPolygons[1];
                            hadInsidePart = true;

                            //add a point to our slicing line points
                            Line slicingLine = polygon.IntersectionLineWithPlane(passedPlane);
                            slicingPlaneLines.Add(slicingLine);
                        }
                    }

                    if (toAddInside != null)
                    {
                        insidePolyhedron.Add(toAddInside);
                    }

                    if (!hadInsidePart)
                    {
                        if (slicedPolygons.Count == 1)
                        {
                            outsidePolyhedron.Add(slicedPolygons[0]);
                        }
                        else
                        {
                            //there in limbo!
                            unknownPolygons.Add(slicedPolygons[0]);
                            unknownPolygonsOther.Add(slicedPolygons[1]);

                            //add a point to our slicing line points                        
                            Line slicingLine = polygon.IntersectionLineWithPlane(passedPlane);
                            slicingPlaneLines.Add(slicingLine);
                        }
                    }
                }
            }

            List<Polygon> toAddInsides = new List<Polygon>();
            List<Polygon> toAddOutsides = new List<Polygon>();
            for (int i = 0; i < unknownPolygons.Count; i++)
            {
                Polygon polygon = unknownPolygons.ElementAt(i);
                bool wasInside = false;
                foreach (Polygon polygonReference in insidePolyhedron)
                {
                    if (!wasInside && polygon.DoesShareSide(polygonReference))
                    {
                        toAddInsides.Add(polygon);
                        toAddOutsides.Add(unknownPolygonsOther.ElementAt(i));
                        wasInside = true;
                    }
                }

                if (!wasInside)
                {
                    toAddOutsides.Add(polygon);
                    toAddInsides.Add(unknownPolygonsOther.ElementAt(i));
                }
            }

            insidePolyhedron.AddRange(toAddInsides);
            outsidePolyhedron.AddRange(toAddOutsides);

            //now make the joining sides
            //create a new plane based on the intersections
            Polygon slicingPlanePolygon = new Polygon(slicingPlaneLines);
            Polygon slicingPlanePolygon2 = new Polygon(slicingPlaneLines);

            if (slicingPlanePolygon.isValidPolygon())
            {
                insidePolyhedron.Add(slicingPlanePolygon);
                outsidePolyhedron.Add(slicingPlanePolygon2);
            }

            List<Polyhedron> toReturn = new List<Polyhedron>() { new Polyhedron(insidePolyhedron), new Polyhedron(outsidePolyhedron) };
            //toReturn.Sort();
            //toReturn.Reverse();
            return toReturn;
        }

        /// <summary>
        /// Returns all the Polyhedrons created by slices. In order of size.
        /// </summary>
        /// <param name="passedPlanes"></param>
        /// <returns></returns>
        public List<Polyhedron> Slice(List<Plane> passedPlanes)
        {
            //This list will be modified every time a slice happens. 
            List<Polyhedron> returnPolyhedrons = new List<Polyhedron> { this };

            //Do this for every passed slice
            for (int i = 0; i < passedPlanes.Count; i++)
            {
                //the slice method will return 1 or more polyhedrons
                List<Polygon> insidePolyhedron = new List<Polygon>();
                List<Polygon> outsidePolyhedron = new List<Polygon>();

                //slice them and put them together
                for (int j = 0; j < returnPolyhedrons.Count; j++)
                {
                    foreach (var polygon in returnPolyhedrons[0].Polygons)
                    {
                        List<Polygon> slicedPolygons = polygon.Slice(passedPlanes[i]);

                        if (insidePolyhedron.Count == 0)
                        {
                            //make the larger one the basis for our "inside" polygon
                            insidePolyhedron.Add(slicedPolygons[0]);

                            //and if it was split then add the other part to the "outside"
                            if (slicedPolygons.Count > 1)
                            {
                                outsidePolyhedron.Add(slicedPolygons[1]);
                            }
                        }
                        else
                        {
                            bool hadInsidePart = false;

                            foreach (Polygon inside in insidePolyhedron)
                            {
                                if (inside.DoesShareSide(slicedPolygons[0]))
                                {
                                    insidePolyhedron.Add(slicedPolygons[0]);
                                    hadInsidePart = true;

                                    if (slicedPolygons.Count > 1)
                                    {
                                        outsidePolyhedron.Add(slicedPolygons[1]);
                                    }
                                }
                                else if (slicedPolygons.Count > 1 && inside.DoesShareSide(slicedPolygons[1]))
                                {
                                    outsidePolyhedron.Add(slicedPolygons[0]);
                                    insidePolyhedron.Add(slicedPolygons[1]);
                                    hadInsidePart = true;
                                }
                            }

                            if (!hadInsidePart)
                            {
                                outsidePolyhedron.Add(slicedPolygons[0]);
                            }
                        }
                    }

                    returnPolyhedrons.Add(new Polyhedron(insidePolyhedron));
                    if (outsidePolyhedron != null)
                    {
                        returnPolyhedrons.Add(new Polyhedron(outsidePolyhedron));
                    }

                    returnPolyhedrons.Remove(returnPolyhedrons[0]);
                }

                //add the slicing plane one
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
        #endregion
    }
}
