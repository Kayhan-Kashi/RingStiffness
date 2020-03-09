using RingStiffness.BusinessLayer.Entities;
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
    public class MockTest : IStatusWritable
    {
        public int TestTotalSeconds { get; set; }
        public double TouchForceExpected { get; set; }
        public double TestInputWeight { get; set; }
        private Stopwatch testSecondCounter = new Stopwatch();
        private bool isTestStarted = false;
        private bool isTestFinished = false;
        public List<TestData> TestResult;

        public MockTest()
        {
            TestResult = new List<TestData>();
        }

        private void AddTestData(double force, int second ,DateTime time, double deflection)
        {
            TestResult.Add(new TestData { Force = force, Second = second, Deflection = deflection });
        }


        //private int second = 0;
        System.Timers.Timer touchPipe_timer = new System.Timers.Timer();
        System.Timers.Timer test_operation_timer = new System.Timers.Timer();
        System.Timers.Timer reach_test_properForce_timer = new System.Timers.Timer();
                

        public double CurrentTestForce { get; set; }
        public int testPassedSecond { get; set; }

        public IPLCWrapper PLCCommand { get; set; }
        public Action<string> WriteStatus { get; set; }

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
                WriteStatus("Pipe has been touched.");
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
                WriteStatus("Test has been started.");
                Debug.WriteLine("//////////////////////////MoveServoMotorUntilTestProperForce////////////////////////////////////////////");
                reach_test_properForce_timer.Enabled = false;
                StartTestOperation();
                reach_test_properForce_timer.Dispose();
            }
        }

        private void TestingOperation(Object source, ElapsedEventArgs e)
        {

            double deflection = PLCCommand.Extensiometer.Displacement;
            double currentForce = PLCCommand.LoadCell.Force;
            testPassedSecond = GetTestSecondsPassed();
            CurrentTestForce = currentForce;

            if (isTestFinished)
            {
                testSecondCounter.Stop();
                test_operation_timer.Dispose();
                StopTestOperation();
                testSecondCounter.Stop();
                return;
            }

            if (testPassedSecond >= TestTotalSeconds )                     // Test total seconds has been reached so the test has to be stopped
            {
            
                StopTestOperation();
                Debug.WriteLine("///////////////////////////TestingOperation////////////////////////////////////////////////");
                PLCCommand.ServoMotor.Stop();
                Debug.WriteLine("Time is : " + testPassedSecond);
                Debug.WriteLine("Test Finished");
                Debug.WriteLine("Force is : " + currentForce);
                AddTestData(currentForce, testPassedSecond, new DateTime(), deflection);
                testSecondCounter.Stop();
                DrawForce(testPassedSecond, currentForce);
                DrawDeflection(testPassedSecond, deflection);
                FillGridView(testPassedSecond, currentForce, deflection);

                Debug.WriteLine("/////////////////////////////////////////////////////////////////////////////////////////////////");
             
                test_operation_timer.Dispose();
                isTestFinished = true;
            }

            if (currentForce < TestInputWeight && !isTestFinished)  // Means proper weight was reached before and hence test had been started but the weight                                                                                                      
            {
                Debug.WriteLine("///////////////////////////TestingOperation/////////////////////////////////////////////////////");
                // has been decreased due to deflection
                Debug.WriteLine("Time is : " + testPassedSecond);
                Debug.WriteLine("Force is : " + currentForce);
                Debug.WriteLine("//// force decreased //////");
                AddTestData(currentForce, testPassedSecond, new DateTime(), 0);
                DrawForce(testPassedSecond, currentForce);
                DrawDeflection(testPassedSecond, deflection);
                FillGridView(testPassedSecond, currentForce, deflection);
                Debug.WriteLine("/////////////////////////////////////////////////////////////////////////////////////////////////");
                PLCCommand.ServoMotor.Down();
            }
            if (currentForce >= TestInputWeight && !isTestFinished)  // Means excessive force has been applied hence the motor has to stop until the force decreases 
            {
                Debug.WriteLine("///////////////////TestingOperation///////////////////////////////////////////////////////////");
                Debug.WriteLine("Time is : " + testPassedSecond);
                Debug.WriteLine("Force is : " + currentForce);
                Debug.WriteLine("//// force is above proper amount //////");
                Debug.WriteLine("/////////////////////////////////////////////////////////////////////////////////////////////////");
                AddTestData(currentForce, testPassedSecond, new DateTime(), 0);
                DrawForce(testPassedSecond, currentForce);
                DrawDeflection(testPassedSecond, deflection);
                FillGridView(testPassedSecond, currentForce, deflection);
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

        public LetResultOut DrawForce;
        public LetResultOut DrawDeflection;
        public LetResultInGridOut FillGridView;


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

    public delegate void LetResultOut(int param1, double param2);
    public delegate void LetResultInGridOut(int param1, double param2, double param3);
    public delegate void SetStatus(string str);
}


