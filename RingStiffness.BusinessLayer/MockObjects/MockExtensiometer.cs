using RingStiffness.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RingStiffness.BusinessLayer.MockObjects
{
    public class MockExtensiometer : MockEncoder , IExtensiometer
    {
        private const int NUM_OF_PULSE_PER_MILIMETER = 5;


        public double Displacement
        {
            get
            {
                return PulseRecieved / (double)NUM_OF_PULSE_PER_MILIMETER;
            }
        }  
    }
}
