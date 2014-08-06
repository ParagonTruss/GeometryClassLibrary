using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using ClearspanTypeLibrary;
using GeometryClassLibrary;

namespace ClearspanLibraryUnitTest
{
    [TestClass()]
    public class MatricesMatrixTests
    {
        [TestMethod()]
        public void MatricesMatrix_TotalRowsTest()
        {
            Matrix m1 = new Matrix(1);
            Matrix m2 = new Matrix(2);
            Matrix m3 = new Matrix(3);
            Matrix m4 = new Matrix(4);

            MatricesMatrix testMatricesMatrix = new MatricesMatrix(2, 2);

            testMatricesMatrix.SetElement(0,0, m1);
            testMatricesMatrix.SetElement(0,1, m3);
            testMatricesMatrix.SetElement(1,0, m2);
            testMatricesMatrix.SetElement(1,1, m4);

            int expectedResult = 7;

            int actualResult = testMatricesMatrix.TotalRows();

            (actualResult == expectedResult).Should().BeTrue();

        }

        [TestMethod()]
        public void MatricesMatrix_TotalColumnsTest()
        {
            Matrix m1 = new Matrix(1);
            Matrix m2 = new Matrix(2);
            Matrix m3 = new Matrix(3);
            Matrix m4 = new Matrix(4);

            MatricesMatrix testMatricesMatrix = new MatricesMatrix(2, 2);

            testMatricesMatrix.SetElement(0, 0, m1);
            testMatricesMatrix.SetElement(0, 1, m3);
            testMatricesMatrix.SetElement(1, 0, m2);
            testMatricesMatrix.SetElement(1, 1, m4);

            int expectedResult = 6;

            int actualResult = testMatricesMatrix.TotalColumns();

            (actualResult == expectedResult).Should().BeTrue();

        }

        [TestMethod()]
        public void TotalRowsTest_EqualRowDimensions()
        {
            Matrix m1 = new Matrix(2);
            Matrix m2 = new Matrix(2, 3);
            Matrix m3 = new Matrix(2);
            Matrix m4 = new Matrix(2, 3);
            
            MatricesMatrix testMatricesMatrix = new MatricesMatrix(2, 2);

            testMatricesMatrix.SetElement(0, 0, m1);
            testMatricesMatrix.SetElement(0, 1, m2);
            testMatricesMatrix.SetElement(1, 0, m3);
            testMatricesMatrix.SetElement(1, 1, m4);

            int expectedResult = 4;

            int actualResult = testMatricesMatrix.TotalRows();

            (actualResult == expectedResult).Should().BeTrue();

        }

        [TestMethod()]
        public void TotalColumnsTest_EqualRowDimensions()
        {
            Matrix m1 = new Matrix(2);
            Matrix m2 = new Matrix(2, 3);
            Matrix m3 = new Matrix(2);
            Matrix m4 = new Matrix(2, 3);

            MatricesMatrix testMatricesMatrix = new MatricesMatrix(2, 2);

            testMatricesMatrix.SetElement(0, 0, m1);
            testMatricesMatrix.SetElement(0, 1, m2);
            testMatricesMatrix.SetElement(1, 0, m3);
            testMatricesMatrix.SetElement(1, 1, m4);

            int expectedResult = 5;

            int actualResult = testMatricesMatrix.TotalColumns();

            (actualResult == expectedResult).Should().BeTrue();

        }

        [TestMethod()]
        public void MatricesMatrix_GetRowHeightTest()
        {
            Matrix m1 = new Matrix(1);
            Matrix m2 = new Matrix(2);
            Matrix m3 = new Matrix(3);
            Matrix m4 = new Matrix(4);

            MatricesMatrix testMatricesMatrix = new MatricesMatrix(2, 2);

            testMatricesMatrix.SetElement(0, 0, m1);
            testMatricesMatrix.SetElement(0, 1, m3);
            testMatricesMatrix.SetElement(1, 0, m2);
            testMatricesMatrix.SetElement(1, 1, m4);

            int expectedResult1 = 3;
            int expectedResult2 = 4;

            int actualResult1 = testMatricesMatrix.GetRowHeight(0);
            int actualResult2 = testMatricesMatrix.GetRowHeight(1);

            (actualResult1 == expectedResult1).Should().BeTrue();
            (actualResult2 == expectedResult2).Should().BeTrue();


        }

