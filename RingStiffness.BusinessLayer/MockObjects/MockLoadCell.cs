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
        private static int num = 0;
        private static int second = 0;

        public double Force
        {
            get
            {
                if(num >= 0 && num <= 5 && numOfRepeat > 2)
                {
                    numOfRepeat++;
                   
                }
                if (num >= 0 && num <= 5 && numOfRepeat == 2)
                {                  
                    num += 1;
                }
                if (num >= 6 && num <= 8 && numOfRepeat < 6)
                {
                    numOfRepeat++;
                }
                if (num >= 6 && num <= 9 && numOfRepeat == 6)
                {
                    num++;
                }
                if (num == 10 && numOfRepeat < 10)
                {
                    numOfRepeat++;
                }
                if (num >= 10 && numOfRepeat == 10)
                {
                    num++;
                    numOfRepeat++;
                }


                return num;
                //return 5;
                //return random.NextDouble() * 10;
            }
        }

        IPLCInterface IHardwareController.PLCInterface => throw new NotImplementedException();
    }
}
