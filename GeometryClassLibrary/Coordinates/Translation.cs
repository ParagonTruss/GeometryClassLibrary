using UnitClassLibrary;

namespace GeometryClassLibrary
{
    public class Translation : Point
    {
        #region Constructors

        /// <summary>
        ///Null Constructor
        /// </summary>
        protected Translation() { }

        /// <summary>
        /// Creates a Translation with offsets in each direction represented by the point
        /// </summary>
        /// <param name="translation">The distance in each direction that this translation represents</param>
        public Translation(Point translation)
            : base(translation) { }

        /// <summary>
        /// Creates a Translation in the direction of the given vector.
        /// </summary>
        /// <param name="vector"></param>
        public Translation(Vector vector) : base(vector.XComponent, vector.YComponent, vector.ZComponent) { }

        /// <summary>
        /// Creates a translation with the given translation Dimesnions in each direction
        /// </summary>
        /// <param name="xTranslation">The distance to translate in the X direction</param>
        /// <param name="yTranslation">The distance to translate in the Y direction</param>
        /// <param name="zTranslation">The distance to translate in the Z direction</param>
        public Translation(Distance xTranslation, Distance yTranslation, Distance zTranslation)
            : base(xTranslation, yTranslation, zTranslation) { }

        /// <summary>
        /// Creates a copy of the given translation
        /// </summary>
        /// <param name="toCopy">The translation to copy</param>
        public Translation(Translation toCopy)
            : base(toCopy) { }

        #endregion

        #region Overloaded Operators

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Not a perfect equality operator, is only accurate up to the Distance Class's accuracy
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
        /// Not a perfect equality operator, is only accurate up to the Distance Class's accuracy
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
