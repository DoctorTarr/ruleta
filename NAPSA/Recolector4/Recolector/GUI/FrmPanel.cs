using DASYS.GUI;
using DASYS.Recolector.BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DASYS.GUI
{
    public partial class FrmPanel : Form
    {
        private bool estadoPuerto;
        private int refreshCounter;
        private SerialPort port;
        private string cadena;
        private Timer tmrSimulacro;
        private Random azarNumero;
        private Random azarComando;
        private int estadoDemo;
        private byte numeroDemo;
        private Timer tmrDemo;

        public FrmPanel()
        {
            InitializeComponent();
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
                    this.port.NewLine = "\r"; // Protocolo contra la cajita de sensores, usa CRLF
                    this.port.DataReceived += new SerialDataReceivedEventHandler(this.port_DataReceived);
                    flag = true;
                }
                else
                {
                    this.port = (SerialPort)null;
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
                    this.port = (SerialPort)null;
                    flag = false;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                this.port = (SerialPort)null;
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
                //this.visorPopUp1.MostrarMensaje(ex.Message);
                Common.Logger.Escribir("Error conexión a BD. No se pudo guardar: " + this.cadena, true);
            }
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
                int num = (int)MessageBox.Show(ex.Message);
            }
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
                            stringBuilder.AppendLine("Numero Ganado:" + (object)((ResultadoNumero)resultadoPaquete).NumeroGanador);
                            stringBuilder.AppendLine("Checksum: " + (object)((ResultadoNumero)resultadoPaquete).CheckSum);
                            break;
                        case ProtocoloNAPSA.ProtocoloTipoPaquete.Status:
                            stringBuilder.AppendLine("Paquete de tipo Status");
                            stringBuilder.AppendLine("Numero Ganado:" + (object)((ResultadoStatus)resultadoPaquete).NumeroGanador);
                            stringBuilder.AppendLine("Estado: " + (object)((ResultadoStatus)resultadoPaquete).Estado);
                            stringBuilder.AppendLine("Velocidad de Giro: " + (object)((ResultadoStatus)resultadoPaquete).VelocidadGiro);
                            stringBuilder.AppendLine("Sentido de Giro: " + (object)((ResultadoStatus)resultadoPaquete).SentidoGiro);
                            stringBuilder.AppendLine("Error: " + (object)((ResultadoStatus)resultadoPaquete).Error);
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
                //this.visorPopUp1.MostrarMensaje(ex.Message);
            }
            return stringBuilder.ToString();
        }

 


    }

}
