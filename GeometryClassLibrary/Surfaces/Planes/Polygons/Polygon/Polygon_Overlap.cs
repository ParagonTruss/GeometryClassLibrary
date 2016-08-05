using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeometryClassLibrary
{
    public partial class Polygon
    {
        // assumes both polygons are in the XY plane
        private static Polygon Overlap(Polygon P, Polygon Q)
        {
            // code based on this algorithm : http://www.iro.umontreal.ca/~plante/compGeom/algorithm.html
            var convexHull = P.Vertices.Concat(Q.Vertices).ToList().ConvexHull();
            var originalSegments = P.LineSegments.Concat(Q.LineSegments).ToList();
            var pocketLids = convexHull.LineSegments.Where(s => !originalSegments.Contains(s));

            ;
            throw new NotImplementedException();
        }

        private static Point LidBottom(List<Point> P, List<Point> Q, LineSegment pocketLid)
        {
            var i = 1;
            var j = 1;
            bool finished;
            do
            {
                finished = true;

                while (leftTurn(P[i], P[i + 1], Q[j + 1]))
                {
                    j++;
                    finished = false;
                }
                while (rightTurn(Q[j], Q[j+1], P[i + 1]))
                { 
                    i++;
                    finished = false;
                }
            } while (!finished);
            var P_segment = new LineSegment(P[i], P[i+1]);
            var Q_segment = new LineSegment(Q[i], Q[i+1]);
            return P_segment.IntersectWithSegment(Q_segment);
        }

        private static bool rightTurn(Point point1, Point point2, Point point3)
        {
            return new Direction(point1, point2).AngleFromThisToThat(new Direction(point1, point3)).IsNegative();
        }

        private static bool leftTurn(Point point1, Point point2, Point point3)
        {
            return new Direction(point1, point2).AngleFromThisToThat(new Direction(point1, point3)).IsPositive();
        }


        /// <summary>
        /// Sutherland Hodgman algorithm for clipping polygons.
        /// </summary>
        public static Polygon Clip(Polygon clipPolygon, Polygon subject)
        {
            // Uses : https://en.wikipedia.org/wiki/Sutherland%E2%80%93Hodgman_algorithm#Pseudo_code
            var outputList = subject.Vertices;
            foreach (var clipEdge in clipPolygon.LineSegments)
            {
                var inputList = outputList;
                outputList.Clear();
                Point S = inputList.LastOrDefault();
                if (S.IsNull())
                {
                    break;
                }
                foreach (Point E in inputList)
                {
                    if (clipPolygon.inside(E, clipEdge))
                    {
                        if (clipPolygon.inside(S, clipEdge).Not())
                        {
                            outputList.Add(ComputeIntersection(S, E, clipEdge));
                        }
                    }
                    else if (clipPolygon.inside(S, clipEdge))
                    {
                        outputList.Add(ComputeIntersection(S, E, clipEdge));
                    }
                    S = E;
                }
            }
            var result = new Polygon(outputList);
            return result;
        }

        private bool inside(Point point, LineSegment clipEdge)
        {
            return clipEdge.Direction.CrossProduct(new Direction(clipEdge.BasePoint, point)) == this.NormalDirection;
        }

        private static Point ComputeIntersection(Point S, Point E, Line clipEdge)
        {
            return new LineSegment(S, E).IntersectWithLine(clipEdge);
        }

      
    }
    public struct Point2D
    {
        public double X { get; }
        public double Y { get; }

        public Point2D(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }
    }
    public struct Segment
    {
        public Point2D BasePoint { get; }
        public Point2D EndPoint { get; }

        public Segment(Point2D basePoint, Point2D endPoint)
        {
            this.BasePoint = basePoint;
            this.EndPoint = endPoint;
        }
    }

    public sealed class Polygon2D
    {
        public Point2D[] Vertices { get; }

        public Polygon2D(IEnumerable<Point2D> points)
        {
            this.Vertices = points.ToArray();
        }

        public void MutateEntry(int index, Point2D point)
        {
            Vertices[index] = point;
        }
    }
   
}
