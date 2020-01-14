// Decompiled with JetBrains decompiler
// Type: GUI.FrmPanel
// Assembly: Recolector, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0D03609E-ECAA-4078-98A3-46CE568862AA
// Assembly location: C:\Program Files (x86)\NAPSA\Colector III\Recolector.exe

using DASYS.GUI;
using DASYS.Recolector.BLL;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO.Ports;
using System.Text;
using System.Windows.Forms;

namespace DASYS.GUI
{
  public class FrmPanel : Form
  {
    private bool estadoPuerto;
    private int refreshCounter;
    private SerialPort port;
    private string cadena;
    private Random azarNumero;
    private Random azarComando;
    private int estadoDemo;
    private byte numeroDemo;
    //private IContainer components;
    private Button btnPuertoOnOff;
    private Panel pnlPanel;
    private VisorPopUp visorPopUp1;
    private Panel pnlProtocoloTest;
    private Button btnProbarCadena;
    private TextBox txtCadenaOriginal;
    private TextBox txtProtocolo;
    private Label lblEstadoCOM;
    private ListView lvwMensajes;
    private Button btnSimular;
    private Label label3;
    private Label lblLine;
    private Label lblTips;
    private Timer tmrSimulacro;
    private PictureBox pictureBox1;
    private Panel pnlNapsaPie;
    private NotifyIcon notifyIcon1;
    private ContextMenuStrip mnuSystemTray;
    private ToolStripMenuItem cerrarToolStripMenuItem;
    private Button btnEstadoX;
    private Button btnError;
    private Button btnEstado5;
    private Button btnEstado4;
    private Button btnNumero;
    private Button btnIniciarDemo;
    private Timer tmrDemo;

    public FrmPanel()
    {
      this.InitializeComponent();
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
        this.azarNumero = new Random((int) DateTime.Now.Ticks);
        this.azarComando = new Random((int) DateTime.Now.Ticks);
        this.lvwMensajes.Columns.Add("mensaje", "Mensaje", 200);
        this.leerUltimoNumero();
        if (Pase.UltimoPase == null)
          Pase.UltimoPase = new Pase();
        this.btnPuertoOnOff.PerformClick();
        this.Hide();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    private void leerUltimoNumero()
    {
      bool flag = false;
      DateTime now = DateTime.Now;
      do
      {
        try
        {
          Application.DoEvents();
          Pase.UltimoPase = Pase.LeerUltimoNumeroDesdeBase();
          flag = true;
        }
        catch
        {
          Common.Logger.Escribir("La conexión a la base de datos ha fallado al iniciar.", true);
        }
        if (flag)
          goto label_5;
      }
      while (!(now.AddMinutes(1.0) < DateTime.Now));
      goto label_6;
label_5:
      return;
label_6:;
    }

    private bool abrirPuerto()
    {
      bool flag;
      try
      {
        if (this.port == null || !this.port.IsOpen)
        {
          this.port = new SerialPort("COM1", 9600, Parity.None, 8, StopBits.One);
          this.port.Open();
          this.port.NewLine = "\r\n";
          this.port.DataReceived += new SerialDataReceivedEventHandler(this.port_DataReceived);
          flag = true;
        }
        else
        {
          this.port = (SerialPort) null;
          flag = false;
        }
      }
      catch (Exception ex)
      {
        throw;
      }
      return flag;
    }

    private bool cerrarPuerto()
    {
      bool flag = false;
      try
      {
        if (this.port != null && this.port.IsOpen)
        {
          this.port.DataReceived -= new SerialDataReceivedEventHandler(this.port_DataReceived);
          this.port.Close();
          flag = true;
        }
        else
        {
          this.port = (SerialPort) null;
          flag = false;
        }
      }
      catch (Exception ex)
      {
        throw;
      }
      finally
      {
        this.port = (SerialPort) null;
      }
      return flag;
    }

    private void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
    {
      try
      {
        this.cadena = "";
        this.cadena = this.port.ReadLine();
        ++this.refreshCounter;
        if (this.refreshCounter > 30)
          this.refreshCounter = 0;
        if (this.tmrDemo.Enabled)
          return;
        Persistencia.Guardar(this.cadena);
      }
      catch (Exception ex)
      {
        this.visorPopUp1.MostrarMensaje(ex.Message);
        Common.Logger.Escribir("Error conexión a BD. No se pudo guardar: " + this.cadena, true);
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
            this.visorPopUp1.MostrarMensaje("Puerto COM1 cerrado.");
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
          this.visorPopUp1.MostrarMensaje("Puerto COM1 abierto.");
          Common.Logger.Escribir("Puerto COM1 abierto correctamente", true);
        }
        else
          this.lblEstadoCOM.Text = "Error";
      }
      catch (Exception ex)
      {
        this.visorPopUp1.MostrarMensaje(ex.Message);
        Common.Logger.Escribir("Error puerto COM: " + ex.Message, true);
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
        this.visorPopUp1.MostrarMensaje(mensaje);
      }
      catch (Exception ex)
      {
        this.visorPopUp1.MostrarMensaje(ex.Message);
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
        this.visorPopUp1.MostrarMensaje(ex.Message);
      }
    }

