// Decompiled with JetBrains decompiler
// Type: DASYS.Framework.ParametroOE
// Assembly: Framework, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1B503700-E29D-4D7A-BD70-519F036595D0
// Assembly location: C:\Program Files (x86)\NAPSA\Colector III\Framework.dll

using DASYS.DAL;
using System;
using System.Collections.Generic;
using System.Data;

namespace DASYS.Framework
{
  public class ParametroOE
  {
    private string clave = string.Empty;
    private object valor;

    public ParametroOE()
    {
    }

    public ParametroOE(string clave, object valor)
    {
      this.clave = clave;
      this.valor = valor;
    }

    public string Clave
    {
      get
      {
        return this.clave;
      }
      set
      {
        this.clave = value;
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

    public static ParametrosOE Cargar()
    {
      ParametrosOE parametrosOe = new ParametrosOE();
      QueryEngine query = new QueryEngine();
      DataSet dataSet1 = (DataSet) null;
      try
      {
        parametrosOe = ParametroOE.ObtenerParametrosPorDefecto();
        if (Utils.oConexiones != null)
        {
          if (Utils.oConexiones.Count > 0)
          {
            query.QueryName = "OE_parametroSelect";
            query.AddNames("id");
            query.AddTypes(DbType.Int32);
            query.AddValues((object) DBNull.Value);
            DataSet dataSet2 = Utils.oConexiones["CP"].DataSetExecuteReader(query);
            if (dataSet2.Tables.Count > 0)
            {
              foreach (DataRow row in (InternalDataCollectionBase) dataSet2.Tables[0].Rows)
              {
                string clave = Utils.Datos.NullToString(row["parametroClave"]).ToUpper();
                ParametroOE parametroOe = parametrosOe.Find((Predicate<ParametroOE>) (p => p.Clave == clave));
                if (parametroOe != null)
                  parametroOe.Valor = (object) row["parametroValor"].ToString();
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        throw;
      }
      finally
      {
        dataSet1 = (DataSet) null;
      }
      return parametrosOe;
    }

    public static string Cargar(string clave)
    {
      string str = string.Empty;
      QueryEngine query = new QueryEngine();
      DataSet dataSet1 = (DataSet) null;
      try
      {
        if (Utils.oConexiones != null)
        {
          if (Utils.oConexiones.Count > 0)
          {
            query.QueryName = "OE_parametroSelectPorClave";
            query.AddNames(nameof (clave));
            query.AddTypes(DbType.String);
            query.AddValues((object) clave);
            DataSet dataSet2 = Utils.oConexiones["CP"].DataSetExecuteReader(query);
            if (dataSet2.Tables.Count > 0)
            {
              if (dataSet2.Tables[0].Rows.Count > 0)
                str = Utils.Datos.NullToString(dataSet2.Tables[0].Rows[0]["parametroValor"]).ToUpper();
            }
          }
        }
      }
      catch (Exception ex)
      {
        throw;
      }
      finally
      {
        dataSet1 = (DataSet) null;
      }
      return str;
    }

    internal static ParametrosOE ObtenerParametrosPorDefecto()
    {
      ParametrosOE parametrosOe = new ParametrosOE();
      parametrosOe.Add(new ParametroOE("APACTIVO", (object) 0));
      parametrosOe.Add(new ParametroOE("APINTERVALO", (object) 60000));
      parametrosOe.Add(new ParametroOE("APEXTENSION", (object) string.Empty));
      parametrosOe.Add(new ParametroOE("APLOG", (object) 1));
      parametrosOe.Add(new ParametroOE("APIDENTIFICADOR", (object) "LEGAJO"));
      parametrosOe.Add(new ParametroOE("APCARPETA", (object) string.Empty));
      parametrosOe.Add(new ParametroOE("APFORMATOFECHA", (object) "dd/MM/yyyy"));
      parametrosOe.Add(new ParametroOE("APSEPARADOR", (object) '\r'));
      parametrosOe.Add(new ParametroOE("AVINTERVALO", (object) 0));
      parametrosOe.Add(new ParametroOE("TMINTERVALO", (object) 60000));
      parametrosOe.Add(new ParametroOE("TMNOMBREBASEDATOS", (object) string.Empty));
      parametrosOe.Add(new ParametroOE("TMHISTORICOHORARIOINICIO", (object) "01:00"));
      parametrosOe.Add(new ParametroOE("TMHISTORICOHORARIOFINAL", (object) "05:00"));
      parametrosOe.Add(new ParametroOE("TMHISTORICOEVENTOSDIAS", (object) 30));
      parametrosOe.Add(new ParametroOE("TMHISTORICOACCESOSDIAS", (object) 30));
      parametrosOe.Add(new ParametroOE("TMHISTORICOPERMANENCIASDIAS", (object) 30));
      parametrosOe.Add(new ParametroOE("TMHISTORICOCANTIDADREGISTROS", (object) 500));
      parametrosOe.Add(new ParametroOE("OEACTUALIZACIONPENDIENTE", (object) 0));
      return parametrosOe;
    }

    public static bool Guardar(string clave, object valor)
    {
      QueryEngine query = new QueryEngine();
      int num = 0;
      try
      {
        if (!string.IsNullOrEmpty(clave))
        {
          query.QueryName = "OE_parametroSave";
          query.AddNames(nameof (clave), nameof (valor));
          query.AddTypes(DbType.String, DbType.String);
          query.AddValues((object) clave, (object) valor.ToString());
          num = Utils.oConexiones["CP"].DbExecuteNonQuery(query);
        }
      }
      catch
      {
        throw;
      }
      finally
      {
      }
      return num > 0;
    }

    public static bool Guardar(ParametrosOE parametrosOE)
    {
      bool flag = true;
      try
      {
        if (parametrosOE != null)
        {
          foreach (ParametroOE parametroOe in (List<ParametroOE>) parametrosOE)
            ParametroOE.Guardar(parametroOe.Clave, parametroOe.Valor);
        }
      }
      catch
      {
        throw;
      }
      return flag;
    }

    public enum TipoParametro
    {
      Global,
      Local,
    }
  }
}
