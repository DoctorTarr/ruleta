using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DASYS.GUI
{
    public partial class FrmPanel2 : Form
    {
        private bool estadoPuerto;
        private int refreshCounter;
        private SerialPort port;
        private string cadena;
        private Random azarNumero;
        private Random azarComando;
        private int estadoDemo;
        private byte numeroDemo;
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

        public FrmPanel2()
        {
            InitializeComponent();
        }
    }
}
