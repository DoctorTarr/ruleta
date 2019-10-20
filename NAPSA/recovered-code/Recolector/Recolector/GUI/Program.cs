// Decompiled with JetBrains decompiler
// Type: GUI.Program
// Assembly: Recolector, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0D03609E-ECAA-4078-98A3-46CE568862AA
// Assembly location: C:\Program Files (x86)\NAPSA\Colector III\Recolector.exe

using DASYS.Recolector.BLL;
using System;
using System.Windows.Forms;

namespace GUI
{
  internal static class Program
  {
    [STAThread]
    private static void Main(string[] args)
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
          Common.Logger.Escribir("*** RECOLECTOR INICIADO ***", true);
          Common.Logger.EscribirLinea();
        }
        catch
        {
        }
        if (!flag)
          return;
        Application.Run((Form) new FrmPanel());
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message, "Error");
      }
    }
  }
}
