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
        bool _calibrateFlag = false, _blurFlag = false;
        int iThreshold = 40;

        // Bitmaps
        Bitmap _bitmapEdgeImage, _bitmapBinaryImage, _bitmapGreyImage, _bitmapBlurImage, _colorFilterImage;

        // Filters
        EuclideanColorFiltering _colorFilter = new EuclideanColorFiltering();
        SobelEdgeDetector _edgeFilter = new SobelEdgeDetector();
        BlobCounter blobCounter = new BlobCounter();

        // Drawing variables
        Pen _PictureboxPen = new Pen(Color.Black, 5);
        System.Drawing.Font _font = new System.Drawing.Font("Times New Roman", 48, FontStyle.Bold);
        System.Drawing.SolidBrush _brush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
        int ipenWidth = 5;

        // Base ruleta sin aro negro - radius 164 color red
        Rectangle rcMain = new Rectangle(120, 5, 410, 460);

        // Cilindro -incluye numeros - radius 204 color red
        Rectangle rcCylinder = new Rectangle(182, 64, 276, 330);

        // Casillas - radius 204 color blue
        Rectangle rcSlots = new Rectangle(225, 111, 198, 240);

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
            Bitmap _BsourceFrame = (Bitmap)eventArgs.Frame.Clone();
            //blobDetection(sender, eventArgs);
            pictureBox1.Image = BlobDetection(_BsourceFrame);
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
                //pictureBox2.Image = null;
            }
            catch (Exception)
            {
                return;
            }
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

            drawBlob(args, pictureBox1);

            //iRedValue = 255; // sbRedColor.Value;
            //iGreenValue = 254; // sbGreenColor.Value;
            //iBlueValue = 254; // sbBlueColor.Value;
            //iMinWidth = 6; iMaxWidth = 6;
            //iMinHeight = 8; iMaxHeight = 8;
            //iRadius = 180;

            //drawBlob(args, pictureBox2);

        }

        private void drawBlob(NewFrameEventArgs args, PictureBox pb)
        {
            Bitmap objectsImage = new Bitmap(args.Frame, new Size(1280, 720)); // 640, 360)); // 1280 * 720
            //objectsImage = (Bitmap)Zoom(objectsImage, new Size(100, 100));
            
            Bitmap mImage = (Bitmap)objectsImage.Clone(); // args.Frame.Clone();
            

            // Color centerColor = Color.LimeGreen; // 50, 205, 50
            _colorFilter.CenterColor = new RGB((byte)iRedValue, (byte)iGreenValue, (byte)iBlueValue);
            _colorFilter.Radius = (short)iRadius;
            _colorFilter.ApplyInPlace(objectsImage);

            textBox1.Text = string.Format("w: {0} - h: {1}", mImage.Width, mImage.Height);

            Rectangle area = new Rectangle(0, 0, objectsImage.Width, objectsImage.Height);

            BitmapData objectsData = objectsImage.LockBits(area, ImageLockMode.ReadOnly, objectsImage.PixelFormat);
            UnmanagedImage grayImage = Grayscale.CommonAlgorithms.BT709.Apply(new UnmanagedImage(objectsData));
            pb.Image = grayImage.ToManagedImage();


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
                    tbZeroPosX.Text = objectRect.Location.X.ToString();
                    tbZeroPosY.Text = objectRect.Location.Y.ToString();

                    Graphics g = Graphics.FromImage(mImage);
                    using (Pen pen = new Pen(Color.FromArgb(160, 255, 160), 5))
                    {
                        g.DrawRectangle(pen, objectRect);
                    }
                    g.Dispose();
                    break;
                }

            //pictureBox1.Image = mImage;
            if (_calibrateFlag)
                CalibrateCamera(mImage);

            args.Frame = mImage;
        }

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

        private Bitmap BlobDetection(Bitmap _bitmapSourceImage)
        {
            #region Color filtering by Euclidean filtering       

            _colorFilter.CenterColor = new RGB((byte)iRedValue, (byte)iGreenValue, (byte)iBlueValue);
            _colorFilter.Radius = (short)iRadius;
            _colorFilterImage = _colorFilter.Apply(_bitmapSourceImage);

            #endregion

            Grayscale _grayscale = new Grayscale(0.2125, 0.7154, 0.0721);
            _bitmapGreyImage = _grayscale.Apply(_colorFilterImage);

            #region blur option with Edge filter
            //create a edge detector instance
            if (_blurFlag == true)
            {
                //Blur _blurfilter = new Blur();
                GaussianBlur _blurfilter = new GaussianBlur(1.5);
                _bitmapBlurImage = _blurfilter.Apply(_bitmapGreyImage);
                _bitmapEdgeImage = _edgeFilter.Apply(_bitmapBlurImage);
            }
            else if (_blurFlag == false)
            {
                _bitmapEdgeImage = _edgeFilter.Apply(_bitmapGreyImage);
            }
            #endregion

            Threshold _threshold = new Threshold(iThreshold);
            _bitmapBinaryImage = _threshold.Apply(_bitmapEdgeImage);

            ///
            /// blob counter algorithm initialize. 
            /// BlobCounter.MinWidth and MinHeight -> for the defined minimum region
            ///
            BlobCounter _blobCounter = new BlobCounter();
            //Configure Filter
            _blobCounter.MinWidth = 70;
            _blobCounter.MinHeight = 70;
            _blobCounter.FilterBlobs = true;
            _blobCounter.ProcessImage(_bitmapBinaryImage);

            Blob[] _blobPoints = _blobCounter.GetObjectsInformation();
            Graphics _g = Graphics.FromImage(_bitmapSourceImage);
            SimpleShapeChecker _shapeChecker = new SimpleShapeChecker();

            for (int i = 0; i < _blobPoints.Length; i++)
            {
                List<IntPoint> _edgePoint = _blobCounter.GetBlobsEdgePoints(_blobPoints[i]);
                //List<IntPoint> _corners = null;
                Accord.Point _center;
                float _radius;

                #region detecting Circle
                ///
                /// _center: the center of circle
                /// _radius: the radius of circle
                ///
                if (_shapeChecker.IsCircle(_edgePoint, out _center, out _radius))
                {
                    //Drawing the reference point
                    _g.DrawEllipse(_PictureboxPen, (float)(pictureBox1.Size.Width),
                                                   (float)(pictureBox1.Size.Height),
                                                   (float)10, (float)10);

                    // Drawing setting for outline of detected object
                    _blobCounter.ObjectsOrder = ObjectsOrder.Size;
                    Rectangle[] _rects = _blobCounter.GetObjectsRectangles();
                    Pen _pen = new Pen(Color.Red, ipenWidth);
                    Pen _pengreen = new Pen(Color.GreenYellow, ipenWidth);
                    Pen _penyellow = new Pen(Color.GreenYellow, ipenWidth);

                    string _shapeString = "" + _shapeChecker.CheckShapeType(_edgePoint);
                    int _x = (int)_center.X;
                    int _y = (int)_center.Y;
                    ///
                    /// Drawing outline of detected object
                    ///
                    _g.DrawString(_shapeString, _font, _brush, _x, _y);
                    _g.DrawEllipse(_pen, (float)(_center.X - _radius),
                                         (float)(_center.Y - _radius),
                                         (float)(_radius * 2),
                                         (float)(_radius * 2));

                    //Drawing the centroid point of object
                    int _centroid_X = (int)_blobPoints[0].CenterOfGravity.X;
                    int _centroid_Y = (int)_blobPoints[0].CenterOfGravity.Y;
                    _g.DrawEllipse(_pen, (float)(_centroid_X), (float)(_centroid_Y), (float)10, (float)10);
                    //Degree calculation
                    int _deg_x = _centroid_X - pictureBox1.Size.Width;
                    int _deg_y = pictureBox1.Size.Height - _centroid_Y;

                    //textBox1.Text = ("Dis: (" + _deg_x + ", " + _deg_y + ")");
                    //                    Rectangle rc = new Rectangle(_centroid_X - (int)(_radius), _centroid_Y - (int)(_radius), (int)(_radius * 2), (int)(_radius * 2));

                    //// Base ruleta sin aro negro - radius 164 color red
                    //Rectangle rcMain = new Rectangle(120, 5, 410, 460);
                    //_g.DrawRectangle(_pengreen, rcMain);

                    ////// Cilindro -incluye numeros - radius 204 color red
                    //Rectangle rcCilinder = new Rectangle(174, 58, 258, 258);
                    //_g.DrawRectangle(_penyellow, rcMain);

                    //// Casillas - radius 204 color blue
                    //Rectangle rcSlots = new Rectangle(225, 111, 198,240);
                    //_g.DrawRectangle(_penyellow, rcSlots);

                    //size of rectange
                    foreach (Rectangle rc in _rects)
                    {
                        ///for debug
                        //System.Diagnostics.Debug.WriteLine(
                        //    string.Format("Circle size: ({0}, {1})", rc.Width, rc.Height));
                        //iFeatureWidth = rc.Width;
                        //double dis = FindDistance(iFeatureWidth);
                        //_g.DrawString(dis.ToString("N2") + "cm", _font, _brush, _x, _y + 60);
                        //textBox1.Text = "Center: (" + _centroid_X.ToString() + ", " + _centroid_X.ToString() +") - W: " + iFeatureWidth.ToString() + ")";
                        _g.DrawRectangle(_pen, rc);

                        textBox1.Text = string.Format("(X: {0}, Y: {1} W:{2} - H: {3})", rc.X, rc.Y, rc.Width, rc.Height);
                        break;
                    }
                }
                #endregion

            }
            return _bitmapSourceImage;
        }
        #endregion

        private double FindDistance(int _pixel)
        {
            ///
            /// distance(D): distance of object from the camera
            /// _focalLength(F): focal length of camera
            /// _pixel(P): apparent width in pixel
            /// _ObjectWidth(W): width of object
            /// 
            /// F = (P*D)/W
            ///     -> D = (W*F)/P
            ///
            double _distance;
            double _ObjectWidth = 10, _focalLength = 604.8;

            //_distance = Convert.ToInt16((_ObjectWidth * _focalLength) / _pixel);
            _distance = (_ObjectWidth * _focalLength) / _pixel;

            return _distance;
        }

        private void CopyRegionIntoImage(Bitmap srcBitmap, Rectangle srcRegion, ref Bitmap destBitmap, Rectangle destRegion)
        {
            using (Graphics grD = Graphics.FromImage(destBitmap))
            {
                grD.DrawImage(srcBitmap, destRegion, srcRegion, GraphicsUnit.Pixel);
            }
        }


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