    private void lvwMensajes_DoubleClick(object sender, EventArgs e)
    {
      if (this.lvwMensajes.SelectedItems.Count <= 0)
        return;
      this.txtProtocolo.Text = this.parsearCadena(this.lvwMensajes.SelectedItems[0].Text);
    }

    private void tmrSimulacro_Tick(object sender, EventArgs e)
    {
      try
      {
        if (this.azarComando.Next(0, 100) > 95)
        {
          this.cadena = "NN" + ((byte) this.azarNumero.Next(0, 38)).ToString("00") + "    N";
        }
        else
        {
          this.cadena = "NS" + Pase.UltimoPase.Numero.ToString("00");
          this.cadena += ((byte) this.azarNumero.Next(1, 7)).ToString();
          this.cadena += ((byte) this.azarNumero.Next(0, 100)).ToString("00");
          this.cadena += ((byte) this.azarNumero.Next(0, 2)).ToString();
          byte num = 0;
          if (this.azarNumero.Next(0, 100) > 95)
            num = (byte) this.azarNumero.Next(0, 10);
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
        this.visorPopUp1.MostrarMensaje(ex.Message);
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

    private string parsearCadena(string cadenaOriginal)
    {
      StringBuilder stringBuilder = new StringBuilder();
      try
      {
        IResultadosPaquete resultadoPaquete = new ProtocoloNAPSA(cadenaOriginal).ResultadoPaquete;
        if (resultadoPaquete != null)
        {
          switch (resultadoPaquete.TipoPaquete)
          {
            case ProtocoloNAPSA.ProtocoloTipoPaquete.NumeroGanador:
              stringBuilder.AppendLine("Paquete de tipo NumeroGanador");
              stringBuilder.AppendLine("Numero Ganado:" + (object) ((ResultadoNumero) resultadoPaquete).NumeroGanador);
              stringBuilder.AppendLine("Checksum: " + (object) ((ResultadoNumero) resultadoPaquete).CheckSum);
              break;
            case ProtocoloNAPSA.ProtocoloTipoPaquete.Status:
              stringBuilder.AppendLine("Paquete de tipo Status");
              stringBuilder.AppendLine("Numero Ganado:" + (object) ((ResultadoStatus) resultadoPaquete).NumeroGanador);
              stringBuilder.AppendLine("Estado: " + (object) ((ResultadoStatus) resultadoPaquete).Estado);
              stringBuilder.AppendLine("Velocidad de Giro: " + (object) ((ResultadoStatus) resultadoPaquete).VelocidadGiro);
              stringBuilder.AppendLine("Sentido de Giro: " + (object) ((ResultadoStatus) resultadoPaquete).SentidoGiro);
              stringBuilder.AppendLine("Error: " + (object) ((ResultadoStatus) resultadoPaquete).Error);
              break;
            default:
              stringBuilder.AppendLine("Paquete No Implementado.");
              break;
          }
        }
        else
          stringBuilder.AppendLine("Paquete mal formado.");
      }
      catch (Exception ex)
      {
        this.visorPopUp1.MostrarMensaje(ex.Message);
      }
      return stringBuilder.ToString();
    }

    private void cerrar()
    {
      try
      {
        this.cerrarPuerto();
        Common.Logger.EscribirLinea();
        Common.Logger.Escribir("*** RECOLECTOR FINALIZADO ***", true);
        Common.Logger.EscribirLinea();
        this.Dispose();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
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
        int num = (int) MessageBox.Show(ex.Message);
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
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    private void btnError_Click(object sender, EventArgs e)
    {
      try
      {
        this.cadena = "NS" + Pase.UltimoPase.Numero.ToString("00");
        this.cadena += ((byte) this.azarNumero.Next(0, 7)).ToString();
        this.cadena += ((byte) this.azarNumero.Next(0, 100)).ToString("00");
        this.cadena += ((byte) this.azarNumero.Next(0, 2)).ToString();
        this.cadena += ((byte) this.azarNumero.Next(0, 10)).ToString();
        Persistencia.Guardar(this.cadena);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
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
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    private void btnNumero_Click(object sender, EventArgs e)
    {
      try
      {
        this.cadena = "NN" + ((byte) this.azarNumero.Next(0, 38)).ToString("00") + "    N";
        Persistencia.Guardar(this.cadena);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    private void FrmPanel_Resize(object sender, EventArgs e)
    {
      if (this.WindowState == FormWindowState.Minimized)
        this.Hide();
      else
        this.Show();
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
            this.numeroDemo = (byte) this.azarNumero.Next(0, 37);
            cadena = "NN" + this.numeroDemo.ToString("00") + "    N";
            this.tmrDemo.Interval = 1000;
            break;
        }
        Persistencia.Guardar(cadena);
      }
      catch (Exception ex)
      {
        int num = 0 + 1;
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
      {
        this.notifyIcon1.Dispose();
        this.components.Dispose();
      }
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmPanel));
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
      ((ISupportInitialize) this.pictureBox1).BeginInit();
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
      this.pnlPanel.Controls.Add((Control) this.btnIniciarDemo);
      this.pnlPanel.Controls.Add((Control) this.btnNumero);
      this.pnlPanel.Controls.Add((Control) this.btnEstadoX);
      this.pnlPanel.Controls.Add((Control) this.btnError);
      this.pnlPanel.Controls.Add((Control) this.btnEstado5);
      this.pnlPanel.Controls.Add((Control) this.btnEstado4);
      this.pnlPanel.Controls.Add((Control) this.visorPopUp1);
      this.pnlPanel.Controls.Add((Control) this.pictureBox1);
      this.pnlPanel.Controls.Add((Control) this.pnlNapsaPie);
      this.pnlPanel.Controls.Add((Control) this.label3);
      this.pnlPanel.Controls.Add((Control) this.lblLine);
      this.pnlPanel.Controls.Add((Control) this.btnSimular);
      this.pnlPanel.Controls.Add((Control) this.lvwMensajes);
      this.pnlPanel.Controls.Add((Control) this.lblEstadoCOM);
      this.pnlPanel.Controls.Add((Control) this.pnlProtocoloTest);
      this.pnlPanel.Controls.Add((Control) this.btnPuertoOnOff);
      this.pnlPanel.Controls.Add((Control) this.txtProtocolo);
      this.pnlPanel.Controls.Add((Control) this.lblTips);
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
      this.pictureBox1.BackgroundImage = (Image) componentResourceManager.GetObject("pictureBox1.BackgroundImage");
      this.pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
      this.pictureBox1.Location = new Point(348, 530);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new Size(87, 51);
      this.pictureBox1.TabIndex = 15;
      this.pictureBox1.TabStop = false;
      this.pnlNapsaPie.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlNapsaPie.BackgroundImage = (Image) componentResourceManager.GetObject("pnlNapsaPie.BackgroundImage");
      this.pnlNapsaPie.BackgroundImageLayout = ImageLayout.Stretch;
      this.pnlNapsaPie.Location = new Point(12, 530);
      this.pnlNapsaPie.Name = "pnlNapsaPie";
      this.pnlNapsaPie.Size = new Size(423, 51);
      this.pnlNapsaPie.TabIndex = 16;
      this.label3.AutoSize = true;
      this.label3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label3.Location = new Point(194, 65);
      this.label3.Name = "label3";
      this.label3.Size = new Size(104, 13);
      this.label3.TabIndex = 13;
      this.label3.Text = "Parseo Protocolo";
      this.lblLine.AutoSize = true;
      this.lblLine.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
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
      this.lblEstadoCOM.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblEstadoCOM.Location = new Point(9, 9);
      this.lblEstadoCOM.Name = "lblEstadoCOM";
      this.lblEstadoCOM.Size = new Size(123, 13);
      this.lblEstadoCOM.TabIndex = 7;
      this.lblEstadoCOM.Text = "Puerto COM Cerrado";
      this.pnlProtocoloTest.Controls.Add((Control) this.txtCadenaOriginal);
      this.pnlProtocoloTest.Controls.Add((Control) this.btnProbarCadena);
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
      this.notifyIcon1.Icon = (Icon) componentResourceManager.GetObject("notifyIcon1.Icon");
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
      this.visorPopUp1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.visorPopUp1.Location = new Point(0, 584);
      this.visorPopUp1.Name = "visorPopUp1";
      this.visorPopUp1.Size = new Size(453, 0);
      this.visorPopUp1.TabIndex = 1;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.FromArgb(25, 23, 26);
      this.ClientSize = new Size(453, 584);
      this.Controls.Add((Control) this.pnlPanel);
      this.ForeColor = Color.White;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = "FrmPanel";
      this.Text = "Panel del Recolector II";
      this.WindowState = FormWindowState.Minimized;
      this.Load += new EventHandler(this.FrmPanel_Load);
      this.FormClosing += new FormClosingEventHandler(this.FrmPanel_FormClosing);
      this.Resize += new EventHandler(this.FrmPanel_Resize);
      this.pnlPanel.ResumeLayout(false);
      this.pnlPanel.PerformLayout();
      ((ISupportInitialize) this.pictureBox1).EndInit();
      this.pnlProtocoloTest.ResumeLayout(false);
      this.pnlProtocoloTest.PerformLayout();
      this.mnuSystemTray.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
