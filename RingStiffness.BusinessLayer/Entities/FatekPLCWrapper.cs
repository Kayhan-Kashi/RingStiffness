using RingStiffness.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RingStiffness.BusinessLayer.Entities
{
    public class FatekPLCWrapper : IPLCWrapper
    {
        private IPLCInterface _plcInterface;
        private ILoadCell _loadCell;
        private IServoMotor _servoMotor;

        public FatekPLCWrapper(string comNumber, int baudRate)
        {
            _plcInterface = new FatekPLCInterface(comNumber, baudRate);
        }

        public IPLCInterface PLCInterface
        {
            get
            {
                return _plcInterface;
            }
        }

        public IServoMotor ServoMotor
        {
            get
            {
                if (_servoMotor == null)
                {
                    _servoMotor = new ServoMotor(PLCInterface);
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
                    _loadCell = new LoadCell(PLCInterface);
                }
                return _loadCell;
            }
        }
    }
}
