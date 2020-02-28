using RingStiffness.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RingStiffness.BusinessLayer.MockObjects
{
    public class MockLoadCell : ILoadCell
    {
        Random random = new Random();


        public double Force
        {
            get
            {
                return random.NextDouble() * 10;
            }
        }

        IPLCInterface IHardwareController.PLCInterface => throw new NotImplementedException();
    }
}
