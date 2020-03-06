using Accord.Video;
using DASYS.Recolector.BLL;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace VideoRecolector
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private Stopwatch stopWatch = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private int lastDisplayedWinner = -2;
        private bool lastBallFound = false;
        private int lastrpm = -5;
        private string lastEstadoMesa = "";

        #region Form_Load, Closing
        private void MainForm_Load(object sender, EventArgs e)
        {
            // assign number of controls 
            int chkNum = 37; // 37 numeros

            chkbNumbers = new System.Windows.Forms.CheckBox[chkNum + 1];
            for (int i = 0; i < chkNum + 1; i++)
            {
                // Initialize one variable
                chkbNumbers[i] = new System.Windows.Forms.CheckBox();
            }
            PopulateNumbersColors();

            this.azarNumero = new Random((int)DateTime.Now.Ticks);
            this.LeerUltimoNumero();
            if (Pase.UltimoPase == null)
                Pase.UltimoPase = new Pase();
            this.estadoMesa = JuegoRuleta.ESTADO_JUEGO.STATE_0;
            this.IsCameraOn = false;
            // Initialize display variables
            this._rpm = 0;
            this.lastDisplayedWinner = -2;
            this.lastBallFound = !this.bDebouncedBallFound;
            this.lastrpm = -5;
            this.lastEstadoMesa = "";

            this.btnStartCamara.PerformClick();
            this.Hide();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this.Cerrar();
            }
            catch
            {
                return;
            }
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
                this.Hide();
            else
                this.Show();
        }

        #endregion

        private void cerrarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cerrar();
        }

        #region Main Timer
        private void DisplayFPS()
        {
            IVideoSource videoSource = videoSourcePlayer1.VideoSource;

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

                    float fps = (float)tmrMain.Interval * 2.0f * framesReceived / stopWatch.ElapsedMilliseconds;
                    lblFPS.Text = fps.ToString("F2") + " fps";

                    stopWatch.Reset();
                    stopWatch.Reset();
                    stopWatch.Start();
                }
            }
        }

        private int[] RouletteNumbersColors;

        private void PopulateNumbersColors()
        {

            this.RouletteNumbersColors = new int[37];

            for (int i = 0; i < 37; i++)
            {
                this.RouletteNumbersColors[winfinder.RouletteNumbers[i, 0]] = winfinder.RouletteNumbers[i, 1];
            }
        }

        private void DisplayWinnerNumber(int winner)
        {
            if (winner != lastDisplayedWinner)
            {
                if (winner != -1)
                {
                    lblDisplayWinner.Text = winner.ToString();
                    switch (RouletteNumbersColors[winner])
                    {
                        case 0:
                            lblDisplayWinner.BackColor = Color.Black;
                            lblDisplayWinner.ForeColor = Color.White;
                            break;
                        case 1:
                            lblDisplayWinner.BackColor = Color.Red;
                            lblDisplayWinner.ForeColor = Color.White;
                            break;
                        case 2:
                            lblDisplayWinner.BackColor = Color.Green;
                            lblDisplayWinner.ForeColor = Color.White;
                            break;
                        default:
                            lblDisplayWinner.BackColor = Color.White;
                            lblDisplayWinner.ForeColor = Color.Black;
                            break;
                    }
                }
                else
                {
                    lblDisplayWinner.Text = "--";
                    lblDisplayWinner.BackColor = Color.White;
                    lblDisplayWinner.ForeColor = Color.Black;
                    //                    lblDisplayWinner.Text = (winner == -1) ? "--" : winner.ToString();
                    lblWinCount.Text = juego.GetContadorNumeroGanador().ToString();
                }
                lastDisplayedWinner = winner;
            }
        }


        private void DisplayStatuses()
        {
            //if (this._rpm != this.lastrpm)
            {
                lblEstadoRuleta.Text = string.Format("{0}-{1}", (this._isMoving ? "M" : "S"), this._rpm);
                this.lastrpm = this._rpm;
            }
            //if (this.lastBallFound != this.bDebouncedBallFound)
            {
                lblBallOn.Text = this.bDebouncedBallFound ? "B " : "NB";
                //this.lastBallFound = this.bDebouncedBallFound;
            }
            if (this.lastEstadoMesa != estadoMesa.ToString())
            {
                lblGameStatus.Text = estadoMesa.ToString();
                this.lastEstadoMesa = estadoMesa.ToString();
            }
        }

        private void tmrMain_Tick(object sender, EventArgs e)
        {
            int winner = -1;
            this._timerTicks++;
            if (this._timerTicks > 1)
            {
                this._timerTicks = 0;
                int framesReceived = videoSourcePlayer1.VideoSource.FramesReceived;
                //if (this._rpmCounter > 0)
                //{
                //    // 1 min = 60000 msec => 60000 msec / 40 msec = 1500
                //    if (this._lastRpmCounter != 0)
                //    {
                //        float delta = _lastRpmCounter / _rpmCounter;
                //        if ((delta > 2) || (delta < 0.8))
                //            _rpmCounter = _lastRpmCounter;
                //    }
                //    this._rpm = 1500 / this._rpmCounter;
                //    _lastRpmCounter = _rpmCounter;
                //    this._rpmCounter = 0;
                //}
                //    this.lblTimingValue.Text = framesReceived.ToString();
                //    this.lblEventCalls.Text = string.Format($"{this._callCounter}-{this._zeroesCounter}-{this._zeroAtNoonCounter}");

                this._callCounter = 0;
                this._zeroesCounter = 0;
                this._zeroAtNoonCounter = 0;
            }

            this.estadoMesa = juego.GetGameState(this._rpm, this.IsCameraOn, this.bDebouncedBallFound);
            if (estadoMesa == JuegoRuleta.ESTADO_JUEGO.WINNING_NUMBER)
            {
                winner = juego.GetCurrentWinnerNumber();
                if (juego.GetCurrentWinnerNumberCmd() == JuegoRuleta.WINNER_CMD_TYPE.WINNER_NUMBER_CMD)
                    this.GuardarNumeroGanador(winner);
                else
                    this.GuardarEstado(estadoMesa, winner, this._rpm, 0);
            }
            else
            {
                this.GuardarEstado(estadoMesa, juego.GetLastWinnerNumber(), this._rpm, 0);
            }
            DisplayWinnerNumber(winner);
            DisplayStatuses();
        }
        #endregion

        #region Demo Timer
        private void tmrDemo_Tick(object sender, EventArgs e)
        {
            //try
            //{
            //    string cadena = string.Empty;
            //    ++this.estadoDemo;
            //    if (this.estadoDemo > 5)
            //        this.estadoDemo = 1;

            //    switch (this.estadoDemo)
            //    {
            //        case 1:
            //            //cadena = "NS" + this.numeroDemo.ToString("00") + "1" + this.azarNumero.Next(0, 100).ToString("00") + this.azarNumero.Next(0, 2).ToString() + "0";
            //            GuardarEstado(this.estadoDemo, this.numeroDemo, this._rpm, this.azarNumero.Next(0, 2));
            //            this.tmrDemo.Interval = 100;
            //            break;
            //        case 2:
            //            //cadena = "NS" + this.numeroDemo.ToString("00") + "2" + this.azarNumero.Next(0, 100).ToString("00") + this.azarNumero.Next(0, 2).ToString() + "0";
            //            GuardarEstado(this.estadoDemo, this.numeroDemo, this._rpm, this.azarNumero.Next(0, 2));
            //            this.tmrDemo.Interval = 6000;
            //            break;
            //        case 3:
            //            //cadena = "NS" + this.numeroDemo.ToString("00") + "3" + this.azarNumero.Next(0, 100).ToString("00") + this.azarNumero.Next(0, 2).ToString() + "0";
            //            GuardarEstado(this.estadoDemo, this.numeroDemo, this._rpm, this.azarNumero.Next(0, 2));
            //            this.tmrDemo.Interval = 6000;
            //            break;
            //        case 4:
            //            //cadena = "NS" + this.numeroDemo.ToString("00") + "4" + this.azarNumero.Next(0, 100).ToString("00") + this.azarNumero.Next(0, 2).ToString() + "0";
            //            GuardarEstado(this.estadoDemo, this.numeroDemo, this._rpm, this.azarNumero.Next(0, 2));
            //            this.tmrDemo.Interval = 10000;
            //            break;
            //        case 5:
            //            //Persistencia.Guardar("NS" + this.numeroDemo.ToString("00") + "5" + this.azarNumero.Next(0, 100).ToString("00") + this.azarNumero.Next(0, 2).ToString() + "0");
            //            this.numeroDemo = (byte)this.azarNumero.Next(0, 37);
            //            GuardarEstado(this.estadoDemo, this.numeroDemo, this._rpm, this.azarNumero.Next(0, 2));
            //            GuardarNumeroGanador(this.numeroDemo);
            //            this.tmrDemo.Interval = 1000;
            //            break;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    int num = 0 + 1;
            //}
        }
        #endregion

        #region Populate Numbers Playlist
        //===================== Functions for Checkbox Array ======================
        // Result of the event click Checkbox
        public void ClickCheckBox(Object sender, System.EventArgs e)
        {
            System.Windows.Forms.CheckBox chkBox = (System.Windows.Forms.CheckBox)sender;

            // Ignore direct click
            if (chkBox.CheckState == CheckState.Checked)
            {
                chkBox.CheckState = CheckState.Unchecked;
            }
        }

        private void ShowNumbersCheckBox()
        {
            int xPos = 6;
            int yPos = 192;
            int chkNum = 37; // 37 numeros

            int n = 0;
            while (n < chkNum)
            {
                chkbNumbers[n].Tag = winfinder.RouletteNumbers[n,0];
                chkbNumbers[n].Width = 40;
                chkbNumbers[n].Height = 18;
                chkbNumbers[n].Font = new Font(chkbNumbers[n].Font, System.Drawing.FontStyle.Bold);

                switch (winfinder.RouletteNumbers[n, 1])
                {
                    case 0:
                        chkbNumbers[n].ForeColor = Color.Black;
                        break;
                    case 1:
                        chkbNumbers[n].ForeColor = Color.Red;
                        break;
                    case 2:
                        chkbNumbers[n].ForeColor = Color.Green;
                        break;

                }

                chkbNumbers[n].Text = chkbNumbers[n].Tag.ToString();
                chkbNumbers[n].Left = xPos;
                chkbNumbers[n].Top = yPos;
                if (xPos > 6)
                {
                    // Two checkbox per row
                    yPos = yPos + chkbNumbers[n].Height + 1;
                    xPos = 6;
                }
                else
                    xPos = xPos + chkbNumbers[n].Width + 2;

                pnlCalibration.Controls.Add(chkbNumbers[n]); // Let panel hold the Checkbox
                                                             // the Event of click Checkbox
                chkbNumbers[n].Click += new System.EventHandler(ClickCheckBox);
                chkbNumbers[n].CheckState = CheckState.Unchecked;
                n++;

            }
        }

        #endregion

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblBallPosAngle = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.lblDistZeroBall = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.lblBolaPosY = new System.Windows.Forms.Label();
            this.lblBolaPosX = new System.Windows.Forms.Label();
            this.lblZeroPosAngle = new System.Windows.Forms.Label();
            this.lblZeroPosY = new System.Windows.Forms.Label();
            this.lblZeroPosX = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.pbBall = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pbZero = new System.Windows.Forms.PictureBox();
            this.videoSourcePlayer1 = new Accord.Controls.VideoSourcePlayer();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label16 = new System.Windows.Forms.Label();
            this.lblEventCalls = new System.Windows.Forms.Label();
            this.chkbGuardarLog = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.lblGameStatus = new System.Windows.Forms.Label();
            this.btnSaveCSV = new System.Windows.Forms.Button();
            this.lblTiming = new System.Windows.Forms.Label();
            this.lblTimingValue = new System.Windows.Forms.Label();
            this.lblVideoStatus = new System.Windows.Forms.Label();
            this.lblDisplayWinner = new System.Windows.Forms.Label();
            this.lblWinCount = new System.Windows.Forms.Label();
            this.lblEstadoRuleta = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.lblFound = new System.Windows.Forms.Label();
            this.cbCalibrateNumbers = new System.Windows.Forms.CheckBox();
            this.lblFPS = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lblBallOn = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.cbCalibrateCamera = new System.Windows.Forms.CheckBox();
            this.btnStartCamara = new System.Windows.Forms.Button();
            this.btnUpdateRGB = new System.Windows.Forms.Button();
            this.tmrDemo = new System.Windows.Forms.Timer(this.components);
            this.txtProtocolo = new System.Windows.Forms.TextBox();
            this.tmrMain = new System.Windows.Forms.Timer(this.components);
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.mnuSystemTray = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cerrarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlCalibration = new System.Windows.Forms.GroupBox();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.label23 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.numUpDownBlue = new System.Windows.Forms.NumericUpDown();
            this.numUpDownGreen = new System.Windows.Forms.NumericUpDown();
            this.numUpDownRed = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.lblChkCount = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.lblAvgY = new System.Windows.Forms.Label();
            this.lblAvgX = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.lblAvgAngle = new System.Windows.Forms.Label();
            this.lblAvgDist = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.btnCalibrateNumber = new System.Windows.Forms.Button();
            this.lblTestCount = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.btnSetNumber = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbBall)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbZero)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.mnuSystemTray.SuspendLayout();
            this.pnlCalibration.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownBlue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownGreen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownRed)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblBallPosAngle);
            this.groupBox1.Controls.Add(this.label25);
            this.groupBox1.Controls.Add(this.lblDistZeroBall);
            this.groupBox1.Controls.Add(this.label20);
            this.groupBox1.Controls.Add(this.lblBolaPosY);
            this.groupBox1.Controls.Add(this.lblBolaPosX);
            this.groupBox1.Controls.Add(this.lblZeroPosAngle);
            this.groupBox1.Controls.Add(this.lblZeroPosY);
            this.groupBox1.Controls.Add(this.lblZeroPosX);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.pbBall);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.pbZero);
            this.groupBox1.Location = new System.Drawing.Point(8, 501);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(662, 251);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // lblBallPosAngle
            // 
            this.lblBallPosAngle.AutoSize = true;
            this.lblBallPosAngle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBallPosAngle.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblBallPosAngle.Location = new System.Drawing.Point(520, 17);
            this.lblBallPosAngle.Name = "lblBallPosAngle";
            this.lblBallPosAngle.Size = new System.Drawing.Size(22, 15);
            this.lblBallPosAngle.TabIndex = 53;
            this.lblBallPosAngle.Text = "---";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label25.Location = new System.Drawing.Point(433, 19);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(88, 13);
            this.label25.TabIndex = 52;
            this.label25.Text = "Angulo al Centro:";
            // 
            // lblDistZeroBall
            // 
            this.lblDistZeroBall.AutoSize = true;
            this.lblDistZeroBall.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDistZeroBall.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblDistZeroBall.Location = new System.Drawing.Point(625, 17);
            this.lblDistZeroBall.Name = "lblDistZeroBall";
            this.lblDistZeroBall.Size = new System.Drawing.Size(22, 15);
            this.lblDistZeroBall.TabIndex = 51;
            this.lblDistZeroBall.Text = "---";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label20.Location = new System.Drawing.Point(552, 19);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(67, 13);
            this.label20.TabIndex = 50;
            this.label20.Text = "Dist. al Cero:";
            // 
            // lblBolaPosY
            // 
            this.lblBolaPosY.AutoSize = true;
            this.lblBolaPosY.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBolaPosY.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblBolaPosY.Location = new System.Drawing.Point(402, 17);
            this.lblBolaPosY.Name = "lblBolaPosY";
            this.lblBolaPosY.Size = new System.Drawing.Size(22, 15);
            this.lblBolaPosY.TabIndex = 49;
            this.lblBolaPosY.Text = "---";
            // 
            // lblBolaPosX
            // 
            this.lblBolaPosX.AutoSize = true;
            this.lblBolaPosX.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBolaPosX.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblBolaPosX.Location = new System.Drawing.Point(358, 17);
            this.lblBolaPosX.Name = "lblBolaPosX";
            this.lblBolaPosX.Size = new System.Drawing.Size(17, 15);
            this.lblBolaPosX.TabIndex = 48;
            this.lblBolaPosX.Text = "--";
            // 
            // lblZeroPosAngle
            // 
            this.lblZeroPosAngle.AutoSize = true;
            this.lblZeroPosAngle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblZeroPosAngle.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblZeroPosAngle.Location = new System.Drawing.Point(210, 17);
            this.lblZeroPosAngle.Name = "lblZeroPosAngle";
            this.lblZeroPosAngle.Size = new System.Drawing.Size(22, 15);
            this.lblZeroPosAngle.TabIndex = 47;
            this.lblZeroPosAngle.Text = "---";
            // 
            // lblZeroPosY
            // 
            this.lblZeroPosY.AutoSize = true;
            this.lblZeroPosY.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblZeroPosY.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblZeroPosY.Location = new System.Drawing.Point(86, 17);
            this.lblZeroPosY.Name = "lblZeroPosY";
            this.lblZeroPosY.Size = new System.Drawing.Size(22, 15);
            this.lblZeroPosY.TabIndex = 46;
            this.lblZeroPosY.Text = "---";
            // 
            // lblZeroPosX
            // 
            this.lblZeroPosX.AutoSize = true;
            this.lblZeroPosX.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblZeroPosX.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblZeroPosX.Location = new System.Drawing.Point(31, 17);
            this.lblZeroPosX.Name = "lblZeroPosX";
            this.lblZeroPosX.Size = new System.Drawing.Size(22, 15);
            this.lblZeroPosX.TabIndex = 45;
            this.lblZeroPosX.Text = "---";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label6.Location = new System.Drawing.Point(338, -1);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(80, 13);
            this.label6.TabIndex = 42;
            this.label6.Text = "Detección Bola";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label4.Location = new System.Drawing.Point(387, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 13);
            this.label4.TabIndex = 39;
            this.label4.Text = "Y:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label5.Location = new System.Drawing.Point(341, 19);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(17, 13);
            this.label5.TabIndex = 37;
            this.label5.Text = "X:";
            // 
            // pbBall
            // 
            this.pbBall.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbBall.Location = new System.Drawing.Point(337, 41);
            this.pbBall.Name = "pbBall";
            this.pbBall.Size = new System.Drawing.Size(320, 200);
            this.pbBall.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbBall.TabIndex = 35;
            this.pbBall.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label3.Location = new System.Drawing.Point(8, -1);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 13);
            this.label3.TabIndex = 24;
            this.label3.Text = "Detección del Cero";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label14.Location = new System.Drawing.Point(116, 19);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(88, 13);
            this.label14.TabIndex = 30;
            this.label14.Text = "Angulo al Centro:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label2.Location = new System.Drawing.Point(66, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 13);
            this.label2.TabIndex = 28;
            this.label2.Text = "Y:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label1.Location = new System.Drawing.Point(8, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 13);
            this.label1.TabIndex = 26;
            this.label1.Text = "X:";
            // 
            // pbZero
            // 
            this.pbZero.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbZero.Location = new System.Drawing.Point(11, 41);
            this.pbZero.Name = "pbZero";
            this.pbZero.Size = new System.Drawing.Size(320, 200);
            this.pbZero.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbZero.TabIndex = 12;
            this.pbZero.TabStop = false;
            // 
            // videoSourcePlayer1
            // 
            this.videoSourcePlayer1.Location = new System.Drawing.Point(11, 15);
            this.videoSourcePlayer1.Name = "videoSourcePlayer1";
            this.videoSourcePlayer1.Size = new System.Drawing.Size(640, 400);
            this.videoSourcePlayer1.TabIndex = 4;
            this.videoSourcePlayer1.Text = "l";
            this.videoSourcePlayer1.VideoSource = null;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label16);
            this.groupBox3.Controls.Add(this.lblEventCalls);
            this.groupBox3.Controls.Add(this.chkbGuardarLog);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.lblGameStatus);
            this.groupBox3.Controls.Add(this.btnSaveCSV);
            this.groupBox3.Controls.Add(this.lblTiming);
            this.groupBox3.Controls.Add(this.lblTimingValue);
            this.groupBox3.Controls.Add(this.lblVideoStatus);
            this.groupBox3.Controls.Add(this.lblDisplayWinner);
            this.groupBox3.Controls.Add(this.lblWinCount);
            this.groupBox3.Controls.Add(this.lblEstadoRuleta);
            this.groupBox3.Controls.Add(this.label15);
            this.groupBox3.Controls.Add(this.lblFound);
            this.groupBox3.Controls.Add(this.cbCalibrateNumbers);
            this.groupBox3.Controls.Add(this.lblFPS);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.lblBallOn);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.cbCalibrateCamera);
            this.groupBox3.Controls.Add(this.btnStartCamara);
            this.groupBox3.Controls.Add(this.videoSourcePlayer1);
            this.groupBox3.ForeColor = System.Drawing.SystemColors.WindowText;
            this.groupBox3.Location = new System.Drawing.Point(8, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(662, 492);
            this.groupBox3.TabIndex = 13;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Cámara en Vivo";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label16.Location = new System.Drawing.Point(600, 461);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(53, 13);
            this.label16.TabIndex = 137;
            this.label16.Text = "C -  Z -  N";
            this.label16.Visible = false;
            // 
            // lblEventCalls
            // 
            this.lblEventCalls.AutoSize = true;
            this.lblEventCalls.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEventCalls.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblEventCalls.Location = new System.Drawing.Point(592, 474);
            this.lblEventCalls.Name = "lblEventCalls";
            this.lblEventCalls.Size = new System.Drawing.Size(65, 15);
            this.lblEventCalls.TabIndex = 136;
            this.lblEventCalls.Text = "00-00-00";
            this.lblEventCalls.Visible = false;
            // 
            // chkbGuardarLog
            // 
            this.chkbGuardarLog.AutoSize = true;
            this.chkbGuardarLog.ForeColor = System.Drawing.SystemColors.WindowText;
            this.chkbGuardarLog.Location = new System.Drawing.Point(212, 442);
            this.chkbGuardarLog.Name = "chkbGuardarLog";
            this.chkbGuardarLog.Size = new System.Drawing.Size(85, 17);
            this.chkbGuardarLog.TabIndex = 52;
            this.chkbGuardarLog.Text = "Guardar Log";
            this.chkbGuardarLog.UseVisualStyleBackColor = true;
            this.chkbGuardarLog.CheckedChanged += new System.EventHandler(this.chkbGuardarLog_CheckedChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label7.Location = new System.Drawing.Point(338, 452);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(28, 13);
            this.label7.TabIndex = 135;
            this.label7.Text = "Hits:";
            // 
            // lblGameStatus
            // 
            this.lblGameStatus.AutoSize = true;
            this.lblGameStatus.Font = new System.Drawing.Font("Microsoft Tai Le", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGameStatus.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblGameStatus.Location = new System.Drawing.Point(87, 463);
            this.lblGameStatus.Name = "lblGameStatus";
            this.lblGameStatus.Size = new System.Drawing.Size(26, 23);
            this.lblGameStatus.TabIndex = 134;
            this.lblGameStatus.Text = "--";
            // 
            // btnSaveCSV
            // 
            this.btnSaveCSV.Location = new System.Drawing.Point(383, 460);
            this.btnSaveCSV.Name = "btnSaveCSV";
            this.btnSaveCSV.Size = new System.Drawing.Size(100, 23);
            this.btnSaveCSV.TabIndex = 133;
            this.btnSaveCSV.Text = "Guardar CSV";
            this.btnSaveCSV.UseVisualStyleBackColor = true;
            this.btnSaveCSV.Visible = false;
            this.btnSaveCSV.Click += new System.EventHandler(this.btnSaveCSV_Click);
            // 
            // lblTiming
            // 
            this.lblTiming.AutoSize = true;
            this.lblTiming.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblTiming.Location = new System.Drawing.Point(545, 442);
            this.lblTiming.Name = "lblTiming";
            this.lblTiming.Size = new System.Drawing.Size(74, 13);
            this.lblTiming.TabIndex = 132;
            this.lblTiming.Text = "Frames (12.5):";
            this.lblTiming.Visible = false;
            // 
            // lblTimingValue
            // 
            this.lblTimingValue.AutoSize = true;
            this.lblTimingValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTimingValue.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblTimingValue.Location = new System.Drawing.Point(625, 440);
            this.lblTimingValue.Name = "lblTimingValue";
            this.lblTimingValue.Size = new System.Drawing.Size(22, 15);
            this.lblTimingValue.TabIndex = 131;
            this.lblTimingValue.Text = "---";
            this.lblTimingValue.Visible = false;
            // 
            // lblVideoStatus
            // 
            this.lblVideoStatus.AutoSize = true;
            this.lblVideoStatus.BackColor = System.Drawing.Color.DarkRed;
            this.lblVideoStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVideoStatus.ForeColor = System.Drawing.Color.White;
            this.lblVideoStatus.Location = new System.Drawing.Point(116, 427);
            this.lblVideoStatus.Name = "lblVideoStatus";
            this.lblVideoStatus.Size = new System.Drawing.Size(37, 16);
            this.lblVideoStatus.TabIndex = 130;
            this.lblVideoStatus.Text = "OFF";
            // 
            // lblDisplayWinner
            // 
            this.lblDisplayWinner.AutoSize = true;
            this.lblDisplayWinner.Font = new System.Drawing.Font("Microsoft Tai Le", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDisplayWinner.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblDisplayWinner.Location = new System.Drawing.Point(301, 418);
            this.lblDisplayWinner.Name = "lblDisplayWinner";
            this.lblDisplayWinner.Size = new System.Drawing.Size(31, 29);
            this.lblDisplayWinner.TabIndex = 129;
            this.lblDisplayWinner.Text = "--";
            // 
            // lblWinCount
            // 
            this.lblWinCount.AutoSize = true;
            this.lblWinCount.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblWinCount.Location = new System.Drawing.Point(341, 468);
            this.lblWinCount.Name = "lblWinCount";
            this.lblWinCount.Size = new System.Drawing.Size(13, 13);
            this.lblWinCount.TabIndex = 67;
            this.lblWinCount.Text = "0";
            // 
            // lblEstadoRuleta
            // 
            this.lblEstadoRuleta.AutoSize = true;
            this.lblEstadoRuleta.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblEstadoRuleta.Location = new System.Drawing.Point(588, 422);
            this.lblEstadoRuleta.Name = "lblEstadoRuleta";
            this.lblEstadoRuleta.Size = new System.Drawing.Size(43, 13);
            this.lblEstadoRuleta.TabIndex = 64;
            this.lblEstadoRuleta.Text = "M-RPM";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label15.Location = new System.Drawing.Point(302, 452);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(36, 13);
            this.label15.TabIndex = 63;
            this.label15.Text = "Leido:";
            // 
            // lblFound
            // 
            this.lblFound.AutoSize = true;
            this.lblFound.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFound.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblFound.Location = new System.Drawing.Point(307, 467);
            this.lblFound.Name = "lblFound";
            this.lblFound.Size = new System.Drawing.Size(18, 16);
            this.lblFound.TabIndex = 62;
            this.lblFound.Text = "--";
            // 
            // cbCalibrateNumbers
            // 
            this.cbCalibrateNumbers.AutoSize = true;
            this.cbCalibrateNumbers.ForeColor = System.Drawing.SystemColors.WindowText;
            this.cbCalibrateNumbers.Location = new System.Drawing.Point(383, 441);
            this.cbCalibrateNumbers.Name = "cbCalibrateNumbers";
            this.cbCalibrateNumbers.Size = new System.Drawing.Size(106, 17);
            this.cbCalibrateNumbers.TabIndex = 60;
            this.cbCalibrateNumbers.Text = "Calibrar Números";
            this.cbCalibrateNumbers.UseVisualStyleBackColor = true;
            this.cbCalibrateNumbers.CheckedChanged += new System.EventHandler(this.cbCalibrateNumbers_CheckedChanged);
            // 
            // lblFPS
            // 
            this.lblFPS.AutoSize = true;
            this.lblFPS.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblFPS.Location = new System.Drawing.Point(518, 442);
            this.lblFPS.Name = "lblFPS";
            this.lblFPS.Size = new System.Drawing.Size(21, 13);
            this.lblFPS.TabIndex = 58;
            this.lblFPS.Text = "fps";
            this.lblFPS.Visible = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label8.Location = new System.Drawing.Point(209, 425);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(88, 13);
            this.label8.TabIndex = 56;
            this.label8.Text = "Display Ganador:";
            // 
            // lblBallOn
            // 
            this.lblBallOn.AutoSize = true;
            this.lblBallOn.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblBallOn.Location = new System.Drawing.Point(627, 422);
            this.lblBallOn.Name = "lblBallOn";
            this.lblBallOn.Size = new System.Drawing.Size(22, 13);
            this.lblBallOn.TabIndex = 54;
            this.lblBallOn.Text = "NB";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label11.Location = new System.Drawing.Point(6, 468);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(75, 13);
            this.label11.TabIndex = 55;
            this.label11.Text = "Estado Juego:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label10.Location = new System.Drawing.Point(508, 422);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(77, 13);
            this.label10.TabIndex = 36;
            this.label10.Text = "Estado Ruleta:";
            // 
            // cbCalibrateCamera
            // 
            this.cbCalibrateCamera.AutoSize = true;
            this.cbCalibrateCamera.ForeColor = System.Drawing.SystemColors.WindowText;
            this.cbCalibrateCamera.Location = new System.Drawing.Point(383, 421);
            this.cbCalibrateCamera.Name = "cbCalibrateCamera";
            this.cbCalibrateCamera.Size = new System.Drawing.Size(100, 17);
            this.cbCalibrateCamera.TabIndex = 13;
            this.cbCalibrateCamera.Text = "Calibrar Camara";
            this.cbCalibrateCamera.UseVisualStyleBackColor = true;
            this.cbCalibrateCamera.CheckedChanged += new System.EventHandler(this.cbCalibrateCamera_CheckedChanged);
            // 
            // btnStartCamara
            // 
            this.btnStartCamara.ForeColor = System.Drawing.SystemColors.WindowText;
            this.btnStartCamara.Location = new System.Drawing.Point(11, 420);
            this.btnStartCamara.Name = "btnStartCamara";
            this.btnStartCamara.Size = new System.Drawing.Size(99, 27);
            this.btnStartCamara.TabIndex = 11;
            this.btnStartCamara.Text = "Iniciar Captura";
            this.btnStartCamara.UseVisualStyleBackColor = true;
            this.btnStartCamara.Click += new System.EventHandler(this.btnStartCamara_Click);
            // 
            // btnUpdateRGB
            // 
            this.btnUpdateRGB.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnUpdateRGB.Enabled = false;
            this.btnUpdateRGB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpdateRGB.ForeColor = System.Drawing.Color.Black;
            this.btnUpdateRGB.Location = new System.Drawing.Point(6, 662);
            this.btnUpdateRGB.Name = "btnUpdateRGB";
            this.btnUpdateRGB.Size = new System.Drawing.Size(75, 23);
            this.btnUpdateRGB.TabIndex = 136;
            this.btnUpdateRGB.Text = "Grabar RGB";
            this.btnUpdateRGB.UseVisualStyleBackColor = true;
            this.btnUpdateRGB.Visible = false;
            this.btnUpdateRGB.Click += new System.EventHandler(this.btnUpdateRGB_Click);
            // 
            // tmrDemo
            // 
            this.tmrDemo.Interval = 500;
            this.tmrDemo.Tick += new System.EventHandler(this.tmrDemo_Tick);
            // 
            // txtProtocolo
            // 
            this.txtProtocolo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txtProtocolo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtProtocolo.Location = new System.Drawing.Point(676, 3);
            this.txtProtocolo.Multiline = true;
            this.txtProtocolo.Name = "txtProtocolo";
            this.txtProtocolo.Size = new System.Drawing.Size(85, 41);
            this.txtProtocolo.TabIndex = 52;
            // 
            // tmrMain
            // 
            this.tmrMain.Interval = 500;
            this.tmrMain.Tick += new System.EventHandler(this.tmrMain_Tick);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.BalloonTipText = "Recolector de datos.";
            this.notifyIcon1.BalloonTipTitle = "NAPSA";
            this.notifyIcon1.ContextMenuStrip = this.mnuSystemTray;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "Recolector de datos NAPSA";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // mnuSystemTray
            // 
            this.mnuSystemTray.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cerrarToolStripMenuItem});
            this.mnuSystemTray.Name = "contextMenuStrip1";
            this.mnuSystemTray.Size = new System.Drawing.Size(107, 26);
            // 
            // cerrarToolStripMenuItem
            // 
            this.cerrarToolStripMenuItem.Name = "cerrarToolStripMenuItem";
            this.cerrarToolStripMenuItem.Size = new System.Drawing.Size(106, 22);
            this.cerrarToolStripMenuItem.Text = "Cerrar";
            this.cerrarToolStripMenuItem.Click += new System.EventHandler(this.cerrarToolStripMenuItem_Click);
            // 
            // pnlCalibration
            // 
            this.pnlCalibration.Controls.Add(this.radioButton2);
            this.pnlCalibration.Controls.Add(this.btnUpdateRGB);
            this.pnlCalibration.Controls.Add(this.radioButton1);
            this.pnlCalibration.Controls.Add(this.label23);
            this.pnlCalibration.Controls.Add(this.label22);
            this.pnlCalibration.Controls.Add(this.label21);
            this.pnlCalibration.Controls.Add(this.numUpDownBlue);
            this.pnlCalibration.Controls.Add(this.numUpDownGreen);
            this.pnlCalibration.Controls.Add(this.numUpDownRed);
            this.pnlCalibration.Controls.Add(this.label9);
            this.pnlCalibration.Controls.Add(this.lblChkCount);
            this.pnlCalibration.Controls.Add(this.label17);
            this.pnlCalibration.Controls.Add(this.lblAvgY);
            this.pnlCalibration.Controls.Add(this.lblAvgX);
            this.pnlCalibration.Controls.Add(this.label18);
            this.pnlCalibration.Controls.Add(this.label19);
            this.pnlCalibration.Controls.Add(this.lblAvgAngle);
            this.pnlCalibration.Controls.Add(this.lblAvgDist);
            this.pnlCalibration.Controls.Add(this.comboBox1);
            this.pnlCalibration.Controls.Add(this.btnCalibrateNumber);
            this.pnlCalibration.Controls.Add(this.lblTestCount);
            this.pnlCalibration.Controls.Add(this.label13);
            this.pnlCalibration.Controls.Add(this.label12);
            this.pnlCalibration.Controls.Add(this.btnSetNumber);
            this.pnlCalibration.Location = new System.Drawing.Point(676, 50);
            this.pnlCalibration.Name = "pnlCalibration";
            this.pnlCalibration.Size = new System.Drawing.Size(85, 692);
            this.pnlCalibration.TabIndex = 62;
            this.pnlCalibration.TabStop = false;
            this.pnlCalibration.Visible = false;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton2.Location = new System.Drawing.Point(41, 565);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(41, 16);
            this.radioButton2.TabIndex = 134;
            this.radioButton2.Text = "Bola";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton1.Location = new System.Drawing.Point(10, 565);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(28, 16);
            this.radioButton1.TabIndex = 133;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "0";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label23.Location = new System.Drawing.Point(7, 638);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(17, 13);
            this.label23.TabIndex = 132;
            this.label23.Text = "B:";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label22.Location = new System.Drawing.Point(7, 613);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(18, 13);
            this.label22.TabIndex = 131;
            this.label22.Text = "G:";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label21.Location = new System.Drawing.Point(7, 590);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(18, 13);
            this.label21.TabIndex = 52;
            this.label21.Text = "R:";
            // 
            // numUpDownBlue
            // 
            this.numUpDownBlue.Location = new System.Drawing.Point(32, 636);
            this.numUpDownBlue.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numUpDownBlue.Name = "numUpDownBlue";
            this.numUpDownBlue.Size = new System.Drawing.Size(48, 20);
            this.numUpDownBlue.TabIndex = 130;
            this.numUpDownBlue.ValueChanged += new System.EventHandler(this.numUpDownBlue_ValueChanged);
            // 
            // numUpDownGreen
            // 
            this.numUpDownGreen.Location = new System.Drawing.Point(32, 611);
            this.numUpDownGreen.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numUpDownGreen.Name = "numUpDownGreen";
            this.numUpDownGreen.Size = new System.Drawing.Size(48, 20);
            this.numUpDownGreen.TabIndex = 129;
            this.numUpDownGreen.ValueChanged += new System.EventHandler(this.numUpDownGreen_ValueChanged);
            // 
            // numUpDownRed
            // 
            this.numUpDownRed.Location = new System.Drawing.Point(32, 588);
            this.numUpDownRed.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numUpDownRed.Name = "numUpDownRed";
            this.numUpDownRed.Size = new System.Drawing.Size(48, 20);
            this.numUpDownRed.TabIndex = 128;
            this.numUpDownRed.ValueChanged += new System.EventHandler(this.numUpDownRed_ValueChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label9.Location = new System.Drawing.Point(6, 136);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(58, 13);
            this.label9.TabIndex = 127;
            this.label9.Text = "Muestra #:";
            // 
            // lblChkCount
            // 
            this.lblChkCount.AutoSize = true;
            this.lblChkCount.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblChkCount.Location = new System.Drawing.Point(64, 35);
            this.lblChkCount.Name = "lblChkCount";
            this.lblChkCount.Size = new System.Drawing.Size(13, 13);
            this.lblChkCount.TabIndex = 126;
            this.lblChkCount.Text = "0";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label17.Location = new System.Drawing.Point(6, 35);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(59, 13);
            this.label17.TabIndex = 125;
            this.label17.Text = "Calibrados:";
            // 
            // lblAvgY
            // 
            this.lblAvgY.AutoSize = true;
            this.lblAvgY.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAvgY.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblAvgY.Location = new System.Drawing.Point(25, 93);
            this.lblAvgY.Name = "lblAvgY";
            this.lblAvgY.Size = new System.Drawing.Size(19, 13);
            this.lblAvgY.TabIndex = 124;
            this.lblAvgY.Text = "---";
            this.lblAvgY.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblAvgX
            // 
            this.lblAvgX.AutoSize = true;
            this.lblAvgX.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAvgX.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblAvgX.Location = new System.Drawing.Point(25, 81);
            this.lblAvgX.Name = "lblAvgX";
            this.lblAvgX.Size = new System.Drawing.Size(19, 13);
            this.lblAvgX.TabIndex = 123;
            this.lblAvgX.Text = "---";
            this.lblAvgX.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label18.Location = new System.Drawing.Point(6, 93);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(17, 13);
            this.label18.TabIndex = 122;
            this.label18.Text = "Y:";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label19.Location = new System.Drawing.Point(6, 81);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(17, 13);
            this.label19.TabIndex = 121;
            this.label19.Text = "X:";
            // 
            // lblAvgAngle
            // 
            this.lblAvgAngle.AutoSize = true;
            this.lblAvgAngle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAvgAngle.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblAvgAngle.Location = new System.Drawing.Point(25, 106);
            this.lblAvgAngle.Name = "lblAvgAngle";
            this.lblAvgAngle.Size = new System.Drawing.Size(19, 13);
            this.lblAvgAngle.TabIndex = 120;
            this.lblAvgAngle.Text = "---";
            this.lblAvgAngle.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblAvgDist
            // 
            this.lblAvgDist.AutoSize = true;
            this.lblAvgDist.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAvgDist.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblAvgDist.Location = new System.Drawing.Point(25, 120);
            this.lblAvgDist.Name = "lblAvgDist";
            this.lblAvgDist.Size = new System.Drawing.Size(19, 13);
            this.lblAvgDist.TabIndex = 119;
            this.lblAvgDist.Text = "---";
            this.lblAvgDist.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(4, 11);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(73, 21);
            this.comboBox1.TabIndex = 63;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // btnCalibrateNumber
            // 
            this.btnCalibrateNumber.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCalibrateNumber.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCalibrateNumber.ForeColor = System.Drawing.Color.Black;
            this.btnCalibrateNumber.Location = new System.Drawing.Point(4, 51);
            this.btnCalibrateNumber.Name = "btnCalibrateNumber";
            this.btnCalibrateNumber.Size = new System.Drawing.Size(73, 27);
            this.btnCalibrateNumber.TabIndex = 64;
            this.btnCalibrateNumber.Text = "Calibrar";
            this.btnCalibrateNumber.UseVisualStyleBackColor = true;
            this.btnCalibrateNumber.Click += new System.EventHandler(this.btnCalibrateNumber_Click);
            // 
            // lblTestCount
            // 
            this.lblTestCount.AutoSize = true;
            this.lblTestCount.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblTestCount.Location = new System.Drawing.Point(63, 136);
            this.lblTestCount.Name = "lblTestCount";
            this.lblTestCount.Size = new System.Drawing.Size(13, 13);
            this.lblTestCount.TabIndex = 70;
            this.lblTestCount.Text = "0";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label13.Location = new System.Drawing.Point(7, 106);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(17, 13);
            this.label13.TabIndex = 68;
            this.label13.Text = "A:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label12.Location = new System.Drawing.Point(6, 120);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(18, 13);
            this.label12.TabIndex = 67;
            this.label12.Text = "D:";
            // 
            // btnSetNumber
            // 
            this.btnSetNumber.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnSetNumber.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSetNumber.ForeColor = System.Drawing.Color.Black;
            this.btnSetNumber.Location = new System.Drawing.Point(6, 152);
            this.btnSetNumber.Name = "btnSetNumber";
            this.btnSetNumber.Size = new System.Drawing.Size(74, 38);
            this.btnSetNumber.TabIndex = 65;
            this.btnSetNumber.Text = "Guardar Calibracion";
            this.btnSetNumber.UseVisualStyleBackColor = true;
            this.btnSetNumber.Click += new System.EventHandler(this.btnSetNumber_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(768, 749);
            this.Controls.Add(this.pnlCalibration);
            this.Controls.Add(this.txtProtocolo);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.ForeColor = System.Drawing.Color.DarkRed;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Panel del Video Recolector IV";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbBall)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbZero)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.mnuSystemTray.ResumeLayout(false);
            this.pnlCalibration.ResumeLayout(false);
            this.pnlCalibration.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownBlue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownGreen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownRed)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private Accord.Controls.VideoSourcePlayer videoSourcePlayer1;
        private System.Windows.Forms.PictureBox pbZero;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnStartCamara;
        private System.Windows.Forms.CheckBox cbCalibrateCamera;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Timer tmrDemo;
        private System.Windows.Forms.TextBox txtProtocolo;
        private System.Windows.Forms.Timer tmrMain;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lblBallOn;
        private System.Windows.Forms.Label lblFPS;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private Label label14;
        private GroupBox pnlCalibration;
        private Label lblTestCount;
        private Button btnCalibrateNumber;
        private Label label13;
        private Label label12;
        private Button btnSetNumber;
        private CheckBox cbCalibrateNumbers;
        private ContextMenuStrip mnuSystemTray;
        private ToolStripMenuItem cerrarToolStripMenuItem;
        private Label lblFound;
        private Label label15;
        private CheckBox[] chkbNumbers; // Declaring array of Button
        private ComboBox comboBox1;
        private Label lblAvgAngle;
        private Label lblAvgDist;
        private Label lblAvgY;
        private Label lblAvgX;
        private Label label18;
        private Label label19;
        private Label label6;
        private Label label4;
        private Label label5;
        private PictureBox pbBall;
        private Label lblEstadoRuleta;
        private Label lblChkCount;
        private Label label17;
        private Label lblWinCount;
        private Label label9;
        private Label lblDisplayWinner;
        private Label lblVideoStatus;
        private Label lblZeroPosX;
        private Label lblZeroPosY;
        private Label lblZeroPosAngle;
        private Label lblBolaPosX;
        private Label lblBolaPosY;
        private Label label20;
        private Label lblDistZeroBall;
        private Label lblTiming;
        private Label lblTimingValue;
        private Button btnSaveCSV;
        private Label lblGameStatus;
        private Label label7;
        private NumericUpDown numUpDownBlue;
        private NumericUpDown numUpDownGreen;
        private NumericUpDown numUpDownRed;
        private Label label23;
        private Label label22;
        private Label label21;
        private RadioButton radioButton2;
        private RadioButton radioButton1;
        private Button btnUpdateRGB;
        private CheckBox chkbGuardarLog;
        private Label label16;
        private Label lblEventCalls;
        private Label lblBallPosAngle;
        private Label label25;
    }
}

