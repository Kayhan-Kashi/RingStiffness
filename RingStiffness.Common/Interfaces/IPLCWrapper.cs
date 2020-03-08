using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RingStiffness.Common.Interfaces
{
    public interface IPLCWrapper
    {
        IServoMotor ServoMotor { get; }
        ILoadCell LoadCell { get; }
        IExtensiometer Extensiometer { get; }
        IPLCInterface PLCInterface { get; }
    }
}
