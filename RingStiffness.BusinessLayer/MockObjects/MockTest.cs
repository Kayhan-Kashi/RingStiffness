﻿using RingStiffness.Common.Interfaces;
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
        private bool isTestFinished = false;

        //private int second = 0;
        System.Timers.Timer touchPipe_timer = new System.Timers.Timer();
        System.Timers.Timer test_operation_timer = new System.Timers.Timer();
        System.Timers.Timer reach_test_properForce_timer = new System.Timers.Timer()
                

        public double CurrentTestForce { get; set; }
        public int TestPassedSecond { get; set; }

        public IPLCWrapper PLCCommand { get; set; }

        public void TouchPipe()
        {
            PLCCommand.ServoMotor.Down();
            touchPipe_timer.Enabled = true;    
        }

        private void MoveServoMotorUntilPipeIsTouched(Object source, ElapsedEventArgs e)
        {
            var forceSensed = PLCCommand.LoadCell.Force;
            CurrentTestForce = forceSensed;
            //Debug.WriteLine(e.SignalTime.ToShortDateString());
            //second += 1;

            if (forceSensed >= TouchForceExpected)
            {
                //Debug.WriteLine("Force is : " + PLCCommand.LoadCell.Force);
          
                Debug.WriteLine("//////////////////////////////MoveServoMotorUntilPipeIsTouched/////////////////////////////////////////////");
                Debug.WriteLine("Force is : " + forceSensed);
                Debug.WriteLine("Pipe has been touched.");
                Debug.WriteLine("/////////////////////////////////////////////////////////////////////////////////////////////////");
                touchPipe_timer.Enabled = false;
                PLCCommand.ServoMotor.Stop();
                touchPipe_timer.Dispose();
                ReachTestProperForce();
     
            }
            else
            {
                Debug.WriteLine("//////////////////////////////MoveServoMotorUntilPipeIsTouched/////////////////////////////////////////////");
                Debug.WriteLine("Force is : " + forceSensed);
                Debug.WriteLine("////still not touched//////");
                Debug.WriteLine("/////////////////////////////////////////////////////////////////////////////////////////////////");
            }

        }

        private void MoveServoMotorUntilTestProperForce(Object source, ElapsedEventArgs e)
        {
            double currentForce = PLCCommand.LoadCell.Force;
            CurrentTestForce = currentForce;

            if (currentForce < TestInputWeight && !isTestStarted)  // Means still proper weight for the test has not been sensed 
            {
                Debug.WriteLine("/////////////////////////////MoveServoMotorUntilTestProperForce/////////////////////////////////////");
                // after touching the pipe and hence test has not been started
                Debug.WriteLine("Time is : " + GetTestSecondsPassed());
                Debug.WriteLine("Force is : " + currentForce);
                Debug.WriteLine("////test proper force not reached yet//////");
                PLCCommand.ServoMotor.Down();
                Debug.WriteLine("//////////////////////////////MoveServoMotorUntilTestProperForce///////////////////////////////////////");
            }

            if (currentForce >= TestInputWeight && !isTestStarted)  //Means proper weight for the test has been reached and so the test has to be started.
            {
                Debug.WriteLine("//////////////////////////MoveServoMotorUntilTestProperForce/////////////////////////////////////////////");
                PLCCommand.ServoMotor.Stop();
                Debug.WriteLine("Time is : " + GetTestSecondsPassed());
                Debug.WriteLine("Force is : " + currentForce);
                Debug.WriteLine("////test proper force reached //////");
                Debug.WriteLine("//////////////////////////MoveServoMotorUntilTestProperForce////////////////////////////////////////////");
                reach_test_properForce_timer.Enabled = false;
                StartTestOperation();
                reach_test_properForce_timer.Dispose();
            }
        }

        private void TestingOperation(Object source, ElapsedEventArgs e)
        {
            double currentForce = PLCCommand.LoadCell.Force;
            TestPassedSecond = GetTestSecondsPassed();
            CurrentTestForce = currentForce;

            if (isTestFinished)
            {
                testSecondCounter.Stop();
                test_operation_timer.Dispose();
                StopTestOperation();
                testSecondCounter.Stop();
                return;
            }

            if (TestPassedSecond >= TestTotalSeconds )                     // Test total seconds has been reached so the test has to be stopped
            {
            
                StopTestOperation();
                Debug.WriteLine("///////////////////////////TestingOperation////////////////////////////////////////////////");
                PLCCommand.ServoMotor.Stop();
                Debug.WriteLine("Time is : " + TestPassedSecond);
                Debug.WriteLine("Test Finished");
                Debug.WriteLine("Force is : " + currentForce);
                testSecondCounter.Stop();
                Debug.WriteLine("/////////////////////////////////////////////////////////////////////////////////////////////////");
             
                test_operation_timer.Dispose();
                isTestFinished = true;
            }
            //if (PLCCommand.LoadCell.Force >= TestInputWeight && !isTestStarted)  //Means proper weight for the test has been reached and so the test has to be started.
            //{
            //    StartTestOperation();
            //    Debug.WriteLine("Time is : " + GetTestSecondsPassed());
            //    Debug.WriteLine("Force is : " + PLCCommand.LoadCell.Force);
            //    PLCCommand.ServoMotor.Stop();
            //}


            if (currentForce < TestInputWeight && !isTestFinished)  // Means proper weight was reached before and hence test had been started but the weight                                                                                                      
            {
                Debug.WriteLine("///////////////////////////TestingOperation/////////////////////////////////////////////////////");
                // has been decreased due to deflection
                Debug.WriteLine("Time is : " + TestPassedSecond);
                Debug.WriteLine("Force is : " + currentForce);
                Debug.WriteLine("//// force decreased //////");
                Debug.WriteLine("/////////////////////////////////////////////////////////////////////////////////////////////////");
                PLCCommand.ServoMotor.Down();
            }
            if (currentForce >= TestInputWeight && !isTestFinished)  // Means excessive force has been applied hence the motor has to stop until the force decreases 
            {
                Debug.WriteLine("///////////////////TestingOperation///////////////////////////////////////////////////////////");
                Debug.WriteLine("Time is : " + TestPassedSecond);
                Debug.WriteLine("Force is : " + currentForce);
                Debug.WriteLine("//// force is above proper amount //////");
                Debug.WriteLine("/////////////////////////////////////////////////////////////////////////////////////////////////");
                PLCCommand.ServoMotor.Stop();
            }
        }

        public void StartTestOperation()
        {
            test_operation_timer.Enabled = true;
            testSecondCounter.Start();
        }

        public void ReachTestProperForce()
        {
            reach_test_properForce_timer.Enabled = true;
        }

        public void PrepareAndStartTheTest()
        {
            ConfigTest();      
            TouchPipe();
            //ReachTestProperForce();
        }


        public void StopTestOperation()
        {
            test_operation_timer.Enabled = false;
        }

        private int GetTestSecondsPassed()
        {
            return testSecondCounter.Elapsed.Seconds;
        }

        public void ConfigTest()
        {
            touchPipe_timer.Interval = 1000;
            touchPipe_timer.Elapsed += MoveServoMotorUntilPipeIsTouched;

            test_operation_timer.Elapsed += TestingOperation;
            test_operation_timer.Interval = 1000;

            reach_test_properForce_timer.Elapsed += MoveServoMotorUntilTestProperForce;
            reach_test_properForce_timer.Interval = 1000;
        }
    }

}
