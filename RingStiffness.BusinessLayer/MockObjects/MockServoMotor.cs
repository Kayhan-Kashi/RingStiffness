﻿using RingStiffness.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace RingStiffness.BusinessLayer.MockObjects
{
    public class MockServoMotor : IServoMotor , IStatusWritable
    {
        public IPLCInterface PLCInterface => throw new NotImplementedException();
        public bool IsMotorStopped { get; set; }
        public string MotorDirection { get; set; }

        public Action<string> WriteStatus { get; set; }
      

        //public Action<string> Action;

   
        public void Down()
        {
            Debug.WriteLine("Motor is going down.");
            WriteStatus("Motor is going down.");
            MotorDirection = "Down";
            IsMotorStopped = false;
        }

        public void Stop()
        {
            Debug.WriteLine("Motor has stopped.");
            WriteStatus("Motor has stopped.");
        }

        public void Up()
        {
            Debug.WriteLine("Motor is going up.");
            IsMotorStopped = false;
            MotorDirection = "Up";
        }
    }
}
