// Decompiled with JetBrains decompiler
// Type: DASYS.Recolector.BLL.Common
// Assembly: BLL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 684D872A-58E1-4C16-9B83-6ABA379FCE9D
// Assembly location: C:\Program Files (x86)\NAPSA\Colector III\BLL.dll

using DASYS.DAL;
using DASYS.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace DASYS.Recolector.BLL
{
  public static class Common
  {
    public static bool EsTest = false;
    public static DataBaseType oTipoBaseDatos = DataBaseType.SQL;
    public static List<DASYS.DAL.Connection> oConexiones = new List<DASYS.DAL.Connection>();
    public static ArrayList NombresPantallas = new ArrayList();
    public static bool RefrescarContenedor = false;
    public static string Registry_Version = "1.0.0.0";
    public const bool leerDesdeBaseDeDatos = false;
    public const string Registry_Empresa = "DASYS";
    public const string Registry_Producto = "NAPSA";
    public const string RegExp_Enteros = "^([1-9]\\d*)$|^0$";
    public const string RegExp_Decimales4 = "^\\d+(?:\\\\d{0,4})?$";
    public const string RegExp_Byte = "^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$";
    public const string RegExp_EMail = "^[\\w-]+(?:\\.[\\w-]+)*@(?:[\\w-]+\\.)+[a-zA-Z]{2,7}$";
    public const string RegExp_IP = "^(?:(?:25[0-5]|2[0-4]\\d|[01]\\d\\d|\\d?\\d)(?(?=\\.?\\d)\\.)){4}$";
    public const string RegExp_Porcentaje4Decimales = "^100$|^(\\d{0,2}(\\\\d{1,4})? *?)$";
    public static Common.ParametrosAppConfig Parametros;

    public static bool ProbarConexionBaseDatos(Connectivity parametrosConexion)
    {
      bool flag = false;
      try
      {
        DASYS.DAL.Connection connection = new DASYS.DAL.Connection(parametrosConexion);
        if (connection.AbrirConexion())
        {
          connection.CerrarConexión();
          flag = true;
        }
      }
      catch
      {
        throw;
      }
      return flag;
    }

    public static bool ProbarConexionBaseDatos(DASYS.DAL.Connection conexion)
    {
      return Common.ProbarConexionBaseDatos(conexion.Connectivity);
    }

    public static Connections ObtenerConexionesDesdeXML()
    {
      return Common.ObtenerConexionesDesdeXML((string) null);
    }

    public static Connections ObtenerConexionesDesdeXML(string rutaArchivo)
    {
      Connections connections = (Connections) null;
      Connectivity connectivity = new Connectivity();
      List<Connectivity> connectivityList = !string.IsNullOrEmpty(rutaArchivo) ? (List<Connectivity>) Archivos.XML.LeerConexionXML(rutaArchivo) : (List<Connectivity>) Archivos.XML.LeerConexionXML();
      if (connectivityList != null && connectivityList.Count > 0)
      {
        connections = new Connections();
        foreach (Connectivity connInfo in connectivityList)
        {
          connInfo.Password = Seguridad.Standard.DesencriptarTripleDES(Seguridad.Standard.ConcatenarBytes(connInfo.Password));
          connections.Add(new DASYS.DAL.Connection(connInfo));
        }
      }
      return connections;
    }

    public static Connectivity ObtenerConexionDesdeXML(string nombreConexion)
    {
      try
      {
        return Common.ObtenerConexionDesdeXML(nombreConexion, (string) null);
      }
      catch
      {
        throw;
      }
    }

    public static Connectivity ObtenerConexionDesdeXML(
      string nombreConexion,
      string rutaArchivo)
    {
      DASYS.DAL.Connection connection = new DASYS.DAL.Connection();
      Connectivity connectivity1 = new Connectivity();
      List<Connectivity> connectivityList = !string.IsNullOrEmpty(rutaArchivo) ? (List<Connectivity>) Archivos.XML.LeerConexionXML(rutaArchivo) : (List<Connectivity>) Archivos.XML.LeerConexionXML();
      if (connectivityList != null && connectivityList.Count > 0)
      {
        foreach (Connectivity connectivity2 in connectivityList)
        {
          if (connectivity2.ConnectionName.ToUpper() == nombreConexion.ToUpper())
            connectivity1 = connectivity2;
        }
        connectivity1.Password = Seguridad.Standard.DesencriptarTripleDES(Seguridad.Standard.ConcatenarBytes(connectivity1.Password));
        connectivity1.ConnectionName = nombreConexion;
      }
      return connectivity1;
    }

    public static void GuardarConexionEnAppConfig(
      string nodoConexion,
      Connectivity oParametrosConexion)
    {
      Hashtable keyValue = new Hashtable();
      try
      {
        keyValue.Add((object) "DSNName", (object) oParametrosConexion.DSNName);
        keyValue.Add((object) "dataBaseType", (object) oParametrosConexion.DataBaseType.ToString());
        keyValue.Add((object) "server", (object) oParametrosConexion.ServerName);
        keyValue.Add((object) "dataBase", (object) oParametrosConexion.DataBaseName);
        keyValue.Add((object) "user", (object) oParametrosConexion.UserName);
        keyValue.Add((object) "password", (object) oParametrosConexion.Password);
        keyValue.Add((object) "timeout", (object) oParametrosConexion.Timeout);
        keyValue.Add((object) "persistSecurity", (object) oParametrosConexion.PersistSecurityInfo);
        keyValue.Add((object) "integratedSecurity", (object) oParametrosConexion.IntegratedSecurity);
        Archivos.XML.EscribirXML(nodoConexion.ToLower(), keyValue, "DASYS.NAPSA.Recolector.config.xml");
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message);
      }
    }

    public static DataView CargarLista(string nombreConexion, string nombrePantalla)
    {
      return Common.CargarLista(nombreConexion, nombrePantalla, string.Empty);
    }

    public static DataView CargarLista(
      string nombreConexion,
      string nombrePantalla,
      string filtroVista)
    {
      //DataSet dataSet = (DataSet) null;
      DataView dataView = (DataView) null;
      QueryEngine queryEngine = (QueryEngine) null;
      try
      {
        switch (nombrePantalla)
        {
          case "USUARIOS":
            queryEngine = new QueryEngine("OS_usuariosSelectAll");
            break;
          case "PERFILES":
            queryEngine = new QueryEngine("OS_perfilSelect");
            queryEngine.AddNames("perfilId");
            queryEngine.AddTypes(DbType.Int32);
            queryEngine.AddValues((object) DBNull.Value);
            break;
          case "TERMINALES":
            queryEngine = new QueryEngine("OS_terminalSelectPorId");
            queryEngine.AddNames("terminalId");
            queryEngine.AddTypes(DbType.Int32);
            queryEngine.AddValues((object) DBNull.Value);
            break;
          case "PERMISOSACCESO":
            queryEngine = new QueryEngine("OS_permisoAccesoSelect");
            queryEngine.AddNames("id");
            queryEngine.AddTypes(DbType.Int32);
            queryEngine.AddValues((object) DBNull.Value);
            break;
          case "TIPOTARJETA":
            queryEngine = new QueryEngine("OS_tipoTarjetaSelect");
            queryEngine.AddNames("id");
            queryEngine.AddTypes(DbType.Int32);
            queryEngine.AddValues((object) DBNull.Value);
            break;
          case "TIPOLECTORA":
            queryEngine = new QueryEngine("OS_tipoLectoraSelect");
            queryEngine.AddNames("id");
            queryEngine.AddTypes(DbType.Int32);
            queryEngine.AddValues((object) DBNull.Value);
            break;
          case "PERSONAS":
            queryEngine = new QueryEngine("OS_personaSelect");
            queryEngine.AddNames("personaId");
            queryEngine.AddTypes(DbType.Int32);
            queryEngine.AddValues((object) DBNull.Value);
            break;
          case "NODOS":
            queryEngine = new QueryEngine("OS_nodoSelect");
            queryEngine.AddNames("id");
            queryEngine.AddTypes(DbType.Int32);
            queryEngine.AddValues((object) DBNull.Value);
            break;
        }
        if (queryEngine != null)
        {
          dataView = ((DataSet) null).Tables[0].DefaultView;
          if (dataView.Table.Columns.Contains(filtroVista))
            dataView.Sort = filtroVista;
        }
      }
      catch (Exception ex)
      {
        dataView = (DataView) null;
        throw new Exception(ex.Message);
      }
      return dataView;
    }

    public static DateTime ObtenerFechaHoraServidorBD()
    {
      string empty = string.Empty;
      DateTime? nullable = new DateTime?();
      DateTime dateTime1 = DateTime.MinValue;
      try
      {
        QueryEngine query = new QueryEngine("OS_ObtenerFechaHora");
        DateTime? dateTime2 = Common.Datos.StringToDateTime(Common.Datos.NullToString(Common.oConexiones[0].DbExecuteScalar(query)));
        if (dateTime2.HasValue)
          dateTime1 = dateTime2.Value;
      }
      catch
      {
        dateTime1 = DateTime.Now;
      }
      return dateTime1;
    }

    public static void EjecutarProceso(string nombreProceso)
    {
      Common.EjecutarProceso(nombreProceso, (string) null);
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

    public struct ParametrosAppConfig
    {
      public int NumeroTerminal;
      public string LogArchivo;
      public string LogRuta;
      public bool LogActivado;
      public int LogMaxKb;
      public int SocketPort;
      public IPAddress ServerIP;

      public ParametrosAppConfig(
        int numeroTerminal,
        string logArchivo,
        string logRuta,
        bool logActivado,
        int logMaxKb,
        int socketPort,
        IPAddress serverIP)
      {
        this.NumeroTerminal = numeroTerminal;
        this.LogArchivo = logArchivo;
        this.LogRuta = logRuta;
        this.LogActivado = logActivado;
        this.LogMaxKb = logMaxKb;
        this.SocketPort = socketPort;
        this.ServerIP = serverIP;
      }
    }

    public static class Datos
    {
      public static char NullToChar(object value)
      {
        return Common.Datos.NullToChar(value, char.MinValue);
      }

      public static char NullToChar(object value, char defValue)
      {
        if (value == null)
          return defValue;
        return Convert.ToChar(value);
      }

      public static string NullToString(object value)
      {
        return Common.Datos.NullToString(value, string.Empty);
      }

      public static string NullToString(object value, string defValue)
      {
        if (value == null)
          return defValue;
        return Convert.ToString(value);
      }

      public static bool NullToBoolean(object value)
      {
        return Common.Datos.NullToBoolean(value, false);
      }

      public static bool NullToBoolean(object value, bool defValue)
      {
        if (value == null || value.ToString() == string.Empty)
          return defValue;
        return Convert.ToBoolean(value);
      }

      public static byte NullToByte(object value)
      {
        return Common.Datos.NullToByte(value, (byte) 0);
      }

      public static byte NullToByte(object value, byte defValue)
      {
        if (value == null || value.ToString().Trim() == string.Empty)
          return defValue;
        return Convert.ToByte(value);
      }

      public static short NullToInt16(object value)
      {
        return Common.Datos.NullToInt16(value, (short) 0);
      }

      public static short NullToInt16(object value, short defValue)
      {
        if (value == null || value.ToString() == string.Empty)
          return defValue;
        return Convert.ToInt16(value);
      }

      public static int NullToInt32(object value)
      {
        return Common.Datos.NullToInt32(value, 0);
      }

      public static int NullToInt32(object value, int defValue)
      {
        if (value == null || value.ToString() == string.Empty)
          return defValue;
        return Convert.ToInt32(value);
      }

      public static long NullToInt64(object value)
      {
        return Common.Datos.NullToInt64(value, 0L);
      }

      public static long NullToInt64(object value, long defValue)
      {
        if (value == null || value.ToString() == string.Empty)
          return defValue;
        return Convert.ToInt64(value);
      }

      public static float NullToFloat(object value)
      {
        return Common.Datos.NullToFloat(value, 0.0f);
      }

      public static float NullToFloat(object value, float defValue)
      {
        if (value == null || value.ToString() == string.Empty)
          return defValue;
        return Convert.ToSingle(value);
      }

      public static double NullToDouble(object value)
      {
        return Common.Datos.NullToDouble(value, 0.0);
      }

      public static double NullToDouble(object value, double defValue)
      {
        if (value == null || value.ToString() == string.Empty)
          return defValue;
        return Convert.ToDouble(value);
      }

      public static DateTime NullToDateTime(object value)
      {
        return Common.Datos.NullToDateTime(value, DateTime.MinValue);
      }

      public static DateTime NullToDateTime(object value, DateTime defValue)
      {
        if (value == null || value.ToString() == string.Empty)
          return defValue;
        return Convert.ToDateTime(value);
      }

      public static TimeSpan NullToTimeSpan(object value)
      {
        return Common.Datos.NullToTimeSpan(value, new TimeSpan(0, 0, 0));
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
        return Common.Datos.NullToDecimal(value, new Decimal(0));
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
        return Common.Datos.Int32ToNull(value, 0);
      }

      public static object Int32ToNull(int value, int defValue)
      {
        if (value == defValue)
          return (object) DBNull.Value;
        return (object) value;
      }

      public static bool StringToBoolean(object valor)
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

      public static IPAddress StringToIP(string ip)
      {
        return Common.Datos.StringToIP(ip, new IPAddress(new byte[4]
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
    }

    public class Item
    {
      private object nombre;
      private object valor;

      public Item(object nombre, object valor)
      {
        this.nombre = nombre;
        this.valor = valor;
      }

      public object Nombre
      {
        get
        {
          return this.nombre;
        }
        set
        {
          this.nombre = value;
        }
      }

      public object Valor
      {
        get
        {
          return this.valor;
        }
        set
        {
          this.valor = value;
        }
      }
    }

    public class Logger
    {
      public static void Escribir(string texto)
      {
        Common.Logger.Escribir(texto, false);
      }

      public static void Escribir(string texto, bool incluirFechaHora)
      {
        StringBuilder stringBuilder = new StringBuilder();
        string empty = string.Empty;
        StreamWriter streamWriter1 = (StreamWriter) null;
        try
        {
          string str = Common.Parametros.LogRuta + Common.Parametros.LogArchivo;
          StreamWriter streamWriter2 = new StreamWriter(str, true);
          if (incluirFechaHora)
            stringBuilder.Append(Common.ObtenerFechaHoraServidorBD().ToString("yyyy/MM/dd HH:mm:ss "));
          stringBuilder.Append(texto);
          streamWriter2.WriteLine((object) stringBuilder);
          streamWriter2.Flush();
          streamWriter2.Close();
          streamWriter2.Dispose();
          if (Common.Parametros.LogMaxKb <= 0)
            return;
          if (new FileInfo(str).Length <= (long) (Common.Parametros.LogMaxKb * 1024))
            return;
          try
          {
            System.IO.File.Move(str, Common.Parametros.LogRuta + string.Format("Log Backup {0}.log", (object) Common.ObtenerFechaHoraServidorBD().ToString("yyyyMMdd HHmmss")));
          }
          catch
          {
          }
        }
        catch
        {
        }
        finally
        {
          streamWriter1 = (StreamWriter) null;
        }
      }

      public static void EscribirLinea()
      {
        Common.Logger.Escribir((string) null, false);
      }
    }

    public static class Forms
    {
      public static Hashtable PropiedadValor = new Hashtable();

      public static bool PersistirPropiedadesFormularios(Hashtable keyValue)
      {
        bool flag = true;
        try
        {
          foreach (DictionaryEntry dictionaryEntry in keyValue)
          {
            if (!Seguridad.Registry.EscribirRegistro(Seguridad.Registry.RegistryKeys.HKEY_LOCAL_MACHINE, "settings", dictionaryEntry.Key.ToString(), dictionaryEntry.Value))
              flag = false;
          }
        }
        catch
        {
          throw;
        }
        return flag;
      }

      public static Hashtable CargarPropiedadesFormularios(Hashtable keyValue)
      {
        Hashtable hashtable = new Hashtable();
        try
        {
          foreach (DictionaryEntry dictionaryEntry in keyValue)
          {
            object obj = (object) Common.Datos.NullToString(Seguridad.Registry.LeerRegistro(Seguridad.Registry.RegistryKeys.HKEY_LOCAL_MACHINE, "settings", dictionaryEntry.Key.ToString()));
            hashtable[dictionaryEntry.Key] = obj;
          }
        }
        catch
        {
          throw;
        }
        return hashtable;
      }
    }

    public static class ComparadorObjetos<T>
    {
      public static int Comparar(T x, T y)
      {
        int num = 0;
        try
        {
          if ((object) x == null && (object) y == null)
            num = 0;
          else if ((object) x == null && (object) y != null || (object) x != null && (object) y == null)
          {
            num = -1;
          }
          else
          {
            Type type = typeof (T);
            PropertyInfo[] properties = type.GetProperties();
            FieldInfo[] fields = type.GetFields();
            foreach (PropertyInfo propertyInfo in properties)
            {
              IComparable comparable = propertyInfo.GetValue((object) x, (object[]) null) as IComparable;
              if (comparable != null)
              {
                object obj = propertyInfo.GetValue((object) y, (object[]) null);
                num = comparable.CompareTo(obj);
                if (num != 0)
                  return num;
              }
            }
            foreach (FieldInfo fieldInfo in fields)
            {
              IComparable comparable = fieldInfo.GetValue((object) x) as IComparable;
              if (comparable != null)
              {
                object obj = fieldInfo.GetValue((object) y);
                num = comparable.CompareTo(obj);
                if (num != 0)
                  return num;
              }
            }
          }
        }
        catch
        {
          throw;
        }
        return num;
      }
    }
  }
}
