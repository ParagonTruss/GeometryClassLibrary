using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace GeometryClassLibrary
{
    [DebuggerDisplay("Size = {NumberOfRows} x {NumberOfColumns}")] 
    public class MatricesMatrix
    {
        #region Properties and Fields

        // declares a two Distanceal array named Matrices but does not initialize it yet.  The "," denotes that it is 2d
        Matrix[,] _matrices;

        /// <summary>
        /// Returns the element at row "rowIndex" and column "columnIndex"
        /// </summary>
        /// <param name="rowIndex">The row number of the desired element</param>
        /// <param name="columnIndex">The column number of the desired element</param>
        /// <returns></returns>
        public Matrix GetElement(int rowIndex, int columnIndex)
        {
            return _matrices[rowIndex, columnIndex];
        }

        // adds element at specified indices
        /// <summary>
        /// Adds an element to a specific lecation in the Matrices
        /// </summary>
        /// <param name="rowIndex">The number of the row where the element will be added</param>
        /// <param name="columnIndex">The number of the column where the element will be added</param>
        /// <param name="element"></param>
        public void SetElement(int rowIndex, int columnIndex, Matrix element)
        {
            _matrices[rowIndex, columnIndex] = element;
        }

        /// <summary>
        /// Returns the number of rows in the Matrix
        /// </summary>
        public int NumberOfRows
        {
            get
            {
                return _matrices.GetLength(0);
            }
        }

        /// <summary>
        /// Returns the number of columns in the Matrix
        /// </summary>
        public int NumberOfColumns
        {
            get
            {
                if (_matrices.Length != 0)
                {
                    return _matrices.GetLength(1);
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Returns the total number of rows required to contain all of the rows from the inner matrices
        /// </summary>
        /// <returns></returns>
        public int TotalRows()
        {
            int totalRows = 0;

            for (int rowIndex = 0; rowIndex < this.NumberOfRows; rowIndex++)
            {
                int maxRowHeight = 0;
                for (int columnIndex = 0; columnIndex < this.NumberOfColumns; columnIndex++)
                {
                    int rowHeight = this.GetRowHeight(rowIndex);
                    if (rowHeight > maxRowHeight)
                    {
                        maxRowHeight = rowHeight;
                    }
                }
                totalRows += maxRowHeight;
            }

            return totalRows;

        }

        /// <summary>
        /// Returns the total number of columns required to contain all of the columns from the inner matrices
        /// </summary>
        /// <returns></returns>
        public int TotalColumns()
        {
            int totalColumns = 0;
            for (int columnIndex = 0; columnIndex < this.NumberOfColumns; columnIndex++)
            {
                int maxColumnWidth = 0;
                for (int rowIndex = 0; rowIndex < this.NumberOfRows; rowIndex++)
                {
                    int columnWidth = this.GetColumnWidth(columnIndex);
                    if (columnWidth > maxColumnWidth)
                    {
                        maxColumnWidth = columnWidth;
                    }
                }
                totalColumns += maxColumnWidth;
            }

            return totalColumns;

        }

        #endregion

        #region Constructors

        /// <summary>
        ///Creates a square matrix with the specified number of rows and columns
        ///the constructor that is called when you say new MatricesMatrix(numberOfRows, numberOfColumns)
        /// </summary>
        public MatricesMatrix(int numRowsAndColumns)
        {
            _matrices = new Matrix[numRowsAndColumns, numRowsAndColumns];
        }

        /// <summary>
        ///the constructor that is called when you say new MatricesMatrix(numberOfRows, numberOfColumns)
        /// </summary>
        /// <param name="numRows">The desired number of rows in the new MatricesMatrix</param>
        /// <param name="numCols">The desired number of columns in the new MatricesMatrix</param>
        public MatricesMatrix(int numRows, int numCols) 
        {
            _matrices = new Matrix[numRows, numCols];
        }

        # endregion

        #region Overloaded Operators

        /// <summary>
        /// Returns true if each matricesMatrix is the same as the other
        /// </summary>
        public static bool operator ==(MatricesMatrix matricies1, MatricesMatrix matricies2)
        {
            if ((object)matricies2 == null)
            {
                if ((object)matricies2 == null)
                {
                    return true;
                }
                return false;
            }
            return matricies2.Equals(matricies2);
        }

        /// <summary>
        /// Returns true if each matricesMatrix is NOT the same as the other
        /// </summary>
        public static bool operator !=(MatricesMatrix matricies1, MatricesMatrix matricies2)
        {
            if ((object)matricies1 == null)
            {
                if ((object)matricies2 == null)
                {
                    return false;
                }
                return true;
            }
            return !matricies1.Equals(matricies2);
        }

        /// <summary>
        /// does the same thing as == if the passed-in object is an identical Matrices
        /// </summary>
        public override bool Equals(object obj)
        {
            //make sure the obj isnt null
            if (obj == null)
            {
                return false;
            }
            
            //try casting it and comparing it
            try
            {
                MatricesMatrix comparableMatricies = (MatricesMatrix)obj;

                if (this.NumberOfColumns != comparableMatricies.NumberOfColumns || this.NumberOfRows != comparableMatricies.NumberOfRows)
                {
                    return false;
                }

                for (int rowIndex = 0; rowIndex < this.NumberOfRows; rowIndex++)
                {
                    for (int columnIndex = 0; columnIndex < this.NumberOfColumns; columnIndex++)
                    {
                        if (this.GetElement(rowIndex, columnIndex) != comparableMatricies.GetElement(rowIndex, columnIndex))
                        {
                            return false;
                        }
                    }
                }

                //if we didnt find a spot were it was not ewual than return true;
                return true;
            }
            //if it was not a matix than they arent equal
            catch (InvalidCastException)
            {
                return false;
            }
        }

        #endregion

        #region Methods 
    
        /// <summary>
        /// Checks to see if this matrix and the passed matrix can be added together.  
        /// In order to be able to add two matrices, they must have the same Distances.
        /// </summary>
        /// <param name="passedMatrices"></param>
        /// <returns></returns>
        public bool CanBeAddedTo(MatricesMatrix passedMatrices)
        {
            throw new NotImplementedException();            
            //Must check that the Distances are same for the corresponding elements of the 2 MatricesMatrixes
                        
            if ((this.NumberOfRows == passedMatrices.NumberOfRows) && (this.NumberOfColumns == passedMatrices.NumberOfColumns))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns the sum of this matrix and the passed matrix
        /// </summary>
        /// <param name="passedMatrices"></param>
        /// <returns></returns>
        public MatricesMatrix AddTo(MatricesMatrix passedMatrices)
        {
            
            throw new NotImplementedException();

            MatricesMatrix resultingMatrices;

            if (this.CanBeAddedTo(passedMatrices))
            {
                resultingMatrices = new MatricesMatrix(this.NumberOfRows, this.NumberOfColumns);
            }
            else
            {
                throw new Exception("The two matrices cannot be added");
            }
            
            for (int rowIndex = 0; rowIndex < resultingMatrices.NumberOfRows; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < resultingMatrices.NumberOfColumns; columnIndex++)
                {
                    Matrix sum = (this.GetElement(rowIndex, columnIndex).AddTo(passedMatrices.GetElement(rowIndex, columnIndex)));
                    resultingMatrices.SetElement(rowIndex, columnIndex, sum);
                }
            }

            return resultingMatrices;
        }

        /// <summary>
        /// Checks to see if this matrix can be multiplied by the passed matrix IN THAT ORDER.
        /// In order to multiply two matrices, the number of columns in the first matrix
        /// must equal the number of columns in the second matrix.
        /// </summary>
        /// <param name="passedMatrices"></param>
        /// <returns></returns>
        public bool CanBeMultipliedBy(MatricesMatrix passedMatrices)
        {
            throw new NotImplementedException();
            //Must check that this.numberofcolumns = passedMatricesMatrix.NumberOfRows for every element of the MatricesMatrix
            

            if (this.NumberOfColumns == passedMatrices.NumberOfRows)
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
        /// <param name="passedMatrices"></param>
        /// <returns></returns>
        public MatricesMatrix MultiplyBy(MatricesMatrix passedMatrices)
        {
            throw new NotImplementedException();
            MatricesMatrix resultingMatrices;

            if (this.CanBeMultipliedBy(passedMatrices))
            {
                resultingMatrices = new MatricesMatrix(this.NumberOfRows, passedMatrices.NumberOfColumns);
            }
            else
            {
                throw new Exception("The two MatrixMatrices cannot be multiplied");
            }


            for (int rowIndex = 0; rowIndex < resultingMatrices.NumberOfRows; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < resultingMatrices.NumberOfColumns; columnIndex++)
                {
                    Matrix sum = null;
                    for (int term = 0; term < this.NumberOfColumns; term++)
                    {
                        sum = sum.AddTo((this.GetElement(rowIndex, term).MultiplyBy( passedMatrices.GetElement(term, columnIndex))));
                    }
                    resultingMatrices.SetElement(rowIndex, columnIndex, sum);
                }
            }

            return resultingMatrices;
        }

        /// <summary>
        /// Checks to see if the MatricesMatrix is square
        /// </summary>
        /// <returns></returns>
        private bool IsSquare()
        {
            // A square MatricesMatrix has the same number of rows and columns.
            if (this.NumberOfRows == this.NumberOfColumns)
            {
                return true;
            }

            return false;
        }      
        
        /// <summary>
        /// Returns the number of rows of the matrix with the largest number of rows in the specified row of the MatricesMatrix
        /// </summary>
        /// <param name="col"></param>
        /// <returns></returns>
        public int GetRowHeight(int row)
        {
            int maxSeen = 0;

            for (int col = 0; col < this.NumberOfColumns; col++)
            {
                if (this.GetElement(row, col).NumberOfRows > maxSeen)
                {
                    maxSeen = this.GetElement(row, col).NumberOfRows;
                }
            }

            return maxSeen;
        }

        /// <summary>
        /// Returns the number of columns of the matrix with the largest number of columns in the specified column of the MatricesMatrix
        /// </summary>
        /// <param name="col"></param>
        /// <returns></returns>
        public int GetColumnWidth(int col)
        {
            int maxSeen = 0;
            
            for (int row = 0; row < this.NumberOfRows; row++)
            {
                if (this.GetElement(row, col).NumberOfColumns > maxSeen)
                {
                    maxSeen = this.GetElement(row, col).NumberOfColumns;
                }
            }

            return maxSeen;
        }

        /// <summary>
        /// Strips out all the elements from each Matrix of a MatricesMatrix and inserts them into the approprate places of a new Matrix
        /// For a more detailed explanation, see http://stackoverflow.com/questions/24678305/converting-a-2d-array-of-2d-arrays-into-a-single-2d-array
        /// </summary>
        /// <returns></returns>
        public Matrix ConvertToMatrix()
        {
            //Creates a matrix big enough to hold all of the values in the MatricesMatrix
            Matrix returnMatrix = new Matrix(this.TotalRows(), this.TotalColumns());

            int nextRow = 0;
            int nextCol = 0;

            //Loop through the rows of this matrices matrix
            for (int row = 0; row < this.NumberOfRows; row++)
            {
                //Loop through the columns of this matrices matrix
                for (int col = 0; col < this.NumberOfColumns; col++)
                {
                    Matrix currentMatrix = this.GetElement(row, col);
                    returnMatrix.InsertMatrixAt(currentMatrix, nextRow, nextCol);
                    nextCol += this.GetColumnWidth(col); //The next column of the Matrix should be inserted a distance away that is equal to the total width of the current MatricesMatrix column
                }
                nextRow += this.GetRowHeight(row); //The next row of the Matrix should be inserted a distance away that is equal to the total height of the current MatricesMatrix row
                nextCol = 0;
            }

            return returnMatrix;
        }

        #endregion
    }
}
