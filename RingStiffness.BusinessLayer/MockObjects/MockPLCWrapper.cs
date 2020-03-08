using RingStiffness.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RingStiffness.BusinessLayer.MockObjects
{
    public class MockPLCWrapper : IPLCWrapper
    {
        private ILoadCell _loadCell;
        private IServoMotor _servoMotor;
        private IExtensiometer _extensiometer;

        public IExtensiometer Extensiometer
        {
            get
            {
                if (_extensiometer == null)
                {
                    _extensiometer = new MockExtensiometer();
                }
                return _extensiometer;
            }
        }

        public IServoMotor ServoMotor
        {
            get
            {
                if (_servoMotor == null)
                {
                    _servoMotor = new MockServoMotor();
                }
                return _servoMotor;
            }
        }

        public ILoadCell LoadCell
        {
            get
            {
                if (_loadCell == null)
                {
                    _loadCell = new MockLoadCell();
                }
                return _loadCell;
            }
        }

        public IPLCInterface PLCInterface => throw new NotImplementedException();
    }
}
