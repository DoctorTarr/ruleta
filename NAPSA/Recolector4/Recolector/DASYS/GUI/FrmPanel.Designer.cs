namespace DASYS.GUI
{
    partial class FrmPanel2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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

        private void InitializeComponent()
        {
            this.components = (IContainer)new Container();
            ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FrmPanel));
            this.btnPuertoOnOff = new Button();
            this.pnlPanel = new Panel();
            this.btnNumero = new Button();
            this.btnEstadoX = new Button();
            this.btnError = new Button();
            this.btnEstado5 = new Button();
            this.btnEstado4 = new Button();
            this.pictureBox1 = new PictureBox();
            this.pnlNapsaPie = new Panel();
            this.label3 = new Label();
            this.lblLine = new Label();
            this.btnSimular = new Button();
            this.lvwMensajes = new ListView();
            this.lblEstadoCOM = new Label();
            this.pnlProtocoloTest = new Panel();
            this.txtCadenaOriginal = new TextBox();
            this.btnProbarCadena = new Button();
            this.txtProtocolo = new TextBox();
            this.lblTips = new Label();
            this.tmrSimulacro = new Timer(this.components);
            this.notifyIcon1 = new NotifyIcon(this.components);
            this.mnuSystemTray = new ContextMenuStrip(this.components);
            this.cerrarToolStripMenuItem = new ToolStripMenuItem();
            this.btnIniciarDemo = new Button();
            this.tmrDemo = new Timer(this.components);
            this.visorPopUp1 = new VisorPopUp();
            this.pnlPanel.SuspendLayout();
            ((ISupportInitialize)this.pictureBox1).BeginInit();
            this.pnlProtocoloTest.SuspendLayout();
            this.mnuSystemTray.SuspendLayout();
            this.SuspendLayout();
            this.btnPuertoOnOff.FlatStyle = FlatStyle.Flat;
            this.btnPuertoOnOff.Location = new Point(12, 32);
            this.btnPuertoOnOff.Name = "btnPuertoOnOff";
            this.btnPuertoOnOff.Size = new Size(75, 23);
            this.btnPuertoOnOff.TabIndex = 0;
            this.btnPuertoOnOff.Text = "Abrir Puerto COM";
            this.btnPuertoOnOff.UseVisualStyleBackColor = true;
            this.btnPuertoOnOff.Click += new EventHandler(this.btnPuertoOnOff_Click);
            this.pnlPanel.Controls.Add((Control)this.btnIniciarDemo);
            this.pnlPanel.Controls.Add((Control)this.btnNumero);
            this.pnlPanel.Controls.Add((Control)this.btnEstadoX);
            this.pnlPanel.Controls.Add((Control)this.btnError);
            this.pnlPanel.Controls.Add((Control)this.btnEstado5);
            this.pnlPanel.Controls.Add((Control)this.btnEstado4);
            this.pnlPanel.Controls.Add((Control)this.visorPopUp1);
            this.pnlPanel.Controls.Add((Control)this.pictureBox1);
            this.pnlPanel.Controls.Add((Control)this.pnlNapsaPie);
            this.pnlPanel.Controls.Add((Control)this.label3);
            this.pnlPanel.Controls.Add((Control)this.lblLine);
            this.pnlPanel.Controls.Add((Control)this.btnSimular);
            this.pnlPanel.Controls.Add((Control)this.lvwMensajes);
            this.pnlPanel.Controls.Add((Control)this.lblEstadoCOM);
            this.pnlPanel.Controls.Add((Control)this.pnlProtocoloTest);
            this.pnlPanel.Controls.Add((Control)this.btnPuertoOnOff);
            this.pnlPanel.Controls.Add((Control)this.txtProtocolo);
            this.pnlPanel.Controls.Add((Control)this.lblTips);
            this.pnlPanel.Dock = DockStyle.Fill;
            this.pnlPanel.Location = new Point(0, 0);
            this.pnlPanel.Name = "pnlPanel";
            this.pnlPanel.Size = new Size(453, 584);
            this.pnlPanel.TabIndex = 1;
            this.btnNumero.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            this.btnNumero.BackColor = Color.Black;
            this.btnNumero.FlatStyle = FlatStyle.Flat;
            this.btnNumero.ForeColor = Color.White;
            this.btnNumero.Location = new Point(320, 450);
            this.btnNumero.Name = "btnNumero";
            this.btnNumero.Size = new Size(71, 25);
            this.btnNumero.TabIndex = 49;
            this.btnNumero.Text = "Insert Num";
            this.btnNumero.UseVisualStyleBackColor = false;
            this.btnNumero.Visible = false;
            this.btnNumero.Click += new EventHandler(this.btnNumero_Click);
            this.btnEstadoX.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            this.btnEstadoX.BackColor = Color.Black;
            this.btnEstadoX.FlatStyle = FlatStyle.Flat;
            this.btnEstadoX.ForeColor = Color.White;
            this.btnEstadoX.Location = new Point(244, 450);
            this.btnEstadoX.Name = "btnEstadoX";
            this.btnEstadoX.Size = new Size(70, 25);
            this.btnEstadoX.TabIndex = 48;
            this.btnEstadoX.Text = "Insert ? Est";
            this.btnEstadoX.UseVisualStyleBackColor = false;
            this.btnEstadoX.Visible = false;
            this.btnEstadoX.Click += new EventHandler(this.btnEstadoX_Click);
            this.btnError.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            this.btnError.BackColor = Color.Black;
            this.btnError.FlatStyle = FlatStyle.Flat;
            this.btnError.ForeColor = Color.White;
            this.btnError.Location = new Point(175, 450);
            this.btnError.Name = "btnError";
            this.btnError.Size = new Size(63, 25);
            this.btnError.TabIndex = 47;
            this.btnError.Text = "Insert Err";
            this.btnError.UseVisualStyleBackColor = false;
            this.btnError.Visible = false;
            this.btnError.Click += new EventHandler(this.btnError_Click);
            this.btnEstado5.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            this.btnEstado5.BackColor = Color.Black;
            this.btnEstado5.FlatStyle = FlatStyle.Flat;
            this.btnEstado5.ForeColor = Color.White;
            this.btnEstado5.Location = new Point(13, 450);
            this.btnEstado5.Name = "btnEstado5";
            this.btnEstado5.Size = new Size(75, 25);
            this.btnEstado5.TabIndex = 46;
            this.btnEstado5.Text = "5 Winning Num";
            this.btnEstado5.UseVisualStyleBackColor = false;
            this.btnEstado5.Visible = false;
            this.btnEstado5.Click += new EventHandler(this.btnEstado5_Click);
            this.btnEstado4.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            this.btnEstado4.BackColor = Color.Black;
            this.btnEstado4.FlatStyle = FlatStyle.Flat;
            this.btnEstado4.ForeColor = Color.White;
            this.btnEstado4.Location = new Point(94, 450);
            this.btnEstado4.Name = "btnEstado4";
            this.btnEstado4.Size = new Size(75, 25);
            this.btnEstado4.TabIndex = 45;
            this.btnEstado4.Text = "4 No More Bets";
            this.btnEstado4.UseVisualStyleBackColor = false;
            this.btnEstado4.Visible = false;
            this.btnEstado4.Click += new EventHandler(this.btnEstado4_Click);
            this.pictureBox1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            this.pictureBox1.BackgroundImage = (Image)componentResourceManager.GetObject("pictureBox1.BackgroundImage");
            this.pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
            this.pictureBox1.Location = new Point(348, 530);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new Size(87, 51);
            this.pictureBox1.TabIndex = 15;
            this.pictureBox1.TabStop = false;
            this.pnlNapsaPie.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.pnlNapsaPie.BackgroundImage = (Image)componentResourceManager.GetObject("pnlNapsaPie.BackgroundImage");
            this.pnlNapsaPie.BackgroundImageLayout = ImageLayout.Stretch;
            this.pnlNapsaPie.Location = new Point(12, 530);
            this.pnlNapsaPie.Name = "pnlNapsaPie";
            this.pnlNapsaPie.Size = new Size(423, 51);
            this.pnlNapsaPie.TabIndex = 16;
            this.label3.AutoSize = true;
            this.label3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte)0);
            this.label3.Location = new Point(194, 65);
            this.label3.Name = "label3";
            this.label3.Size = new Size(104, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Parseo Protocolo";
            this.lblLine.AutoSize = true;
            this.lblLine.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte)0);
            this.lblLine.Location = new Point(9, 65);
            this.lblLine.Name = "lblLine";
            this.lblLine.Size = new Size(89, 13);
            this.lblLine.TabIndex = 12;
            this.lblLine.Text = "Línea recibida";
            this.btnSimular.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            this.btnSimular.FlatStyle = FlatStyle.Flat;
            this.btnSimular.Location = new Point(397, 450);
            this.btnSimular.Name = "btnSimular";
            this.btnSimular.Size = new Size(43, 23);
            this.btnSimular.TabIndex = 9;
            this.btnSimular.Text = "Sim";
            this.btnSimular.UseVisualStyleBackColor = true;
            this.btnSimular.Click += new EventHandler(this.btnSimular_Click);
            this.lvwMensajes.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            this.lvwMensajes.BackColor = Color.LightGray;
            this.lvwMensajes.Location = new Point(12, 81);
            this.lvwMensajes.Name = "lvwMensajes";
            this.lvwMensajes.Size = new Size(179, 363);
            this.lvwMensajes.TabIndex = 8;
            this.lvwMensajes.UseCompatibleStateImageBehavior = false;
            this.lvwMensajes.View = View.Details;
            this.lvwMensajes.DoubleClick += new EventHandler(this.lvwMensajes_DoubleClick);
            this.lblEstadoCOM.AutoSize = true;
            this.lblEstadoCOM.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte)0);
            this.lblEstadoCOM.Location = new Point(9, 9);
            this.lblEstadoCOM.Name = "lblEstadoCOM";
            this.lblEstadoCOM.Size = new Size(123, 13);
            this.lblEstadoCOM.TabIndex = 7;
            this.lblEstadoCOM.Text = "Puerto COM Cerrado";
            this.pnlProtocoloTest.Controls.Add((Control)this.txtCadenaOriginal);
            this.pnlProtocoloTest.Controls.Add((Control)this.btnProbarCadena);
            this.pnlProtocoloTest.Location = new Point(222, 3);
            this.pnlProtocoloTest.Name = "pnlProtocoloTest";
            this.pnlProtocoloTest.Size = new Size(217, 57);
            this.pnlProtocoloTest.TabIndex = 4;
            this.txtCadenaOriginal.BorderStyle = BorderStyle.FixedSingle;
            this.txtCadenaOriginal.Location = new Point(3, 3);
            this.txtCadenaOriginal.Name = "txtCadenaOriginal";
            this.txtCadenaOriginal.Size = new Size(208, 20);
            this.txtCadenaOriginal.TabIndex = 4;
            this.txtCadenaOriginal.KeyPress += new KeyPressEventHandler(this.txtCadenaOriginal_KeyPress);
            this.btnProbarCadena.FlatStyle = FlatStyle.Flat;
            this.btnProbarCadena.Location = new Point(55, 29);
            this.btnProbarCadena.Name = "btnProbarCadena";
            this.btnProbarCadena.Size = new Size(156, 23);
            this.btnProbarCadena.TabIndex = 3;
            this.btnProbarCadena.Text = "Intentar Parsear Cadena";
            this.btnProbarCadena.UseVisualStyleBackColor = true;
            this.btnProbarCadena.Click += new EventHandler(this.btnProbarCadena_Click);
            this.txtProtocolo.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            this.txtProtocolo.BorderStyle = BorderStyle.FixedSingle;
            this.txtProtocolo.Location = new Point(197, 81);
            this.txtProtocolo.Multiline = true;
            this.txtProtocolo.Name = "txtProtocolo";
            this.txtProtocolo.Size = new Size(242, 363);
            this.txtProtocolo.TabIndex = 6;
            this.lblTips.AutoSize = true;
            this.lblTips.Location = new Point(10, 488);
            this.lblTips.Name = "lblTips";
            this.lblTips.Size = new Size(365, 26);
            this.lblTips.TabIndex = 14;
            this.lblTips.Text = "N | S | Nro | Nro | Estado | Velocidad | Velocidad | SentidoGiro | Error | CR(13)\r\nN | N | Nro | Nro | | | | | CheckSum | CR(13)";
            this.lblTips.Visible = false;
            this.tmrSimulacro.Interval = 500;
            this.tmrSimulacro.Tick += new EventHandler(this.tmrSimulacro_Tick);
            this.notifyIcon1.BalloonTipText = "Recolector de datos.";
            this.notifyIcon1.BalloonTipTitle = "NAPSA";
            this.notifyIcon1.ContextMenuStrip = this.mnuSystemTray;
            this.notifyIcon1.Icon = (Icon)componentResourceManager.GetObject("notifyIcon1.Icon");
            this.notifyIcon1.Text = "Recolector de datos NAPSA";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            this.mnuSystemTray.BackColor = Color.FromArgb(25, 23, 26);
            this.mnuSystemTray.Items.AddRange(new ToolStripItem[1]
            {
        (ToolStripItem) this.cerrarToolStripMenuItem
            });
            this.mnuSystemTray.Name = "mnuSystemTray";
            this.mnuSystemTray.Size = new Size(117, 26);
            this.cerrarToolStripMenuItem.ForeColor = Color.White;
            this.cerrarToolStripMenuItem.Name = "cerrarToolStripMenuItem";
            this.cerrarToolStripMenuItem.Size = new Size(116, 22);
            this.cerrarToolStripMenuItem.Text = "Cerrar";
            this.cerrarToolStripMenuItem.Click += new EventHandler(this.cerrarToolStripMenuItem_Click);
            this.btnIniciarDemo.FlatStyle = FlatStyle.Flat;
            this.btnIniciarDemo.Location = new Point(94, 32);
            this.btnIniciarDemo.Name = "btnIniciarDemo";
            this.btnIniciarDemo.Size = new Size(97, 23);
            this.btnIniciarDemo.TabIndex = 50;
            this.btnIniciarDemo.Text = "Iniciar Demo";
            this.btnIniciarDemo.UseVisualStyleBackColor = true;
            this.btnIniciarDemo.Click += new EventHandler(this.btnIniciarDemo_Click);
            this.tmrDemo.Interval = 500;
            this.tmrDemo.Tick += new EventHandler(this.tmrDemo_Tick);
            this.visorPopUp1.BackColor = SystemColors.Control;
            this.visorPopUp1.BorderStyle = BorderStyle.FixedSingle;
            this.visorPopUp1.Dock = DockStyle.Bottom;
            this.visorPopUp1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte)0);
            this.visorPopUp1.Location = new Point(0, 584);
            this.visorPopUp1.Name = "visorPopUp1";
            this.visorPopUp1.Size = new Size(453, 0);
            this.visorPopUp1.TabIndex = 1;
            this.AutoScaleDimensions = new SizeF(6f, 13f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(25, 23, 26);
            this.ClientSize = new Size(453, 584);
            this.Controls.Add((Control)this.pnlPanel);
            this.ForeColor = Color.White;
            this.Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
            this.Name = "FrmPanel";
            this.Text = "Panel del Recolector II";
            this.WindowState = FormWindowState.Minimized;
            this.Load += new EventHandler(this.FrmPanel_Load);
            this.FormClosing += new FormClosingEventHandler(this.FrmPanel_FormClosing);
            this.Resize += new EventHandler(this.FrmPanel_Resize);
            this.pnlPanel.ResumeLayout(false);
            this.pnlPanel.PerformLayout();
            ((ISupportInitialize)this.pictureBox1).EndInit();
            this.pnlProtocoloTest.ResumeLayout(false);
            this.pnlProtocoloTest.PerformLayout();
            this.mnuSystemTray.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Text = "FrmPanel2";
        }

        #endregion
    }
}