using RingStiffness.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RingStiffness.BusinessLayer.Entities
{
    public class LoadCell : HardwareController, ILoadCell
    {

        public LoadCell(IPLCInterface plcInterface) : base(plcInterface)
        {

        }
        public double Force
        {
            get
            {
                return Convert.ToDouble(PLCInterface.ReadDataRegister(IndustrialNetworks.FatekCom.MemoryType.M, 0, IndustrialNetworks.FatekCom.DataType.WORD));
            }
        }
    }
}
