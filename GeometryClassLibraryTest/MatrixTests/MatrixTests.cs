using System;
using System.Collections.Generic;
using FluentAssertions;
using GeometryClassLibrary;
using Newtonsoft.Json;
using NUnit.Framework;
using UnitClassLibrary;
using UnitClassLibrary.AngleUnit;
using UnitClassLibrary.AngleUnit;

namespace GeometryClassLibraryTest
{
    [TestFixture()]
    public class MatrixTests
    {
        [Test]
        public void Matrix_JSON()
        {
            Matrix matrix = new Matrix(3, 2);
            matrix.SetElement(0, 0, 1);
            matrix.SetElement(0, 1, 1);
            matrix.SetElement(1, 0, 2);
            matrix.SetElement(1, 1, 0);
            matrix.SetElement(2, 0, 0);
            matrix.SetElement(2, 1, 3);

            var json = JsonConvert.SerializeObject(matrix);
            Matrix deserializedMatrix = JsonConvert.DeserializeObject<Matrix>(json);

            bool areEqual = (matrix == deserializedMatrix);
            areEqual.Should().BeTrue();
        }

        [Test()]
        public void Matrix_StandardMatrixMultiplyTest()
        {
            Matrix matrix1 = new Matrix(3, 2);
            matrix1.SetElement(0, 0, 1);
            matrix1.SetElement(0, 1, 1);
            matrix1.SetElement(1, 0, 2);
            matrix1.SetElement(1, 1, 0);
            matrix1.SetElement(2, 0, 0);
            matrix1.SetElement(2, 1, 3);

            Matrix matrix2 = new Matrix(2, 4);
            matrix2.SetElement(0, 0, 0);
            matrix2.SetElement(0, 1, 1);
            matrix2.SetElement(0, 2, 1);
            matrix2.SetElement(0, 3, 0);
            matrix2.SetElement(1, 0, 2);
            matrix2.SetElement(1, 1, 3);
            matrix2.SetElement(1, 2, 0);
            matrix2.SetElement(1, 3, 1);

            Matrix answerMatrix = new Matrix(3, 4);
            answerMatrix.SetElement(0, 0, 2);
            answerMatrix.SetElement(0, 1, 4);
            answerMatrix.SetElement(0, 2, 1);
            answerMatrix.SetElement(0, 3, 1);
            answerMatrix.SetElement(1, 0, 0);
            answerMatrix.SetElement(1, 1, 2);
            answerMatrix.SetElement(1, 2, 2);
            answerMatrix.SetElement(1, 3, 0);
            answerMatrix.SetElement(2, 0, 6);
            answerMatrix.SetElement(2, 1, 9);
            answerMatrix.SetElement(2, 2, 0);
            answerMatrix.SetElement(2, 3, 3);

            Matrix resultingMatrix = matrix1.MultiplyBy(matrix2);

            bool answer = (resultingMatrix == answerMatrix);
            answer.Should().BeTrue();

        }

        [Test()]
        public void Matrix_CanBeMultipliedTest()
        {
            Matrix matrix1 = new Matrix(3, 2);
            matrix1.SetElement(0, 0, 1);
            matrix1.SetElement(0, 1, 1);
            matrix1.SetElement(1, 0, 2);
            matrix1.SetElement(1, 1, 0);
            matrix1.SetElement(2, 0, 0);
            matrix1.SetElement(2, 1, 3);

            Matrix matrix2 = new Matrix(2, 4);
            matrix2.SetElement(0, 0, 0);
            matrix2.SetElement(0, 1, 1);
            matrix2.SetElement(0, 2, 1);
            matrix2.SetElement(0, 3, 0);
            matrix2.SetElement(1, 0, 2);
            matrix2.SetElement(1, 1, 3);
            matrix2.SetElement(1, 2, 0);
            matrix2.SetElement(1, 3, 1);

            bool result = matrix1.CanBeMultipliedBy(matrix2);

            result.Should().BeTrue();

        }

        [Test()]
        public void Matrix_CannotBeMultipliedTest()
        {
            Matrix matrix1 = new Matrix(2, 3);
            matrix1.SetElement(0, 0, 1);
            matrix1.SetElement(0, 1, 1);
            matrix1.SetElement(0, 2, 2);
            matrix1.SetElement(1, 0, 0);
            matrix1.SetElement(1, 1, 0);
            matrix1.SetElement(1, 2, 3);

            Matrix matrix2 = new Matrix(2, 4);
            matrix2.SetElement(0, 0, 0);
            matrix2.SetElement(0, 1, 1);
            matrix2.SetElement(0, 2, 1);
            matrix2.SetElement(0, 3, 0);
            matrix2.SetElement(1, 0, 2);
            matrix2.SetElement(1, 1, 3);
            matrix2.SetElement(1, 2, 0);
            matrix2.SetElement(1, 3, 1);

            bool result = matrix1.CanBeMultipliedBy(matrix2);

            result.Should().BeFalse();

        }

        [Test()]
        public void Matrix_IdentityMatrixMultiplyTest()
        {
            Matrix matrix1 = new Matrix(3, 2);
            matrix1.SetElement(0, 0, 1);
            matrix1.SetElement(0, 1, 1);
            matrix1.SetElement(1, 0, 2);
            matrix1.SetElement(1, 1, 0);
            matrix1.SetElement(2, 0, 0);
            matrix1.SetElement(2, 1, 3);

            Matrix matrix2 = new Matrix(2, 2);
            matrix2.SetElement(0, 0, 1);
            matrix2.SetElement(0, 1, 0);
            matrix2.SetElement(1, 0, 0);
            matrix2.SetElement(1, 1, 1);

            Matrix resultingMatrix = matrix1.MultiplyBy(matrix2);

            bool answer = (resultingMatrix == matrix1);
            answer.Should().BeTrue();

        }

        [Test()]
        public void Matrix_StandardMatrixAdditionTest()
        {
            Matrix matrix1 = new Matrix(3, 2);
            matrix1.SetElement(0, 0, 1);
            matrix1.SetElement(0, 1, 1);
            matrix1.SetElement(1, 0, 2);
            matrix1.SetElement(1, 1, 0);
            matrix1.SetElement(2, 0, 0);
            matrix1.SetElement(2, 1, 3);

            Matrix matrix2 = new Matrix(3, 2);
            matrix2.SetElement(0, 0, 0);
            matrix2.SetElement(0, 1, 1);
            matrix2.SetElement(1, 0, 1);
            matrix2.SetElement(1, 1, 0);
            matrix2.SetElement(2, 0, 2);
            matrix2.SetElement(2, 1, 3);

            Matrix answerMatrix = new Matrix(3, 2);
            answerMatrix.SetElement(0, 0, 1);
            answerMatrix.SetElement(0, 1, 2);
            answerMatrix.SetElement(1, 0, 3);
            answerMatrix.SetElement(1, 1, 0);
            answerMatrix.SetElement(2, 0, 2);
            answerMatrix.SetElement(2, 1, 6);
            
            Matrix resultingMatrix = matrix1.AddTo(matrix2);

            bool answer = (resultingMatrix == answerMatrix);
            answer.Should().BeTrue();

        }

