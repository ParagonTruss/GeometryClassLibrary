/*
    This file is part of Geometry Class Library.
    Copyright (C) 2017 Paragon Component Systems, LLC.

    This library is free software; you can redistribute it and/or
    modify it under the terms of the GNU Lesser General Public
    License as published by the Free Software Foundation; either
    version 2.1 of the License, or (at your option) any later version.

    This library is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
    Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public
    License along with this library; if not, write to the Free Software
    Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
*/

using System;
using System.Collections.Generic;
using System.Linq;
using UnitClassLibrary.AreaUnit.AreaTypes.Imperial.SquareInchesUnit;
using UnitClassLibrary.AreaUnit;
using UnitClassLibrary.DistanceUnit.DistanceTypes.Imperial.InchUnit;
using UnitClassLibrary.DistanceUnit;
using UnitClassLibrary.AngleUnit;
using static UnitClassLibrary.AngleUnit.Angle;
using UnitClassLibrary.DistanceUnit.DistanceTypes;

namespace GeometryClassLibrary
{
    /// <summary>
    /// A plane region is a section of a plane.
    /// </summary>
    public partial class Polygon : Plane, IShift<Polygon>, IEquatable<Polygon>
    {
        public static explicit operator Polygon(PlaneRegion p)
        {
            return new Polygon(p.Edges.Select(e => (LineSegment)e).ToList(), false);
        }

        #region Properties and Fields

        public virtual List<LineSegment> LineSegments { get; }

        public virtual List<Point> Vertices { get { return LineSegments.Select(s => s.BasePoint).ToList(); } }
        
        /// <summary>
        /// determines if the polygon is convex
        /// i.e. all segments whose endpoints are inside the polygon, are inside the polygon
        /// </summary>
        public bool IsConvex
        {
            get
            {
                if (_isConvex == null)
                {
                    _isConvex = _getIsConvex();
                }
                return (bool)_isConvex;
            }
        }
        private bool? _isConvex = null;

