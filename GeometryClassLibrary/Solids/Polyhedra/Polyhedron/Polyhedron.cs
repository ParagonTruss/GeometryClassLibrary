using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnitClassLibrary;
using UnitClassLibrary.DistanceUnit;

namespace GeometryClassLibrary
{
    [JsonObject(MemberSerialization.OptIn)]
    public partial class Polyhedron
    {
        #region Properties and Fields

        /// <summary>
        /// A list containing the polygons that make up this polyhedron
        /// </summary>
        [JsonProperty]
        public virtual List<Polygon> Polygons { get; set; }

        /// <summary>
        /// All the polyhedron's linesegments.
        /// </summary>
        public List<LineSegment> LineSegments
        {
            get
            {
                List<LineSegment> returnList = new List<LineSegment>();

                foreach (Polygon region in this.Polygons)
                {
                    foreach (LineSegment segment in region.LineSegments)
                    {
                        if (!returnList.Contains(segment))
                        {
                            returnList.Add(segment);
                        }
                    }
                }
                return returnList;
            }
        }

        /// <summary>
        /// Returns a list of all the vertices in the Polyhedron
        /// </summary>
        public List<Point> Vertices
        {
            get
            {
                return this.LineSegments.GetAllPoints();
            }
        }

        /// <summary>
        /// determines if the polyhedron is convex
        /// i.e. all segments whose endpoints are inside the polygon, are inside the polygon
        /// </summary>
        public bool IsConvex
        {
            get
            {
                //First we check the Euler Characteristic: http://en.wikipedia.org/wiki/Euler_characteristic#Polyhedra
                //Concave polyhedra sometimes have a characteristic other than 2.
                int eulerCharacteristic = Vertices.Count - LineSegments.Count + Polygons.Count;
                if (eulerCharacteristic != 2)
                {
                    return false;
                }

                //Now we check to see if any of the faces are concave, which would make the polyhedron concave
                if (Polygons.Any(face => !face.IsConvex))
                {
                    return false;
                }

                //Now we check that for every face all the remaining vertices are on the same side of that plane
                //http://stackoverflow.com/a/30380541/4875161
                var dir = Polygons.Select(f => f.NormalDirection);
                return !Polygons.Any(face => Vertices.Except(face.Vertices).Any(vertex => face.PointIsOnNormalSide(vertex)));
               
            }
        }
        /// <summary>
        /// Finds the CenterPoint of the Polyhedron by averaging the vertices
        /// </summary>
        public Point CenterPoint
        {
            get
            {
                if (_centerPoint == null)
                {
                    _centerPoint=  this.Vertices.CenterPoint();
                }
                return _centerPoint;
            }
        }
        private Point _centerPoint;

        /// <summary>
        /// The volume of the Polyhedron. Uses the 1st method described on this webpage: http://www.ecse.rpi.edu/~wrf/Research/Short_Notes/volume.html
        /// Now using this formula instead: http://stackoverflow.com/a/1849746/4875161
        /// </summary>
        public virtual Volume Volume
        {
            get
            {
                if (_volume == null)
                {
                    var volume = this.Polygons.SplitIntoTriangles()
                        .Select(p => _volumeOfTetrahedronFormedWithTheOrigin(p)).Sum();
                    _volume = new Volume(new CubicInch(), volume);
                }
                if (_volume < Volume.Zero)
                {
                    throw new Exception("Bad Polyhedron! Volume should not be negative.");
                }
                return _volume;
            }
        }
        private Volume _volume;

        /// <summary>
        /// returns as a double the volume of the tetrahedron with that triangle as a base
        /// </summary>
        /// <param name="triangle"></param>
        /// <returns></returns>
        private static double _volumeOfTetrahedronFormedWithTheOrigin(Polygon triangle)
        {
            double X1 = triangle.Vertices[0].X.InInches.Value;
            double X2 = triangle.Vertices[1].X.InInches.Value;
            double X3 = triangle.Vertices[2].X.InInches.Value;

            double Y1 = triangle.Vertices[0].Y.InInches.Value;
            double Y2 = triangle.Vertices[1].Y.InInches.Value;
            double Y3 = triangle.Vertices[2].Y.InInches.Value;

            double Z1 = triangle.Vertices[0].Z.InInches.Value;
            double Z2 = triangle.Vertices[1].Z.InInches.Value;
            double Z3 = triangle.Vertices[2].Z.InInches.Value;

            double[,] array = new double[,] { { X1, X2, X3 }, { Y1, Y2, Y3 }, { Z1, Z2, Z3 } };

            Matrix volumeMatrix = new Matrix(array);

            return volumeMatrix.Determinant() / 6;
        }

