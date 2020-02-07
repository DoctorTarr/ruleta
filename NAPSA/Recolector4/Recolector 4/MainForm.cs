using Accord;
using Accord.Imaging;
using Accord.Imaging.Filters;
using Accord.Video;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Configuration;
using DASYS.Recolector.BLL;
using Accord.Vision.Motion;
using Microsoft.Win32;
using System.IO;
using Newtonsoft.Json;
using System.Threading;

namespace VideoRecolector
{
    public partial class MainForm : Form
    {
        // Blob detection references
        // http://www.aforgenet.com/framework/features/blobs_processing.html
        // https://www.codeproject.com/Articles/139628/Detect-and-Track-Objects-in-Live-Webcam-Video-Base

        // Camera variables
        private bool IsCameraOn = false;

        // Filter to resize image to get correct aspect ratio and get cylinder as a circle 
        private ResizeNearestNeighbor _resizeFilter = new ResizeNearestNeighbor(640, 400);
        // Mask the image around the roulette to avoid false positives
        Bitmap subtractImage = new Bitmap(640, 400, PixelFormat.Format24bppRgb);

        // Blob detection variables
        private EuclideanColorFiltering _zeroColorFilter = new EuclideanColorFiltering();
        private BlobCounter _zeroBlobCounter = new BlobCounter();

        private EuclideanColorFiltering _ballColorFilter = new EuclideanColorFiltering();
        private BlobCounter _ballBlobCounter = new BlobCounter();

        SobelEdgeDetector _edgeFilter = new SobelEdgeDetector();

        private Rectangle _frameArea; // Required to apply filters to the full frame
        private Rectangle _bowlArea; // Area where the ball rolls before getting into a pocket
        private Rectangle _numbersArea; // Area where all the numbers are
        private Rectangle _ballPocketsArea; // Cylinder area to detect ball presence
        private Rectangle _centerArea; // Center of the roulette area
        private System.Drawing.Point _centerPoint; // Center of the roulette area rectangle
        private Rectangle _zeroCenterArea; // Where the zero is at 12 am


        // Drawing variables
        //private Pen _drawPen;
        //System.Drawing.Font _font = new System.Drawing.Font("Times New Roman", 48, FontStyle.Bold);
        //System.Drawing.SolidBrush _brush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);


        // Positioning variables
        private System.Drawing.Point ZeroPos, ZeroPosToCenter, BallPos, BallPosToCenter;
        private int ZeroAngleToCenter = 0;

        // Measurement variables
        private int _Distance = 0, _Angle = 0;

        // Winner number variables
        private bool bZeroFound = false;
        private bool bDebouncedBallFound = false;
        private bool bBallStateChanged = false;
        private int iBallUnchangeCount = 0;

        private int _WinnerNumber = -1;

        private bool _calibrateFlag = false;

        // motion detection and processing algorithm
        MotionDetector detector = new MotionDetector(new TwoFramesDifferenceDetector()); 
        private bool _isMoving = false;
        //private float motionAlarmLevel = 0.003f;
        private int _rpm = 0;

        // Demo variables
        private int estadoDemo;
        private byte numeroDemo;
        private Random azarNumero;

        // Roulette Status variables
        //private int lastBallX = 0;
        //private int sentidoGiro = 0; // 0=horario, 1=antihorario
        private JuegoRuleta.ESTADO_JUEGO estadoMesa;
        private JuegoRuleta juego;


        public MainForm()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            setupDetectionVariables(); // Filter for blob detecting. Parameters setup in caller
            ReadNumbersTable();
            juego = new JuegoRuleta();
            estadoMesa = juego.GetCurrentState();
            this.tmrMain.Interval = 500; // msec
            this.tmrMain.Start();
            //MessageBox.Show(Application.ExecutablePath);
        }