        [Test()]
        public void Matrix_CanBeAddedTest()
        {
            Matrix matrix1 = new Matrix(3, 2);
            matrix1.SetElement(0, 0, 1);
            matrix1.SetElement(0, 1, 1);
            matrix1.SetElement(1, 0, 2);
            matrix1.SetElement(1, 1, 0);
            matrix1.SetElement(2, 0, 0);
            matrix1.SetElement(2, 1, 3);

            Matrix matrix2 = new Matrix(3, 2);
            matrix2.SetElement(0, 0, 0);
            matrix2.SetElement(0, 1, 1);
            matrix2.SetElement(1, 0, 1);
            matrix2.SetElement(1, 1, 0);
            matrix2.SetElement(2, 0, 2);
            matrix2.SetElement(2, 1, 3);

            bool result = matrix1.CanBeAddedTo(matrix2);

            result.Should().BeTrue();

        }

        [Test()]
        public void Matrix_CannotBeAddedTest()
        {
            Matrix matrix1 = new Matrix(3, 2);
            matrix1.SetElement(0, 0, 1);
            matrix1.SetElement(0, 1, 1);
            matrix1.SetElement(1, 0, 2);
            matrix1.SetElement(1, 1, 0);
            matrix1.SetElement(2, 0, 0);
            matrix1.SetElement(2, 1, 3);

            Matrix matrix2 = new Matrix(2, 3);
            matrix2.SetElement(0, 0, 0);
            matrix2.SetElement(0, 1, 1);
            matrix2.SetElement(0, 2, 1);
            matrix2.SetElement(1, 0, 0);
            matrix2.SetElement(1, 1, 2);
            matrix2.SetElement(1, 2, 3);

            bool result = matrix1.CanBeAddedTo(matrix2);

            result.Should().BeFalse();

        }

        [Test()]
        public void Matrix_IdentityMatrixAdditionTest()
        {
            Matrix matrix1 = new Matrix(3, 2);
            matrix1.SetElement(0, 0, 1);
            matrix1.SetElement(0, 1, 1);
            matrix1.SetElement(1, 0, 2);
            matrix1.SetElement(1, 1, 0);
            matrix1.SetElement(2, 0, 0);
            matrix1.SetElement(2, 1, 3);

            Matrix matrix2 = new Matrix(3, 2);
            matrix2.SetElement(0, 0, 0);
            matrix2.SetElement(0, 1, 0);
            matrix2.SetElement(1, 0, 0);
            matrix2.SetElement(1, 1, 0);
            matrix2.SetElement(2, 0, 0);
            matrix2.SetElement(2, 1, 0);

            Matrix resultingMatrix = matrix1.AddTo(matrix2);

            bool answer = (resultingMatrix == matrix1);
            answer.Should().BeTrue();

        }

        [Test()]
        public void Matrix_GetRowStandardTest()
        {
            Matrix testMatrix = new Matrix(2, 3);
            double[] rowTwoOfTestMatrix = { 7, 8, 9 };

            testMatrix.SetRow(1, rowTwoOfTestMatrix);

            testMatrix.GetRow(1).ShouldBeEquivalentTo(rowTwoOfTestMatrix);

        }

        [Test()]
        public void Matrix_GetColumnStandardTest()
        {
            Matrix testMatrix = new Matrix(3, 2);
            double[] columnTwoOfTestMatrix = {7,8,9};

            testMatrix.SetColumn(1, columnTwoOfTestMatrix);

            testMatrix.GetColumn(1).ShouldBeEquivalentTo(columnTwoOfTestMatrix);            

        }

        [Test()]
        public void Matrix_SystemSolveStandardTest()
        {
            //Ax = b
            Matrix matrixA = new Matrix(4, 4);
            Matrix matrixB = new Matrix(4, 1);

            //Set up matrix A
            double[] columnOneOfMatrixA = { 3, 1, 2, 5 };
            double[] columnTwoOfMatrixA = { 7, 8, 1, 4 };
            double[] columnThreeOfMatrixA = { 2, 4, 9, 7 };
            double[] columnFourOfMatrixA = { 5, 2, 3, 1 };

            matrixA.SetColumn(0, columnOneOfMatrixA);
            matrixA.SetColumn(1, columnTwoOfMatrixA);
            matrixA.SetColumn(2, columnThreeOfMatrixA);
            matrixA.SetColumn(3, columnFourOfMatrixA);

            //Set up matrix b
            double[] bColumn = { 49, 30, 43, 52 };
            matrixB.SetColumn(0, bColumn);            

            //Solve for x
            Matrix expectedResultMatrix = new Matrix(4, 1);
            double[] expectedResult = { 6, 1, 2, 4 };
            expectedResultMatrix.SetColumn(0, expectedResult);

            Matrix actualResultMatrix = matrixA.SystemSolve(matrixB);

            double tolerance = .001;
            Matrix differenceMatrix = expectedResultMatrix - actualResultMatrix;

            for (int i = 0; i < differenceMatrix.NumberOfRows; i++)
            {
                for (int j = 0; j < differenceMatrix.NumberOfColumns; j++)
                {
                    (Math.Abs(differenceMatrix.GetElement(i, j)) < tolerance).Should().BeTrue();
                }
            }

            
        }

        [Test]
        public void MatrixTest_SystemSolve_ZeroMatrix()
        {
            //Fails

            //Ax = b
            Matrix matrixA = new Matrix(4, 4);
            Matrix matrixB = new Matrix(4, 1);
                                 
            //Solve for x
            Matrix expectedResultMatrix = new Matrix(4, 1);
            
            Matrix actualResultMatrix = matrixA.SystemSolve(matrixB);

            actualResultMatrix.Should().Be(expectedResultMatrix);
        }

        [Test()]
        public void Matrix_DecomposeSimpleTest()
        {
            //Set up IDENTITY matrix A            
            Matrix matrixA = Matrix.IdentityMatrix(4);
                        
            //The LUP Decomposition

            //Correct L Part
            Matrix correctLPartOfLUPDecomposition = Matrix.IdentityMatrix(4);

            //Correct U Part
            Matrix correctUPartOfLUPDecomposition = Matrix.IdentityMatrix(4);

            //The other 2 output variables are a permutation array and the toggle value
            //int[] correctPermutationArray = { 3, 1, 2, 0 };
            //int correctToggleValue = -1;


            //Calculate values for the above
            int[] permutationArray;
            int toggleValue;

            Matrix decomposeResult = matrixA.Decompose(out permutationArray, out toggleValue);

            Matrix calculatedLPartOfLUPDecomposition = decomposeResult.ExtractLower();
            Matrix calculatedUPartOfLUPDecomposition = decomposeResult.ExtractUpper();

            //Check all the variables
                //permutationArray.ShouldBeEquivalentTo(correctPermutationArray);
                //toggleValue.ShouldBeEquivalentTo(correctToggleValue);
            calculatedLPartOfLUPDecomposition.ShouldBeEquivalentTo(correctLPartOfLUPDecomposition);
            calculatedUPartOfLUPDecomposition.ShouldBeEquivalentTo(correctLPartOfLUPDecomposition);
        }

