﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeometryClassLibrary
{
    public interface IEdge
    {
        IEdge Shift(Shift passedShift);
    }
}
