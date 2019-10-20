// Decompiled with JetBrains decompiler
// Type: DASYS.Framework.Seguridad
// Assembly: Framework, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1B503700-E29D-4D7A-BD70-519F036595D0
// Assembly location: C:\Program Files (x86)\NAPSA\Colector III\Framework.dll

using Microsoft.Win32;
using System;
using System.Collections;
using System.IO;
using System.Management;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading;

namespace DASYS.Framework
{
  public class Seguridad
  {
    public class Activacion
    {
      public static ArrayList ObtenerPermisos()
      {
        ArrayList arrayList = new ArrayList();
        Thread.GetDomain().SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);
        WindowsPrincipal currentPrincipal = (WindowsPrincipal) Thread.CurrentPrincipal;
        foreach (object obj in Enum.GetValues(typeof (WindowsBuiltInRole)))
        {
          try
          {
            if (currentPrincipal.IsInRole((WindowsBuiltInRole) obj))
              arrayList.Add((object) obj.ToString());
          }
          catch
          {
          }
        }
        return arrayList;
      }

      public static string GenerarLicencia(string cf)
      {
        return Seguridad.Activacion.GenerarLicencia(0, cf);
      }

      public static string GenerarLicencia(int diasDePrueba, string cf)
      {
        string empty1 = string.Empty;
        string empty2 = string.Empty;
        DateTime? nullable = Utils.RelojInterno.ObtenerFechaHoraServidorBD();
        string empty3 = string.Empty;
        try
        {
          string volumeSerial = Seguridad.InformacionMaquina.GetVolumeSerial(Utils.hardDriveLetter);
          if (string.IsNullOrEmpty(volumeSerial))
            throw new Exception("No es posible generar la licencia. Faltan parámetros.");
          if (!nullable.HasValue)
            throw new Exception("Error acceso a base de datos.");
          object[] objArray = new object[5]
          {
            (object) nullable.Value.ToString("yyyyMMddHHmmssfff"),
            (object) ' ',
            (object) volumeSerial,
            (object) ' ',
            (object) diasDePrueba.ToString("000")
          };
          foreach (byte num in Seguridad.Standard.EncriptarTripleDES(string.Concat(objArray)))
            empty2 += num.ToString("000");
          return Seguridad.Activacion.ofuscar(empty2, Utils.Datos.StringToByteArray(cf));
        }
        catch (Exception ex)
        {
          throw new Exception(ex.Message);
        }
      }

      public static string GenerarLicenciaBase(string cf)
      {
        string empty1 = string.Empty;
        string empty2 = string.Empty;
        DateTime? nullable = Utils.RelojInterno.ObtenerFechaHoraServidorBD(true);
        string empty3 = string.Empty;
        try
        {
          string volumeSerial = Seguridad.InformacionMaquina.GetVolumeSerial(Utils.hardDriveLetter);
          if (string.IsNullOrEmpty(volumeSerial))
            throw new Exception("No es posible generar la licencia. Faltan parámetros.");
          if (!nullable.HasValue)
            throw new Exception("Error acceso a base de datos.");
          foreach (byte num in Seguridad.Standard.EncriptarTripleDES(nullable.Value.ToString("yyyyMMddHHmmssfff") + (object) ' ' + volumeSerial))
            empty2 += num.ToString("000");
          return Seguridad.Activacion.ofuscar(empty2, Utils.Datos.StringToByteArray(cf));
        }
        catch
        {
          throw;
        }
      }

      public static string CompletarLicenciaBase(string licenciaBase, int dias, string cf)
      {
        string empty1 = string.Empty;
        string empty2 = string.Empty;
        string empty3 = string.Empty;
        try
        {
          foreach (byte num in Seguridad.Standard.EncriptarTripleDES(Seguridad.Activacion.obtenerLicenciaLimpia(licenciaBase, cf) + (object) ' ' + dias.ToString("000")))
            empty1 += num.ToString("000");
          return Seguridad.Activacion.ofuscar(empty1, Utils.Datos.StringToByteArray(cf));
        }
        catch
        {
          throw;
        }
      }

