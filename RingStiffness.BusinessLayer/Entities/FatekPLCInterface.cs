using IndustrialNetworks.FatekCom;
using RingStiffness.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;

namespace RingStiffness.BusinessLayer.Entities
{
    public class FatekPLCInterface : IPLCInterface
    {
        private string _comNumber;
        private FatekCommunication objFatekCommunication;

        /// <summary>
        /// This constructor takes the COM number and baude rate and adds Parity bit and number of bit values and calls Fatek core constructor
        /// </summary>
        /// <param name="COMno"></param>
        /// <param name="baudRate"></param>
        public FatekPLCInterface(string COMno, int baudRate)
        {
            _comNumber = COMno;
            objFatekCommunication = new FatekCommunication(COMNumber, baudRate, Parity.Even, 7, StopBits.One);
            objFatekCommunication.Connect();
        }

        public string COMNumber
        {
            get
            {
                return _comNumber;
            }
        }

        /// <summary>
        /// This method searches the MemoryType enum and finds the appropriate one by the parameter given
        /// </summary>
        /// <param name="memoryType"></param>
        /// <returns></returns>
        private MemoryType FindMemoryType(string memoryType)
        {
            MemoryType memory;
            Enum.TryParse<MemoryType>(memoryType, out memory);
            return memory;
        }

        public string ReadDataRegister(MemoryType memoryT, int registerNo, DataType dataTypeT)
        {
            return objFatekCommunication.ReadRegisters(1, 1, memoryT, (ushort)registerNo, dataTypeT)[0].ToString();
        }

        public void WriteToDiscreteRegister(MemoryType memoryT, int registerNo, bool valueToWrite)
        {
            //MemoryType memoryTypeValue = FindMemoryType(memoryT);
            objFatekCommunication.WriteMultipeDiscretes(1, 1, memoryT, (ushort)registerNo, DataType.BOOL, new bool[] { valueToWrite });
        }



    }
}
