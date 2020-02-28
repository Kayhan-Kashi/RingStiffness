using RingStiffness.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RingStiffness.BusinessLayer.Entities
{
    public class HardwareController : IHardwareController
    {
        private IPLCInterface _plcInterface;
        public HardwareController(IPLCInterface plcInterface)
        {
            _plcInterface = plcInterface;
        }

        public IPLCInterface PLCInterface
        {
            get
            {
                return _plcInterface;
            }
        }
    }
}