        public virtual Point Centroid
        {
            get
            {
                List<Polygon> triangles = this.Polygons.SplitIntoTriangles();
                Vector weightedSum = new Vector(Point.Origin);
                double totalVolume = 0;

                foreach (Polygon triangle in triangles)
                {
                    double volume = _volumeOfTetrahedronFormedWithTheOrigin(triangle);

                    Vector currentCentroidAsVector = new Vector(triangle.Vertices[0] + triangle.Vertices[1] + triangle.Vertices[2]) / 4;

                    weightedSum += volume * currentCentroidAsVector;
                    totalVolume += volume;

                }
                return (weightedSum / totalVolume).EndPoint;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Null constructor for initializing derived class objects.
        /// </summary>
        protected Polyhedron() { }

        /// <summary>
        /// Makes a Polyhedron using the giving line segments made into polygons based on if they are coplanar and share a point
        /// WARNING: THIS CONSTRUCTOR LEADS TO AMBIGUOUS CASES, DON'T USE IT!
        /// </summary>
        /// <param name="passedLineSegments">The line segments that define this Polyhedron</param>
        public Polyhedron(List<LineSegment> passedLineSegments)
        {
            this.Polygons = passedLineSegments.MakeCoplanarLineSegmentsIntoPolygons();
        }

        /// <summary>
        /// Creates a Polyhedron using the passed polygons as its side/polygons
        /// </summary>
        /// <param name="passedPolygons">The list of polygons that define this Polyhedron</param>
        [JsonConstructor]
        public Polyhedron(List<Polygon> polygons, bool checkAndRebuildValidPolyhedron = true)
            : base()
        {
            if (checkAndRebuildValidPolyhedron)
            {
                polygons = _makeFacesWithProperOrientation(polygons);
            }
            if (polygons == null || polygons.Count == 0)
            {
                throw new Exception("The polygons you're attempting to use do not form a single closed region.");
            }

            this.Polygons = polygons;
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="passedSolid">The polyhedron to copy</param>
        public Polyhedron(Polyhedron toCopy)
            : this(toCopy.Polygons, false) { }

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
            if (obj == null || !(obj is Polyhedron))
            {
                return false;
            }
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
        #endregion

        #region Methods

        /// <summary>
        /// Determines if the the plane intersects the polyhedron and not just along a face, segment or vertex.
        /// </summary>
        public bool DoesIntersectNotTouching(Plane passedPlane)
        {
            return (!this.Vertices.AllPointsAreOnTheSameSideOf(passedPlane));
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
        /// Finds all the intersection points between the given line and this Polyhedrons surfaces
        /// </summary>
        /// <param name="intersectingLine">The line that presumable intersects this Polyhedron</param>
        /// <returns>A list of all the points where the line intersects the polygon's surface or an empty list if there are not intersections</returns>
        public List<Point> FindAllIntersectionPoints(Line intersectingLine)
        {
            List<Point> intersections = new List<Point>();

            //check each polygon for an intersection
            foreach (Polygon polygon in this.Polygons)
            {
                //if it exists add it to the list
                Point intersectionPoint = polygon.IntersectWithLine(intersectingLine);
                if (intersectionPoint != null && !intersections.Contains(intersectionPoint))
                {
                    intersections.Add(intersectionPoint);
                }
            }
            return intersections;
        }

        public Polygon CrossSection(Plane plane)
        {
            var newSegments = new List<LineSegment>();
            foreach(Polygon polygon in this.Polygons)
            {
               Line line = plane.IntersectWithPlane(polygon);
                if (line != null)
                {
                    var segments = polygon.IntersectionCoplanarLineSegments(line);
                    if (segments.Count == 1)
                    {
                        newSegments.Add(segments[0]);
                    }
                    else if (segments.Count > 1)
                    {
                        throw new NotImplementedException("We don't support this yet.");
                    }
                }
            }
            return newSegments.CreatePolygonIfValid();
        }

        /// <summary>
        /// Returns the two Polyhedrons created by a slice. returns the solid on the normal side of the plane first
        /// </summary>
        public List<Polyhedron> Slice(Plane slicingPlane)
        {
            if (this.DoesIntersectNotTouching(slicingPlane))
            {
                //make a list to keep track of all the new segments
                List<LineSegment> slicingPlaneLineSegments = new List<LineSegment>();

                //the slice method will return 1 or more polyhedrons
                List<Polygon> unconstructedInsidePolyhedron = new List<Polygon>();
                List<Polygon> unconstructedOutsidePolyhedron = new List<Polygon>();

                //slice each polygon
                foreach (var polygon in this.Polygons)
                {
                    List<Polygon> slicedPolygons = polygon.Slice(slicingPlane);
                    _addPolygonToCorrectPolyhedron(slicedPolygons, unconstructedInsidePolyhedron, unconstructedOutsidePolyhedron, slicingPlane);
                    _addSlicingLine(slicingPlane, polygon, slicingPlaneLineSegments);
                }

                //if we actually slice and did not just not overlap with a side then return two polyhedrons
                if (unconstructedInsidePolyhedron.Count != 0 && unconstructedOutsidePolyhedron.Count != 0)
                {
                    //now make the joining sides based on where the intersection plane was
                    //create a new plane based on the intersections
                    //try
                    //{
                        Polygon slicingPlanePolygon = new Polygon(slicingPlaneLineSegments);
                        Polygon slicingPlanePolygon2 = new Polygon(slicingPlaneLineSegments);

                        unconstructedInsidePolyhedron.Add(slicingPlanePolygon);
                        unconstructedOutsidePolyhedron.Add(slicingPlanePolygon2);
                    //}
                    //catch (Exception) { }
                    Polyhedron insidePolyhedron = new Polyhedron(unconstructedInsidePolyhedron);
                    Polyhedron outsidePolyhedron = new Polyhedron(unconstructedOutsidePolyhedron);

                    List<Polyhedron> toReturn = new List<Polyhedron>() { insidePolyhedron, outsidePolyhedron };
                    return toReturn;
                }
            }
            return new List<Polyhedron>() { new Polyhedron(this) };
        }

        #region Slice _helper Methods
        /// <summary>
        /// Adds the Polygons to the correct new Polyhedron
        /// </summary>
        private void _addPolygonToCorrectPolyhedron(List<Polygon> slicedPolygons, List<Polygon> unconstructedInsidePolyhedron, List<Polygon> unconstructedOutsidePolyhedron,
            Plane slicingPlane)
        {
            int? side = null;
            for (int i = 0; i < slicedPolygons[0].Vertices.Count; i++)
            {
                if (slicingPlane.Contains(slicedPolygons[0].Vertices[i]))
                {
                   continue;
                }
                else if (slicingPlane.PointIsOnNormalSide(slicedPolygons[0].Vertices[i]))
                {
                    if (side == null)
                    {
                        side = 0;
                    }
                    else if (side == 1)
                    {
                        throw new Exception("New polygons shouldn't straddle the slice.");
                    }

                }
                else
                {
                    if (side == null)
                    {
                        side = 1;
                    }
                    else if (side == 0)
                    {
                        throw new Exception("New polygons shouldn't straddle the slice.");
                    }
                }
            }
            if (side == 0)
            {
                unconstructedOutsidePolyhedron.Add(slicedPolygons[0]);
                if (slicedPolygons.Count > 1)
                {
                    unconstructedInsidePolyhedron.Add(slicedPolygons[1]);
                }
            }
            else if (side == 1)
            {
                unconstructedInsidePolyhedron.Add(slicedPolygons[0]);
                
                if (slicedPolygons.Count > 1)
                {
                    unconstructedOutsidePolyhedron.Add(slicedPolygons[1]);
                }
            }
        }

        /// <summary>
        /// Adds the slicing line to the list of lines so we can create a polygon to represent the side that is on the slicing plane
        /// </summary>
        /// <param name="slicingPlane">The plane that is slicing the polygon</param>
        /// <param name="polygon">The polygon the is being sliced</param>
        /// <param name="slicingPlaneLines">The list of lines creates by the slicing plane</param>
        private void _addSlicingLine(Plane slicingPlane, Polygon polygon, List<LineSegment> slicingPlaneLines)
        {
            LineSegment slicingLine = polygon.IntersectingSegment(slicingPlane);
            if (slicingLine != null && polygon.DoesIntersect(slicingLine))
            {
                if (!slicingPlaneLines.Contains(slicingLine))
                {
                    slicingPlaneLines.Add(slicingLine);
                }
            }
        }
        #endregion

        /// <summary>
        /// Returns all the Polyhedrons created by slices. In order of size.
        /// </summary>
        /// <param name="slicingPlanes">The planes to slice the Polyhedron With</param>
        /// <returns>Returns a list of Polyhedrons that were created by the slice, with the largest first</returns>
        public List<Polyhedron> Slice(List<Plane> slicingPlanes)
        {
            var results = new List<Polyhedron>() { this };
            while (slicingPlanes.Count != 0)
            {
                results = results.SelectMany(solid => solid.Slice(slicingPlanes[0])).ToList();
                slicingPlanes.RemoveAt(0);
            }
            return results;
        }

        ////Do not rename this to be an overload of slice!
        ////Right now some places we slice but pass a polygon.
        ////We do not want to change their functionality.
        //public List<Polyhedron> SliceAlongPolygon(Polygon polygon)
        //{
        //    throw new NotImplementedException();
        //}

        

        /// <summary>
        /// Shifts the Polyhedron to another location and orientation
        /// </summary>
        public Polyhedron Shift(Shift passedShift)
        {
            List<Polygon> shiftedRegions = this.Polygons.Shift(passedShift);

            return new Polyhedron(shiftedRegions, false);
        }

        public bool Contains(Point point)
        {
            if (Polygons.Any(face => face.Contains(point)))
            {
                return true;
            }
            throw new NotImplementedException();
        }

        public bool Contains(Polygon polygon)
        {
            return polygon.Vertices.All(vertex => this.Contains(vertex));
        }
        public bool Contains(Polyhedron polyhedron)
        {
            polyhedron.Vertices.All(vertex => this.Contains(vertex));
            //if (this.IsConvex)
            //{
            //    foreach(Point vertex in polyhedron.Vertices)
            //    {
            //        foreach(Plane face in this.Polygons)
            //        {
            //            if (face.PointIsOnNormalSide(vertex))
            //            {
            //                return false;
            //            }
            //        }
            //    }
            //    return true;
            //}
            throw new NotImplementedException();
        }

        // Do we need this?
        /// <summary>
        /// Determines if the polygon has any common edges to this polyhedron
        /// </summary>
        public bool DoesShareExactSide(Polygon polygon)
        {
            return this.Polygons.Any(p => p.DoesShareExactSide(polygon));
        }
        
        // Do we need this? 
        /// <summary>
        /// Determines if this polyhedron has any edges that contain edges of this polygon.
        /// </summary>
        public bool DoesShareOrContainSide(Polygon polygon)
        {
            return this.Polygons.Any(p => p.DoesShareOrContainSide(polygon));
        }

        // Do we need this?
        /// <summary>
        /// Determines whether or not the point is on an edge of this Polyhedron.
        /// </summary>
        public bool DoesContainPointAlongSides(Point pointToCheckIfItContains)
        {
            return this.LineSegments.Any(s => s.Contains(pointToCheckIfItContains));
        }

        /// <summary>
        /// If these Polyhedrons touch along their faces, this returns the region of those overlapping faces as a polygon
        /// Note: this method assumes, the faces only have one possible side touching
        /// Needs Implementation for non convex bodies
        /// </summary>
        /// <param name="toFindOverlapWith">The Polyhedron to find the overlap with</param>
        /// <returns>The overlapping region of the two Polyhedrons as a Polygon or null if they do not overlap</returns>
        public Polygon OverlappingPolygon(Polyhedron polyhedron)
        {
            List<Polygon> faces1 = this.Polygons;
            List<Polygon> faces2 = polyhedron.Polygons;

            //Loop through the faces until we find overlapping faces
            for (int i = 0; i < faces1.Count; i++)
            {
                for (int j = 0; j < faces2.Count; j++)
                {
                    if (faces1[i].NormalVector.HasOppositeDirectionOf(faces2[j].NormalVector))
                    {
                        Polygon overlappingFace = faces1[i].OverlappingPolygon(faces2[j]);
                        if (overlappingFace != null)
                        {
                            return overlappingFace;
                        }
                    }
                }
            }
            return null;
        }

        public Polygon OverlappingPolygon(Polygon polygon)
        {
            Polygon intersectionPlane;
            for (int j = 0; j < this.Polygons.Count; j++)
            {
                intersectionPlane = polygon.OverlappingPolygon(this.Polygons[j]);
                if (intersectionPlane != null)
                {
                    return intersectionPlane;
                }

            }
            return null;
        }

        public bool IsRectangularPrism()
        {
            if (this.Polygons.Count != 6 || this.LineSegments.Count != 12 || this.Vertices.Count != 8)
            {
                return false;
            }
            foreach(Polygon face in this.Polygons)
            {
                if (!face.IsRectangle())
                {
                    return false;
                }
            }
            return true;
        }

        public bool HasOverlappingFace(Polygon polygon)
        {
            foreach(Polygon face in this.Polygons)
            {
                if (face.OverlappingPolygon(polygon) != null)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Projects a polyhedron onto a plane, and returns a polygon.
        /// Doesn't handle nonConvex polyhedra properly in some cases.
        /// </summary>
        public Polygon ProjectOntoPlane(Plane plane)
        {
            var segments2D = this.LineSegments.ProjectAllOntoPlane(plane);

            return segments2D.ExteriorProfileFromSegments(plane.NormalVector);
        }

        /// <summary>
        /// Determines if the boundaries of the solids intersect.
        ///  If they just touch, this method should return false.
        ///  If one entirely encloses the other, this will return false.
        ///
        /// </summary>
        /// <param name="polyhedron"></param>
        /// <returns></returns>
        public bool DoesIntersect(Polyhedron polyhedron)
        {
            foreach(var face in this.Polygons)
            {
                foreach(var segment in polyhedron.LineSegments)
                {
                    if (face.DoesIntersect(segment))
                    {
                        return true;
                    }
                }
            }
            foreach (var face in polyhedron.Polygons)
            {
                foreach (var segment in this.LineSegments)
                {
                    if (face.DoesIntersect(segment))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        #endregion

        #region Static Factory Methods
        /// <summary>
        /// Creates a parralelepiped.
        /// shifts the vectors so their basepoints are the passed basepoint, and creates the parralelepiped spanned by those vectors.
        /// </summary>
        public static Polyhedron MakeParallelepiped(Vector vector1, Vector vector2, Vector vector3, Point basePoint = null)
        {
            if (basePoint == null)
            {
                basePoint = vector1.BasePoint;
            }
            Polygon bottom = Polygon.Parallelogram(vector1, vector2, basePoint);
            Polyhedron solid = bottom.Extrude(vector3);
            return solid;
        }

        public static Polyhedron MakeRectangularPrism(Point point1, Point point2)
        {
            return new RectangularPrism(point2, point1);
        }

        public static Polyhedron MakeCube(Distance sidelength)
        {
            throw new NotImplementedException();
        }

        public static Polyhedron MakeRegularTetrahedron(Distance sidelength)
        {
            throw new NotImplementedException();
        }

        public static Polyhedron MakeRegularOctahedron(Distance sidelength)
        {
            throw new NotImplementedException();
        }

        public static Polyhedron MakeRegularDodecahedron(Distance sidelength)
        {
            throw new NotImplementedException();
        }

        public static Polyhedron MakeRegularIcosahedron(Distance sidelength)
        {
            throw new NotImplementedException();
        } 
        #endregion

        #region Constructor helpers
        /// <summary>
        ///Checks if the polygons form a closed bounded region.
        ///If they don't returns null. Otherwise it reorients every face, so that all their normalVectors point outward
        ///and every set of edges on each face circulates counterclockwise when looked at from outside to inside
        ///i.e. right hand rule
        ///returns the oriented faces
        /// </summary>
        private static List<Polygon> _makeFacesWithProperOrientation(List<Polygon> polygonList)
        { 

            //Now we try to build piecewise the polyhedron from the faces
            //we abort and return null if run out of faces, but have unmet edges, or
            //all edges have two faces but we have faces left over
            List<Polygon> placedFaces = new List<Polygon>();
            List<Polygon> unplacedFaces = polygonList;

            //keeps track of all edges in placedFaces that don't have neighboring faces
            //Should be empty by the end of this algorithm
            List<LineSegment> edgesWithoutNeighboringFace = new List<LineSegment>();

            //we find the first face
            Polygon lowestFace = _findLowestFace(polygonList);

            //place the first face
            _placeFace(lowestFace, placedFaces, unplacedFaces, edgesWithoutNeighboringFace);

            LineSegment currentEdge = null;
            Polygon nextFace = null;

            while (edgesWithoutNeighboringFace.Count != 0)
            {
                currentEdge = edgesWithoutNeighboringFace[0];
                nextFace = _findAndOrientNextFace(currentEdge, unplacedFaces);
                _placeFace(nextFace, placedFaces, unplacedFaces, edgesWithoutNeighboringFace);
            }

            if (unplacedFaces.Count == 0)
            {
                return placedFaces;
            }
            throw new Exception();

        }

        private static Polygon _findLowestFace(List<Polygon> polygonList)
        {
            var candidates = polygonList.OrderBy(p => p.Vertices.Min(v => v.Y)).ThenBy(f => f.CenterPoint.Y);
            foreach (Polygon face in candidates)
            {
                if (face.NormalVector.IsPerpendicularTo(Line.YAxis))
                {
                    continue;
                }
                if (face.NormalVector.Direction.DotProduct(Direction.Down) < 0)
                {
                    return face.ReverseOrientation();
                }
                else
                {
                    return new Polygon(face);
                }
            }
            throw new Exception();
        }

     
        /// <summary>
        /// Places the face by updating all relevant lists 
        /// </summary>
        private static void _placeFace(Polygon face, List<Polygon> placedFaces, List<Polygon> unplacedFaces, List<LineSegment> edgesWithoutNeighboringFace)
        {
            placedFaces.Add(face);
            unplacedFaces.Remove(face);

            //adds the new edges and removes the edges that now have two neighbor faces
            _disjointUnion(edgesWithoutNeighboringFace, face.LineSegments.ToList());
        }

        private static void _disjointUnion(List<LineSegment> list1, List<LineSegment> list2)
        {
            for (int i = 0; i < list1.Count; i++)
            {
                for (int j = 0; j < list2.Count; j++)
                {
                    if (list1[i] == list2[j])
                    {
                        list1.RemoveAt(i);
                        list2.RemoveAt(j);
                        i--;
                        break;
                    }
                }
            }
            list1.AddRange(list2);
        }

        private static Polygon _findAndOrientNextFace(LineSegment currentEdge, List<Polygon> unplacedFaces)
        {
            foreach (Polygon polygon in unplacedFaces)
            {
                foreach (LineSegment edge in polygon.LineSegments)
                {
                    if (currentEdge == edge)
                    {
                        //checks for same direction by comparing basepoints
                        if (currentEdge.BasePoint == edge.BasePoint)
                        {
                            //every edge should have opposite orientations on neighboring faces
                            //this is a simple consequence of the right hand rule and the convention that normal vectors should point outward
                            return polygon.ReverseOrientation();
                        }
                        else
                        {
                            return new Polygon(polygon);
                        }
                    }
                }
            }
            throw new Exception("The passed list of faces do not form a closed region.");
        }
        #endregion

    }
}

