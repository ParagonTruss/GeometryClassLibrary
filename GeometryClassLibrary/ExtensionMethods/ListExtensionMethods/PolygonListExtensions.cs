﻿using System.Collections.Generic;
using System.Linq;

namespace GeometryClassLibrary
{
    public static class PolygonListExtensions
    {
        /// <summary>
        /// Rotates the list of polygons with the given rotation
        /// </summary>
        public static List<Polygon> Rotate(this IList<Polygon> passedPolygons, Rotation passedRotation)
        {
            return passedPolygons.Select(p => p.Rotate(passedRotation)).ToList();
        }

        /// <summary>
        /// Shifts the polygons in the list with the given shift
        /// </summary>
        /// <param name="passedPolygon">The polygons to Shift</param>
        /// <param name="passedShift">The shift to apply</param>
        /// <returns>A nes list of ne Polygons that have been shifted</returns>
        public static List<Polygon> Shift(this List<Polygon> passedPolygon, Shift passedShift)
        {
            return passedPolygon.Select(p => p.Shift(passedShift)).ToList();
        }

        /// <summary>
        /// Returns whether or not the polygon has a common side with any of the polygons in this list
        /// </summary>
        /// <param name="polygonsList">The List of Polygons to check if the passed polygon shares a side with</param>
        /// <param name="polygonToCheckSidesOf">The polygon to see if any of the polygons in this list share a side with</param>
        /// <returns>Returns a bool of whether or not the Polyhedron has a common side with the polygon</returns>
        public static bool DoesShareSideWithPolygonInList(this List<Polygon> polygonsList, Polygon polygonToCheckSidesOf)
        {
            foreach (Polygon polygon in polygonsList)
            {
                if (polygonToCheckSidesOf.DoesShareExactSide(polygon))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Finds all the polygons in this list that intersect the given line and returns a list of those Polygons
        /// </summary>
        public static List<Polygon> FindPolygonsThatAreIntersectedByLine(this List<Polygon> polygonList, Line intersectingLine)
        {
            return polygonList.Where(p => p.DoesIntersect(intersectingLine)).ToList();
        }

        /// <summary>
        /// Finds all the polygons in this list that touch the plane passed in. If the includeContained flag is false, it will not return any 
        /// polygons contained by the the plane
        /// </summary>
        /// <param name="polygonList">The list of polygons to to see if they touch the planes</param>
        /// <param name="toSeeIfTouches">The plane to see if the polygons touch</param>
        /// <param name="includeContained">If true it will include any polygons in the list that are contained in the plane, false it will not</param>
        /// <returns>Returns the polygons that are touching this plane</returns>
        public static List<Polygon> FindPolygonsTouchingPlane(this List<Polygon> polygonList, Plane toSeeIfTouches, bool includeContained = false)
        {
            List<Polygon> touchingPolygons = new List<Polygon>();
            foreach (Polygon polygon in polygonList)
            {
                //see if they intersect
                if (toSeeIfTouches.DoesIntersect(polygon))
                {
                    touchingPolygons.Add(polygon);
                }
                //they will not intersect if the are coplanar so we check here if we want to include contained ones
                else if (includeContained && toSeeIfTouches.Contains(polygon))
                {
                    touchingPolygons.Add(polygon);
                }
            }

            return touchingPolygons;
        }

        /// <summary>
        /// returns all the edges from a list of polygons
        /// </summary>
        /// <param name="polygonList"></param>
        /// <returns></returns>
        public static List<LineSegment> GetAllEdges(this List<Polygon> polygonList)
        {
            List<LineSegment> edges = new List<LineSegment>();
            foreach(Polygon polygon in polygonList)
            {
                foreach(LineSegment edge in polygon.LineSegments)
                {
                    if (!edges.Contains(edge))
                    {
                        edges.Add(edge);
                    }
                }
            }
            return edges;
        }

        /// <summary>
        /// Copies the list
        /// Note: the edges may be oriented differently
        /// </summary>
        public static List<Polygon> CopyList(this List<Polygon> polygonList)
        {
            return polygonList.Select(p => new Polygon(p)).ToList();
        }

        public static List<Polygon> SplitIntoTriangles(this List<Polygon> polygonList)
        {
            return polygonList.SelectMany(p => p.SplitIntoTriangles()).ToList();
        }

        public static List<Polygon> ProjectAllOntoPlane(this IEnumerable<Polygon> polygonList, Plane plane)
        {
            var listOfProjected = new List<Polygon>();
            foreach(var polygon in polygonList)
            {
                var projected = polygon.ProjectOntoPlane(plane);
                if (projected != null && !listOfProjected.Contains(projected))
                {
                    listOfProjected.Add(projected);
                }
            }
            return listOfProjected;
        }
    }
}
