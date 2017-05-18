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
                    new Point(Distance.Inches,72.0000000000743,54.9222944654548,1.5),
                    new Point(Distance.Inches,72.0000000000743,50.649999999948,1.5),
                    new Point(Distance.Inches,69.9928817948027,52.0549827436381,1.49999999999805)),
                new Polygon(true,
                    new Point(Distance.Inches,72.0000000000743,54.9222944654548,1.5),
                    new Point(Distance.Inches,72.0000000000743,54.9222944654527,-1.41848550355773E-24),
                    new Point(Distance.Inches,72.0000000000743,50.649999999949,0),
                    new Point(Distance.Inches,72.0000000000743,50.649999999948,1.5)),
                new Polygon(true,
                    new Point(Distance.Inches,69.9928817948027,52.0549827436381,1.49999999999805),
                    new Point(Distance.Inches,72.0000000000743,50.649999999948,1.5),
                    new Point(Distance.Inches,72.0000000000743,50.649999999949,3.8649358130156E-17),
                    new Point(Distance.Inches,69.9928817948041,52.0549827436381,-1.94511073914327E-12)),
                new Polygon(true,
                    new Point(Distance.Inches,72.0000000000743,54.9222944654548,1.5),
                    new Point(Distance.Inches,69.9928817948027,52.0549827436381,1.49999999999805),
                    new Point(Distance.Inches,69.9928817948041,52.0549827436381,-1.94511073914327E-12),
                    new Point(Distance.Inches,72.0000000000758,54.9222944654548,0)),
                new Polygon(true,
                    new Point(Distance.Inches,69.9928817948041,52.0549827436381,-1.94511073914327E-12),
                    new Point(Distance.Inches,72.0000000000743,50.649999999949,3.86493581414651E-17),
                    new Point(Distance.Inches,72.0000000000758,54.9222944654548,0))
            );
            var slicingPlane = new Plane(new Point(Distance.Inches,72.0000000000743,0,0),new Direction(1,0,0));
            var sliceResults = polyhedron.Slice(slicingPlane);
        }
    }
}