      public static string GenerarActivacion(string licencia, string cf)
      {
        string empty1 = string.Empty;
        string empty2 = string.Empty;
        string str = string.Empty;
        if (licencia != string.Empty)
        {
          try
          {
            if (string.IsNullOrEmpty(Seguridad.InformacionMaquina.GetVolumeSerial(Utils.hardDriveLetter)))
              throw new Exception("No es posible generar la licencia. Faltan parámetros.");
            str = Seguridad.Standard.EncriptarMD5(Seguridad.Activacion.obtenerLicienciaDesencriptada(licencia, cf));
          }
          catch
          {
            str = (string) null;
          }
        }
        return str;
      }

      public static DateTime? ObtenerFechaInicio(string licencia)
      {
        string empty = string.Empty;
        DateTime? nullable1 = new DateTime?();
        DateTime? nullable2;
        try
        {
          byte[] bytesEncriptados = new byte[licencia.Length / 3];
          for (int startIndex = 0; startIndex < licencia.Length; startIndex += 3)
            bytesEncriptados[startIndex / 3] = Convert.ToByte(licencia.Substring(startIndex, 3));
          string str = Seguridad.Standard.DesencriptarTripleDES(bytesEncriptados);
          nullable2 = new DateTime?(new DateTime(int.Parse(str.Substring(0, 4)), int.Parse(str.Substring(4, 2)), int.Parse(str.Substring(6, 2)), int.Parse(str.Substring(8, 2)), int.Parse(str.Substring(10, 2)), int.Parse(str.Substring(12, 2)), int.Parse(str.Substring(14, 3))));
        }
        catch
        {
          nullable2 = new DateTime?();
        }
        return nullable2;
      }

      public static int ObtenerDiasHabilitado(string licencia, string cf)
      {
        string empty = string.Empty;
        int num;
        try
        {
          licencia = Seguridad.Activacion.desofuscar(licencia, Utils.Datos.StringToByteArray(cf));
          byte[] bytesEncriptados = new byte[licencia.Length / 3];
          for (int startIndex = 0; startIndex < licencia.Length; startIndex += 3)
            bytesEncriptados[startIndex / 3] = Convert.ToByte(licencia.Substring(startIndex, 3));
          string str = Seguridad.Standard.DesencriptarTripleDES(bytesEncriptados);
          num = int.Parse(str.Substring(str.Length - 3, 3));
          if (num == 0)
            num = 36500;
        }
        catch
        {
          num = -1;
        }
        return num;
      }

      private static string obtenerLicienciaDesencriptada(string licencia, string cf)
      {
        string str1;
        try
        {
          str1 = Seguridad.Activacion.obtenerLicenciaLimpia(licencia, cf);
          if (str1 != null)
          {
            string str2 = str1.Substring(0, 17);
            string empty = string.Empty;
            foreach (char ch in str2)
              empty += Convert.ToByte(ch).ToString();
            str1 = empty + str1.Substring(17);
          }
        }
        catch
        {
          str1 = (string) null;
        }
        return str1;
      }

      private static string obtenerLicenciaLimpia(string licencia, string cf)
      {
        string empty = string.Empty;
        try
        {
          licencia = Seguridad.Activacion.desofuscar(licencia, Utils.Datos.StringToByteArray(cf));
          byte[] bytesEncriptados = new byte[licencia.Length / 3];
          for (int startIndex = 0; startIndex < licencia.Length; startIndex += 3)
            bytesEncriptados[startIndex / 3] = Convert.ToByte(licencia.Substring(startIndex, 3));
          return Seguridad.Standard.DesencriptarTripleDES(bytesEncriptados);
        }
        catch
        {
          return string.Empty;
        }
      }

