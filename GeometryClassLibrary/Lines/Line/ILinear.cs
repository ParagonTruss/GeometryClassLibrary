﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeometryClassLibrary.Lines.Line
{
    interface ILinear
    {
        Point BasePoint { get; }
        Direction Direction { get; }
    }
}