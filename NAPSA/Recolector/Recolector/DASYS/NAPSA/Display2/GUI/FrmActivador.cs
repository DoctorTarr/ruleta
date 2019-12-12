// Decompiled with JetBrains decompiler
// Type: DASYS.NAPSA.Display2.GUI.FrmActivador
// Assembly: Recolector, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0D03609E-ECAA-4078-98A3-46CE568862AA
// Assembly location: C:\Program Files (x86)\NAPSA\Colector III\Recolector.exe

using DASYS.Framework;
using DASYS.Recolector.BLL;
using Recolector.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DASYS.NAPSA.Display2.GUI
{
  public class FrmActivador : Form
  {
    private IContainer components;
    private TextBox txtLicencia;
    private Label lblLicencia;
    private Label lblActivacion;
    private TextBox txtActivacion;
    private Button btnGenerar;
    private Button btnAbrir;
    private ToolTip toolTip1;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmActivador));
      this.txtLicencia = new TextBox();
      this.lblLicencia = new Label();
      this.lblActivacion = new Label();
      this.txtActivacion = new TextBox();
      this.btnGenerar = new Button();
      this.btnAbrir = new Button();
      this.toolTip1 = new ToolTip(this.components);
      this.SuspendLayout();
      this.txtLicencia.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.txtLicencia.BackColor = Color.AntiqueWhite;
      this.txtLicencia.BorderStyle = BorderStyle.FixedSingle;
      this.txtLicencia.Location = new Point(12, 25);
      this.txtLicencia.Multiline = true;
      this.txtLicencia.Name = "txtLicencia";
      this.txtLicencia.ReadOnly = true;
      this.txtLicencia.Size = new Size(276, 82);
      this.txtLicencia.TabIndex = 0;
      this.toolTip1.SetToolTip((Control) this.txtLicencia, "Licencia del producto.");
      this.lblLicencia.AutoSize = true;
      this.lblLicencia.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblLicencia.ForeColor = Color.White;
      this.lblLicencia.Location = new Point(12, 9);
      this.lblLicencia.Name = "lblLicencia";
      this.lblLicencia.Size = new Size(55, 13);
      this.lblLicencia.TabIndex = 1;
      this.lblLicencia.Text = "Licencia";
      this.lblActivacion.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.lblActivacion.AutoSize = true;
      this.lblActivacion.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblActivacion.ForeColor = Color.White;
      this.lblActivacion.Location = new Point(12, 110);
      this.lblActivacion.Name = "lblActivacion";
      this.lblActivacion.Size = new Size(67, 13);
      this.lblActivacion.TabIndex = 2;
      this.lblActivacion.Text = "Activación";
      this.txtActivacion.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.txtActivacion.BackColor = Color.AntiqueWhite;
      this.txtActivacion.BorderStyle = BorderStyle.FixedSingle;
      this.txtActivacion.Location = new Point(15, 126);
      this.txtActivacion.Name = "txtActivacion";
      this.txtActivacion.ReadOnly = true;
      this.txtActivacion.Size = new Size(249, 20);
      this.txtActivacion.TabIndex = 3;
      this.toolTip1.SetToolTip((Control) this.txtActivacion, "Activación del producto.");
      this.btnGenerar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnGenerar.BackColor = Color.White;
      this.btnGenerar.FlatStyle = FlatStyle.Flat;
      this.btnGenerar.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.btnGenerar.Location = new Point(177, 151);
      this.btnGenerar.Name = "btnGenerar";
      this.btnGenerar.Size = new Size(113, 22);
      this.btnGenerar.TabIndex = 4;
      this.btnGenerar.Text = "Generar licencia";
      this.toolTip1.SetToolTip((Control) this.btnGenerar, "Genera una licencia para el producto, correspondiente a esta PC.");
      this.btnGenerar.UseVisualStyleBackColor = false;
      this.btnGenerar.Visible = false;
      this.btnGenerar.Click += new EventHandler(this.btnGenerar_Click);
      this.btnAbrir.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnAbrir.BackColor = Color.White;
      this.btnAbrir.FlatStyle = FlatStyle.Flat;
      this.btnAbrir.Image = (Image) Resources.miniAbrirCarpeta;
      this.btnAbrir.Location = new Point(270, 126);
      this.btnAbrir.Name = "btnAbrir";
      this.btnAbrir.Size = new Size(20, 19);
      this.btnAbrir.TabIndex = 5;
      this.toolTip1.SetToolTip((Control) this.btnAbrir, "Abrir un archivo de activación, en base a la licencia generada.");
      this.btnAbrir.UseVisualStyleBackColor = false;
      this.btnAbrir.Click += new EventHandler(this.btnAbrir_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = SystemColors.ControlText;
      this.ClientSize = new Size(300, 187);
      this.Controls.Add((Control) this.btnAbrir);
      this.Controls.Add((Control) this.btnGenerar);
      this.Controls.Add((Control) this.txtActivacion);
      this.Controls.Add((Control) this.lblActivacion);
      this.Controls.Add((Control) this.lblLicencia);
      this.Controls.Add((Control) this.txtLicencia);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.KeyPreview = true;
      this.Name = nameof (FrmActivador);
      this.Text = "NAPSA - Activación del producto";
      this.Load += new EventHandler(this.FrmActivador_Load);
      this.KeyDown += new KeyEventHandler(this.FrmActivador_KeyDown);
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    public FrmActivador()
    {
      this.InitializeComponent();
    }

    private void FrmActivador_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyValue != 27)
        return;
      this.Dispose();
    }

    private void btnGenerar_Click(object sender, EventArgs e)
    {
      string empty = string.Empty;
      try
      {
        this.txtLicencia.Clear();
        this.txtActivacion.Clear();
        string licencia = Seguridad.Activacion.CompletarLicenciaBase(Seguridad.Activacion.GenerarLicenciaBase("Néstor Pastor"), 0, "Néstor Pastor");
        this.txtLicencia.Text = licencia;
        string activacion = Seguridad.Activacion.GenerarActivacion(licencia, "Néstor Pastor");
        if (!Seguridad.Activacion.VerificarLicencia(licencia, activacion, "Néstor Pastor"))
          return;
        this.txtActivacion.Text = activacion;
        bool flag1 = Seguridad.Registry.EscribirRegistro(Seguridad.Registry.RegistryKeys.HKEY_LOCAL_MACHINE, "napsa", "display2", "product", "licence", (object) licencia);
        bool flag2 = Seguridad.Registry.EscribirRegistro(Seguridad.Registry.RegistryKeys.HKEY_LOCAL_MACHINE, "napsa", "display2", "product", "activation", (object) activacion);
        if (!flag1 || !flag2)
        {
          int num1 = (int) MessageBox.Show("No fue posible guardar el registro.");
        }
        else
        {
          int num2 = (int) MessageBox.Show("Producto Activado correctamente.", "Activación", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          this.Dispose();
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    private bool activar()
    {
      bool flag1 = false;
      try
      {
        string licencia = Seguridad.Activacion.CompletarLicenciaBase(this.txtLicencia.Text, 0, "Néstor Pastor");
        flag1 = Seguridad.Activacion.VerificarLicencia(licencia, this.txtActivacion.Text, "Néstor Pastor");
        if (flag1)
        {
          Producto.Licence = licencia;
          Producto.Activation = this.txtActivacion.Text;
          bool flag2 = Seguridad.Registry.EscribirRegistro(Seguridad.Registry.RegistryKeys.HKEY_LOCAL_MACHINE, "napsa", "display", "product", "licence", (object) Producto.Licence);
          bool flag3 = Seguridad.Registry.EscribirRegistro(Seguridad.Registry.RegistryKeys.HKEY_LOCAL_MACHINE, "napsa", "display", "product", "activation", (object) Producto.Activation);
          if (flag2)
          {
            if (flag3)
              goto label_6;
          }
          int num = (int) MessageBox.Show("No fue posible guardar el registro.");
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
label_6:
      return flag1;
    }

    private void FrmActivador_Load(object sender, EventArgs e)
    {
      this.txtLicencia.Text = Producto.Licence;
      this.txtActivacion.Text = Producto.Activation;
    }

    private void btnAbrir_Click(object sender, EventArgs e)
    {
      try
      {
        if (this.leerArchivoNAP().Equals("7874871878943175685964857418930371897410986783124879037890127438091274380917245189475098465849357894321613786504813465781647085674835067438015418934305894236136087461078564310895618904567891436780136507891657648706108756405431654154516098431658907698670423089659862982650916710891051876540916507841678407803318574310956657840413543149217120973489075189513570"))
          this.btnGenerar.Visible = true;
        else
          this.btnGenerar.Visible = false;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    private string leerArchivoACT()
    {
      string str = (string) null;
      try
      {
        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.Filter = "Archivo de activación (*.act)|*.act";
        openFileDialog.CheckFileExists = true;
        openFileDialog.CheckPathExists = true;
        openFileDialog.Multiselect = false;
        if (openFileDialog.ShowDialog() == DialogResult.OK)
          str = Archivos.Texto.LeerArchivo(openFileDialog.FileName);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
      return str;
    }

    private string leerArchivoNAP()
    {
      string str = (string) null;
      try
      {
        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.Filter = "Archivo llave (*.nap)|*.nap";
        openFileDialog.CheckFileExists = true;
        openFileDialog.CheckPathExists = true;
        openFileDialog.Multiselect = false;
        if (openFileDialog.ShowDialog() == DialogResult.OK)
          str = Archivos.Texto.LeerArchivo(openFileDialog.FileName);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
      return str;
    }
  }
}