        [TestMethod()]
        public void MatricesMatrix_GetColumnWidthTest()
        {
            Matrix m1 = new Matrix(1);
            Matrix m2 = new Matrix(2);
            Matrix m3 = new Matrix(3);
            Matrix m4 = new Matrix(4);

            MatricesMatrix testMatricesMatrix = new MatricesMatrix(2, 2);

            testMatricesMatrix.SetElement(0, 0, m1);
            testMatricesMatrix.SetElement(0, 1, m3);
            testMatricesMatrix.SetElement(1, 0, m2);
            testMatricesMatrix.SetElement(1, 1, m4);

            int expectedResult1 = 2;
            int expectedResult2 = 4;

            int actualResult1 = testMatricesMatrix.GetColumnWidth(0);
            int actualResult2 = testMatricesMatrix.GetColumnWidth(1);

            (actualResult1 == expectedResult1).Should().BeTrue();
            (actualResult2 == expectedResult2).Should().BeTrue();


        }

        [TestMethod()]
        public void MatricesMatrix_ConvertToMatrixTest()
        {
            Matrix m1 = new Matrix(2);
            Matrix m2 = new Matrix(2, 3);
            Matrix m3 = new Matrix(2);
            Matrix m4 = new Matrix(2, 3);

            double[] m1Row1 = { 1, 1 };
            double[] m1Row2 = { 1, 1 };

            double[] m2Row1 = { 2, 2, 2 };
            double[] m2Row2 = { 2, 2, 2 };            

            double[] m3Row1 = { 3, 3 };
            double[] m3Row2 = { 3, 3 };

            double[] m4Row1 = { 4, 4, 4 };
            double[] m4Row2 = { 4, 4, 4 };

            m1.SetRowOfMatrix(0, m1Row1);
            m1.SetRowOfMatrix(1, m1Row2);
            m2.SetRowOfMatrix(0, m2Row1);
            m2.SetRowOfMatrix(1, m2Row2); 
            m3.SetRowOfMatrix(0, m3Row1);
            m3.SetRowOfMatrix(1, m3Row2);
            m4.SetRowOfMatrix(0, m4Row1);
            m4.SetRowOfMatrix(1, m4Row2);

            MatricesMatrix testMatricesMatrix = new MatricesMatrix(2, 2);

            testMatricesMatrix.SetElement(0, 0, m1);
            testMatricesMatrix.SetElement(0, 1, m2);
            testMatricesMatrix.SetElement(1, 0, m3);
            testMatricesMatrix.SetElement(1, 1, m4);

            Matrix expectedResult = new Matrix(4, 5);

            double[] expectedRow1 = { 1, 1, 2, 2, 2 };
            double[] expectedRow2 = { 1, 1, 2, 2, 2 };
            double[] expectedRow3 = { 3, 3, 4, 4, 4 };
            double[] expectedRow4 = { 3, 3, 4, 4, 4 };

            expectedResult.SetRowOfMatrix(0, expectedRow1);
            expectedResult.SetRowOfMatrix(1, expectedRow2);
            expectedResult.SetRowOfMatrix(2, expectedRow3);
            expectedResult.SetRowOfMatrix(3, expectedRow4);

            Matrix actualResult = testMatricesMatrix.ConvertToMatrix();

            (actualResult == expectedResult).Should().BeTrue();

        }

