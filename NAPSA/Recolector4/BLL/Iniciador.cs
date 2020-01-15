// Decompiled with JetBrains decompiler
// Type: DASYS.Recolector.BLL.Iniciador
// Assembly: BLL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 684D872A-58E1-4C16-9B83-6ABA379FCE9D
// Assembly location: C:\Program Files (x86)\NAPSA\Colector III\BLL.dll

using DASYS.DAL;
using DASYS.Framework;
using System.Collections;
using System.Collections.Generic;

namespace DASYS.Recolector.BLL
{
  public static class Iniciador
  {
    public static bool Iniciar(string exePath)
    {
      bool flag = false;
      try
      {
        if (!exePath.EndsWith("\\"))
          exePath += "\\";
        try
        {
          Connection connection = new Connection(Common.ObtenerConexionDesdeXML("CP", exePath + "DASYS.NAPSA.Recolector4.config.xml"));
          Common.oConexiones = (List<Connection>) new Connections();
          Common.oConexiones.Add(connection);
          flag = Common.ProbarConexionBaseDatos(Common.oConexiones[0].Connectivity);
        }
        catch
        {
          Common.Logger.Escribir("La conexión a la base de datos ha fallado", true);
        }
        Hashtable hashtable = Archivos.XML.LeerXML("log", exePath + "DASYS.NAPSA.Recolector4.config.xml");
        if (hashtable != null && hashtable.Count > 0)
        {
          if (hashtable.Contains((object) "logActivado"))
            Common.Parametros.LogActivado = Common.Datos.StringToBoolean((object) hashtable[(object) "logActivado"].ToString());
          if (hashtable.Contains((object) "logRuta"))
            Common.Parametros.LogRuta = Common.Datos.NullToString(hashtable[(object) "logRuta"]);
          if (hashtable.Contains((object) "logArchivo"))
            Common.Parametros.LogArchivo = Common.Datos.NullToString(hashtable[(object) "logArchivo"]);
          if (hashtable.Contains((object) "logMaxKb"))
            Common.Parametros.LogMaxKb = Common.Datos.NullToInt32(hashtable[(object) "logMaxKb"]);
        }
        return true;
      }
      catch
      {
        throw;
      }
    }
  }
}
