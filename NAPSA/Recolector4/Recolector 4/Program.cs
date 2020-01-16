using DASYS.Recolector.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Recolector4
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                foreach (string str in args)
                {
                    if (str.ToLower().Contains("test"))
                        Common.EsTest = true;
                }
                bool flag = Iniciador.Iniciar(Application.StartupPath);
                try
                {
                    Common.Logger.EscribirLinea();
                    Common.Logger.Escribir("*** RECOLECTOR 4 v0.1 INICIADO ***", true);
                    Common.Logger.EscribirLinea();
                }
                catch
                {
                }
                if (!flag)
                    return;
                Application.Run(new MainForm());
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show(ex.Message, "Error");
            }

        }
    }
}
