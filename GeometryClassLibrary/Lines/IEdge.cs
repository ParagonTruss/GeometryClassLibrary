namespace GeometryClassLibrary
{
    /// <summary>
    /// This class allows us to make irregular polygons and shapes with curves as well as line segments and access
    /// them in a generic way while still allowing for more specific implementation when needed
    /// </summary>
    public interface IEdge
    {
        #region Properties and Fields

        Direction InitialDirection { get; }
        Point BasePoint { get; }
        Point EndPoint { get; }

        bool IsClosed { get; }
        #endregion

        #region Methods
        IEdge Copy();
        IEdge Reverse();
        IEdge Translate(Point point);
        IEdge Rotate(Rotation rotation);
        IEdge Shift(Shift shift);
       

        #endregion

    }
}
