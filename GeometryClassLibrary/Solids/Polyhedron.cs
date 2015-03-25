using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace GeometryClassLibrary
{
    [DebuggerDisplay("Base Point = {BasePoint}")]

    public class Polyhedron : Solid
    {
        #region Properties and Fields

        /// <summary>
        /// Returns a List of all the linesegments that make up this polyhedron
        /// </summary>
        public override IList<IEdge> Edges
        {
            get 
            {
                List<IEdge> toReturn = new List<IEdge>();

                //go through each face and get the line segments
                foreach (var face in this.Polygons)
                {
                    //then check to see if each line segment is already in the list or else we will get duplicates
                    foreach (var lineSegment in face.LineSegments)
                    {
                        if(!toReturn.Contains(lineSegment))
                        {
                            toReturn.Add(lineSegment);
                        }
                    }
                }

                return toReturn;
            }
        }

        /// <summary>
        /// A list containing the polygons that make up this polyhedron
        /// </summary>
        public virtual List<Polygon> Polygons
        {
            get 
            {
                List<Polygon> polygons = new List<Polygon>();
                foreach (var item in this.Faces)
                {
                    polygons.Add((Polygon)item);
                }
                return polygons;
            }
            internal set 
            {
                List<PlaneRegion> planeRegions = new List<PlaneRegion>();
                foreach (var item in value)
                {
                    planeRegions.Add(item);
                }
                this.Faces = planeRegions;

            }
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
                    returnPoint = this.Polygons[0].LineSegments[0].BasePoint;
                }
                return returnPoint;
            }
        }

        /// <summary>
        /// returns all the linesegments contained in this polyhedron's polygons
        /// </summary>
        public List<LineSegment> LineSegments
        {
            get
            {
                List<LineSegment> returnList = new List<LineSegment>();

                foreach (Polygon region in this.Polygons)
                {
                    if (region.LineSegments != null)
                    {
                        foreach(LineSegment segment in region.LineSegments)
                        {
                            if (!returnList.Contains(segment))
                            {
                                returnList.Add(segment);
                            }
                        }
                    }
                }

                return returnList;
            }
            internal set
            {
                this.Polygons = value.MakeCoplanarLineSegmentsIntoPolygons();
            }
        }

        /// <summary>
        /// Returns a list of all the verticies in the Polyhedron
        /// </summary>
        public override List<Point> Verticies
        {
            get
            {
                return this.LineSegments.GetAllPoints();
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new empty Polyhedron
        /// </summary>
        public Polyhedron()
            : base()
        {
            this.Polygons = new List<Polygon>();
        }

        /// <summary>
        /// Makes a Polyhedron using the giving line segments made into polygons based on if they are coplanar and share a point
        /// </summary>
        /// <param name="passedLineSegments">The line segments that define this Polyhedron</param>
        public Polyhedron(List<LineSegment> passedLineSegments)
            : base()
        {
            this.Polygons = passedLineSegments.MakeCoplanarLineSegmentsIntoPolygons();
        }

        /// <summary>
        /// Creates a Polyhedron using the passed polygons as its side/polygons
        /// </summary>
        /// <param name="passedPolygons">The list of polygons that define this Polyhedron</param>
        public Polyhedron(List<Polygon> passedPolygons)
            : base()
        {
            List<Polygon> polygonsToUse = new List<Polygon>();
            foreach (Polygon polygon in passedPolygons)
            {
                polygonsToUse.Add(polygon);
            }

            this.Polygons = polygonsToUse;
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="passedSolid">The polyhedron to copy</param>
        public Polyhedron(Polyhedron toCopy)
            : this(toCopy.Polygons) { }

        #endregion

        #region Overloaded Operators

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }


        /// <summary>
        /// Not a perfect equality operator, is only accurate up to the Distance Class's accuracy
        /// </summary>
        public static bool operator ==(Polyhedron polyhedron1, Polyhedron polyhedron2)
        {
            if ((object)polyhedron1 == null)
            {
                if ((object)polyhedron2 == null)
                {
                    return true;
                }
                return false;
            }
            return polyhedron1.Equals(polyhedron2);
        }

        /// <summary>
        /// Not a perfect equality operator, is only accurate up to the Distance Class's accuracy
        /// </summary>
        public static bool operator !=(Polyhedron polyhedron1, Polyhedron polyhedron2)
        {
            if ((object)polyhedron1 == null)
            {
                if ((object)polyhedron2 == null)
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
            //make sure the obj passed is not null
            if (obj == null)
            {
                return false;
            }

            //try to cast the object to a Polygon, if it fails then we know the user passed in the wrong type of object
            try
            {
                Polyhedron comparablePolyhedron = (Polyhedron)obj;

                //compare each of its polygons to see if they are all included
                if (this.Polygons.Count != comparablePolyhedron.Polygons.Count)
                {
                    return false;
                }

                foreach (Polygon polygon in this.Polygons)
                {
                    //make sure each polygon is represented exactly once
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

                //if they were all used exactly once than its equal
                return true;
            }
            //if it wasnt a polygon than it must not be eqaul
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
        /// Returns whether or not the given plane intersects with the polyhedron but is not coplanar with any of its sides
        /// also can be thought of as if it has a single distinct intersect point or line
        /// </summary>
        /// <param name="passedPolyhedron">The polyghedron to check if it intersects with but not coplanar to any of its sides</param>
        /// <returns>Returns a bool of whether or not the Polyhedron intersects the plane and is not coplanar with it</returns>
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
        /// <param name="passedPolyhedron">The Polyhedron to see if the plane intersects with</param>
        /// <returns>Returns a bool of whether or not the polyhedron touches the plane at any point</returns>
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
        /// <param name="slicingPlane">The plane to slice the Polyhedron with</param>
        /// <returns>Returns two new Polyhedrons created by the slice in order of size</returns>
        public new List<Polyhedron> Slice(Plane slicingPlane)
        {
            if (this.DoesIntersect(slicingPlane))
            {
                //make a list to keep track of all the points we sliced at
                List<Line> slicingPlaneLines = new List<Line>();

                List<Polygon> unknownPolygons = new List<Polygon>();
                List<Polygon> unknownPolygonsOther = new List<Polygon>();

                //the slice method will return 1 or more polyhedrons
                List<Polygon> unconstructedInsidePolyhedron = new List<Polygon>();
                List<Polygon> unconstructedOutsidePolyhedron = new List<Polygon>();

                //slice each polygon
                foreach (var polygon in this.Polygons)
                {
                    List<Polygon> slicedPolygons = polygon.Slice(slicingPlane);

                    bool wasAdded = _addPolygonToCorrectPolyhedron(slicedPolygons, unconstructedInsidePolyhedron, unconstructedOutsidePolyhedron, slicingPlane);

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
                    _addSlicingLine(slicingPlane, polygon, slicingPlaneLines);
                }

                //figure out where our undetermined polygons go
                _addUndeterminedPolygons(unconstructedInsidePolyhedron, unconstructedOutsidePolyhedron, unknownPolygons, unknownPolygonsOther);

                //now make the joining sides based on where the intersection plane was
                //create a new plane based on the intersections
                try
                {
                    Polygon slicingPlanePolygon = new Polygon(slicingPlaneLines);
                    Polygon slicingPlanePolygon2 = new Polygon(slicingPlaneLines);

                    //if (slicingPlanePolygon.isValidPolygon())
                    //{
                        unconstructedInsidePolyhedron.Add(slicingPlanePolygon);
                        unconstructedOutsidePolyhedron.Add(slicingPlanePolygon2);
                    //}
                }
                catch (ArgumentException) { }

                List<Polyhedron> toReturn = new List<Polyhedron>() { new Polyhedron(unconstructedInsidePolyhedron), new Polyhedron(unconstructedOutsidePolyhedron) };
                //toReturn.Sort();
                //toReturn.Reverse();
                return toReturn;
            }
            return new List<Polyhedron>() { new Polyhedron(this) };
        }

        /// <summary>
        /// Adds the previously unknown polygons to the correct Polyhedron
        /// </summary>
        /// <param name="unconstructedInsidePolyhedron">the "inside" polyhedron's list of polygons</param>
        /// <param name="unconstructedOutsidePolyhedron">the "outside" polyhedron's list of polygons</param>
        /// <param name="unknownPolygons">The list of unknown Polygons to add </param>
        /// <param name="unknownPolygonsOther">The list of unknown Polygons to add that represents the other part of the first list</param>
        private void _addUndeterminedPolygons(List<Polygon> unconstructedInsidePolyhedron, List<Polygon> unconstructedOutsidePolyhedron, 
            List<Polygon> unknownPolygons, List<Polygon> unknownPolygonsOther)
        {
            List<Polygon> toAddInsides = new List<Polygon>();
            List<Polygon> toAddOutsides = new List<Polygon>();
            for (int i = 0; i < unknownPolygons.Count; i++)
            {
                Polygon polygon = unknownPolygons.ElementAt(i);
                Polygon otherPolygon = unknownPolygonsOther.ElementAt(i);

                if (unconstructedInsidePolyhedron.DoesShareSideWithPolygonInList(polygon))
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
                    if (unconstructedOutsidePolyhedron.DoesShareSideWithPolygonInList(polygon))
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

            unconstructedInsidePolyhedron.AddRange(toAddInsides);
            unconstructedOutsidePolyhedron.AddRange(toAddOutsides);
        }

        /// <summary>
        /// Adds the Polygons to the correct new Polyhedron
        /// </summary>
        /// <param name="slicedPolygons">The sliced Polygons</param>
        /// <param name="unconstructedInsidePolyhedron">the "inside" polyhedron's list of polygons</param>
        /// <param name="unconstructedOutsidePolyhedron">the "outside" polyhedron's list of polygons</param>
        /// <returns></returns>
        private bool _addPolygonToCorrectPolyhedron(List<Polygon> slicedPolygons, List<Polygon> unconstructedInsidePolyhedron, List<Polygon> unconstructedOutsidePolyhedron,
            Plane slicingPlane)
        {
            if (unconstructedInsidePolyhedron.Count == 0)
            {
                //make the larger one the basis for our "inside" polygon
                unconstructedInsidePolyhedron.Add(slicedPolygons[0]);

                //and if it was split then add the other part to the "outside"
                if (slicedPolygons.Count > 1)
                {
                    unconstructedOutsidePolyhedron.Add(slicedPolygons[1]);
                }
                return true;
            }
            else
            {
                Polygon toAddInside = null;

                Point insidePolyhedronReferencePoint = unconstructedInsidePolyhedron.FindVertexToUseAsReferenceNotOnThePlane(slicingPlane);
                Point outsidePolyhedronReferencePoint = unconstructedOutsidePolyhedron.FindVertexToUseAsReferenceNotOnThePlane(slicingPlane);
                Point slicedPolygonZeroReferencePoint = slicedPolygons[0].FindVertexNotOnTheGivenPlane(slicingPlane);
                //we have to be careful initializing this one because there could be only one item in the list
                Point slicedPolygonOneReferencePoint = null;
                if(slicedPolygons.Count() > 1)
                {
                    slicedPolygonOneReferencePoint = slicedPolygons[1].FindVertexNotOnTheGivenPlane(slicingPlane);
                }

                //if the inside shares a point with the first sliced polygon
                if (insidePolyhedronReferencePoint != null && slicedPolygonZeroReferencePoint != null && 
                    slicingPlane.PointIsOnSameSideAs(insidePolyhedronReferencePoint, slicedPolygonZeroReferencePoint))
                    //unconstructedInsidePolyhedron.DoesShareSideWithPolygonInList(slicedPolygons[0]))
                {
                    toAddInside = slicedPolygons[0];

                    if (slicedPolygons.Count > 1)
                    {
                        unconstructedOutsidePolyhedron.Add(slicedPolygons[1]);
                    }
                }
                    //if the inside shares a point with the second sliced polygon
                else if (insidePolyhedronReferencePoint != null && slicedPolygonOneReferencePoint != null &&
                    slicingPlane.PointIsOnSameSideAs(insidePolyhedronReferencePoint, slicedPolygonOneReferencePoint))
                    //slicedPolygons.Count > 1 && unconstructedInsidePolyhedron.DoesShareSideWithPolygonInList(slicedPolygons[1]))
                {

                    unconstructedOutsidePolyhedron.Add(slicedPolygons[0]);
                    toAddInside = slicedPolygons[1];
                }

                //If we found the polygon to add to the inside unconstructed polygon
                if (toAddInside != null)
                {
                    unconstructedInsidePolyhedron.Add(toAddInside);
                    return true;
                }
                //if we didnt find it inside see if it belongs outside
                else if (slicedPolygons.Count == 1 && outsidePolyhedronReferencePoint != null && slicedPolygonZeroReferencePoint != null &&
                    slicingPlane.PointIsOnSameSideAs(outsidePolyhedronReferencePoint, slicedPolygonZeroReferencePoint))
                {
                    unconstructedOutsidePolyhedron.Add(slicedPolygons[0]);
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Adds the slicing line to the list of lines so we can create a polygon to represent the side that is on the slicing plane
        /// </summary>
        /// <param name="slicingPlane">The plane that is slicing the polygon</param>
        /// <param name="polygon">The polygon the is being sliced</param>
        /// <param name="slicingPlaneLines">The list of lines creates by the slicing plane</param>
        private void _addSlicingLine(Plane slicingPlane, Polygon polygon, List<Line> slicingPlaneLines)
        {
            Line slicingLine = polygon.Intersection(slicingPlane);
            if (slicingLine != null && polygon.DoesIntersect(slicingLine))
            {
                if (!slicingPlaneLines.Contains(slicingLine))
                {
                    slicingPlaneLines.Add(slicingLine);
                }
            }
        }

        /// <summary>
        /// Returns all the Polyhedrons created by slices. In order of size.
        /// </summary>
        /// <param name="slicingPlanes">The planes to slice the Polyhedron With</param>
        /// <returns>Returns a list of Polyhedrons that were created by the slice, with the largest first</returns>
        public List<Polyhedron> Slice(List<Plane> slicingPlanes)
        {
            //This list will be modified every time a slice happens. 
            List<Polyhedron> toSlicePolyhedrons = new List<Polyhedron> { this };
            List<Polyhedron> returnPolyhedrons = new List<Polyhedron>();

            //Do this for every passed slice
            foreach (Plane slicingPlane in slicingPlanes)
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
        /// Shifts the Polyhedron to another location and orientation
        /// </summary>
        /// <param name="passedShift">The shift to preform on the Polyhedron</param>
        /// <returns>A new Polyhedron that has been shifted</returns>
        public new Polyhedron Shift(Shift passedShift)
        {
            List<Polygon> shiftedRegions = this.Polygons.Shift(passedShift);

            return new Polyhedron(shiftedRegions);
        }


        public Polyhedron SystemShift(CoordinateSystem systemToShiftTo)
        {
            Polyhedron toReturn = this.Shift(systemToShiftTo.ShiftToThisFromWorld());

            //CoordinateSystem.CurrentSystem = systemToShiftTo;

            return toReturn;
        }


        /// <summary>
        /// Returns whether or not the polygan has a common side with the polyhedron / any of the polyhedrons sides
        /// </summary>
        /// <param name="polygon">The polygon to see if any of the the Polyhedrons side share a side with</param>
        /// <returns>Returns a bool of whether or not the Polyhedron has a common side with the polygon</returns>
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

        /// <summary>
        /// Determines whether or not the point is on the sides of this Polyhedron
        /// </summary>
        /// <param name="pointToCheckIfItContains">The point to see if it is on the sides of this Polyhedron</param>
        /// <returns>Returns a bool of whether or not the point is on a side of this Polyhedron</returns>
        public bool DoesContainPointAlongSides(Point pointToCheckIfItContains)
        {
            foreach (var segment in LineSegments)
            {
                if (segment.Contains(pointToCheckIfItContains))
                {
                    return true;
                }
            }
            return false;
        }

        #endregion
    }
}