      public static bool VerificarLicencia(string licencia, string activacion, string cf)
      {
        string empty1 = string.Empty;
        bool flag = false;
        string empty2 = string.Empty;
        try
        {
          string str1 = Seguridad.Activacion.GenerarActivacion(licencia, cf);
          if (!string.IsNullOrEmpty(str1))
          {
            if (str1 == activacion)
            {
              string volumeSerial = Seguridad.InformacionMaquina.GetVolumeSerial(Utils.hardDriveLetter);
              if (!string.IsNullOrEmpty(volumeSerial))
              {
                string empty3 = string.Empty;
                string str2 = Seguridad.Activacion.obtenerLicenciaLimpia(licencia, cf);
                str2.Substring(0, 17);
                str2.Substring(str2.Length - 3, 3);
                if (str2.Substring(18, str2.Length - 22) == volumeSerial)
                  flag = true;
              }
            }
          }
        }
        catch (Exception ex)
        {
          throw new Exception(ex.Message);
        }
        return flag;
      }

      private static string ofuscar(string licenciaEncriptada, byte[] clienteFinal)
      {
        string str = string.Empty;
        try
        {
          if (!string.IsNullOrEmpty(licenciaEncriptada))
          {
            int num1 = 0;
            if (clienteFinal != null)
              num1 = clienteFinal.Length;
            if (num1 > 0)
            {
              int index = 0;
              foreach (char ch in licenciaEncriptada)
              {
                if (num1 == index)
                  index = 0;
                byte num2 = byte.Parse(ch.ToString());
                byte num3 = 0;
                if (num2 < (byte) 9)
                {
                  byte num4 = Seguridad.Activacion.reducirByte(clienteFinal[index]);
                  num3 = num4 != (byte) 0 || num2 != (byte) 0 ? Seguridad.Activacion.reducirByte((byte) ((uint) num2 + (uint) num4)) : (byte) 9;
                }
                str += num3.ToString();
                ++index;
              }
            }
            else
              str = licenciaEncriptada;
          }
        }
        catch
        {
          throw;
        }
        return str;
      }

      private static string desofuscar(string licenciaOfuscada, byte[] clienteFinal)
      {
        string str = string.Empty;
        try
        {
          if (!string.IsNullOrEmpty(licenciaOfuscada))
          {
            int num1 = 0;
            if (clienteFinal != null)
              num1 = clienteFinal.Length;
            if (num1 > 0)
            {
              int index = 0;
              foreach (char ch in licenciaOfuscada)
              {
                if (num1 == index)
                  index = 0;
                byte num2 = Seguridad.Activacion.reducirByte(clienteFinal[index]);
                byte num3 = byte.Parse(ch.ToString());
                byte num4 = 0;
                switch (num3)
                {
                  case 0:
                    num4 = (byte) 9;
                    break;
                  case 9:
                    if (num2 == (byte) 0)
                    {
                      num4 = (byte) 0;
                      break;
                    }
                    goto default;
                  default:
                    int num5 = (int) num3 - (int) num2;
                    num4 = num5 < 0 ? (byte) (9 + num5) : (byte) num5;
                    break;
                }
                str += num4.ToString();
                ++index;
              }
            }
            else
              str = licenciaOfuscada;
          }
        }
        catch
        {
          throw;
        }
        return str;
      }

      private static byte reducirByte(byte byteToReduce)
      {
        string str = byteToReduce.ToString("000");
        byte byteToReduce1 = 0;
        foreach (char ch in str)
          byteToReduce1 += byte.Parse(ch.ToString());
        if (byteToReduce1 > (byte) 9)
          byteToReduce1 = Seguridad.Activacion.reducirByte(byteToReduce1);
        return byteToReduce1;
      }

      public static bool GuardarValidezRegistry(string licencia, string cf)
      {
        return Seguridad.Activacion.GuardarValidezRegistry(licencia, Utils.RelojInterno.ObtenerFechaHoraServidorBD(), cf);
      }

