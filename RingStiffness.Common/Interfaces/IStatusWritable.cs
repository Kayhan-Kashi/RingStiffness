﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RingStiffness.Common.Interfaces
{
    public interface IStatusWritable
    {
        Action<string> WriteStatus { get; set; }
    }
}
