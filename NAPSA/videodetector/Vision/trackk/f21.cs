﻿using System;
using System.Drawing;
using System.Windows.Forms;
using Accord.Imaging;
using Accord.Imaging.Filters;
using Accord.Video.DirectShow;
using System.Drawing.Imaging;
using System.Threading;
using Accord.Video;

namespace trackk
{
    public partial class f21 : Form
    {
        //string d = "";
        private FilterInfoCollection videoDevices;
        EuclideanColorFiltering filter = new EuclideanColorFiltering();
        Color color = Color.Black;
        //GrayscaleBT709 grayscaleFilter = new GrayscaleBT709();
        BlobCounter blobCounter = new BlobCounter();
        int range = 120;
        public f21()
        {
            InitializeComponent();
           
            blobCounter.MinWidth = 2;
            blobCounter.MinHeight = 2;
            blobCounter.FilterBlobs = true;
            blobCounter.ObjectsOrder = ObjectsOrder.Size;
            try
            {
                // enumerate video devices
                videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

                if (videoDevices.Count == 0)
                    throw new ApplicationException();

                // add all devices to combo
                foreach (FilterInfo device in videoDevices)
                {
                    camerasCombo.Items.Add(device.Name);
                }
                camerasCombo.Items.Add("HikVision Camera");
                camerasCombo.SelectedIndex = 0;
            }
            catch (ApplicationException)
            {
                camerasCombo.Items.Add("No local capture devices");
                videoDevices = null;
            }

            Bitmap b = new Bitmap(320, 240);
           // Rectangle a = (Rectangle)r;
            Pen pen1 = new Pen(Color.FromArgb(160, 255, 160), 3);
            Graphics g2 = Graphics.FromImage(b);
            pen1 = new Pen(Color.FromArgb(255, 0, 0), 3);
            g2.Clear(Color.White);
            g2.DrawLine(pen1, b.Width / 2, 0, b.Width / 2, b.Width);
            g2.DrawLine(pen1, b.Width, b.Height / 2, 0, b.Height / 2); 
            pictureBox1.Image = (System.Drawing.Image)b;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
           
        
        }

        private void videoSourcePlayer1_NewFrame(object sender, NewFrameEventArgs args)
        {
            //DateTime now = DateTime.Now;
            //Graphics g = Graphics.FromImage(args.Frame);

            //// paint current time
            //SolidBrush brush = new SolidBrush(Color.Red);
            //g.DrawString(now.ToString(), this.Font, brush, new PointF(5, 5));
            //brush.Dispose();

            //g.Dispose();

        }


        private void videoSourcePlayer2_NewFrame(object sender, NewFrameEventArgs args)
        {
            Bitmap objectsImage = null;
            Bitmap mImage = null;
            mImage=(Bitmap)args.Frame.Clone();
            filter.CenterColor = new RGB(color);
            filter.Radius =(short)range;
           
            objectsImage = args.Frame;
            filter.ApplyInPlace(objectsImage);

            BitmapData objectsData = objectsImage.LockBits(new Rectangle(0, 0, args.Frame.Width, args.Frame.Height),
            ImageLockMode.ReadOnly, args.Frame.PixelFormat);
            UnmanagedImage grayImage = Grayscale.CommonAlgorithms.BT709.Apply(new UnmanagedImage(objectsData));
            objectsImage.UnlockBits(objectsData);

            
            blobCounter.ProcessImage(grayImage);
            Rectangle[] rects = blobCounter.GetObjectsRectangles();
           
            if (rects.Length > 0)
            {

                foreach (Rectangle objectRect in rects)
                {
                    Graphics g = Graphics.FromImage(mImage);
                    using (Pen pen = new Pen(Color.FromArgb(160, 255, 160), 5))
                    {
                        g.DrawRectangle(pen, objectRect);
                    }

                    g.Dispose();
                }
            }

            args.Frame = mImage;
        }