      public static bool GuardarValidezRegistry(
        string licencia,
        DateTime? fechaHoraActuales,
        string cf)
      {
        DateTime? inicio = Seguridad.Activacion.ObtenerFechaInicio(licencia);
        int num = Seguridad.Activacion.ObtenerDiasHabilitado(licencia, cf);
        DateTime? fin = new DateTime?(inicio.Value.AddDays((double) num));
        return Seguridad.Activacion.GuardarValidezRegistry(inicio, fin, fechaHoraActuales);
      }

      public static bool GuardarValidezRegistry(DateTime? inicio, DateTime? fin)
      {
        return Seguridad.Activacion.GuardarValidezRegistry(inicio, fin, Utils.RelojInterno.ObtenerFechaHoraServidorBD());
      }

      public static bool GuardarValidezRegistry(
        DateTime? inicio,
        DateTime? fin,
        DateTime? fechaHoraActuales)
      {
        try
        {
          if (!fechaHoraActuales.HasValue)
            throw new Exception("Error obtención fecha y hora.");
          DateTime? nullable = new DateTime?(new DateTime(fechaHoraActuales.Value.Year, fechaHoraActuales.Value.Month, fechaHoraActuales.Value.Day, fechaHoraActuales.Value.Hour, fechaHoraActuales.Value.Minute, fechaHoraActuales.Value.Second, fechaHoraActuales.Value.Millisecond));
          bool flag1 = Seguridad.Registry.EscribirRegistro(Seguridad.Registry.RegistryKeys.HKEY_LOCAL_MACHINE, nameof (Activacion), "Inicio", (object) Seguridad.Standard.EncriptarTripleDES(inicio.Value.ToFileTimeUtc().ToString()));
          bool flag2 = Seguridad.Registry.EscribirRegistro(Seguridad.Registry.RegistryKeys.HKEY_LOCAL_MACHINE, nameof (Activacion), "Acceso", (object) Seguridad.Standard.EncriptarTripleDES(nullable.Value.ToFileTimeUtc().ToString()));
          bool flag3 = Seguridad.Registry.EscribirRegistro(Seguridad.Registry.RegistryKeys.HKEY_LOCAL_MACHINE, nameof (Activacion), "Fin", (object) Seguridad.Standard.EncriptarTripleDES(fin.Value.ToFileTimeUtc().ToString()));
          return flag1 && flag2 && flag3;
        }
        catch
        {
          throw;
        }
      }

      public static bool VerificarValidezRegistry()
      {
        return Seguridad.Activacion.VerificarValidezRegistry(Utils.RelojInterno.ObtenerFechaHoraServidorBD());
      }

      public static bool VerificarValidezRegistry(DateTime? fechaHoraActuales)
      {
        bool flag;
        try
        {
          DateTime? inicio = Seguridad.Activacion.obtenerFechaHoraRegistry("Fin");
          DateTime? ultimoAcceso = Seguridad.Activacion.obtenerFechaHoraRegistry("Acceso");
          DateTime? fin = Seguridad.Activacion.obtenerFechaHoraRegistry("Fin");
          if (!fechaHoraActuales.HasValue)
            throw new Exception("Error Fecha y hora.");
          DateTime? nullable1 = new DateTime?(new DateTime(fechaHoraActuales.Value.Year, fechaHoraActuales.Value.Month, fechaHoraActuales.Value.Day, fechaHoraActuales.Value.Hour, fechaHoraActuales.Value.Minute, fechaHoraActuales.Value.Second, fechaHoraActuales.Value.Millisecond));
          flag = Seguridad.Activacion.validarFechas(inicio, ultimoAcceso, fin, fechaHoraActuales);
          DateTime? nullable2 = nullable1;
          DateTime? nullable3 = ultimoAcceso;
          if ((nullable2.HasValue & nullable3.HasValue ? (nullable2.GetValueOrDefault() > nullable3.GetValueOrDefault() ? 1 : 0) : 0) != 0)
            Seguridad.Registry.EscribirRegistro(Seguridad.Registry.RegistryKeys.HKEY_LOCAL_MACHINE, nameof (Activacion), "Acceso", (object) Seguridad.Standard.EncriptarTripleDES(nullable1.Value.ToFileTimeUtc().ToString()));
        }
        catch
        {
          throw;
        }
        return flag;
      }