        [Test()]
        public void Matrix_DecomposeComplicatedTest()
        {
            Matrix matrixA = new Matrix(4, 4);

            //Set up matrix A
            double[] columnOneOfMatrixA = { 3, 1, 2, 5 };
            double[] columnTwoOfMatrixA = { 7, 8, 1, 4 };
            double[] columnThreeOfMatrixA = { 2, 4, 9, 7 };
            double[] columnFourOfMatrixA = { 5, 2, 3, 1 };

            matrixA.SetColumn(0, columnOneOfMatrixA);
            matrixA.SetColumn(1, columnTwoOfMatrixA);
            matrixA.SetColumn(2, columnThreeOfMatrixA);
            matrixA.SetColumn(3, columnFourOfMatrixA);

            //The LUP Decomposition

            //Correct L Part
            Matrix correctLPartOfLUPDecomposition = new Matrix(4, 4);

            double[] columnOneOfLMatrix = { 1,0.2,0.4,0.6 };
            double[] columnTwoOfLMatrix = { 0, 1, -0.083, 0.639 };
            double[] columnThreeOfLMatrix = { 0, 0, 1, -0.602 };
            double[] columnFourOfLMatrix = { 0,0,0,1 };

            correctLPartOfLUPDecomposition.SetColumn(0, columnOneOfLMatrix);
            correctLPartOfLUPDecomposition.SetColumn(1, columnTwoOfLMatrix);
            correctLPartOfLUPDecomposition.SetColumn(2, columnThreeOfLMatrix);
            correctLPartOfLUPDecomposition.SetColumn(3, columnFourOfLMatrix);

            //Correct U Part
            Matrix correctUPartOfLUPDecomposition = new Matrix(4, 4);

            double[] columnOneOfUMatrix = { 5, 0,0,0 };
            double[] columnTwoOfUMatrix = { 4, 7.2, 0,0 };
            double[] columnThreeOfUMatrix = { 7, 2.6, 6.417, 0 };
            double[] columnFourOfUMatrix = { 1, 1.8, 2.75, 4.905 };

            correctUPartOfLUPDecomposition.SetColumn(0, columnOneOfUMatrix);
            correctUPartOfLUPDecomposition.SetColumn(1, columnTwoOfUMatrix);
            correctUPartOfLUPDecomposition.SetColumn(2, columnThreeOfUMatrix);
            correctUPartOfLUPDecomposition.SetColumn(3, columnFourOfUMatrix);

            //The other 2 output variables are a permutation array and the toggle value
            int[] correctPermutationArray = { 3, 1, 2, 0 };
            int correctToggleValue = -1;


            //Calculate values for the above
            int[] permutationArray;
            int toggleValue;

            Matrix decomposeResult = matrixA.Decompose(out permutationArray, out toggleValue);

            Matrix calculatedLPartOfLUPDecomposition = decomposeResult.ExtractLower();
            Matrix calculatedUPartOfLUPDecomposition = decomposeResult.ExtractUpper();
            
            //Check all the variables
            permutationArray.ShouldBeEquivalentTo(correctPermutationArray);
            toggleValue.ShouldBeEquivalentTo(correctToggleValue);

            //Compare the two matrices

            double tolerance = .001;
            Matrix LDifferenceMatrix = calculatedLPartOfLUPDecomposition - correctLPartOfLUPDecomposition;
            Matrix UDifferenceMatrix = calculatedUPartOfLUPDecomposition - correctUPartOfLUPDecomposition;

            for (int i = 0; i < LDifferenceMatrix.NumberOfRows; i++)
            {
                for (int j = 0; j < LDifferenceMatrix.NumberOfColumns; j++)
                {
                    (Math.Abs(LDifferenceMatrix.GetElement(i, j)) < tolerance).Should().BeTrue();
                }
            }

            for (int i = 0; i < UDifferenceMatrix.NumberOfRows; i++)
            {
                for (int j = 0; j < UDifferenceMatrix.NumberOfColumns; j++)
                {
                    (Math.Abs(UDifferenceMatrix.GetElement(i, j)) < tolerance).Should().BeTrue();
                }
            }
       
        }

        [Test()]
        public void Matrix_DecomposeTest2()
        {
            Matrix matrixA = new Matrix(9);

            //Set up matrix A
            double[] row1OfMatrixA = { 3, 0, 0, 7, 0, 0, 0, 0, 0 };
            double[] row2OfMatrixA = { 0, 1, 8, 0, 4, 2, 0, 0, 0 };
            double[] row3OfMatrixA = { 0, 2, 8, 0, 9, 3, 0, 0, 0 };
            double[] row4OfMatrixA = { 5, 0, 0, 4, 0, 7, 1, 0, 3 };
            double[] row5OfMatrixA = { 0, 3, 7, 0, 7, 8, 0, 3, 0 };
            double[] row6OfMatrixA = { 0, 7, 2, 5, 3, 7, 2, 0, 4 };
            double[] row7OfMatrixA = { 0, 0, 0 ,2, 0 ,8, 4 ,0, 7 };
            double[] row8OfMatrixA = { 0, 0, 0, 0, 9, 0, 0, 3, 0 };
            double[] row9OfMatrixA = { 0, 0, 0, 1, 0, 4, 7, 0, 6 };

            matrixA.SetRow(0, row1OfMatrixA);
            matrixA.SetRow(1, row2OfMatrixA);
            matrixA.SetRow(2, row3OfMatrixA);
            matrixA.SetRow(3, row4OfMatrixA);
            matrixA.SetRow(4, row5OfMatrixA);
            matrixA.SetRow(5, row6OfMatrixA);
            matrixA.SetRow(6, row7OfMatrixA);
            matrixA.SetRow(7, row8OfMatrixA);
            matrixA.SetRow(8, row9OfMatrixA);

            //The LUP Decomposition

            //Correct L Part
            Matrix correctLPartOfLUPDecomposition = new Matrix(9);

            double[] row1OfLMatrix = { 1, 0, 0, 0, 0, 0, 0, 0, 0 };
            double[] row2OfLMatrix = { 0, 1, 0, 0, 0, 0, 0, 0, 0 };
            double[] row3OfLMatrix = { 0, .143, 1, 0, 0, 0, 0, 0, 0 };
            double[] row4OfLMatrix = {.6, 0, 0, 1, 0, 0, 0, 0, 0 };
            double[] row5OfLMatrix = { 0, 0, 0, 0, 1, 0, 0, 0, 0 };
            double[] row6OfLMatrix = { 0, 0, 0, .435, 0, 1, 0, 0, 0 };
            double[] row7OfLMatrix = { 0, 0, 0, .217, 0, .5, 1, 0, 0 };
            double[] row8OfLMatrix = { 0, .429, .796, -.342, .319, .282, -.407, 1, 0 };
            double[] row9OfLMatrix = { 0.000,  0.286,  0.963, -0.161,  0.523, -0.065, -0.023, -0.767,  1.000 };

            correctLPartOfLUPDecomposition.SetRow(0, row1OfLMatrix);
            correctLPartOfLUPDecomposition.SetRow(1, row2OfLMatrix);
            correctLPartOfLUPDecomposition.SetRow(2, row3OfLMatrix);
            correctLPartOfLUPDecomposition.SetRow(3, row4OfLMatrix);
            correctLPartOfLUPDecomposition.SetRow(4, row5OfLMatrix);
            correctLPartOfLUPDecomposition.SetRow(5, row6OfLMatrix);
            correctLPartOfLUPDecomposition.SetRow(6, row7OfLMatrix);
            correctLPartOfLUPDecomposition.SetRow(7, row8OfLMatrix);
            correctLPartOfLUPDecomposition.SetRow(8, row9OfLMatrix);
          
            //Correct U Part
            Matrix correctUPartOfLUPDecomposition = new Matrix(9);

            double[] row1OfUMatrix = { 5.000,  0.000,  0.000,  4.000,  0.000,  7.000 , 1.000 , 0.000 , 3.000 };
            double[] row2OfUMatrix = { 0.000, 7.000, 2.000, 5.000, 3.000, 7.000, 2.000, 0.000, 4.000 };
            double[] row3OfUMatrix = { 0.000, 0.000, 7.714, -0.714, 3.571, 1.000, -0.286, 0.000, -0.571 };
            double[] row4OfUMatrix = { 0.000, 0.000, 0.000, 4.600, 0.000, -4.200, -0.600, 0.000, -1.800 };
            double[] row5OfUMatrix = { 0.000, 0.000, 0.000, 0.000, 9.000, 0.000, 0.000, 3.000, 0.000 };
            double[] row6OfUMatrix = { 0.000, 0.000, 0.000, 0.000, 0.000, 9.826, 4.261, 0.000, 7.783 };
            double[] row7OfUMatrix = { 0.000, 0.000, 0.000, 0.000, 0.000, 0.000, 5.000, 0.000, 2.500 };
            double[] row8OfUMatrix = { 0.000, 0.000, 0.000, 0.000, 0.000, 0.000, 0.000, 2.043, -3.049 };
            double[] row9OfUMatrix = { 0.000, 0.000, 0.000, 0.000, 0.000, 0.000, 0.000, 0.000, -2.658 };

            correctUPartOfLUPDecomposition.SetRow(0, row1OfUMatrix);
            correctUPartOfLUPDecomposition.SetRow(1, row2OfUMatrix);
            correctUPartOfLUPDecomposition.SetRow(2, row3OfUMatrix);
            correctUPartOfLUPDecomposition.SetRow(3, row4OfUMatrix);
            correctUPartOfLUPDecomposition.SetRow(4, row5OfUMatrix);
            correctUPartOfLUPDecomposition.SetRow(5, row6OfUMatrix);
            correctUPartOfLUPDecomposition.SetRow(6, row7OfUMatrix);
            correctUPartOfLUPDecomposition.SetRow(7, row8OfUMatrix);
            correctUPartOfLUPDecomposition.SetRow(8, row9OfUMatrix);

            //Calculate values for the above
            int[] permutationArray;
            int toggleValue;
            Matrix decomposeResult = matrixA.Decompose(out permutationArray, out toggleValue);

            Matrix calculatedLPartOfLUPDecomposition = decomposeResult.ExtractLower();
            Matrix calculatedUPartOfLUPDecomposition = decomposeResult.ExtractUpper();

            //Compare the two matrices

            double tolerance = .001;
            Matrix LDifferenceMatrix = calculatedLPartOfLUPDecomposition - correctLPartOfLUPDecomposition;
            Matrix UDifferenceMatrix = calculatedUPartOfLUPDecomposition - correctUPartOfLUPDecomposition;

            for (int i = 0; i < LDifferenceMatrix.NumberOfRows; i++)
            {
                for (int j = 0; j < LDifferenceMatrix.NumberOfColumns; j++)
                {
                    (Math.Abs(LDifferenceMatrix.GetElement(i, j)) < tolerance).Should().BeTrue();
                }
            }

            for (int i = 0; i < UDifferenceMatrix.NumberOfRows; i++)
            {
                for (int j = 0; j < UDifferenceMatrix.NumberOfColumns; j++)
                {
                    (Math.Abs(UDifferenceMatrix.GetElement(i, j)) < tolerance).Should().BeTrue();
                }
            }

        }

