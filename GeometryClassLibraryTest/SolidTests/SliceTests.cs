using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using GeometryClassLibrary;
using NUnit.Framework;
using UnitClassLibrary.AngleUnit;
using UnitClassLibrary.DistanceUnit;
using static UnitClassLibrary.DistanceUnit.Distance;

namespace GeometryClassLibraryTest

{
    [TestFixture]
    public class Polyhedron_Validation_Tests
    {
        [Test]
        public void SliceError_Case_00550D27()
        {
            var polyhedron = new Polyhedron(true,
                new Polygon(true,
                    new Point(Inches,72.0000000000743,54.9222944654548,1.5),
                    new Point(Inches,72.0000000000743,50.649999999948,1.5),
                    new Point(Inches,69.9928817948027,52.0549827436381,1.49999999999805)),
                new Polygon(true,
                    new Point(Inches,72.0000000000743,54.9222944654548,1.5),
                    new Point(Inches,72.0000000000743,54.9222944654527,-1.41848550355773E-24),
                    new Point(Inches,72.0000000000743,50.649999999949,0),
                    new Point(Inches,72.0000000000743,50.649999999948,1.5)),
                new Polygon(true,
                    new Point(Inches,69.9928817948027,52.0549827436381,1.49999999999805),
                    new Point(Inches,72.0000000000743,50.649999999948,1.5),
                    new Point(Inches,72.0000000000743,50.649999999949,3.8649358130156E-17),
                    new Point(Inches,69.9928817948041,52.0549827436381,-1.94511073914327E-12)),
                new Polygon(true,
                    new Point(Inches,72.0000000000743,54.9222944654548,1.5),
                    new Point(Inches,69.9928817948027,52.0549827436381,1.49999999999805),
                    new Point(Inches,69.9928817948041,52.0549827436381,-1.94511073914327E-12),
                    new Point(Inches,72.0000000000758,54.9222944654548,0)),
                new Polygon(true,
                    new Point(Inches,69.9928817948041,52.0549827436381,-1.94511073914327E-12),
                    new Point(Inches,72.0000000000743,50.649999999949,3.86493581414651E-17),
                    new Point(Inches,72.0000000000758,54.9222944654548,0))
            );
            var slicingPlane = new Plane(new Point(Inches,72.0000000000743,0,0),new Direction(1,0,0));
            var sliceResults = polyhedron.Slice(slicingPlane);
        }

        // Automatically generated at 5/18/2017 1:44:51 PM
        [Test]
        public void SliceError_Case_004DF458()
        {
            var polyhedron = new Polyhedron(true, new[]
            {
                // Back face
                new Polygon(true,
                    new Point(Inches, 181.75, 3.5, 0),
                    new Point(Inches, 178.25, 3.5, 0),
                    new Point(Inches, 178.25, 30.25, 0),
                    new Point(Inches, 180.00, 30.25, 0),
                    new Point(Inches, 181.75, 29.375, 0)),
                // Front face
                new Polygon(true,
                    new Point(Inches, 178.25, 30.25, 1.5),
                    new Point(Inches, 178.25, 3.5, 1.5),
                    new Point(Inches, 181.75, 3.5, 1.5),
                    new Point(Inches, 181.75, 29.375, 1.5),
                    new Point(Inches, 180.00, 30.25, 1.5)),
                // Bottom face
                new Polygon(true,
                    new Point(Inches, 181.75, 3.5, 0),
                    new Point(Inches, 181.75, 3.5, 1.5),
                    new Point(Inches, 178.25, 3.5, 1.5),
                    new Point(Inches, 178.25, 3.5, 0)),
                // Left face
                new Polygon(true,
                    new Point(Inches, 178.25, 3.5, 0),
                    new Point(Inches, 178.25, 3.5, 1.5),
                    new Point(Inches, 178.25, 30.25, 1.5),
                    new Point(Inches, 178.25, 30.25, 0)),
                // Right face
                new Polygon(true,
                    new Point(Inches, 181.75, 3.5, 1.5),
                    new Point(Inches, 181.75, 3.5, 0),
                    new Point(Inches, 181.75, 29.375, 0),
                    new Point(Inches, 181.75, 29.375, 1.5)),
                // Top face
                new Polygon(true,
                    new Point(Inches, 178.25, 30.25, 0),
                    new Point(Inches, 178.25, 30.25, 1.5),
                    new Point(Inches, 180.00, 30.25, 1.5),
                    new Point(Inches, 180.00, 30.25, 0)),
                // Cut face
                new Polygon(true,
                    new Point(Inches, 180.00, 30.25, 0),
                    new Point(Inches, 180.00, 30.25, 1.5),
                    new Point(Inches, 181.75, 29.375, 1.5),
                    new Point(Inches, 181.75, 29.375, 0)),
            });
            var slicingPlane = new Plane(new Point(Inches, 127.31379563066, 3.5, 1.5), new Direction(0.452633527072262, -0.891696635728836, 0));
            var sliceResults = polyhedron.Slice(slicingPlane);
        }
        