      private static bool validarFechas(
        DateTime? inicio,
        DateTime? ultimoAcceso,
        DateTime? fin,
        DateTime? fechaHoraActuales)
      {
        bool flag1 = false;
        bool flag2;
        try
        {
          if (!inicio.HasValue || !ultimoAcceso.HasValue || !fin.HasValue)
            flag2 = false;
          else if (fin.Value.Year > 2100)
          {
            flag2 = true;
          }
          else
          {
            DateTime? nullable1 = fechaHoraActuales;
            DateTime? nullable2 = ultimoAcceso;
            if ((nullable1.HasValue & nullable2.HasValue ? (nullable1.GetValueOrDefault() <= nullable2.GetValueOrDefault() ? 1 : 0) : 0) == 0)
            {
              DateTime? nullable3 = fechaHoraActuales;
              DateTime? nullable4 = fin;
              if ((nullable3.HasValue & nullable4.HasValue ? (nullable3.GetValueOrDefault() >= nullable4.GetValueOrDefault() ? 1 : 0) : 0) == 0)
              {
                flag2 = true;
                goto label_10;
              }
            }
            flag2 = false;
          }
        }
        catch
        {
          flag1 = false;
          throw;
        }
label_10:
        return flag2;
      }

      private static DateTime? obtenerFechaHoraRegistry(string clave)
      {
        DateTime? nullable1 = new DateTime?();
        DateTime? nullable2;
        try
        {
          nullable2 = new DateTime?(DateTime.FromFileTimeUtc(long.Parse(Seguridad.Standard.DesencriptarTripleDES(Seguridad.Registry.LeerRegistro(Seguridad.Registry.RegistryKeys.HKEY_LOCAL_MACHINE, nameof (Activacion), clave) as byte[]))));
        }
        catch
        {
          nullable2 = new DateTime?();
        }
        return nullable2;
      }
    }

    public class Standard
    {
      private const string myKey = "7]j_3E!L?=.S";
      private const string myIV = "#@5k%&Dm+;Jq";

      public static string EncriptarMD5(string texto)
      {
        byte[] hash = new MD5CryptoServiceProvider().ComputeHash(new ASCIIEncoding().GetBytes(texto));
        return Convert.ToBase64String(hash, 0, hash.Length);
      }

      public static byte[] EncriptarTripleDES(string texto)
      {
        return Seguridad.Standard.EncriptarTripleDES(texto, string.Empty, string.Empty);
      }

      public static byte[] EncriptarTripleDES(string texto, string clave, string vector)
      {
        UnicodeEncoding unicodeEncoding = new UnicodeEncoding();
        byte[] bytes1;
        byte[] bytes2;
        if (clave == string.Empty || vector == string.Empty)
        {
          char[] charArray1 = "7]j_3E!L?=.S".ToCharArray();
          char[] charArray2 = "#@5k%&Dm+;Jq".ToCharArray();
          bytes1 = unicodeEncoding.GetBytes(charArray1);
          bytes2 = unicodeEncoding.GetBytes(charArray2);
        }
        else
        {
          char[] charArray1 = clave.ToCharArray();
          char[] charArray2 = vector.ToCharArray();
          bytes1 = unicodeEncoding.GetBytes(charArray1);
          bytes2 = unicodeEncoding.GetBytes(charArray2);
          if (bytes1.Length != 24 || bytes2.Length != 24)
            throw new Exception("La clave y el vector de inicialización deben ser de 24 bytes cada uno.");
        }
        return Seguridad.Standard.EncriptarTripleDES(texto, bytes1, bytes2);
      }

