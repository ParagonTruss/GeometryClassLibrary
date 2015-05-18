using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeometryClassLibrary
{
    public static class IListPolygonExtensionMethods
    {
        /// <summary>
        /// Rotates the list of polygons with the given rotation
        /// </summary>
        /// <param name="passedPolygons">The polygons to rotate</param>
        /// <param name="passedRotation">The Rotation to apply</param>
        /// <returns>A new list of new Polygons that have been rotated</returns>
        public static List<Polygon> Rotate(this IList<Polygon> passedPolygons, Rotation passedRotation)
        {
            List<Polygon> rotatedRegion = new List<Polygon>();
            foreach (var planeRegion in passedPolygons)
            {
                rotatedRegion.Add(planeRegion.Rotate(passedRotation));
            }
            return rotatedRegion;
        }

        /// <summary>
        /// Shifts the polygons in the list with the given shift
        /// </summary>
        /// <param name="passedPolygon">The polygons to Shift</param>
        /// <param name="passedShift">The shift to apply</param>
        /// <returns>A nes list of ne Polygons that have been shifted</returns>
        public static List<Polygon> Shift(this List<Polygon> passedPolygon, Shift passedShift)
        {
            List<Polygon> shiftedPolygons = new List<Polygon>();

            foreach (var region in passedPolygon)
            {
                shiftedPolygons.Add(region.Shift(passedShift));
            }

            return shiftedPolygons;
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
        /// Finds a vertex of one of the polygons in this list that is not contained by the given plane in order to use it as a reference point 
        /// to determine what side of the plane this list of polygons lies. Presumably, the plane is a side of the "unconstructed polhedron" that
        /// this list of polygons represents.
        /// </summary>
        /// <param name="polygonsList">The list of polygons to find the vertex from</param>
        /// <param name="planeNotToFindTheVertexOn">The plane that presumably is a side of the "unconstructed" polyhedron to determine what side of the plane this list of polygons lies</param>
        /// <returns>A vertex from this list of polygons that is not on the given plane</returns>
        public static Point FindVertexToUseAsReferenceNotOnThePlane(this List<Polygon> polygonsList, Plane planeNotToFindTheVertexOn)
        {
            Point pointFound;

            foreach (Polygon polygon in polygonsList)
            {
                pointFound = polygon.FindVertexNotOnTheGivenPlane(planeNotToFindTheVertexOn);
                if (pointFound != null)
                {
                    return pointFound;
                }
            }

            return null;
        }

        /// <summary>
        /// Finds all the polygons in this list that intersect the given line and returns a list of those Polygons
        /// </summary>
        /// <param name="intersectingLine">The line that potentially intersects with this list of Polygons</param>
        /// <returns>Returns a list of all the polygons the line intersects in the list or an empty list if the line did not intersect any of them</returns>
        public static List<Polygon> FindPolygonsThatAreIntersectedByLine(this List<Polygon> polygonList, Line intersectingLine)
        {
            List<Polygon> sidesIntersected = new List<Polygon>();

            foreach (Polygon side in polygonList)
            {
                if (side.DoesIntersect(intersectingLine))
                {
                    sidesIntersected.Add(side);
                }
            }

            return sidesIntersected;
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

        private static bool _everyEdgeIsOntwoFaces(this List<Polygon> polygonList, List<LineSegment> edges)
        {
            foreach(LineSegment edge in edges)
            {
                int count = 0;
                Polygon temp = new Polygon();
                foreach(Polygon polygon in polygonList)
                {
                    if (polygon.HasSide(edge))
                    {
                        count++;
                        temp = polygon;
                    }
                }
                if (count != 2)
                {
                    throw new Exception("count =" + count + "side = " + edge + ". On face = " + temp);
                }
            }
            return true;
        }

        /// <summary>
        /// Checks if the set of polygons forms a single closed region.
        /// </summary>
        /// <returns></returns>
        public static bool DoesFormSingleClosedRegion(this List<Polygon> polygonList)
        {
            List<LineSegment> edges = polygonList.GetAllEdges();
            //First check to that every edge sits on exactly two faces.
            if (!polygonList._everyEdgeIsOntwoFaces(edges))
            {
                return false;
            }



            return true;
        }
    }
}