        // Automatically generated at 5/19/2017 2:06:22 PM
        [Test]
        public void SliceError_Case_0146718B()
        {
            var polyhedron = new Polyhedron(true, new[]
            {
                new Polygon(true,
                    new Point(Inches, 120, 60.25, 0),
                    new Point(Inches, 120, 64.16311896, 0),
                    new Point(Inches, 260, -5.83688104, 0),
                    new Point(Inches, 260, -9.75, 0)),
                new Polygon(true,
                    new Point(Inches, 260, -9.75, 0),
                    new Point(Inches, 260, -9.75, 1.5),
                    new Point(Inches, 120, 60.25, 1.5),
                    new Point(Inches, 120, 60.25, 0)),
                new Polygon(true,
                    new Point(Inches, 260, -5.83688104, 0),
                    new Point(Inches, 260, -5.83688104, 1.5),
                    new Point(Inches, 260, -9.75, 1.5),
                    new Point(Inches, 260, -9.75, 0)),
                new Polygon(true,
                    new Point(Inches, 120, 64.16311896, 0),
                    new Point(Inches, 120, 64.16311896, 1.5),
                    new Point(Inches, 260, -5.83688104, 1.5),
                    new Point(Inches, 260, -5.83688103999999, 0)),
                new Polygon(true,
                    new Point(Inches, 120, 60.25, 0),
                    new Point(Inches, 120, 60.25, 1.5),
                    new Point(Inches, 120, 64.16311896, 1.5),
                    new Point(Inches, 120, 64.16311896, 0)),
                new Polygon(true,
                    new Point(Inches, 120, 60.25, 1.5),
                    new Point(Inches, 260, -9.75, 1.5),
                    new Point(Inches, 260, -5.83688103999999, 1.5),
                    new Point(Inches, 120, 64.16311896, 1.5))
            });
            var slicingPlane = new Plane(new Point(Inches, 252, -1.83688104, 0),new Direction(0.967862745704715, -0.251479035859714, 0));
            var sliceResults = polyhedron.Slice(slicingPlane);
            var referencePoint = new Point(Inches, 120, 62.2, 0);
            var result = sliceResults.First(solid => slicingPlane.PointIsOnSameSideAs(solid.CenterPoint, referencePoint));
            
            #region Expected solid
            Angle pitchAngle =  new Angle(new Radian(), Math.Atan2(6, 12));
            Angle cutAngle = new Angle(new Degree(), 78);
            //make the points (0.25 is from the butt cut)
            Point rightMemberTopRight = Point.MakePointWithInches(10 * 12, 64.16311896, 0); //60.25 + 3.91311896 = 64.16311896
            Point rightMemberBottomRight = Point.MakePointWithInches(10 * 12, 60.25, 0); //10 * 12 * .5 = 60 + 0.25
            Point rightMemberTopRightBack = Point.MakePointWithInches(10 * 12, 64.16311896, 1.5); //60.25 + 3.91311896 = 64.16311896
            Point rightMemberBottomRightBack = Point.MakePointWithInches(10 * 12, 60.25, 1.5); //10 * 12 * .5 = 60 + 0.25

            Angle largeAngleForAdjustment = cutAngle + pitchAngle - Angle.RightAngle;
            double largeAngleXAdjust = 3.5 / (Angle.Cosine(pitchAngle - largeAngleForAdjustment) * Angle.Sine(largeAngleForAdjustment));//4.16311896 * Math.Tan(largeAngleForAdjustment.InRadians.Value);
            Point newRightLargeMemberBottomLeft = Point.MakePointWithInches(252 - largeAngleXAdjust, -6 + 0.25 + largeAngleXAdjust * 0.5, 0);
            Point newRightLargeMemberTopLeft = Point.MakePointWithInches(252, -6 + 4.16311896, 0);
            Point newRightLargeMemberBottomLeftBack = Point.MakePointWithInches(252 - largeAngleXAdjust, -6 + 0.25 + largeAngleXAdjust * 0.5, 1.5);
            Point newRightLargeMemberTopLeftBack = Point.MakePointWithInches(252, -6 + 4.16311896, 1.5);

            List<Polygon> expectedRightFaces = new List<Polygon>();
            expectedRightFaces.Add(new Polygon(new List<Point> { newRightLargeMemberBottomLeft, newRightLargeMemberTopLeft, rightMemberTopRight, rightMemberBottomRight }));
            expectedRightFaces.Add(new Polygon(new List<Point> { newRightLargeMemberBottomLeftBack, newRightLargeMemberTopLeftBack, rightMemberTopRightBack, rightMemberBottomRightBack }));
            expectedRightFaces.Add(new Polygon(new List<Point> { newRightLargeMemberBottomLeft, newRightLargeMemberBottomLeftBack, newRightLargeMemberTopLeftBack, newRightLargeMemberTopLeft }));
            expectedRightFaces.Add(new Polygon(new List<Point> { newRightLargeMemberTopLeft, newRightLargeMemberTopLeftBack, rightMemberTopRightBack, rightMemberTopRight }));
            expectedRightFaces.Add(new Polygon(new List<Point> { rightMemberTopRight, rightMemberTopRightBack, rightMemberBottomRightBack, rightMemberBottomRight }));
            expectedRightFaces.Add(new Polygon(new List<Point> { rightMemberBottomRight, rightMemberBottomRightBack, newRightLargeMemberBottomLeftBack, newRightLargeMemberBottomLeft }));
    
            var expected = new Polyhedron(expectedRightFaces);
            #endregion        
            
            result.Should().Be(expected);
        }
    }
}