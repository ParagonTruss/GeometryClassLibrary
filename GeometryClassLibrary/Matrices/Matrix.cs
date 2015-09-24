using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using UnitClassLibrary;
using Newtonsoft.Json;

namespace GeometryClassLibrary
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Matrix
    {
        #region Properties and Fields

        [JsonProperty]
        private Matrix<double> _matrix;


        /// <summary>
        /// Returns the number of rows in the matrix
        /// </summary>
        public int NumberOfRows
        {
            get
            {
                return _matrix.RowCount;
            }
        }

        /// <summary>
        /// Returns the number of columns in the matrix
        /// </summary>
        public int NumberOfColumns
        {
            get
            {
                return _matrix.ColumnCount;
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
            _matrix = DenseMatrix.OfArray(new double[numRowsAndColumns, numRowsAndColumns]);
        }


        /// <summary>
        /// Creates a matrix of the desired number of rows and columns, with all entries initialized to zero.
        /// </summary>
        public Matrix(int numRows, int numCols)
        {
            _matrix = DenseMatrix.OfArray(new double[numRows, numCols]);
        }

        /// <summary>
        /// Makes a copy of the passed matrix
        /// </summary>
        /// <param name="passedMatrix"></param>
        [JsonConstructor]
        public Matrix(Matrix<double> passedMatrix)
        {
            double norm =  passedMatrix.InfinityNorm();
            if (Double.IsNaN(norm))
            {
                throw new Exception("Matrix entries should all be numbers!");
            }
            _matrix = passedMatrix.Clone();
        }

        public Matrix(double[] array)
        {
            this._matrix = DenseMatrix.OfArray(new double[array.Length, 1]);

            this.SetColumn(0, array);
        }

        /// <summary> Constructs Matrix object from a 2dArray </summary>
        public Matrix(double[,] passed2DArray)
        {
            this._matrix = DenseMatrix.OfArray(passed2DArray);
        }

        /// <summary>
        /// Makes a copy of the passed matrix
        /// </summary>
        /// <param name="passedMatrix"></param>
        public Matrix(Matrix passedMatrix)
        {
            _matrix = passedMatrix._matrix.Clone();
        }
        #endregion

        #region Overloaded Operators
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

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
            //return matrix1._matrix.Equals(matrix2._matrix);
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
            if (obj == null)
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
        public static Matrix operator +(Matrix m1, Matrix m2)
        {
            return new Matrix(m1._matrix.Add(m2._matrix));
        }

        /// <summary>
        /// Returns the difference of 2 matrices of equal size
        /// </summary>
        public static Matrix operator -(Matrix m1, Matrix m2)
        {
            return new Matrix(m1._matrix.Subtract(m2._matrix));
        }

        /// <summary>
        /// Returns the product of the 2 matrices if they can be multiplied
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="m2"></param>
        /// <returns></returns>
        public static Matrix operator *(Matrix m1, Matrix m2)
        {
            var matrix = new Matrix(m1._matrix.Multiply(m2._matrix));
            return matrix;
        }

        /// <summary>
        /// Returns a new matrix with each element multiplied by the passed multiplier
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="scalarMultiplier"></param>
        /// <returns></returns>
        public static Matrix operator *(Matrix m1, double scalarMultiplier)
        {
            return new Matrix(m1._matrix.Multiply(scalarMultiplier));
        }

        /// <summary>
        /// Returns a new matrix with each element multiplied by the passed multiplier
        /// </summary>
        /// <param name="scalarMultiplier"></param>
        /// <param name="m1"></param>
        /// <returns></returns>
        public static Matrix operator *(double scalarMultiplier, Matrix m1)
        {
            return new Matrix(m1._matrix.Multiply(scalarMultiplier));
        }

        public override string ToString()
        {
            return _matrix.ToString();
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
            if (Double.IsNaN(element))
            {
                throw new Exception("Matrix entry should be a number!");
            }
            _matrix.At(rowIndex, columnIndex, element);
        }

        /// <summary>
        /// Returns the element at row "rowIndex" and column "columnIndex"
        /// </summary>
        /// <param name="rowIndex">The row number of the desired element</param>
        /// <param name="columnIndex">The column number of the desired element</param>
        /// <returns></returns>
        public double GetElement(int rowIndex, int columnIndex)
        {
            return _matrix.At(rowIndex, columnIndex);
        }


        /// <summary>
        /// Returns the specified row of the matrix
        /// </summary>
        /// <param name="passedrowIndex"></param>
        /// <returns></returns>
        public double[] GetRow(int passedrowIndex)
        {
            Vector <double> row = _matrix.Row(passedrowIndex);
            return row.ToArray();
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
            Vector<double> column = _matrix.Column(passedColumnIndex);
            return column.ToArray();
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
        /// Returns the matrix as a double[,]
        /// </summary>
        public double[,] As2DArray()
        {
            return _matrix.ToArray();
        }

        /// <summary>
        /// Returns the euler angles (for rotations in x, y,z order) assuming this matrix is a pure rotation matrix
        /// </summary>
        /// <returns>Returns a list of the euler angles in this order: x, y, z</returns>
        public List<Angle> GetAnglesOutOfRotationMatrixForXYZRotationOrder()
        {
            //Try getting the angles out of the matrix (based of of the following question but modified for x,y,z rotation order)
            //http://stackoverflow.com/questions/1996957/conversion-euler-to-matrix-and-matrix-to-euler
            //Note the Matrix is formed by multiplying together in the z,y,x order and so we need to multipy the matricies in that order as well
            //but this gets the angle for the x,y,z order shift. This is due to how matricies are multiplied together and is difficult to grasp conceptually
            List<Angle> extractedAngles = new List<Angle>();
            extractedAngles.Add(new Angle(AngleType.Radian, Math.Atan2(this.GetElement(2, 1), this.GetElement(2, 2)))); //x
            extractedAngles.Add(new Angle(AngleType.Radian, Math.Asin(-this.GetElement(2, 0)))); //y
            extractedAngles.Add(new Angle(AngleType.Radian, Math.Atan2(this.GetElement(1, 0), this.GetElement(0, 0)))); //z


            return extractedAngles;
        }

        /// <summary>
        /// Converts the given rotation Matrix into a Quaternion, which also represents orientation but in a more mathematically unique way
        /// since only two quaternions represent the same orientation, and they have the following relation to each other: q = -q
        /// </summary>
        /// <returns>Returns the Quarternion representation of this Rotation Matrix, stored in a 4x1 Matrix</returns>
        public Matrix ConvertRotationMatrixToQuaternion()
        {

            //Follows this question asked by user1283674 on stack overflow
            //http://stackoverflow.com/questions/21455139/matrix-rotation-to-quaternion

            // Output quaternion
            double w,x,y,z;

            // Determine which of w,x,y, or z has the largest absolute value
            double fourWSquaredMinus1 = this.GetElement(0, 0) + this.GetElement(1, 1) + this.GetElement(2, 2);
            double fourXSquaredMinus1 = this.GetElement(0, 0) - this.GetElement(1, 1) - this.GetElement(2, 2);
            double fourYSquaredMinus1 = this.GetElement(1, 1) - this.GetElement(0, 0) - this.GetElement(2, 2);
            double fourZSquaredMinus1 = this.GetElement(2, 2) - this.GetElement(0, 0) - this.GetElement(1, 1);

            int biggestIndex = 0;
            double fourBiggestSquaredMinus1 = fourWSquaredMinus1;

            if(fourXSquaredMinus1 > fourBiggestSquaredMinus1) {
                fourBiggestSquaredMinus1 = fourXSquaredMinus1;
                biggestIndex = 1;
            }
            if (fourYSquaredMinus1 > fourBiggestSquaredMinus1) {
                fourBiggestSquaredMinus1 = fourYSquaredMinus1;
                biggestIndex = 2;
            }
            if (fourZSquaredMinus1 > fourBiggestSquaredMinus1) {
                fourBiggestSquaredMinus1 = fourZSquaredMinus1;
                biggestIndex = 3;
            }

            // Perform square root and division
            //(I have also seen it this way: mult = 0.25 * Math.Sqrt (fourBiggestSquaredMinus1 + 1.0 ) * 2
            //  and then you divide by mult below instead of * i.e  x = (this.GetElement(1, 2) - this.GetElement(2, 1)) / mult;
            //  and both ways seem to work the same)
            double biggestVal = Math.Sqrt (fourBiggestSquaredMinus1 + 1.0 ) * 0.5;
            double mult = 0.25 / biggestVal;

            // Apply table to compute quaternion values
            switch (biggestIndex) 
            {
                case 0:
                    w = biggestVal;
                    x = (this.GetElement(1, 2) - this.GetElement(2, 1)) * mult;
                    y = (this.GetElement(2, 0) - this.GetElement(0, 2)) * mult;
                    z = (this.GetElement(0, 1) - this.GetElement(1, 0)) * mult;
                    break;
                case 1:
                    x = biggestVal;
                    w = (this.GetElement(1, 2) - this.GetElement(2, 1)) * mult;
                    y = (this.GetElement(0, 1) + this.GetElement(1, 0)) * mult;
                    z = (this.GetElement(2, 0) + this.GetElement(0, 2)) * mult;
                    break;
                case 2:
                    y = biggestVal;
                    w = (this.GetElement(2, 0) - this.GetElement(0, 2)) * mult;
                    x = (this.GetElement(0, 1) + this.GetElement(1, 0)) * mult;
                    z = (this.GetElement(1, 2) + this.GetElement(2, 1)) * mult;
                    break;
                case 3:
                    z = biggestVal;
                    w = (this.GetElement(0, 1) - this.GetElement(1, 0)) * mult;
                    x = (this.GetElement(2, 0) + this.GetElement(0, 2)) * mult;
                    y = (this.GetElement(1, 2) + this.GetElement(2, 1)) * mult;
                    break;
                default:
                    throw new Exception("Error creating quaternion");
            }

            var returnMatrix = new Matrix(4, 1);
            returnMatrix.SetElement(0, 0, w);
            returnMatrix.SetElement(1, 0, x);
            returnMatrix.SetElement(2, 0, y);
            returnMatrix.SetElement(3, 0, z);
            return returnMatrix;

            //return new double[] { w, x, y, z };
        }

        public static Point ShiftPoint(Point point, Matrix matrix)
        {
            var col =  PointAsProjectiveColumnVector(point);
            var result = matrix * col;
            return _pointFromProjectiveColumnVector(result);
        }

        public static Matrix PointAsProjectiveColumnVector(Point point)
        {
            return new Matrix(new double[]
            { point.X.Inches, point.Y.Inches, point.Z.Inches, 1 });
        }
        private static Point _pointFromProjectiveColumnVector(Matrix projectiveVector)
        {
            var col = projectiveVector.GetColumn(0);
            //smallest we're allowing is 1 millionth
            if (Math.Abs(col[3]) < 0.000001)
            {
                return Point.Origin;
            }
            else
            {
                var x = col[0] / col[3];
                var y = col[1] / col[3];
                var z = col[2] / col[3];
                return Point.MakePointWithInches(x, y, z);
            }
        }
        #endregion

        public static Matrix Identity(int dimension)
        {
            return new Matrix(DenseMatrix.CreateIdentity(dimension));
        }

        /// <summary>
        /// Returns a matrix with the specified row and column removed
        /// </summary>
        public Matrix GetSubMatrix(int indexOfRowToLeaveOut, int indexOfColumnToLeaveOut)
        {
            Matrix subMatrix = new Matrix(this.NumberOfRows - 1, this.NumberOfColumns - 1);

            subMatrix = this.RemoveRow(indexOfRowToLeaveOut);
            subMatrix = subMatrix.RemoveColumn(indexOfColumnToLeaveOut);
            return subMatrix;
            //return new Matrix(_matrix.SubMatrix(indexOfColumnToLeaveOut, this.NumberOfColumns - 1, indexOfColumnToLeaveOut, this.NumberOfRows - 1));
        }

        /// <summary>
        /// Takes values from a matrix and determines a cofactor by removing a certain row and column from the matrix.
        /// Logic developed with help from Andrew Morton on stackoverflow:
        /// http://stackoverflow.com/questions/24416946/next-step-in-calculating-a-matrix-determinant
        /// </summary>
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
        /// In order to be able to add two matrices, they must have the same Distances.
        /// </summary>
        public bool CanBeAddedTo(Matrix passedMatrix)
        {
            return this.NumberOfRows == passedMatrix.NumberOfRows && this.NumberOfColumns == passedMatrix.NumberOfColumns;
        }

        /// <summary>
        /// Returns the sum of this matrix and the passed matrix
        /// </summary>
        public Matrix AddTo(Matrix passedMatrix)
        {
            return passedMatrix + new Matrix(_matrix);
        }

        /// <summary>
        /// Checks to see if this matrix can be multiplied by the passed matrix IN THAT ORDER.
        /// In order to multiply two matrices, the number of columns in the first matrix
        /// must equal the number of rows in the second matrix.
        /// </summary>
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
        public Matrix MultiplyBy(Matrix passedMatrix)
        {
            return new Matrix(_matrix) * passedMatrix;
        }

        /// <summary>
        /// Checks to see if this matrix is square
        /// </summary>
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
        public static Matrix IdentityMatrix(int passedNumberOfRowsAndColumns)
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
        public double Determinant()
        {
            return _matrix.Determinant();
        }

        /// <summary>
        /// Inserts the passed Matrix into this Matrix starting at the specified index
        /// </summary>
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
           return new Matrix(_matrix.Transpose());
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
            double[] row2 = { 0, Math.Cos(rotationAngle.Radians), -Math.Sin(rotationAngle.Radians) };
            double[] row3 = { 0, Math.Sin(rotationAngle.Radians), Math.Cos(rotationAngle.Radians) };

            rotationMatrix.SetRow(0, row1);
            rotationMatrix.SetRow(1, row2);
            rotationMatrix.SetRow(2, row3);

            return rotationMatrix;
        }

        /// <summary>
        /// Returns a matrix that can be multiplied by another matrix to represent a rotation of that matrix about the Y axis by the specified angle
        /// </summary>
        public static Matrix RotationMatrixAboutY(Angle rotationAngle)
        {
            Matrix rotationMatrix = new Matrix(3);

            double[] row1 = { Math.Cos(rotationAngle.Radians), 0, Math.Sin(rotationAngle.Radians) };
            double[] row2 = { 0, 1, 0 };
            double[] row3 = { -Math.Sin(rotationAngle.Radians), 0, Math.Cos(rotationAngle.Radians) };

            rotationMatrix.SetRow(0, row1);
            rotationMatrix.SetRow(1, row2);
            rotationMatrix.SetRow(2, row3);

            return rotationMatrix;
        }

        /// <summary>
        /// Returns a matrix that can be multiplied by another matrix to represent a rotation of that matrix about the Z axis by the specified angle
        /// </summary>
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
        /// </summary>>
        public static Matrix RotationMatrixAboutOrigin(Rotation passedRotation)
        {
            Matrix rotationMatrix = new Matrix(3);

            Direction rotationUnitVector = passedRotation.AxisOfRotation.Direction;

            double unitX = rotationUnitVector.XComponent; //Projection onto x-axis
            double unitY = rotationUnitVector.YComponent;
            double unitZ = rotationUnitVector.ZComponent;
            double theta = passedRotation.RotationAngle.Radians;

            double sinTheta = Math.Sin(theta);
            double cosTheta = Math.Cos(theta);

            double row0column0 = Math.Cos(theta) + unitX*unitX * (1 - Math.Cos(theta));
            double row0column1 = unitX * unitY * (1 - Math.Cos(theta)) - unitZ * Math.Sin(theta);
            double row0column2 = unitX * unitZ * (1 - Math.Cos(theta)) + unitY * Math.Sin(theta);
            double row1column0 = unitY * unitX * (1 - Math.Cos(theta)) + unitZ * Math.Sin(theta);
            double row1column1 = Math.Cos(theta) + unitY*unitY * (1 - Math.Cos(theta));
            double row1column2 = unitY * unitZ * (1 - Math.Cos(theta)) - unitX * Math.Sin(theta);
            double row2column0 = unitZ * unitX * (1 - Math.Cos(theta)) - unitY * Math.Sin(theta);
            double row2column1 = unitZ * unitY * (1 - Math.Cos(theta)) + unitX * Math.Sin(theta);
            double row2column2 = Math.Cos(theta) + unitZ*unitZ * (1 - Math.Cos(theta));

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
        public bool IsAllZeros()
        {
            foreach (double d in this._matrix.Enumerate())
            {
                if (d != 0)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Returns a matrix with the specified row removed
        /// </summary>
        public Matrix RemoveRow(int rowIndexToDelete)
        {
            return new Matrix(_matrix.RemoveRow(rowIndexToDelete));
        }

        /// <summary>
        /// Returns a matrix with the specified column removed
        /// </summary>
        public Matrix RemoveColumn(int columnIndexToDelete)
        {
            return new Matrix(_matrix.RemoveColumn(columnIndexToDelete));
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

            Matrix decomposedMatrix = new Matrix(_matrix); // copy this matrix before messing with it

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
                    if (Math.Abs(decomposedMatrix.GetElement(rowIndex, columnIndex)) > maxOfColumn)
                    {
                        maxOfColumn = Math.Abs(decomposedMatrix.GetElement(rowIndex, columnIndex));
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
            return new Matrix(_matrix.Inverse());
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
                    double cofactor = GetCofactor(rowIndex, columnIndex);
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

        public double[] SystemSolve(double[] b)
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
