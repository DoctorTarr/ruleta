// Decompiled with JetBrains decompiler
// Type: DASYS.Framework.Utils
// Assembly: Framework, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1B503700-E29D-4D7A-BD70-519F036595D0
// Assembly location: C:\Program Files (x86)\NAPSA\Colector III\Framework.dll

using DASYS.ACL;
using DASYS.DAL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace DASYS.Framework
{
  public static class Utils
  {
    public static Connections oConexiones = new Connections();
    public static string hardDriveLetter = "c";
    public static Producto oProducto = (Producto) null;

    internal static DateTime? ObtenerFechaHoraServidorBD()
    {
      string empty = string.Empty;
      DateTime? nullable = new DateTime?();
      try
      {
        QueryEngine query = new QueryEngine("OS_ObtenerFechaHora");
        nullable = Utils.Datos.StringToDateTime(Utils.Datos.NullToString(Utils.oConexiones["CP"].DbExecuteScalar(query)));
      }
      catch
      {
        nullable = DateTime.Now;
      }
            return nullable;
    }

    public static void EjecutarProceso(string nombreProceso)
    {
      Utils.EjecutarProceso(nombreProceso, (string) null);
    }

    public static void EjecutarProceso(string nombreProceso, string argumento)
    {
      try
      {
        Process process = new Process();
        process.StartInfo.FileName = nombreProceso;
        if (argumento != null && argumento != string.Empty)
          process.StartInfo.Arguments = argumento;
        process.StartInfo.CreateNoWindow = false;
        process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
        process.Start();
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message);
      }
    }

    public static class Datos
    {
      public static char NullToChar(object value)
      {
        return Utils.Datos.NullToChar(value, char.MinValue);
      }

      public static char NullToChar(object value, char defValue)
      {
        if (value == null)
          return defValue;
        return Convert.ToChar(value);
      }

      public static string NullToString(object value)
      {
        return Utils.Datos.NullToString(value, string.Empty);
      }

      public static string NullToString(object value, string defValue)
      {
        if (value == null)
          return defValue;
        return Convert.ToString(value);
      }

      public static bool NullToBoolean(object value)
      {
        return Utils.Datos.NullToBoolean(value, false);
      }

      public static bool NullToBoolean(object value, bool defValue)
      {
        if (value == null || value.ToString() == string.Empty)
          return defValue;
        return Convert.ToBoolean(value);
      }

      public static byte NullToByte(object value)
      {
        return Utils.Datos.NullToByte(value, (byte) 0);
      }

      public static byte NullToByte(object value, byte defValue)
      {
        if (value == null || value.ToString() == string.Empty)
          return defValue;
        return Convert.ToByte(value);
      }

      public static short NullToInt16(object value)
      {
        return Utils.Datos.NullToInt16(value, (short) 0);
      }

      public static short NullToInt16(object value, short defValue)
      {
        if (value == null || value.ToString() == string.Empty)
          return defValue;
        return Convert.ToInt16(value);
      }

      public static int NullToInt32(object value)
      {
        return Utils.Datos.NullToInt32(value, 0);
      }

      public static int NullToInt32(object value, int defValue)
      {
        if (value == null || value.ToString() == string.Empty)
          return defValue;
        return Convert.ToInt32(value);
      }

      public static long NullToInt64(object value)
      {
        return Utils.Datos.NullToInt64(value, 0L);
      }

      public static long NullToInt64(object value, long defValue)
      {
        if (value == null || value.ToString() == string.Empty)
          return defValue;
        return Convert.ToInt64(value);
      }

      public static float NullToFloat(object value)
      {
        return Utils.Datos.NullToFloat(value, 0.0f);
      }

      public static float NullToFloat(object value, float defValue)
      {
        if (value == null || value.ToString() == string.Empty)
          return defValue;
        return Convert.ToSingle(value);
      }

      public static double NullToDouble(object value)
      {
        return Utils.Datos.NullToDouble(value, 0.0);
      }

      public static double NullToDouble(object value, double defValue)
      {
        if (value == null || value.ToString() == string.Empty)
          return defValue;
        return Convert.ToDouble(value);
      }

      public static DateTime NullToDateTime(object value)
      {
        return Utils.Datos.NullToDateTime(value, DateTime.MinValue);
      }

      public static DateTime NullToDateTime(object value, DateTime defValue)
      {
        if (value == null || value.ToString() == string.Empty)
          return defValue;
        return Convert.ToDateTime(value);
      }

      public static TimeSpan NullToTimeSpan(object value)
      {
        return Utils.Datos.NullToTimeSpan(value, new TimeSpan(0, 0, 0));
      }

      public static TimeSpan NullToTimeSpan(object value, TimeSpan defValue)
      {
        if (value == null || value.ToString() == string.Empty)
          return defValue;
        TimeSpan result;
        TimeSpan.TryParse(value.ToString(), out result);
        return result;
      }

      public static Decimal NullToDecimal(object value)
      {
        return Utils.Datos.NullToDecimal(value, new Decimal(0));
      }

      public static Decimal NullToDecimal(object value, Decimal defValue)
      {
        if (value == null || value.ToString() == string.Empty)
          return defValue;
        return Convert.ToDecimal(value);
      }

      public static object DateTimeToNull(DateTime value)
      {
        if (value == DateTime.MinValue)
          return (object) DBNull.Value;
        return (object) value;
      }

      public static object StringToNull(string value)
      {
        if (value == string.Empty)
          return (object) DBNull.Value;
        return (object) value;
      }

      public static object Int32ToNull(int value)
      {
        return Utils.Datos.Int32ToNull(value, 0);
      }

      public static object Int32ToNull(int value, int defValue)
      {
        if (value == defValue)
          return (object) DBNull.Value;
        return (object) value;
      }

      public static float StringToCulturalFloat(string value)
      {
        return Utils.Datos.StringToCulturalFloat(value, 0.0f);
      }

      public static float StringToCulturalFloat(string value, float defValue)
      {
        float result = defValue;
        if (!string.IsNullOrEmpty(value))
        {
          if (CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator != ".")
            value = value.Replace(".", CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator);
          if (!float.TryParse(value, out result))
            result = defValue;
        }
        return result;
      }

      public static string FirstLetterToCapital(string value)
      {
        return Utils.Datos.FirstLetterToCapital(value, false);
      }

      public static string FirstLetterToCapital(string value, bool allWords)
      {
        string str1 = string.Empty;
        if (!string.IsNullOrEmpty(value))
        {
          if (allWords)
          {
            string str2 = value;
            char[] chArray = new char[1]{ ' ' };
            foreach (string str3 in str2.Split(chArray))
            {
              if (!string.IsNullOrEmpty(str3))
                str1 = str1 + str3.Substring(0, 1).ToUpper() + str3.Substring(1, str3.Length - 1).ToLower() + " ";
            }
            if (str1.EndsWith(" "))
              str1 = str1.Substring(0, str1.Length - 1);
          }
          else
          {
            string str2 = value.Trim();
            if (!string.IsNullOrEmpty(str2))
              str1 = str2.Substring(0, 1).ToUpper() + str2.Substring(1, str2.Length - 1);
          }
        }
        return str1;
      }

      public static string Int32ArrayToString(List<int> lectores)
      {
        string str = string.Empty;
        if (lectores != null && lectores.Count > 0)
        {
          foreach (int lectore in lectores)
            str = str + lectore.ToString() + ", ";
          str = str.Remove(str.Length - 2, 1);
        }
        return str;
      }

      public static List<int> StringToInt32Array(string value, char separador)
      {
        List<int> intList = new List<int>();
        if (!string.IsNullOrEmpty(value))
        {
          intList = new List<int>();
          string str1 = value;
          char[] chArray = new char[1]{ separador };
          foreach (string str2 in str1.Split(chArray))
            intList.Add(Utils.Datos.NullToInt32((object) str2));
        }
        return intList;
      }

      public static bool StringToBoolean(object valor)
      {
        return Utils.Datos.StringToBoolean(valor, false);
      }

      public static bool StringToBoolean(object valor, bool defValue)
      {
        bool flag = false;
        if (valor != null)
        {
          switch (valor.ToString().ToUpper())
          {
            case "T":
            case "TRUE":
            case "V":
            case "VERDADERO":
            case "1":
              flag = true;
              break;
          }
        }
        return flag;
      }

      public static DateTime? StringToDateTime(string fechaHora)
      {
        DateTime? nullable = new DateTime?();
        try
        {
          nullable = new DateTime?(DateTime.Parse(fechaHora));
        }
        catch
        {
        }
        return nullable;
      }

      public static byte[] StringToByteArray(string cadena)
      {
        byte[] numArray = (byte[]) null;
        try
        {
          if (!string.IsNullOrEmpty(cadena))
            numArray = new ASCIIEncoding().GetBytes(cadena);
        }
        catch
        {
        }
        return numArray;
      }

      public static IPAddress StringToIP(string ip)
      {
        return Utils.Datos.StringToIP(ip, new IPAddress(new byte[4]
        {
          (byte) 127,
          (byte) 0,
          (byte) 0,
          (byte) 1
        }));
      }

      public static IPAddress StringToIP(string ip, IPAddress defValue)
      {
        try
        {
          return IPAddress.Parse(ip);
        }
        catch (ArgumentNullException ex)
        {
          return defValue;
        }
        catch (FormatException ex)
        {
          return defValue;
        }
      }

      public static string StringToHexa(string cadena, string defValue)
      {
        if (string.IsNullOrEmpty(cadena) || cadena.Length % 3 != 0)
          return defValue;
        string empty = string.Empty;
        for (int startIndex = 0; startIndex < cadena.Length; startIndex += 3)
        {
          byte result;
          if (!byte.TryParse(cadena.Substring(startIndex, 3), out result))
            return defValue;
          empty += result.ToString("X2");
        }
        return empty;
      }

      public static bool ValidarExpresion(string cadena, string patron)
      {
        try
        {
          return Regex.Match(cadena, patron).Success;
        }
        catch (Exception ex)
        {
          throw new Exception(ex.Message);
        }
      }

      public static bool EsNumeroEntero(string cadena)
      {
        bool flag = true;
        foreach (char c in cadena)
        {
          if (!char.IsDigit(c))
          {
            flag = false;
            break;
          }
        }
        return flag;
      }

      public static List<int> StringToIntArray(string cadena)
      {
        List<int> intList = (List<int>) null;
        if (!string.IsNullOrEmpty(cadena))
        {
          intList = new List<int>();
          string str = cadena;
          char[] chArray = new char[1]{ ',' };
          foreach (string s in str.Split(chArray))
          {
            int result = 0;
            if (int.TryParse(s, out result))
              intList.Add(result);
          }
        }
        return intList;
      }

      public static string NormalizarCadena(string cadena, bool convertirMayusculas)
      {
        cadena = cadena.Normalize(NormalizationForm.FormD);
        cadena = new Regex("[^a-zA-Z0-9 ]").Replace(cadena, "");
        if (convertirMayusculas)
          cadena = cadena.ToUpper();
        return cadena;
      }
    }

    public static class RelojInterno
    {
      private static TimeSpan diferenciaHoraServidorPC = new TimeSpan();
      private static DateTime? fechaHoraServidorReal = new DateTime?();

      public static DateTime? ObtenerFechaHoraServidorBD()
      {
        return Utils.RelojInterno.ObtenerFechaHoraServidorBD(false);
      }

      public static DateTime? ObtenerFechaHoraServidorBD(bool defaultPCDateTime)
      {
        DateTime? nullable = Utils.ObtenerFechaHoraServidorBD();
        if (nullable.HasValue)
        {
          Utils.RelojInterno.fechaHoraServidorReal = new DateTime?(nullable.Value);
          Utils.RelojInterno.diferenciaHoraServidorPC = Utils.RelojInterno.fechaHoraServidorReal.Value.Subtract(DateTime.Now);
        }
        else if (defaultPCDateTime)
        {
          nullable = new DateTime?(DateTime.Now);
          Utils.RelojInterno.fechaHoraServidorReal = nullable;
        }
        return nullable;
      }

      public static DateTime? CalcularFechaHoraServidor()
      {
        return Utils.RelojInterno.CalcularFechaHoraServidor(true, true);
      }

      public static DateTime? CalcularFechaHoraServidor(
        bool accederBD,
        bool defaultPCDateTime)
      {
        DateTime? nullable = new DateTime?();
        nullable = !Utils.RelojInterno.fechaHoraServidorReal.HasValue ? Utils.RelojInterno.ObtenerFechaHoraServidorBD(defaultPCDateTime) : new DateTime?(DateTime.Now.Add(Utils.RelojInterno.diferenciaHoraServidorPC));
        return nullable;
      }
    }
  }
}
