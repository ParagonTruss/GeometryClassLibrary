using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnitClassLibrary.AngleUnit;

namespace GeometryClassLibrary
{
    public interface IRotate<T>
    {
        T Rotate(Rotation rotation);
    }

    public static class IRotateExtensions
    {
        public static T Rotate<T>(this IRotate<T> objectToRotate, Angle angleOfRotation, Line axisOfRotation = null)
        {
            return objectToRotate.Rotate(new Rotation(angleOfRotation, axisOfRotation));
        }

        public static T Rotate<T>(this IRotate<T> objectToRotate, Line axisOfRotation, Angle angleOfRotation)
        {
            return objectToRotate.Rotate(new Rotation(angleOfRotation, axisOfRotation));
        }
    }

    public interface IShift<T> : IRotate<T>
    {
        T Shift(Shift shift);
    }

    public static class IShiftExtensions
    {
        public static T Translate<T>(this IShift<T> objectToTranslate, Point point)
        {
            return objectToTranslate.Shift(new Shift(point));
        }
    }
}