        [Test()]
        public void Matrix_DecomposeTest3_DifferentSigns()
        {
            Matrix matrixA = new Matrix(9);

            //Set up matrix A
            double[] row1OfMatrixA = { 3, 0, 0, -7, 0, 0, 0, 0, 0 };
            double[] row2OfMatrixA = { 0, 1, 8, 0, -4, 2, 0, 0, 0 };
            double[] row3OfMatrixA = { 0, 2, 8, 0, -9, 3, 0, 0, 0 };
            double[] row4OfMatrixA = { -5, 0, 0, 4, 0, 7, -1, 0, 3 };
            double[] row5OfMatrixA = { 0, -3, -7, 0, 7, -8, 0, -3, 0 };
            double[] row6OfMatrixA = { 0, 7, 2, 5, -3, 7, -2, 0, 4 };
            double[] row7OfMatrixA = { 0, 0, 0, -2, 0, -8, 4, 0, 7 };
            double[] row8OfMatrixA = { 0, 0, 0, 0, -9, 0, 0, 3, 0 };
            double[] row9OfMatrixA = { 0, 0, 0, 1, 0, 4, 7, 0, 6 };

            matrixA.SetRow(0, row1OfMatrixA);
            matrixA.SetRow(1, row2OfMatrixA);
            matrixA.SetRow(2, row3OfMatrixA);
            matrixA.SetRow(3, row4OfMatrixA);
            matrixA.SetRow(4, row5OfMatrixA);
            matrixA.SetRow(5, row6OfMatrixA);
            matrixA.SetRow(6, row7OfMatrixA);
            matrixA.SetRow(7, row8OfMatrixA);
            matrixA.SetRow(8, row9OfMatrixA);



            //The LUP Decomposition

            //Correct L Part
            Matrix correctLPartOfLUPDecomposition = new Matrix(9);

            double[] row1OfLMatrix = { 1, 0, 0, 0, 0, 0, 0, 0, 0 };
            double[] row2OfLMatrix = { 0, 1, 0, 0, 0, 0, 0, 0, 0 };
            double[] row3OfLMatrix = { 0, .143, 1, 0, 0, 0, 0, 0, 0 };
            double[] row4OfLMatrix = { -.6, 0, 0, 1, 0, 0, 0, 0, 0 };
            double[] row5OfLMatrix = { 0, 0, 0, 0, 1, 0, 0, 0, 0 };
            double[] row6OfLMatrix = { 0, 0, 0, .435, 0, 1, 0, 0, 0 };
            double[] row7OfLMatrix = { 0, 0, 0, -.217, 0, -.5, 1, 0, 0 };
            double[] row8OfLMatrix = { 0, -.429, -.796, -.342, -.319, .282, -.226, 1, 0 };
            double[] row9OfLMatrix = { 0.000, 0.286, 0.963, 0.161, 0.523, 0.065, 0.013, 0.767, 1.000 };

            correctLPartOfLUPDecomposition.SetRow(0, row1OfLMatrix);
            correctLPartOfLUPDecomposition.SetRow(1, row2OfLMatrix);
            correctLPartOfLUPDecomposition.SetRow(2, row3OfLMatrix);
            correctLPartOfLUPDecomposition.SetRow(3, row4OfLMatrix);
            correctLPartOfLUPDecomposition.SetRow(4, row5OfLMatrix);
            correctLPartOfLUPDecomposition.SetRow(5, row6OfLMatrix);
            correctLPartOfLUPDecomposition.SetRow(6, row7OfLMatrix);
            correctLPartOfLUPDecomposition.SetRow(7, row8OfLMatrix);
            correctLPartOfLUPDecomposition.SetRow(8, row9OfLMatrix);

            //Correct U Part
            Matrix correctUPartOfLUPDecomposition = new Matrix(9);

            double[] row1OfUMatrix = { -5.000, 0.000, 0.000, 4.000, 0.000, 7.000, -1.000, 0.000, 3.000 };
            double[] row2OfUMatrix = { 0.000, 7.000, 2.000, 5.000, -3.000, 7.000, -2.000, 0.000, 4.000 };
            double[] row3OfUMatrix = { 0.000, 0.000, 7.714, -0.714, -3.571, 1.000, 0.286, 0.000, -0.571 };
            double[] row4OfUMatrix = { 0.000, 0.000, 0.000, -4.600, 0.000, 4.200, -0.600, 0.000, 1.800 };
            double[] row5OfUMatrix = { 0.000, 0.000, 0.000, 0.000, -9.000, 0.000, 0.000, 3.000, 0.000 };
            double[] row6OfUMatrix = { 0.000, 0.000, 0.000, 0.000, 0.000, -9.826, 4.261, 0.000, 6.217 };
            double[] row7OfUMatrix = { 0.000, 0.000, 0.000, 0.000, 0.000, 0.000, 9.000, 0.000, 9.500 };
            double[] row8OfUMatrix = { 0.000, 0.000, 0.000, 0.000, 0.000, 0.000, 0.000, -2.043, 2.272 };
            double[] row9OfUMatrix = { 0.000, 0.000, 0.000, 0.000, 0.000, 0.000, 0.000, 0.000, -3.153 };

            correctUPartOfLUPDecomposition.SetRow(0, row1OfUMatrix);
            correctUPartOfLUPDecomposition.SetRow(1, row2OfUMatrix);
            correctUPartOfLUPDecomposition.SetRow(2, row3OfUMatrix);
            correctUPartOfLUPDecomposition.SetRow(3, row4OfUMatrix);
            correctUPartOfLUPDecomposition.SetRow(4, row5OfUMatrix);
            correctUPartOfLUPDecomposition.SetRow(5, row6OfUMatrix);
            correctUPartOfLUPDecomposition.SetRow(6, row7OfUMatrix);
            correctUPartOfLUPDecomposition.SetRow(7, row8OfUMatrix);
            correctUPartOfLUPDecomposition.SetRow(8, row9OfUMatrix);

            //Calculate values for the above
            int[] permutationArray;
            int toggleValue;
            Matrix decomposeResult = matrixA.Decompose(out permutationArray, out toggleValue);

            Matrix calculatedLPartOfLUPDecomposition = decomposeResult.ExtractLower();
            Matrix calculatedUPartOfLUPDecomposition = decomposeResult.ExtractUpper();

            //Compare the two matrices
            double tolerance = .001;
            Matrix LDifferenceMatrix = calculatedLPartOfLUPDecomposition - correctLPartOfLUPDecomposition;
            Matrix UDifferenceMatrix = calculatedUPartOfLUPDecomposition - correctUPartOfLUPDecomposition;

            for (int i = 0; i < LDifferenceMatrix.NumberOfRows; i++)
            {
                for (int j = 0; j < LDifferenceMatrix.NumberOfColumns; j++)
                {
                    (Math.Abs(LDifferenceMatrix.GetElement(i, j)) < tolerance).Should().BeTrue();
                }
            }

            for (int i = 0; i < UDifferenceMatrix.NumberOfRows; i++)
            {
                for (int j = 0; j < UDifferenceMatrix.NumberOfColumns; j++)
                {
                    (Math.Abs(UDifferenceMatrix.GetElement(i, j)) < tolerance).Should().BeTrue();
                }
            }



        }

