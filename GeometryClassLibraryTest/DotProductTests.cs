using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClearspanTypeLibrary;
using FluentAssertions;

namespace ClearspanLibraryUnitTest
{
    [TestClass()]
    public class DotProductTests
    {
        ///// <summary>
        ///// Tests regular positive values
        ///// </summary>
        //[TestMethod()]
        //public void StandardDotProductTest()
        //{
        //    //Dimension x1 = new Dimension(DimensionType.Millimeter, 1);
        //    //Dimension y1 = new Dimension(DimensionType.Millimeter, 2);
        //    //Dimension z1 = new Dimension(DimensionType.Millimeter, 3);

        //    //Dimension x2 = new Dimension(DimensionType.Millimeter, 2);
        //    //Dimension y2 = new Dimension(DimensionType.Millimeter, 3);
        //    //Dimension z2 = new Dimension(DimensionType.Millimeter, 4);

        //    //Point dPoint1 = new Point(x1, y1, z1);
        //    //Point dPoint2 = new Point(x2, y2, z2);

        //    //Dimension answer = dPoint1.DotProduct(dPoint2);
        //    //Dimension dim = new Dimension(DimensionType.Millimeter, 20);
        //    //answer.Should().Be(dim);

        //}

        ///// <summary>
        ///// 
        ///// </summary>
        //[TestMethod()]
        //public void NegativeDotProductTest()
        //{
        //    //DimensionGenerator inchesDimensionGenerator = new DimensionGenerator();

        //    //inchesDimensionGenerator.MakeDimension(10);

        //    //Dimension x1 = new Dimension(DimensionType.Millimeter, -1);
        //    //Dimension y1 = new Dimension(DimensionType.Millimeter, -2);
        //    //Dimension z1 = new Dimension(DimensionType.Millimeter, -3);

        //    //Dimension x2 = new Dimension(DimensionType.Millimeter, 2);
        //    //Dimension y2 = new Dimension(DimensionType.Millimeter, 3);
        //    //Dimension z2 = new Dimension(DimensionType.Millimeter, 4);

        //    //Point dPoint1 = new Point(x1, y1, z1);
        //    //Point dPoint2 = new Point(x2, y2, z2);

        //    //Dimension answer = dPoint1.DotProduct(dPoint2);
        //    //answer.Millimeters.Should().Be(-20);
        //}

        //[TestMethod()]
        //[ExpectedException(typeof(NullReferenceException))]
        //public void DotProduct_NullTest()
        //{
        //    Dimension x1 = new Dimension(DimensionType.Millimeter, -1);
        //    Dimension y1 = new Dimension(DimensionType.Millimeter, -2);
        //    Dimension z1 = new Dimension(DimensionType.Millimeter, -3);

        //    Point dPoint1 = new Point(x1, y1, z1);

        //    Dimension answer = dPoint1.DotProduct(null);
        //}

    }
}
