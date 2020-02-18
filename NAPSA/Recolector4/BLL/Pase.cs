// Decompiled with JetBrains decompiler
// Type: DASYS.Recolector.BLL.Pase
// Assembly: BLL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 684D872A-58E1-4C16-9B83-6ABA379FCE9D
// Assembly location: C:\Program Files (x86)\NAPSA\Colector III\BLL.dll

using DASYS.DAL;
using DASYS.Framework;
using System;
using System.Collections.Generic;
using System.Data;

namespace DASYS.Recolector.BLL
{
  public class Pase
  {
    public static Pase UltimoPase;
    private int id;
    private byte numero;
    private DateTime fechaHora;
    private int numeroTiro;
    private byte velocidad;
    private byte sentidoGiro;

    public int Id
    {
      get
      {
        return this.id;
      }
      set
      {
        this.id = value;
      }
    }

    public byte NumeroGanador
    {
      get
      {
        return this.numero;
      }
      set
      {
        this.numero = value;
      }
    }

    public DateTime FechaHora
    {
      get
      {
        return this.fechaHora;
      }
      set
      {
        this.fechaHora = value;
      }
    }

    public int NumeroTiro
    {
      get
      {
        return this.numeroTiro;
      }
      set
      {
        this.numeroTiro = value;
      }
    }

    public byte Velocidad
    {
      get
      {
        return this.velocidad;
      }
      set
      {
        this.velocidad = value;
      }
    }

    public byte SentidoGiro
    {
      get
      {
        return this.sentidoGiro;
      }
      set
      {
        this.sentidoGiro = value;
      }
    }

    public static Pase LeerUltimoNumeroDesdeBase()
    {
      Pase pase = (Pase) null;
      try
      {
        DataSet dataSet = Common.oConexiones[0].DataSetExecuteReader(new QueryEngine()
        {
          QueryName = "paseSelectUltimo"
        });
        if (dataSet != null)
        {
          if (dataSet.Tables.Count > 0)
          {
            if (dataSet.Tables[0].Rows.Count == 1)
            {
              pase = new Pase();
              pase.NumeroGanador = Common.Datos.NullToByte(dataSet.Tables[0].Rows[0]["paseNumero"]);
              pase.Id = Common.Datos.NullToInt32(dataSet.Tables[0].Rows[0]["paseId"]);
              pase.NumeroTiro = Common.Datos.NullToInt32(dataSet.Tables[0].Rows[0]["paseTiroNumero"]);
              pase.fechaHora = Common.Datos.NullToDateTime(dataSet.Tables[0].Rows[0]["paseFechaHora"]);
              pase.velocidad = Common.Datos.NullToByte(dataSet.Tables[0].Rows[0]["paseVelocidad"]);
              pase.sentidoGiro = Common.Datos.NullToByte(dataSet.Tables[0].Rows[0]["paseSentidoGiro"]);
            }
          }
        }
      }
      catch (Exception ex)
      {
        throw;
      }
      return pase;
    }

    public static List<Pase> LeerUltimoNumerosDesdeBase()
    {
      List<Pase> paseList = (List<Pase>) null;
      try
      {
        DataSet dataSet = Common.oConexiones[0].DataSetExecuteReader(new QueryEngine()
        {
          QueryName = "paseSelectUltimos"
        });
        if (dataSet != null)
        {
          if (dataSet.Tables.Count > 0)
          {
            if (dataSet.Tables[0].Rows.Count > 0)
            {
              paseList = new List<Pase>();
              foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[0].Rows)
                paseList.Add(new Pase()
                {
                  NumeroGanador = Common.Datos.NullToByte(row["paseNumero"]),
                  Id = Common.Datos.NullToInt32(row["paseId"]),
                  numeroTiro = Common.Datos.NullToInt32(row["paseTiroNumero"]),
                  fechaHora = Common.Datos.NullToDateTime(row["paseFechaHora"]),
                  velocidad = Common.Datos.NullToByte(row["paseVelocidad"]),
                  sentidoGiro = Common.Datos.NullToByte(row["paseSentidoGiro"])
                });
            }
          }
        }
      }
      catch
      {
        throw;
      }
      return paseList;
    }

    public static bool EscribirEnBase(
      int tiroNumero,
      byte numero,
      byte velocidad,
      byte sentidoGiro,
      bool persistirIncidencia)
    {
      int num;
      try
      {
        QueryEngine query = new QueryEngine();
        query.QueryName = "paseInsert";
        query.AddNames("paseTiroNumero", "paseNumero", "paseVelocidad", "paseSentidoGiro");
        query.AddTypes(DbType.Int32, DbType.Byte, DbType.Byte, DbType.Byte);
        query.AddValues((object) tiroNumero, (object) numero, (object) velocidad, (object) sentidoGiro);
        num = Common.oConexiones[0].DbExecuteNonQuery(query);
        if (persistirIncidencia)
          Pase.PersistirIncidencia(0, numero);
      }
      catch
      {
        throw;
      }
      return num > 0;
    }

    public static bool PersistirIncidencia(int mesa, byte numero)
    {
      int num;
      try
      {
        QueryEngine query = new QueryEngine();
        query.QueryName = "estadisticanumeroSave";
        query.AddNames(nameof (mesa), nameof (numero));
        query.AddTypes(DbType.Int32, DbType.Byte);
        query.AddValues((object) mesa, (object) numero);
        num = Common.oConexiones[0].DbExecuteNonQuery(query);
      }
      catch
      {
        throw;
      }
      return num > 0;
    }

    public static ResultadoStatus.EstadoJuego ObtenerUltimoEstado()
    {
      ResultadoStatus.EstadoJuego statusEstado = ResultadoStatus.EstadoJuego.Indeterminado;
      try
      {
        //QueryEngine query = new QueryEngine();
        //query.QueryName = "estadoSelectUltimo";
        int num = (int)Common.oConexiones[0].DbExecuteScalar("estadoSelectUltimo");
        if (num < 7)
            statusEstado = (ResultadoStatus.EstadoJuego)num;
      }
      catch (Exception ex)
      {
          Common.Logger.Escribir($"Error ObtenerUltimoEstado(): {ex.Message} - {ex.StackTrace}", true);
      }
      return statusEstado;
    }

    public static bool GuardarUltimoEstado(ResultadoStatus.EstadoJuego estado)
    {
      int num;
      try
      {
        QueryEngine query = new QueryEngine();
        query.QueryName = "estadoUpdate";
        query.AddNames("valor");
        query.AddTypes(DbType.Int32);
        query.AddValues((object) (int) estado);
        num = Common.oConexiones[0].DbExecuteNonQuery(query);
      }
      catch
      {
        throw;
      }
      return num > 0;
    }
  }
}