      public static byte[] EncriptarTripleDES(string texto, byte[] key, byte[] IV)
      {
        UnicodeEncoding unicodeEncoding = new UnicodeEncoding();
        TripleDESCryptoServiceProvider cryptoServiceProvider = new TripleDESCryptoServiceProvider();
        try
        {
          byte[] bytes = unicodeEncoding.GetBytes(texto);
          ICryptoTransform encryptor = cryptoServiceProvider.CreateEncryptor(key, IV);
          MemoryStream memoryStream = new MemoryStream();
          CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, encryptor, CryptoStreamMode.Write);
          cryptoStream.Write(bytes, 0, bytes.Length);
          cryptoStream.FlushFinalBlock();
          memoryStream.Position = 0L;
          byte[] buffer = new byte[memoryStream.Length];
          memoryStream.Read(buffer, 0, (int) memoryStream.Length);
          cryptoStream.Close();
          return buffer;
        }
        catch (Exception ex)
        {
          throw new Exception(ex.Message);
        }
      }

      public static string DesencriptarTripleDES(byte[] bytesEncriptados)
      {
        return Utils.Datos.NullToString((object) Seguridad.Standard.DesencriptarTripleDES(bytesEncriptados, string.Empty, string.Empty));
      }

      public static string DesencriptarTripleDES(
        byte[] bytesEncriptados,
        string clave,
        string vector)
      {
        UnicodeEncoding unicodeEncoding = new UnicodeEncoding();
        byte[] bytes1;
        byte[] bytes2;
        if (clave == string.Empty || vector == string.Empty)
        {
          char[] charArray1 = "7]j_3E!L?=.S".ToCharArray();
          char[] charArray2 = "#@5k%&Dm+;Jq".ToCharArray();
          bytes1 = unicodeEncoding.GetBytes(charArray1);
          bytes2 = unicodeEncoding.GetBytes(charArray2);
        }
        else
        {
          char[] charArray1 = clave.ToCharArray();
          char[] charArray2 = vector.ToCharArray();
          bytes1 = unicodeEncoding.GetBytes(charArray1);
          bytes2 = unicodeEncoding.GetBytes(charArray2);
          if (bytes1.Length != 24 || bytes2.Length != 24)
            throw new Exception("La clave y el vector de inicialización deben ser de 24 bytes cada uno.");
        }
        return Seguridad.Standard.DesencriptarTripleDES(bytesEncriptados, bytes1, bytes2);
      }

