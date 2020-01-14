using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DASYS.GUI
{
    partial class Mensajero
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(Mensajero));
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
            ((ISupportInitialize)this.picImagen).BeginInit();
            this.SuspendLayout();
            this.Imagenes.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("Imagenes.ImageStream");
            this.Imagenes.TransparentColor = Color.Transparent;
            this.Imagenes.Images.SetKeyName(0, "informacion");
            this.Imagenes.Images.SetKeyName(1, "pregunta");
            this.Imagenes.Images.SetKeyName(2, "error");
            this.Imagenes.Images.SetKeyName(3, "exclamacion");
            this.Vinetas.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("Vinetas.ImageStream");
            this.Vinetas.TransparentColor = Color.Transparent;
            this.Vinetas.Images.SetKeyName(0, "contraer");
            this.Vinetas.Images.SetKeyName(1, "expandir");
            this.pnlMensajero.BorderStyle = BorderStyle.FixedSingle;
            this.pnlMensajero.Controls.Add((Control)this.picImagen);
            this.pnlMensajero.Controls.Add((Control)this.lblEnviarMail);
            this.pnlMensajero.Controls.Add((Control)this.txtDetalle);
            this.pnlMensajero.Controls.Add((Control)this.txtMensaje);
            this.pnlMensajero.Controls.Add((Control)this.btn1);
            this.pnlMensajero.Controls.Add((Control)this.btn2);
            this.pnlMensajero.Controls.Add((Control)this.btn3);
            this.pnlMensajero.Controls.Add((Control)this.lblVineta);
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
            this.txtMensaje.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte)0);
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
            this.Controls.Add((Control)this.pnlMensajero);
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
            ((ISupportInitialize)this.picImagen).EndInit();
            this.ResumeLayout(false);
        }
        #endregion

        private string _mensaje = string.Empty;
        private string _detalle = string.Empty;
        private string _fuente = string.Empty;
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

    }
}