        private bool _getIsConvex()
        {
            int n = LineSegments.Count;
            Vector crossProduct;
            for (int i = 0; i < n; i++)
            {
                if (i < n - 1)
                {
                    crossProduct = this.LineSegments[i].CrossProduct(LineSegments[i + 1]);
                }
                else
                {
                    crossProduct = this.LineSegments.Last().CrossProduct(LineSegments.First());
                }
                if (crossProduct.Magnitude != Distance.ZeroDistance && crossProduct.Direction != NormalVector.Direction)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// The average of the vertices.
        /// Not the same as centroid, but close enough for relatively small convex polygons.
        /// </summary>
        public Point CenterPoint
        {
            get
            {
                if (_centerPoint == null)
                {
                    _centerPoint = this.Vertices.CenterPoint();
                }
                return _centerPoint;
            }
        }
        private Point _centerPoint;

        /// <summary>
        /// The area of the polygon.
        /// </summary>
        public override Area Area
        {
            get
            {   
                if (_area == null)
                {
                    _area = new Area(new SquareInch(), Math.Abs(_findArea()));
                }
                return _area;
            }
        }
        private Area _area;

        /// <summary>
        /// The centroid represents the balancing point, if the polygon were a sheet of paper.
        /// </summary>
        public virtual Point Centroid
        {
            get
            {
                if (_centroid == null)
                {
                    _centroid = _findCentroid();
                }
                return _centroid;
            }
        }
        private Point _centroid;

        #endregion

        #region Constructors

        /// <summary>
        /// Empty constructor
        /// </summary>
        protected Polygon() { }

        public Polygon(bool shouldValidate, params Point[] points) : this(points, shouldValidate) { }        
        public Polygon(bool shouldValidate, params LineSegment[] segments) : this(segments, shouldValidate) { }
        /// <summary>
        /// Makes a polygon by connecting the points with line segments in the order they are in the list. If they are not in the correct order you can tell it
        /// to sort the linessegments of the polygon clockwise with the boolean flag unless you need it in the specific order it is in
        /// </summary>
        /// <param name="passedPoints">The List of points to make the polygon with. It will create the linesegments based on the order the points are inputted</param>
        public Polygon(IEnumerable<Point> passedPoints, bool shouldValidate = true)
            : this(passedPoints.MakeIntoLineSegmentsThatMeet(), shouldValidate)
        { }

        /// <summary>
        /// Creates a polygon from the passed linesegments, after validating that they in fact form a closed nondegenerate planar region.
        /// </summary>
        public Polygon(IEnumerable<LineSegment> lineSegments, bool shouldValidate = true)
        {
            this.LineSegments = shouldValidate ? (lineSegments.FixSegmentOrientation()) : (lineSegments).ToList();

            this.NormalLine = this._getNormalLine();
        }

        private Line _getNormalLine()
        {
            Vector vector1 = (LineSegments.OrderBy(s => s.EndPoint.X).ThenBy(s => s.EndPoint.Y).ThenBy(s => s.EndPoint.Z).First());

            Vector vector2 = LineSegments.First(s => s.BasePoint == vector1.EndPoint);

            var normal = vector1.CrossProduct(vector2);

            return new Line(vector1.BasePoint, normal.Direction);
        }

        /// <summary>
        /// Creates a new Polygon that is a copy of the passed polygon
        /// </summary>
        /// <param name="passedBoundaries"></param>
        public Polygon(Polygon polygonToCopy)
            : base(polygonToCopy)
        {
            this.LineSegments = polygonToCopy.LineSegments.ToList();
        }

        #endregion

        #region Overloaded Operators

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Not a perfect equality operator, is only accurate up to the Distance Class's accuracy
        /// </summary>
        public static bool operator ==(Polygon region1, Polygon region2)
        {
            if ((object)region1 == null)
            {
                if ((object)region2 == null)
                {
                    return true;
                }
                return false;
            }
            return region1.Equals(region2);
        }

        /// <summary>
        /// Not a perfect equality operator, is only accurate up to the Distance Class's accuracy
        /// </summary>
        public static bool operator !=(Polygon region1, Polygon region2)
        {
            if ((object)region1 == null)
            {
                if ((object)region2 == null)
                {
                    return false;
                }
                return true;
            }
            return !region1.Equals(region2);
        }

        public bool Equals(Polygon other)
        {
            //make sure we didnt get a null
            if (other == null)
            {
                return false;
            }
            if (this.Vertices.Count != other.Vertices.Count) return false;
            if (!this.NormalDirection.Equals(other.NormalDirection))
            {
                other = other.ReverseOrientation();
            }
            var count = this.Vertices.Count;
            var offset = other.Vertices.FindIndex(v => v.Equals(this.Vertices[0]));

            if (offset == -1) return false;

            var allEqual = this.Vertices.Select((point, i) => point.Equals(other.Vertices[(i + offset) % count])).All(_.Identity);
            return allEqual;
        }

        /// <summary>
        /// does the same thing as ==
        /// </summary>
        public override bool Equals(object obj) => (obj as Polygon)?.Equals(this) ?? false;

        #endregion

        #region Methods
        public Polygon Rotate2D (Point centerOfRotation, Angle rotationAngle)
        {
            return new Polygon(this.Vertices.Select(vertex => vertex.Rotate2D(rotationAngle, centerOfRotation)).ToList(), false);
        }
        public new Polygon Shift(Shift passedShift)
        {
            return new Polygon(this.LineSegments.Shift(passedShift), false);
        }

        /// <summary>
        /// Rotates the polygon about the given axis by the specified angle
        /// </summary>
        public new Polygon Rotate(Rotation rotationToApply)
        {
            return new Polygon(
                this.LineSegments.Select(segment => segment.Rotate(rotationToApply)).ToList(), false);
        }


        public Polygon Translate(Translation translation)
        {
            return new Polygon(
                this.Vertices.Select(v => v.Translate(translation)).ToList(), false);
        }

        public LineSegment ContainedPieceOfSegment(LineSegment segment)
        {
            var slicedPieces = segment.Slice(this.LineSegments);
            //var piece = slicedPieces.FirstOrDefault(s => this.Contains(s));
            for (int i = 0; i < slicedPieces.Count; i++)
            {
                if (this.Contains(slicedPieces[i])) return slicedPieces[i];
            }
            return null;
        }

        public virtual Polygon SmallestEnclosingRectangle()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Extends a polygon to a 3 dimensional solid. 
        /// Makes a copy of the polygon shifted over by the extrusion vector, then connects corresponding vertices.
        /// </summary>
        public Polyhedron Extrude(Vector extrusionVector)
        {
            var baseFace = extrusionVector.DotProduct(this.NormalVector) < new Area(0,Area.SquareInches)
                ? this
                : this.ReverseOrientation();

            List<Polygon> faces = new List<Polygon> {baseFace};
            List<LineSegment> oppositeFace = new List<LineSegment>();

            foreach (LineSegment segment in baseFace.LineSegments)
            {
                LineSegment opposite = segment.Shift(extrusionVector);

                Polygon sideFace = new Polygon(true, opposite.BasePoint, opposite.EndPoint, segment.EndPoint, segment.BasePoint);
                faces.Add(sideFace);

                oppositeFace.Add(opposite.Reverse());
            }
            oppositeFace.Reverse();
            faces.Add(new Polygon(oppositeFace, false));

            var solid = new Polyhedron(faces,false);
            return solid;
        }

        /// <summary>
        /// returns a list of the points of intersection between these polygons
        /// if there are any overlapping sides, the endpoints of the shared segment are included in the list
        /// </summary>
        public List<Point> IntersectionPoints(Polygon otherPolygon)
        {
            List<Point> newVertices = new List<Point>();
            foreach (LineSegment segment in this.LineSegments)
            {
                foreach (LineSegment otherSegment in otherPolygon.LineSegments)
                {
                    if (segment.IsParallelTo(otherSegment))
                    {
                        LineSegment overlap = segment.OverlappingSegment(otherSegment);
                        if (overlap != null)
                        {
                            _addToList(newVertices, overlap.BasePoint);
                            _addToList(newVertices, overlap.EndPoint);
                        }
                    }
                    else
                    {
                        Point intersection = segment.IntersectWithSegment(otherSegment);
                        if (intersection != null)
                        {
                            _addToList(newVertices, intersection);
                        }
                    }
                }
            }
            return newVertices;
        }

        private static void _addToList(List<Point> list, Point point)
        {
            if (point != null && !list.Contains(point))
            {
                list.Add(point);
            }
        }

        /// <summary>
        /// This function Slices a plane and returns both halves of the plane, with the larger piece returning first. If the
        /// Line is not in this Plane it will return a copy of the original Polygon in a list
        /// </summary>
        /// <param name="slicingLine">The line in the Polygon to slice at</param>
        /// <returns>returns the Polygons from slicing along the Line in descending size order or the original Plane if the line
        /// is not in the plane</returns>
        public List<Polygon> Slice(Line slicingLine)
        {
            if (slicingLine == null || !((Plane)this).Contains(slicingLine))
            {
                return new List<Polygon>() { new Polygon(this) };
            }

            //find the normal direction of the plane we will use to slice with
            Vector divisionPlaneNormal = this.NormalVector.CrossProduct(slicingLine.UnitVector(new Inch()));

            //now make it with the normal we found and the lines basepoint
            Plane divisionPlane = new Plane(divisionPlaneNormal.Direction, slicingLine.BasePoint);
            slicingLine = this.IntersectingSegment(divisionPlane);

            return this._slice(slicingLine, divisionPlane);
        }

        /// <summary>
        /// This function Slices a plane and returns both halves of the plane, with the larger piece returning first. If the
        /// Plane does not intersect this Polygon it returns a copy of the original plane region in a list
        /// </summary>
        /// <param name="slicingPlane">the plane to use to slice this Polygon where they intersect</param>
        /// <returns>returns a List of the two plane Regions that represent the slices region with the region with the larger area first</returns>
        public List<Polygon> Slice(Plane slicingPlane)
        {
            Line slicingLine = this.IntersectingSegment(slicingPlane);

            return this._slice(slicingLine, slicingPlane);
        }

        #region Slice Helpers
        private List<Polygon> _slice(Line slicingLine, Plane slicingPlane)
        {

            //if it doesnt intersect then return the original
            if (slicingLine == null || slicingPlane.Equals(this as Plane))
            {
                return new List<Polygon>() { new Polygon(this) };
            }

            var intersectionPoints = this.IntersectionCoplanarPoints(slicingLine);

            if (intersectionPoints.Count < 2 || (intersectionPoints.Count == 2 && this.HasSide((LineSegment)slicingLine)))
            {
                return new List<Polygon>() { new Polygon(this) };
            }

            var segments = this.LineSegments.ToList();
            foreach (var point in intersectionPoints)
            {
                foreach (var segment in segments)
                {
                    if (segment.Contains(point) && !point.IsBaseOrEndPointOf(segment))
                    {
                        segments.Remove(segment);
                        var segment1 = new LineSegment(point, segment.BasePoint);
                        var segment2 = new LineSegment(point, segment.EndPoint);
                        segments.Add(segment1);
                        segments.Add(segment2);
                        break;
                    }
                }
            }

            var separated = _segmentsInTwoLists(segments, slicingPlane);
            var list1 = separated[0];
            var list2 = separated[1];

            for (int i = 0; i < intersectionPoints.Count / 2; i++)
            {
                var basePoint = intersectionPoints[2 * i];
                var endPoint = intersectionPoints[2 * i + 1];
                list1.Add(new LineSegment(basePoint, endPoint));
                list2.Add(new LineSegment(basePoint, endPoint));
            }
            var polygon1 = (list1).CreatePolygonIfValid();
            var polygon2 = list2.CreatePolygonIfValid();
            bool breakpoint = (polygon1 == null || polygon2 == null);
            return new List<Polygon>() { polygon1, polygon2 };
        }

        private List<List<LineSegment>> _segmentsInTwoLists(List<LineSegment> segments, Plane slicingPlane)
        {
            var referencePoint = slicingPlane.NormalVector.EndPoint;
            var list1 = new List<LineSegment>();
            var list2 = segments.ToList();
            foreach (var segment in segments)
            {
                var point1 = segment.BasePoint;
                var point2 = segment.EndPoint;
                if (slicingPlane.PointIsOnSameSideAs(point1, referencePoint) || slicingPlane.PointIsOnSameSideAs(point2, referencePoint))
                {
                    list1.Add(segment);
                    list2.Remove(segment);
                }
            }
            return new List<List<LineSegment>>() { list1, list2 };
        }
        #endregion

        /// <summary>
        /// Finds the intersection between this polygon and the line
        /// </summary>
        public new Point IntersectWithLine(Line passedLine)
        {
            Point intersection = new Plane(this).IntersectWithLine(passedLine);

            if(intersection != null && this.Contains(intersection))
            {
                return intersection;
            }
            return null;
        }

        /// <summary>
        /// returns the linesegment of the intersection between the polygon and the plane
        /// should not be used unless the polygon is convex
        /// </summary>
        public LineSegment IntersectingSegment(Plane plane)
        {
            List<Point> pointsOfIntersection = new List<Point>();
            foreach(LineSegment segment in this.LineSegments)
            {
                Point point = plane.IntersectWithSegment(segment);
                if (point != null && !pointsOfIntersection.Contains(point))
                {
                    pointsOfIntersection.Add(point);
                }
            }
            if (pointsOfIntersection.Count == 2)
            {
                return new LineSegment(pointsOfIntersection[0], pointsOfIntersection[1]);
            }
            return null;
        }

        /// <summary>
        /// Returns a list of the points that line intersects the edges of the polygon
        /// </summary>
        public List<Point> IntersectionCoplanarPoints(Line passedLine)
        {
            List<Point> intersections = new List<Point>();

            foreach (LineSegment edge in this.LineSegments)
            {
                var point = edge.IntersectWithLine(passedLine);
                if (point != null && !intersections.Contains(point))
                {
                    intersections.Add(point);
                }
            }
            return intersections;
        }

        /// <summary>
        /// Returns a list of the points where the linesegment intersects the edges of the polygon
        /// </summary>
        public List<Point> IntersectionCoplanarPoints(LineSegment linesegment)
        {
            List<Point> intersections = new List<Point>();

            foreach (LineSegment edge in this.LineSegments)
            {
                Point intersection = edge.IntersectWithSegment(linesegment);
                if (intersection != null && !intersections.Contains(intersection))
                {
                    intersections.Add(intersection);
                }
               
            }
            intersections = intersections.OrderBy(point => point.DistanceTo(linesegment.BasePoint)).ToList();
            return intersections;
        }

        /// <summary>
        /// Returns a list of the lineSegments of intersection through the interior of the polygon
        /// </summary>
        public List<LineSegment> IntersectionCoplanarLineSegments(Line passedLine)
        {      
            List<LineSegment> lineSegmentsOfIntersection = new List<LineSegment>();

            List<Point> pointsOfIntersection = IntersectionCoplanarPoints(passedLine);

            for (int i = 0; 2*i + 1 < pointsOfIntersection.Count; i++ )
            {
                LineSegment newLineSegment = new LineSegment(pointsOfIntersection[2*i], pointsOfIntersection[2*i + 1]);
                lineSegmentsOfIntersection.Add(newLineSegment);
            }
            return lineSegmentsOfIntersection;
        }

        ///// <summary>
        ///// Returns whether or not the polygon and line intersection, but returns false if they are coplanar
        ///// </summary>
        //public new bool DoesIntersectNotCoplanar(Line passedLine)
        //{
        //    var point = this.IntersectWithLine(passedLine);
        //    return (point != null);
        //    //doesn't check coplanar at all..
        //}

        /// <summary>
        /// Returns whether or not the given line and polygon intersect or are coplanar and intersect on the plane
        /// </summary>
        public new bool DoesIntersect(Line line)
        {
            Point intersection = new Plane(this).IntersectWithLine(line);
            return this.Contains(intersection);
        }

        public new bool DoesIntersect(LineSegment segment)
        {
            var point = this.IntersectWithLine(segment);
            return segment.Contains(point);
        }

        /// <summary>
        /// Returns true if the point is contained within this PlaneRegion, Does not include the boundaries!
        /// </summary>
        public bool ContainsOnInside(Point passedPoint)
        {
            bool planeContains = new Plane(this).Contains(passedPoint);
            if (!planeContains)
            {
                return false;
            }
            //bool notTouching = !Touches(passedPoint);

            bool containsOnInside = _containsOnInside(passedPoint);
      
            return containsOnInside;
        }

        /// <summary>
        /// Returns true if the point is inside the Polygon, including on its boundaries.
        /// </summary>
        public new bool Contains(Point passedPoint)
        {
            //check if it is in our plane first
            if (!((Plane)this).Contains(passedPoint))
            {
                return false;
            }
            if (this.Touches(passedPoint))
            {
                return true;
            }
            if (this._containsOnInside(passedPoint))
            {
                return true;
            }
            return false;
        }

        private bool _containsOnInside(Point passedPoint)
        {
            Angle angularDistance = Angle.ZeroAngle;
            for (int i = 0; i < this.Vertices.Count; i++)
            {
                Point previous;
                Point next = this.Vertices[i];
                if (i == 0)
                {
                    previous = this.Vertices[this.Vertices.Count - 1];
                }
                else
                {
                    previous = this.Vertices[i - 1];
                }
                var direction1 = new Direction(passedPoint, previous);
                var direction2 = new Direction(passedPoint, next);

                var angle = direction1.AngleBetween(direction2);
                if (direction1.CrossProduct(direction2) == this.NormalVector.Direction.Reverse())
                {
                    angle = angle.Negate();
                }
                angularDistance += angle;

            }
            angularDistance -= Angle.FullCircle;
            if (angularDistance % (2*Angle.FullCircle) == Angle.ZeroAngle)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// determines if this polygon contains the entirety of the other polygon
        /// </summary>
        public new bool Contains(Polygon polygon)
        {
            //First check all vertices
            if (!this.ContainsAll(polygon.Vertices))
            {
                return false;
            }

            //if this is convex than we're done
            if (this.IsConvex)
            {
                return true;
            }

            //if not, we have to check that none of the outside vertices are inside the interior polygon
            foreach(Point vertex in this.Vertices)
            {
                if (polygon.ContainsOnInside(vertex))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Checks if the point is on any of the boundaries of the polygon.
        /// </summary>
        public bool Touches(Point point)
        {
            //check each of our boundaries if the point is on the LineSegment
            foreach (LineSegment line in this.LineSegments)
            {
                if (point.IsOnLineSegment(line))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// determines if the polygon contains this segment as an edge
        /// </summary>
        public bool HasSide(LineSegment segment)
        {
            return this.LineSegments.Any(s => s == segment);
        }

        /// <summary>
        /// Returns whether or not the two Polygons share a common side meaning that the side is exactly the same as the other's side
        /// </summary>
        public bool DoesShareExactSide(Polygon otherPolygon)
        {
            return this.LineSegments.Any(s => otherPolygon.HasSide(s));
        }

        /// <summary>
        /// Returns whether or not any side in this polygon contains a side of the other polygon. If this polygons side is larger but contains the other it will return true
        /// </summary>
        public bool DoesShareOrContainSide(Polygon otherPolygon)
        {
            return this.LineSegments.Any(s => otherPolygon.LineSegments.Any(t => s.Contains(t)));
        }

        /// <summary>
        /// Determines whether or not the point is on the sides of this polygon
        /// </summary>
        public bool DoesContainPointAlongSides(Point pointToCheckIfContained)
        {
            return this.LineSegments.Any(s => s.Contains(pointToCheckIfContained));
        }

        /// <summary>
        /// checks if a linesegment is a chord. i.e. endpoints on boundary & all other points interior.
        /// </summary>
        public bool Contains(LineSegment lineSegment)
        {
            if (!this.Contains(lineSegment.BasePoint) || !this.Contains(lineSegment.EndPoint))
            {
                return false;
            }
            var intersections = this.IntersectionCoplanarPoints(lineSegment);
            
            for(int i = 1; i < intersections.Count; i++)
            {
                var tempSegment = new LineSegment(intersections[i - 1], intersections[i]);
                if (!this.Contains(tempSegment.MidPoint))
                {
                    return false;
                }
            }
            return true;
        }

        private double _findArea()
        {
            var vertices = Vertices;
            var total = Vector.Zero;
            for (int i = 1; i + 1 < this.LineSegments.Count; i++)
            {
                var vector1 = new Vector(vertices[0], vertices[i]);
                var vector2 = new Vector(vertices[0], vertices[i+1]);
                total += vector1.CrossProduct(vector2);
            }
            return total.Magnitude.ValueInInches / 2;
        }

        private Point _findCentroid()
        {
            var vertices = Vertices;
            var total = Point.Origin;
            for (int i = 2; i < this.LineSegments.Count; i++)
            {
                var vector1 = new Vector(vertices.First(), vertices[i - 1]);
                var vector2 = new Vector(vertices.First(), vertices[i]);
                var crossProduct = vector1.CrossProduct(vector2);
                
                var  centroidOfLocalTriangle = new List<Point>() { vertices[0],vector1.EndPoint,vector2.EndPoint }.CenterPoint();
                if (crossProduct.Direction == NormalVector.Direction)
                {
                    total += crossProduct.Magnitude.ValueInInches * centroidOfLocalTriangle;
                }
                else
                {
                    total -= crossProduct.Magnitude.ValueInInches * centroidOfLocalTriangle;
                }
            }
            return total/(2*_findArea());
           
        }

        /// <summary>
        /// Returns the polygon with every linesegment's orientation reversed
        /// and the linesegments listed in opposite order
        /// so that vertices are also listed in this new order
        /// If the polygon already circulates in a consistent direction,
        /// Then this swaps the polygon's normal face with its outside one
        /// </summary>
        public Polygon ReverseOrientation()
        {
            return new Polygon(LineSegments.Select(edge => edge.Reverse()).Reverse(), false);
        }
      
        public bool IsRectangle()
        {
            if (LineSegments.Count != 4)
            {
                return false;
            }
            for (int i = 0; i < 3; i++)//we only need to check 3 angles, because the last angle is determined by the need to close the polygon.
            {
                if (LineSegments[i].AngleBetween(LineSegments[i + 1]) != Angle.RightAngle)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Returns the polygon projected onto a Plane
        /// If the the polygon is perpendicular to the plane we return null
        /// </summary>
        public Polygon ProjectOntoPlane(Plane plane)
        {
            if (!this.IsPerpendicularTo(plane))
            {
                var newVertices = new List<Point>();
                foreach (Point point in this.Vertices)
                {
                    newVertices.Add(point.ProjectOntoPlane(plane));
                }

                return new Polygon(newVertices, false);
            }
            //if the polygon is perpendicular to the plane, the projection is degenerate, we get just a linesegment;
            return null;     
        }

        public bool ContainsAll(IList<Point> pointList)
        {
            return !pointList.Any(p => !this.Contains(p));
        }
        
        public List<Polygon> OverlappingPolygons(Polygon otherPolygon)
        {
            var polygon1 = this;
            var polygon2 = otherPolygon;
            if (!polygon1.IsCoplanarTo(polygon2))
            {
                return new List<Polygon>();
            }
            if (polygon1.NormalDirection != polygon2.NormalDirection)
            {
                polygon2 = polygon2.ReverseOrientation();
            }
            var results = _traceRegions(polygon1, polygon2).ToList();
            var filtered = results.Where(p => p.NormalDirection == this.NormalDirection).ToList();
            filtered = results.Where(p => polygon1.Contains(p) && polygon2.Contains(p)).ToList();
            return filtered;
        }

        /// <summary>
        /// Returns the region of overlap between the two Polygons or null if there is no overlap. 
        /// </summary>
        public Polygon OverlappingPolygon(Polygon otherPolygon)
        {
            return ClipperPort.Overlap(this, otherPolygon);
        }

        public List<Polygon> RemoveOverlappingPolygons(List<Polygon> otherPolygons)
        {
            return ClipperPort.RemoveOverlap(this, otherPolygons);
        }
        #endregion

        #region private helper methods
        private static List<Polygon> _traceRegions(Polygon polygon1, Polygon polygon2)
        {
            List<LineSegment> segments1 = polygon1.LineSegments.ToList();
            List<LineSegment> segments2 = polygon2.LineSegments.ToList();
             #region Remove Overlapping Opposite segments
            for (int i = 0; i < segments1.Count; i++)
            {
                var segment1 = segments1[i];
                for (int j = 0; j < segments2.Count; j++)
                {           
                    var segment2 = segments2[j];
                    
                    if (segment1.Direction == segment2.Direction.Reverse() && 
                        segment1.Overlaps(segment2))
                    {
                        List<List<LineSegment>> segments = _subtract(segment1, segment2);
                        segments1.RemoveAt(i);
                        segments2.RemoveAt(j);
                        segments1.AddRange(segments[0]);
                        segments2.AddRange(segments[1]);
                        i -= 1;
                        break;
                    }
                }
            }
            #endregion

            #region Variable Initialization
            var lists = new List<List<LineSegment>>() { segments1, segments2 };
            var polygons = new List<Polygon>() { polygon1, polygon2 };
            var results = new List<Polygon>();
            var polygonUnderConstruction = new List<LineSegment>();

            var index = 0;
            List<LineSegment> currentList, otherList = null;
            LineSegment currentSegment = null;
            #endregion
            
            while (true)
            {
                if (polygonUnderConstruction.Count > 2 &&
                    polygonUnderConstruction.First().BasePoint ==
                    polygonUnderConstruction.Last().EndPoint)
                {
                    var newPolygons = _splitPolygon(polygonUnderConstruction);
                    results.AddRange(newPolygons);
                    polygonUnderConstruction = new List<LineSegment>();
                }

                currentList = lists[index % 2];
                otherList = lists[(index + 1) % 2];

                #region current Segment

                if (currentList.Count == 0)
                {
                    if (otherList.Count == 0)
                    {
                        break;
                    }
                    index++;
                    continue;
                }
                if (polygonUnderConstruction.Count == 0)
                {
                    currentSegment = currentList.First();
                }
                else
                {
                    var lastPoint = polygonUnderConstruction.Last().EndPoint;
                    currentSegment = currentList.FirstOrDefault(s => s.BasePoint == lastPoint);
                    if (currentSegment == null)
                    {
                        currentSegment = otherList.FirstOrDefault(s => s.BasePoint == lastPoint);
                        if (currentSegment == null)
                        {
                            polygonUnderConstruction = new List<LineSegment>();
                            continue;
                        }
                        index++;
                        currentList = lists[index % 2];
                        otherList = lists[(index + 1) % 2];
                    }
                }
                currentList.Remove(currentSegment);

                #endregion

                #region Check For Intersection
                var candidates = new List<Tuple<Point, LineSegment>>();
                foreach (var segment in otherList)
                {
                    var intersection = segment.IntersectWithSegment(currentSegment);
                    if (intersection != null &&
                        intersection != segment.EndPoint &&
                        intersection != currentSegment.BasePoint)
                    {
                        candidates.Add(Tuple.Create(intersection, segment));
                    }
                }

                if (candidates.Count == 0)
                {
                    polygonUnderConstruction.Add(currentSegment);
                    continue;
                }
                Point point = null;
                LineSegment lineSegment = null;
                var nextSegment = polygons[index % 2].LineSegments.FirstOrDefault(s => s.BasePoint == currentSegment.EndPoint);

                var found = false;
                var tuples = candidates.OrderBy(p => p.Item1.DistanceTo(currentSegment.BasePoint));
                foreach (var pair in tuples)
                {
                    point = pair.Item1;
                    lineSegment = pair.Item2;
                    if (point == currentSegment.EndPoint)
                    {
                        if (nextSegment != null && nextSegment.IsParallelTo(lineSegment))
                        {
                            continue;
                        }
                    }
                    if (point == lineSegment.BasePoint)
                    {
                        #region Vertices Touching
                        if (point == currentSegment.EndPoint)
                        {
                            #region Next Segment
                            if (nextSegment == null)
                            {
                                var crossProduct = currentSegment.CrossProduct(lineSegment);
                                if (crossProduct.Direction == polygons[index % 2].NormalDirection)
                                {
                                    found = true;
                                    break;
                                }
                                continue;
                            }
                            #endregion

                            var normal = polygons[index % 2].NormalVector;
                            var angle1 = (nextSegment.SignedAngleBetween(currentSegment.Reverse(), normal)).ProperAngle;
                            var angle2 = (nextSegment.SignedAngleBetween(lineSegment, normal)).ProperAngle;
                            if (angle2 < angle1)
                            {
                                found = true;
                                break;
                            }
                        }
                        #endregion
                        #region Vertex touching an Edge
                        else
                        {
                            var crossProduct = currentSegment.CrossProduct(lineSegment);
                            if (crossProduct.Direction == polygons[index % 2].NormalDirection)
                            {
                                found = true;
                                break;
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    polygonUnderConstruction.Add(currentSegment);
                    continue;
                }

                _addNewSegmentIfPossible(currentList, point, currentSegment.EndPoint);
                _addNewSegmentIfPossible(otherList, point, lineSegment.EndPoint);
                _addNewSegmentIfPossible(otherList, lineSegment.BasePoint, point);

                otherList.Remove(lineSegment);

                polygonUnderConstruction.Add(new LineSegment(currentSegment.BasePoint, point));

                index++;

                #endregion
            }
            return results;
        }

        private static List<Polygon> _splitPolygon(List<LineSegment> polygonUnderConstruction)
        {
            var results = new List<Polygon>();
            for (int i = 0; i < polygonUnderConstruction.Count; i++)
            {
                for (int j = i + 2; j + 1 < polygonUnderConstruction.Count; j++)
                {
                    var segment1 = polygonUnderConstruction[i];
                    var segment2 = polygonUnderConstruction[j];
                    var intersection = segment1.IntersectWithSegment(segment2);
                    if (intersection != null)
                    {
                        if (intersection == segment2.EndPoint && !intersection.IsBaseOrEndPointOf(segment1))
                        {
                            var s1 = new LineSegment(intersection, segment1.EndPoint);
                            var s2 = new LineSegment(segment1.BasePoint, intersection);

                            var newTempPolygon = new List<LineSegment>() { s1 };
                            for (int k = i + 1; k < j + 1; k++)
                            {
                                newTempPolygon.Add(polygonUnderConstruction[k]);
                            }
                            results.Add(new Polygon(newTempPolygon));
                            polygonUnderConstruction.RemoveAt(i);
                            foreach (var segment in newTempPolygon)
                            {
                                polygonUnderConstruction.Remove(segment);
                            }
                            polygonUnderConstruction.Add(s2);
                            j = i + 1;
                        }
                        else if (intersection == segment1.BasePoint && !intersection.IsBaseOrEndPointOf(segment2))
                        {
                            var s1 = new LineSegment(segment2.BasePoint, intersection);
                            var s2 = new LineSegment(intersection, segment2.EndPoint);

                            var newTempPolygon = new List<LineSegment>() { s1 };
                            for (int k = i; k < j; k++)
                            {
                                newTempPolygon.Add(polygonUnderConstruction[k]);
                            }
                            results.Add(new Polygon(newTempPolygon));
                            polygonUnderConstruction.RemoveAt(j);
                            foreach (var segment in newTempPolygon)
                            {
                                polygonUnderConstruction.Remove(segment);
                            }
                            polygonUnderConstruction.Add(s2);
                            j = i + 1;
                        }
                    }
                }
            }
            var leftoverPolygon = polygonUnderConstruction.CreatePolygonIfValid();
            if (leftoverPolygon != null)
            {
                results.Add(leftoverPolygon);
            }
            return results;
        }

        private static List<List<LineSegment>> _subtract(LineSegment segment, LineSegment toSubtract)
        {
            var results = new List<List<LineSegment>>() { new List<LineSegment>(), new List<LineSegment>() };
            if (segment.EndPoint == toSubtract.BasePoint && segment.BasePoint == toSubtract.EndPoint)
            {
                ; // return nothing;
            }
            else if (new Vector(segment.BasePoint, toSubtract.BasePoint).HasSameDirectionAs(new Vector(segment.EndPoint, toSubtract.EndPoint)))
            {
                results[0].Add(segment);
                results[1].Add(toSubtract);
            }
            else if (segment.ContainsOnInside(toSubtract))
            {
                results[0].Add(new LineSegment(toSubtract.BasePoint, segment.EndPoint));
                results[0].Add(new LineSegment(segment.BasePoint, toSubtract.EndPoint));
            }
            else if (toSubtract.ContainsOnInside(segment))
            {
                results[1].Add(new LineSegment(toSubtract.BasePoint, segment.EndPoint));
                results[1].Add(new LineSegment(segment.BasePoint, toSubtract.EndPoint));
            }
            else if (segment.ContainsOnInside(toSubtract.BasePoint) && toSubtract.ContainsOnInside(segment.BasePoint))
            {
                results[0].Add(new LineSegment(toSubtract.BasePoint, segment.EndPoint));
                results[1].Add(new LineSegment(segment.BasePoint, toSubtract.EndPoint));
            }
            else if (segment.ContainsOnInside(toSubtract.EndPoint) && toSubtract.ContainsOnInside(segment.EndPoint))
            {
                results[1].Add(new LineSegment(toSubtract.BasePoint, segment.EndPoint));
                results[0].Add(new LineSegment(segment.BasePoint, toSubtract.EndPoint));
            }
            else
            {
                throw new Exception();
                ;
            }
            return results;
        }
    
        private static void _addNewSegmentIfPossible(List<LineSegment> list, Point basePoint, Point endPoint)
        {
            if (basePoint != endPoint)
            {
                list.Add(new LineSegment(basePoint, endPoint));
            }
        }

        #endregion

        #region Static Factory Methods
        public static Polygon CreateInXYPlane(DistanceType distanceType, params double[] coordinates)
        {
            if (coordinates.Length % 2 != 0)
            {
                throw new ArgumentException("You must pass in an x & y coordinate for each vertex.");
            }
            var list = coordinates.Select((cord, index) => new {cord, index}).ToList();
            var Xs = list.Where(pair => pair.index % 2 == 0);
            var Ys = list.Where(pair => pair.index % 2 == 1);
            var vertices = Enumerable.Zip(Xs, Ys, (x, y) => new Point(distanceType, x.cord, y.cord));
            return new Polygon(vertices);
        }
        public static Polygon Triangle(
            DistanceType type,
            double x1, double y1,
            double x2, double y2,
            double x3, double y3)
        {
            return new Polygon(new List<Point>{
                new Point(type, x1, y1),
                new Point(type, x2, y2),
                new Point(type, x3, y3) }, false);
        }

        public static Polygon Quad(
            DistanceType type,
            double x1, double y1,
            double x2, double y2,
            double x3, double y3,
            double x4, double y4)
        {
            return new Polygon(new List<Point>{
                new Point(type, x1, y1),
                new Point(type, x2, y2),
                new Point(type, x3, y3),
                new Point(type, x4, y4)});
        }

        public static Polygon Rectangle(
            DistanceType type,
            double x1, double y1,
            double x2, double y2)
        {
            return new Polygon(new List<Point>{
                new Point(type, x1, y1),
                new Point(type, x2, y1),
                new Point(type, x2, y2),
                new Point(type, x1, y2)});
        }

        public static Polygon EquilateralTriangle(Distance sideLength)
        {
            return RegularPolygon(3, sideLength);
        }

        /// <summary>
        /// Creates a parallelogram. 
        /// shifts both vectors so their basepoints are the passed basepoint, and creates the parrelogram spanned by those sides.
        /// </summary>
        public static Polygon Parallelogram(Vector vector1, Vector vector2, Point basePoint = null)
        {
            if (basePoint == null)
            {
                basePoint = vector1.BasePoint;
            }
            LineSegment segment1 = new LineSegment(basePoint, vector1);
            LineSegment segment2 = new LineSegment(basePoint, vector2);
            LineSegment segment3 = new LineSegment(segment2.EndPoint, vector1);
            LineSegment segment4 = new LineSegment(segment1.EndPoint, vector2);

            return new Polygon(new List<LineSegment>() { segment1, segment2, segment3, segment4 });
        }

        public static Polygon Pentagon(Distance sideLength)
        {
            return RegularPolygon(5, sideLength);
        }

        public static Polygon Rectangle(Distance xLength, Distance yLength, Point basePoint = null)
        {
            if (basePoint == null)
            {
                basePoint = Point.Origin;
            }
            var vector1 = new Vector(basePoint, Direction.Right, xLength);
            var vector2 = new Vector(basePoint, Direction.Up, yLength);
            return Parallelogram(vector1, vector2);
        }

        /// <summary>
        /// Creates a regular polygon centered at the origin in the XY-plane.
        /// </summary>
        public static Polygon RegularPolygon(int numberOfSides, Distance sideLength, Angle startingAngle = null, Point centerPoint = null)
        {
            if (numberOfSides < 3)
            {
                throw new ArgumentException("A polygon must have atleast 3 sides.");
            }

            Angle step = new Angle(360.0 / numberOfSides, Degrees);
            Angle otherAngle = (StraightAngle - step) / 2;

            //Law of Sines
            Distance length = sideLength * Sine(otherAngle) / Sine(step);

            Point firstPoint;
            if (startingAngle == null)
            {   // We want the polygon to be centered at the origin,
                // and lie "flat" from the viewers perspective
                if (numberOfSides % 4 == 0)
                {
                    firstPoint = new Point(length, Distance.ZeroDistance);
                    firstPoint = firstPoint.Rotate2D(step / 2);
                }
                else if (numberOfSides % 2 == 0)
                {
                    firstPoint = new Point(length, Distance.ZeroDistance);
                }
                else
                {
                    firstPoint = new Point(Distance.ZeroDistance, length);
                }
            }
            else
            {
                firstPoint = new Point(length, Distance.ZeroDistance);
                firstPoint = firstPoint.Rotate2D(startingAngle);
            }
            List<Point> points = new List<Point>() { firstPoint };
            for (int i = 1; i < numberOfSides; i++)
            {
                points.Add(firstPoint.Rotate2D(step*i));
            }
            if (centerPoint == null)
            {
                return new Polygon(points, false);
            }
            else
            {
                return new Polygon(points.Select(p => p + centerPoint).ToList(), false);
            }
        }

        /// <summary>
        /// Creates a polygon with the sidelength and this angle at the origin
        /// </summary>
        public static Polygon Rhombus(Angle angle, Distance sideLength)
        {
            var vector1 = Direction.Right * sideLength;
            var vector2 = vector1.Rotate(new Rotation(angle));
            return Parallelogram(vector1, vector2);
        }

        public static Polygon Square(Distance sideLength, Point basePoint = null)
        {
            return Rectangle(sideLength, sideLength, basePoint);
        }

        public static Polygon Triangle(Vector vector1, Vector vector2, Point basePoint = null)
        {
            if (basePoint == null)
            {
                basePoint = vector1.BasePoint;
            }
            else
            {
                vector1 = new Vector(basePoint, vector1);
            }
            vector2 = new Vector(basePoint, vector2);

            var point1 = basePoint;
            var point2 = vector1.EndPoint;
            var point3 = vector2.EndPoint;

            return new Polygon(new List<Point>() { point1, point2, point3 });
        }
        #endregion
    }
}
