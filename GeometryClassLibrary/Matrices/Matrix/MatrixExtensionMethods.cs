
using System;
//using MathNet.Numerics.LinearAlgebra.Double.Factorization;
using MathNet.Numerics.LinearAlgebra.Factorization;

namespace MathNet.Numerics.LinearAlgebra.Double
    {
        public static class ExtensionMethods
        {
            /// <summary> 
            /// Moore–Penrose pseudoinverse 
            /// If A = U • Σ • VT is the singular value decomposition of A, then A† = V • Σ† • UT. 
            /// For a diagonal matrix such as Σ, we get the pseudoinverse by taking the reciprocal of each non-zero element 
            /// on the diagonal, leaving the zeros in place, and transposing the resulting matrix. 
            /// In numerical computation, only elements larger than some small tolerance are taken to be nonzero,
            /// and the others are replaced by zeros. For example, in the MATLAB or NumPy function pinv, 
            /// the tolerance is taken to be t = ε • max(m,n) • max(Σ), where ε is the machine epsilon. (Wikipedia) 
            /// </summary> 
            /// <param name="M">The matrix to pseudoinverse</param> 
            /// <returns>The pseudoinverse of this Matrix</returns> 
            public static Matrix PseudoInverse(this Matrix M)
            {
                var D = M.Svd(true);
                
                Matrix W = (Matrix)D.W;
                Vector s = (Vector)D.S;

                // The first element of W has the maximum value. 
                double tolerance = Precision.EpsilonOf(2) * Math.Max(M.RowCount, M.ColumnCount) * W[0, 0];

                for (int i = 0; i < s.Count; i++)
                {
                    if (s[i] < tolerance)
                        s[i] = 0;
                    else
                        s[i] = 1 / s[i];
                }
                W.SetDiagonal(s);

                // (U * W * VT)T is equivalent with V * WT * UT 
                return (Matrix)(D.U * W * D.VT).Transpose();
            }
        }
    }