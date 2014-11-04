using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using UnitClassLibrary;

namespace GeometryClassLibrary
{
    [DebuggerDisplay("Size = {NumberOfRows} x {NumberOfColumns} |  First Row = {this.GetElement(0,0)} {this.GetElement(0,1)}... |  Second Row = {this.GetElement(1,0)} {this.GetElement(1,1)}...")]
    public class Matrix
    {
        #region Properties and Fields

        // declares a two dimensional array named _matrix. The "," denotes that it is 2d
        double[,] _matrix;


        /// <summary>
        /// Returns the number of rows in the matrix
        /// </summary>
        public int NumberOfRows
        {
            get
            {
                return _matrix.GetLength(0);
            }
        }

        /// <summary>
        /// Returns the number of columns in the matrix
        /// </summary>
        public int NumberOfColumns
        {
            get
            {
                // Check to make sure that there is at least one row
                if (_matrix.Length != 0)
                {
                    return _matrix.GetLength(1);
                }
                else
                {
                    return 0;
                }
            }
        }

#endregion

        #region Constructors

        /// <summary>
        /// Creates a square matrix with the passed number of rows and columns
        /// </summary>
        /// <param name="numRowsAndColumns"></param>
        public Matrix(int numRowsAndColumns)
        {
            _matrix = new double[numRowsAndColumns, numRowsAndColumns];
        }

        /// <summary>
        ///the constructor that is called when you say "new Matrix(numberOfRows, numberOfColumns);"
        /// </summary>
        /// <param name="numRows">The desired number of rows in the new matrix</param>
        /// <param name="numCols">The desired number of columns in the new matrix</param>
        public Matrix (int numRows, int numCols) 
        {
            _matrix = new double[numRows, numCols];
        }

