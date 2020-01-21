using Accord.Video;
using DASYS.Recolector.BLL;
using System;
using System.Diagnostics;

namespace Recolector4
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        // Roulette Status variables
        //private int lastBallX = 0;
        //private int sentidoGiro = 0; // 0=horario, 1=antihorario
        private ProtocoloNAPSA.EstadoJuego estadoMesa;
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

        #region Form_Load, Closing
        private void MainForm_Load(object sender, EventArgs e)
        {
            this.azarNumero = new Random((int)DateTime.Now.Ticks);
            this.LeerUltimoNumero();
            if (Pase.UltimoPase == null)
                Pase.UltimoPase = new Pase();
            this.estadoMesa = ProtocoloNAPSA.EstadoJuego.CameraOff;
            this.IsCameraOn = false;

        }

        #endregion

        #region Main Timer
        private void tmrMain_Tick(object sender, EventArgs e)
        {
            textBox5.Text = estadoMesa.ToString();

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

                    float fps = 1000.0f * framesReceived / stopWatch.ElapsedMilliseconds;
                    lblFPS.Text = fps.ToString("F2") + " fps";

                    stopWatch.Reset();
                    stopWatch.Reset();
                    stopWatch.Start();
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
                        GuardarEstado(this.estadoDemo, this.numeroDemo, this.azarNumero.Next(0, 2));
                        this.tmrDemo.Interval = 100;
                        break;
                    case 2:
                        //cadena = "NS" + this.numeroDemo.ToString("00") + "2" + this.azarNumero.Next(0, 100).ToString("00") + this.azarNumero.Next(0, 2).ToString() + "0";
                        GuardarEstado(this.estadoDemo, this.numeroDemo, this.azarNumero.Next(0, 2));
                        this.tmrDemo.Interval = 6000;
                        break;
                    case 3:
                        //cadena = "NS" + this.numeroDemo.ToString("00") + "3" + this.azarNumero.Next(0, 100).ToString("00") + this.azarNumero.Next(0, 2).ToString() + "0";
                        GuardarEstado(this.estadoDemo, this.numeroDemo, this.azarNumero.Next(0, 2));
                        this.tmrDemo.Interval = 6000;
                        break;
                    case 4:
                        //cadena = "NS" + this.numeroDemo.ToString("00") + "4" + this.azarNumero.Next(0, 100).ToString("00") + this.azarNumero.Next(0, 2).ToString() + "0";
                        GuardarEstado(this.estadoDemo, this.numeroDemo, this.azarNumero.Next(0, 2));
                        this.tmrDemo.Interval = 10000;
                        break;
                    case 5:
                        //Persistencia.Guardar("NS" + this.numeroDemo.ToString("00") + "5" + this.azarNumero.Next(0, 100).ToString("00") + this.azarNumero.Next(0, 2).ToString() + "0");
                        GuardarEstado(this.estadoDemo, this.numeroDemo, this.azarNumero.Next(0, 2));
                        this.numeroDemo = (byte)this.azarNumero.Next(0, 37);
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
            this.label2 = new System.Windows.Forms.Label();
            this.tbZeroPosY = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbZeroPosX = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.pbZero = new System.Windows.Forms.PictureBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.videoSourcePlayer1 = new Accord.Controls.VideoSourcePlayer();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lblBallOn = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.tbVideoStatus = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cbCalibrate = new System.Windows.Forms.CheckBox();
            this.btnStartCamara = new System.Windows.Forms.Button();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbBolaPosY = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbBolaPosX = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.pbBall = new System.Windows.Forms.PictureBox();
            this.tmrDemo = new System.Windows.Forms.Timer(this.components);
            this.btnIniciarDemo = new System.Windows.Forms.Button();
            this.txtProtocolo = new System.Windows.Forms.TextBox();
            this.tmrMain = new System.Windows.Forms.Timer(this.components);
            this.lblFPS = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbZero)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbBall)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.tbZeroPosY);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.tbZeroPosX);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.pbZero);
            this.groupBox1.Location = new System.Drawing.Point(706, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(337, 259);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label2.Location = new System.Drawing.Point(213, 232);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 13);
            this.label2.TabIndex = 28;
            this.label2.Text = "Cero Y:";
            // 
            // tbZeroPosY
            // 
            this.tbZeroPosY.Enabled = false;
            this.tbZeroPosY.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbZeroPosY.Location = new System.Drawing.Point(261, 227);
            this.tbZeroPosY.Name = "tbZeroPosY";
            this.tbZeroPosY.Size = new System.Drawing.Size(65, 22);
            this.tbZeroPosY.TabIndex = 27;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label1.Location = new System.Drawing.Point(7, 232);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 26;
            this.label1.Text = "Cero X:";
            // 
            // tbZeroPosX
            // 
            this.tbZeroPosX.Enabled = false;
            this.tbZeroPosX.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbZeroPosX.Location = new System.Drawing.Point(55, 227);
            this.tbZeroPosX.Name = "tbZeroPosX";
            this.tbZeroPosX.Size = new System.Drawing.Size(65, 22);
            this.tbZeroPosX.TabIndex = 25;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label3.Location = new System.Drawing.Point(6, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 13);
            this.label3.TabIndex = 24;
            this.label3.Text = "Detección del Cero";
            // 
            // pbZero
            // 
            this.pbZero.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbZero.Location = new System.Drawing.Point(6, 21);
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
            this.textBox1.Location = new System.Drawing.Point(472, 428);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(177, 22);
            this.textBox1.TabIndex = 29;
            // 
            // videoSourcePlayer1
            // 
            this.videoSourcePlayer1.Location = new System.Drawing.Point(19, 19);
            this.videoSourcePlayer1.Name = "videoSourcePlayer1";
            this.videoSourcePlayer1.Size = new System.Drawing.Size(640, 400);
            this.videoSourcePlayer1.TabIndex = 4;
            this.videoSourcePlayer1.Text = "l";
            this.videoSourcePlayer1.VideoSource = null;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lblBallOn);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.tbVideoStatus);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.textBox5);
            this.groupBox3.Controls.Add(this.textBox4);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.textBox2);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.cbCalibrate);
            this.groupBox3.Controls.Add(this.btnStartCamara);
            this.groupBox3.Controls.Add(this.textBox1);
            this.groupBox3.Controls.Add(this.videoSourcePlayer1);
            this.groupBox3.ForeColor = System.Drawing.SystemColors.WindowText;
            this.groupBox3.Location = new System.Drawing.Point(12, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(670, 534);
            this.groupBox3.TabIndex = 13;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Cámara en Vivo";
            // 
            // lblBallOn
            // 
            this.lblBallOn.AutoSize = true;
            this.lblBallOn.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblBallOn.Location = new System.Drawing.Point(282, 477);
            this.lblBallOn.Name = "lblBallOn";
            this.lblBallOn.Size = new System.Drawing.Size(22, 13);
            this.lblBallOn.TabIndex = 54;
            this.lblBallOn.Text = "NB";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label11.Location = new System.Drawing.Point(16, 505);
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
            this.tbVideoStatus.Location = new System.Drawing.Point(125, 431);
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
            this.label10.Location = new System.Drawing.Point(16, 477);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(77, 13);
            this.label10.TabIndex = 36;
            this.label10.Text = "Estado Ruleta:";
            // 
            // textBox5
            // 
            this.textBox5.Enabled = false;
            this.textBox5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox5.Location = new System.Drawing.Point(99, 500);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(177, 22);
            this.textBox5.TabIndex = 54;
            // 
            // textBox4
            // 
            this.textBox4.Enabled = false;
            this.textBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox4.Location = new System.Drawing.Point(99, 472);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(177, 22);
            this.textBox4.TabIndex = 35;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label9.Location = new System.Drawing.Point(363, 461);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(92, 13);
            this.label9.TabIndex = 34;
            this.label9.Text = "Angulo Cero-Bola:";
            // 
            // textBox2
            // 
            this.textBox2.Enabled = false;
            this.textBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox2.Location = new System.Drawing.Point(472, 461);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(177, 22);
            this.textBox2.TabIndex = 31;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label7.Location = new System.Drawing.Point(363, 433);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(103, 13);
            this.label7.TabIndex = 30;
            this.label7.Text = "Distancia Cero-Bola:";
            // 
            // cbCalibrate
            // 
            this.cbCalibrate.AutoSize = true;
            this.cbCalibrate.ForeColor = System.Drawing.SystemColors.WindowText;
            this.cbCalibrate.Location = new System.Drawing.Point(260, 433);
            this.cbCalibrate.Name = "cbCalibrate";
            this.cbCalibrate.Size = new System.Drawing.Size(67, 17);
            this.cbCalibrate.TabIndex = 13;
            this.cbCalibrate.Text = "Calibrate";
            this.cbCalibrate.UseVisualStyleBackColor = true;
            this.cbCalibrate.CheckedChanged += new System.EventHandler(this.cbCalibrate_CheckedChanged);
            // 
            // btnStartCamara
            // 
            this.btnStartCamara.ForeColor = System.Drawing.SystemColors.WindowText;
            this.btnStartCamara.Location = new System.Drawing.Point(20, 429);
            this.btnStartCamara.Name = "btnStartCamara";
            this.btnStartCamara.Size = new System.Drawing.Size(99, 27);
            this.btnStartCamara.TabIndex = 11;
            this.btnStartCamara.Text = "Iniciar Captura";
            this.btnStartCamara.UseVisualStyleBackColor = true;
            this.btnStartCamara.Click += new System.EventHandler(this.btnStartCamara_Click);
            // 
            // textBox3
            // 
            this.textBox3.Enabled = false;
            this.textBox3.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox3.Location = new System.Drawing.Point(126, 578);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(177, 29);
            this.textBox3.TabIndex = 33;
            this.textBox3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label8.Location = new System.Drawing.Point(29, 583);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(91, 13);
            this.label8.TabIndex = 32;
            this.label8.Text = "Número Ganador:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.tbBolaPosY);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.tbBolaPosX);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.pbBall);
            this.groupBox2.Location = new System.Drawing.Point(706, 269);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(337, 268);
            this.groupBox2.TabIndex = 30;
            this.groupBox2.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label4.Location = new System.Drawing.Point(214, 240);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 13);
            this.label4.TabIndex = 28;
            this.label4.Text = "Bola Y:";
            // 
            // tbBolaPosY
            // 
            this.tbBolaPosY.Enabled = false;
            this.tbBolaPosY.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbBolaPosY.Location = new System.Drawing.Point(261, 235);
            this.tbBolaPosY.Name = "tbBolaPosY";
            this.tbBolaPosY.Size = new System.Drawing.Size(65, 22);
            this.tbBolaPosY.TabIndex = 27;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label5.Location = new System.Drawing.Point(7, 240);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 13);
            this.label5.TabIndex = 26;
            this.label5.Text = "Bola X:";
            // 
            // tbBolaPosX
            // 
            this.tbBolaPosX.Enabled = false;
            this.tbBolaPosX.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbBolaPosX.Location = new System.Drawing.Point(55, 235);
            this.tbBolaPosX.Name = "tbBolaPosX";
            this.tbBolaPosX.Size = new System.Drawing.Size(65, 22);
            this.tbBolaPosX.TabIndex = 25;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label6.Location = new System.Drawing.Point(6, 6);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(95, 13);
            this.label6.TabIndex = 24;
            this.label6.Text = "Detección de Bola";
            // 
            // pbBall
            // 
            this.pbBall.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbBall.Location = new System.Drawing.Point(6, 21);
            this.pbBall.Name = "pbBall";
            this.pbBall.Size = new System.Drawing.Size(320, 200);
            this.pbBall.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbBall.TabIndex = 12;
            this.pbBall.TabStop = false;
            // 
            // tmrDemo
            // 
            this.tmrDemo.Interval = 500;
            this.tmrDemo.Tick += new System.EventHandler(this.tmrDemo_Tick);
            // 
            // btnIniciarDemo
            // 
            this.btnIniciarDemo.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnIniciarDemo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnIniciarDemo.ForeColor = System.Drawing.Color.Black;
            this.btnIniciarDemo.Location = new System.Drawing.Point(967, 564);
            this.btnIniciarDemo.Name = "btnIniciarDemo";
            this.btnIniciarDemo.Size = new System.Drawing.Size(76, 23);
            this.btnIniciarDemo.TabIndex = 51;
            this.btnIniciarDemo.Text = "Iniciar Demo";
            this.btnIniciarDemo.UseVisualStyleBackColor = true;
            this.btnIniciarDemo.Click += new System.EventHandler(this.btnIniciarDemo_Click);
            // 
            // txtProtocolo
            // 
            this.txtProtocolo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txtProtocolo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtProtocolo.Location = new System.Drawing.Point(706, 543);
            this.txtProtocolo.Multiline = true;
            this.txtProtocolo.Name = "txtProtocolo";
            this.txtProtocolo.Size = new System.Drawing.Size(255, 69);
            this.txtProtocolo.TabIndex = 52;
            // 
            // tmrMain
            // 
            this.tmrMain.Interval = 500;
            this.tmrMain.Tick += new System.EventHandler(this.tmrMain_Tick);
            // 
            // lblFPS
            // 
            this.lblFPS.AutoSize = true;
            this.lblFPS.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblFPS.Location = new System.Drawing.Point(361, 583);
            this.lblFPS.Name = "lblFPS";
            this.lblFPS.Size = new System.Drawing.Size(21, 13);
            this.lblFPS.TabIndex = 53;
            this.lblFPS.Text = "fps";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnIniciarDemo;
            this.ClientSize = new System.Drawing.Size(1055, 624);
            this.Controls.Add(this.lblFPS);
            this.Controls.Add(this.txtProtocolo);
            this.Controls.Add(this.btnIniciarDemo);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.groupBox1);
            this.ForeColor = System.Drawing.Color.DarkRed;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Panel del Video Recolector IV";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbZero)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbBall)).EndInit();
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
        private System.Windows.Forms.CheckBox cbCalibrate;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbBolaPosY;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbBolaPosX;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.PictureBox pbBall;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Timer tmrDemo;
        private System.Windows.Forms.Button btnIniciarDemo;
        private System.Windows.Forms.TextBox txtProtocolo;
        private System.Windows.Forms.Timer tmrMain;
        private System.Windows.Forms.TextBox tbVideoStatus;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.Label lblFPS;
        private System.Windows.Forms.Label lblBallOn;
    }
}

