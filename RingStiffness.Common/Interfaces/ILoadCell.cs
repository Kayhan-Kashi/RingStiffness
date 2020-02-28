using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RingStiffness.Common.Interfaces
{
    public interface ILoadCell : IHardwareController
    {
        double Force { get; }
    }
}