        private void videoSourcePlayer3_NewFrame(object sender, NewFrameEventArgs args)
        {
            Bitmap objectsImage = null;
      
                
            // set center colol and radius
            filter.CenterColor = new RGB(color);
            filter.Radius = (short)range;
            // apply the filter
            objectsImage = args.Frame;
            filter.ApplyInPlace(args.Frame);

            // lock image for further processing
            BitmapData objectsData = objectsImage.LockBits(new Rectangle(0, 0, args.Frame.Width, args.Frame.Height),
                ImageLockMode.ReadOnly, args.Frame.PixelFormat);

            // grayscaling
            UnmanagedImage grayImage = Grayscale.CommonAlgorithms.BT709.Apply(new UnmanagedImage(objectsData));

            // unlock image
            objectsImage.UnlockBits(objectsData);

            // locate blobs 
            blobCounter.ProcessImage(grayImage);
            Rectangle[] rects = blobCounter.GetObjectsRectangles();
          
            if (rects.Length > 0)
            {
                Rectangle objectRect = rects[0];

                // draw rectangle around derected object
                Graphics g = Graphics.FromImage(args.Frame);

                using (Pen pen = new Pen(Color.FromArgb(160, 255, 160), 5))
                {
                    g.DrawRectangle(pen, objectRect);
                }
                g.Dispose();
                int objectX = objectRect.X + objectRect.Width / 2 - args.Frame.Width / 2;
                int objectY = args.Frame.Height / 2 - (objectRect.Y + objectRect.Height / 2);
                ParameterizedThreadStart t = new ParameterizedThreadStart(p);
               Thread aa = new Thread(t);
               aa.Start(rects[0]);               
            }
            Graphics g1 = Graphics.FromImage(args.Frame);
            Pen pen1 = new Pen(Color.FromArgb(160, 255, 160), 3);
            g1.DrawLine(pen1, args.Frame.Width/2,0, args.Frame.Width/2, args.Frame.Width);
            g1.DrawLine(pen1, args.Frame.Width , args.Frame.Height / 2, 0, args.Frame.Height / 2);
            g1.Dispose();
       }

  


       void p(object r)
       {
           try
           {
          
           Bitmap b = new Bitmap(pictureBox1.Image);
           Rectangle a = (Rectangle)r;
           Pen pen1 = new Pen(Color.FromArgb(160, 255, 160), 3);
           Graphics g2 = Graphics.FromImage(b);
           pen1 = new Pen(color, 3);
           // Brush b5 = null;
           SolidBrush b5 = new SolidBrush(color);
           //   g2.Clear(Color.Black);


           Font f = new Font(Font, FontStyle.Bold);

           g2.DrawString("o", f, b5, a.Location);
           g2.Dispose();
           pictureBox1.Image = (System.Drawing.Image)b;
           this.Invoke((MethodInvoker)delegate
               {
                   richTextBox1.Text = a.Location.ToString() + "\n" + richTextBox1.Text + "\n"; ;
               });
           }
           catch (Exception ex)
           {
                Console.WriteLine(ex.ToString());
                Thread.CurrentThread.Abort();
           }
       }
        
        private void button1_Click(object sender, EventArgs e)
        {

            videoSourcePlayer1.SignalToStop();
            videoSourcePlayer1.WaitForStop();
            videoSourcePlayer2.SignalToStop();
            videoSourcePlayer2.WaitForStop();
            videoSourcePlayer3.SignalToStop();
            videoSourcePlayer3.WaitForStop();
            // videoDevices = null;
//            VideoCaptureDevice videoSource = new VideoCaptureDevice(videoDevices[camerasCombo.SelectedIndex].MonikerString);
            // create video source
            MJPEGStream videoSource = new MJPEGStream("http://192.168.1.64/Streaming/Channels/1/httppreview");
            videoSource.Login = "admin";
            videoSource.Password = "Qwer1234";

            //for (int i = 0; i < videoSource.VideoCapabilities.Length; i++)
            //{

            //    String resolution = "Resolution Number " + Convert.ToString(i);
            //    String resolution_size = videoSource.VideoCapabilities[i].FrameSize.ToString();
            //    //System.Windows.Forms.MessageBox.Show(resolution + ":" + resolution_size);
            //    Console.WriteLine(resolution + ":" + resolution_size);
            //}



            //mjpegSource.VideoResolution = videoSource.VideoCapabilities[0]; // new Size(320, 240);
            
            //videoSource.DesiredFrameRate = 12;

            videoSourcePlayer1.VideoSource = videoSource;
            videoSourcePlayer1.Start();
            videoSourcePlayer2.VideoSource = videoSource;
            videoSourcePlayer2.Start();
            videoSourcePlayer3.VideoSource = videoSource;
            videoSourcePlayer3.Start();
            //groupBox1.Enabled = false;
        }

        private void f21_FormClosing(object sender, FormClosingEventArgs e)
        {
            
            videoSourcePlayer1.SignalToStop();
            videoSourcePlayer1.WaitForStop();
            videoSourcePlayer2.SignalToStop();
            videoSourcePlayer2.WaitForStop();
            videoSourcePlayer3.SignalToStop();
            videoSourcePlayer3.WaitForStop();
            groupBox1.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            videoSourcePlayer1.SignalToStop();
            videoSourcePlayer1.WaitForStop();
            videoSourcePlayer2.SignalToStop();
            videoSourcePlayer2.WaitForStop();
            videoSourcePlayer3.SignalToStop();
            videoSourcePlayer3.WaitForStop();
        }

         private void button3_Click(object sender, EventArgs e)
        {
           
            colorDialog1.ShowDialog();
            color = colorDialog1.Color;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e) => range = Convert.ToInt32(numericUpDown1.Value);

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            blobCounter.MaxWidth = Convert.ToInt32(numericUpDown2.Value);
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            blobCounter.MinWidth  = Convert.ToInt32(numericUpDown3.Value);
        }
    }
}
