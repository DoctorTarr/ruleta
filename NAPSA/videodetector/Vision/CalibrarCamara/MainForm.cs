﻿// Simple Player sample application
// AForge.NET framework
// http://www.aforgenet.com/framework/
//
// Copyright © AForge.NET, 2006-2011
// contacts@aforgenet.com
//

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

using Accord.Video;
using Accord.Video.DirectShow;
using System.Drawing.Imaging;
using Accord.Imaging;
using Accord;

namespace SampleApp
{
    public partial class MainForm : Form
    {
        private Stopwatch stopWatch = null;
        private Size _pbSize = new Size(640, 400);
        // Class constructor
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseCurrentVideoSource();
        }

        // "Exit" menu item clicked
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Open local video capture device
        private void localVideoCaptureDeviceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VideoCaptureDeviceForm form = new VideoCaptureDeviceForm();

            if (form.ShowDialog(this) == DialogResult.OK)
            {
                // create video source
                VideoCaptureDevice videoSource = form.VideoDevice;

                // open it
                OpenVideoSource(videoSource);
            }
        }

        // Open video file using DirectShow
        private void openVideofileusingDirectShowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // create video source
                FileVideoSource fileSource = new FileVideoSource(openFileDialog.FileName);

                // open it
                OpenVideoSource(fileSource);
            }
        }

        // Open JPEG URL
        private void openJPEGURLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            URLForm form = new URLForm();

            form.Description = "Enter URL of an updating JPEG from a web camera:";
            form.URLs = new string[]
            {
                "http://195.243.185.195/axis-cgi/jpg/image.cgi?camera=1",
            };

            if (form.ShowDialog(this) == DialogResult.OK)
            {
                // create video source
                JPEGStream jpegSource = new JPEGStream(form.URL);

                // open it
                OpenVideoSource(jpegSource);
            }
        }

        // Open MJPEG URL
        private void openMJPEGURLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            URLForm form = new URLForm();

            form.Description = "Enter URL of an MJPEG video stream:";
            form.URLs = new string[]
            {
                "http://webcam.st-malo.com/axis-cgi/mjpg/video.cgi?resolution=352x288",
                "http://88.53.197.250/axis-cgi/mjpg/video.cgi?resolution=320x240",
            };

            if (form.ShowDialog(this) == DialogResult.OK)
            {
                // create video source
                MJPEGStream mjpegSource = new MJPEGStream(form.URL);

                // open it
                OpenVideoSource(mjpegSource);
            }
        }

        // Capture 1st display in the system
        private void capture1stDisplayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenVideoSource(new ScreenCaptureStream(Screen.AllScreens[0].Bounds, 100));
        }

        // Open video source
        private void OpenVideoSource(IVideoSource source)
        {
            // set busy cursor
            this.Cursor = Cursors.WaitCursor;

            // stop current video source
            CloseCurrentVideoSource();

            // start new video source
            videoSourcePlayer.VideoSource = source;
            videoSourcePlayer.Start();

            // reset stop watch
            stopWatch = null;

            // start timer
            timer.Start();

            this.Cursor = Cursors.Default;
        }

        // Close video source if it is running
        private void CloseCurrentVideoSource()
        {
            if (videoSourcePlayer.VideoSource != null)
            {
                videoSourcePlayer.SignalToStop();
                videoSourcePlayer.VideoSource = null;
            }
        }

        // New frame received by the player
        private void videoSourcePlayer_NewFrame(object sender, NewFrameEventArgs args)
        {
            //DateTime now = DateTime.Now;
            Bitmap objectsImage = new Bitmap(args.Frame, _pbSize);
            args.Frame = objectsImage;
            Graphics g = Graphics.FromImage(args.Frame);

            // paint current time
            SolidBrush brush = new SolidBrush(Color.Red);
            //g.DrawString(now.ToString(), this.Font, brush, new PointF(5, 5));
            brush.Dispose();

            g.Dispose();

            DrawGrid(args.Frame);


        }

        // On timer event - gather statistics
        private void timer_Tick(object sender, EventArgs e)
        {
            IVideoSource videoSource = videoSourcePlayer.VideoSource;

            if (videoSource != null)
            {
                // get number of frames since the last timer tick
                int framesReceived = videoSource.FramesReceived;

                if (stopWatch == null)
                {
                    stopWatch = new Stopwatch();
                    stopWatch.Start();
                }
                else
                {
                    stopWatch.Stop();

                    float fps = 1000.0f * framesReceived / stopWatch.ElapsedMilliseconds;
                    fpsLabel.Text = fps.ToString("F2") + " fps";

                    stopWatch.Reset();
                    stopWatch.Start();
                }
            }
        }

        private void videoSourcePlayer_Click(object sender, EventArgs e)
        {
            videoSourcePlayer.SignalToStop();
            videoSourcePlayer.WaitForStop();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
                // create video source
                MJPEGStream mjpegSource = new MJPEGStream("http://192.168.1.64/Streaming/Channels/1/httppreview");
                mjpegSource.Login = "admin";
                mjpegSource.Password = "Qwer1234";

                // open it
                OpenVideoSource(mjpegSource);
        }

        // Draw motion history
        private void DrawGrid(Bitmap image)
        {
 
            BitmapData bitmapData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height),
                ImageLockMode.ReadWrite, image.PixelFormat);

            int rectWidth = image.Width / 2;
            int rectHeight = image.Height / 4;
            int rectX = image.Width / 2 - rectWidth / 2;
            int rectY = image.Height / 2 - rectHeight / 2;

            Drawing.Line(bitmapData, new IntPoint(image.Width / 2, 0), new IntPoint(image.Width / 2, image.Width),
                Color.Red);

            Drawing.Line(bitmapData, new IntPoint(image.Width, image.Height / 2), new IntPoint(0, image.Height / 2),
                Color.Red);

            //Graphics g1 = Graphics.FromImage(image);
            //Pen pen1 = new Pen(Color.FromArgb(160, 255, 160), 3);
            //g1.DrawLine(pen1, image.Width / 2, 0, image.Width / 2, image.Width);
            //g1.DrawLine(pen1, image.Width, image.Height / 2, 0, image.Height / 2);
            //g1.Dispose();

            image.UnlockBits(bitmapData);
        }

    }
}
