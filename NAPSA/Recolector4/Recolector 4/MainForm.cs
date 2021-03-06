﻿using Accord;
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
        private System.Drawing.Point ZeroPos, BallPos;
        private int _ZeroAngleToCenter = 0;
        // Measurement variables
        //private int _DistanceZeroBall = 0, _BallAngleToCenter = 0;

        private bool bZeroFound = false;
        private bool bBallFound = false;
        private bool bZeroFoundAt12 = false;

        private bool bShowText = false;

        // Winner number variables
        private bool bDebouncedBallFound = false;
        private bool bBallStateChanged = false;
        private int iBallUnchangeCount = 0;

 
        private bool _IsCalibratingCamera = false;
        private bool _IsCalibratingNumbers = false;
        private bool _LogDetectedNumbers = false;

        private Bitmap capturedFrame;

        // motion detection and processing algorithm
        MotionDetector detector = new MotionDetector(new TwoFramesDifferenceDetector()); 
        private bool _isMoving = false;
        //private float motionAlarmLevel = 0.003f;
        private int _rpm = 0;
        private int _rpmCounter = 0;
        private int _lastRpmCounter = 0;
        private int _callCounter = 0;
        private int _zeroesCounter = 0;
        private int _zeroAtNoonCounter = 0;
        private int _timerTicks = 0;
        private float movePercentage = 0.0f;

        // Demo variables
        //private int estadoDemo;
        //private byte numeroDemo;
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

        private WinnerFinder winfinder;

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
            CheckForIllegalCrossThreadCalls = false; // TO-DO avoid illegal cross thread calls
            setupDetectionVariables(); // Filter for blob detecting. Parameters setup in caller
            juego = new JuegoRuleta();
            estadoMesa = juego.GetCurrentState();
            this.tmrMain.Interval = 500; // msec
            this._timerTicks = 0;
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
                this._rpm = 0;
                this._rpmCounter = 0;
                juego.SetCurrentState(JuegoRuleta.ESTADO_JUEGO.TABLE_CLOSED);
                this.GuardarEstado(estadoMesa, juego.GetLastWinnerNumber(), 0, 0);
                IsCalibratingCamera = true;
                this.lblFPS.Visible = true;
                lblEventCalls.Visible = true;
                StopCamera();
                StartCameraCalibration();
            }
            else
            {
                lblEventCalls.Visible = false;
                IsCalibratingCamera = false;
                this.pnlCalibration.Visible = false;
                this.lblFPS.Visible = false;
                StopCameraCalibration();
                StartCamera();
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

        private void StartCameraCalibration()
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
                videoSourcePlayer1.NewFrameReceived += new Accord.Video.NewFrameEventHandler(get_Frame_Calibration);
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

        private void StopCameraCalibration()
        {
            try
            {
                videoSourcePlayer1.SignalToStop();
                videoSourcePlayer1.WaitForStop();
                videoSourcePlayer1.NewFrameReceived -= new Accord.Video.NewFrameEventHandler(get_Frame_Calibration);
                lblVideoStatus.BackColor = Color.DarkRed;
                lblVideoStatus.Text = "OFF";

                pbZero.Image = null;
                pbBall.Image = null;
                lblDisplayWinner.Text = "";
                lblDisplayWinner.ForeColor = Color.Black;
                lblEstadoRuleta.Text = "";
                lblBolaPosX.Text = "---";
                lblBolaPosY.Text = "---";
                lblZeroPosX.Text = "---";
                lblZeroPosY.Text = "---";
                lblBallAbsX.Text = "---";
                lblBallAbsY.Text = "---";
                lblZeroAbsX.Text = "---";
                lblZeroAbsY.Text = "---";
                lblBallOn.Text = "";
                this.bDebouncedBallFound = false;
                this.bBallStateChanged = true;
                this._rpm = 0;
                _isMoving = false;
                // reset motion detector
                if (detector != null)
                    detector.Reset();
            }
            catch (Exception)
            {
                return;
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
                videoSourcePlayer1.NewFrameReceived -= new Accord.Video.NewFrameEventHandler(get_Frame);   
                lblVideoStatus.BackColor = Color.DarkRed;
                lblVideoStatus.Text = "OFF";

                pbZero.Image = null;
                pbBall.Image = null;
                lblDisplayWinner.Text = "";
                lblDisplayWinner.ForeColor = Color.Black;
                lblEstadoRuleta.Text = "";
                lblBolaPosX.Text = "";
                lblBolaPosY.Text = "";
                lblZeroPosAngle.Text = "---";
                lblZeroPosX.Text = "";
                lblZeroPosY.Text = "";
                lblBallOn.Text = "";
                lblBallPosAngle.Text = "";
                lblDistZeroBall.Text = "---";
                bDebouncedBallFound = false;
                bBallStateChanged = true;
                _rpm = 0;
                _isMoving = false;
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
            
            Pen _penyellow = new Pen(Color.Yellow, ipenWidth);
            Pen _penviolet = new Pen(Color.DarkViolet, ipenWidth+10);
            Pen _penred = new Pen(Color.Red, ipenWidth);
            Pen _penblue = new Pen(Color.Blue, ipenWidth);

            // Cilindro -incluye numeros - radius 204 color red
            _g.DrawEllipse(_penyellow, _bowlArea);

            _g.DrawEllipse(_penviolet, _numbersArea);

            // Casillas - radius 204 color blue
            _g.DrawEllipse(_penred, _ballPocketsArea);
            _g.DrawRectangle(_penred, _centerPoint.X, _centerPoint.Y, 1, 1);
            lblEventCalls.Text = $"{_centerPoint.X},{_centerPoint.Y}";
            DrawZeroArea(_bitmapSourceImage);
        }

        private void DrawZeroArea( Bitmap _bitmapSourceImage )
        {
            int ipenWidth = 5;

            Graphics _g = Graphics.FromImage(_bitmapSourceImage);
            Pen _pengreen = new Pen(Color.FromArgb(Byte.Parse(GetSetting("ZeroRed")),
                                                   Byte.Parse(GetSetting("ZeroGreen")),
                                                   Byte.Parse(GetSetting("ZeroBlue"))), ipenWidth);
            _g.DrawRectangle(_pengreen, _zeroNumberArea);

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
            _zeroBlobCounter.CoupledSizeFiltering = false;
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
            _ballBlobCounter.CoupledSizeFiltering = false;
            _ballBlobCounter.ObjectsOrder = ObjectsOrder.Size;
            _ballBlobCounter.MinWidth = int.Parse(GetSetting("BallMinSize"));
            _ballBlobCounter.MaxWidth = int.Parse(GetSetting("BallMaxSize"));

            Pen ballPen = new Pen(Color.FromArgb(ballColor.Red, ballColor.Green, ballColor.Blue), 5);

            // Base ruleta sin aro negro - radius 164 color red
            // Area where the ball rolls before getting into a pocket
            int bowlX = subtractImage.Width / 4; // 640 / 4 = 160
            int bowlY = 90;
            int bowlWidthHeight = 310;
            _bowlArea = new Rectangle(bowlX, bowlY, bowlWidthHeight, bowlWidthHeight);     

            int numsAreaX = bowlX + 58;                                         // 160 + 58 = 218
            int numsAreaY = bowlY + 62;                                         // 90 + 62 = 152
            int numsAreaWH = bowlWidthHeight - 114;                             // 310 - 114 = 196
            // Include the ring outside the numbers
            numsAreaX -= 10;                                                    // 218 - 10 = 208
            numsAreaY -= 10;                                                    // 152 - 10 = 142
            numsAreaWH += 20;                                                   // 196 + 20 = 218
            // Area where all the numbers are
            _numbersArea = new Rectangle(numsAreaX, numsAreaY, numsAreaWH, numsAreaWH);

            // Cylinder area to detect ball presence
            int ballPocketsX = bowlX + 78;                                      // 160 + 78 = 238
            int ballPocketsY = bowlY + 80;                                      // 90 + 80 = 170
            int ballPocketsWH = bowlWidthHeight - 152;                          // 310 - 152 = 158
            _ballPocketsArea = new Rectangle(ballPocketsX, ballPocketsY, ballPocketsWH, ballPocketsWH);

            // Center of the roulette area
            int centerAreaX = bowlX + 102;                                      // 160 + 102 = 262
            int centerAreaY = bowlY + 106;                                      //  90 + 106 = 196
            int centerAreaWH = bowlWidthHeight - 202;                           // 310 - 202 = 108

            // Make the center bigger
            centerAreaX -= 6;
            centerAreaY -= 6;
            centerAreaWH += 12;
            _centerArea = new Rectangle(centerAreaX, centerAreaY, centerAreaWH, centerAreaWH);

            _centerPoint = _bowlArea.Center(); // _centerArea.Center();                                // Center of the roulette area rectangle

            int zeroAtNoonX = bowlX + 150;                                      // 160 + 150 = 310
            int zeroAtNoonY = bowlY + 62;                                       //  90 + 62  = 152
            int zeroAtNoonW = 12;
            int zeroAtNoowH = 20;
            _zeroNumberArea = new Rectangle(zeroAtNoonX, zeroAtNoonY, zeroAtNoonW, zeroAtNoowH);     // Where the zero is at 12 am

            winfinder = new WinnerFinder(_centerPoint);

            // Mask the image around the roulette to avoid false positives 
            // substractImage masking drawing here
            using (Graphics graph = Graphics.FromImage(subtractImage))
            {
                Rectangle ImageSize = new Rectangle(0, 0, subtractImage.Width, subtractImage.Height);
                graph.FillRectangle(Brushes.White, ImageSize);
                graph.FillEllipse(Brushes.Black, _bowlArea);
                graph.FillEllipse(Brushes.White, _centerArea);
                Pen _penwhite = new Pen(Color.White, 20);
                graph.DrawEllipse(_penwhite, _numbersArea);
                //Pen _penwhite2 = new Pen(Color.White, 4);
                //graph.DrawEllipse(_penwhite2, _ballPocketsArea);
            }
            // Required to apply filters to the full frame
            _frameArea = new Rectangle(0, 0, subtractImage.Width, subtractImage.Height);

            for (int i = 0; i < 37; i++)
            {
                this.comboBox1.Items.Add(winfinder.RouletteNumbers[i,0].ToString());
            }
            this.comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBox1.SelectedIndex = this.comboBox1.FindStringExact("0");
        }


        const int CHECK_MSEC = 40; // Read hardware every 5 msec
        const int BALL_FOUND_MSEC = 2 * 12 * CHECK_MSEC; // Stable time before registering pressed
        const int BALL_NOT_FOUND_MSEC = 4 * 12 * CHECK_MSEC; // Stable time before registering released

        // This function reads the key state from the hardware.
        bool RawBallFound()
        {
            return (_ballPocketsArea.Contains(BallPos));
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

        private void CalculateRPM()
        {
            bZeroFoundAt12 = winfinder.IsZeroAtNoon(ZeroPos);

            if (bZeroFoundAt12)
            {
                this._zeroAtNoonCounter++;
                if (_rpmCounter > 0)
                {
                    if (this._lastRpmCounter != 0)
                    {
                        float delta = (float)_rpmCounter / (float)_lastRpmCounter;
                        if (delta > 2)
                            _rpmCounter /= (int)delta;
                    }
                    // 1 min = 60000 msec => 60000 msec / 40 msec = 1500
                    this._rpm = 1500 / this._rpmCounter;
                    if (_rpm > 60)
                        _rpm = 60;
                    _lastRpmCounter = _rpmCounter;
                    this._rpmCounter = 0;
                }
            }
            else
            {
                this._rpmCounter++;
            }
        }

        private void get_Frame(object sender, NewFrameEventArgs args)
        {
            lock (this)
            {
                // Count call
                this._callCounter++;

                //Stopwatch stopWatch = new Stopwatch();
                //stopWatch.Start();

                Bitmap _BsourceFrame = (Bitmap)args.Frame.Clone();
                _BsourceFrame = _resizeFilter.Apply(_BsourceFrame); // new Bitmap(args.Frame, _pbSize);
                Subtract _subtractFilter = new Subtract(subtractImage);
                _subtractFilter.ApplyInPlace(_BsourceFrame);
                //Difference _subtractFilter = new Difference(subtractImage);
                //_subtractFilter.ApplyInPlace(_BsourceFrame);

                // Detect movement
                this.movePercentage = detector.ProcessFrame(_BsourceFrame);
                _isMoving = (this.movePercentage > 0.01f);

                pbZero.Image = ZeroBlobDetection(_BsourceFrame);
                if (_isMoving)
                {
                    if (this.bZeroFound)
                    {
                        this._zeroesCounter++;
                        CalculateRPM();
                    }
                }
                else
                {
                    this._rpmCounter = 0;
                    this._rpm = 0;
                }

                pbBall.Image = BallBlobDetection(_BsourceFrame);
                DebounceBallInSlot();
                if (estadoMesa == JuegoRuleta.ESTADO_JUEGO.NO_MORE_BETS)
                {
                    if ((this.bZeroFoundAt12) && (this.bDebouncedBallFound))
                    {
                        winfinder.FindWinnerNumber(ZeroPos, BallPos, juego);
                    }
                }

                args.Frame = _BsourceFrame;
                //stopWatch.Stop();

                //if (stopWatch.ElapsedMilliseconds > 40)
                //{
                //    MessageBox.Show($"Exceeded: {stopWatch.ElapsedMilliseconds.ToString()}");
                //}
                //lblZeroBlobDetectionTime.Text = stopWatch.ElapsedMilliseconds.ToString();
            }
        }

        private void get_Frame_Calibration(object sender, NewFrameEventArgs args)
        {
            lock (this)
            {
                int winner = -1;

                bool bShowText = (this.IsCalibratingCamera || this.IsCalibratingNumbers);

                //Stopwatch stopWatch = new Stopwatch();
                //stopWatch.Start();

                Bitmap _BsourceFrame = (Bitmap)args.Frame.Clone();
                _BsourceFrame = _resizeFilter.Apply(_BsourceFrame); // new Bitmap(args.Frame, _pbSize);
                Subtract _subtractFilter = new Subtract(subtractImage);
                _subtractFilter.ApplyInPlace(_BsourceFrame);
                if (IsCalibratingCamera)
                {
                    CalibrateCamera(_BsourceFrame);
                }

                if (IsCalibratingNumbers)
                {
                    DrawZeroArea(_BsourceFrame);
                }

                if (this.CalibrationInProgress)
                {
                    if (this.capturedFrame == null)
                    {
                        this.capturedFrame = _BsourceFrame;
                    }

                    _BsourceFrame = this.capturedFrame;
                }

                this.movePercentage = detector.ProcessFrame(_BsourceFrame);
                _isMoving = (this.movePercentage > 0.01f);

                if (bShowText)
                    _isMoving = false;


                pbZero.Image = ZeroBlobDetection(_BsourceFrame);
                if (this._isMoving)
                {
                    if (this.bZeroFound)
                    {
                        this._zeroesCounter++;
                        CalculateRPM();
                    }
                }

                pbBall.Image = BallBlobDetection(_BsourceFrame);
                DebounceBallInSlot();
                if ((this.bZeroFoundAt12) && (this.bDebouncedBallFound))
                {
                    winfinder.FindWinnerNumber(ZeroPos, BallPos, ref winner);
                }

                if (this.CalibrationInProgress)
                {
                    AcumulateCalibration();
                }

                args.Frame = _BsourceFrame;
                if (bShowText)
                {
                    DisplayValues();
                }
                //stopWatch.Stop();

                //if (stopWatch.ElapsedMilliseconds > 40)
                //{
                //    MessageBox.Show($"Exceeded: {stopWatch.ElapsedMilliseconds.ToString()}");
                //}
                //lblZeroBlobDetectionTime.Text = stopWatch.ElapsedMilliseconds.ToString();
            }
        }

        private void DisplayValues()
        {
            int winner = -1;
            if (this.bZeroFound)
            {
                System.Drawing.Point ZeroPosToCenter = new System.Drawing.Point(ZeroPos.X - _centerPoint.X, _centerPoint.Y - ZeroPos.Y);
                this.lblZeroPosX.Text = ZeroPosToCenter.X.ToString();
                this.lblZeroPosY.Text = ZeroPosToCenter.Y.ToString();
                _ZeroAngleToCenter = winfinder.GetAngleOfPointToZero(ZeroPosToCenter);
                this.lblZeroPosAngle.Text = _ZeroAngleToCenter.ToString();
                this.lblZeroAbsX.Text = ZeroPos.X.ToString();
                this.lblZeroAbsY.Text = ZeroPos.Y.ToString();
            }

            if (this.bBallFound)
            {
                System.Drawing.Point BallPosToCenter = new System.Drawing.Point(BallPos.X - _centerPoint.X, _centerPoint.Y - BallPos.Y);
                this.lblBolaPosX.Text = BallPosToCenter.X.ToString();
                this.lblBolaPosY.Text = BallPosToCenter.Y.ToString();
                int BallAngleToCenter = winfinder.GetAngleOfPointToZero(BallPosToCenter);
                this.lblBallPosAngle.Text = BallAngleToCenter.ToString();
                this.lblBallAbsX.Text = BallPos.X.ToString();
                this.lblBallAbsY.Text = BallPos.Y.ToString();
            }

            if (this.bZeroFound && this.bBallFound)
            {
                int _DistanceZeroBall = winfinder.FindDistance(ZeroPos, BallPos);
                this.lblDistZeroBall.Text = _DistanceZeroBall.ToString();

                winfinder.FindWinnerNumber(ZeroPos, BallPos, ref winner);
            }

            lblFound.Text = winner != -1 ? winner.ToString() : "---";
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
        //
        // Zero position detection
        private Bitmap ZeroBlobDetection(Bitmap _bitmapSourceImage)
        {
            // Filter pixels with zero's color
            Bitmap _colorFilterImage = _zeroColorFilter.Apply(_bitmapSourceImage);

            // Apply grayscale filter
            BitmapData objectsData = _colorFilterImage.LockBits(_frameArea, ImageLockMode.ReadOnly, _colorFilterImage.PixelFormat);
            UnmanagedImage grayImage = Grayscale.CommonAlgorithms.BT709.Apply(new UnmanagedImage(objectsData));
            _zeroBlobCounter.FilterBlobs = true;
            _zeroBlobCounter.CoupledSizeFiltering = true;

            _zeroBlobCounter.ProcessImage(grayImage);
            _colorFilterImage.UnlockBits(objectsData);

            Rectangle[] rects = _zeroBlobCounter.GetObjectsRectangles();

            if (rects.Length > 0)
            {
                if ((rects[0].Width >= _zeroBlobCounter.MinWidth) && (rects[0].Width != rects[0].Height) && _numbersArea.Contains(rects[0]))
                {
                    Rectangle objectRect = rects[0];
                    ZeroPos = objectRect.Center();
                    bZeroFound = true;
                    //Graphics _g = Graphics.FromImage(_colorFilterImage);
                    //Pen ballPen = new Pen(Color.FromArgb(_zeroColorFilter.CenterColor.Red, _zeroColorFilter.CenterColor.Green, _zeroColorFilter.CenterColor.Blue), 5);
                    //_g.DrawRectangle(ballPen, rects[0]);
                }
                else
                {
                    ZeroPos = System.Drawing.Point.Empty;
                    bZeroFound = false;
                    bZeroFoundAt12 = false;
                }
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
            _ballBlobCounter.FilterBlobs = false;
            _ballBlobCounter.CoupledSizeFiltering = true;
            _ballBlobCounter.ProcessImage(grayImage);
            _colorFilterImage.UnlockBits(objectsData);

            Rectangle[] rects = _ballBlobCounter.GetObjectsRectangles();

            if (rects.Length > 0)
            {
                Rectangle objectRect = rects[0];
                if ((objectRect.Width >= _ballBlobCounter.MinWidth) && (objectRect.Width <= _ballBlobCounter.MaxWidth))
                {
                    BallPos = objectRect.Center();
                    bBallFound = true;

                    //Graphics _g = Graphics.FromImage(_colorFilterImage);
                    //Pen ballPen = new Pen(Color.FromArgb(_ballColorFilter.CenterColor.Red, _ballColorFilter.CenterColor.Green, _ballColorFilter.CenterColor.Blue), 5);
                    //_g.DrawRectangle(ballPen, rects[0]);
                }
                else
                {
                    BallPos = System.Drawing.Point.Empty;
                    bBallFound = false;
                }
            }

            return _colorFilterImage;
        }

        #endregion

        #region Calibration boolean
        public bool IsCalibratingNumbers { get => _IsCalibratingNumbers; set => _IsCalibratingNumbers = value; }
        public bool IsCalibratingCamera { get => _IsCalibratingCamera; set => _IsCalibratingCamera = value; }
        public bool LogDetectedNumbers { get => _LogDetectedNumbers; set => _LogDetectedNumbers = value; }
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
            this.capturedFrame = null;
            this.CalibrationInProgress = true;


        }

        private void AcumulateCalibration()
        {
            System.Drawing.Point ZeroPosToCenter = new System.Drawing.Point(ZeroPos.X - _centerPoint.X, _centerPoint.Y - ZeroPos.Y);
            System.Drawing.Point BallPosToCenter = new System.Drawing.Point(BallPos.X - _centerPoint.X, _centerPoint.Y - BallPos.Y);
            if (this.bBallFound)
            {
                this.countCalibrationSamples++;
                lblTestCount.Text = this.countCalibrationSamples.ToString();

                countDistance++;
                int DistanceZeroBall = winfinder.FindDistance(ZeroPosToCenter, BallPosToCenter);
                acumDist += DistanceZeroBall;

                int BallAngleToCenter = winfinder.GetAngleOfPointToZero(BallPosToCenter);
                acumAngle += BallAngleToCenter;
                countAngle++;

                this.countX++;
                this.acumX += BallPosToCenter.X;
                this.countY++;
                this.acumY += BallPosToCenter.Y;
            }

            try
            {
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

                    if (this.bZeroFound)
                    {
                        winfinder.SetZeroCoordinates(ZeroPos);
                    }
                    //else
                    //{
                    //    int x = Int32.Parse(lblZeroPosX.Text);
                    //    int y = Int32.Parse(lblZeroPosY.Text);
                    //    int angle = Int32.Parse(lblZeroPosAngle.Text);
                    //    winfinder.SetZeroCoordinates(x, y, angle);
                    //}

                    this.CalibrationInProgress = false;
                }
            } catch (Exception e)
            {
                MessageBox.Show(e.Message + " - " + e.Source);
            }
        }

        private void btnSetNumber_Click(object sender, EventArgs e)
        {
            try
            {
                int num = int.Parse(this.comboBox1.SelectedItem.ToString());
                int index = this.comboBox1.SelectedIndex;

                // Check if distance and angle values are already assigned to another number
                int winner = winfinder.FindNumberByAngle(this.averageDist, this.averageAngle);
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
                System.Drawing.Point BallPosToCenter = new System.Drawing.Point(BallPos.X - _centerPoint.X, _centerPoint.Y - BallPos.Y);
                winner = winfinder.FindNumberByXY(BallPosToCenter.X, BallPosToCenter.Y);
                if ((winner != -1) && (winner != num))
                {
                    DialogResult dialogResult = MessageBox.Show("El numero " + winner.ToString() + " ya tiene estas coordenadas, actualiza?", "Distancia y Angulo asignados ya al numero " + winner.ToString(), MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.No)
                    {
                        MessageBox.Show("Seleccione otro numero");
                        return; // Do not update
                    }
                }

                winfinder.SetNumberCoordinates(num, this.averageX, this.averageY, this.averageDist, this.averageAngle);
                winfinder.WriteNumbersTable();

                if (chkbNumbers[index].CheckState != CheckState.Checked)
                {
                    chkbNumbers[index].CheckState = CheckState.Checked;
                    this.numCalibrated++;
                    lblChkCount.Text = this.numCalibrated.ToString();
                }

                MessageBox.Show($"Numero {num} actualizado:\nX: {this.averageX}\n Y:{this.averageY}\nA:{this.averageAngle}\nD:{this.averageDist}", "Número Calibrado");

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

        private void GuardarEstado(JuegoRuleta.ESTADO_JUEGO estado, int numero, int rpm, int sentidoDeGiro)
        {
            string cadena = string.Empty;
            if (estado != JuegoRuleta.ESTADO_JUEGO.STATE_0)
            {
                cadena = ProtocoloNAPSA.FormatearCadenaEstado(numero, (int)estado, rpm, sentidoDeGiro, 0);
                Persistencia.Guardar(cadena);
                //if (txtProtocolo.Lines.Length > 5)
                //    txtProtocolo.Text = "";
                txtProtocolo.AppendText(cadena);
                txtProtocolo.AppendText(Environment.NewLine);
            }
        }

        private void cbCalibrateNumbers_CheckedChanged(object sender, EventArgs e)
        {
            this.IsCalibratingNumbers = cbCalibrateNumbers.Checked;

            if (this.IsCalibratingNumbers)
            {
                this._rpm = 0;
                this._rpmCounter = 0;
                juego.SetCurrentState(JuegoRuleta.ESTADO_JUEGO.TABLE_CLOSED);
                this.GuardarEstado(estadoMesa, juego.GetLastWinnerNumber(), 0, 0);

                this.comboBox1.SelectedIndex = 0;
                this.numCalibrated = 0;
                lblChkCount.Text = this.numCalibrated.ToString();
                ShowNumbersCheckBox();
                this.pnlCalibration.Visible = true;
                this.btnSaveCSV.Visible = true;
                this.btnSavePNG.Visible = true;
                radioButton1.PerformClick();
                btnUpdateRGB.Visible = true;
                btnUpdateRGB.Enabled = true;
                //lblTiming.Visible = true;
                //lblTimingValue.Visible = true;
                StopCamera();
                StartCameraCalibration();
            }
            else
            {
                this.pnlCalibration.Visible = false;
                this.btnSaveCSV.Visible = false;
                this.btnSavePNG.Visible = false;
                btnUpdateRGB.Visible = false;
                btnUpdateRGB.Enabled = false;
                //lblTiming.Visible = false;
                //lblTimingValue.Visible = false;
                StopCameraCalibration();
                StartCamera();
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

                this.lblAvgDist.Text = winfinder.GetDistance(num).ToString();
                this.lblAvgAngle.Text = winfinder.GetAngle(num).ToString();

                this.lblAvgX.Text = winfinder.GetNumberX(num).ToString();
                this.lblAvgY.Text = winfinder.GetNumberY(num).ToString();
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
            winfinder.SaveCSV();
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

        private void btnSavePNG_Click(object sender, EventArgs e)
        {
            Bitmap frame = videoSourcePlayer1.GetCurrentVideoFrame();
            int num = int.Parse(this.comboBox1.SelectedItem.ToString());
            String filename = $"./frame_{num.ToString("D2")}.png";
            frame.Save(Common.Parametros.LogRuta + filename, ImageFormat.Png);
            MessageBox.Show($"Captura {filename} Guardada");
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

    }
}
