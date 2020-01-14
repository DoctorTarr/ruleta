// Decompiled with JetBrains decompiler
// Type: DASYS.GUI.VisorPopUp
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
  public class VisorPopUp : UserControl
  {
    public static int Altura = 100;
    private static UserControl _userForm = (UserControl) null;
    private static bool cerrar = true;
    private IContainer components;
    private Panel pnlVisorPopUp;
    private TextBox txtVisorPopUp;
    private Timer tmrVisorPopUp;

    public VisorPopUp()
    {
      this.InitializeComponent();
    }

    public VisorPopUp(int altura)
    {
      //this.InitializeComponent();
      //this.visorPopUp(altura, false);
    }

    public VisorPopUp(int altura, bool borde)
    {
      //this.InitializeComponent();
      //this.visorPopUp(altura, borde);
    }

    private void visorPopUp(int altura, bool borde)
    {
      //try
      //{
      //  VisorPopUp.Altura = altura;
      //  this.Height = VisorPopUp.Altura;
      //  if (!borde)
      //    return;
      //  this.pnlVisorPopUp.BorderStyle = BorderStyle.FixedSingle;
      //}
      //catch (Exception ex)
      //{
      //  Mensajero mensajero = new Mensajero("Error en el Popup de avisos.", ex, true);
      //}
    }

    public void MostrarMensaje(string mensaje)
    {
      this.MostrarMensaje(mensaje, (string) null, 3, Color.Black, Color.White, (UserControl) null);
    }

    public void MostrarMensaje(string mensaje, string detalleLog)
    {
      this.MostrarMensaje(mensaje, detalleLog, 3, Color.Black, Color.White, (UserControl) null);
    }

    public void MostrarMensaje(string mensaje, int segundos)
    {
      this.MostrarMensaje(mensaje, (string) null, segundos, Color.Black, Color.White, (UserControl) null);
    }

    public void MostrarMensaje(string mensaje, Color colorFuente)
    {
      this.MostrarMensaje(mensaje, (string) null, 3, colorFuente, Color.White, (UserControl) null);
    }

    public void MostrarMensaje(string mensaje, string detalleLog, int segundos)
    {
      this.MostrarMensaje(mensaje, detalleLog, segundos, Color.Black, Color.White, (UserControl) null);
    }

    public void MostrarMensaje(string mensaje, int segundos, UserControl userForm)
    {
      this.MostrarMensaje(mensaje, (string) null, segundos, Color.Black, Color.White, userForm);
    }

    public void MostrarMensaje(
      string mensaje,
      string detalleLog,
      int segundos,
      UserControl userForm)
    {
      this.MostrarMensaje(mensaje, detalleLog, segundos, Color.Black, Color.White, userForm);
    }

    public void MostrarMensaje(string mensaje, int segundos, Color colorFuente, Color colorFondo)
    {
      this.MostrarMensaje(mensaje, (string) null, segundos, colorFuente, colorFondo, (UserControl) null);
    }

    public void MostrarMensaje(
      string mensaje,
      string detalleLog,
      int segundos,
      Color colorFuente,
      Color colorFondo)
    {
      this.MostrarMensaje(mensaje, detalleLog, segundos, colorFuente, colorFondo, (UserControl) null);
    }

    public void MostrarMensaje(
      string mensaje,
      string detalleLog,
      int segundos,
      Color colorFuente,
      Color colorFondo,
      UserControl userForm)
    {
      if (!Common.Parametros.LogActivado || detalleLog == null)
        return;
      if (detalleLog == string.Empty)
        Common.Logger.Escribir(mensaje, true);
      else
        Common.Logger.Escribir(detalleLog, true);
    }

    private void VisorPopUp_Load(object sender, EventArgs e)
    {
      this.txtVisorPopUp.BackColor = Color.White;
      this.txtVisorPopUp.ForeColor = Color.Black;
    }

    private void tmrVisorPopUp_Tick(object sender, EventArgs e)
    {
      if (!VisorPopUp.cerrar)
        return;
      this.Height = 0;
      this.txtVisorPopUp.Text = string.Empty;
      this.txtVisorPopUp.ForeColor = Color.Black;
      this.tmrVisorPopUp.Stop();
      if (VisorPopUp._userForm == null)
        return;
      VisorPopUp._userForm.Dispose();
      VisorPopUp._userForm = (UserControl) null;
    }

    private void mantenerPopUpAbierto(object sender, EventArgs e)
    {
      VisorPopUp.cerrar = false;
    }

    private void cerrarPopUp(object sender, EventArgs e)
    {
      VisorPopUp.cerrar = true;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new Container();
      this.pnlVisorPopUp = new Panel();
      this.txtVisorPopUp = new TextBox();
      this.tmrVisorPopUp = new Timer(this.components);
      this.pnlVisorPopUp.SuspendLayout();
      this.SuspendLayout();
      this.pnlVisorPopUp.Controls.Add((Control) this.txtVisorPopUp);
      this.pnlVisorPopUp.Dock = DockStyle.Fill;
      this.pnlVisorPopUp.Location = new Point(0, 0);
      this.pnlVisorPopUp.Name = "pnlVisorPopUp";
      this.pnlVisorPopUp.Size = new Size(626, 100);
      this.pnlVisorPopUp.TabIndex = 0;
      this.txtVisorPopUp.BackColor = SystemColors.Window;
      this.txtVisorPopUp.BorderStyle = BorderStyle.None;
      this.txtVisorPopUp.Dock = DockStyle.Fill;
      this.txtVisorPopUp.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.txtVisorPopUp.Location = new Point(0, 0);
      this.txtVisorPopUp.Multiline = true;
      this.txtVisorPopUp.Name = "txtVisorPopUp";
      this.txtVisorPopUp.ReadOnly = true;
      this.txtVisorPopUp.Size = new Size(626, 100);
      this.txtVisorPopUp.TabIndex = 0;
      this.txtVisorPopUp.TextAlign = HorizontalAlignment.Center;
      this.txtVisorPopUp.Enter += new EventHandler(this.mantenerPopUpAbierto);
      this.txtVisorPopUp.MouseLeave += new EventHandler(this.cerrarPopUp);
      this.txtVisorPopUp.MouseEnter += new EventHandler(this.mantenerPopUpAbierto);
      this.tmrVisorPopUp.Tick += new EventHandler(this.tmrVisorPopUp_Tick);
      this.AutoScaleDimensions = new SizeF(7f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = SystemColors.Control;
      this.BorderStyle = BorderStyle.FixedSingle;
      this.Controls.Add((Control) this.pnlVisorPopUp);
      this.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.Name = "VisorPopUp";
      this.Size = new Size(626, 100);
      this.Load += new EventHandler(this.VisorPopUp_Load);
      this.pnlVisorPopUp.ResumeLayout(false);
      this.pnlVisorPopUp.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
