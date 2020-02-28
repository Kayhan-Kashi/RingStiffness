﻿using RingStiffness.BusinessLayer.MockObjects;
using RingStiffness.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace RingStiffness.UI.Forms
{
    public partial class FrmMain : Form
    {
        private IPLCWrapper plc;
        private System.Threading.Timer timer;
        private Stopwatch stopWatch = new Stopwatch();
        private TimeSpan currentTimeElapsed;


        public void StartSplachScreen()
        {
            Application.Run(new FrmSplashScreen());
        }

        public FrmMain()
        {
            Thread splashThread = new Thread(new ThreadStart(StartSplachScreen));
            splashThread.Start();
            Thread.Sleep(2000);

            InitializeComponent();
            splashThread.Abort();

        }


        private void FrmMain_Load(object sender, EventArgs e)
        {
            //plc = new FatekPLCWrapper("COM7", 9600
            plc = new MockPLCWrapper();
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            plc.ServoMotor.Up();
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            plc.ServoMotor.Down();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            plc.ServoMotor.Stop();
        }

        private void ShowForce()
        {
            string text = plc.LoadCell.Force.ToString();
            currentTimeElapsed = stopWatch.Elapsed;
            lblForce.Text = text;
            Draw(plc.LoadCell.Force, currentTimeElapsed.TotalSeconds);
        }

        public void Draw(double force, double time)
        {
            chart1.Series["force–time"].Points.AddXY(time, force);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            stopWatch.Start();
            var startTimeSpan = TimeSpan.Zero;
            var periodTimeSpan = TimeSpan.FromMilliseconds(300);

            timer = new System.Threading.Timer((ce) =>
            {
                lblForce.Invoke(new MethodInvoker(delegate { ShowForce(); }));
                //ShowForce();
            }, null, startTimeSpan, periodTimeSpan);

        }

        private void btnStopDraw_Click(object sender, EventArgs e)
        {
            stopWatch.Stop();
            timer.Dispose();
        }

        private void btnStartDraw_Click(object sender, EventArgs e)
        {

        }
    }
}