        [TestMethod()]
        public void MatricesMatrix_ConvertToMatrixTest_VariedDimensions()
        {
            Matrix m1 = new Matrix(1);
            Matrix m2 = new Matrix(2);
            Matrix m3 = new Matrix(3);
            Matrix m4 = new Matrix(4);

            double[] m1Row1 = { 1};

            double[] m2Row1 = { 2, 2 };
            double[] m2Row2 = { 2, 2 };

            double[] m3Row1 = { 3, 3, 3 };
            double[] m3Row2 = { 3, 3, 3 };
            double[] m3Row3 = { 3, 3, 3 };

            double[] m4Row1 = { 4, 4, 4, 4 };
            double[] m4Row2 = { 4, 4, 4, 4 };
            double[] m4Row3 = { 4, 4, 4, 4 };
            double[] m4Row4 = { 4, 4, 4, 4 };


            m1.SetRowOfMatrix(0, m1Row1);

            m2.SetRowOfMatrix(0, m2Row1);
            m2.SetRowOfMatrix(1, m2Row2);

            m3.SetRowOfMatrix(0, m3Row1);
            m3.SetRowOfMatrix(1, m3Row2);
            m3.SetRowOfMatrix(2, m3Row3);

            m4.SetRowOfMatrix(0, m4Row1);
            m4.SetRowOfMatrix(1, m4Row2);
            m4.SetRowOfMatrix(2, m4Row3);
            m4.SetRowOfMatrix(3, m4Row4);

            MatricesMatrix testMatricesMatrix = new MatricesMatrix(2, 2);

            testMatricesMatrix.SetElement(0, 0, m1);
            testMatricesMatrix.SetElement(0, 1, m3);
            testMatricesMatrix.SetElement(1, 0, m2);
            testMatricesMatrix.SetElement(1, 1, m4);

            Matrix expectedResult = new Matrix(7, 6);

            double[] expectedRow1 = { 1, 0, 3, 3, 3, 0 };
            double[] expectedRow2 = { 0, 0, 3, 3, 3, 0 };
            double[] expectedRow3 = { 0, 0, 3, 3, 3, 0 };
            double[] expectedRow4 = { 2, 2, 4, 4, 4, 4 };
            double[] expectedRow5 = { 2, 2, 4, 4, 4, 4 };
            double[] expectedRow6 = { 0, 0, 4, 4, 4, 4 };
            double[] expectedRow7 = { 0, 0, 4, 4, 4, 4 };

            expectedResult.SetRowOfMatrix(0, expectedRow1);
            expectedResult.SetRowOfMatrix(1, expectedRow2);
            expectedResult.SetRowOfMatrix(2, expectedRow3);
            expectedResult.SetRowOfMatrix(3, expectedRow4);
            expectedResult.SetRowOfMatrix(4, expectedRow5);
            expectedResult.SetRowOfMatrix(5, expectedRow6);
            expectedResult.SetRowOfMatrix(6, expectedRow7);


            Matrix actualResult = testMatricesMatrix.ConvertToMatrix();

            (actualResult == expectedResult).Should().BeTrue();

        }

        [TestMethod()]
        public void MatricesMatrix_ConvertToMatrixTest_SingleColumn()
        {
            Matrix m1 = new Matrix(2, 1);
            Matrix m2 = new Matrix(3, 1);

            double[] m1Column1 = { 1, 1 };

            double[] m2Column1 = { 2, 2, 2 };

            m1.SetColumnOfMatrix(0, m1Column1);
            m2.SetColumnOfMatrix(0, m2Column1);
            
            MatricesMatrix testMatricesMatrix = new MatricesMatrix(2, 1);

            testMatricesMatrix.SetElement(0, 0, m1);
            testMatricesMatrix.SetElement(1, 0, m2);

            Matrix expectedResult = new Matrix(5, 1);

            double[] expectedColumn1 = { 1, 1, 2, 2, 2 };

            expectedResult.SetColumnOfMatrix(0, expectedColumn1);

            Matrix actualResult = testMatricesMatrix.ConvertToMatrix();

            (actualResult == expectedResult).Should().BeTrue();
        }



    }
}