        private void btnStartCamara_Click(object sender, EventArgs e)
        {

            if (this.IsCameraOn)
            {
                StopCamera();
                this.txtProtocolo.Text = "";
                this.btnStartCamara.Text = "Iniciar Captura";
                this.IsCameraOn = false;
                this.iBallUnchangeCount = RELEASE_MSEC / CHECK_MSEC;
                this.estadoMesa = JuegoRuleta.ESTADO_JUEGO.STATE_0;
            }
            else
            {
                StartCamera();
                this.IsCameraOn = true;
                this.btnStartCamara.Text = "Detener Captura";
                this.estadoMesa = JuegoRuleta.ESTADO_JUEGO.BEFORE_GAME;
            }
        }

        private void cbCalibrateCamera_CheckedChanged(object sender, EventArgs e)
        {
            if (cbCalibrateCamera.Checked)
            {
                this._calibrateFlag = true;
                this.btnAddToBoot.Visible = true;
                this.lblFPS.Visible = true;
            }
            else
            {
                this._calibrateFlag = false;
                this.btnAddToBoot.Visible = false;
                this.btnSaveNumTable.Visible = false;
                this.groupBox4.Visible = false;
                this.lblFPS.Visible = false;
            }
        }

        public void AddApplicationToStartup()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
            {
                key.SetValue(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, "\"" + Application.ExecutablePath + "\"");
                MessageBox.Show("Agregado al arranque");
            }
        }


        private void btnIniciarDemo_Click(object sender, EventArgs e)
        {
            if (this.tmrDemo.Enabled)
            {
                this.tmrDemo.Stop();
                this.txtProtocolo.Text = "";
                this.btnIniciarDemo.Text = "Iniciar Demo";
                btnStartCamara.Enabled = true;
            }
            else
            {
                if (this.IsCameraOn)
                {
                    btnStartCamara.PerformClick();
                    btnStartCamara.Enabled = false;
                }
                this.btnIniciarDemo.Text = "Detener Demo";
                this.estadoDemo = 0;
                //this.cantZerosFound = 0; // Reset spin counter
                this.tmrDemo.Interval = 1000;
                this.tmrDemo.Start();
            }
        }

        private void StartCamera()
        {
            try
            {
                StopCamera();
                // IP Camera
                MJPEGStream videoSource = new MJPEGStream("http://192.168.1.64/Streaming/Channels/1/preview");
                videoSource.Login = "admin";
                videoSource.Password = "Qwer1234";
                videoSourcePlayer1.VideoSource = videoSource;
                videoSourcePlayer1.NewFrameReceived += new Accord.Video.NewFrameEventHandler(get_Frame);

                videoSourcePlayer1.Start();
                tbVideoStatus.BackColor = Color.Red;
                tbVideoStatus.Text = "ON";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void StopCamera()
        {
            try
            {
                videoSourcePlayer1.SignalToStop();
                videoSourcePlayer1.WaitForStop();
                tbVideoStatus.BackColor = Color.DarkRed;
                tbVideoStatus.Text = "OFF";

                pbZero.Image = null;
                pbBall.Image = null;
                textBox1.Text = "";
                textBox2.Text = "";
                txtWinner.Text = "";
                textBox4.Text = "";
                tbBolaPosX.Text = "";
                tbBolaPosY.Text = "";
                tbZeroPosX.Text = "";
                tbZeroPosY.Text = "";
                lblBallOn.Text = "";
                bDebouncedBallFound = false;
                this._rpm = 0;
                _isMoving = false;
                this._WinnerNumber = -1;
                // reset motion detector
                if (detector != null)
                    detector.Reset();


            }
            catch (Exception)
            {
                return;
            }
        }

        private void CalibrateCamera(Bitmap _bitmapSourceImage)
        {
            int ipenWidth = 5;

            Graphics _g = Graphics.FromImage(_bitmapSourceImage);

            Pen _pengreen = new Pen(Color.LimeGreen, ipenWidth);
            Pen _penyellow = new Pen(Color.Yellow, ipenWidth);
            Pen _penred = new Pen(Color.Red, ipenWidth);
            Pen _penblue = new Pen(Color.Blue, ipenWidth);

            // Cilindro -incluye numeros - radius 204 color red
            _g.DrawRectangle(_pengreen, _bowlArea);

            _g.DrawRectangle(_penyellow, _numbersArea);

            // Casillas - radius 204 color blue
            _g.DrawRectangle(_penred, _ballPocketsArea);
            _g.DrawRectangle(_penred, _centerPoint.X, _centerPoint.Y, 1, 1);

            _g.DrawRectangle(_penblue, _zeroCenterArea);
        }


        #region Blob Detection
        // All the filters etc are configured here
        private void setupDetectionVariables()
        {
            // Configure Zero Color Filter
            RGB zeroColor = new RGB(Byte.Parse(ConfigurationManager.AppSettings["ZeroRed"].ToString()),
                                            Byte.Parse(ConfigurationManager.AppSettings["ZeroGreen"].ToString()),
                                            Byte.Parse(ConfigurationManager.AppSettings["ZeroBlue"].ToString()));
            _zeroColorFilter.CenterColor = zeroColor;
            _zeroColorFilter.Radius = short.Parse(ConfigurationManager.AppSettings["ZeroRadius"].ToString());

            // Configure Zero number blob detection parameters

            // If the property is equal to false, then there is no any additional
            //  post processing after image was processed.If the property is set to true, 
            //  then blobs filtering is done right after image processing routine. 
            // If BlobsFilter is set, then custom blobs' filtering is done, which is
            // implemented by user. Otherwise blobs are filtered according to dimensions
            // specified in MinWidth, MinHeight, MaxWidth and MaxHeight properties.
            _zeroBlobCounter.FilterBlobs = false;
            _zeroBlobCounter.ObjectsOrder = ObjectsOrder.Size;
            _zeroBlobCounter.MinWidth = int.Parse(ConfigurationManager.AppSettings["ZeroMinSize"].ToString());
            _zeroBlobCounter.MaxWidth = int.Parse(ConfigurationManager.AppSettings["ZeroMaxSize"].ToString());

            // Drawing pen for zero
            Pen zeroPen = new Pen(Color.FromArgb(zeroColor.Red, zeroColor.Green, zeroColor.Blue), 5);

            // Configure Ball Color Filter
            RGB ballColor = new RGB(Byte.Parse(ConfigurationManager.AppSettings["BallRed"].ToString()),
                                            Byte.Parse(ConfigurationManager.AppSettings["BallGreen"].ToString()),
                                            Byte.Parse(ConfigurationManager.AppSettings["BallBlue"].ToString()));

            _ballColorFilter.CenterColor = ballColor;
            _ballColorFilter.Radius = short.Parse(ConfigurationManager.AppSettings["BallRadius"].ToString());

            // Ball blob detection parameters
            _ballBlobCounter.FilterBlobs = false;
            _ballBlobCounter.ObjectsOrder = ObjectsOrder.Size;
            _ballBlobCounter.MinWidth = int.Parse(ConfigurationManager.AppSettings["BallMinSize"].ToString());
            _ballBlobCounter.MaxWidth = int.Parse(ConfigurationManager.AppSettings["BallMaxSize"].ToString());

            Pen ballPen = new Pen(Color.FromArgb(ballColor.Red, ballColor.Green, ballColor.Blue), 5);

            // Base ruleta sin aro negro - radius 164 color red
            _bowlArea = new Rectangle(160, 93, 307, 307);
            _numbersArea = new Rectangle(217, 150, 196, 196);
            _ballPocketsArea = new Rectangle(238, 171, 154, 154);
            _centerArea = new Rectangle(266, 198, 100, 100);
            _centerPoint = _centerArea.Center();
            _zeroCenterArea = new Rectangle(308, 148, 20, 25);
            

            using (Graphics graph = Graphics.FromImage(subtractImage))
            {
                Rectangle ImageSize = new Rectangle(0, 0, subtractImage.Width, subtractImage.Height);
                graph.FillRectangle(Brushes.White, ImageSize);
                graph.FillEllipse(Brushes.Black, _bowlArea);
                graph.FillEllipse(Brushes.White, _centerArea);
            }

            _frameArea = new Rectangle(0, 0, subtractImage.Width, subtractImage.Height);

            //for (int i = 0; i < 37; i++)
            //{
            //    this.comboBox1.Items.Add(i.ToString());
            //}
            //this.comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            //this.comboBox1.SelectedIndex = this.comboBox1.FindStringExact("0");

            this.bDebouncedBallFound = RawKeyPressed();
            this.bBallStateChanged = true;

        }

        const int CHECK_MSEC = 40; // Read hardware every 5 msec
        const int PRESS_MSEC = 200; // Stable time before registering pressed
        const int RELEASE_MSEC = 400; // Stable time before registering released

        // This function reads the key state from the hardware.
        bool RawKeyPressed()
        {
            return _ballPocketsArea.Contains(BallPos);
        }


        // Service routine called every CHECK_MSEC to
        // debounce both edges
        void DebounceSwitch1(ref bool Key_changed, ref bool Key_pressed)
        {
            bool DebouncedKeyPress = this.bDebouncedBallFound;
            bool RawState = RawKeyPressed();
            Key_changed = false;
            Key_pressed = DebouncedKeyPress;
            if (RawState == DebouncedKeyPress)
            {
                // Set the timer which allows a change from current state.
                if (DebouncedKeyPress) this.iBallUnchangeCount = RELEASE_MSEC / CHECK_MSEC;
                else this.iBallUnchangeCount = PRESS_MSEC / CHECK_MSEC;
            }
            else
            {
                // Key has changed - wait for new state to become stable.
                if (--this.iBallUnchangeCount == 0)
                {
                    // Timer expired - accept the change.
                    DebouncedKeyPress = RawState;
                    Key_changed = true;
                    Key_pressed = DebouncedKeyPress;
                    // And reset the timer.
                    if (DebouncedKeyPress) this.iBallUnchangeCount = RELEASE_MSEC / CHECK_MSEC;
                    else this.iBallUnchangeCount = PRESS_MSEC / CHECK_MSEC;
                }
            }
        }

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
        private void get_Frame(object sender, NewFrameEventArgs args)
        {
            lock (this)
            {
                int winner = -1;

                Bitmap _BsourceFrame = (Bitmap)args.Frame.Clone();
                //textBox4.Text = _BsourceFrame.Width.ToString();
                //textBox5.Text = _BsourceFrame.Height.ToString();

                this._rpm = (int)(1000.0f * detector.ProcessFrame(args.Frame));
                _isMoving = (this._rpm > 0);

                _BsourceFrame = _resizeFilter.Apply(_BsourceFrame); // new Bitmap(args.Frame, _pbSize);

                Subtract _subtractFilter = new Subtract(subtractImage);
                _subtractFilter.ApplyInPlace(_BsourceFrame);

                ZeroPos.X = -640;
                ZeroAngleToCenter = 0;
                pbZero.Image = ZeroBlobDetection(_BsourceFrame);
                tbZeroPosX.Text = ZeroPosToCenter.X.ToString();
                tbZeroPosY.Text = ZeroPosToCenter.Y.ToString();
                bZeroFound = ZeroPos.X != -640;
                if (bZeroFound)
                {
                    ZeroAngleToCenter = GetAngleOfPointToZero(ZeroPosToCenter);
                    tbZeroPosAngle.Text = ZeroAngleToCenter.ToString();
                }

                BallPos.X = -640;
                pbBall.Image = BallBlobDetection(_BsourceFrame);
                tbBolaPosX.Text = BallPosToCenter.X.ToString();
                tbBolaPosY.Text = BallPosToCenter.Y.ToString();
                
                //    Roulette slots
                // 		314, 175
                //241, 246        381,246
                //      314, 314
                DebounceSwitch1(ref this.bBallStateChanged, ref this.bDebouncedBallFound);
                _Angle = GetAngleOfPointToZero(BallPosToCenter);
                textBox2.Text = _Angle.ToString();
                if (this.bDebouncedBallFound)
                {
                    _Distance = FindDistance(ZeroPosToCenter, BallPosToCenter);
                    textBox1.Text = _Distance.ToString();

                }

                if (this.bBallStateChanged)
                {
                    lblBallOn.Text = this.bDebouncedBallFound ? "B " : "NB";
                    if(!this.bDebouncedBallFound)
                    {
                        winner = -1;
                        lblWinner.Text = "";
                    }
                }


                if (Math.Abs(ZeroAngleToCenter-90) < 2)
                {
                    //if (bZeroFound && bDebouncedBallFound)
                    {
                        winner = FindNumberByAngle(_Distance, _Angle);

                        if (winner > -1)
                        {
                            _WinnerNumber = winner;
                            juego.SetNewWinnerNumber(_WinnerNumber);
                            lblWinner.Text = string.Format("{0}", _WinnerNumber);
                        }
                        else
                            lblWinner.Text = "";

                    }
                }

                if (_calibrateFlag)
                    CalibrateCamera(_BsourceFrame);

                args.Frame = _BsourceFrame;
            }
        }

        // Zero position detection
        private Bitmap ZeroBlobDetection(Bitmap _bitmapSourceImage)
        {
            // Filter pixels with zero's color
            Bitmap _colorFilterImage = _zeroColorFilter.Apply(_bitmapSourceImage);

            // Apply grayscale filter
            BitmapData objectsData = _colorFilterImage.LockBits(_frameArea, ImageLockMode.ReadOnly, _colorFilterImage.PixelFormat);
            UnmanagedImage grayImage = Grayscale.CommonAlgorithms.BT709.Apply(new UnmanagedImage(objectsData));
            _zeroBlobCounter.ProcessImage(grayImage);
            _colorFilterImage.UnlockBits(objectsData);

            Rectangle[] rects = _zeroBlobCounter.GetObjectsRectangles();

            if (rects.Length > 0)
            {
                Rectangle objectRect = rects[0];
                ZeroPos = objectRect.Center();
                ZeroPosToCenter.X = ZeroPos.X - _centerPoint.X;
                ZeroPosToCenter.Y = _centerPoint.Y - ZeroPos.Y;
            }
            return _colorFilterImage;
        }


        // Ball position detection
        private Bitmap BallBlobDetection(Bitmap _bitmapSourceImage)
        {
            // Filter pixels with balls's color
            Bitmap _colorFilterImage = _ballColorFilter.Apply(_bitmapSourceImage);

            // Apply grayscale filter
            BitmapData objectsData = _colorFilterImage.LockBits(_frameArea, ImageLockMode.ReadOnly, _colorFilterImage.PixelFormat);
            UnmanagedImage grayImage = Grayscale.CommonAlgorithms.BT709.Apply(new UnmanagedImage(objectsData));
            _ballBlobCounter.ProcessImage(grayImage);
            _colorFilterImage.UnlockBits(objectsData);

            Rectangle[] rects = _ballBlobCounter.GetObjectsRectangles();

            if (rects.Length > 0)
            {
                Rectangle objectRect = rects[0];
                BallPos = objectRect.Center();
                BallPosToCenter.X = BallPos.X - _centerPoint.X;
                BallPosToCenter.Y = _centerPoint.Y - BallPos.Y;
            }

            return _colorFilterImage;
        }

        //private System.Drawing.Point[] ToPointsArray(List<IntPoint> points)
        //{
        //    System.Drawing.Point[] array = new System.Drawing.Point[points.Count];

        //    for (int i = 0, n = points.Count; i < n; i++)
        //    {
        //        array[i] = new System.Drawing.Point(points[i].X, points[i].Y);
        //    }

        //    return array;
        //}

        #endregion

        #region Finding Winner Number
        // Atan2 aproximation1 2x times faster thatn Math.Atan2
        double atan2_approximation1(double y, double x)
        {
            //http://pubs.opengroup.org/onlinepubs/009695399/functions/atan2.html
            //https://gist.github.com/volkansalma/2972237
            //Volkan SALMA

            const double ONEQTR_PI = (Math.PI / 4.0);
            const double THRQTR_PI = (3.0 * Math.PI / 4.0);
            double r, angle;
            double abs_y = Math.Abs(y) + 1e-10f;      // kludge to prevent 0/0 condition
            if (x < 0.0f)
            {
                r = (x + abs_y) / (abs_y - x);
                angle = THRQTR_PI;
            }
            else
            {
                r = (x - abs_y) / (x + abs_y);
                angle = ONEQTR_PI;
            }
            angle += (0.1963f * r * r - 0.9817f) * r;
            if (y < 0.0f)
                return (-angle);     // negate if in quad III or IV
            else
                return (angle);
        }

        /**
        * Determines the angle of a straight line drawn between point one and two. The number returned, which is a float in degrees, tells us how much we have to rotate a horizontal line clockwise for it to match the line between the two points.
        * If you prefer to deal with angles using radians instead of degrees, just change the last line to: "return Math.Atan2(yDiff, xDiff);"
        */
        private int GetAngleOfLineBetweenTwoPoints(System.Drawing.Point p1, System.Drawing.Point p2)
        {
            double xDiff = p2.X - p1.X;
            double yDiff = p1.Y - p2.Y;
            return (int)Math.Round(atan2_approximation1(yDiff, xDiff) * this.radian);
        }

        /**
        * Determines the angle of a point against the coordinates center
        */
        private int GetAngleOfPointToZero(System.Drawing.Point p)
        {
            return (int)Math.Round(atan2_approximation1(p.Y, p.X) * this.radian);
        }

        /**
         * Work out the angle from the x horizontal winding anti-clockwise 
         * in screen space. 
         * 
         * The value returned from the following should be 315. 
         * <pre>
         * x,y -------------
         *     |  1,1
         *     |    \
         *     |     \
         *     |     2,2
         * </pre>
         * @param p1
         * @param p2
         * @return - a double from 0 to 360
         */
        private int angleOf(System.Drawing.Point p1, System.Drawing.Point p2)
        {
            // NOTE: Remember that most math has the Y axis as positive above the X.
            // However, for screens we have Y as positive below. For this reason, 
            // the Y values are inverted to get the expected results.
            double deltaY = (p1.Y - p2.Y);
            double deltaX = (p2.X - p1.X);
            double result = Math.Atan2(deltaY, deltaX) * this.radian;
            return (int)((result < 0) ? (360d + result) : result);
        }

        // Finds the integer square root of a positive number  
        private int Isqrt(int num)
        {
            if (0 == num) { return 0; }  // Avoid zero divide  
            int n = (num / 2) + 1;       // Initial estimate, never low  
            int n1 = (n + (num / n)) / 2;
            while (n1 < n)
            {
                n = n1;
                n1 = (n + (num / n)) / 2;
            } // end while  
            return n;
        } // end Isqrt()  

        // Find distance between zero and ball
        private int FindDistance(System.Drawing.Point p1, System.Drawing.Point p2)
        {
            return Isqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
        }



        //Roulette wheel number sequence
        //The pockets of the roulette wheel are numbered from 0 to 36.
        //In number ranges from 1 to 10 and 19 to 28, odd numbers are red and even are black.
        //In ranges from 11 to 18 and 29 to 36, odd numbers are black and even are red.
        //There is a green pocket numbered 0 (zero). In American roulette, there is a second green pocket marked 00. 
        //Pocket number order on the roulette wheel adheres to the following clockwise sequence in most casinos

        //Single-zero wheel 
        //0-32-15-19-4-21-2-25-17-34-6-27-13-36-11-30-8-23-10-5-24-16-33-1-20-14-31-9-22-18-29-7-28-12-35-3-26
        private int[,] RouletteNumbers =
        {
           { 0, 2},
           {32, 1},
           {15, 0},
           {19, 1},
           { 4, 0},
           {21, 1},
           { 2, 0},
           {25, 1},
           {17, 0},
           {34, 1},
           { 6, 0},
           {27, 1},
           {13, 0},
           {36, 1},
           {11, 0},
           {30, 1},
           { 8, 0},
           {23, 1},
           {10, 0},
           { 5, 1},
           {24, 0},
           {16, 1},
           {33, 0},
           { 1, 1},
           {20, 0},
           {14, 1},
           {31, 0},
           { 9, 1},
           {22, 0},
           {18, 1},
           {29, 0},
           { 7, 1},
           {28, 0},
           {12, 1},
           {35, 0},
           { 3, 1},
           {26, 0},
        };

        //Double-zero wheel 
        //0-28-9-26-30-11-7-20-32-17-5-22-34-15-3-24-36-13-1-00-27-10-25-29-12-8-19-31-18-6-21-33-16-4-23-35-14-2
        //Triple-zero wheel 
        //0-000-00-32-15-19-4-21-2-25-17-34-6-27-13-36-11-30-8-23-10-5-24-16-33-1-20-14-31-9-22-18-29-7-28-12-35-3-26
        private int[,] NumbersByAngle;

        private readonly double radian = 180.0F / (float)Math.PI;

        public bool CalibrateFlag { get => _calibrateFlag; set => _calibrateFlag = value; }

        private int FindNumberByAngle(int distance, int angle)
        {
            int winner = -1;

            for (int i = 0; i < 37; i++)
            {
                if ((Math.Abs(NumbersByAngle[i, 0] - distance) < 3) &&
                    (Math.Abs(NumbersByAngle[i, 1] - angle) < 3))
                {
                    winner = i;
                    break;
                }
            }

            return winner;
        }
        #endregion

        #region Game state handling
        private void LeerUltimoNumero()
        {
            DateTime now = DateTime.Now;
            do
            {
                try
                {
                    Application.DoEvents();
                    Pase.UltimoPase = Pase.LeerUltimoNumeroDesdeBase();
                    return;
                }
                catch
                {
                    Common.Logger.Escribir("La conexión a la base de datos ha fallado al iniciar.", true);
                }
            }
            while (!(now.AddMinutes(1.0) < DateTime.Now));
        }

        private void btnSaveNumTable_Click(object sender, EventArgs e)
        {
            WriteNumbersTable();
        }

        private void btnCalibrateNumber_Click(object sender, EventArgs e)
        {
            int acumDist = 0;
            int acumAngle = 0;
            int averageDist = 0;
            int averageAngle = 0;
            int count = 0;

            for (int i = 0; i < 100; i++)
            {
                Application.DoEvents();
                count++;
                acumDist += this._Distance;
                averageDist = acumDist / count;
                this.tbAvgDist.Text = averageDist.ToString();

                acumAngle += this._Angle;
                averageAngle = acumAngle / count;
                this.tbAvgAngle.Text = averageAngle.ToString();
                this.lblTestCount.Text = count.ToString();
            }
        }

        private void btnSetNumber_Click(object sender, EventArgs e)
        {
            int distance = int.Parse(this.tbAvgDist.Text);
            int angle = int.Parse(this.tbAvgAngle.Text);
            //NumbersByAngle[this.comboBox1.SelectedIndex, 0] = distance;
            //NumbersByAngle[this.comboBox1.SelectedIndex, 1] = angle;
        }

        private void GuardarEstado(int estado, int numero, int rpm, int sentidoDeGiro)
        {
            string cadena = string.Empty;
            switch (estado)
            {
                case 1:
                    //cadena = "NS" + this.numeroDemo.ToString("00") + "1" + this.azarNumero.Next(0, 100).ToString("00") + this.azarNumero.Next(0, 2).ToString() + "0";
                    cadena = ProtocoloNAPSA.FormatearCadenaEstado(numero,
                                                            (int)JuegoRuleta.ESTADO_JUEGO.BEFORE_GAME,
                                                             rpm, sentidoDeGiro, 0);
                    break;
                case 2:
                    //cadena = "NS" + this.numeroDemo.ToString("00") + "2" + this.azarNumero.Next(0, 100).ToString("00") + this.azarNumero.Next(0, 2).ToString() + "0";
                    cadena = ProtocoloNAPSA.FormatearCadenaEstado(numero,
                                                            (int)JuegoRuleta.ESTADO_JUEGO.PLACE_YOUR_BETS,
                                                            rpm, sentidoDeGiro, 0);
                    break;
                case 3:
                    //cadena = "NS" + this.numeroDemo.ToString("00") + "3" + this.azarNumero.Next(0, 100).ToString("00") + this.azarNumero.Next(0, 2).ToString() + "0";
                    cadena = ProtocoloNAPSA.FormatearCadenaEstado(numero,
                                                            (int)JuegoRuleta.ESTADO_JUEGO.FINISH_BETTING,
                                                            rpm, sentidoDeGiro, 0);
                    break;
                case 4:
                    //                        cadena = "NS" + this.numeroDemo.ToString("00") + "4" + this.azarNumero.Next(0, 100).ToString("00") + this.azarNumero.Next(0, 2).ToString() + "0";
                    cadena = ProtocoloNAPSA.FormatearCadenaEstado(numero,
                                                            (int)JuegoRuleta.ESTADO_JUEGO.NO_MORE_BETS,
                                                            this._rpm, sentidoDeGiro, 0);
                    break;
                case 5:
                    //Persistencia.Guardar("NS" + this.numeroDemo.ToString("00") + "5" + this.azarNumero.Next(0, 100).ToString("00") + this.azarNumero.Next(0, 2).ToString() + "0");
                    cadena = ProtocoloNAPSA.FormatearCadenaEstado(numero,
                                                            (int)JuegoRuleta.ESTADO_JUEGO.WINNING_NUMBER,
                                                            this._rpm, sentidoDeGiro, 0);
                    break;
                case 6:
                    cadena = ProtocoloNAPSA.FormatearCadenaMesaCerrada();
                    break;
            }
            if (cadena != string.Empty)
            {
                Persistencia.Guardar(cadena);
                txtProtocolo.AppendText(cadena);
                txtProtocolo.AppendText(Environment.NewLine);
            }
        }

        private void cbCalibrateNumbers_CheckedChanged(object sender, EventArgs e)
        {
            if (cbCalibrateNumbers.Checked)
            {
                this.btnSaveNumTable.Visible = true;
                this.groupBox4.Visible = true;
            }
            else
            {
                this.btnSaveNumTable.Visible = false;
                this.groupBox4.Visible = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.AddApplicationToStartup();
        }

        private void GuardarNumeroGanador(int numero)
        {
            string cadena = ProtocoloNAPSA.FormatearCadenaNumeroGanador(numero);
            Persistencia.Guardar(cadena);
            txtProtocolo.AppendText(cadena);
            txtProtocolo.AppendText(Environment.NewLine);
        }
        #endregion

        private void Cerrar()
        {
            try
            {
                StopCamera();
                Common.Logger.EscribirLinea();
                Common.Logger.Escribir("*** RECOLECTOR FINALIZADO ***", true);
                Common.Logger.EscribirLinea();
                this.Dispose();
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show(ex.Message);
            }
        }

        private void ReadNumbersTable()
        {
            using (var stream = new StreamReader(@"./data.json"))
            {
                this.NumbersByAngle = JsonConvert.DeserializeObject<int[,]>(stream.ReadToEnd());
            }
        }

        private void WriteNumbersTable()
        {
            // write the data (overwrites)
            using (var stream = new StreamWriter(@"./data.json", append: false))
            {
                stream.Write(JsonConvert.SerializeObject(this.NumbersByAngle));
            }
        }

    }
}
