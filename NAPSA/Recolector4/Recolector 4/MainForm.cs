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
using System.Linq;
using System.Diagnostics;

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
        private Rectangle _zeroNumberArea; // Where the zero is at 12 am


        // Drawing variables
        //private Pen _drawPen;
        //System.Drawing.Font _font = new System.Drawing.Font("Times New Roman", 48, FontStyle.Bold);
        //System.Drawing.SolidBrush _brush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);


        // Positioning variables
        private System.Drawing.Point ZeroPos, ZeroPosToCenter, BallPos, BallPosToCenter;
        private int _ZeroAngleToCenter = 0;

        private bool bZeroFound = false;
        private bool bBallFound = false;
        private bool bZeroFoundAt12 = false;

        // Measurement variables
        private int _DistanceZeroBall = 0, _BallAngleToCenter = 0;

        private bool bShowText = false;

        // Winner number variables
        private bool bDebouncedBallFound = false;
        private bool bBallStateChanged = false;
        private int iBallUnchangeCount = 0;

        private int _WinnerNumber = -1;

        //private int _DetectionMethod = 0; // 0 == Distance/Angle
                                          // 1 = X/Y

        private bool _IsCalibratingCamera = false;
        private bool _IsCalibratingNumbers = false;
        private bool _LogDetectedNumbers = false;


        // motion detection and processing algorithm
        MotionDetector detector = new MotionDetector(new TwoFramesDifferenceDetector()); 
        private bool _isMoving = false;
        //private float motionAlarmLevel = 0.003f;
        private int _rpm = 0;
        private int _rpmCounter = 0;
        private float movePercentage = 0.0f;

        // Demo variables
        private int estadoDemo;
        private byte numeroDemo;
        private Random azarNumero;

        // Roulette Status variables
        //private int lastBallX = 0;
        //private int sentidoGiro = 0; // 0=horario, 1=antihorario
        private JuegoRuleta.ESTADO_JUEGO estadoMesa;
        private JuegoRuleta juego;

        // Number of numbers calibrated
        private int numCalibrated = 0;

        // For calibration
        private int acumDist = 0;
        private int countDistance = 0;
        private int averageDist = 0;

        private int acumAngle = 0;
        private int countAngle = 0;
        private int averageAngle = 0;

        private int acumX = 0;
        private int countX = 0;
        private int averageX = 0;

        private int acumY = 0;
        private int countY = 0;
        private int averageY = 0;

        private int countCalibrationSamples = 0;
        private bool CalibrationInProgress = false;

        public MainForm()
        {
            InitializeComponent();
            if (!Producto.VerificarActivacion())
            {
                MessageBox.Show("Producto No Activado. Por favor, actívelo", "PRODUCTO NO ACTIVADO",
                                 MessageBoxButtons.OK, MessageBoxIcon.Error);
                Form Form2 = new FrmActivador();
                Form2.ShowDialog();
                System.Environment.Exit(0);
            }
            this.bBallStateChanged = false;
            CheckForIllegalCrossThreadCalls = true;
            setupDetectionVariables(); // Filter for blob detecting. Parameters setup in caller
            ReadNumbersTable();
            juego = new JuegoRuleta();
            estadoMesa = juego.GetCurrentState();
            this.tmrMain.Interval = 500; // msec
            this.tmrMain.Start();
        }


        private void btnStartCamara_Click(object sender, EventArgs e)
        {

            if (this.IsCameraOn)
            {
                StopCamera();
                this.txtProtocolo.Text = "";
                this.btnStartCamara.Text = "Iniciar Captura";
                this.IsCameraOn = false;
                this.iBallUnchangeCount = BALL_NOT_FOUND_MSEC / CHECK_MSEC;
                this.bBallStateChanged = false;
                this.estadoMesa = JuegoRuleta.ESTADO_JUEGO.STATE_0;
            }
            else
            {
                StartCamera();
                this.iBallUnchangeCount = BALL_NOT_FOUND_MSEC / CHECK_MSEC;
                this.bBallStateChanged = false;
                this.IsCameraOn = true;
                this.btnStartCamara.Text = "Detener Captura";
                this.estadoMesa = JuegoRuleta.ESTADO_JUEGO.BEFORE_GAME;
            }
        }

        private void cbCalibrateCamera_CheckedChanged(object sender, EventArgs e)
        {
            if (cbCalibrateCamera.Checked)
            {
                IsCalibratingCamera = true;
                this.lblFPS.Visible = true;
            }
            else
            {
                IsCalibratingCamera = false;
                this.pnlCalibration.Visible = false;
                this.lblFPS.Visible = false;
            }
        }


        private void StartCamera()
        {
            try
            {
                StopCamera();
                // IP Camera
                //MJPEGStream videoSource = new MJPEGStream("http://192.168.0.205:8080/video");
                MJPEGStream videoSource = new MJPEGStream("http://192.168.1.64/Streaming/Channels/1/preview");
                videoSource.Login = "admin";
                videoSource.Password = "Qwer1234";
                videoSourcePlayer1.VideoSource = videoSource;
                videoSourcePlayer1.NewFrameReceived += new Accord.Video.NewFrameEventHandler(get_Frame);
                videoSourcePlayer1.VideoSource.VideoSourceError += new VideoSourceErrorEventHandler(videoSourcePlayer1_VideoSourceError);

                videoSourcePlayer1.Start();
                lblVideoStatus.BackColor = Color.Red;
                lblVideoStatus.Text = "ON";
                this.iBallUnchangeCount = BALL_NOT_FOUND_MSEC / CHECK_MSEC;
                this.bDebouncedBallFound = RawBallFound();
                this.bBallStateChanged = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void videoSourcePlayer1_VideoSourceError(object sender, VideoSourceErrorEventArgs eventArgs)
        {
            //Common.Logger.Escribir("*** Error en Cerrar() ***" + eventArgs.Exception.ToString(), true);
        }

        private void StopCamera()
        {
            try
            {
                videoSourcePlayer1.SignalToStop();
                videoSourcePlayer1.WaitForStop();
                lblVideoStatus.BackColor = Color.DarkRed;
                lblVideoStatus.Text = "OFF";

                pbZero.Image = null;
                pbBall.Image = null;
                lblDisplayWinner.Text = "";
                lblDisplayWinner.ForeColor = Color.Black;
                lblEstadoRuleta.Text = "";
                lblBolaPosX.Text = "";
                lblBolaPosY.Text = "";
                lblZeroPosX.Text = "";
                lblZeroPosY.Text = "";
                lblBallOn.Text = "";
                this.bDebouncedBallFound = false;
                this.bBallStateChanged = true;
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
            Pen _penviolet = new Pen(Color.Black, ipenWidth);
            Pen _penred = new Pen(Color.Red, ipenWidth);
            Pen _penblue = new Pen(Color.Blue, ipenWidth);

            // Cilindro -incluye numeros - radius 204 color red
            _g.DrawEllipse(_pengreen, _bowlArea);

            _g.DrawEllipse(_penviolet, _numbersArea);

            // Casillas - radius 204 color blue
            _g.DrawEllipse(_penred, _ballPocketsArea);
            _g.DrawRectangle(_penred, _centerPoint.X, _centerPoint.Y, 1, 1);

            _g.DrawRectangle(_penblue, _zeroNumberArea);
        }

        #region app config access
        private static string GetSetting(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        private static void SetSetting(string key, string value)
        {
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configuration.AppSettings.Settings[key].Value = value;
            configuration.Save(ConfigurationSaveMode.Full, true);
            ConfigurationManager.RefreshSection("appSettings");
        }

        #endregion

        #region Blob Detection


        // All the filters etc are configured here
        private void setupDetectionVariables()
        {
            // Configure Zero Color Filter
            RGB zeroColor = new RGB(Byte.Parse(GetSetting("ZeroRed")),
                                    Byte.Parse(GetSetting("ZeroGreen")),
                                    Byte.Parse(GetSetting("ZeroBlue")));
            _zeroColorFilter.CenterColor = zeroColor;
            _zeroColorFilter.Radius = short.Parse(GetSetting("ZeroRadius"));

            // Configure Zero number blob detection parameters

            // If the property is equal to false, then there is no any additional
            //  post processing after image was processed.If the property is set to true, 
            //  then blobs filtering is done right after image processing routine. 
            // If BlobsFilter is set, then custom blobs' filtering is done, which is
            // implemented by user. Otherwise blobs are filtered according to dimensions
            // specified in MinWidth, MinHeight, MaxWidth and MaxHeight properties.
            _zeroBlobCounter.FilterBlobs = false;
            _zeroBlobCounter.ObjectsOrder = ObjectsOrder.Size;
            _zeroBlobCounter.MinWidth = int.Parse(GetSetting("ZeroMinSize"));
            _zeroBlobCounter.MaxWidth = int.Parse(GetSetting("ZeroMaxSize"));

            // Drawing pen for zero
            Pen zeroPen = new Pen(Color.FromArgb(zeroColor.Red, zeroColor.Green, zeroColor.Blue), 5);

            // Configure Ball Color Filter
            RGB ballColor = new RGB(Byte.Parse(GetSetting("BallRed")),
                                    Byte.Parse(GetSetting("BallGreen")),
                                    Byte.Parse(GetSetting("BallBlue")));

            _ballColorFilter.CenterColor = ballColor;
            _ballColorFilter.Radius = short.Parse(GetSetting("BallRadius"));

            // Ball blob detection parameters
            _ballBlobCounter.FilterBlobs = false;
            _ballBlobCounter.ObjectsOrder = ObjectsOrder.Size;
            _ballBlobCounter.MinWidth = int.Parse(GetSetting("BallMinSize"));
            _ballBlobCounter.MaxWidth = int.Parse(GetSetting("BallMaxSize"));

            Pen ballPen = new Pen(Color.FromArgb(ballColor.Red, ballColor.Green, ballColor.Blue), 5);

            // Base ruleta sin aro negro - radius 164 color red
            _bowlArea = new Rectangle(160, 93, 307, 307);
            _numbersArea = new Rectangle(217, 150, 196, 196);
            _ballPocketsArea = new Rectangle(238, 171, 154, 154);
            _centerArea = new Rectangle(266, 198, 100, 100);
            _centerPoint = _centerArea.Center();
            _zeroNumberArea = new Rectangle(_centerPoint.X-7, 148, 14, 25);
            

            using (Graphics graph = Graphics.FromImage(subtractImage))
            {
                Rectangle ImageSize = new Rectangle(0, 0, subtractImage.Width, subtractImage.Height);
                graph.FillRectangle(Brushes.White, ImageSize);
                graph.FillEllipse(Brushes.Black, _bowlArea);
                graph.FillEllipse(Brushes.White, _centerArea);
            }

            _frameArea = new Rectangle(0, 0, subtractImage.Width, subtractImage.Height);

            for (int i = 0; i < 37; i++)
            {
                this.comboBox1.Items.Add(RouletteNumbers[i,0].ToString());
            }
            this.comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBox1.SelectedIndex = this.comboBox1.FindStringExact("0");


        }

        const int CHECK_MSEC = 40; // Read hardware every 5 msec
        const int BALL_FOUND_MSEC = 400; // Stable time before registering pressed
        const int BALL_NOT_FOUND_MSEC = 800; // Stable time before registering released

        // This function reads the key state from the hardware.
        bool RawBallFound()
        {
            return _ballPocketsArea.Contains(BallPos);
        }


        // Service routine called every CHECK_MSEC to
        // debounce both edges
        void DebounceBallInSlot()
        {
            this.bBallStateChanged = false;
            bool RawState = RawBallFound();
            if (RawState == this.bDebouncedBallFound)
            {
                // Set the timer which allows a change from current state.
                if (this.bDebouncedBallFound) this.iBallUnchangeCount = BALL_NOT_FOUND_MSEC / CHECK_MSEC;
                else this.iBallUnchangeCount = BALL_FOUND_MSEC / CHECK_MSEC;
            }
            else
            {
                // Key has changed - wait for new state to become stable.
                if (--this.iBallUnchangeCount == 0)
                {
                    // Timer expired - accept the change.
                    this.bDebouncedBallFound = RawState;
                    this.bBallStateChanged = true;
                    // And reset the timer.
                    if (this.bDebouncedBallFound) this.iBallUnchangeCount = BALL_NOT_FOUND_MSEC / CHECK_MSEC;
                    else this.iBallUnchangeCount = BALL_FOUND_MSEC / CHECK_MSEC;
                }
            }
        }

        private void get_Frame(object sender, NewFrameEventArgs args)
        {
            lock (this)
            {
                int winnerDA = -1;
                int winnerXY = -1;
                int winner = -1;

                //Stopwatch stopWatch = new Stopwatch();
                //stopWatch.Start();

                Bitmap _BsourceFrame = (Bitmap)args.Frame.Clone();
                _BsourceFrame = _resizeFilter.Apply(_BsourceFrame); // new Bitmap(args.Frame, _pbSize);
                Subtract _subtractFilter = new Subtract(subtractImage);
                _subtractFilter.ApplyInPlace(_BsourceFrame);

                ZeroPos = System.Drawing.Point.Empty;
                ZeroPosToCenter = ZeroPos;
                BallPos = System.Drawing.Point.Empty;
                BallPosToCenter = BallPos;

                pbZero.Image = ZeroBlobDetection(_BsourceFrame);
                pbBall.Image = BallBlobDetection(_BsourceFrame);
                this.movePercentage = detector.ProcessFrame(_BsourceFrame);
                _isMoving = (this.movePercentage > 0.01f);


                //    Roulette slots
                // 		314, 175
                //241, 246        381,246
                //      314, 314
                DebounceBallInSlot();
                if ((this.bZeroFoundAt12) && (this.bDebouncedBallFound))
                {
                    winnerXY = FindNumberByXY(this.BallPosToCenter.X, this.BallPosToCenter.Y);
                    this._DistanceZeroBall = FindDistance(this.ZeroPosToCenter, this.BallPosToCenter);
                    this._BallAngleToCenter = GetAngleOfPointToZero(this.BallPosToCenter);
                    winnerDA = FindNumberByAngle(this._DistanceZeroBall, this._BallAngleToCenter);
                    //winnerXY = winnerDA;
                    if ((winnerDA != -1) && (winnerXY == winnerDA))
                    {
                        winner = winnerDA;
                        _WinnerNumber = winner;
                        juego.SetNewWinnerNumber(_WinnerNumber);
                        //lblFound.Text = string.Format("{0}", _WinnerNumber);
                    }
                }
                else
                {
                    winner = -1;
                    //lblFound.Text = "--";
                }

                if (this._isMoving)
                {
                    if (bZeroFoundAt12)
                    {
                        if (this._rpmCounter > 0)
                        {
                            this._rpm = 1500 / this._rpmCounter;
                            if (this._rpm > 60)
                                this._rpm = 60;
                            this._rpmCounter = 0;
                        }

                    }
                    else
                    {
                       this._rpmCounter++;
                    }
                }
                else
                {
                    this._rpmCounter = 0;
                    this._rpm = 0;
                }

                //stopWatch.Stop();

                //if (stopWatch.ElapsedMilliseconds > 40)
                //{
                //    MessageBox.Show($"Exceeded: {stopWatch.ElapsedMilliseconds.ToString()}");
                //}
                ////                lblZeroBlobDetectionTime.Text = stopWatch.ElapsedMilliseconds.ToString();
                args.Frame = _BsourceFrame;
            }
        }



        //private void get_Frame_Calibration(object sender, NewFrameEventArgs args)
        //{
        //    lock (this)
        //    {
        //        int winnerDA = -1;
        //        int winnerXY = -1;
        //        int winner = -1;
        //        bool bZeroFound = false;
        //        bool bBallFound = false;
        //        bool bZeroFoundAt12 = false;
        //        bool bShowText = (this.IsCalibratingCamera || this.IsCalibratingNumbers);


        //        //Stopwatch stopWatch = new Stopwatch();
        //        //stopWatch.Start();

        //        Bitmap _BsourceFrame = (Bitmap)args.Frame.Clone();
        //        _BsourceFrame = _resizeFilter.Apply(_BsourceFrame); // new Bitmap(args.Frame, _pbSize);
        //        Subtract _subtractFilter = new Subtract(subtractImage);
        //        _subtractFilter.ApplyInPlace(_BsourceFrame);

        //        this.movePercentage = detector.ProcessFrame(_BsourceFrame);
        //        _isMoving = (this.movePercentage > 0.01f);
        //        if (!this._isMoving)
        //        {
        //            this._rpmCounter = 0;
        //            this._rpm = 0;
        //        }

        //        ZeroPos.X = -640;
        //        _ZeroAngleToCenter = 720;
        //        pbZero.Image = ZeroBlobDetection(_BsourceFrame);
        //        bZeroFound = ZeroPos.X != -640;
        //        if (bZeroFound)
        //        {
        //            _ZeroAngleToCenter = GetAngleOfPointToZero(ZeroPosToCenter);
        //            bZeroFoundAt12 = (_ZeroAngleToCenter >= 88 && _ZeroAngleToCenter <= 92); //_zeroNumberArea.Contains(ZeroPos);

        //            //Common.Logger.Escribir($"Zero [X: {ZeroPosToCenter.X} Y: {ZeroPosToCenter.Y} A: {_ZeroAngleToCenter}]", true);

        //            if (bShowText)
        //            {
        //                lblZeroPosX.Text = ZeroPosToCenter.X.ToString();
        //                lblZeroPosY.Text = ZeroPosToCenter.Y.ToString();
        //                lblZeroPosAngle.Text = _ZeroAngleToCenter.ToString();
        //            }

        //            if (bZeroFoundAt12)
        //            {
        //                if (this._rpmCounter > 0)
        //                {
        //                    this._rpm = 1500 / this._rpmCounter;
        //                    if (this._rpm > 60)
        //                        this._rpm = 60;
        //                    this._rpmCounter = 0;
        //                }

        //            }
        //            else
        //            {
        //                if (this._isMoving)
        //                    this._rpmCounter++;
        //            }
        //        }

        //        BallPos.X = -640;
        //        _BallAngleToCenter = 0;
        //        pbBall.Image = BallBlobDetection(_BsourceFrame);
        //        bBallFound = BallPos.X != -640;
        //        if (bBallFound)
        //        {
        //            _BallAngleToCenter = GetAngleOfPointToZero(BallPosToCenter);
        //            //Common.Logger.Escribir($"Ball [X: {BallPosToCenter.X} Y: {BallPosToCenter.Y} A: {_ZeroAngleToCenter}]", true);
        //        }

        //        //    Roulette slots
        //        // 		314, 175
        //        //241, 246        381,246
        //        //      314, 314
        //        DebounceSwitch1();
        //        if (bShowText)
        //        {
        //            lblBolaPosX.Text = BallPosToCenter.X.ToString();
        //            lblBolaPosY.Text = BallPosToCenter.Y.ToString();
        //            lblDistZeroBall.Text = string.Format("{0}px - {1}°", _DistanceZeroBall, _BallAngleToCenter);
        //        }


        //        if (bZeroFoundAt12)
        //        {
        //            if (this.bDebouncedBallFound)
        //            {
        //                _DistanceZeroBall = FindDistance(ZeroPosToCenter, BallPosToCenter);
        //                if (bShowText)
        //                    lblDistZeroBall.Text = string.Format("{0}px - {1}°", _DistanceZeroBall, _BallAngleToCenter);

        //                if (this.CalibrationInProgress)
        //                    AcumulateCalibration();

        //                winnerXY = FindNumberByXY(BallPosToCenter.X, BallPosToCenter.Y);
        //                if (winnerXY != -1)
        //                {
        //                    winnerDA = FindNumberByAngle(this._DistanceZeroBall, this._BallAngleToCenter);
        //                    if ((winnerDA != -1) && (winnerXY == winnerDA))
        //                    {
        //                        winner = winnerDA;
        //                    }
        //                    else
        //                    {
        //                        if ((LogDetectedNumbers) && (this._rpm > 0))
        //                        {
        //                            Common.Logger.Escribir($"Zero [X: {ZeroPosToCenter.X} - Y: {ZeroPosToCenter.Y} - A: {_ZeroAngleToCenter} ]", true);
        //                            Common.Logger.Escribir($"-->FoundXY: {winnerXY} - TableXY: [X: {NumbersByXY[winnerXY, 0]} - Y: {NumbersByXY[winnerXY, 1]}] = Found [X: {BallPosToCenter.X} - Y: {BallPosToCenter.Y} ]", true);
        //                            Common.Logger.Escribir($"-->FoundDA: {winnerDA} - TableDA: [D: {this.NumbersByDistAngle[winnerXY, 0]} - A: {this.NumbersByDistAngle[winnerXY, 1]}] = Found [D: {this._DistanceZeroBall} - A : {this._BallAngleToCenter} ]", true);
        //                        }
        //                    }
        //                }

        //                if (winner != -1)
        //                {
        //                    _WinnerNumber = winner;
        //                    juego.SetNewWinnerNumber(_WinnerNumber);
        //                    lblWinner.Text = string.Format("{0}", _WinnerNumber);
        //                    if ((LogDetectedNumbers) && (this._rpm > 0))
        //                    {
        //                        Common.Logger.Escribir($"Zero [X: {ZeroPosToCenter.X} - Y: {ZeroPosToCenter.Y} - A: {_ZeroAngleToCenter} ]", true);
        //                        Common.Logger.Escribir($"-->WinnerXY: {winnerXY} - TableXY: [X: {NumbersByXY[winnerXY, 0]} - Y: {NumbersByXY[winnerXY, 1]}] = Found [X: {BallPosToCenter.X} - Y: {BallPosToCenter.Y} ]", true);
        //                        Common.Logger.Escribir($"-->WinnerDA: {winnerDA} - TableDA: [D: {this.NumbersByDistAngle[winnerXY, 0]} - A: {this.NumbersByDistAngle[winnerXY, 1]}] = Found [D: {this._DistanceZeroBall} - A : {this._BallAngleToCenter} ]", true);
        //                    }
        //                }
        //            }
        //        }
        //        else
        //        {
        //            winner = -1;
        //            lblWinner.Text = "--";
        //        }



        //        if (IsCalibratingCamera)
        //        {
        //            CalibrateCamera(_BsourceFrame);
        //        }

        //        args.Frame = _BsourceFrame;
        //        //stopWatch.Stop();
        //        //lblZeroBlobDetectionTime.Text = stopWatch.ElapsedMilliseconds.ToString();

        //    }
        //}

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
        //
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
                if (rects[0].Width >= 4)
                {
                    Rectangle objectRect = rects[0];
                    ZeroPos = objectRect.Center();
                    ZeroPosToCenter.X = ZeroPos.X - _centerPoint.X;
                    ZeroPosToCenter.Y = _centerPoint.Y - ZeroPos.Y;
                    this.bZeroFound = true;
                    _ZeroAngleToCenter = GetAngleOfPointToZero(ZeroPosToCenter);
                    this.bZeroFoundAt12 = (_ZeroAngleToCenter >= 88 && _ZeroAngleToCenter <= 92); //_zeroNumberArea.Contains(ZeroPos);
                }
                else
                    _ZeroAngleToCenter = 720;
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
                if (objectRect.Width >= 4)
                {
                    BallPos = objectRect.Center();
                    BallPosToCenter.X = BallPos.X - _centerPoint.X;
                    BallPosToCenter.Y = _centerPoint.Y - BallPos.Y;
                }
                else
                    _BallAngleToCenter = 720;

            }

            return _colorFilterImage;
        }

        #endregion

        #region Finding Winner Number
        /**
        * Determines the angle of a point against the coordinates center
        */
        private int GetAngleOfPointToZero(System.Drawing.Point p)
        {
            return (int)(Math.Round(Math.Atan2(p.Y, p.X) * radian) + 360) % 360;
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

        // Numbers by Angle
        private int[,] NumbersByDistAngle;

        // Numbers by Coordinates XY
        private int[,] NumbersByXY;

        private readonly double radian = 180.0 / Math.PI;

        public bool IsCalibratingNumbers { get => _IsCalibratingNumbers; set => _IsCalibratingNumbers = value; }
        public bool IsCalibratingCamera { get => _IsCalibratingCamera; set => _IsCalibratingCamera = value; }
        public bool LogDetectedNumbers { get => _LogDetectedNumbers; set => _LogDetectedNumbers = value; }

        const int DIFF_DIST = 2;
        const int DIFF_ANGLE = 1;

        private int FindNumberByAngle(int distance, int angle)
        {

            int winner = -1;
            if (angle != 0)
            {
                int min_angle = ((angle - DIFF_ANGLE) + 360) % 360;
                int max_angle = ((angle + DIFF_ANGLE) + 360) % 360;
                //if (min_angle > max_angle)
                //{
                //    int tmp = min_angle;
                //    min_angle = max_angle;
                //    max_angle = tmp;
                //}

                // For some reason, Atan2() is 0 when it shouldn't probably

                for (int i = 0; i < 37; i++)
                {

                    if ((NumbersByDistAngle[i, 0] >= (distance - DIFF_DIST)) &&
                        (NumbersByDistAngle[i, 0] <= (distance + DIFF_DIST)))
                    {
                        if ((NumbersByDistAngle[i, 1] >= Math.Min(min_angle, max_angle)) &&
                            (NumbersByDistAngle[i, 1] <= Math.Max(min_angle, max_angle)))
                        {
                            winner = i;
                            break;
                        }
                    }
                }

            }

            return winner;
        }

        // Delta x, y accepted range
        const int DIFF_XY = 3;

        private int FindNumberByXY(int x, int y)
        {
            int winner = -1;
            int min_X = (x - DIFF_XY);
            int max_X = (x + DIFF_XY);
            int min_Y = (y - DIFF_XY);
            int max_Y = (y + DIFF_XY);

            for (int i = 0; i < 37; i++)
            {
                if ((NumbersByXY[i, 0] >= min_X && NumbersByXY[i, 0] <= max_X) &&
                    (NumbersByXY[i, 1] >= min_Y && NumbersByXY[i, 1] <= max_Y))
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

        private void btnCalibrateNumber_Click(object sender, EventArgs e)
        {
            this.acumDist = 0;
            this.countDistance = 0;
            this.averageDist = 0;

            this.acumAngle = 0;
            this.countAngle = 0;
            this.averageAngle = 0;

            this.acumX = 0;
            this.countX = 0;
            this.averageX = 0;

            this.acumY = 0;
            this.countY = 0;
            this.averageY = 0;

            
            this.countCalibrationSamples = 0;
            this.CalibrationInProgress = true;

        }

        private void AcumulateCalibration()
        {
            this.countCalibrationSamples++;
            lblTestCount.Text = this.countCalibrationSamples.ToString();

            countDistance++;
            acumDist += this._DistanceZeroBall;

            if ((this._BallAngleToCenter != 720) && (this._BallAngleToCenter != 0))
            {
                acumAngle += this._BallAngleToCenter;
                countAngle++;
            }

            if (BallPosToCenter.X != 640)
            {
                this.countX++;
                this.acumX += BallPosToCenter.X;
                this.countY++;
                this.acumY += BallPosToCenter.Y;
            }

            if (this.countCalibrationSamples >= 100)
            {
                averageDist = acumDist / countDistance;
                this.lblAvgDist.Text = averageDist.ToString();
                averageAngle = acumAngle / countAngle;
                this.lblAvgAngle.Text = averageAngle.ToString();

                this.averageX = this.acumX / this.countX;
                this.lblAvgX.Text = this.averageX.ToString();

                this.averageY = this.acumY / this.countY;
                this.lblAvgY.Text = this.averageY.ToString();

                this.CalibrationInProgress = false;
            }
        }

        private void btnSetNumber_Click(object sender, EventArgs e)
        {
            try
            {
                int num = int.Parse(this.comboBox1.SelectedItem.ToString());
                int index = this.comboBox1.SelectedIndex;

                // Check if distance and angle values are already assigned to another number
                int winner = FindNumberByAngle(this.averageDist, this.averageAngle);
                if ((winner != -1) && (winner != num))
                {
                    DialogResult dialogResult = MessageBox.Show("El numero " + winner.ToString() + " ya tiene estas coordenadas, actualiza?", "Distancia y Angulo asignados ya al numero " + winner.ToString(), MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.No)
                    {
                        MessageBox.Show("Seleccione otro numero");
                        return; // Do not update
                    }
                }

                // Check if X and Y values are already assigned to another number
                winner = FindNumberByXY(BallPosToCenter.X, BallPosToCenter.Y);
                if ((winner != -1) && (winner != num))
                {
                    DialogResult dialogResult = MessageBox.Show("El numero " + winner.ToString() + " ya tiene estas coordenadas, actualiza?", "Distancia y Angulo asignados ya al numero " + winner.ToString(), MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.No)
                    {
                        MessageBox.Show("Seleccione otro numero");
                        return; // Do not update
                    }
                }

                NumbersByDistAngle[num, 0] = this.averageDist;
                NumbersByDistAngle[num, 1] = this.averageAngle;

                NumbersByXY[num, 0] = this.averageX;
                NumbersByXY[num, 1] = this.averageY;

                WriteNumbersTable();
                if (chkbNumbers[index].CheckState != CheckState.Checked)
                {
                    chkbNumbers[index].CheckState = CheckState.Checked;
                    this.numCalibrated++;
                    lblChkCount.Text = this.numCalibrated.ToString();
                }
                MessageBox.Show("Numero " + num.ToString() + " actualizado", "Número Calibrado");
                if (this.comboBox1.SelectedIndex < this.comboBox1.Items.Count - 1)
                {
                    this.comboBox1.SelectedIndex = this.comboBox1.SelectedIndex + 1;
                }
                else
                    this.comboBox1.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al grabar la calibracion. Requiere credenciales de administrador" + ex.GetType().ToString());
            }
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
            this.IsCalibratingNumbers = cbCalibrateNumbers.Checked;

            if (this.IsCalibratingNumbers)
            {
                juego.SetCurrentState(JuegoRuleta.ESTADO_JUEGO.TABLE_CLOSED);
                this.estadoMesa = juego.GetGameState(this._rpm, this.IsCameraOn, this.bDebouncedBallFound);
                this.GuardarEstado((int)estadoMesa, juego.GetLastWinnerNumber(), this._rpm, 0);
                this.comboBox1.SelectedIndex = 0;
                this.numCalibrated = 0;
                lblChkCount.Text = this.numCalibrated.ToString();
                ShowNumbersCheckBox();
                this.pnlCalibration.Visible = true;
                this.btnSaveCSV.Visible = true;
                radioButton1.PerformClick();
                btnUpdateRGB.Visible = true;
                btnUpdateRGB.Enabled = true;
                lblTiming.Visible = true;
                lblTimingValue.Visible = true;
            }
            else
            {
                this.pnlCalibration.Visible = false;
                this.btnSaveCSV.Visible = false;
                btnUpdateRGB.Visible = false;
                btnUpdateRGB.Enabled = false;
                lblTiming.Visible = false;
                lblTimingValue.Visible = false;
            }
        }

        void UpdateRGBUpDown()
        {
            if (radioButton1.Checked)
            {
                numUpDownRed.Value = this._zeroColorFilter.CenterColor.Red;
                numUpDownGreen.Value = this._zeroColorFilter.CenterColor.Green;
                numUpDownBlue.Value = this._zeroColorFilter.CenterColor.Blue;
            }
            else
            {
                numUpDownRed.Value = this._ballColorFilter.CenterColor.Red;
                numUpDownGreen.Value = this._ballColorFilter.CenterColor.Green;
                numUpDownBlue.Value = this._ballColorFilter.CenterColor.Blue;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBox1.Visible)
            {
                int num = int.Parse(this.comboBox1.SelectedItem.ToString());

                this.lblAvgDist.Text = NumbersByDistAngle[num, 0].ToString();
                this.lblAvgAngle.Text = NumbersByDistAngle[num, 1].ToString();

                this.lblAvgX.Text = this.NumbersByXY[num, 0].ToString();
                this.lblAvgY.Text = this.NumbersByXY[num, 1].ToString();
            }
        }

        private void GuardarNumeroGanador(int numero)
        {
            string cadena = ProtocoloNAPSA.FormatearCadenaNumeroGanador(numero);
            Persistencia.Guardar(cadena);
            txtProtocolo.AppendText(cadena);
            txtProtocolo.AppendText(Environment.NewLine);
        }
        #endregion

        private void btnSaveCSV_Click(object sender, EventArgs e)
        {
            SaveCSV();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            UpdateRGBUpDown();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            UpdateRGBUpDown();
        }

        private void UpdateColorFilter()
        {
            RGB newColor = new RGB((byte)numUpDownRed.Value,
                            (byte)numUpDownGreen.Value,
                            (byte)numUpDownBlue.Value);

            if (radioButton1.Checked)
            {
                this._zeroColorFilter.CenterColor = newColor;
            }
            else
            {
                this._ballColorFilter.CenterColor = newColor;
            }
        }

        private void SaveRGBToConfig()
        {

            if (radioButton1.Checked)
            {
                SetSetting("ZeroRed", this._zeroColorFilter.CenterColor.Red.ToString());
                SetSetting("ZeroGreen", this._zeroColorFilter.CenterColor.Green.ToString());
                SetSetting("ZeroBlue", this._zeroColorFilter.CenterColor.Blue.ToString());
            }
            else
            {
                SetSetting("BallRed", this._ballColorFilter.CenterColor.Red.ToString());
                SetSetting("BallGreen", this._ballColorFilter.CenterColor.Green.ToString());
                SetSetting("BallBlue", this._ballColorFilter.CenterColor.Blue.ToString());
            }

        }

        private void numUpDownRed_ValueChanged(object sender, EventArgs e)
        {
            //UpdateColorFilter();
        }

        private void numUpDownGreen_ValueChanged(object sender, EventArgs e)
        {
            //UpdateColorFilter();
        }

        private void numUpDownBlue_ValueChanged(object sender, EventArgs e)
        {
            //UpdateColorFilter();
        }

        private void btnUpdateRGB_Click(object sender, EventArgs e)
        {
            UpdateColorFilter();
            SaveRGBToConfig();
        }

        private void chkbGuardarLog_CheckedChanged(object sender, EventArgs e)
        {
            this.LogDetectedNumbers = chkbGuardarLog.Checked;
        }

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
//                int num = (int)MessageBox.Show(ex.Message);
                Common.Logger.Escribir("*** Error en Cerrar() ***" + ex.Message.ToString(), true);
            }
        }

        private void ReadNumbersTable()
        {
            // Read numbers data for distance and angle detection
            using (var stream = new StreamReader(@"./dataDA.json"))
            {
                this.NumbersByDistAngle = JsonConvert.DeserializeObject<int[,]>(stream.ReadToEnd());
            }
            // Read numbers' data for ball's X & Y detection
            using (var stream = new StreamReader(@"./dataXY.json"))
            {
                this.NumbersByXY = JsonConvert.DeserializeObject<int[,]>(stream.ReadToEnd());
            }
        }

        private void WriteNumbersTable()
        {
            // write the data (overwrites) distance and angle detection data
            using (var stream = new StreamWriter(@"./dataDA.json", append: false))
            {
                stream.Write(JsonConvert.SerializeObject(this.NumbersByDistAngle));
                stream.Flush();
            }
            // write the data (overwrites) X & Y detection data
            using (var stream = new StreamWriter(@"./dataXY.json", append: false))
            {
                stream.Write(JsonConvert.SerializeObject(this.NumbersByXY));
                stream.Flush();
            }

        }

        void SaveCSV()
        {
            using (var w = new StreamWriter(@"./dataDA.csv", append: false))
            {
                for (int i = 0; i < 37; i++)
                {
                    var first = NumbersByDistAngle[i, 0];
                    var second = NumbersByDistAngle[i, 1];
                    var third = NumbersByXY[i, 0];
                    var fourth = NumbersByXY[i, 1];
                    string line = string.Format("{0},{1},{2},{3},{4}", first, second, third, fourth, i);
                    w.WriteLine(line);
                }
                w.Flush();
                MessageBox.Show("Archivo CSV generado");
            }

        }
    }
}
