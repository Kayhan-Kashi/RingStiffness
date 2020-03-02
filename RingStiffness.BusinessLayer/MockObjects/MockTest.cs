using RingStiffness.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace RingStiffness.BusinessLayer.MockObjects
{
    public class MockTest
    {
        public int TestTotalSeconds { get; set; }
        public double TouchForceExpected { get; set; }
        public double TestInputWeight { get; set; }
        private Stopwatch testSecondCounter = new Stopwatch();
        private bool isTestStarted = false;

        //private int second = 0;
        System.Timers.Timer touch_timer = new System.Timers.Timer();
        System.Timers.Timer test_operation = new System.Timers.Timer();
        System.Timers.Timer test_preperation = new System.Timers.Timer();

        public IPLCWrapper PLCCommand { get; set; }

        public void TouchPipe()
        {
            touch_timer.Interval = 1000;
            touch_timer.Elapsed += OnLoadCellReadForce;
            PLCCommand.ServoMotor.Down();
            touch_timer.Enabled = true;    
        }

        private void OnLoadCellReadForce(Object source, ElapsedEventArgs e)
        {
            var forceSensed = PLCCommand.LoadCell.Force;
            //Debug.WriteLine(e.SignalTime.ToShortDateString());
            //second += 1;
            Debug.WriteLine("Force is : " + forceSensed);
            Debug.WriteLine("//////////////");
            if (forceSensed >= TouchForceExpected)
            {
                //Debug.WriteLine("Force is : " + PLCCommand.LoadCell.Force);
                Debug.WriteLine("Pipe has been touched.");
                touch_timer.Enabled = false;
                PLCCommand.ServoMotor.Stop();        
            }
        }

        public void StartTestOperation()
        {
            test_operation.Enabled = true;
            testSecondCounter.Start();
        }

        public void StartTestPreperation()
        {
            test_preperation.Enabled = true;
        }

        private void OnPreperationServoMove(Object source, ElapsedEventArgs e)
        {
            if (PLCCommand.LoadCell.Force < TestInputWeight && !isTestStarted)  // Means still proper weight for the test has not been sensed 
            {                                                                   // after touching the pipe and hence test has not been started
                Debug.WriteLine("Time is : " + GetTestSecondsPassed());
                Debug.WriteLine("Force is : " + PLCCommand.LoadCell.Force);
                PLCCommand.ServoMotor.Down();
            }

            if (PLCCommand.LoadCell.Force >= TestInputWeight && !isTestStarted)  //Means proper weight for the test has been reached and so the test has to be started.
            {
                test_preperation.Enabled = false;
                PLCCommand.ServoMotor.Stop();
                Debug.WriteLine("Time is : " + GetTestSecondsPassed());
                Debug.WriteLine("Force is : " + PLCCommand.LoadCell.Force);       
                StartTestOperation();                   
            }
        }

        public void StartTest()
        {
            ConfigTest();      
            TouchPipe();
            StartTestPreperation();
        }

        public void StopTestOperation()
        {
            test_operation.Enabled = false;
        }

        private int GetTestSecondsPassed()
        {
            return testSecondCounter.Elapsed.Seconds;
        }

        private void OnTestServoMove(Object source, ElapsedEventArgs e)
        {
            if (GetTestSecondsPassed() >= TestTotalSeconds)                     // Test total seconds has been reached so the test has to be stopped
            {
                Debug.WriteLine("Time is : " + GetTestSecondsPassed());
                Debug.WriteLine("Test Finished");
                Debug.WriteLine("Force is : " + PLCCommand.LoadCell.Force);
                StopTestOperation();
                testSecondCounter.Stop();
            }
            //if (PLCCommand.LoadCell.Force >= TestInputWeight && !isTestStarted)  //Means proper weight for the test has been reached and so the test has to be started.
            //{
            //    StartTestOperation();
            //    Debug.WriteLine("Time is : " + GetTestSecondsPassed());
            //    Debug.WriteLine("Force is : " + PLCCommand.LoadCell.Force);
            //    PLCCommand.ServoMotor.Stop();
            //}
    

            if (PLCCommand.LoadCell.Force < TestInputWeight && isTestStarted)  // Means proper weight was reached before and hence test had been started but the weight                                                                                                      
            {                                                                  // has been decreased due to deflection
                Debug.WriteLine("Time is : " + GetTestSecondsPassed());
                Debug.WriteLine("Force is : " + PLCCommand.LoadCell.Force);
                PLCCommand.ServoMotor.Down();
            }
            if (PLCCommand.LoadCell.Force >= TestInputWeight && isTestStarted)  // Means excessive force has been applied hence the motor has to stop until the force decreases 
            {                                                                   
                Debug.WriteLine("Time is : " + GetTestSecondsPassed());
                Debug.WriteLine("Force is : " + PLCCommand.LoadCell.Force);
                PLCCommand.ServoMotor.Stop();
            }
        }


        public void ConfigTest()
        {
            test_operation.Elapsed += OnTestServoMove;
            test_operation.Interval = 1000;

            test_preperation.Elapsed += OnPreperationServoMove;
            test_preperation.Interval = 1000;
        }




    }

}