      public static string DesencriptarTripleDES(byte[] bytesEncriptados, byte[] key, byte[] iv)
      {
        try
        {
          if (bytesEncriptados != null)
          {
            UnicodeEncoding unicodeEncoding = new UnicodeEncoding();
            ICryptoTransform decryptor = new TripleDESCryptoServiceProvider().CreateDecryptor(key, iv);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, decryptor, CryptoStreamMode.Write);
            cryptoStream.Write(bytesEncriptados, 0, bytesEncriptados.Length);
            cryptoStream.FlushFinalBlock();
            memoryStream.Position = 0L;
            byte[] numArray = new byte[memoryStream.Length];
            memoryStream.Read(numArray, 0, (int) memoryStream.Length);
            cryptoStream.Close();
            return new UnicodeEncoding().GetString(numArray);
          }
        }
        catch
        {
          return (string) null;
        }
        return (string) null;
      }

      public static byte[] ConcatenarBytes(string cadena)
      {
        byte[] numArray = (byte[]) null;
        try
        {
          if (!string.IsNullOrEmpty(cadena))
          {
            if (cadena.Length % 2 == 0)
            {
              numArray = new byte[cadena.Length / 2];
              for (int startIndex = 0; startIndex < cadena.Length; startIndex += 2)
                numArray[startIndex / 2] = Convert.ToByte(cadena.Substring(startIndex, 2), 16);
            }
          }
        }
        catch (Exception ex)
        {
          throw new Exception(ex.Message);
        }
        return numArray;
      }

      public static string ConcatenarBytes(byte[] bytes)
      {
        string str = (string) null;
        try
        {
          if (bytes.Length > 0)
          {
            foreach (byte num in bytes)
              str += string.Format("{0:x2}", (object) num).ToUpper();
          }
        }
        catch (Exception ex)
        {
          throw new Exception(ex.Message);
        }
        return str;
      }
    }

    public class Registry
    {
      public static bool EscribirRegistro(
        Seguridad.Registry.RegistryKeys HKEY,
        string carpeta,
        string clave,
        object valor)
      {
        return Seguridad.Registry.EscribirRegistro(HKEY, "DASYS", "Open Shields", carpeta, clave, valor);
      }

      public static bool EscribirRegistro(
        Seguridad.Registry.RegistryKeys HKEY,
        string nombreEntidad,
        string nombreProducto,
        string carpeta,
        string clave,
        object valor)
      {
        try
        {
          switch (HKEY)
          {
            case Seguridad.Registry.RegistryKeys.HKEY_LOCAL_MACHINE:
              RegistryKey registryKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software", true);
              RegistryKey subKey1 = registryKey.CreateSubKey(nombreEntidad);
              RegistryKey subKey2 = subKey1.CreateSubKey(nombreProducto);
              RegistryKey subKey3 = subKey2.CreateSubKey(carpeta);
              subKey3.SetValue(clave, valor);
              subKey3.Close();
              subKey2.Close();
              subKey1.Close();
              registryKey.Close();
              break;
          }
          return true;
        }
        catch (Exception ex)
        {
          throw new Exception(ex.Message);
        }
      }

      public static object LeerRegistro(
        Seguridad.Registry.RegistryKeys HKEY,
        string carpeta,
        string clave)
      {
        return Seguridad.Registry.LeerRegistro(HKEY, "DASYS", "Open Shields", carpeta, clave);
      }

      public static object LeerRegistro(
        Seguridad.Registry.RegistryKeys HKEY,
        string nombreEntidad,
        string nombreProducto,
        string carpeta,
        string clave)
      {
        object obj = (object) null;
        try
        {
          switch (HKEY)
          {
            case Seguridad.Registry.RegistryKeys.HKEY_LOCAL_MACHINE:
              string name = "Software\\" + nombreEntidad + "\\" + nombreProducto + "\\" + carpeta;
              RegistryKey registryKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(name, true);
              if (registryKey != null)
              {
                obj = registryKey.GetValue(clave);
                break;
              }
              break;
          }
        }
        catch (Exception ex)
        {
          throw new Exception(ex.Message);
        }
        return obj;
      }

      public enum RegistryKeys
      {
        HKEY_LOCAL_MACHINE,
        HKEY_CURRENT_USER,
        HKEY_CLASSES_ROOT,
        HKEY_USERS,
        HKEY_CURRENT_CONFIG,
      }
    }

    public static class InformacionMaquina
    {
      public static string GetVolumeSerial(string strDriveLetter)
      {
        if (strDriveLetter == "" || strDriveLetter == null)
          strDriveLetter = "C";
        ManagementObject managementObject = new ManagementObject("win32_logicaldisk.deviceid=\"" + strDriveLetter + ":\"");
        managementObject.Get();
        return managementObject["VolumeSerialNumber"].ToString();
      }

      public static string GetMACAddress()
      {
        ManagementObjectCollection instances = new ManagementClass("Win32_NetworkAdapterConfiguration").GetInstances();
        string empty = string.Empty;
        foreach (ManagementObject managementObject in instances)
        {
          if (empty == string.Empty && (bool) managementObject["IPEnabled"])
            empty = managementObject["MacAddress"].ToString();
          managementObject.Dispose();
        }
        return empty.Replace(":", "");
      }

      public static string GetCPUId()
      {
        string empty1 = string.Empty;
        string empty2 = string.Empty;
        foreach (ManagementObject instance in new ManagementClass("Win32_Processor").GetInstances())
        {
          if (empty1 == string.Empty)
            empty1 = instance.Properties["ProcessorId"].Value.ToString();
        }
        return empty1;
      }
    }
  }
}
