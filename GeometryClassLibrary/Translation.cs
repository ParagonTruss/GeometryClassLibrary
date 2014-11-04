using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnitClassLibrary;

namespace GeometryClassLibrary
{
    public class Translation : Point
    {
        #region Constructors

        public Translation()
            : base() { }

        public Translation(Point translation)
            : base(translation) { }

        public Translation(Dimension xTranslation, Dimension yTranslation, Dimension zTranslation)
            : base(xTranslation, yTranslation, zTranslation) { }

        public Translation(DimensionType passedType, double xTranslation, double yTranslation, double zTranslation)
            : base(passedType, xTranslation, yTranslation, zTranslation) { }

        #endregion

        #region Overloaded Operators

        /// <summary>
        /// Not a perfect equality operator, is only accurate up to the Dimension Class's accuracy
        /// </summary>
        public static bool operator ==(Translation translation1, Translation translation2)
        {
            if ((object)translation1 == null)
            {
                if ((object)translation2 == null)
                {
                    return true;
                }
                return false;
            }
            return translation1.Equals(translation2);
        }

        /// <summary>
        /// Not a perfect equality operator, is only accurate up to the Dimension Class's accuracy
        /// </summary>
        public static bool operator !=(Translation translation1, Translation translation2)
        {
            if ((object)translation1 == null)
            {
                if ((object)translation2 == null)
                {
                    return false;
                }
                return true;
            }
            return !translation1.Equals(translation2);
        }

        /// <summary>
        /// does the same thing as ==
        /// </summary>
        public override bool Equals(object obj)
        {
            //make sure we didnt get a null
            if (obj == null)
            {
                return false;
            }

            //try to cast the object to a translation, if it fails then we know the user passed in the wrong type of object
            try
            {
                Translation comparableTranslation = (Translation)obj;

                //now just check if they are equavalent as points
                return ((Point)this).Equals((Point)comparableTranslation);
            }
            //if we didnt get a translation than its not equal
            catch
            {
                return false;
            }
        }

        #endregion
    }
}