        [Test()]
        public void Matrix_ExtractLowerComplicatedTest()
        {
            Matrix matrixA = new Matrix(4, 4);

            //Set up matrix A
            double[] columnOneOfMatrixA = { 3, 1, 2, 5 };
            double[] columnTwoOfMatrixA = { 7, 8, 1, 4 };
            double[] columnThreeOfMatrixA = { 2, 4, 9, 7 };
            double[] columnFourOfMatrixA = { 5, 2, 3, 1 };

            matrixA.SetColumn(0, columnOneOfMatrixA);
            matrixA.SetColumn(1, columnTwoOfMatrixA);
            matrixA.SetColumn(2, columnThreeOfMatrixA);
            matrixA.SetColumn(3, columnFourOfMatrixA);

            //The LUP Decomposition

            //Correct L Part
            Matrix correctLPartOfLUPDecomposition = new Matrix(4, 4);

            double[] columnOneOfLMatrix = { 1, 1, 2, 5 };
            double[] columnTwoOfLMatrix = { 0, 1, 1, 4 };
            double[] columnThreeOfLMatrix = { 0, 0, 1, 7 };
            double[] columnFourOfLMatrix = { 0, 0, 0, 1 };

            correctLPartOfLUPDecomposition.SetColumn(0, columnOneOfLMatrix);
            correctLPartOfLUPDecomposition.SetColumn(1, columnTwoOfLMatrix);
            correctLPartOfLUPDecomposition.SetColumn(2, columnThreeOfLMatrix);
            correctLPartOfLUPDecomposition.SetColumn(3, columnFourOfLMatrix);

            //Calculate L Part
            Matrix calculatedLPartOfLUPDecomposition = matrixA.ExtractLower();

            (calculatedLPartOfLUPDecomposition == correctLPartOfLUPDecomposition).Should().BeTrue();

        }

         [Test()]
        public void Matrix_ExtractUpperComplicatedTest()
        {
            Matrix matrixA = new Matrix(4, 4);

            //Set up matrix A
            double[] columnOneOfMatrixA = { 3, 1, 2, 5 };
            double[] columnTwoOfMatrixA = { 7, 8, 1, 4 };
            double[] columnThreeOfMatrixA = { 2, 4, 9, 7 };
            double[] columnFourOfMatrixA = { 5, 2, 3, 1 };

            matrixA.SetColumn(0, columnOneOfMatrixA);
            matrixA.SetColumn(1, columnTwoOfMatrixA);
            matrixA.SetColumn(2, columnThreeOfMatrixA);
            matrixA.SetColumn(3, columnFourOfMatrixA);

            //The LUP Decomposition

            //Correct U Part
            Matrix correctUPartOfLUPDecomposition = new Matrix(4, 4);

            double[] columnOneOfUMatrix = { 3, 0, 0, 0 };
            double[] columnTwoOfUMatrix = { 7, 8, 0, 0 };
            double[] columnThreeOfUMatrix = { 2, 4, 9, 0 };
            double[] columnFourOfUMatrix = { 5, 2, 3, 1 };

            correctUPartOfLUPDecomposition.SetColumn(0, columnOneOfUMatrix);
            correctUPartOfLUPDecomposition.SetColumn(1, columnTwoOfUMatrix);
            correctUPartOfLUPDecomposition.SetColumn(2, columnThreeOfUMatrix);
            correctUPartOfLUPDecomposition.SetColumn(3, columnFourOfUMatrix);

            //Calculate U Part
            Matrix calculatedUPartOfLUPDecomposition = matrixA.ExtractUpper();

            (calculatedUPartOfLUPDecomposition == correctUPartOfLUPDecomposition).Should().BeTrue();

        }

        #region Operator Tests

        [Test()]
         public void Matrix_EqualsOperatorTrueTest()
         {
            Matrix matrix1 = new Matrix(2, 2);
            Matrix matrix2 = new Matrix(2, 2);

            double[] matrix1Column1 = { 2, 3 };
            double[] matrix1Column2 = { 4, 5 };

            matrix1.SetColumn(0, matrix1Column1);
            matrix1.SetColumn(1, matrix1Column2);

            double[] matrix2Column1 = { 2, 3 };
            double[] matrix2Column2 = { 4, 5 };

            matrix2.SetColumn(0, matrix2Column1);
            matrix2.SetColumn(1, matrix2Column2);

            bool result = (matrix1 == matrix2);
            result.Should().BeTrue();
         }

        [Test()]
        public void Matrix_EqualsOperatorFalseTest()
        {
            Matrix matrix1 = new Matrix(2, 2);
            Matrix matrix2 = new Matrix(2, 2);

            double[] matrix1Column1 = { 2, 3 };
            double[] matrix1Column2 = { 4, 5 };

            matrix1.SetColumn(0, matrix1Column1);
            matrix1.SetColumn(1, matrix1Column2);

            double[] matrix2Column1 = { 2, 3 };
            double[] matrix2Column2 = { 4, 1 };

            matrix2.SetColumn(0, matrix2Column1);
            matrix2.SetColumn(1, matrix2Column2);

            bool result = (matrix1 == matrix2);
            result.Should().BeFalse();
        }


