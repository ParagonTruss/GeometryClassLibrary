﻿/*
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

using System.Collections.Generic;
using System.Linq;
using UnitClassLibrary.AngleUnit;

namespace GeometryClassLibrary
{
    public static class LineSegmentListExtensions
    {
        public static List<Polygon> MakeCoplanarLineSegmentsIntoPolygons(this List<LineSegment> passedLineSegments)
        {
            List<Plane> planesList = new List<Plane>();

            foreach (var segment1 in passedLineSegments)
            {
                foreach (var segment2 in passedLineSegments)
                {
                    if ((segment1.BasePoint == segment2.BasePoint && segment1.EndPoint != segment2.EndPoint) ||
                        (segment1.BasePoint != segment2.BasePoint && segment1.EndPoint == segment2.EndPoint))     
                    {
                        Plane possiblePlane = new Plane(segment1, segment2);
                        if (!planesList.Contains(possiblePlane))
                        {
                            planesList.Add(possiblePlane);
                        }
                    }
                }
            }

            List<Polygon> returnList = new List<Polygon>();

            foreach (var plane in planesList)
            {
                List<LineSegment> newRegionBoundaries = new List<LineSegment>();
                foreach (var segment in passedLineSegments)
                {
                    if (plane.Contains(segment) && !newRegionBoundaries.Contains(segment))
                    {
                        newRegionBoundaries.Add(segment);
                    }
                }
                returnList.Add(new Polygon(newRegionBoundaries));
            }

            return returnList;
        }

        /// <summary>
        /// Checks that all the boundaries meet end to end.
        /// </summary>
        /// <param name="segments"></param>
        /// <returns></returns>
        public static bool DoFormClosedRegion(this IList<LineSegment> segments)
        {
            var groups = segments.SelectMany(segment => segment.EndPoints).GroupByEquatable(_.Identity);
            return groups.Count == segments.Count && groups.All(group => group.Elements.Count == 2);
        }

        public static bool AtleastOneIntersection(this IList<LineSegment> listOfSegments)
        {
            for (int i = 0; i < listOfSegments.Count; i++)
            {
                for (int j = i + 1; j < listOfSegments.Count; j++)
                {
                    var segment1 = listOfSegments[i];
                    var segment2 = listOfSegments[j];
                    var intersection = segment1.IntersectWithSegment(segment2);
                    if (intersection != null &&
                        (!intersection.IsBaseOrEndPointOf(segment1) ||
                        !intersection.IsBaseOrEndPointOf(segment2)))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Returns true if the Polygon is valid (is a closed region and the LineSegments are all coplaner)
        /// </summary>
        /// <returns>returns true if the LineSegments form a closed area and they are all coplaner</returns>
        public static void ValidateForPolygon(this IList<LineSegment> segments )
        {
            bool notEnoughSegments = (segments == null || segments.Count < 3);
            if (notEnoughSegments)
            {
                throw new NotEnoughSegmentsPolygonException("The passed list is either null or has less than 3 segments.");
            }
            bool notClosed = !segments.DoFormClosedRegion();
            if (notClosed)
            {
                throw new NotClosedPolygonException("The passed list of segments do not form a closed region.");
            }
            bool areNotCoplanar = !segments.AreAllCoplanar();
            if (areNotCoplanar)
            {
                throw new NotCoplanarPolygonException("The passed list of segments are not coplanar.");
            }
            bool selfIntersecting = segments.AtleastOneIntersection();
            if (selfIntersecting)
            {
                throw new SelfIntersectionPolygonException("The passed list of segments are self intersecting.");
            }
        }

        
        /// <summary>
        /// Creates a polygon if its valid to, otherwise returns null
        /// Use this over the ordinary constructor if you want to avoid a throw exception
        /// </summary>
        /// <param name="segments"></param>
        /// <returns></returns>
        public static Polygon CreatePolygonIfValid(this List<LineSegment> segments)
        {
            try
            {
                var polygon = new Polygon(segments);
                return polygon;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Returns a new list of segments with their directions such that they all flow in a consistent direction.
        /// </summary>
        public static List<LineSegment> FixSegmentOrientation(this IEnumerable<LineSegment> segments)
        {
            List<LineSegment> unSorted = segments.ToList();
            unSorted.ValidateForPolygon();

            List<LineSegment> sorted = new List<LineSegment>() { unSorted.First() };

            unSorted.RemoveAt(0);
            Point currentVertex = sorted[0].EndPoint;
            while (unSorted.Count != 0)
            {
                LineSegment nextSegment = null;
                for (int i = 0; i < unSorted.Count; i++)
                {
                    if (unSorted[i].BasePoint == currentVertex)
                    {
                        nextSegment = unSorted[i];
                    }
                    else if (unSorted[i].EndPoint == currentVertex)
                    {
                        nextSegment = unSorted[i].Reverse();
                    }
                    if (nextSegment != null)
                    {
                        sorted.Add(nextSegment);
                        unSorted.RemoveAt(i);
                        currentVertex = nextSegment.EndPoint;
                        nextSegment = null;
                    }
                }
            }
            return sorted;
        }

        /// <summary>
        /// Gets a list of all the unique Points represented in this list of LineSegments
        /// </summary>
        public static List<Point> GetAllPoints(this IEnumerable<LineSegment> segments)
        {
            return segments.SelectMany(s => s.EndPoints).DistinctByEquatable(_.Identity).ToList();
        }


        /// <summary>
        /// Shifts the List of LineSegments with the given shift
        /// </summary>
        /// <param name="lineSegments">The List of Line Segments to Shift</param>
        /// <param name="passedShift">the Shift to apply to the List of LineSegmnets</param>
        /// <returns>a new List of LineSegmnets that have been shifted with the given shift</returns>
        public static List<LineSegment> Shift(this IEnumerable<LineSegment> lineSegments, Shift shift)
        {
            return lineSegments.Select(s => s.Shift(shift)).ToList();
        }

        /// <summary>
        /// Returns true if all of the passed LineSegments are in the same plane, false otherwise
        /// </summary>
        /// <param name="passedLine">passed LineSegments</param>
        /// <returns></returns>
        public static bool AreAllCoplanar(this IEnumerable<LineSegment> passedLineList)
        {
            List<Point> vertices = passedLineList.GetAllPoints();
            return vertices.AreAllCoplanar();
        }

        /// <summary>
        /// Returns true if all of the passed LineSegments are in the same plane, false otherwise
        /// </summary>
        public static bool AreAllParallel(this List<LineSegment> passedLineList)
        {
            for (int i = 0; i < passedLineList.Count - 1; i++)
            {
                if (!passedLineList[i].IsParallelTo(passedLineList[i + 1]))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Finds all points that are points adjacent to this point via one of these linesegments
        /// </summary>
        /// <param name="edgeList"></param>
        /// <param name="vertex"></param>
        /// <returns></returns>
        public static List<Point> VerticesAdjacentTo(this List<LineSegment> edgeList, Point vertex)
        {
            List<Point> adjacentVertices = new List<Point>();
            foreach(LineSegment segment in edgeList)
            {
                if (vertex == segment.BasePoint)
                {
                    adjacentVertices.Add(segment.EndPoint);
                }
                else if (vertex == segment.EndPoint)
                {
                    adjacentVertices.Add(segment.BasePoint);
                }
            }
            return adjacentVertices;
        }

        /// <summary>
        /// given a list of edges, and a vertex, this returns all the edges that use the vertex as a base or end point
        /// NoteL this returns copies of those edges, and reorients them so that they use the vertex as a basepoint
        /// </summary>
        /// <param name="segmentList"></param>
        /// <param name="vertex"></param>
        /// <returns></returns>
        public static List<LineSegment> SegmentsAdjacentTo(this List<LineSegment> segmentList, Point vertex)
        {
            List<LineSegment> segments = new List<LineSegment>();
            foreach(LineSegment segment in segmentList)
            {
                if (segment.BasePoint == vertex)
                {
                    segments.Add(new LineSegment(segment));
                }
                else if (segment.EndPoint == vertex)
                {
                    segments.Add(segment.Reverse());
                }
            }
            return segments;
        }

        /// <summary>
        /// Note this will return two copies of the segment itself as well
        /// </summary>
        /// <param name="segmentList"></param>
        /// <param name="lineSegment"></param>
        /// <returns></returns>
        public static List<LineSegment> SegmentsAdjacentTo(this List<LineSegment> segmentList, LineSegment lineSegment)
        {
            List<LineSegment> segments = new List<LineSegment>();
            segments.AddRange(segmentList.SegmentsAdjacentTo(lineSegment.BasePoint));
            segments.AddRange(segmentList.SegmentsAdjacentTo(lineSegment.EndPoint));
            
            segments.RemoveAll(s => s == lineSegment);
            return segments;
        }

        public static List<LineSegment> ProjectAllOntoPlane(this IList<LineSegment> segmentList, Plane plane)
        {
            var results = new List<LineSegment>();
            foreach(var segment in segmentList)
            {
                var newSegment = segment.ProjectOntoPlane(plane);
                if (newSegment != null && !results.Contains(newSegment))
                {
                    results.Add(newSegment);
                }
            }
            return results;
        }

       
        public static Polygon ExteriorProfileFromSegments(this List<LineSegment> segments2D, IVector referenceNormal = null)
        {
            Point firstPoint = segments2D.GetAllPoints().OrderBy(p => p.X).ThenBy(p => p.Y).First();
            List<LineSegment> profileSegments = new List<LineSegment>();

            Point currentPoint = null;
            Vector referenceVector = new Vector(Point.MakePointWithInches(0, -1));
            while (currentPoint != firstPoint)
            {
                if (currentPoint == null)
                {
                    currentPoint = firstPoint;
                }
                var segmentsExtendingFromThisPoint = new List<LineSegment>();
                foreach (var segment in segments2D)
                {
                    if (segment.EndPoint == currentPoint)
                    {
                        segmentsExtendingFromThisPoint.Add(segment.Reverse());
                    }
                    else if (segment.BasePoint == currentPoint)
                    {
                        segmentsExtendingFromThisPoint.Add(segment);
                    }
                }
                var nextSegment = segmentsExtendingFromThisPoint.OrderBy(s => new Angle(s.CounterClockwiseAngleTo(referenceVector, referenceNormal))).First();
                profileSegments.Add(nextSegment);
                segments2D.Remove(nextSegment);
                referenceVector = new Vector(nextSegment.Reverse());
                currentPoint = nextSegment.EndPoint;
            }
            return new Polygon(profileSegments, false);
        }

        
    }
}