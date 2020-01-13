using Accord;
using Accord.Imaging;
using Accord.Imaging.Filters;
using Accord.Math.Geometry;
using Accord.Video;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RouletteNumberDetection
{
    public partial class MainForm : Form
    {
        // Blob detection references
        // http://www.aforgenet.com/framework/features/blobs_processing.html
        // https://www.codeproject.com/Articles/139628/Detect-and-Track-Objects-in-Live-Webcam-Video-Base

        // Color parameters
        int iRedValue = 30, iGreenValue = 220, iBlueValue = 30;
        int iMinWidth = 8, iMaxWidth = 10, iMinHeight = 8, iMaxHeight = 10, iRadius = 110;
        Color color = Color.LimeGreen;
        bool _calibrateFlag = false;
        Size _pbSize = new Size(640, 400);
        int _Distance = 0, _Angle = 0;
        int _WinnerNumber = 0;

        System.Drawing.Point ZeroPos, BallPos;

        // Bitmaps
        //Bitmap _bitmapEdgeImage, _bitmapBinaryImage, _bitmapGreyImage, _bitmapBlurImage, _colorFilterImage;

        // Filters
        EuclideanColorFiltering _colorFilter = new EuclideanColorFiltering();
        SobelEdgeDetector _edgeFilter = new SobelEdgeDetector();
        BlobCounter blobCounter = new BlobCounter();

        // Drawing variables
        Pen _PictureboxPen = new Pen(Color.Black, 5);
        System.Drawing.Font _font = new System.Drawing.Font("Times New Roman", 48, FontStyle.Bold);
        System.Drawing.SolidBrush _brush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
        int ipenWidth = 5;


        public MainForm()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        #region Form_Load, Closing
        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                StopCameras();
            }
            catch
            {
                return;
            }
        }
        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            StartCameras();
        }

        private void cbCalibrate_CheckedChanged(object sender, EventArgs e)
        {
            if (cbCalibrate.Checked)
                _calibrateFlag = true;
            else
                _calibrateFlag = false;

        }

       private void button2_Click(object sender, EventArgs e)
        {
            StopCameras();
        }

        #region MyMethods

        private void CalibrateCamera(Bitmap _bitmapSourceImage)
        {
            // Base ruleta sin aro negro - radius 164 color red
            Rectangle rcMain = new Rectangle(120, 5, 410, 460);

            // Cilindro -incluye numeros - radius 204 color red
            Rectangle rcCylinder = new Rectangle(182, 64, 276, 330);

            // Casillas - radius 204 color blue
            Rectangle rcSlots = new Rectangle(225, 111, 198, 240);

            Graphics _g = Graphics.FromImage(_bitmapSourceImage);

            Pen _pengreen = new Pen(Color.GreenYellow, ipenWidth);
            Pen _penyellow = new Pen(Color.GreenYellow, ipenWidth);
            Pen _penred = new Pen(Color.Red, ipenWidth);

            _g.DrawRectangle(_pengreen, rcMain);

            _g.DrawRectangle(_penyellow, rcCylinder);

            _g.DrawRectangle(_penred, rcSlots);

        }

        //private void get_Frame(object sender, NewFrameEventArgs eventArgs)
        //{
        //    blobDetection(sender, eventArgs);
        //}


        private void StartCameras()
        {
            try
            {
                StopCameras();
                // IP Camera
                MJPEGStream videoSource = new MJPEGStream("http://192.168.1.64/Streaming/Channels/101/httppreview");
                videoSource.Login = "admin";
                videoSource.Password = "Qwer1234";
                videoSourcePlayer1.VideoSource = videoSource;
                videoSourcePlayer1.NewFrameReceived += new Accord.Video.NewFrameEventHandler(blobDetection);

                videoSourcePlayer1.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void StopCameras()
        {
            try
            {
                videoSourcePlayer1.SignalToStop();
                videoSourcePlayer1.WaitForStop();
                pictureBox1.Image = null;
                pictureBox2.Image = null;
            }
            catch (Exception)
            {
                return;
            }
        }

        #endregion

        #region Blob Detection
        //For blob recognition, there is a demo application which you will find after you download all the source code.
        //Adding features to it was easy.Typically, you would need to perform some other transformations to the image 
        // before recognition.First of all, I would recommend to increase contrast to maximum. In some cases, you need
        // to perform color transformations, if the features you need should be recognized by subtly different color.And so on…

        /// <summary> Blob Detection    
        /// This method for color object detection by Blob counter algorithm.
        /// If you using this method, then you can detecting as follows:
        ///             red circle, rectangle, triangle
        ///             blue circle, rectangle, triangle
        ///             green circle, rectangle, triangle
        /// the process of this method as follow:
        ///     1. color filtering by Euclidean filtering(R, G, B).
        ///     2. the grayscale filtering based on color filtered image.
        ///     3. In this step, you can choose the blur option. Applied blur option(or not),
        ///        this method donging Sobel edge filtering based on grayscale(or grayscale + blur) image.
        ///     4. the binary filtering based on edge filter image.
        ///     5. Finally, detecting object, distance from the camera and degree are expreed on picturebox 1.
        /// </summary>
        /// 
        private void blobDetection(object sender, NewFrameEventArgs args)
        {
            int winner = -1;

            iRedValue = 5; // sbRedColor.Value;
            iGreenValue = 240; // sbGreenColor.Value;
            iBlueValue = 5; // sbBlueColor.Value;
            iMinWidth = 12; iMaxWidth = 16;
            iMinHeight = 12; iMaxHeight = 16;
            iRadius = 180;

            if (drawBlob(args, pictureBox1, ref ZeroPos))
            {
                tbZeroPosX.Text = ZeroPos.X.ToString();
                tbZeroPosY.Text = ZeroPos.Y.ToString();
            }


            iRedValue = 251; // sbRedColor.Value;
            iGreenValue = 254; // sbGreenColor.Value;
            iBlueValue = 150; // sbBlueColor.Value;
            iMinWidth = 8; iMaxWidth = 12;
            iMinHeight = 8; iMaxHeight = 12;
            iRadius = 50;

            if (drawBlob(args, pictureBox2, ref BallPos))
            {
                tbBolaPosX.Text = BallPos.X.ToString();
                tbBolaPosY.Text = BallPos.Y.ToString();
                _Distance = FindDistance(ZeroPos, BallPos);
                _Angle = GetAngleOfLineBetweenTwoPoints(ZeroPos, BallPos);
                textBox1.Text = string.Format("{0}", _Distance);
                textBox2.Text = string.Format("{0}", _Angle);
                winner = FindWinnerNumber(_Distance, _Angle);

                if (winner > -1)
                {
                    _WinnerNumber = winner;
                    textBox3.Text = string.Format("{0}", _WinnerNumber);
                }
            }
        }

        private bool drawBlob(NewFrameEventArgs args, PictureBox pb, ref System.Drawing.Point position)
        {
            bool found = false;
            Bitmap objectsImage = new Bitmap(args.Frame, _pbSize);

            //CopyRegionIntoImage(objectsImage, new Rectangle(197, 125, 315, 285), ref objectsImage, new Rectangle(new System.Drawing.Point(0, 0), new Size(398, 360)));
            //objectsImage = (Bitmap)Zoom(objectsImage, new Size(100, 100));

            Bitmap mImage = (Bitmap)objectsImage.Clone(); // args.Frame.Clone();
            Pen pen = new Pen(Color.FromArgb(iRedValue, iGreenValue, iBlueValue), 5);

            // Color centerColor = Color.LimeGreen; // 50, 205, 50
            _colorFilter.CenterColor = new RGB((byte)iRedValue, (byte)iGreenValue, (byte)iBlueValue);
            _colorFilter.Radius = (short)iRadius;
            pb.Image = _colorFilter.Apply(objectsImage);
            _colorFilter.ApplyInPlace(objectsImage);

            Rectangle area = new Rectangle(0, 0, objectsImage.Width, objectsImage.Height);

            BitmapData objectsData = objectsImage.LockBits(area, ImageLockMode.ReadOnly, objectsImage.PixelFormat);
            UnmanagedImage grayImage = Grayscale.CommonAlgorithms.BT709.Apply(new UnmanagedImage(objectsData));
//            pb.Image = grayImage.ToManagedImage();
            objectsImage.UnlockBits(objectsData);

            blobCounter.MinWidth = iMinWidth;
            blobCounter.MaxWidth = iMaxWidth;
            blobCounter.MinHeight = iMinHeight;
            blobCounter.MaxHeight = iMaxHeight;

            blobCounter.FilterBlobs = true;
            blobCounter.ObjectsOrder = ObjectsOrder.Size;
            blobCounter.ProcessImage(grayImage);


            Rectangle[] rects = blobCounter.GetObjectsRectangles();

            foreach (Rectangle recs in rects)
            {
                found = rects.Length > 0;
                if (found)
                {
                    Rectangle objectRect = rects[0];
                    position = objectRect.Location;

                    Graphics g = Graphics.FromImage(mImage);
                    g.DrawRectangle(pen, objectRect);
                    g.Dispose();
                    break;
                }
            }

            //pictureBox1.Image = mImage;
            if (_calibrateFlag)
                CalibrateCamera(mImage);

            args.Frame = mImage;
            return found;
        }

        #endregion
        private System.Drawing.Point[] ToPointsArray(List<IntPoint> points)
        {
            System.Drawing.Point[] array = new System.Drawing.Point[points.Count];

            for (int i = 0, n = points.Count; i < n; i++)
            {
                array[i] = new System.Drawing.Point(points[i].X, points[i].Y);
            }

            return array;
        }

        private System.Drawing.Image Zoom(System.Drawing.Image img, Size size)
        {
            Bitmap bmp = new Bitmap(img, img.Width + (img.Width * size.Width / 100), img.Height + (img.Height * size.Height / 100));
            Graphics g = Graphics.FromImage(bmp);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            return bmp;
        }

        private static void CopyRegionIntoImage(Bitmap srcBitmap, Rectangle srcRegion, ref Bitmap destBitmap, Rectangle destRegion)
        {
            using (Graphics grD = Graphics.FromImage(destBitmap))
            {
                grD.DrawImage(srcBitmap, destRegion, srcRegion, GraphicsUnit.Pixel);
            }
        }

        /**
        * Determines the angle of a straight line drawn between point one and two. The number returned, which is a float in degrees, tells us how much we have to rotate a horizontal line clockwise for it to match the line between the two points.
        * If you prefer to deal with angles using radians instead of degrees, just change the last line to: "return Math.Atan2(yDiff, xDiff);"
        */
        static double radian = 180.0F / (float)Math.PI;

        private static int GetAngleOfLineBetweenTwoPoints(System.Drawing.Point p1, System.Drawing.Point p2)
        {
            double xDiff = p2.X - p1.X;
            double yDiff = p2.Y - p1.Y;
            return (int)Math.Round(Math.Atan2(yDiff, xDiff) * radian);
        }

        private int FindDistance(System.Drawing.Point p1, System.Drawing.Point p2)
        {
            float distance = (float)Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));

            return (int)Math.Round(distance);
        }

        //Roulette wheel number sequence
        //The pockets of the roulette wheel are numbered from 0 to 36.
        //In number ranges from 1 to 10 and 19 to 28, odd numbers are red and even are black.
        //In ranges from 11 to 18 and 29 to 36, odd numbers are black and even are red.
        //There is a green pocket numbered 0 (zero). In American roulette, there is a second green pocket marked 00. 
        //Pocket number order on the roulette wheel adheres to the following clockwise sequence in most casinos

        //Single-zero wheel 
        //0-32-15-19-4-21-2-25-17-34-6-27-13-36-11-30-8-23-10-5-24-16-33-1-20-14-31-9-22-18-29-7-28-12-35-3-26
        //Double-zero wheel 
        //0-28-9-26-30-11-7-20-32-17-5-22-34-15-3-24-36-13-1-00-27-10-25-29-12-8-19-31-18-6-21-33-16-4-23-35-14-2
        //Triple-zero wheel 
        //0-000-00-32-15-19-4-21-2-25-17-34-6-27-13-36-11-30-8-23-10-5-24-16-33-1-20-14-31-9-22-18-29-7-28-12-35-3-26
        int[,] Numbers = new int[,]
        {
            // distance, angle = number
            {  24,  90 },// 0
            { 150, 113 },// 1
            {  81,  46 },// 2
            {  31, 130 },// 3
            {  60,  44 },// 4
            { 164,  95 },// 5
            { 125,  58 },// 6
            {  76, 143 },// 7
            { 160,  81 },// 8
            { 117, 129 },// 9
            { 165,  90 },// 10
            { 151,  73 },// 11
            {  53, 141 },// 12
            { 140,  65 },// 13
            { 135, 121 },// 14
            {  38,  50 },// 15
            { 160, 103 },// 16
            { 102,  51 },// 17
            {  98, 136 },// 18
            {  48,  45 },// 19
            { 142, 116 },// 20
            {  71,  44 },// 21
            { 107, 132 },// 22
            { 163,  86 },// 23
            { 161,  99 },// 24
            {  94,  48 },// 25
            {  26, 111 },// 26
            { 132,  61 },// 27
            {  63, 141 },// 28
            {  87, 138 },// 29
            { 155,  77 },// 30
            { 126, 124 },// 31
            {  28,  62 },// 32
            { 155, 108 },// 33
            { 115,  55 },// 34 
            {  45, 138 },// 35
            { 148,  68 } // 36
        };
        private int FindWinnerNumber(int distance, int angle)
        {
            int winner = -1;

            for (int i = 0; i < 37; i++)
            {
                if ((Math.Abs(Numbers[i, 0] - distance) < 3) &&
                    (Math.Abs(Numbers[i, 1] - angle) < 3))
                {
                    winner = i;
                    break;
                }
            }

            return winner;
        }
    }

}
