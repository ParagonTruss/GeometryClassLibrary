using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeometryClassLibrary
{
    public class EllipticArc : Arc, IEdge
    {
        public Point Focus1 { get { return _focus1; } }
        public Point Focus2 { get { return _focus2; } }
        private Point _focus1;
        private Point _focus2;

        public Point BasePoint { get { return _basePoint; } }
        private Point _basePoint;

        public Point EndPoint { get { return _endPoint; } }
        private Point _endPoint;

        public bool IsClosed { get { return this.BasePoint == this.EndPoint; } }

        public Direction InitialDirection
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        public IEdge Copy()
        {
            throw new NotImplementedException();
        }

        public IEdge Reverse()
        {
            throw new NotImplementedException();
        }

        public IEdge Translate(Point point)
        {
            throw new NotImplementedException();
        }

        public IEdge Rotate(Rotation rotation)
        {
            throw new NotImplementedException();
        }

        public IEdge Shift(Shift shift)
        {
            throw new NotImplementedException();
        }
    }
}