        [Test()]
        public void Matrix_EqualsTest()
        {
            Matrix matrix1 = new Matrix(2, 2);
            Matrix matrix2 = new Matrix(2, 2);

            double[] matrix1Column1 = { 2, 3 };
            double[] matrix1Column2 = { 4, 5 };

            matrix1.SetColumn(0, matrix1Column1);
            matrix1.SetColumn(1, matrix1Column2);

            double[] matrix2Column1 = { 2, 3 };
            double[] matrix2Column2 = { 4, 5 };

            matrix2.SetColumn(0, matrix2Column1);
            matrix2.SetColumn(1, matrix2Column2);

            bool result = matrix1.Equals(matrix2);
            result.Should().BeTrue();
        }

        [Test()]
        public void Matrix_MinusOperatorTest()
        {
            Matrix matrix1 = new Matrix(2, 2);
            Matrix matrix2 = new Matrix(2, 2);

            double[] matrix1Column1 = { 2, 3 };
            double[] matrix1Column2 = { 4, 5 };

            matrix1.SetColumn(0, matrix1Column1);
            matrix1.SetColumn(1, matrix1Column2);

            double[] matrix2Column1 = { 1, 1 };
            double[] matrix2Column2 = { 1, 1 };

            matrix2.SetColumn(0, matrix2Column1);
            matrix2.SetColumn(1, matrix2Column2);

            Matrix expectedResult = new Matrix(2);

            double[] expectedResultColumn1 = { 1, 2 };
            double[] expectedResultColumn2 = { 3, 4 };

            expectedResult.SetColumn(0, expectedResultColumn1);
            expectedResult.SetColumn(1, expectedResultColumn2);

            Matrix actualResult = matrix1 - matrix2;

            bool equalityResult = (actualResult == expectedResult);

            equalityResult.Should().BeTrue();
        }

        [Test()]
        public void Matrix_MultiplyOperatorTest()
        {
            Matrix matrix1 = new Matrix(2, 2);

            double[] matrix1Column1 = { 2, 3 };
            double[] matrix1Column2 = { 4, 5 };

            matrix1.SetColumn(0, matrix1Column1);
            matrix1.SetColumn(1, matrix1Column2);

            Matrix matrix2 = new Matrix(2, 2);

            double[] matrix2Column1 = { 1, 3 };
            double[] matrix2Column2 = { 5, 7 };

            matrix2.SetColumn(0, matrix2Column1);
            matrix2.SetColumn(1, matrix2Column2);

            Matrix expectedResult = new Matrix(2);

            double[] expectedResultColumn1 = { 14, 18 };
            double[] expectedResultColumn2 = { 38, 50 };

            expectedResult.SetColumn(0, expectedResultColumn1);
            expectedResult.SetColumn(1, expectedResultColumn2);

            Matrix actualResult = matrix1 * matrix2;

            bool equalityResult = (actualResult == expectedResult);

            equalityResult.Should().BeTrue();
        }

        [Test()]
        public void Matrix_ScalarMultiplyOperatorTest()
        {
            Matrix matrix1 = new Matrix(2, 2);

            double[] matrix1Column1 = { 2, 3 };
            double[] matrix1Column2 = { 4, 5 };

            matrix1.SetColumn(0, matrix1Column1);
            matrix1.SetColumn(1, matrix1Column2);

            Matrix matrix2 = new Matrix(matrix1);

            double multiplier = 3;

            Matrix expectedResult = new Matrix(2);

            double[] expectedResultColumn1 = { 6, 9 };
            double[] expectedResultColumn2 = { 12, 15 };

            expectedResult.SetColumn(0, expectedResultColumn1);
            expectedResult.SetColumn(1, expectedResultColumn2);

            Matrix actualResult1 = matrix1 * multiplier;
            Matrix actualResult2 = multiplier * matrix2;

            bool equalityResult = (actualResult1 == expectedResult && actualResult2 == expectedResult);

            equalityResult.Should().BeTrue();
        }

        #endregion

        #region Method Tests

        [Test()]
        public void Matrix_IsSquareTrueTest()
        {
            Matrix matrix1 = new Matrix(2, 2);

            double[] matrix1Column1 = { 2, 3 };
            double[] matrix1Column2 = { 4, 5 };

            matrix1.SetColumn(0, matrix1Column1);
            matrix1.SetColumn(1, matrix1Column2);

            matrix1.IsSquare().Should().BeTrue();

        }

        [Test()]
        public void Matrix_IsSquareFalseTest()
        {
            Matrix matrix1 = new Matrix(3, 2);

            double[] matrix1Column1 = { 2, 3, 1 };
            double[] matrix1Column2 = { 4, 5, 1 };

            matrix1.SetColumn(0, matrix1Column1);
            matrix1.SetColumn(1, matrix1Column2);

            matrix1.IsSquare().Should().BeFalse();

        }

        [Test()]
        public void Matrix_IsSquareZeroTest()
        {
            Matrix matrix1 = new Matrix(3, 3);

            matrix1.IsSquare().Should().BeTrue();
        }

        [Test()]
        public void Matrix_GetSubMatrixTwiceTest()
        {
            Matrix matrix1 = new Matrix(3);

            double[] matrix1Column1 = { 1, 5, 6 };
            double[] matrix1Column2 = { 2, 4, 7 };
            double[] matrix1Column3 = { 3, 6, 8 };

            matrix1.SetColumn(0, matrix1Column1);
            matrix1.SetColumn(1, matrix1Column2);
            matrix1.SetColumn(2, matrix1Column3);
            
            Matrix expectedResult = new Matrix(1);

            double[] expectedResultColumn = { 4 };

            expectedResult.SetColumn(0, expectedResultColumn);

            Matrix actualResult = matrix1.GetSubMatrix(0, 0).GetSubMatrix(1,1);

            bool equalityResult = (actualResult == expectedResult);

            equalityResult.Should().BeTrue();

        }

        [Test()]
        public void Matrix_GetSubMatrixStandardTest()
        {
            Matrix matrix1 = new Matrix(3);

            double[] matrix1Column1 = { 1, 5, 6 };
            double[] matrix1Column2 = { 2, 4, 7 };
            double[] matrix1Column3 = { 3, 6, 8 };

            matrix1.SetColumn(0, matrix1Column1);
            matrix1.SetColumn(1, matrix1Column2);
            matrix1.SetColumn(2, matrix1Column3);

            Matrix expectedResult = new Matrix(2);

            double[] expectedResultColumn1 = { 4, 7 };
            double[] expectedResultColumn2 = { 6, 8 };

            expectedResult.SetColumn(0, expectedResultColumn1);
            expectedResult.SetColumn(1, expectedResultColumn2);

            Matrix cofactor = new Matrix(2);

            Matrix actualResult = matrix1.GetSubMatrix(0, 0);

            bool equalityResult = (actualResult == expectedResult);

            equalityResult.Should().BeTrue();

        }

