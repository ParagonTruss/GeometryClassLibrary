using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeometryClassLibrary.ExtensionMethods.ListExtensionMethods
{
    public static class ShiftListExtensions
    {
        public static Shift ComposeLeftToRight(this List<Shift> listOfShifts)
        {
            return listOfShifts.Aggregate((s, t) => t.Compose(s));
        }

        public static Shift ComposeRightToLeft(this List<Shift> listOfShifts)
        {
            listOfShifts = listOfShifts.ToList();
            listOfShifts.Reverse();
            return listOfShifts.Aggregate((s, t) => t.Compose(s));
        }
    }
}
