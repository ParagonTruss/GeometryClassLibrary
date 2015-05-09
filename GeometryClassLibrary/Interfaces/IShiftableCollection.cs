namespace GeometryClassLibrary
{
    public interface IShiftableCollection: IShiftable
    {
        //Note: Coordinate systems are implemented in IShiftable an this inherits them from it.
        //      However, current coordinate system can get confusing for collections since you can shift each block's current system separately, 
        //      but the current system is used when you want to position the whole set and it will move all its lego blocks into the set's current 
        //      coordinate system


        /// <summary>
        /// Shifts this object's geometry using the given shift. 
        /// Note: This should not be used outside of Shift
        /// because if we do this outside, we will lose our home coordinates
        /// </summary>
        /// <param name="shiftToApply">The shift to apply to this object's geometry</param>
        /// <param name="systemShiftIsBasedOn">The coordinate system the shift is based on and to shift in</param>
        void ShiftGeometry(Shift shiftToApply, CoordinateSystem systemShiftIsBasedOn);

        /// <summary>
        /// Shifts this object's geometry to the given coordinate system. This is distinct form the other
        /// so that it handles composite objects correctly where the objects geometry consists of other
        /// IShiftable objects. If the object is based only on a geometry object that is not IShiftable
        /// then this should be equivalent to ShiftGeometry
        /// Note: This should not be used outside of Shift
        /// because if we do this outside, we will lose our home coordinates
        /// </summary>
        /// <param name="toShiftTo"></param>
        void ShiftGeometryToCoordinateSystem(CoordinateSystem toShiftTo);
    }
}
