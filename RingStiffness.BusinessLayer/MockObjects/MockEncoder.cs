using RingStiffness.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RingStiffness.BusinessLayer.MockObjects
{
    public class MockEncoder : IEncoder
    {
        private int pulse;
        private static int second = 0;

        public int PulseRecieved
        {
            get
            {
                return second++ * 5;             
            }
            set
            {
                pulse = value;
            }
        }
    }
}
