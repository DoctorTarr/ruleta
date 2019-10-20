// Decompiled with JetBrains decompiler
// Type: DASYS.GUI.Mensajero
// Assembly: Recolector, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0D03609E-ECAA-4078-98A3-46CE568862AA
// Assembly location: C:\Program Files (x86)\NAPSA\Colector III\Recolector.exe

using DASYS.Recolector.BLL;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DASYS.GUI
{
  public class Mensajero : Form
  {
    private string _mensaje = string.Empty;
    private string _detalle = string.Empty;
    private string _fuente = string.Empty;
    private IContainer components;
    private ImageList Imagenes;
    private ImageList Vinetas;
    private Panel pnlMensajero;
    private TextBox txtMensaje;
    private Button btn1;
    private Button btn2;
    private Button btn3;
    private Label lblVineta;
    private TextBox txtDetalle;
    private Label lblEnviarMail;
    private PictureBox picImagen;
    private Point posicionAnterior;
    private bool _escribirEvento;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager( typeof( Mensajero ));
      this.Imagenes = new ImageList(this.components);
      this.Vinetas = new ImageList(this.components);
      this.pnlMensajero = new Panel();
      this.picImagen = new PictureBox();
      this.lblEnviarMail = new Label();
      this.txtDetalle = new TextBox();
      this.txtMensaje = new TextBox();
      this.btn1 = new Button();
      this.btn2 = new Button();
      this.btn3 = new Button();
      this.lblVineta = new Label();
      this.pnlMensajero.SuspendLayout();
      ((ISupportInitialize) this.picImagen).BeginInit();
      this.SuspendLayout();
      this.Imagenes.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("Imagenes.ImageStream");
      this.Imagenes.TransparentColor = Color.Transparent;
      this.Imagenes.Images.SetKeyName(0, "informacion");
      this.Imagenes.Images.SetKeyName(1, "pregunta");
      this.Imagenes.Images.SetKeyName(2, "error");
      this.Imagenes.Images.SetKeyName(3, "exclamacion");
      this.Vinetas.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("Vinetas.ImageStream");
      this.Vinetas.TransparentColor = Color.Transparent;
      this.Vinetas.Images.SetKeyName(0, "contraer");
      this.Vinetas.Images.SetKeyName(1, "expandir");
      this.pnlMensajero.BorderStyle = BorderStyle.FixedSingle;
      this.pnlMensajero.Controls.Add((Control) this.picImagen);
      this.pnlMensajero.Controls.Add((Control) this.lblEnviarMail);
      this.pnlMensajero.Controls.Add((Control) this.txtDetalle);
      this.pnlMensajero.Controls.Add((Control) this.txtMensaje);
      this.pnlMensajero.Controls.Add((Control) this.btn1);
      this.pnlMensajero.Controls.Add((Control) this.btn2);
      this.pnlMensajero.Controls.Add((Control) this.btn3);
      this.pnlMensajero.Controls.Add((Control) this.lblVineta);
      this.pnlMensajero.Dock = DockStyle.Fill;
      this.pnlMensajero.Location = new Point(0, 0);
      this.pnlMensajero.Name = "pnlMensajero";
      this.pnlMensajero.Size = new Size(381, 143);
      this.pnlMensajero.TabIndex = 0;
      this.pnlMensajero.MouseDown += new MouseEventHandler(this.pnlMensajero_MouseDown);
      this.pnlMensajero.MouseMove += new MouseEventHandler(this.pnlMensajero_MouseMove);
      this.picImagen.Location = new Point(12, 11);
      this.picImagen.Name = "picImagen";
      this.picImagen.Size = new Size(53, 53);
      this.picImagen.TabIndex = 8;
      this.picImagen.TabStop = false;
      this.lblEnviarMail.FlatStyle = FlatStyle.Flat;
      this.lblEnviarMail.Location = new Point(44, 67);
      this.lblEnviarMail.Name = "lblEnviarMail";
      this.lblEnviarMail.Size = new Size(16, 16);
      this.lblEnviarMail.TabIndex = 7;
      this.lblEnviarMail.Visible = false;
      this.txtDetalle.BackColor = SystemColors.Window;
      this.txtDetalle.BorderStyle = BorderStyle.FixedSingle;
      this.txtDetalle.Location = new Point(11, 100);
      this.txtDetalle.Multiline = true;
      this.txtDetalle.Name = "txtDetalle";
      this.txtDetalle.ReadOnly = true;
      this.txtDetalle.Size = new Size(357, 75);
      this.txtDetalle.TabIndex = 6;
      this.txtDetalle.Visible = false;
      this.txtMensaje.BackColor = SystemColors.Window;
      this.txtMensaje.BorderStyle = BorderStyle.FixedSingle;
      this.txtMensaje.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.txtMensaje.Location = new Point(79, 11);
      this.txtMensaje.Multiline = true;
      this.txtMensaje.Name = "txtMensaje";
      this.txtMensaje.ReadOnly = true;
      this.txtMensaje.Size = new Size(289, 72);
      this.txtMensaje.TabIndex = 1;
      this.txtMensaje.TextAlign = HorizontalAlignment.Center;
      this.btn1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btn1.DialogResult = DialogResult.Yes;
      this.btn1.FlatStyle = FlatStyle.Flat;
      this.btn1.Location = new Point(36, 100);
      this.btn1.Name = "btn1";
      this.btn1.Size = new Size(62, 29);
      this.btn1.TabIndex = 2;
      this.btn1.UseVisualStyleBackColor = true;
      this.btn1.Visible = false;
      this.btn2.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btn2.DialogResult = DialogResult.No;
      this.btn2.FlatStyle = FlatStyle.Flat;
      this.btn2.Location = new Point(147, 101);
      this.btn2.Name = "btn2";
      this.btn2.Size = new Size(62, 29);
      this.btn2.TabIndex = 3;
      this.btn2.UseVisualStyleBackColor = true;
      this.btn2.Visible = false;
      this.btn3.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btn3.DialogResult = DialogResult.Cancel;
      this.btn3.FlatStyle = FlatStyle.Flat;
      this.btn3.Location = new Point(259, 101);
      this.btn3.Name = "btn3";
      this.btn3.Size = new Size(62, 29);
      this.btn3.TabIndex = 4;
      this.btn3.UseVisualStyleBackColor = true;
      this.btn3.Visible = false;
      this.lblVineta.FlatStyle = FlatStyle.Flat;
      this.lblVineta.ImageIndex = 1;
      this.lblVineta.ImageList = this.Vinetas;
      this.lblVineta.Location = new Point(12, 67);
      this.lblVineta.Name = "lblViñeta";
      this.lblVineta.Size = new Size(16, 16);
      this.lblVineta.TabIndex = 5;
      this.lblVineta.Click += new EventHandler(this.lblViñeta_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.Snow;
      this.ClientSize = new Size(381, 143);
      this.ControlBox = false;
      this.Controls.Add((Control) this.pnlMensajero);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Name = "Mensajero";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Mensajero";
      this.TopMost = true;
      this.Load += new EventHandler(this.Mensajero_Load);
      this.pnlMensajero.ResumeLayout(false);
      this.pnlMensajero.PerformLayout();
      ((ISupportInitialize) this.picImagen).EndInit();
      this.ResumeLayout(false);
    }

    public Mensajero()
    {
      this.InitializeComponent();
    }

    public Mensajero(string mensaje)
    {
      this.mostrarMensajero(mensaje, string.Empty, string.Empty, MessageBoxButtons.OK, MessageBoxDefaultButton.Button1, MessageBoxIcon.Asterisk, false);
    }

    public Mensajero(string mensaje, string detalle)
    {
      this.mostrarMensajero(mensaje, detalle, string.Empty, MessageBoxButtons.OK, MessageBoxDefaultButton.Button1, MessageBoxIcon.Asterisk, false);
    }

    public Mensajero(string mensaje, string detalle, string fuente)
    {
      this.mostrarMensajero(mensaje, detalle, fuente, MessageBoxButtons.OK, MessageBoxDefaultButton.Button1, MessageBoxIcon.Asterisk, false);
    }

    public Mensajero(
      string mensaje,
      string detalle,
      string fuente,
      MessageBoxButtons botones,
      MessageBoxIcon icono)
    {
      this.mostrarMensajero(mensaje, detalle, fuente, botones, MessageBoxDefaultButton.Button1, icono, false);
    }

    public Mensajero(
      string mensaje,
      string detalle,
      MessageBoxButtons botones,
      MessageBoxIcon icono)
    {
      this.mostrarMensajero(mensaje, detalle, string.Empty, botones, MessageBoxDefaultButton.Button1, icono, false);
    }

    public Mensajero(
      string mensaje,
      string detalle,
      MessageBoxButtons botones,
      MessageBoxIcon icono,
      bool escribirEvento)
    {
      this.mostrarMensajero(mensaje, detalle, string.Empty, botones, MessageBoxDefaultButton.Button1, icono, escribirEvento);
    }

    public Mensajero(
      string mensaje,
      string detalle,
      MessageBoxButtons botones,
      MessageBoxDefaultButton botonDefecto,
      MessageBoxIcon icono)
    {
      this.mostrarMensajero(mensaje, detalle, string.Empty, botones, botonDefecto, icono, false);
    }

    public Mensajero(
      string mensaje,
      string detalle,
      string fuente,
      MessageBoxButtons botones,
      MessageBoxIcon icono,
      bool escribirEvento)
    {
      this.mostrarMensajero(mensaje, detalle, fuente, botones, MessageBoxDefaultButton.Button1, icono, escribirEvento);
    }

    public Mensajero(
      string mensaje,
      string detalle,
      string fuente,
      MessageBoxButtons botones,
      MessageBoxDefaultButton botonDefault,
      MessageBoxIcon icono,
      bool escribirEvento)
    {
      this.mostrarMensajero(mensaje, detalle, fuente, botones, botonDefault, icono, escribirEvento);
    }

    public Mensajero(Exception exception)
    {
      this.mostrarMensajero("Se ha generado un error.", exception.Message, exception.Source, MessageBoxButtons.OK, MessageBoxDefaultButton.Button1, MessageBoxIcon.Hand, false);
    }

    public Mensajero(string mensaje, Exception exception)
    {
      this.mostrarMensajero(mensaje, exception.Message, exception.Source, MessageBoxButtons.OK, MessageBoxDefaultButton.Button1, MessageBoxIcon.Hand, false);
    }

    public Mensajero(string mensaje, Exception exception, bool escribirEvento)
    {
      this.mostrarMensajero(mensaje, exception.Message, exception.Source, MessageBoxButtons.OK, MessageBoxDefaultButton.Button1, MessageBoxIcon.Hand, escribirEvento);
    }

    private void mostrarMensajero(
      string mensaje,
      string detalle,
      string fuente,
      MessageBoxButtons botones,
      MessageBoxDefaultButton botonDefault,
      MessageBoxIcon icono,
      bool escribirEvento)
    {
      this.InitializeComponent();
      try
      {
        this._mensaje = mensaje;
        this._detalle = detalle;
        this._fuente = fuente;
        this._escribirEvento = escribirEvento;
        if (Common.Parametros.LogActivado && escribirEvento)
        {
          if (detalle == string.Empty)
            Common.Logger.Escribir(mensaje, true);
          else
            Common.Logger.Escribir(detalle, true);
        }
        this.txtMensaje.Text = mensaje;
        if (detalle == null || detalle == string.Empty)
        {
          this.lblVineta.Hide();
        }
        else
        {
          this.txtDetalle.Text = detalle;
          this.lblVineta.Show();
        }
        switch (botones)
        {
          case MessageBoxButtons.OK:
            this.btn1.Text = "Aceptar";
            this.btn1.DialogResult = DialogResult.OK;
            this.organizarBotones(1, botonDefault);
            break;
          case MessageBoxButtons.OKCancel:
            this.btn1.Text = "Aceptar";
            this.btn1.DialogResult = DialogResult.OK;
            this.btn2.Text = "Cancelar";
            this.btn2.DialogResult = DialogResult.Cancel;
            this.organizarBotones(2, botonDefault);
            break;
          case MessageBoxButtons.AbortRetryIgnore:
            this.btn1.Text = "Abortar";
            this.btn1.DialogResult = DialogResult.Abort;
            this.btn2.Text = "Reintentar";
            this.btn2.DialogResult = DialogResult.Retry;
            this.btn3.Text = "Ignorar";
            this.btn3.DialogResult = DialogResult.Ignore;
            this.organizarBotones(3, botonDefault);
            break;
          case MessageBoxButtons.YesNo:
            this.btn1.Text = "Sí";
            this.btn1.DialogResult = DialogResult.Yes;
            this.btn2.Text = "No";
            this.btn2.DialogResult = DialogResult.No;
            this.organizarBotones(2, botonDefault);
            break;
          default:
            this.btn1.Text = "Aceptar";
            this.btn1.DialogResult = DialogResult.OK;
            this.organizarBotones(1, botonDefault);
            break;
        }
        switch (icono)
        {
          case MessageBoxIcon.Hand:
            this.picImagen.Image = this.Imagenes.Images["error"];
            break;
          case MessageBoxIcon.Question:
            this.picImagen.Image = this.Imagenes.Images["pregunta"];
            break;
          case MessageBoxIcon.Exclamation:
            this.picImagen.Image = this.Imagenes.Images["exclamacion"];
            break;
          case MessageBoxIcon.Asterisk:
            this.picImagen.Image = this.Imagenes.Images["informacion"];
            break;
          default:
            this.picImagen.Image = (Image) null;
            break;
        }
        int num = (int) this.ShowDialog();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message, nameof (Mensajero), MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void lblViñeta_Click(object sender, EventArgs e)
    {
      if (this.lblVineta.ImageKey == "Contraer")
      {
        this.lblVineta.ImageKey = "Expandir";
        this.Height = 140;
        this.txtDetalle.Hide();
      }
      else
      {
        this.lblVineta.ImageKey = "Contraer";
        this.Height = 230;
        this.txtDetalle.Show();
      }
    }

    private void Mensajero_Load(object sender, EventArgs e)
    {
      try
      {
        this.BackColor = Color.White;
        this.ForeColor = Color.Black;
        this.Height = 140;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    private void btnEnviarMail_Click(object sender, EventArgs e)
    {
    }

    private void pnlMensajero_MouseMove(object sender, MouseEventArgs e)
    {
      if (e.Button != MouseButtons.Left || e.Button != MouseButtons.Left)
        return;
      this.Left = this.Left + e.X - this.posicionAnterior.X;
      this.Top = this.Top + e.Y - this.posicionAnterior.Y;
    }

    private void pnlMensajero_MouseDown(object sender, MouseEventArgs e)
    {
      if (e.Button != MouseButtons.Left)
        return;
      this.posicionAnterior = e.Location;
    }

    private void organizarBotones(int cantidadBotones, MessageBoxDefaultButton botonDefault)
    {
      int width1 = this.Width;
      int width2 = this.btn1.Width;
      int num1 = width2 / 5;
      int num2 = width2 + num1 * (cantidadBotones - 1);
      if (cantidadBotones < 1 || cantidadBotones > 3)
        cantidadBotones = 1;
      int num3 = (width1 - (width2 * cantidadBotones + num1 * (cantidadBotones - 1))) / 2;
      switch (cantidadBotones)
      {
        case 2:
          this.btn1.Left = num3;
          this.btn2.Left = num3 + num2;
          this.btn1.Show();
          this.btn2.Show();
          break;
        case 3:
          this.btn1.Left = num3;
          this.btn2.Left = num3 + num2;
          this.btn3.Left = num3 * 2 + num2;
          this.btn1.Show();
          this.btn2.Show();
          this.btn3.Show();
          break;
        default:
          this.btn1.Left = num3;
          this.btn1.Show();
          break;
      }
      switch (botonDefault)
      {
        case MessageBoxDefaultButton.Button2:
          this.btn2.Select();
          break;
        case MessageBoxDefaultButton.Button3:
          this.btn3.Select();
          break;
        default:
          this.btn1.Select();
          break;
      }
    }
  }
}
