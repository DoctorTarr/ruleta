// Decompiled with JetBrains decompiler
// Type: DASYS.Framework.UtilidadesImportacion
// Assembly: Framework, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1B503700-E29D-4D7A-BD70-519F036595D0
// Assembly location: C:\Program Files (x86)\NAPSA\Colector III\Framework.dll

using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Runtime.InteropServices;

namespace DASYS.Framework
{
  public static class UtilidadesImportacion
  {
    public static DataSet ImportarColumnas(
      UtilidadesImportacion.MotorBase motorBase,
      string ruta,
      string nombreTabla,
      List<string> nombresColumnas)
    {
      DataSet dataSet = (DataSet) null;
      try
      {
        OleDbConnection oleDbConnection = UtilidadesImportacion.obtenerOleDbConnection(motorBase, ruta);
        if (oleDbConnection != null)
        {
          OleDbCommand oleDbCommand = new OleDbCommand();
          oleDbCommand.Connection = oleDbConnection;
          string str = string.Empty;
          if (nombresColumnas != null && nombresColumnas.Count > 0)
          {
            foreach (string nombresColumna in nombresColumnas)
              str = str + nombresColumna + ",";
            if (str.EndsWith(","))
              str = str.Substring(0, str.Length - 1);
          }
          else
            str = "*";
          oleDbCommand.CommandText = string.Format("SELECT {0} FROM {1}", (object) str, (object) nombreTabla);
          OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter();
          oleDbDataAdapter.SelectCommand = oleDbCommand;
          dataSet = new DataSet();
          oleDbConnection.Open();
          oleDbDataAdapter.Fill(dataSet);
        }
      }
      catch
      {
        throw;
      }
      return dataSet;
    }

    private static OleDbConnection obtenerOleDbConnection(
      UtilidadesImportacion.MotorBase motorBase,
      string ruta)
    {
      OleDbConnection oleDbConnection = (OleDbConnection) null;
      try
      {
        string connectionString = (string) null;
        switch (motorBase)
        {
          case UtilidadesImportacion.MotorBase.ACCESS:
            connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + ruta + ";User Id=admin;Password=;";
            break;
          case UtilidadesImportacion.MotorBase.DBASE_IV:
            connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + ruta + ";Extended Properties=dBASE IV;User ID=Admin;Password=";
            break;
          case UtilidadesImportacion.MotorBase.MYSQL:
          case UtilidadesImportacion.MotorBase.MSSQL:
            connectionString = ruta;
            break;
        }
        if (!string.IsNullOrEmpty(connectionString))
          oleDbConnection = new OleDbConnection(connectionString);
      }
      catch
      {
        throw;
      }
      return oleDbConnection;
    }

    public static bool CompararInt32(int valorComparado, string signoComparativo, int referencia)
    {
      bool flag = false;
      switch (signoComparativo)
      {
        case "=":
          flag = valorComparado == referencia;
          break;
        case "<":
          flag = valorComparado < referencia;
          break;
        case ">":
          flag = valorComparado > referencia;
          break;
        case "<=":
          flag = valorComparado <= referencia;
          break;
        case ">=":
          flag = valorComparado >= referencia;
          break;
        case "<>":
          flag = valorComparado != referencia;
          break;
      }
      return flag;
    }

    public static bool CompararString(
      string valorComparado,
      string signoComparativo,
      string referencia)
    {
      bool flag = false;
      valorComparado = valorComparado.ToUpper();
      referencia = referencia.ToUpper();
      switch (signoComparativo)
      {
        case "=":
          flag = valorComparado == referencia;
          break;
        case "<>":
          flag = valorComparado != referencia;
          break;
      }
      return flag;
    }

    public static bool CompararDate(
      DateTime valorComparado,
      string signoComparativo,
      DateTime referencia)
    {
      bool flag = false;
      switch (signoComparativo)
      {
        case "=":
          flag = valorComparado.Date == referencia.Date;
          break;
        case "<":
          flag = valorComparado.Date < referencia.Date;
          break;
        case ">":
          flag = valorComparado.Date > referencia.Date;
          break;
        case "<=":
          flag = valorComparado.Date <= referencia.Date;
          break;
        case ">=":
          flag = valorComparado.Date >= referencia.Date;
          break;
        case "<>":
          flag = valorComparado.Date != referencia.Date;
          break;
      }
      return flag;
    }

    public static bool CompararBool(bool valorComparado, string signoComparativo, bool referencia)
    {
      bool flag = false;
      switch (signoComparativo)
      {
        case "=":
          flag = valorComparado == referencia;
          break;
        case "<>":
          flag = valorComparado != referencia;
          break;
      }
      return flag;
    }

    public enum MotorBase
    {
      Ninguno,
      ACCESS,
      DBASE_IV,
      MYSQL,
      MSSQL,
    }

    public static class ExcelUtility
    {
      public static DataSet ObtenerHojaDataSet(string archivo, List<string> columnas)
      {
        DataSet dataSet = (DataSet) null;
        try
        {
          if (!File.Exists(archivo))
            throw new Exception("El archivo no existe.");
          ApplicationClass app = new ApplicationClass();
          Worksheet activeSheet = (Worksheet) app.Workbooks.Open(archivo, (object) 0, (object) true, (object) 5, (object) "", (object) "", (object) true, (object) XlPlatform.xlWindows, (object) "\t", (object) false, (object) false, (object) 0, (object) true, (object) 1, (object) 0).ActiveSheet;
          int index1 = 0;
          object index2 = (object) 1;
          try
          {
            dataSet = new DataSet();
            dataSet.Tables.Add(new System.Data.DataTable());
            foreach (string columna in columnas)
              dataSet.Tables[0].Columns.Add(columna);
            for (; ((Microsoft.Office.Interop.Excel.Range) activeSheet.Cells[(object) (index1 + 1), index2]).Value2 != null; ++index1)
            {
              dataSet.Tables[0].Rows.Add((object) "");
              for (int index3 = 0; index3 < columnas.Count; ++index3)
              {
                if (((Microsoft.Office.Interop.Excel.Range) activeSheet.Cells[(object) (index1 + 1), (object) (index3 + 1)]).Value2 != null)
                  dataSet.Tables[0].Rows[index1][index3] = (object) ((Microsoft.Office.Interop.Excel.Range) activeSheet.Cells[(object) (index1 + 1), (object) (index3 + 1)]).Value2.ToString();
              }
            }
          }
          catch
          {
            UtilidadesImportacion.ExcelUtility.shutDownExcel(app);
            throw;
          }
          finally
          {
            UtilidadesImportacion.ExcelUtility.shutDownExcel(app);
          }
        }
        catch
        {
          throw;
        }
        return dataSet;
      }

      private static void NAR(object o)
      {
        try
        {
          Marshal.ReleaseComObject(o);
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex.StackTrace);
        }
        finally
        {
          o = (object) null;
        }
      }

      private static void shutDownExcel(ApplicationClass app)
      {
        UtilidadesImportacion.ExcelUtility.NAR((object) app.Worksheets);
        app.Workbooks.Close();
        UtilidadesImportacion.ExcelUtility.NAR((object) app.Workbooks);
        app.Quit();
        UtilidadesImportacion.ExcelUtility.NAR((object) app);
      }
    }
  }
}
