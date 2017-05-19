using System.Collections.Generic;
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

    }
}