        /// <summary>
        /// Creates a matrix with the specified number of rows and columns, and containing the values in the given array, filled in from top-to-bottom and left-to-right
        /// </summary>
        /// <param name="passedNumRows"></param>
        /// <param name="passedNumColumns"></param>
        /// <param name="passedValuesInMatrix"></param>
        public Matrix (int passedNumRows, int passedNumColumns, double[] passedValuesInMatrix)
        {
            Matrix newMatrix = new Matrix(passedNumRows, passedNumColumns);
            int arrayIndex = 0;
            for (int i = 0; i < passedNumRows; i++) //row loop
            {
                for (int j = 0; j < passedNumColumns; j++) //column loop
                {
                    if (arrayIndex < passedValuesInMatrix.Count())
                    {
                        newMatrix.SetElement(i, j, passedValuesInMatrix[arrayIndex]);
                        arrayIndex++;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            _matrix = newMatrix.getMatrixAs2DArray;
        }

        /// <summary>
        /// Makes a copy of the passed matrix
        /// </summary>
        /// <param name="passedMatrix"></param>
        public Matrix(Matrix passedMatrix)
        {
            int rows = passedMatrix.NumberOfRows;
            int cols = passedMatrix.NumberOfColumns;

            _matrix = new double[rows, cols];

            this.InsertMatrixAt(passedMatrix, 0, 0);
        }

        #endregion      
  
        #region Overloaded Operators

        /// <summary>
        /// Not a perfect equality operator, is only accurate up to difference of 0.00000001 in any two elements
        /// </summary>
        public static bool operator ==(Matrix matrix1, Matrix matrix2)
        {
            if ((object)matrix1 == null)
            {
                if ((object)matrix2 == null)
                {
                    return true;
                }
                return false;
            }
            return matrix1.Equals(matrix2);
        }

        /// <summary>
        /// Not a perfect inequality operator, is only accurate up to a difference of 0.00000001 in any two elements
        /// </summary>
        public static bool operator !=(Matrix matrix1, Matrix matrix2)
        {
            if ((object)matrix1 == null)
            {
                if ((object)matrix2 == null)
                {
                    return false;
                }
                return true;
            }
            return !matrix1.Equals(matrix2);
        }

        /// <summary>
        /// Does the same thing as == if the passed-in object is an identical matrix
        /// </summary>
        public override bool Equals(object obj)
        {
            //make sure the passed obj is not null
            if(obj == null)
            {
                return false;
            }

            //try casting and then comparing
            try
            {
                Matrix comparableMatrix = (Matrix)obj;

                if (this.NumberOfColumns != comparableMatrix.NumberOfColumns || this.NumberOfRows != comparableMatrix.NumberOfRows)
                {
                    return false;
                }

                for (int rowIndex = 0; rowIndex < this.NumberOfRows; rowIndex++)
                {
                    for (int columnIndex = 0; columnIndex < this.NumberOfColumns; columnIndex++)
                    {
                        if (Math.Abs(this.GetElement(rowIndex, columnIndex) - comparableMatrix.GetElement(rowIndex, columnIndex)) > 0.001)
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
            //if it was not a matrix than it was not equal
            catch (InvalidCastException)
            {
                return false;
            }
        }

        /// <summary>
        /// Returns the sum of 2 matrices of equal size
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="m2"></param>
        /// <returns></returns>
        public static Matrix operator +(Matrix m1, Matrix m2)
        {
            return m1.AddTo(m2);
        }

        /// <summary>
        /// Returns the difference of 2 matrices of equal size
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="m2"></param>
        /// <returns></returns>
        public static Matrix operator -(Matrix m1, Matrix m2)
        {
            return m1.AddTo(m2 * -1);
        }

        /// <summary>
        /// Returns the product of the 2 matrices if they can be multiplied
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="m2"></param>
        /// <returns></returns>
        public static Matrix operator *(Matrix m1, Matrix m2)
        {
            return m1.MultiplyBy(m2);
        }

        /// <summary>
        /// Returns a new matrix with each element multiplied by the passed multiplier
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="scalarMultiplier"></param>
        /// <returns></returns>
        public static Matrix operator *(Matrix m1, double scalarMultiplier)
        {
            for (int i = 0; i < m1.NumberOfRows; i++ )
            {
                for (int j = 0; j < m1.NumberOfColumns; j++)
                {
                    double newElement = scalarMultiplier * m1.GetElement(i, j);
                    m1.SetElement(i, j, newElement);
                }
            }
            return m1;
        }

        /// <summary>
        /// Returns a new matrix with each element multiplied by the passed multiplier
        /// </summary>
        /// <param name="scalarMultiplier"></param>
        /// <param name="m1"></param>
        /// <returns></returns>
        public static Matrix operator *(double scalarMultiplier, Matrix m1)
        {
            for (int i = 0; i < m1.NumberOfRows; i++)
            {
                for (int j = 0; j < m1.NumberOfColumns; j++)
                {
                    double newElement = scalarMultiplier * m1.GetElement(i, j);
                    m1.SetElement(i, j, newElement);
                }
            }
            return m1;
        }

       
        #endregion

        #region Methods

        #region Get/Set Methods
        /// <summary>
        /// </summary>
        /// <param name="rowIndex">The number of the row where the element will be added</param>
        /// <param name="columnIndex">The number of the column where the element will be added</param>
        /// <param name="element"></param>
        public void SetElement(int rowIndex, int columnIndex, double element)
        {
            _matrix[rowIndex, columnIndex] = element;
        }

        /// <summary>
        /// Returns the element at row "rowIndex" and column "columnIndex"
        /// </summary>
        /// <param name="rowIndex">The row number of the desired element</param>
        /// <param name="columnIndex">The column number of the desired element</param>
        /// <returns></returns>
        public double GetElement(int rowIndex, int columnIndex)
        {
            return _matrix[rowIndex, columnIndex];
        }


        /// <summary>
        /// Returns the specified row of the matrix
        /// </summary>
        /// <param name="passedrowIndex"></param>
        /// <returns></returns>
        public double[] GetRow(int passedrowIndex)
        {
            double[] rowToReturn = new double[NumberOfColumns];

            for (int columnIndex = 0; columnIndex < NumberOfColumns; columnIndex++)
            {
                rowToReturn[columnIndex] = GetElement(passedrowIndex, columnIndex);
            }

            return rowToReturn;
        }

        /// <summary>
        /// Replaces the specified row of the matrix with the passed row
        /// </summary>
        /// <param name="passedrowIndex"></param>
        /// <returns></returns>
        public void SetRow(int passedrowIndex, double[] passedRow)
        {
            for (int columnIndex = 0; columnIndex < passedRow.Length; columnIndex++)
            {
                SetElement(passedrowIndex, columnIndex, passedRow[columnIndex]);
            }
        }

        /// <summary>
        /// Returns the specified column of the matrix
        /// </summary>
        /// <param name="passedColumnIndex"></param>
        /// <returns></returns>
        public double[] GetColumn(int passedColumnIndex)
        {
            double[] columnToReturn = new double[NumberOfRows];

            for (int rowIndex = 0; rowIndex < NumberOfRows; rowIndex++)
            {
                columnToReturn[rowIndex] = GetElement(rowIndex, passedColumnIndex);
            }

            return columnToReturn;
        }

        /// <summary>
        /// Replaces the specified column of the matrix with the passed column
        /// </summary>
        /// <param name="passedrowIndex"></param>
        /// <returns></returns>
        public void SetColumn(int passedColumnIndex, double[] passedColumn)
        {
            for (int rowIndex = 0; rowIndex < passedColumn.Length; rowIndex++)
            {
                SetElement(rowIndex, passedColumnIndex, passedColumn[rowIndex]);
            }
        }       


        /// <summary>
        /// For internal use only. Returns the matrix as a double[,]
        /// </summary>
        internal double[,] getMatrixAs2DArray
        {
            get { return _matrix; }
        }

        #endregion

        /// <summary>
        /// Returns a matrix with the specified row and column removed
        /// </summary>
        /// <param name="indexOfRowToLeaveOut"></param>
        /// <param name="indexOfColumnToLeaveOut"></param>
        /// <returns></returns>
        public Matrix GetSubMatrix(int indexOfRowToLeaveOut, int indexOfColumnToLeaveOut)
        {
            Matrix subMatrix = new Matrix(this.NumberOfRows - 1, this.NumberOfColumns - 1);

            subMatrix = this.RemoveRow(indexOfRowToLeaveOut);
            subMatrix = subMatrix.RemoveColumn(indexOfColumnToLeaveOut);

            return subMatrix;
        }

        /// <summary>
        /// Takes values from a matrix and determines a cofactor by removing a certain row and column from the matrix.
        /// Logic developed with help from Andrew Morton on stackoverflow:
        /// http://stackoverflow.com/questions/24416946/next-step-in-calculating-a-matrix-determinant
        /// </summary>
        /// <param name="passedMatrix"></param>
        /// <param name="knockOutColumn"></param>
        /// <returns></returns>
        public Matrix GetSubMatrix(int[] indicesOfRowsToKeep, int[] indicesOfColumnsToKeep)
        {
            Matrix subMatrix = new Matrix(indicesOfRowsToKeep.Length, indicesOfColumnsToKeep.Length);
            Matrix tempMatrix = new Matrix(indicesOfRowsToKeep.Length, this.NumberOfColumns);

            int insertRowAt = 0;
            foreach (int rowToKeep in indicesOfRowsToKeep)
            {
                tempMatrix.SetRow(insertRowAt, this.GetRow(rowToKeep));
                insertRowAt++;
            }

            int insertColumnAt = 0;
            foreach (int columnToKeep in indicesOfColumnsToKeep)
            {
                subMatrix.SetColumn(insertColumnAt, tempMatrix.GetColumn(columnToKeep));
                insertColumnAt++;
            }

            return subMatrix;
        }

        /// <summary>
        /// Returns a new matrix with all of the specified rows removed
        /// </summary>
        /// <param name="indicesOfRowsToRemove"></param> 
        /// <returns></returns>
        public Matrix RemoveRows(int[] passedIndicesOfRowsToRemove)
        {
            Matrix returnMatrix = new Matrix(this);
            int[] indicesOfRowsToRemove = new int[passedIndicesOfRowsToRemove.Count()];
            passedIndicesOfRowsToRemove.CopyTo(indicesOfRowsToRemove, 0);

            foreach (int rowToRemove in indicesOfRowsToRemove)
            {
                returnMatrix = returnMatrix.RemoveRow(rowToRemove);

                //Now that the matrix has shrunk by 1 row, all of the indices at or after that row need to be decreased by 1
                int arrayIndex = 0;
                foreach (int index in indicesOfRowsToRemove)
                {
                    if (index >= rowToRemove)
                    {
                        indicesOfRowsToRemove[arrayIndex] = index - 1;
                    }
                    arrayIndex++;
                }
            }

            return returnMatrix;
        }

        /// <summary>
        /// Returns a new matrix with all of the specified columns removed
        /// </summary>
        /// <param name="indicesOfColumnsToRemove"></param>
        /// <returns></returns>
        public Matrix RemoveColumns(int[] passedIndicesOfColumnsToRemove)
        {
            Matrix returnMatrix = new Matrix(this);
            int[] indicesOfColumnsToRemove = new int[passedIndicesOfColumnsToRemove.Count()];
            passedIndicesOfColumnsToRemove.CopyTo(indicesOfColumnsToRemove, 0);

            foreach (int columnToRemove in indicesOfColumnsToRemove)
            {
                returnMatrix = returnMatrix.RemoveColumn(columnToRemove);

                //Now that the matrix has shrunk by 1 column, all of the indices at or after that column need to be decreased by 1
                int arrayIndex = 0;
                foreach (int index in indicesOfColumnsToRemove)
                {
                    if (index >= columnToRemove)
                    {
                        indicesOfColumnsToRemove[arrayIndex] = index - 1;
                    }
                    arrayIndex++;
                }
            }

            return returnMatrix;
        }

        /// <summary>
        /// Checks to see if this matrix and the passed matrix can be added together.  
        /// In order to be able to add two matrices, they must have the same dimensions.
        /// </summary>
        /// <param name="passedMatrix"></param>
        /// <returns></returns>
        public bool CanBeAddedTo(Matrix passedMatrix)
        {
            return this.NumberOfRows == passedMatrix.NumberOfRows && this.NumberOfColumns == passedMatrix.NumberOfColumns;
        }

        /// <summary>
        /// Returns the sum of this matrix and the passed matrix
        /// </summary>
        /// <param name="passedMatrix"></param>
        /// <returns></returns>
        public Matrix AddTo(Matrix passedMatrix)
        {
            Matrix resultingMatrix;

            if (this.CanBeAddedTo(passedMatrix))
            {
                resultingMatrix = new Matrix(this.NumberOfRows, this.NumberOfColumns);
            }
            else
            {
                throw new Exception("The two matrices cannot be added");
            }
            
            // Nested loops to travel through the matrices row by row
            for (int rowIndex = 0; rowIndex < resultingMatrix.NumberOfRows; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < resultingMatrix.NumberOfColumns; columnIndex++)
                {
                    double sum = (this.GetElement(rowIndex, columnIndex) + passedMatrix.GetElement(rowIndex, columnIndex));
                    resultingMatrix.SetElement(rowIndex, columnIndex, sum);
                }
            }

            return resultingMatrix;
        }

        /// <summary>
        /// Checks to see if this matrix can be multiplied by the passed matrix IN THAT ORDER.
        /// In order to multiply two matrices, the number of columns in the first matrix
        /// must equal the number of rows in the second matrix.
        /// </summary>
        /// <param name="passedMatrix"></param>
        /// <returns></returns>
        public bool CanBeMultipliedBy(Matrix passedMatrix)
        {
            if (this.NumberOfColumns == passedMatrix.NumberOfRows)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns the product matrix of this matrix multiplied by the passed matrix, in that order.
        /// </summary>
        /// <param name="passedMatrix"></param>
        /// <returns></returns>
        public Matrix MultiplyBy(Matrix passedMatrix)
        {
            Matrix resultingMatrix;

            if (this.CanBeMultipliedBy(passedMatrix))
            {
                resultingMatrix = new Matrix(this.NumberOfRows, passedMatrix.NumberOfColumns);
            }
            else
            {
                throw new Exception("The two Matrices cannot be multiplied");
            }

            for (int rowIndex = 0; rowIndex < resultingMatrix.NumberOfRows; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < resultingMatrix.NumberOfColumns; columnIndex++)
                {
                    double sum = 0;
                    for (int term = 0; term < this.NumberOfColumns; term++)
                    {
                        sum += (this.GetElement(rowIndex, term) * passedMatrix.GetElement(term, columnIndex));
                    }
                    resultingMatrix.SetElement(rowIndex, columnIndex, sum);
                }
            }

            return resultingMatrix;
        }

        /// <summary>
        /// Checks to see if this matrix is square
        /// </summary>
        /// <returns></returns>
        public bool IsSquare()
        {
            // A square matrix has the same number of rows and columns.
            if (this.NumberOfRows == this.NumberOfColumns)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns an Identity Matrix of the specified size. That is, a square matrix with 1's on the diagonal and 0's everywhere else.
        /// </summary>
        /// <param name="passedNumberOfRowsAndColumns"></param>
        /// <returns></returns>
        public static Matrix CreateIdentityMatrix(int passedNumberOfRowsAndColumns)
        {
            // returns an n x n Identity matrix
            Matrix result = new Matrix(passedNumberOfRowsAndColumns);
            for (int i = 0; i < passedNumberOfRowsAndColumns; i++)
                result.SetElement(i, i, 1.0);

            return result;
        }
        
        /// <summary>
        /// Computes the determinant of a matrix
        /// </summary>
        /// <returns></returns>
        public double Determinant()
        {
            // Check to make sure the matrix is square.  Only square matrices have determinants.
            if (!this.IsSquare())
            {
                throw new Exception("The matrix is not square; it does not have a determinant");
            }

            // Check to see if the matrix has dimensions 1x1.  The determinant of a 1x1 matrix is equal to the single value in the matrix.
            if(this.NumberOfRows == 1)
            {
                return this.GetElement(0, 0);
            }

            double determinant = 0;

            // Loop through the top row of the matrix.
            for (int columnIndex = 0; columnIndex < this.NumberOfColumns; columnIndex++)
            {
                //The determinant is the sum of all the elements of one row multiplied by their respective cofactors
                determinant += this.GetElement(0, columnIndex) * this.GetCofactor(0, columnIndex);
            }

            return determinant;
        }       

        /// <summary>
        /// Inserts the passed Matrix into this Matrix starting at the specified index
        /// </summary>
        /// <param name="passedMatrix"></param>
        /// <param name="destinationRow"></param>
        /// <param name="destinationColumn"></param>
        /// <returns></returns>
        public Matrix InsertMatrixAt(Matrix passedMatrix, int destinationRow, int destinationColumn)
        {
            for (int row = 0; row < passedMatrix.NumberOfRows; row++)
            {
                for (int col = 0; col < passedMatrix.NumberOfColumns; col++)
                {
                    this.SetElement(row + destinationRow, col + destinationColumn, passedMatrix.GetElement(row, col));
                }
            }
            return this;
        }

        /// <summary>
        /// Returns the cofactor for the specified element of a square matrix
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        public double GetCofactor(int rowIndex, int columnIndex)
        {
            //Remove the row and column that contain this element
            Matrix subMatrix = this.RemoveRow(rowIndex);
            subMatrix = subMatrix.RemoveColumn(columnIndex);

            //Find the determinant of the remaining matrix
            double determinantOfSubMatrix = subMatrix.Determinant();

            //Give the cofactor the correct sign (+/-) based on its position in the matrix
            int indexSum = rowIndex + columnIndex;
            double cofactor = determinantOfSubMatrix * Math.Pow(-1, indexSum);
            
            return cofactor;
        }

        /// <summary>
        /// Returns true if this matrix has an inverse, false otherwise
        /// </summary>
        /// <returns></returns>
        public bool IsInvertible()
        {
            // To have an inverse, a matrix must be square and have a nonzero determinant.
            if (this.IsSquare() && (this.Determinant() != 0))
            {
                return true;
            }
            
            return false;
        }

        /// <summary>
        /// Returns a matrix that is this matrix with the absolute value function applied to each element
        /// </summary>
        /// <returns></returns>
        public Matrix AbsoluteValue()
        {
            Matrix resultingMatrix = new Matrix(NumberOfRows, NumberOfColumns);
            
            for (int i = 0; i < NumberOfRows; i++)
            {
                for (int j = 0; j < NumberOfColumns; j++)
                {
                    double newElement = Math.Abs(GetElement(i, j));
                    resultingMatrix.SetElement(i, j, newElement);
                }
            }
            return resultingMatrix;
        }

        /// <summary>
        /// Returns the transpose of a given matrix.  The transpose is found by swapping the rows and columns (i.e. reflecting all the elements across the main diagonal)
        /// </summary>
        /// <returns></returns>
        public Matrix Transpose()
        {
            Matrix transpose = new Matrix(this.NumberOfColumns, this.NumberOfRows);
            double element;

            for (int rowIndex = 0; rowIndex < this.NumberOfColumns; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < this.NumberOfRows; columnIndex++)
                {
                    element = this.GetElement(columnIndex, rowIndex);
                    transpose.SetElement(rowIndex, columnIndex, element);
                }
            }
            return transpose;
        }

        /// <summary>
        /// Returns a matrix that can be multiplied by another matrix to represent a rotation of that matrix about the X axis by the specified angle
        /// </summary>
        /// <param name="rotationAngle"></param>
        /// <returns></returns>
        public static Matrix RotationMatrixAboutX(Angle rotationAngle)
        {
            Matrix rotationMatrix = new Matrix(3);

            double[] row1 = { 1, 0, 0 };
            double[] row2 = { 0, Math.Cos(rotationAngle.Radians), -Math.Sin(rotationAngle.Radians)};
            double[] row3 = { 0, Math.Sin(rotationAngle.Radians), Math.Cos(rotationAngle.Radians)};

            rotationMatrix.SetRow(0, row1);
            rotationMatrix.SetRow(1, row2);
            rotationMatrix.SetRow(2, row3);

            return rotationMatrix;
        }

        /// <summary>
        /// Returns a matrix that can be multiplied by another matrix to represent a rotation of that matrix about the Y axis by the specified angle
        /// </summary>
        /// <param name="rotationAngle"></param>
        /// <returns></returns>
        public static Matrix RotationMatrixAboutY(Angle rotationAngle)
        {
            Matrix rotationMatrix = new Matrix(3);

            double[] row1 = { Math.Cos(rotationAngle.Radians), 0, Math.Sin(rotationAngle.Radians)};
            double[] row2 = { 0, 1, 0};
            double[] row3 = { -Math.Sin(rotationAngle.Radians), 0, Math.Cos(rotationAngle.Radians)};

            rotationMatrix.SetRow(0, row1);
            rotationMatrix.SetRow(1, row2);
            rotationMatrix.SetRow(2, row3);

            return rotationMatrix;
        }

        /// <summary>
        /// Returns a matrix that can be multiplied by another matrix to represent a rotation of that matrix about the Z axis by the specified angle
        /// </summary>
        /// <param name="rotationAngle"></param>
        /// <returns></returns>
        public static Matrix RotationMatrixAboutZ(Angle rotationAngle)
        {
            Matrix rotationMatrix = new Matrix(3);

            double[] row1 = { Math.Cos(rotationAngle.Radians), -Math.Sin(rotationAngle.Radians), 0 };
            double[] row2 = { Math.Sin(rotationAngle.Radians), Math.Cos(rotationAngle.Radians), 0 };
            double[] row3 = { 0, 0, 1 };

            rotationMatrix.SetRow(0, row1);
            rotationMatrix.SetRow(1, row2);
            rotationMatrix.SetRow(2, row3);

            return rotationMatrix;
        }

        /// <summary>
        /// Returns a matrix that can be multiplied by another matrix to represent a rotation of that matrix about the passed axis line by the specified angle
        /// </summary>
        /// <param name="passedRotationAxisLine"></param>
        /// <param name="passedRotationAngle"></param>
        /// <returns></returns>
        public static Matrix RotationMatrixAboutAxis(Line passedRotationAxisLine, Angle passedRotationAngle)
        {
            Matrix rotationMatrix = new Matrix(3);
         
            Direction rotationUnitVector = passedRotationAxisLine.Direction;

            double unitX = rotationUnitVector.XComponentOfDirection; //Projection onto x-axis?
            double unitY = rotationUnitVector.YComponentOfDirection;
            double unitZ = rotationUnitVector.ZComponentOfDirection;
            double theta = passedRotationAngle.Radians;

            double row0column0 = Math.Cos(theta) + Math.Pow(unitX, 2) * (1 - Math.Cos(theta));
            double row0column1 = unitX * unitY * (1 - Math.Cos(theta)) - unitZ * Math.Sin(theta);
            double row0column2 = unitX * unitZ * (1 - Math.Cos(theta)) + unitY * Math.Sin(theta);
            double row1column0 = unitY * unitX * (1 - Math.Cos(theta)) + unitZ * Math.Sin(theta);
            double row1column1 = Math.Cos(theta) + Math.Pow(unitY, 2) * (1 - Math.Cos(theta));
            double row1column2 = unitY * unitZ * (1 - Math.Cos(theta)) - unitX * Math.Sin(theta);
            double row2column0 = unitZ * unitX * (1 - Math.Cos(theta)) - unitY * Math.Sin(theta);
            double row2column1 = unitZ * unitY * (1 - Math.Cos(theta)) + unitX * Math.Sin(theta);
            double row2column2 = Math.Cos(theta) + Math.Pow(unitZ, 2) * (1 - Math.Cos(theta));

            rotationMatrix.SetElement(0, 0, row0column0);
            rotationMatrix.SetElement(0, 1, row0column1);
            rotationMatrix.SetElement(0, 2, row0column2);
            rotationMatrix.SetElement(1, 0, row1column0);
            rotationMatrix.SetElement(1, 1, row1column1);
            rotationMatrix.SetElement(1, 2, row1column2);
            rotationMatrix.SetElement(2, 0, row2column0);
            rotationMatrix.SetElement(2, 1, row2column1);
            rotationMatrix.SetElement(2, 2, row2column2);

            return rotationMatrix;
        }

        /// <summary>
        /// Returns true if every element in this matrix is zero, false otherwise
        /// </summary>
        /// <param name="passedArray"></param>
        /// <returns></returns>
        public static bool IsAllZeros(double[] passedArray)
        {
            bool AreAllZeros = true;
            for (int i = 0; i < passedArray.Length; i++)
            {
                if(passedArray[i] != 0)
                {
                    AreAllZeros = false;
                }
            }
            return AreAllZeros;
        }

        /// <summary>
        /// Returns a matrix with the specified row removed
        /// </summary>
        /// <param name="rowIndexToDelete"></param>
        /// <returns></returns>
        public Matrix RemoveRow(int rowIndexToDelete)
        {
            Matrix returnMatrix = new Matrix(this.NumberOfRows - 1, this.NumberOfColumns);

            //We want to keep all of the rows except the one to be deleted
            int[] rowsToKeep = new int[this.NumberOfRows - 1];
            int indexOfRowArray = 0;

            for (int i = 0; i < this.NumberOfRows; i++)
            {
                if (i != rowIndexToDelete)
                {
                    rowsToKeep[indexOfRowArray] = i;
                    indexOfRowArray++;
                }
            }

            //We want to keep all of the columns
            int[] columnsToKeep = new int[this.NumberOfColumns];
            for (int i = 0; i < this.NumberOfColumns; i++)
            {
                columnsToKeep[i] = i;
            }

            //Return a submatrix of the original with only the rows and columns we want to KEEP
            return this.GetSubMatrix(rowsToKeep, columnsToKeep);
        }

        /// <summary>
        /// Returns a matrix with the specified column removed
        /// </summary>
        /// <param name="rowIndexToDelete"></param>
        /// <returns></returns>
        public Matrix RemoveColumn(int columnIndexToDelete)
        {
            Matrix returnMatrix = new Matrix(this.NumberOfRows, this.NumberOfColumns - 1);

            //We want to keep all of the columns except the one to be deleted
            int[] columnsToKeep = new int[this.NumberOfColumns - 1];
            
            int indexOfColumnArray = 0;
            for (int i = 0; i < this.NumberOfColumns; i++)
            {
                if (i != columnIndexToDelete)
                {
                    columnsToKeep[indexOfColumnArray] = i;
                    indexOfColumnArray++;
                }
            }

            //We want to keep all of the rows
            int[] rowsToKeep = new int[this.NumberOfRows];
            for (int i = 0; i < this.NumberOfRows; i++)
            {
                rowsToKeep[i] = i;
            }

            //Return a submatrix of the original with only the rows and columns we want to KEEP
            return this.GetSubMatrix(rowsToKeep, columnsToKeep);
        }

        #region Matrix Decomposition Stuff (Source: http://msdn.microsoft.com/en-us/magazine/jj863137.aspx)
        // --------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Performs Doolittle decomposition with partial pivoting and returns a combined LU matrix
        /// </summary>
        /// <param name="permutationArray">Keeps track of how the matrices have been rearranged during the calculation</param>
        /// <param name="toggle">is +1 or -1 (even or odd)</param>
        /// <returns></returns>
        public Matrix Decompose(out int[] permutationArray, out int toggle)
        {            
            if (!this.IsSquare())
            {
                throw new Exception("LU-Decomposition can only be found for a square matrix");
            }

            Matrix decomposedMatrix = new Matrix(this); // copy this matrix before messing with it

            permutationArray = new int[NumberOfRows]; // set up row permutation result
            for (int i = 0; i < NumberOfRows; ++i)
            {
                permutationArray[i] = i; 
            }
            
            toggle = 1; // toggle tracks row swaps. +1 -> even, -1 -> odd

            //Loop through the columns
            for (int columnIndex = 0; columnIndex < NumberOfRows - 1; columnIndex++) 
            {
                //Find largest value in the current column
                double maxOfColumn = Math.Abs(decomposedMatrix.GetElement(columnIndex, columnIndex)); 
                
                int pivotRowIndex = columnIndex;

                // loop through all of the rows in this column
                for (int rowIndex = columnIndex + 1; rowIndex < NumberOfRows; rowIndex++)
                {
                    if (Math.Abs(decomposedMatrix.GetElement(rowIndex,columnIndex)) > maxOfColumn)
                    {
                        maxOfColumn = Math.Abs(decomposedMatrix.GetElement(rowIndex,columnIndex));
                        pivotRowIndex = rowIndex;
                    }
                }

                if (pivotRowIndex != columnIndex) // if largest value not on pivot, swap rows
                {
                    double[] rowPtr = decomposedMatrix.GetRow(pivotRowIndex);
                    decomposedMatrix.SetRow(pivotRowIndex, decomposedMatrix.GetRow(columnIndex));
                    decomposedMatrix.SetRow(columnIndex, rowPtr);

                    int tmp = permutationArray[pivotRowIndex]; // and swap permutation info
                    permutationArray[pivotRowIndex] = permutationArray[columnIndex];
                    permutationArray[columnIndex] = tmp;

                    toggle = -toggle; // adjust the row-swap toggle
                }

                //Check to see if there is a zero on the diagonal. If so, try to get rid of it by swapping with a row that is beneath the row with a zero on the diagonal
                double elementOnDiagonal = decomposedMatrix.GetElement(columnIndex, columnIndex);

                if (Math.Round(elementOnDiagonal, 6) == 0.0)
                {
                    // find a good row to swap
                    int goodRow = -1;
                    for (int row = columnIndex + 1; row < decomposedMatrix.NumberOfRows; row++)
                    {
                        double element = decomposedMatrix.GetElement(row, columnIndex);
                        if (Math.Round(element, 6) != 0.0)
                        {
                            goodRow = row;
                        }
                    }

                    if (goodRow == -1)
                    {
                        throw new Exception("Cannot use Doolittle's method on this matrix");
                    }

                    // swap rows so 0.0 no longer on diagonal
                    double[] rowPtr = decomposedMatrix.GetRow(goodRow);
                    decomposedMatrix.SetRow(goodRow, decomposedMatrix.GetRow(columnIndex));
                    decomposedMatrix.SetRow(columnIndex, rowPtr);

                    int tmp = permutationArray[goodRow]; // and swap perm info
                    permutationArray[goodRow] = permutationArray[columnIndex];
                    permutationArray[columnIndex] = tmp;

                    toggle = -toggle; // adjust the row-swap toggle

                }

                //Find the next value to insert into the decomposed matrix    
                for (int rowIndex = columnIndex + 1; rowIndex < this.NumberOfRows; ++rowIndex)
                {
                    double valueToStore = decomposedMatrix.GetElement(rowIndex, columnIndex) / decomposedMatrix.GetElement(columnIndex, columnIndex);
                    decomposedMatrix.SetElement(rowIndex, columnIndex, valueToStore);
                    for (int nextColumnIndex = columnIndex + 1; nextColumnIndex < NumberOfRows; ++nextColumnIndex)
                    {
                        double valueToStore2 = decomposedMatrix.GetElement(rowIndex, nextColumnIndex) -
                                                decomposedMatrix.GetElement(rowIndex, columnIndex) * decomposedMatrix.GetElement(columnIndex, nextColumnIndex);
                        decomposedMatrix.SetElement(rowIndex, nextColumnIndex, valueToStore2);
                    }
                }
                
            } // main column loop

            return decomposedMatrix;
        } // MatrixDecompose

        // --------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Returns the inverse of this matrix
        /// </summary>
        /// <returns></returns>
        public Matrix Invert()
        {
            if (this.IsInvertible())
            {
                Matrix cofactorMatrix = GenerateCofactorMatrix();
                Matrix transposedCofactorMatrix = cofactorMatrix.Transpose();

                double determinant = this.Determinant();

                Matrix inverseOfMatrixA = (1 / determinant) * transposedCofactorMatrix;

                return inverseOfMatrixA;
            }
            
            else
            {
                throw new Exception("The matrix cannot be inverted");
            }
        }

        /// <summary>
        /// Creates the cofactor matrix corresponding to this matrix
        /// </summary>
        /// <returns></returns>
        public Matrix GenerateCofactorMatrix()
        {
            Matrix cofactorMatrix = new Matrix(NumberOfRows, NumberOfColumns);

            for (int rowIndex = 0; rowIndex < NumberOfRows; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < NumberOfColumns; columnIndex++)
                {
                    double cofactor = GetCofactor(rowIndex,columnIndex);
                    cofactorMatrix.SetElement(rowIndex, columnIndex, cofactor);
                }
            }

            return cofactorMatrix;
        }        
        
        // --------------------------------------------------------------------------------------------------------------
        
        /// <summary>
        /// Helper method for System Solve
        /// </summary>
        /// <param name="decomposedMatrix"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        internal double[] SolveHelper(Matrix decomposedMatrix, double[] b)
        {
            // before calling this helper, permute b using the perm array from MatrixDecompose that generated luMatrix
            int NumberOfColumns = decomposedMatrix.NumberOfColumns;
            double[] x = new double[NumberOfColumns];
            b.CopyTo(x, 0);

            for (int rowIndex = 1; rowIndex < NumberOfColumns; rowIndex++)
            {
                double sum = x[rowIndex];
                for (int columnIndex = 0; columnIndex < rowIndex; columnIndex++)
                    sum -= decomposedMatrix.GetElement(rowIndex, columnIndex) * x[columnIndex];
                x[rowIndex] = sum;
            }

            if (decomposedMatrix.GetElement(NumberOfColumns - 1, NumberOfColumns - 1) != 0)
            {
                x[NumberOfColumns - 1] /= decomposedMatrix.GetElement(NumberOfColumns - 1, NumberOfColumns - 1);
            }

            for (int rowIndex = NumberOfColumns - 2; rowIndex >= 0; rowIndex--)
            {
                double sum = x[rowIndex];
                for (int columnIndex = rowIndex + 1; columnIndex < NumberOfColumns; ++columnIndex)
                {
                    sum -= decomposedMatrix.GetElement(rowIndex, columnIndex) * x[columnIndex];
                }
                if (decomposedMatrix.GetElement(rowIndex, rowIndex) != 0)
                {
                    x[rowIndex] = sum / decomposedMatrix.GetElement(rowIndex, rowIndex);
                }
            }

            return x;
        }

        // --------------------------------------------------------------------------------------------------------------

        public double [] SystemSolve(double[] b)
        {
            Matrix matrixA = this;
            // Solve Ax = b

            while (!matrixA.IsSquare())
            {                
                if (this.NumberOfRows > this.NumberOfColumns)
                {
                    matrixA = new Matrix(this.NumberOfRows, this.NumberOfColumns + 1);
                    matrixA.InsertMatrixAt(this, 0, 0);
                    matrixA.SetColumn(this.NumberOfColumns - 1, matrixA.GetColumn(0));
                }
                else
                {
                    matrixA = new Matrix(this.NumberOfRows + 1, this.NumberOfColumns);
                    matrixA.InsertMatrixAt(this, 0, 0);
                    matrixA.SetRow(this.NumberOfRows - 1, matrixA.GetRow(0));
                }                
            }

            // 1. decompose A
            int[] permutationArray;
            int toggle;
            Matrix decomposedMatrix = matrixA.Decompose(out permutationArray, out toggle);

            // 2. permute b according to permutationArray[]
            double[] bPermuted = new double[b.Length];
            for (int i = 0; i < matrixA.NumberOfColumns; ++i)
            {
                bPermuted[i] = b[permutationArray[i]];
            }
            // 3. call helper
            double[] solution = SolveHelper(decomposedMatrix, bPermuted);

            //return solution (x) to Ax = b
            return solution;
        } // SystemSolve

        // --------------------------------------------------------------------------------------------------------------


        /// <summary>
        /// This method makes it possible to use SystemSolve on a matrix. It calls the other SystemSolve method and returns the result as a new Matrix.
        /// </summary>
        /// <param name="A"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public Matrix SystemSolve(Matrix passedSolutionMatrix)
        {
            Matrix solution = new Matrix(NumberOfRows, 1);
            if (passedSolutionMatrix.IsZeroMatrix())
            {
                solution.SetColumn(0, passedSolutionMatrix.GetColumn(0));
            }
            else
            {
                solution.SetColumn(0, SystemSolve(passedSolutionMatrix.GetColumn(0)));
            }

            return solution;

        }

        private bool IsZeroMatrix()
        {
            for (int i = 0; i < this.NumberOfRows; i++)
            {
                for (int j = 0; j < this.GetRow(i).Length; j++)
                {
                    if (this.GetElement(i, j) != 0)
                    {
                        return false;
                    }
                }

            }

            return true;
        }
        // --------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Returns Matrix with only the values in the lower triangle (below main diagonal) intact.  Puts 1.0's on diagonal and 0.0's in upper triangle
        /// </summary>
        /// <returns></returns>
        public Matrix ExtractLower()
        {
            Matrix LowerPart = new Matrix(NumberOfRows, NumberOfColumns);
            for (int i = 0; i < NumberOfRows; ++i)
            {
                for (int j = 0; j < NumberOfColumns; ++j)
                {
                    if (i == j)
                    {
                        //This element is on the main diagonal
                        LowerPart.SetElement(i, j, 1.0);
                    }
                    else if (i > j)
                    {
                        //This element is below the main diagonal
                        LowerPart.SetElement(i, j, this.GetElement(i, j));
                    }
                }
            }
            return LowerPart;
        }

        /// <summary>
        /// Returns Matrix with only the values in the main diagonal and upper triangle (above main diagonal) intact.  Has 0.0's in lower triangle
        /// </summary>
        /// <returns></returns>
        public Matrix ExtractUpper()
        {
            Matrix UpperPart = new Matrix(NumberOfRows, NumberOfColumns);

            for (int i = 0; i < NumberOfRows; ++i)
            {
                for (int j = 0; j < NumberOfColumns; ++j)
                {
                    if (i <= j)
                        UpperPart.SetElement(i, j, this.GetElement(i, j));
                }
            }
            return UpperPart;
        }

        #endregion

        #endregion
    }
}
