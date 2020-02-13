using Accord.Video;
using DASYS.Recolector.BLL;
using System;
using System.Diagnostics;
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

        #region Form_Load, Closing
        private void MainForm_Load(object sender, EventArgs e)
        {
            this.azarNumero = new Random((int)DateTime.Now.Ticks);
            this.LeerUltimoNumero();
            if (Pase.UltimoPase == null)
                Pase.UltimoPase = new Pase();
            this.estadoMesa = JuegoRuleta.ESTADO_JUEGO.STATE_0;
            this.IsCameraOn = false;
            this._rpm = 0;
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
        private void tmrMain_Tick(object sender, EventArgs e)
        {
            lblEstadoRuleta.Text = string.Format("{0}-{1}", (this._isMoving ? "M" : "S"), this._rpm);
            txtbGameStatus.Text = estadoMesa.ToString();
            txtbDisplayStatus.Text = Pase.ObtenerUltimoEstado().ToString();
            int winner = juego.GetCurrentWinnerNumber();

            txtWinner.Text = (winner == -1) ? "--" : winner.ToString();

            if (this._calibrateFlag || this.cbCalibrateNumbers.Checked)
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
            else
            {
                this.estadoMesa = juego.GetGameState(this._rpm, this.IsCameraOn, this.bDebouncedBallFound);
                if (estadoMesa == JuegoRuleta.ESTADO_JUEGO.WINNING_NUMBER)
                {
                    if (juego.GetCurrentWinnerNumberCmd() == JuegoRuleta.WINNER_CMD_TYPE.WINNER_NUMBER_CMD)
                        this.GuardarNumeroGanador(juego.GetCurrentWinnerNumber());
                    else
                        this.GuardarEstado((int)estadoMesa, juego.GetCurrentWinnerNumber(), this._rpm, 0);
                }
                else
                {
                    this.GuardarEstado((int)estadoMesa, juego.GetLastWinnerNumber(), this._rpm, 0);
                }
            }
        }
        #endregion

        #region Demo Timer
        private void tmrDemo_Tick(object sender, EventArgs e)
        {
            try
            {
                string cadena = string.Empty;
                ++this.estadoDemo;
                if (this.estadoDemo > 5)
                    this.estadoDemo = 1;

                switch (this.estadoDemo)
                {
                    case 1:
                        //cadena = "NS" + this.numeroDemo.ToString("00") + "1" + this.azarNumero.Next(0, 100).ToString("00") + this.azarNumero.Next(0, 2).ToString() + "0";
                        GuardarEstado(this.estadoDemo, this.numeroDemo, this._rpm, this.azarNumero.Next(0, 2));
                        this.tmrDemo.Interval = 100;
                        break;
                    case 2:
                        //cadena = "NS" + this.numeroDemo.ToString("00") + "2" + this.azarNumero.Next(0, 100).ToString("00") + this.azarNumero.Next(0, 2).ToString() + "0";
                        GuardarEstado(this.estadoDemo, this.numeroDemo, this._rpm, this.azarNumero.Next(0, 2));
                        this.tmrDemo.Interval = 6000;
                        break;
                    case 3:
                        //cadena = "NS" + this.numeroDemo.ToString("00") + "3" + this.azarNumero.Next(0, 100).ToString("00") + this.azarNumero.Next(0, 2).ToString() + "0";
                        GuardarEstado(this.estadoDemo, this.numeroDemo, this._rpm, this.azarNumero.Next(0, 2));
                        this.tmrDemo.Interval = 6000;
                        break;
                    case 4:
                        //cadena = "NS" + this.numeroDemo.ToString("00") + "4" + this.azarNumero.Next(0, 100).ToString("00") + this.azarNumero.Next(0, 2).ToString() + "0";
                        GuardarEstado(this.estadoDemo, this.numeroDemo, this._rpm, this.azarNumero.Next(0, 2));
                        this.tmrDemo.Interval = 10000;
                        break;
                    case 5:
                        //Persistencia.Guardar("NS" + this.numeroDemo.ToString("00") + "5" + this.azarNumero.Next(0, 100).ToString("00") + this.azarNumero.Next(0, 2).ToString() + "0");
                        this.numeroDemo = (byte)this.azarNumero.Next(0, 37);
                        GuardarEstado(this.estadoDemo, this.numeroDemo, this._rpm, this.azarNumero.Next(0, 2));
                        GuardarNumeroGanador(this.numeroDemo);
                        this.tmrDemo.Interval = 1000;
                        break;
                }
            }
            catch (Exception ex)
            {
                int num = 0 + 1;
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
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tbBolaPosY = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbBolaPosX = new System.Windows.Forms.TextBox();
            this.pbBall = new System.Windows.Forms.PictureBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tbBallAngleToCenter = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.tbZeroPosAngle = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbZeroPosY = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbZeroPosX = new System.Windows.Forms.TextBox();
            this.pbZero = new System.Windows.Forms.PictureBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.videoSourcePlayer1 = new Accord.Controls.VideoSourcePlayer();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label16 = new System.Windows.Forms.Label();
            this.txtbDisplayStatus = new System.Windows.Forms.TextBox();
            this.lblEstadoRuleta = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.lblWinner = new System.Windows.Forms.Label();
            this.cbCalibrateNumbers = new System.Windows.Forms.CheckBox();
            this.txtWinner = new System.Windows.Forms.TextBox();
            this.lblFPS = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lblBallOn = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.tbVideoStatus = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtbGameStatus = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cbCalibrateCamera = new System.Windows.Forms.CheckBox();
            this.btnStartCamara = new System.Windows.Forms.Button();
            this.btnIniciarDemo = new System.Windows.Forms.Button();
            this.tmrDemo = new System.Windows.Forms.Timer(this.components);
            this.txtProtocolo = new System.Windows.Forms.TextBox();
            this.tmrMain = new System.Windows.Forms.Timer(this.components);
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.mnuSystemTray = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cerrarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.lblAvgY = new System.Windows.Forms.Label();
            this.lblAvgX = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.lblAvgAngle = new System.Windows.Forms.Label();
            this.lblAvgDist = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.checkBox36 = new System.Windows.Forms.CheckBox();
            this.checkBox34 = new System.Windows.Forms.CheckBox();
            this.checkBox35 = new System.Windows.Forms.CheckBox();
            this.checkBox32 = new System.Windows.Forms.CheckBox();
            this.checkBox33 = new System.Windows.Forms.CheckBox();
            this.checkBox30 = new System.Windows.Forms.CheckBox();
            this.checkBox31 = new System.Windows.Forms.CheckBox();
            this.checkBox28 = new System.Windows.Forms.CheckBox();
            this.checkBox29 = new System.Windows.Forms.CheckBox();
            this.checkBox26 = new System.Windows.Forms.CheckBox();
            this.checkBox27 = new System.Windows.Forms.CheckBox();
            this.checkBox24 = new System.Windows.Forms.CheckBox();
            this.checkBox25 = new System.Windows.Forms.CheckBox();
            this.checkBox22 = new System.Windows.Forms.CheckBox();
            this.checkBox23 = new System.Windows.Forms.CheckBox();
            this.checkBox20 = new System.Windows.Forms.CheckBox();
            this.checkBox21 = new System.Windows.Forms.CheckBox();
            this.checkBox18 = new System.Windows.Forms.CheckBox();
            this.btnCalibrateNumber = new System.Windows.Forms.Button();
            this.checkBox19 = new System.Windows.Forms.CheckBox();
            this.checkBox17 = new System.Windows.Forms.CheckBox();
            this.checkBox16 = new System.Windows.Forms.CheckBox();
            this.checkBox15 = new System.Windows.Forms.CheckBox();
            this.checkBox14 = new System.Windows.Forms.CheckBox();
            this.checkBox13 = new System.Windows.Forms.CheckBox();
            this.checkBox12 = new System.Windows.Forms.CheckBox();
            this.checkBox11 = new System.Windows.Forms.CheckBox();
            this.checkBox10 = new System.Windows.Forms.CheckBox();
            this.checkBox9 = new System.Windows.Forms.CheckBox();
            this.checkBox8 = new System.Windows.Forms.CheckBox();
            this.checkBox7 = new System.Windows.Forms.CheckBox();
            this.checkBox6 = new System.Windows.Forms.CheckBox();
            this.checkBox5 = new System.Windows.Forms.CheckBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox0 = new System.Windows.Forms.CheckBox();
            this.lblTestCount = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.btnSetNumber = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbBall)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbZero)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.mnuSystemTray.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.tbBolaPosY);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.tbBolaPosX);
            this.groupBox1.Controls.Add(this.pbBall);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.tbBallAngleToCenter);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.tbZeroPosAngle);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.tbZeroPosY);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.tbZeroPosX);
            this.groupBox1.Controls.Add(this.pbZero);
            this.groupBox1.Location = new System.Drawing.Point(8, 501);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(662, 251);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
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
            this.label4.Location = new System.Drawing.Point(432, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 13);
            this.label4.TabIndex = 39;
            this.label4.Text = "Y:";
            // 
            // tbBolaPosY
            // 
            this.tbBolaPosY.Enabled = false;
            this.tbBolaPosY.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbBolaPosY.Location = new System.Drawing.Point(455, 15);
            this.tbBolaPosY.Name = "tbBolaPosY";
            this.tbBolaPosY.Size = new System.Drawing.Size(65, 22);
            this.tbBolaPosY.TabIndex = 38;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label5.Location = new System.Drawing.Point(338, 19);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(17, 13);
            this.label5.TabIndex = 37;
            this.label5.Text = "X:";
            // 
            // tbBolaPosX
            // 
            this.tbBolaPosX.Enabled = false;
            this.tbBolaPosX.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbBolaPosX.Location = new System.Drawing.Point(361, 15);
            this.tbBolaPosX.Name = "tbBolaPosX";
            this.tbBolaPosX.Size = new System.Drawing.Size(65, 22);
            this.tbBolaPosX.TabIndex = 36;
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
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label9.Location = new System.Drawing.Point(542, 19);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(43, 13);
            this.label9.TabIndex = 41;
            this.label9.Text = "Angulo:";
            // 
            // tbBallAngleToCenter
            // 
            this.tbBallAngleToCenter.Enabled = false;
            this.tbBallAngleToCenter.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbBallAngleToCenter.Location = new System.Drawing.Point(591, 15);
            this.tbBallAngleToCenter.Name = "tbBallAngleToCenter";
            this.tbBallAngleToCenter.Size = new System.Drawing.Size(58, 22);
            this.tbBallAngleToCenter.TabIndex = 40;
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
            this.label14.Location = new System.Drawing.Point(212, 19);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(43, 13);
            this.label14.TabIndex = 30;
            this.label14.Text = "Angulo:";
            // 
            // tbZeroPosAngle
            // 
            this.tbZeroPosAngle.Enabled = false;
            this.tbZeroPosAngle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbZeroPosAngle.Location = new System.Drawing.Point(260, 14);
            this.tbZeroPosAngle.Name = "tbZeroPosAngle";
            this.tbZeroPosAngle.Size = new System.Drawing.Size(65, 22);
            this.tbZeroPosAngle.TabIndex = 29;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label2.Location = new System.Drawing.Point(100, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 13);
            this.label2.TabIndex = 28;
            this.label2.Text = "Y:";
            // 
            // tbZeroPosY
            // 
            this.tbZeroPosY.Enabled = false;
            this.tbZeroPosY.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbZeroPosY.Location = new System.Drawing.Point(123, 14);
            this.tbZeroPosY.Name = "tbZeroPosY";
            this.tbZeroPosY.Size = new System.Drawing.Size(65, 22);
            this.tbZeroPosY.TabIndex = 27;
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
            // tbZeroPosX
            // 
            this.tbZeroPosX.Enabled = false;
            this.tbZeroPosX.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbZeroPosX.Location = new System.Drawing.Point(29, 14);
            this.tbZeroPosX.Name = "tbZeroPosX";
            this.tbZeroPosX.Size = new System.Drawing.Size(65, 22);
            this.tbZeroPosX.TabIndex = 25;
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
            // textBox1
            // 
            this.textBox1.Enabled = false;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(591, 421);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(58, 22);
            this.textBox1.TabIndex = 29;
            // 
            // videoSourcePlayer1
            // 
            this.videoSourcePlayer1.Location = new System.Drawing.Point(9, 15);
            this.videoSourcePlayer1.Name = "videoSourcePlayer1";
            this.videoSourcePlayer1.Size = new System.Drawing.Size(640, 400);
            this.videoSourcePlayer1.TabIndex = 4;
            this.videoSourcePlayer1.Text = "l";
            this.videoSourcePlayer1.VideoSource = null;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label16);
            this.groupBox3.Controls.Add(this.txtbDisplayStatus);
            this.groupBox3.Controls.Add(this.lblEstadoRuleta);
            this.groupBox3.Controls.Add(this.label15);
            this.groupBox3.Controls.Add(this.lblWinner);
            this.groupBox3.Controls.Add(this.cbCalibrateNumbers);
            this.groupBox3.Controls.Add(this.txtWinner);
            this.groupBox3.Controls.Add(this.lblFPS);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.lblBallOn);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.tbVideoStatus);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.txtbGameStatus);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.cbCalibrateCamera);
            this.groupBox3.Controls.Add(this.btnStartCamara);
            this.groupBox3.Controls.Add(this.textBox1);
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
            this.label16.Location = new System.Drawing.Point(403, 469);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(80, 13);
            this.label16.TabIndex = 66;
            this.label16.Text = "Estado Display:";
            // 
            // txtbDisplayStatus
            // 
            this.txtbDisplayStatus.Enabled = false;
            this.txtbDisplayStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbDisplayStatus.Location = new System.Drawing.Point(484, 464);
            this.txtbDisplayStatus.Name = "txtbDisplayStatus";
            this.txtbDisplayStatus.Size = new System.Drawing.Size(165, 22);
            this.txtbDisplayStatus.TabIndex = 65;
            // 
            // lblEstadoRuleta
            // 
            this.lblEstadoRuleta.AutoSize = true;
            this.lblEstadoRuleta.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblEstadoRuleta.Location = new System.Drawing.Point(588, 448);
            this.lblEstadoRuleta.Name = "lblEstadoRuleta";
            this.lblEstadoRuleta.Size = new System.Drawing.Size(43, 13);
            this.lblEstadoRuleta.TabIndex = 64;
            this.lblEstadoRuleta.Text = "M-RPM";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label15.Location = new System.Drawing.Point(260, 468);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(36, 13);
            this.label15.TabIndex = 63;
            this.label15.Text = "Leido:";
            // 
            // lblWinner
            // 
            this.lblWinner.AutoSize = true;
            this.lblWinner.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWinner.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblWinner.Location = new System.Drawing.Point(307, 467);
            this.lblWinner.Name = "lblWinner";
            this.lblWinner.Size = new System.Drawing.Size(18, 16);
            this.lblWinner.TabIndex = 62;
            this.lblWinner.Text = "--";
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
            // txtWinner
            // 
            this.txtWinner.Enabled = false;
            this.txtWinner.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtWinner.Location = new System.Drawing.Point(260, 421);
            this.txtWinner.Name = "txtWinner";
            this.txtWinner.Size = new System.Drawing.Size(117, 31);
            this.txtWinner.TabIndex = 59;
            this.txtWinner.Text = "---";
            this.txtWinner.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblFPS
            // 
            this.lblFPS.AutoSize = true;
            this.lblFPS.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblFPS.Location = new System.Drawing.Point(176, 426);
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
            this.label8.Location = new System.Drawing.Point(203, 428);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(51, 13);
            this.label8.TabIndex = 56;
            this.label8.Text = "Ganador:";
            // 
            // lblBallOn
            // 
            this.lblBallOn.AutoSize = true;
            this.lblBallOn.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblBallOn.Location = new System.Drawing.Point(627, 448);
            this.lblBallOn.Name = "lblBallOn";
            this.lblBallOn.Size = new System.Drawing.Size(22, 13);
            this.lblBallOn.TabIndex = 54;
            this.lblBallOn.Text = "NB";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label11.Location = new System.Drawing.Point(8, 468);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(75, 13);
            this.label11.TabIndex = 55;
            this.label11.Text = "Estado Juego:";
            // 
            // tbVideoStatus
            // 
            this.tbVideoStatus.BackColor = System.Drawing.Color.DarkRed;
            this.tbVideoStatus.Enabled = false;
            this.tbVideoStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbVideoStatus.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.tbVideoStatus.Location = new System.Drawing.Point(114, 423);
            this.tbVideoStatus.Name = "tbVideoStatus";
            this.tbVideoStatus.Size = new System.Drawing.Size(47, 22);
            this.tbVideoStatus.TabIndex = 53;
            this.tbVideoStatus.Text = "OFF";
            this.tbVideoStatus.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label10.Location = new System.Drawing.Point(508, 448);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(77, 13);
            this.label10.TabIndex = 36;
            this.label10.Text = "Estado Ruleta:";
            // 
            // txtbGameStatus
            // 
            this.txtbGameStatus.Enabled = false;
            this.txtbGameStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbGameStatus.Location = new System.Drawing.Point(89, 463);
            this.txtbGameStatus.Name = "txtbGameStatus";
            this.txtbGameStatus.Size = new System.Drawing.Size(165, 22);
            this.txtbGameStatus.TabIndex = 54;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label7.Location = new System.Drawing.Point(505, 424);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(80, 13);
            this.label7.TabIndex = 30;
            this.label7.Text = "Dist. Cero-Bola:";
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
            this.btnStartCamara.Location = new System.Drawing.Point(9, 421);
            this.btnStartCamara.Name = "btnStartCamara";
            this.btnStartCamara.Size = new System.Drawing.Size(99, 27);
            this.btnStartCamara.TabIndex = 11;
            this.btnStartCamara.Text = "Iniciar Captura";
            this.btnStartCamara.UseVisualStyleBackColor = true;
            this.btnStartCamara.Click += new System.EventHandler(this.btnStartCamara_Click);
            // 
            // btnIniciarDemo
            // 
            this.btnIniciarDemo.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnIniciarDemo.Enabled = false;
            this.btnIniciarDemo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnIniciarDemo.ForeColor = System.Drawing.Color.Black;
            this.btnIniciarDemo.Location = new System.Drawing.Point(6, 644);
            this.btnIniciarDemo.Name = "btnIniciarDemo";
            this.btnIniciarDemo.Size = new System.Drawing.Size(64, 23);
            this.btnIniciarDemo.TabIndex = 51;
            this.btnIniciarDemo.Text = "Demo";
            this.btnIniciarDemo.UseVisualStyleBackColor = true;
            this.btnIniciarDemo.Visible = false;
            this.btnIniciarDemo.Click += new System.EventHandler(this.btnIniciarDemo_Click);
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
            this.txtProtocolo.Size = new System.Drawing.Size(78, 66);
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
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.lblAvgY);
            this.groupBox4.Controls.Add(this.lblAvgX);
            this.groupBox4.Controls.Add(this.label18);
            this.groupBox4.Controls.Add(this.label19);
            this.groupBox4.Controls.Add(this.lblAvgAngle);
            this.groupBox4.Controls.Add(this.lblAvgDist);
            this.groupBox4.Controls.Add(this.btnIniciarDemo);
            this.groupBox4.Controls.Add(this.comboBox1);
            this.groupBox4.Controls.Add(this.checkBox36);
            this.groupBox4.Controls.Add(this.checkBox34);
            this.groupBox4.Controls.Add(this.checkBox35);
            this.groupBox4.Controls.Add(this.checkBox32);
            this.groupBox4.Controls.Add(this.checkBox33);
            this.groupBox4.Controls.Add(this.checkBox30);
            this.groupBox4.Controls.Add(this.checkBox31);
            this.groupBox4.Controls.Add(this.checkBox28);
            this.groupBox4.Controls.Add(this.checkBox29);
            this.groupBox4.Controls.Add(this.checkBox26);
            this.groupBox4.Controls.Add(this.checkBox27);
            this.groupBox4.Controls.Add(this.checkBox24);
            this.groupBox4.Controls.Add(this.checkBox25);
            this.groupBox4.Controls.Add(this.checkBox22);
            this.groupBox4.Controls.Add(this.checkBox23);
            this.groupBox4.Controls.Add(this.checkBox20);
            this.groupBox4.Controls.Add(this.checkBox21);
            this.groupBox4.Controls.Add(this.checkBox18);
            this.groupBox4.Controls.Add(this.btnCalibrateNumber);
            this.groupBox4.Controls.Add(this.checkBox19);
            this.groupBox4.Controls.Add(this.checkBox17);
            this.groupBox4.Controls.Add(this.checkBox16);
            this.groupBox4.Controls.Add(this.checkBox15);
            this.groupBox4.Controls.Add(this.checkBox14);
            this.groupBox4.Controls.Add(this.checkBox13);
            this.groupBox4.Controls.Add(this.checkBox12);
            this.groupBox4.Controls.Add(this.checkBox11);
            this.groupBox4.Controls.Add(this.checkBox10);
            this.groupBox4.Controls.Add(this.checkBox9);
            this.groupBox4.Controls.Add(this.checkBox8);
            this.groupBox4.Controls.Add(this.checkBox7);
            this.groupBox4.Controls.Add(this.checkBox6);
            this.groupBox4.Controls.Add(this.checkBox5);
            this.groupBox4.Controls.Add(this.checkBox4);
            this.groupBox4.Controls.Add(this.checkBox3);
            this.groupBox4.Controls.Add(this.checkBox2);
            this.groupBox4.Controls.Add(this.checkBox1);
            this.groupBox4.Controls.Add(this.checkBox0);
            this.groupBox4.Controls.Add(this.lblTestCount);
            this.groupBox4.Controls.Add(this.label13);
            this.groupBox4.Controls.Add(this.label12);
            this.groupBox4.Controls.Add(this.btnSetNumber);
            this.groupBox4.Location = new System.Drawing.Point(676, 75);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(85, 677);
            this.groupBox4.TabIndex = 62;
            this.groupBox4.TabStop = false;
            this.groupBox4.Visible = false;
            // 
            // lblAvgY
            // 
            this.lblAvgY.AutoSize = true;
            this.lblAvgY.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAvgY.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblAvgY.Location = new System.Drawing.Point(26, 120);
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
            this.lblAvgX.Location = new System.Drawing.Point(26, 107);
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
            this.label18.Location = new System.Drawing.Point(7, 120);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(17, 13);
            this.label18.TabIndex = 122;
            this.label18.Text = "Y:";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label19.Location = new System.Drawing.Point(7, 107);
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
            this.lblAvgAngle.Location = new System.Drawing.Point(25, 94);
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
            this.lblAvgDist.Location = new System.Drawing.Point(25, 81);
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
            this.comboBox1.TabIndex = 118;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // checkBox36
            // 
            this.checkBox36.AutoSize = true;
            this.checkBox36.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox36.ForeColor = System.Drawing.Color.Black;
            this.checkBox36.Location = new System.Drawing.Point(6, 596);
            this.checkBox36.Name = "checkBox36";
            this.checkBox36.Size = new System.Drawing.Size(40, 17);
            this.checkBox36.TabIndex = 117;
            this.checkBox36.Text = "26";
            this.checkBox36.UseVisualStyleBackColor = true;
            // 
            // checkBox34
            // 
            this.checkBox34.AutoSize = true;
            this.checkBox34.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox34.ForeColor = System.Drawing.Color.Red;
            this.checkBox34.Location = new System.Drawing.Point(44, 575);
            this.checkBox34.Name = "checkBox34";
            this.checkBox34.Size = new System.Drawing.Size(33, 17);
            this.checkBox34.TabIndex = 116;
            this.checkBox34.Text = "5";
            this.checkBox34.UseVisualStyleBackColor = true;
            // 
            // checkBox35
            // 
            this.checkBox35.AutoSize = true;
            this.checkBox35.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox35.ForeColor = System.Drawing.Color.Black;
            this.checkBox35.Location = new System.Drawing.Point(6, 575);
            this.checkBox35.Name = "checkBox35";
            this.checkBox35.Size = new System.Drawing.Size(40, 17);
            this.checkBox35.TabIndex = 115;
            this.checkBox35.Text = "35";
            this.checkBox35.UseVisualStyleBackColor = true;
            // 
            // checkBox32
            // 
            this.checkBox32.AutoSize = true;
            this.checkBox32.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox32.ForeColor = System.Drawing.Color.Red;
            this.checkBox32.Location = new System.Drawing.Point(43, 553);
            this.checkBox32.Name = "checkBox32";
            this.checkBox32.Size = new System.Drawing.Size(40, 17);
            this.checkBox32.TabIndex = 114;
            this.checkBox32.Text = "12";
            this.checkBox32.UseVisualStyleBackColor = true;
            // 
            // checkBox33
            // 
            this.checkBox33.AutoSize = true;
            this.checkBox33.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox33.ForeColor = System.Drawing.Color.Black;
            this.checkBox33.Location = new System.Drawing.Point(6, 553);
            this.checkBox33.Name = "checkBox33";
            this.checkBox33.Size = new System.Drawing.Size(40, 17);
            this.checkBox33.TabIndex = 113;
            this.checkBox33.Text = "28";
            this.checkBox33.UseVisualStyleBackColor = true;
            // 
            // checkBox30
            // 
            this.checkBox30.AutoSize = true;
            this.checkBox30.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox30.ForeColor = System.Drawing.Color.Red;
            this.checkBox30.Location = new System.Drawing.Point(43, 530);
            this.checkBox30.Name = "checkBox30";
            this.checkBox30.Size = new System.Drawing.Size(33, 17);
            this.checkBox30.TabIndex = 112;
            this.checkBox30.Text = "7";
            this.checkBox30.UseVisualStyleBackColor = true;
            // 
            // checkBox31
            // 
            this.checkBox31.AutoSize = true;
            this.checkBox31.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox31.ForeColor = System.Drawing.Color.Black;
            this.checkBox31.Location = new System.Drawing.Point(6, 530);
            this.checkBox31.Name = "checkBox31";
            this.checkBox31.Size = new System.Drawing.Size(40, 17);
            this.checkBox31.TabIndex = 111;
            this.checkBox31.Text = "29";
            this.checkBox31.UseVisualStyleBackColor = true;
            // 
            // checkBox28
            // 
            this.checkBox28.AutoSize = true;
            this.checkBox28.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox28.ForeColor = System.Drawing.Color.Red;
            this.checkBox28.Location = new System.Drawing.Point(43, 507);
            this.checkBox28.Name = "checkBox28";
            this.checkBox28.Size = new System.Drawing.Size(40, 17);
            this.checkBox28.TabIndex = 110;
            this.checkBox28.Text = "18";
            this.checkBox28.UseVisualStyleBackColor = true;
            // 
            // checkBox29
            // 
            this.checkBox29.AutoSize = true;
            this.checkBox29.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox29.ForeColor = System.Drawing.Color.Black;
            this.checkBox29.Location = new System.Drawing.Point(6, 507);
            this.checkBox29.Name = "checkBox29";
            this.checkBox29.Size = new System.Drawing.Size(40, 17);
            this.checkBox29.TabIndex = 109;
            this.checkBox29.Text = "22";
            this.checkBox29.UseVisualStyleBackColor = true;
            // 
            // checkBox26
            // 
            this.checkBox26.AutoSize = true;
            this.checkBox26.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox26.ForeColor = System.Drawing.Color.Red;
            this.checkBox26.Location = new System.Drawing.Point(43, 484);
            this.checkBox26.Name = "checkBox26";
            this.checkBox26.Size = new System.Drawing.Size(33, 17);
            this.checkBox26.TabIndex = 108;
            this.checkBox26.Text = "9";
            this.checkBox26.UseVisualStyleBackColor = true;
            // 
            // checkBox27
            // 
            this.checkBox27.AutoSize = true;
            this.checkBox27.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox27.ForeColor = System.Drawing.Color.Black;
            this.checkBox27.Location = new System.Drawing.Point(6, 484);
            this.checkBox27.Name = "checkBox27";
            this.checkBox27.Size = new System.Drawing.Size(40, 17);
            this.checkBox27.TabIndex = 107;
            this.checkBox27.Text = "31";
            this.checkBox27.UseVisualStyleBackColor = true;
            // 
            // checkBox24
            // 
            this.checkBox24.AutoSize = true;
            this.checkBox24.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox24.ForeColor = System.Drawing.Color.Red;
            this.checkBox24.Location = new System.Drawing.Point(44, 461);
            this.checkBox24.Name = "checkBox24";
            this.checkBox24.Size = new System.Drawing.Size(40, 17);
            this.checkBox24.TabIndex = 106;
            this.checkBox24.Text = "14";
            this.checkBox24.UseVisualStyleBackColor = true;
            // 
            // checkBox25
            // 
            this.checkBox25.AutoSize = true;
            this.checkBox25.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox25.ForeColor = System.Drawing.Color.Black;
            this.checkBox25.Location = new System.Drawing.Point(7, 461);
            this.checkBox25.Name = "checkBox25";
            this.checkBox25.Size = new System.Drawing.Size(40, 17);
            this.checkBox25.TabIndex = 105;
            this.checkBox25.Text = "20";
            this.checkBox25.UseVisualStyleBackColor = true;
            // 
            // checkBox22
            // 
            this.checkBox22.AutoSize = true;
            this.checkBox22.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox22.ForeColor = System.Drawing.Color.Red;
            this.checkBox22.Location = new System.Drawing.Point(44, 438);
            this.checkBox22.Name = "checkBox22";
            this.checkBox22.Size = new System.Drawing.Size(33, 17);
            this.checkBox22.TabIndex = 104;
            this.checkBox22.Text = "1";
            this.checkBox22.UseVisualStyleBackColor = true;
            // 
            // checkBox23
            // 
            this.checkBox23.AutoSize = true;
            this.checkBox23.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox23.ForeColor = System.Drawing.Color.Black;
            this.checkBox23.Location = new System.Drawing.Point(7, 438);
            this.checkBox23.Name = "checkBox23";
            this.checkBox23.Size = new System.Drawing.Size(40, 17);
            this.checkBox23.TabIndex = 103;
            this.checkBox23.Text = "33";
            this.checkBox23.UseVisualStyleBackColor = true;
            // 
            // checkBox20
            // 
            this.checkBox20.AutoSize = true;
            this.checkBox20.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox20.ForeColor = System.Drawing.Color.Red;
            this.checkBox20.Location = new System.Drawing.Point(44, 415);
            this.checkBox20.Name = "checkBox20";
            this.checkBox20.Size = new System.Drawing.Size(40, 17);
            this.checkBox20.TabIndex = 102;
            this.checkBox20.Text = "16";
            this.checkBox20.UseVisualStyleBackColor = true;
            // 
            // checkBox21
            // 
            this.checkBox21.AutoSize = true;
            this.checkBox21.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox21.ForeColor = System.Drawing.Color.Black;
            this.checkBox21.Location = new System.Drawing.Point(7, 415);
            this.checkBox21.Name = "checkBox21";
            this.checkBox21.Size = new System.Drawing.Size(40, 17);
            this.checkBox21.TabIndex = 101;
            this.checkBox21.Text = "24";
            this.checkBox21.UseVisualStyleBackColor = true;
            // 
            // checkBox18
            // 
            this.checkBox18.AutoSize = true;
            this.checkBox18.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox18.ForeColor = System.Drawing.Color.Red;
            this.checkBox18.Location = new System.Drawing.Point(44, 392);
            this.checkBox18.Name = "checkBox18";
            this.checkBox18.Size = new System.Drawing.Size(33, 17);
            this.checkBox18.TabIndex = 100;
            this.checkBox18.Text = "5";
            this.checkBox18.UseVisualStyleBackColor = true;
            // 
            // btnCalibrateNumber
            // 
            this.btnCalibrateNumber.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCalibrateNumber.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCalibrateNumber.ForeColor = System.Drawing.Color.Black;
            this.btnCalibrateNumber.Location = new System.Drawing.Point(4, 51);
            this.btnCalibrateNumber.Name = "btnCalibrateNumber";
            this.btnCalibrateNumber.Size = new System.Drawing.Size(73, 27);
            this.btnCalibrateNumber.TabIndex = 69;
            this.btnCalibrateNumber.Text = "Calibrar";
            this.btnCalibrateNumber.UseVisualStyleBackColor = true;
            this.btnCalibrateNumber.Click += new System.EventHandler(this.btnCalibrateNumber_Click);
            // 
            // checkBox19
            // 
            this.checkBox19.AutoSize = true;
            this.checkBox19.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox19.ForeColor = System.Drawing.Color.Black;
            this.checkBox19.Location = new System.Drawing.Point(7, 392);
            this.checkBox19.Name = "checkBox19";
            this.checkBox19.Size = new System.Drawing.Size(40, 17);
            this.checkBox19.TabIndex = 99;
            this.checkBox19.Text = "10";
            this.checkBox19.UseVisualStyleBackColor = true;
            // 
            // checkBox17
            // 
            this.checkBox17.AutoSize = true;
            this.checkBox17.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox17.ForeColor = System.Drawing.Color.Red;
            this.checkBox17.Location = new System.Drawing.Point(44, 369);
            this.checkBox17.Name = "checkBox17";
            this.checkBox17.Size = new System.Drawing.Size(40, 17);
            this.checkBox17.TabIndex = 98;
            this.checkBox17.Text = "23";
            this.checkBox17.UseVisualStyleBackColor = true;
            // 
            // checkBox16
            // 
            this.checkBox16.AutoSize = true;
            this.checkBox16.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox16.ForeColor = System.Drawing.Color.Black;
            this.checkBox16.Location = new System.Drawing.Point(6, 369);
            this.checkBox16.Name = "checkBox16";
            this.checkBox16.Size = new System.Drawing.Size(33, 17);
            this.checkBox16.TabIndex = 97;
            this.checkBox16.Text = "8";
            this.checkBox16.UseVisualStyleBackColor = true;
            // 
            // checkBox15
            // 
            this.checkBox15.AutoSize = true;
            this.checkBox15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox15.ForeColor = System.Drawing.Color.Red;
            this.checkBox15.Location = new System.Drawing.Point(44, 346);
            this.checkBox15.Name = "checkBox15";
            this.checkBox15.Size = new System.Drawing.Size(40, 17);
            this.checkBox15.TabIndex = 96;
            this.checkBox15.Text = "30";
            this.checkBox15.UseVisualStyleBackColor = true;
            // 
            // checkBox14
            // 
            this.checkBox14.AutoSize = true;
            this.checkBox14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox14.ForeColor = System.Drawing.Color.Black;
            this.checkBox14.Location = new System.Drawing.Point(7, 346);
            this.checkBox14.Name = "checkBox14";
            this.checkBox14.Size = new System.Drawing.Size(40, 17);
            this.checkBox14.TabIndex = 95;
            this.checkBox14.Text = "11";
            this.checkBox14.UseVisualStyleBackColor = true;
            // 
            // checkBox13
            // 
            this.checkBox13.AutoSize = true;
            this.checkBox13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox13.ForeColor = System.Drawing.Color.Red;
            this.checkBox13.Location = new System.Drawing.Point(44, 320);
            this.checkBox13.Name = "checkBox13";
            this.checkBox13.Size = new System.Drawing.Size(40, 17);
            this.checkBox13.TabIndex = 94;
            this.checkBox13.Text = "36";
            this.checkBox13.UseVisualStyleBackColor = true;
            // 
            // checkBox12
            // 
            this.checkBox12.AutoSize = true;
            this.checkBox12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox12.ForeColor = System.Drawing.Color.Black;
            this.checkBox12.Location = new System.Drawing.Point(7, 323);
            this.checkBox12.Name = "checkBox12";
            this.checkBox12.Size = new System.Drawing.Size(40, 17);
            this.checkBox12.TabIndex = 93;
            this.checkBox12.Text = "13";
            this.checkBox12.UseVisualStyleBackColor = true;
            // 
            // checkBox11
            // 
            this.checkBox11.AutoSize = true;
            this.checkBox11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox11.ForeColor = System.Drawing.Color.Red;
            this.checkBox11.Location = new System.Drawing.Point(43, 300);
            this.checkBox11.Name = "checkBox11";
            this.checkBox11.Size = new System.Drawing.Size(40, 17);
            this.checkBox11.TabIndex = 92;
            this.checkBox11.Text = "27";
            this.checkBox11.UseVisualStyleBackColor = true;
            // 
            // checkBox10
            // 
            this.checkBox10.AutoSize = true;
            this.checkBox10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox10.ForeColor = System.Drawing.Color.Black;
            this.checkBox10.Location = new System.Drawing.Point(6, 302);
            this.checkBox10.Name = "checkBox10";
            this.checkBox10.Size = new System.Drawing.Size(33, 17);
            this.checkBox10.TabIndex = 91;
            this.checkBox10.Text = "6";
            this.checkBox10.UseVisualStyleBackColor = true;
            // 
            // checkBox9
            // 
            this.checkBox9.AutoSize = true;
            this.checkBox9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox9.ForeColor = System.Drawing.Color.Red;
            this.checkBox9.Location = new System.Drawing.Point(44, 277);
            this.checkBox9.Name = "checkBox9";
            this.checkBox9.Size = new System.Drawing.Size(40, 17);
            this.checkBox9.TabIndex = 90;
            this.checkBox9.Text = "34";
            this.checkBox9.UseVisualStyleBackColor = true;
            // 
            // checkBox8
            // 
            this.checkBox8.AutoSize = true;
            this.checkBox8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox8.ForeColor = System.Drawing.Color.Black;
            this.checkBox8.Location = new System.Drawing.Point(6, 279);
            this.checkBox8.Name = "checkBox8";
            this.checkBox8.Size = new System.Drawing.Size(40, 17);
            this.checkBox8.TabIndex = 89;
            this.checkBox8.Text = "17";
            this.checkBox8.UseVisualStyleBackColor = true;
            // 
            // checkBox7
            // 
            this.checkBox7.AutoSize = true;
            this.checkBox7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox7.ForeColor = System.Drawing.Color.Red;
            this.checkBox7.Location = new System.Drawing.Point(44, 254);
            this.checkBox7.Name = "checkBox7";
            this.checkBox7.Size = new System.Drawing.Size(40, 17);
            this.checkBox7.TabIndex = 88;
            this.checkBox7.Text = "25";
            this.checkBox7.UseVisualStyleBackColor = true;
            // 
            // checkBox6
            // 
            this.checkBox6.AutoSize = true;
            this.checkBox6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox6.ForeColor = System.Drawing.Color.Black;
            this.checkBox6.Location = new System.Drawing.Point(6, 256);
            this.checkBox6.Name = "checkBox6";
            this.checkBox6.Size = new System.Drawing.Size(33, 17);
            this.checkBox6.TabIndex = 87;
            this.checkBox6.Text = "2";
            this.checkBox6.UseVisualStyleBackColor = true;
            // 
            // checkBox5
            // 
            this.checkBox5.AutoSize = true;
            this.checkBox5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox5.ForeColor = System.Drawing.Color.Red;
            this.checkBox5.Location = new System.Drawing.Point(44, 231);
            this.checkBox5.Name = "checkBox5";
            this.checkBox5.Size = new System.Drawing.Size(40, 17);
            this.checkBox5.TabIndex = 86;
            this.checkBox5.Text = "21";
            this.checkBox5.UseVisualStyleBackColor = true;
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox4.ForeColor = System.Drawing.Color.Black;
            this.checkBox4.Location = new System.Drawing.Point(6, 231);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(33, 17);
            this.checkBox4.TabIndex = 85;
            this.checkBox4.Text = "4";
            this.checkBox4.UseVisualStyleBackColor = true;
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox3.ForeColor = System.Drawing.Color.Red;
            this.checkBox3.Location = new System.Drawing.Point(44, 208);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(40, 17);
            this.checkBox3.TabIndex = 84;
            this.checkBox3.Text = "19";
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox2.ForeColor = System.Drawing.Color.Black;
            this.checkBox2.Location = new System.Drawing.Point(6, 208);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(40, 17);
            this.checkBox2.TabIndex = 83;
            this.checkBox2.Text = "15";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox1.ForeColor = System.Drawing.Color.Red;
            this.checkBox1.Location = new System.Drawing.Point(44, 185);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(40, 17);
            this.checkBox1.TabIndex = 82;
            this.checkBox1.Text = "32";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // checkBox0
            // 
            this.checkBox0.AutoSize = true;
            this.checkBox0.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox0.ForeColor = System.Drawing.Color.LimeGreen;
            this.checkBox0.Location = new System.Drawing.Point(6, 185);
            this.checkBox0.Name = "checkBox0";
            this.checkBox0.Size = new System.Drawing.Size(33, 17);
            this.checkBox0.TabIndex = 81;
            this.checkBox0.Text = "0";
            this.checkBox0.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.checkBox0.UseVisualStyleBackColor = true;
            // 
            // lblTestCount
            // 
            this.lblTestCount.AutoSize = true;
            this.lblTestCount.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblTestCount.Location = new System.Drawing.Point(60, 35);
            this.lblTestCount.Name = "lblTestCount";
            this.lblTestCount.Size = new System.Drawing.Size(13, 13);
            this.lblTestCount.TabIndex = 70;
            this.lblTestCount.Text = "0";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label13.Location = new System.Drawing.Point(7, 94);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(17, 13);
            this.label13.TabIndex = 68;
            this.label13.Text = "A:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label12.Location = new System.Drawing.Point(6, 81);
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
            this.btnSetNumber.Location = new System.Drawing.Point(6, 141);
            this.btnSetNumber.Name = "btnSetNumber";
            this.btnSetNumber.Size = new System.Drawing.Size(74, 38);
            this.btnSetNumber.TabIndex = 64;
            this.btnSetNumber.Text = "Guardar Calibracion";
            this.btnSetNumber.UseVisualStyleBackColor = true;
            this.btnSetNumber.Click += new System.EventHandler(this.btnSetNumber_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnIniciarDemo;
            this.ClientSize = new System.Drawing.Size(768, 759);
            this.Controls.Add(this.groupBox4);
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
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
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
        private System.Windows.Forms.TextBox tbZeroPosY;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbZeroPosX;
        private System.Windows.Forms.Button btnStartCamara;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.CheckBox cbCalibrateCamera;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Timer tmrDemo;
        private System.Windows.Forms.Button btnIniciarDemo;
        private System.Windows.Forms.TextBox txtProtocolo;
        private System.Windows.Forms.Timer tmrMain;
        private System.Windows.Forms.TextBox tbVideoStatus;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtbGameStatus;
        private System.Windows.Forms.Label lblBallOn;
        private System.Windows.Forms.Label lblFPS;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private Label label14;
        private TextBox tbZeroPosAngle;
        private GroupBox groupBox4;
        private Label lblTestCount;
        private Button btnCalibrateNumber;
        private Label label13;
        private Label label12;
        private Button btnSetNumber;
        private TextBox txtWinner;
        private CheckBox cbCalibrateNumbers;
        private CheckBox checkBox17;
        private CheckBox checkBox16;
        private CheckBox checkBox15;
        private CheckBox checkBox14;
        private CheckBox checkBox13;
        private CheckBox checkBox12;
        private CheckBox checkBox11;
        private CheckBox checkBox10;
        private CheckBox checkBox9;
        private CheckBox checkBox8;
        private CheckBox checkBox7;
        private CheckBox checkBox6;
        private CheckBox checkBox5;
        private CheckBox checkBox4;
        private CheckBox checkBox3;
        private CheckBox checkBox2;
        private CheckBox checkBox1;
        private CheckBox checkBox0;
        private CheckBox checkBox18;
        private CheckBox checkBox19;
        private CheckBox checkBox20;
        private CheckBox checkBox21;
        private CheckBox checkBox22;
        private CheckBox checkBox23;
        private CheckBox checkBox24;
        private CheckBox checkBox25;
        private CheckBox checkBox36;
        private CheckBox checkBox34;
        private CheckBox checkBox35;
        private CheckBox checkBox32;
        private CheckBox checkBox33;
        private CheckBox checkBox30;
        private CheckBox checkBox31;
        private CheckBox checkBox28;
        private CheckBox checkBox29;
        private CheckBox checkBox26;
        private CheckBox checkBox27;
        private ContextMenuStrip mnuSystemTray;
        private ToolStripMenuItem cerrarToolStripMenuItem;
        private Label lblWinner;
        private Label label15;
        //private CheckBox[] chkbNumbers; // Declaring array of Button
        private ComboBox comboBox1;
        private Label lblAvgAngle;
        private Label lblAvgDist;
        private Label lblAvgY;
        private Label lblAvgX;
        private Label label18;
        private Label label19;
        private Label label6;
        private Label label4;
        private TextBox tbBolaPosY;
        private Label label5;
        private TextBox tbBolaPosX;
        private PictureBox pbBall;
        private Label label9;
        private TextBox tbBallAngleToCenter;
        private Label lblEstadoRuleta;
        private Label label16;
        private TextBox txtbDisplayStatus;
    }
}

