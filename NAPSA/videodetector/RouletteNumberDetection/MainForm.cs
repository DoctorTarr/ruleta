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
        // Color parameters
        int iRedValue = 30, iGreenValue = 220, iBlueValue = 30;
        int iMinWidth = 8, iMaxWidth = 10, iMinHeight = 8, iMaxHeight = 10, iRadius = 110;
        Color color = Color.LimeGreen;
        bool _calibrateFlag = false;
        System.Drawing.Point ZeroPos, BallPos;

        System.Drawing.Image imgOriginal;

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

        private void get_Frame(object sender, NewFrameEventArgs eventArgs)
        {
            blobDetection(sender, eventArgs);
            //Bitmap _BsourceFrame = (Bitmap)eventArgs.Frame.Clone();
            //pictureBox1.Image = BlobDetection(_BsourceFrame);
            //pictureBox2.Image = _bitmapEdgeImage;
            //pictureBox3.Image = _bitmapBinaryImage;
            //pictureBox4.Image = _colorFilterImage;

        }


        private void StartCameras()
        {
            try
            {
                StopCameras();
                // IP Camera
                MJPEGStream videoSource = new MJPEGStream("http://192.168.1.64/Streaming/Channels/1/httppreview");
                videoSource.Login = "admin";
                videoSource.Password = "Qwer1234";
                videoSourcePlayer1.VideoSource = videoSource;
                videoSourcePlayer1.NewFrameReceived += new Accord.Video.NewFrameEventHandler(get_Frame);

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

        private double FindDistance(System.Drawing.Point p1, System.Drawing.Point p2)
        {
            double distance = Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));

            return distance;
        }
        #endregion

        #region Blob Detection
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
        private void blobDetection(object sender, NewFrameEventArgs args)
        {
            iRedValue = 5; // sbRedColor.Value;
            iGreenValue = 240; // sbGreenColor.Value;
            iBlueValue = 5; // sbBlueColor.Value;
            iMinWidth = 10; iMaxWidth = 16;
            iMinHeight = 10; iMaxHeight = 16;
            iRadius = 180;

            drawBlob(args, pictureBox1, ref ZeroPos);
            tbZeroPosX.Text = ZeroPos.X.ToString();
            tbZeroPosY.Text = ZeroPos.Y.ToString();


            iRedValue = 240; // sbRedColor.Value;
            iGreenValue = 235; // sbGreenColor.Value;
            iBlueValue = 251; // sbBlueColor.Value;
            iMinWidth = 8; iMaxWidth = 10;
            iMinHeight = 8; iMaxHeight = 10;
            iRadius = 60;

            drawBlob(args, pictureBox2, ref BallPos);
            tbBolaPosX.Text = BallPos.X.ToString();
            tbBolaPosY.Text = BallPos.Y.ToString();
            textBox1.Text = string.Format("{0}", FindDistance(BallPos, ZeroPos));
        }

        private void drawBlob(NewFrameEventArgs args, PictureBox pb, ref System.Drawing.Point position)
        {
            Bitmap objectsImage = new Bitmap(args.Frame, new Size(640, 360));
            //objectsImage = (Bitmap)Zoom(objectsImage, new Size(100, 100));
            
            Bitmap mImage = (Bitmap)objectsImage.Clone(); // args.Frame.Clone();
            //Pen pen = new Pen(Color.FromArgb(160, 255, 160), 5);
            Pen pen = new Pen(Color.FromArgb(iRedValue, iGreenValue, iBlueValue), 5);

            // Color centerColor = Color.LimeGreen; // 50, 205, 50
            _colorFilter.CenterColor = new RGB((byte)iRedValue, (byte)iGreenValue, (byte)iBlueValue);
            _colorFilter.Radius = (short)iRadius;
            _colorFilter.ApplyInPlace(objectsImage);

            Rectangle area = new Rectangle(0, 0, objectsImage.Width, objectsImage.Height);

            BitmapData objectsData = objectsImage.LockBits(area, ImageLockMode.ReadOnly, objectsImage.PixelFormat);
            UnmanagedImage grayImage = Grayscale.CommonAlgorithms.BT709.Apply(new UnmanagedImage(objectsData));
            pb.Image = grayImage.ToManagedImage();
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
                if (rects.Length > 0)
                {
                    Rectangle objectRect = rects[0];
                    position = objectRect.Location;

                    Graphics g = Graphics.FromImage(mImage);
                    g.DrawRectangle(pen, objectRect);
                    g.Dispose();
                    break;
                }

            //pictureBox1.Image = mImage;
            if (_calibrateFlag)
                CalibrateCamera(mImage);

            args.Frame = mImage;
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

    }

}
