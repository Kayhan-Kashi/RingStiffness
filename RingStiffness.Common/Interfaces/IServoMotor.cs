using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RingStiffness.Common.Interfaces
{
    public interface IServoMotor : IHardwareController
    {
        void Up();
        void Down();
        void Stop();
    }
}