        [Test()]
        public void Matrix_InsertMatrixTest()
        {
            Matrix originalMatrix = new Matrix(3);

            double[] matrix1Row1 = { 1, 5, 6 };
            double[] matrix1Row2 = { 2, 4, 7 };
            double[] matrix1Row3 = { 3, 6, 8 };

            originalMatrix.SetRow(0, matrix1Row1);
            originalMatrix.SetRow(1, matrix1Row2);
            originalMatrix.SetRow(2, matrix1Row3);

            Matrix matrixToInsert = Matrix.IdentityMatrix(2);

            Matrix expectedResult = new Matrix(3);

            double[] expectedResultRow1 = { 1, 5, 6 };
            double[] expectedResultRow2 = { 1, 0, 7 };
            double[] expectedResultRow3 = { 0, 1, 8 };

            expectedResult.SetRow(0, expectedResultRow1);
            expectedResult.SetRow(1, expectedResultRow2);
            expectedResult.SetRow(2, expectedResultRow3);

            Matrix actualResult = originalMatrix.InsertMatrixAt(matrixToInsert, 1, 0);

            bool equalityResult = (actualResult == expectedResult);

            equalityResult.Should().BeTrue();

        }

        [Test()]
        public void DeterminantSmallMatrix()
        {
            Matrix matrix1 = new Matrix(2, 2);

            double[] matrix1Column1 = { 2, 4 };
            double[] matrix1Column2 = { 4, 5 };
            
            matrix1.SetColumn(0, matrix1Column1);
            matrix1.SetColumn(1, matrix1Column2);

            double expectedResult = -6;

            double actualResult = matrix1.Determinant();

            bool equalityResult = (actualResult == expectedResult);

            equalityResult.Should().BeTrue();
        }

        [Test()]
        public void Matrix_DeterminantStandardTest()
        {
            Matrix matrix1 = new Matrix(3, 3);

            double[] matrix1Column1 = { 2, 4, 4 };
            double[] matrix1Column2 = { 4, 5, 6 };
            double[] matrix1Column3 = { 1, 2, 3 };


            matrix1.SetColumn(0, matrix1Column1);
            matrix1.SetColumn(1, matrix1Column2);
            matrix1.SetColumn(2, matrix1Column3);


            double expectedResult = -6;

            double actualResult = matrix1.Determinant();

            bool equalityResult = (actualResult == expectedResult);

            equalityResult.Should().BeTrue();
        }

        [Test()]
        public void Matrix_DeterminantIsZeroTest()
        {
            Matrix matrix1 = new Matrix(2, 2);

            double[] matrix1Column1 = { 2, 4 };
            double[] matrix1Column2 = { 4, 8 };

            matrix1.SetColumn(0, matrix1Column1);
            matrix1.SetColumn(1, matrix1Column2);

            double expectedResult = 0;

            double actualResult = matrix1.Determinant();

            bool equalityResult = (actualResult == expectedResult);

            equalityResult.Should().BeTrue();
        }

        [Test()]
        public void Matrix_IsInvertibleTrueTest()
        {
            Matrix matrix1 = new Matrix(2, 2);

            double[] matrix1Column1 = { 1, 4 };
            double[] matrix1Column2 = { 4, 8 };

            matrix1.SetColumn(0, matrix1Column1);
            matrix1.SetColumn(1, matrix1Column2);

            matrix1.IsInvertible().Should().BeTrue();
        }

        [Test()]
        public void Matrix_IsInvertibleFalseBecauseOfDeterminantTest()
        {
            Matrix matrix1 = new Matrix(2, 2);

            double[] matrix1Column1 = { 2, 4 };
            double[] matrix1Column2 = { 4, 8 };

            matrix1.SetColumn(0, matrix1Column1);
            matrix1.SetColumn(1, matrix1Column2);

            matrix1.IsInvertible().Should().BeFalse();
        }

        [Test()]
        public void Matrix_GenerateCofactorMatrixTest()
        {
            Matrix matrix1 = new Matrix(3);

            double[] matrix1Column1 = { 1, 5, 6 };
            double[] matrix1Column2 = { 2, 4, 7 };
            double[] matrix1Column3 = { 3, 6, 8 };

            matrix1.SetColumn(0, matrix1Column1);
            matrix1.SetColumn(1, matrix1Column2);
            matrix1.SetColumn(2, matrix1Column3);

            Matrix expectedResult = new Matrix(3);

            double[] expectedResultColumn1 = { -10, 5, 0 };
            double[] expectedResultColumn2 = { -4, -10, 9};
            double[] expectedResultColumn3 = { 11, 5, -6 };

            expectedResult.SetColumn(0, expectedResultColumn1);
            expectedResult.SetColumn(1, expectedResultColumn2);
            expectedResult.SetColumn(2, expectedResultColumn3);
            
            Matrix actualResult = matrix1.GenerateCofactorMatrix();

            bool equalityResult = (actualResult == expectedResult);

            equalityResult.Should().BeTrue();
        }

        [Test()]
        public void Matrix_InvertTest()
        {
            Matrix matrix1 = new Matrix(3);

            double[] matrix1Column1 = { 1, 5, 6 };
            double[] matrix1Column2 = { 2, 4, 7 };
            double[] matrix1Column3 = { 3, 6, 8 };

            matrix1.SetColumn(0, matrix1Column1);
            matrix1.SetColumn(1, matrix1Column2);
            matrix1.SetColumn(2, matrix1Column3);

            Matrix expectedResult = new Matrix(3);

            double[] expectedResultColumn1 = { (double)-2 / 3, (double)-4 / 15, (double)11 / 15 };
            double[] expectedResultColumn2 = { (double)1 / 3, (double)-2 / 3, (double)1 / 3 };
            double[] expectedResultColumn3 = { (double)0, (double)3 / 5, (double)-2 / 5 };

            expectedResult.SetColumn(0, expectedResultColumn1);
            expectedResult.SetColumn(1, expectedResultColumn2);
            expectedResult.SetColumn(2, expectedResultColumn3);

            Matrix actualResult = matrix1.Invert();

            bool equalityResult = (actualResult == expectedResult);

            equalityResult.Should().BeTrue();
        }

        [Test()]
        public void Matrix_MatrixTimesItsInverseTest()
        {
            Matrix matrix1 = new Matrix(4);

            double[] matrix1Column1 = { 1, 5, 6, 7 };
            double[] matrix1Column2 = { 2, 4, 7, 6 };
            double[] matrix1Column3 = { 3, 6, 8, 7 };
            double[] matrix1Column4 = { 7, 6, 8, 7 };

            matrix1.SetColumn(0, matrix1Column1);
            matrix1.SetColumn(1, matrix1Column2);
            matrix1.SetColumn(2, matrix1Column3);
            matrix1.SetColumn(3, matrix1Column4);

            Matrix expectedResult = Matrix.IdentityMatrix(4);

            Matrix actualResult = matrix1 * matrix1.Invert();

            bool equalityResult = (actualResult == expectedResult);

            equalityResult.Should().BeTrue();
        }

        [Test()]
        public void Matrix_AbsoluteValueOfMatrixTest()
        {
            Matrix matrix1 = new Matrix(3);

            double[] matrix1Column1 = { -1, 5, 6 };
            double[] matrix1Column2 = { 2, -4, 7 };
            double[] matrix1Column3 = { 3, 6, -8 };

            matrix1.SetColumn(0, matrix1Column1);
            matrix1.SetColumn(1, matrix1Column2);
            matrix1.SetColumn(2, matrix1Column3);

            Matrix expectedResult = new Matrix(3);

            double[] expectedResultColumn1 = { 1, 5, 6 };
            double[] expectedResultColumn2 = { 2, 4, 7 };
            double[] expectedResultColumn3 = { 3, 6, 8 };

            expectedResult.SetColumn(0, expectedResultColumn1);
            expectedResult.SetColumn(1, expectedResultColumn2);
            expectedResult.SetColumn(2, expectedResultColumn3);

            Matrix actualResult = matrix1.AbsoluteValue();

            bool equalityResult = (actualResult == expectedResult);

            equalityResult.Should().BeTrue();
        }

