using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RingStiffness.Common.Interfaces
{
    public interface IServoMotor : IHardwareController
    {
        bool IsMotorStopped { get; set; }
        string MotorDirection { get; set; }
        void Up();
        void Down();
        void Stop();
    }
}
