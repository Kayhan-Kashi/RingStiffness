using IndustrialNetworks.FatekCom;
using RingStiffness.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RingStiffness.BusinessLayer.Entities
{
    public class ServoMotor : HardwareController, IServoMotor
    {
        public bool IsMotorStopped { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string MotorDirection { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public ServoMotor(IPLCInterface plcInterface) : base(plcInterface)
        {

        }

        public void Up()
        {
            PLCInterface.WriteToDiscreteRegister(MemoryType.M, 0, true);
        }

        public void Down()
        {
            PLCInterface.WriteToDiscreteRegister(MemoryType.M, 1, true);
        }

        public void Stop()
        {
            PLCInterface.WriteToDiscreteRegister(MemoryType.M, 2, true);
        }
    }
}
