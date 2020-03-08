using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RingStiffness.BusinessLayer.Entities
{
    public class TestData
    {
        public Guid Id { get; set; }
        public double Force { get; set; }
        public int Second { get; set; }
        public double Deflection { get; set; }
    }
}
