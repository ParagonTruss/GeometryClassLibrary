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

        /// <summary>
        /// Creates a new zero translation
        /// </summary>
        public Translation()
            : base() { }

        /// <summary>
        /// Creates a Translation with offsets in each direction represented by the point
        /// </summary>
        /// <param name="translation">The distance in each direction that this translation represents</param>
        public Translation(Point translation)
            : base(translation) { }

        /// <summary>
        /// Creates a translation with the given translation Dimesnions in each direction
        /// </summary>
        /// <param name="xTranslation">The distance to translate in the X direction</param>
        /// <param name="yTranslation">The distance to translate in the Y direction</param>
        /// <param name="zTranslation">The distance to translate in the Z direction</param>
        public Translation(Dimension xTranslation, Dimension yTranslation, Dimension zTranslation)
            : base(xTranslation, yTranslation, zTranslation) { }

        /// <summary>
        /// Creates a Translation in the given units with the translation distance with each of the given direction
        /// </summary>
        /// <param name="passedType">The dimesnions to use for this translation object</param>
        /// <param name="xTranslation">The unitless distance to translate in the X direction</param>
        /// <param name="yTranslation">The unitless distance to translate in the Y direction</param>
        /// <param name="zTranslation">The unitless distance to translate in the Z direction</param>
        public Translation(DimensionType passedType, double xTranslation, double yTranslation, double zTranslation)
            : base(passedType, xTranslation, yTranslation, zTranslation) { }

        /// <summary>
        /// Creates a copy of the given translation
        /// </summary>
        /// <param name="toCopy">The translation to copy</param>
        public Translation(Translation toCopy)
            : base(toCopy) { }

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
