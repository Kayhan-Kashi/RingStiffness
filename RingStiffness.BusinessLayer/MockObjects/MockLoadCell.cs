using RingStiffness.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace RingStiffness.BusinessLayer.MockObjects
{
    public class MockLoadCell : ILoadCell
    {
        Random random = new Random();
        private static int num = 0;
        private static int second = 0;

        public MockLoadCell(Action<string> action)
        {
            Action = action;
        }

        public double Force
        {
            get
            {
                Debug.WriteLine("loadcell time is : " + second);
                //Action("this is force");

                if (second == 1)
                {
                    second++;
                    return 1;
                }
                if (second == 2)
                {
                    second++;
                    return 2;
                }
                if (second == 3)
                {
                    second++;
                    return 3;
                }
                if (second == 4)
                {
                    second++;
                    return 4;
                }
                if (second == 5)
                {
                    second++;
                    return 5;
                }
                if (second == 6)
                {
                    second++;
                    return 5;
                }
                if (second == 7 )
                {
                    second++;
                    return 4;
                }
                if (second == 8)
                {
                    second++;
                    return 5;
                }
                if (second == 9)
                {
                    second++;
                    return 6;
                }
                if (second == 10)
                {
                    second++;
                    return 5;
                }
                if (second == 11)
                {
                    second++;
                    return 4;
                }
                if (second == 12)
                {
                    second++;
                    return 4;
                }
                if (second == 13)
                {
                    second++;
                    return 6;
                }
                if (second == 14)
                {
                    second++;
                    return 4;
                }
                if (second == 15)
                {
                    second++;
                    return 5;
                }
                if (second == 16)
                {
                    second++;
                    return 4;
                }
                if (second == 17)
                {
                    second++;
                    return 4;
                }
                if (second == 18)
                {
                    second++;
                    return 5;
                }
                second++;
                return 6;

                //return num;
                //return 5;
                //return random.NextDouble() * 10;
            }
        }

        public Action<string> Action;

        IPLCInterface IHardwareController.PLCInterface => throw new NotImplementedException();
    }
}
