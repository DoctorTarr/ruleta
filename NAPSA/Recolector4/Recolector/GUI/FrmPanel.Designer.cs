using DASYS.GUI;
using DASYS.Recolector.BLL;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DASYS.GUI
{
    partial class FrmPanel
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

        private void FrmPanel_Load(object sender, EventArgs e)
        {
            try
            {
                this.pnlProtocoloTest.Visible = Common.EsTest;
                this.lblTips.Visible = Common.EsTest;
                this.btnSimular.Visible = Common.EsTest;
                this.btnError.Visible = Common.EsTest;
                this.btnEstado4.Visible = Common.EsTest;
                this.btnEstado5.Visible = Common.EsTest;
                this.btnNumero.Visible = Common.EsTest;
                this.tmrSimulacro.Enabled = false;
                this.tmrSimulacro.Interval = 500;
                this.azarNumero = new Random((int)DateTime.Now.Ticks);
                this.azarComando = new Random((int)DateTime.Now.Ticks);
                this.lvwMensajes.Columns.Add("mensaje", "Mensaje", 200);
                this.leerUltimoNumero();
                if (Pase.UltimoPase == null)
                    Pase.UltimoPase = new Pase();
                this.btnPuertoOnOff.PerformClick();
                this.Hide();
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show(ex.Message);
            }
        }

        private void btnPuertoOnOff_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.estadoPuerto)
                {
                    if (this.cerrarPuerto())
                    {
                        this.estadoPuerto = false;
                        this.btnPuertoOnOff.Text = "Abrir Puerto";
                        this.lblEstadoCOM.Text = "Puerto COM Cerrado";
                        this.lblEstadoCOM.ForeColor = Color.DarkRed;
 //                       this.visorPopUp1.MostrarMensaje("Puerto COM1 cerrado.");
                        Common.Logger.Escribir("Puerto COM1 cerrado correctamente", true);
                    }
                    else
                        this.lblEstadoCOM.Text = "Error";
                }
                else if (this.abrirPuerto())
                {
                    this.estadoPuerto = true;
                    this.btnPuertoOnOff.Text = "Cerrar Puerto";
                    this.lblEstadoCOM.Text = "Puerto COM Abierto";
                    this.lblEstadoCOM.ForeColor = Color.LightGreen;
//                    this.visorPopUp1.MostrarMensaje("Puerto COM1 abierto.");
                    Common.Logger.Escribir("Puerto COM1 abierto correctamente", true);
                }
                else
                    this.lblEstadoCOM.Text = "Error";
            }
            catch (Exception ex)
            {
 //               this.visorPopUp1.MostrarMensaje(ex.Message);
                Common.Logger.Escribir("Error puerto COM: " + ex.Message, true);
            }
        }

        private void btnNumero_Click(object sender, EventArgs e)
        {
            try
            {
                this.cadena = "NN" + ((byte)this.azarNumero.Next(0, 38)).ToString("00") + "    N";
                Persistencia.Guardar(this.cadena);
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show(ex.Message);
            }
        }

        private void btnIniciarDemo_Click(object sender, EventArgs e)
        {
            if (this.tmrDemo.Enabled)
            {
                this.tmrDemo.Stop();
                this.btnIniciarDemo.Text = "Iniciar Demo";
            }
            else
            {
                this.btnIniciarDemo.Text = "Detener Demo";
                this.tmrDemo.Interval = 100;
                this.tmrDemo.Start();
            }
        }

        private void FrmPanel_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
                this.Hide();
            else
                this.Show();
        }

        private void btnEstadoX_Click(object sender, EventArgs e)
        {
            try
            {
                this.cadena = "NS" + Pase.UltimoPase.Numero.ToString("00");
                this.cadena += this.azarNumero.Next(0, 4).ToString();
                this.cadena += this.azarNumero.Next(0, 100).ToString("00");
                this.cadena += this.azarNumero.Next(0, 2).ToString();
                this.cadena += "0";
                Persistencia.Guardar(this.cadena);
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show(ex.Message);
            }
        }

        private void btnError_Click(object sender, EventArgs e)
        {
            try
            {
                this.cadena = "NS" + Pase.UltimoPase.Numero.ToString("00");
                this.cadena += ((byte)this.azarNumero.Next(0, 7)).ToString();
                this.cadena += ((byte)this.azarNumero.Next(0, 100)).ToString("00");
                this.cadena += ((byte)this.azarNumero.Next(0, 2)).ToString();
                this.cadena += ((byte)this.azarNumero.Next(0, 10)).ToString();
                Persistencia.Guardar(this.cadena);
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show(ex.Message);
            }
        }

        private void btnEstado5_Click(object sender, EventArgs e)
        {
            try
            {
                this.cadena = "NS" + Pase.UltimoPase.Numero.ToString("00");
                this.cadena += "5";
                this.cadena += this.azarNumero.Next(0, 100).ToString("00");
                this.cadena += this.azarNumero.Next(0, 2).ToString();
                this.cadena += "0";
                Persistencia.Guardar(this.cadena);
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show(ex.Message);
            }
        }

        private void btnEstado4_Click(object sender, EventArgs e)
        {
            try
            {
                this.cadena = "NS" + Pase.UltimoPase.Numero.ToString("00");
                this.cadena += "4";
                this.cadena += this.azarNumero.Next(0, 100).ToString("00");
                this.cadena += this.azarNumero.Next(0, 2).ToString();
                this.cadena += "0";
                Persistencia.Guardar(this.cadena);
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show(ex.Message);
            }
        }

        private void btnSimular_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.tmrSimulacro.Enabled)
                {
                    this.btnSimular.Text = "Activar Simulacro Plato...";
                    this.tmrSimulacro.Stop();
                }
                else
                {
                    if (MessageBox.Show("El modo simulacro, creará al azar Números y Status y los escribirá en la base.\n\r¿Activar modo simulacro?", "Modo Simulacro", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                        return;
                    this.btnSimular.Text = "Detener Simulacro Plato";
                    this.tmrSimulacro.Start();
                }
            }
            catch (Exception ex)
            {
                //this.visorPopUp1.MostrarMensaje(ex.Message);
            }
        }

        private void lvwMensajes_DoubleClick(object sender, EventArgs e)
        {
            if (this.lvwMensajes.SelectedItems.Count <= 0)
                return;
            this.txtProtocolo.Text = this.parsearCadena(this.lvwMensajes.SelectedItems[0].Text);
        }

        private void txtCadenaOriginal_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\r')
                return;
            e.Handled = true;
            this.btnProbarCadena.PerformClick();
        }

        private void btnProbarCadena_Click(object sender, EventArgs e)
        {
            try
            {
                string empty = string.Empty;
                string mensaje = this.parsearCadena(this.txtCadenaOriginal.Text);
                Persistencia.Guardar(this.txtCadenaOriginal.Text);
  //              this.visorPopUp1.MostrarMensaje(mensaje);
            }
            catch (Exception ex)
            {
//                this.visorPopUp1.MostrarMensaje(ex.Message);
            }
        }

        private void tmrSimulacro_Tick(object sender, EventArgs e)
        {
            try
            {
                if (this.azarComando.Next(0, 100) > 95)
                {
                    this.cadena = "NN" + ((byte)this.azarNumero.Next(0, 38)).ToString("00") + "    N";
                }
                else
                {
                    this.cadena = "NS" + Pase.UltimoPase.Numero.ToString("00");
                    this.cadena += ((byte)this.azarNumero.Next(1, 7)).ToString();
                    this.cadena += ((byte)this.azarNumero.Next(0, 100)).ToString("00");
                    this.cadena += ((byte)this.azarNumero.Next(0, 2)).ToString();
                    byte num = 0;
                    if (this.azarNumero.Next(0, 100) > 95)
                        num = (byte)this.azarNumero.Next(0, 10);
                    this.cadena += num.ToString();
                }
                this.lvwMensajes.Items.Add(this.cadena);
                ++this.refreshCounter;
                if (this.refreshCounter > 30)
                {
                    this.lvwMensajes.Items.Clear();
                    this.txtProtocolo.Clear();
                    this.refreshCounter = 0;
                }
                Persistencia.Guardar(this.cadena);
            }
            catch (Exception ex)
            {
//                this.visorPopUp1.MostrarMensaje(ex.Message);
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void cerrarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.cerrar();
        }

        private void tmrDemo_Tick(object sender, EventArgs e)
        {
            try
            {
                ++this.estadoDemo;
                if (this.estadoDemo > 5)
                    this.estadoDemo = 1;
                string cadena = string.Empty;
                switch (this.estadoDemo)
                {
                    case 1:
                        cadena = "NS" + this.numeroDemo.ToString("00") + "1" + this.azarNumero.Next(0, 100).ToString("00") + this.azarNumero.Next(0, 2).ToString() + "0";
                        this.tmrDemo.Interval = 100;
                        break;
                    case 2:
                        cadena = "NS" + this.numeroDemo.ToString("00") + "2" + this.azarNumero.Next(0, 100).ToString("00") + this.azarNumero.Next(0, 2).ToString() + "0";
                        this.tmrDemo.Interval = 6000;
                        break;
                    case 3:
                        cadena = "NS" + this.numeroDemo.ToString("00") + "3" + this.azarNumero.Next(0, 100).ToString("00") + this.azarNumero.Next(0, 2).ToString() + "0";
                        this.tmrDemo.Interval = 6000;
                        break;
                    case 4:
                        cadena = "NS" + this.numeroDemo.ToString("00") + "4" + this.azarNumero.Next(0, 100).ToString("00") + this.azarNumero.Next(0, 2).ToString() + "0";
                        this.tmrDemo.Interval = 10000;
                        break;
                    case 5:
                        Persistencia.Guardar("NS" + this.numeroDemo.ToString("00") + "5" + this.azarNumero.Next(0, 100).ToString("00") + this.azarNumero.Next(0, 2).ToString() + "0");
                        this.numeroDemo = (byte)this.azarNumero.Next(0, 37);
                        cadena = "NN" + this.numeroDemo.ToString("00") + "    N";
                        this.tmrDemo.Interval = 1000;
                        break;
                }
                Persistencia.Guardar(cadena);
                txtProtocolo.AppendText(cadena);
                txtProtocolo.AppendText(Environment.NewLine);
            }
            catch (Exception ex)
            {
                int num = 0 + 1;
            }
        }

        private void FrmPanel_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this.cerrarPuerto();
            }
            catch
            {
            }
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPanel));
            this.btnPuertoOnOff = new System.Windows.Forms.Button();
            this.pnlPanel = new System.Windows.Forms.Panel();
            this.btnIniciarDemo = new System.Windows.Forms.Button();
            this.btnNumero = new System.Windows.Forms.Button();
            this.btnEstadoX = new System.Windows.Forms.Button();
            this.btnError = new System.Windows.Forms.Button();
            this.btnEstado5 = new System.Windows.Forms.Button();
            this.btnEstado4 = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pnlNapsaPie = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.lblLine = new System.Windows.Forms.Label();
            this.btnSimular = new System.Windows.Forms.Button();
            this.lvwMensajes = new System.Windows.Forms.ListView();
            this.lblEstadoCOM = new System.Windows.Forms.Label();
            this.pnlProtocoloTest = new System.Windows.Forms.Panel();
            this.txtCadenaOriginal = new System.Windows.Forms.TextBox();
            this.btnProbarCadena = new System.Windows.Forms.Button();
            this.txtProtocolo = new System.Windows.Forms.TextBox();
            this.lblTips = new System.Windows.Forms.Label();
            this.tmrSimulacro = new System.Windows.Forms.Timer(this.components);
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.mnuSystemTray = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cerrarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tmrDemo = new System.Windows.Forms.Timer(this.components);
            this.pnlPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.pnlProtocoloTest.SuspendLayout();
            this.mnuSystemTray.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnPuertoOnOff
            // 
            this.btnPuertoOnOff.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPuertoOnOff.ForeColor = System.Drawing.Color.White;
            this.btnPuertoOnOff.Location = new System.Drawing.Point(12, 32);
            this.btnPuertoOnOff.Name = "btnPuertoOnOff";
            this.btnPuertoOnOff.Size = new System.Drawing.Size(75, 23);
            this.btnPuertoOnOff.TabIndex = 0;
            this.btnPuertoOnOff.Text = "Abrir Puerto COM";
            this.btnPuertoOnOff.UseVisualStyleBackColor = true;
            this.btnPuertoOnOff.Click += new System.EventHandler(this.btnPuertoOnOff_Click);
            // 
            // pnlPanel
            // 
            this.pnlPanel.Controls.Add(this.btnPuertoOnOff);
            this.pnlPanel.Controls.Add(this.btnIniciarDemo);
            this.pnlPanel.Controls.Add(this.btnNumero);
            this.pnlPanel.Controls.Add(this.btnEstadoX);
            this.pnlPanel.Controls.Add(this.btnError);
            this.pnlPanel.Controls.Add(this.btnEstado5);
            this.pnlPanel.Controls.Add(this.btnEstado4);
            this.pnlPanel.Controls.Add(this.pictureBox1);
            this.pnlPanel.Controls.Add(this.pnlNapsaPie);
            this.pnlPanel.Controls.Add(this.label3);
            this.pnlPanel.Controls.Add(this.lblLine);
            this.pnlPanel.Controls.Add(this.btnSimular);
            this.pnlPanel.Controls.Add(this.lvwMensajes);
            this.pnlPanel.Controls.Add(this.lblEstadoCOM);
            this.pnlPanel.Controls.Add(this.pnlProtocoloTest);
            this.pnlPanel.Controls.Add(this.txtProtocolo);
            this.pnlPanel.Controls.Add(this.lblTips);
            this.pnlPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPanel.Location = new System.Drawing.Point(0, 0);
            this.pnlPanel.Name = "pnlPanel";
            this.pnlPanel.Size = new System.Drawing.Size(453, 584);
            this.pnlPanel.TabIndex = 1;
            // 
            // btnIniciarDemo
            // 
            this.btnIniciarDemo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnIniciarDemo.Location = new System.Drawing.Point(94, 32);
            this.btnIniciarDemo.Name = "btnIniciarDemo";
            this.btnIniciarDemo.Size = new System.Drawing.Size(97, 23);
            this.btnIniciarDemo.TabIndex = 50;
            this.btnIniciarDemo.Text = "Iniciar Demo";
            this.btnIniciarDemo.UseVisualStyleBackColor = true;
            this.btnIniciarDemo.Click += new System.EventHandler(this.btnIniciarDemo_Click);
            // 
            // btnNumero
            // 
            this.btnNumero.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnNumero.BackColor = System.Drawing.Color.Black;
            this.btnNumero.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNumero.ForeColor = System.Drawing.Color.White;
            this.btnNumero.Location = new System.Drawing.Point(320, 450);
            this.btnNumero.Name = "btnNumero";
            this.btnNumero.Size = new System.Drawing.Size(71, 25);
            this.btnNumero.TabIndex = 49;
            this.btnNumero.Text = "Insert Num";
            this.btnNumero.UseVisualStyleBackColor = false;
            this.btnNumero.Visible = false;
            this.btnNumero.Click += new System.EventHandler(this.btnNumero_Click);
            // 
            // btnEstadoX
            // 
            this.btnEstadoX.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnEstadoX.BackColor = System.Drawing.Color.Black;
            this.btnEstadoX.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEstadoX.ForeColor = System.Drawing.Color.White;
            this.btnEstadoX.Location = new System.Drawing.Point(244, 450);
            this.btnEstadoX.Name = "btnEstadoX";
            this.btnEstadoX.Size = new System.Drawing.Size(70, 25);
            this.btnEstadoX.TabIndex = 48;
            this.btnEstadoX.Text = "Insert ? Est";
            this.btnEstadoX.UseVisualStyleBackColor = false;
            this.btnEstadoX.Visible = false;
            this.btnEstadoX.Click += new System.EventHandler(this.btnEstadoX_Click);
            // 
            // btnError
            // 
            this.btnError.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnError.BackColor = System.Drawing.Color.Black;
            this.btnError.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnError.ForeColor = System.Drawing.Color.White;
            this.btnError.Location = new System.Drawing.Point(175, 450);
            this.btnError.Name = "btnError";
            this.btnError.Size = new System.Drawing.Size(63, 25);
            this.btnError.TabIndex = 47;
            this.btnError.Text = "Insert Err";
            this.btnError.UseVisualStyleBackColor = false;
            this.btnError.Visible = false;
            this.btnError.Click += new System.EventHandler(this.btnError_Click);
            // 
            // btnEstado5
            // 
            this.btnEstado5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnEstado5.BackColor = System.Drawing.Color.Black;
            this.btnEstado5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEstado5.ForeColor = System.Drawing.Color.White;
            this.btnEstado5.Location = new System.Drawing.Point(13, 450);
            this.btnEstado5.Name = "btnEstado5";
            this.btnEstado5.Size = new System.Drawing.Size(75, 25);
            this.btnEstado5.TabIndex = 46;
            this.btnEstado5.Text = "5 Winning Num";
            this.btnEstado5.UseVisualStyleBackColor = false;
            this.btnEstado5.Visible = false;
            this.btnEstado5.Click += new System.EventHandler(this.btnEstado5_Click);
            // 
            // btnEstado4
            // 
            this.btnEstado4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnEstado4.BackColor = System.Drawing.Color.Black;
            this.btnEstado4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEstado4.ForeColor = System.Drawing.Color.White;
            this.btnEstado4.Location = new System.Drawing.Point(94, 450);
            this.btnEstado4.Name = "btnEstado4";
            this.btnEstado4.Size = new System.Drawing.Size(75, 25);
            this.btnEstado4.TabIndex = 45;
            this.btnEstado4.Text = "4 No More Bets";
            this.btnEstado4.UseVisualStyleBackColor = false;
            this.btnEstado4.Visible = false;
            this.btnEstado4.Click += new System.EventHandler(this.btnEstado4_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.BackgroundImage")));
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(348, 530);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(87, 51);
            this.pictureBox1.TabIndex = 15;
            this.pictureBox1.TabStop = false;
            // 
            // pnlNapsaPie
            // 
            this.pnlNapsaPie.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlNapsaPie.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlNapsaPie.BackgroundImage")));
            this.pnlNapsaPie.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlNapsaPie.Location = new System.Drawing.Point(12, 530);
            this.pnlNapsaPie.Name = "pnlNapsaPie";
            this.pnlNapsaPie.Size = new System.Drawing.Size(423, 51);
            this.pnlNapsaPie.TabIndex = 16;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(194, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Parseo Protocolo";
            // 
            // lblLine
            // 
            this.lblLine.AutoSize = true;
            this.lblLine.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLine.Location = new System.Drawing.Point(9, 65);
            this.lblLine.Name = "lblLine";
            this.lblLine.Size = new System.Drawing.Size(89, 13);
            this.lblLine.TabIndex = 12;
            this.lblLine.Text = "Línea recibida";
            // 
            // btnSimular
            // 
            this.btnSimular.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSimular.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSimular.Location = new System.Drawing.Point(397, 450);
            this.btnSimular.Name = "btnSimular";
            this.btnSimular.Size = new System.Drawing.Size(43, 23);
            this.btnSimular.TabIndex = 9;
            this.btnSimular.Text = "Sim";
            this.btnSimular.UseVisualStyleBackColor = true;
            this.btnSimular.Click += new System.EventHandler(this.btnSimular_Click);
            // 
            // lvwMensajes
            // 
            this.lvwMensajes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lvwMensajes.BackColor = System.Drawing.Color.LightGray;
            this.lvwMensajes.Location = new System.Drawing.Point(12, 81);
            this.lvwMensajes.Name = "lvwMensajes";
            this.lvwMensajes.Size = new System.Drawing.Size(179, 363);
            this.lvwMensajes.TabIndex = 8;
            this.lvwMensajes.UseCompatibleStateImageBehavior = false;
            this.lvwMensajes.View = System.Windows.Forms.View.Details;
            this.lvwMensajes.DoubleClick += new System.EventHandler(this.lvwMensajes_DoubleClick);
            // 
            // lblEstadoCOM
            // 
            this.lblEstadoCOM.AutoSize = true;
            this.lblEstadoCOM.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEstadoCOM.Location = new System.Drawing.Point(9, 9);
            this.lblEstadoCOM.Name = "lblEstadoCOM";
            this.lblEstadoCOM.Size = new System.Drawing.Size(123, 13);
            this.lblEstadoCOM.TabIndex = 7;
            this.lblEstadoCOM.Text = "Puerto COM Cerrado";
            // 
            // pnlProtocoloTest
            // 
            this.pnlProtocoloTest.Controls.Add(this.txtCadenaOriginal);
            this.pnlProtocoloTest.Controls.Add(this.btnProbarCadena);
            this.pnlProtocoloTest.Location = new System.Drawing.Point(222, 3);
            this.pnlProtocoloTest.Name = "pnlProtocoloTest";
            this.pnlProtocoloTest.Size = new System.Drawing.Size(217, 57);
            this.pnlProtocoloTest.TabIndex = 4;
            // 
            // txtCadenaOriginal
            // 
            this.txtCadenaOriginal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCadenaOriginal.Location = new System.Drawing.Point(3, 3);
            this.txtCadenaOriginal.Name = "txtCadenaOriginal";
            this.txtCadenaOriginal.Size = new System.Drawing.Size(208, 20);
            this.txtCadenaOriginal.TabIndex = 4;
            this.txtCadenaOriginal.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCadenaOriginal_KeyPress);
            // 
            // btnProbarCadena
            // 
            this.btnProbarCadena.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProbarCadena.Location = new System.Drawing.Point(55, 29);
            this.btnProbarCadena.Name = "btnProbarCadena";
            this.btnProbarCadena.Size = new System.Drawing.Size(156, 23);
            this.btnProbarCadena.TabIndex = 3;
            this.btnProbarCadena.Text = "Intentar Parsear Cadena";
            this.btnProbarCadena.UseVisualStyleBackColor = true;
            this.btnProbarCadena.Click += new System.EventHandler(this.btnProbarCadena_Click);
            // 
            // txtProtocolo
            // 
            this.txtProtocolo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txtProtocolo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtProtocolo.Location = new System.Drawing.Point(197, 81);
            this.txtProtocolo.Multiline = true;
            this.txtProtocolo.Name = "txtProtocolo";
            this.txtProtocolo.Size = new System.Drawing.Size(242, 363);
            this.txtProtocolo.TabIndex = 6;
            // 
            // lblTips
            // 
            this.lblTips.AutoSize = true;
            this.lblTips.Location = new System.Drawing.Point(10, 488);
            this.lblTips.Name = "lblTips";
            this.lblTips.Size = new System.Drawing.Size(365, 26);
            this.lblTips.TabIndex = 14;
            this.lblTips.Text = "N | S | Nro | Nro | Estado | Velocidad | Velocidad | SentidoGiro | Error | CR(13)" +
    "\r\nN | N | Nro | Nro | | | | | CheckSum | CR(13)";
            this.lblTips.Visible = false;
            // 
            // tmrSimulacro
            // 
            this.tmrSimulacro.Interval = 500;
            this.tmrSimulacro.Tick += new System.EventHandler(this.tmrSimulacro_Tick);
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
            this.mnuSystemTray.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(23)))), ((int)(((byte)(26)))));
            this.mnuSystemTray.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cerrarToolStripMenuItem});
            this.mnuSystemTray.Name = "mnuSystemTray";
            this.mnuSystemTray.Size = new System.Drawing.Size(107, 26);
            // 
            // cerrarToolStripMenuItem
            // 
            this.cerrarToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.cerrarToolStripMenuItem.Name = "cerrarToolStripMenuItem";
            this.cerrarToolStripMenuItem.Size = new System.Drawing.Size(106, 22);
            this.cerrarToolStripMenuItem.Text = "Cerrar";
            this.cerrarToolStripMenuItem.Click += new System.EventHandler(this.cerrarToolStripMenuItem_Click);
            // 
            // tmrDemo
            // 
            this.tmrDemo.Interval = 500;
            this.tmrDemo.Tick += new System.EventHandler(this.tmrDemo_Tick);
            // 
            // FrmPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(23)))), ((int)(((byte)(26)))));
            this.ClientSize = new System.Drawing.Size(453, 584);
            this.Controls.Add(this.pnlPanel);
            this.ForeColor = System.Drawing.Color.White;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmPanel";
            this.Text = "Panel del Recolector III";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmPanel_FormClosing);
            this.Load += new System.EventHandler(this.FrmPanel_Load);
            this.Resize += new System.EventHandler(this.FrmPanel_Resize);
            this.pnlPanel.ResumeLayout(false);
            this.pnlPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.pnlProtocoloTest.ResumeLayout(false);
            this.pnlProtocoloTest.PerformLayout();
            this.mnuSystemTray.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlPanel;
        private System.Windows.Forms.Button btnPuertoOnOff;
        private Label lblEstadoCOM;
        private Panel pnlNapsaPie;
        private Button btnNumero;
        private Button btnIniciarDemo;
        private Button btnEstadoX;
        private Button btnError;
        private Button btnEstado5;
        private Button btnEstado4;
 //       private DASYS.GUI.VisorPopUp visorPopUp1;
        private PictureBox pictureBox1;
        private Label label3;
        private Label lblLine;
        private Label lblTips;
        private Button btnSimular;
        private ListView lvwMensajes;
        private Panel pnlProtocoloTest;
        private TextBox txtProtocolo;
        private Button btnProbarCadena;
        private TextBox txtCadenaOriginal;
        private NotifyIcon notifyIcon1;
        private ContextMenuStrip mnuSystemTray;
        private ToolStripMenuItem cerrarToolStripMenuItem;
    }
}