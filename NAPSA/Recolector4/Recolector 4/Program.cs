﻿using DASYS.Recolector.BLL;
using System;
using System.Diagnostics;
using System.Security.Principal;
using System.Threading;
using System.Windows.Forms;

namespace VideoRecolector
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            bool instanceCountOne = false;
            using (Mutex mtex = new Mutex(true, "Recolector 4", out instanceCountOne))
            {
                if (instanceCountOne)
                {
                    using (Process p = Process.GetCurrentProcess())
                        p.PriorityClass = ProcessPriorityClass.RealTime;
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
                        Common.Logger.Escribir($"Error! {ex.Message}-{ex.StackTrace}" , true);
                    }

                }
                else
                {
                    MessageBox.Show("Ya se está ejecutando.");
                }
            }
        }

        private static bool IsAdministrator()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
}