        [Test()]
        public void Matrix_CreateIdentityMatrixTest()
        {
            Matrix expectedResult = new Matrix(3);

            double[] expectedResultColumn1 = { 1, 0, 0 };
            double[] expectedResultColumn2 = { 0, 1, 0 };
            double[] expectedResultColumn3 = { 0, 0, 1 };

            expectedResult.SetColumn(0, expectedResultColumn1);
            expectedResult.SetColumn(1, expectedResultColumn2);
            expectedResult.SetColumn(2, expectedResultColumn3);

            Matrix actualResult = Matrix.IdentityMatrix(3);

            bool equalityResult = (actualResult == expectedResult);

            equalityResult.Should().BeTrue();
        }

        [Test()]
        public void Matrix_TransposeTest()
        {
            Matrix matrix1 = new Matrix(3);

            double[] matrix1Column1 = { 1, 5, 6 };
            double[] matrix1Column2 = { 2, 4, 7 };
            double[] matrix1Column3 = { 3, 6, 8 };

            matrix1.SetColumn(0, matrix1Column1);
            matrix1.SetColumn(1, matrix1Column2);
            matrix1.SetColumn(2, matrix1Column3);

            Matrix expectedResult = new Matrix(3);

            double[] expectedResultColumn1 = { 1, 2, 3 };
            double[] expectedResultColumn2 = { 5, 4, 6 };
            double[] expectedResultColumn3 = { 6, 7, 8 };

            expectedResult.SetColumn(0, expectedResultColumn1);
            expectedResult.SetColumn(1, expectedResultColumn2);
            expectedResult.SetColumn(2, expectedResultColumn3);

            Matrix actualResult = matrix1.Transpose();

            bool equalityResult = (actualResult == expectedResult);

            equalityResult.Should().BeTrue();
        }

        [Test]
        public void Matrix_RotateAboutXTest()
        {
            Point pointToRotate = Point.MakePointWithInches(4, -2, 2);
            Angle angleToRotate = new Angle(new Degree(), 199);

            Matrix rotationMatrix = Matrix.RotationMatrixAboutX(angleToRotate);

            Matrix actualResultMatrix = rotationMatrix * pointToRotate.ConvertToMatrixColumn();

            double[] expectedResultColumn = { 4.0, 2.5421734601129469, -1.23990084228432 };

            Matrix expectedResultMatrix = new Matrix(3, 1);
            expectedResultMatrix.SetColumn(0, expectedResultColumn);

            (actualResultMatrix == expectedResultMatrix).Should().BeTrue();
        }

        [Test]
        public void Matrix_RotateAboutYTest()
        {
            Point pointToRotate = Point.MakePointWithInches(4, -2, 2);
            Angle angleToRotate = new Angle(new Degree(), 199);

            Matrix rotationMatrix = Matrix.RotationMatrixAboutY(angleToRotate);

            Matrix actualResultMatrix = rotationMatrix * pointToRotate.ConvertToMatrixColumn();

            double[] expectedResultColumn = { -4.4332106113115808, -2, -0.58876453337000645 };

            Matrix expectedResultMatrix = new Matrix(3, 1);
            expectedResultMatrix.SetColumn(0, expectedResultColumn);

            (actualResultMatrix == expectedResultMatrix).Should().BeTrue();
        }

        [Test]
        public void Matrix_RotateAboutZTest()
        {
            Point pointToRotate = Point.MakePointWithInches(4, -2, 2);
            Angle angleToRotate = new Angle(new Degree(), 199);

            Matrix rotationMatrix = Matrix.RotationMatrixAboutZ(angleToRotate);

            Matrix actualResultMatrix = rotationMatrix * pointToRotate.ConvertToMatrixColumn();

            double[] expectedResultColumn = { -4.4332106113115808, 0.58876453337000645, 2.0 };

            Matrix expectedResultMatrix = new Matrix(3, 1);
            expectedResultMatrix.SetColumn(0, expectedResultColumn);

            (actualResultMatrix == expectedResultMatrix).Should().BeTrue();            
        }

        [Test()]
        public void Matrix_RemoveRowsAndColumnsTest()
        {
            Matrix testMatrix = new Matrix(3);
            testMatrix.InsertMatrixAt(Matrix.IdentityMatrix(2), 1, 1);

            int[] rowsToRemove = { 0 };
            int[] columnsToRemove = { 0, 1 };

            Matrix actualMatrix = testMatrix.RemoveRows(rowsToRemove);
            actualMatrix = actualMatrix.RemoveColumns(columnsToRemove);

            Matrix expectedMatrix = new Matrix(2, 1);
            expectedMatrix.SetElement(1, 0, 1.0);

            (actualMatrix == expectedMatrix).Should().BeTrue();
        }

        [Test()]
        public void Matrix_RemoveColumnsTest()
        {
            Matrix testMatrix = new Matrix(2,5);

            double[] row1 = { 1, 2, 3, 4, 5 };
            double[] row2 = { 1, 2, 3, 4, 5 };

            testMatrix.SetRow(0, row1);
            testMatrix.SetRow(1, row2);
           
            int[] columnsToRemove = { 1, 3, 4 };

            Matrix actualMatrix = testMatrix.RemoveColumns(columnsToRemove);

            Matrix expectedMatrix = new Matrix(2);

            double[] expectedRow1 = { 1, 3};
            double[] expectedRow2 = { 1, 3};

            expectedMatrix.SetRow(0, expectedRow1);
            expectedMatrix.SetRow(1, expectedRow2);

            (actualMatrix == expectedMatrix).Should().BeTrue();
        }

        [Test()]
        public void Matrix_GetAnglesOutOfRotationMatrix()
        {
            Angle xAngle = new Angle(new Degree(), 90);
            Angle yAngle = new Angle(new Degree(), -33);
            Angle zAngle = new Angle(new Degree(), 40);

            Matrix xMatrix = Matrix.RotationMatrixAboutX(xAngle);
            Matrix yMatrix = Matrix.RotationMatrixAboutY(yAngle);
            Matrix zMatrix = Matrix.RotationMatrixAboutZ(zAngle);

            Matrix xyzRotation = zMatrix * yMatrix * xMatrix;

            List<Angle> results = xyzRotation.GetAnglesOutOfRotationMatrixForXYZRotationOrder();

            (results[0] == xAngle).Should().BeTrue();
            (results[1] == yAngle).Should().BeTrue();
            (results[2] == zAngle).Should().BeTrue();

            Angle xAngle2 = new Angle(new Degree(), -124);
            Angle yAngle2 = new Angle(new Degree(), -56.32);
            Angle zAngle2 = new Angle(new Degree(), 6.221);

            Matrix xMatrix2 = Matrix.RotationMatrixAboutX(xAngle2);
            Matrix yMatrix2 = Matrix.RotationMatrixAboutY(yAngle2);
            Matrix zMatrix2 = Matrix.RotationMatrixAboutZ(zAngle2);

            Matrix xyzRotation2 = zMatrix2 * yMatrix2 * xMatrix2;

            List<Angle> results2 = xyzRotation2.GetAnglesOutOfRotationMatrixForXYZRotationOrder();

            (results2[0] == xAngle2).Should().BeTrue();
            (results2[1] == yAngle2).Should().BeTrue();
            (results2[2] == zAngle2).Should().BeTrue();
        }
        
        #endregion
    }
}
