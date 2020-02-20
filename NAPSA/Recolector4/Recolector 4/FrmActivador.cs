// Decompiled with JetBrains decompiler
// Type: DASYS.NAPSA.Display2.GUI.FrmActivador
// Assembly: Recolector, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0D03609E-ECAA-4078-98A3-46CE568862AA
// Assembly location: C:\Program Files (x86)\NAPSA\Colector III\Recolector.exe

using DASYS.Framework;
using DASYS.Recolector.BLL;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace VideoRecolector
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
            this.components = new System.ComponentModel.Container();
            this.txtLicencia = new System.Windows.Forms.TextBox();
            this.lblLicencia = new System.Windows.Forms.Label();
            this.lblActivacion = new System.Windows.Forms.Label();
            this.txtActivacion = new System.Windows.Forms.TextBox();
            this.btnGenerar = new System.Windows.Forms.Button();
            this.btnAbrir = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // txtLicencia
            // 
            this.txtLicencia.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLicencia.BackColor = System.Drawing.Color.AntiqueWhite;
            this.txtLicencia.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLicencia.Location = new System.Drawing.Point(12, 25);
            this.txtLicencia.Multiline = true;
            this.txtLicencia.Name = "txtLicencia";
            this.txtLicencia.ReadOnly = true;
            this.txtLicencia.Size = new System.Drawing.Size(276, 82);
            this.txtLicencia.TabIndex = 0;
            this.toolTip1.SetToolTip(this.txtLicencia, "Licencia del producto.");
            // 
            // lblLicencia
            // 
            this.lblLicencia.AutoSize = true;
            this.lblLicencia.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLicencia.ForeColor = System.Drawing.Color.White;
            this.lblLicencia.Location = new System.Drawing.Point(12, 9);
            this.lblLicencia.Name = "lblLicencia";
            this.lblLicencia.Size = new System.Drawing.Size(55, 13);
            this.lblLicencia.TabIndex = 1;
            this.lblLicencia.Text = "Licencia";
            // 
            // lblActivacion
            // 
            this.lblActivacion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblActivacion.AutoSize = true;
            this.lblActivacion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblActivacion.ForeColor = System.Drawing.Color.White;
            this.lblActivacion.Location = new System.Drawing.Point(12, 110);
            this.lblActivacion.Name = "lblActivacion";
            this.lblActivacion.Size = new System.Drawing.Size(67, 13);
            this.lblActivacion.TabIndex = 2;
            this.lblActivacion.Text = "Activación";
            // 
            // txtActivacion
            // 
            this.txtActivacion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtActivacion.BackColor = System.Drawing.Color.AntiqueWhite;
            this.txtActivacion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtActivacion.Location = new System.Drawing.Point(15, 126);
            this.txtActivacion.Name = "txtActivacion";
            this.txtActivacion.ReadOnly = true;
            this.txtActivacion.Size = new System.Drawing.Size(249, 20);
            this.txtActivacion.TabIndex = 3;
            this.toolTip1.SetToolTip(this.txtActivacion, "Activación del producto.");
            // 
            // btnGenerar
            // 
            this.btnGenerar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGenerar.BackColor = System.Drawing.Color.White;
            this.btnGenerar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGenerar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGenerar.Location = new System.Drawing.Point(177, 151);
            this.btnGenerar.Name = "btnGenerar";
            this.btnGenerar.Size = new System.Drawing.Size(113, 22);
            this.btnGenerar.TabIndex = 4;
            this.btnGenerar.Text = "Generar licencia";
            this.toolTip1.SetToolTip(this.btnGenerar, "Genera una licencia para el producto, correspondiente a esta PC.");
            this.btnGenerar.UseVisualStyleBackColor = false;
            this.btnGenerar.Visible = false;
            this.btnGenerar.Click += new System.EventHandler(this.btnGenerar_Click);
            // 
            // btnAbrir
            // 
            this.btnAbrir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAbrir.BackColor = System.Drawing.Color.White;
            this.btnAbrir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAbrir.Image = global::VideoRecolector.Properties.Resources.foldericon;
            this.btnAbrir.Location = new System.Drawing.Point(270, 126);
            this.btnAbrir.Name = "btnAbrir";
            this.btnAbrir.Size = new System.Drawing.Size(20, 19);
            this.btnAbrir.TabIndex = 5;
            this.toolTip1.SetToolTip(this.btnAbrir, "Abrir un archivo de activación, en base a la licencia generada.");
            this.btnAbrir.UseVisualStyleBackColor = false;
            this.btnAbrir.Click += new System.EventHandler(this.btnAbrir_Click);
            // 
            // FrmActivador
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlText;
            this.ClientSize = new System.Drawing.Size(300, 187);
            this.Controls.Add(this.btnAbrir);
            this.Controls.Add(this.btnGenerar);
            this.Controls.Add(this.txtActivacion);
            this.Controls.Add(this.lblActivacion);
            this.Controls.Add(this.lblLicencia);
            this.Controls.Add(this.txtLicencia);
            this.KeyPreview = true;
            this.Name = "FrmActivador";
            this.Text = "NAPSA - Activación del producto";
            this.Load += new System.EventHandler(this.FrmActivador_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmActivador_KeyDown);
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
        bool flag1 = Seguridad.Registry.EscribirRegistro(Seguridad.Registry.RegistryKeys.HKEY_LOCAL_MACHINE, "napsa", "recolector4", "product", "licence", (object) licencia);
        bool flag2 = Seguridad.Registry.EscribirRegistro(Seguridad.Registry.RegistryKeys.HKEY_LOCAL_MACHINE, "napsa", "recolector4", "product", "activation", (object) activacion);
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
               return flag1;
          }
          Common.Logger.Escribir("Error en activar(): No fue posible guardar el registro.");
        }
      }
      catch (Exception ex)
      {
        Common.Logger.Escribir($"Error ObtenerUltimoEstado(): {ex.Message} - {ex.StackTrace}");
      }

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
