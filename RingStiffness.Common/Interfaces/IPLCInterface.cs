using IndustrialNetworks.FatekCom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RingStiffness.Common.Interfaces
{
    public interface IPLCInterface
    {
        string COMNumber { get; }
        void WriteToDiscreteRegister(MemoryType memoryT, int registerNo, bool valueToWrite);
        string ReadDataRegister(MemoryType memoryT, int registerNo, DataType dataTypeT);
    }
}
