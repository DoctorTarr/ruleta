// Decompiled with JetBrains decompiler
// Type: DASYS.Framework.Parametro
// Assembly: Framework, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1B503700-E29D-4D7A-BD70-519F036595D0
// Assembly location: C:\Program Files (x86)\NAPSA\Colector III\Framework.dll

using DASYS.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace DASYS.Framework
{
  public class Parametro
  {
    private string nombre = string.Empty;
    private DateTime? ultimaLecturaDesdeBase = new DateTime?();
    private object valor;
    private Parametro.TipoParametro alcance;

    public Parametro()
    {
    }

    public Parametro(string nombre, object valor)
    {
      this.nombre = nombre;
      this.valor = valor;
    }

    public Parametro(
      string nombre,
      object valor,
      Parametro.TipoParametro alcance,
      DateTime? ultimaLecturaDesdeBase)
    {
      this.nombre = nombre;
      this.valor = valor;
      this.alcance = alcance;
      this.ultimaLecturaDesdeBase = ultimaLecturaDesdeBase;
    }

    public string Nombre
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

    public Parametro.TipoParametro Alcance
    {
      get
      {
        return this.alcance;
      }
      set
      {
        this.alcance = value;
      }
    }

    public DateTime? UltimaLecturaDesdeBase
    {
      get
      {
        return this.ultimaLecturaDesdeBase;
      }
      set
      {
        this.ultimaLecturaDesdeBase = value;
      }
    }

    public static Parametros CargarParametros(int terminalId)
    {
      Parametros parametros = new Parametros();
      QueryEngine query = new QueryEngine();
      DataSet dataSet1 = (DataSet) null;
      try
      {
        parametros = Parametro.ObtenerParametrosPorDefecto();
        if (Utils.oConexiones != null)
        {
          if (Utils.oConexiones.Count > 0)
          {
            query.QueryName = "OS_parametrosSelectAll";
            query.AddNames(nameof (terminalId));
            query.AddTypes(DbType.Int32);
            query.AddValues((object) terminalId);
            DataSet dataSet2 = Utils.oConexiones["CP"].DataSetExecuteReader(query);
            if (dataSet2.Tables.Count > 0)
            {
              foreach (DataRow row in (InternalDataCollectionBase) dataSet2.Tables[0].Rows)
              {
                string clave = Utils.Datos.NullToString(row["parametroClave"]).ToUpper();
                Parametro parametro = parametros.Find((Predicate<Parametro>) (p => p.Nombre == clave));
                if (parametro != null)
                {
                  parametro.UltimaLecturaDesdeBase = Utils.RelojInterno.CalcularFechaHoraServidor();
                  if (row["parametroTerminalValor"] != DBNull.Value)
                  {
                    parametro.Alcance = Parametro.TipoParametro.Local;
                    parametro.Valor = (object) row["parametroTerminalValor"].ToString().ToUpper();
                  }
                  else
                  {
                    parametro.Alcance = Parametro.TipoParametro.Global;
                    parametro.Valor = (object) row["parametroValor"].ToString().ToUpper();
                  }
                }
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
      return parametros;
    }

    public static Parametro CargarParametro(
      int terminalId,
      string clave,
      List<Parametro> parametros)
    {
      QueryEngine query = new QueryEngine();
      DataSet dataSet1 = (DataSet) null;
      Parametro parametro = (Parametro) null;
      try
      {
        parametro = parametros.Find((Predicate<Parametro>) (p => p.Nombre == clave.ToUpper()));
        if (parametro != null)
        {
          query.QueryName = "OS_parametroSelectPorClave";
          query.AddNames(nameof (terminalId), "parametroClave");
          query.AddTypes(DbType.Int32, DbType.String);
          query.AddValues((object) terminalId, (object) clave);
          DataSet dataSet2 = Utils.oConexiones["CP"].DataSetExecuteReader(query);
          if (dataSet2.Tables.Count > 0)
          {
            foreach (DataRow row in (InternalDataCollectionBase) dataSet2.Tables[0].Rows)
            {
              parametro.UltimaLecturaDesdeBase = Utils.RelojInterno.CalcularFechaHoraServidor();
              if (row["parametroTerminalValor"] != DBNull.Value)
              {
                parametro.Alcance = Parametro.TipoParametro.Local;
                parametro.Valor = (object) row["parametroTerminalValor"].ToString().ToUpper();
              }
              else
              {
                parametro.Alcance = Parametro.TipoParametro.Global;
                parametro.Valor = (object) row["parametroValor"].ToString().ToUpper();
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message);
      }
      finally
      {
        dataSet1 = (DataSet) null;
      }
      return parametro;
    }

    public static Parametro ObtenerParametro(string clave, List<Parametro> parametros)
    {
      return parametros.Find((Predicate<Parametro>) (p => p.Nombre == clave.ToUpper()));
    }

    public static Parametros ObtenerParametrosPorDefecto()
    {
      Parametros parametros = new Parametros();
      parametros.Add(new Parametro("PRIMERDIASEMANA", (object) 1, Parametro.TipoParametro.Global, new DateTime?()));
      parametros.Add(new Parametro("FOTOSRUTA", (object) "C:\\", Parametro.TipoParametro.Global, new DateTime?()));
      parametros.Add(new Parametro("FOTOSNOMBREARCHIVO", (object) "DOC", Parametro.TipoParametro.Global, new DateTime?()));
      parametros.Add(new Parametro("CCTOLERANCIA", (object) 5, Parametro.TipoParametro.Global, new DateTime?()));
      parametros.Add(new Parametro("CCPERSISTENCIA", (object) 100, Parametro.TipoParametro.Global, new DateTime?()));
      parametros.Add(new Parametro("COLORGENERALGRILLAA", (object) -1, Parametro.TipoParametro.Global, new DateTime?()));
      parametros.Add(new Parametro("COLORGENERALGRILLAB", (object) -1828, Parametro.TipoParametro.Global, new DateTime?()));
      parametros.Add(new Parametro("COLORGENERALPANELSUPERIOR", (object) SystemColors.Window.ToArgb(), Parametro.TipoParametro.Global, new DateTime?()));
      parametros.Add(new Parametro("COLORGENERALPANELINFERIOR", (object) SystemColors.Window.ToArgb(), Parametro.TipoParametro.Global, new DateTime?()));
      parametros.Add(new Parametro("COLORGENERALMENUFONDO", (object) SystemColors.Window.ToArgb(), Parametro.TipoParametro.Global, new DateTime?()));
      parametros.Add(new Parametro("COLORGENERALMENUSELECCIONADO", (object) SystemColors.ActiveCaption.ToArgb(), Parametro.TipoParametro.Global, new DateTime?()));
      parametros.Add(new Parametro("COLORGENERALSUBMENUFONDO", (object) SystemColors.Window.ToArgb().ToString(), Parametro.TipoParametro.Global, new DateTime?()));
      parametros.Add(new Parametro("COLORGENERALSUBMENUSELECCIONADO", (object) SystemColors.ActiveCaption.ToArgb(), Parametro.TipoParametro.Global, new DateTime?()));
      parametros.Add(new Parametro("COLORGENERALPOPUPFONDO", (object) SystemColors.Window.ToArgb().ToString(), Parametro.TipoParametro.Global, new DateTime?()));
      parametros.Add(new Parametro("COLORGENERALPOPUPFRENTE", (object) SystemColors.WindowText.ToArgb(), Parametro.TipoParametro.Global, new DateTime?()));
      parametros.Add(new Parametro("COLORGENERALPOPUPINFOFONDO", (object) SystemColors.Window.ToArgb().ToString(), Parametro.TipoParametro.Global, new DateTime?()));
      parametros.Add(new Parametro("COLORGENERALPOPUPINFOFRENTE", (object) SystemColors.WindowText.ToArgb(), Parametro.TipoParametro.Global, new DateTime?()));
      parametros.Add(new Parametro("COLORGENERALPOPUPOKFONDO", (object) Color.Green.ToArgb().ToString(), Parametro.TipoParametro.Global, new DateTime?()));
      parametros.Add(new Parametro("COLORGENERALPOPUPOKFRENTE", (object) Color.White.ToArgb(), Parametro.TipoParametro.Global, new DateTime?()));
      parametros.Add(new Parametro("COLORGENERALPOPUPERRFONDO", (object) Color.Red.ToArgb().ToString(), Parametro.TipoParametro.Global, new DateTime?()));
      parametros.Add(new Parametro("COLORGENERALPOPUPERRFRENTE", (object) Color.White.ToArgb(), Parametro.TipoParametro.Global, new DateTime?()));
      parametros.Add(new Parametro("COLORGENERALPOPUPADVFONDO", (object) Color.Yellow.ToArgb().ToString(), Parametro.TipoParametro.Global, new DateTime?()));
      parametros.Add(new Parametro("COLORGENERALPOPUPADVFRENTE", (object) Color.Black.ToArgb(), Parametro.TipoParametro.Global, new DateTime?()));
      parametros.Add(new Parametro("COLORGENERALPADREFRENTE", (object) SystemColors.WindowText.ToArgb(), Parametro.TipoParametro.Global, new DateTime?()));
      parametros.Add(new Parametro("COLORGENERALPADREFONDO", (object) SystemColors.Window.ToArgb(), Parametro.TipoParametro.Global, new DateTime?()));
      parametros.Add(new Parametro("COLORGENERALSTATUSFRENTE", (object) SystemColors.WindowText.ToArgb(), Parametro.TipoParametro.Global, new DateTime?()));
      parametros.Add(new Parametro("COLORGENERALSTATUSFONDO", (object) SystemColors.Window.ToArgb(), Parametro.TipoParametro.Global, new DateTime?()));
      parametros.Add(new Parametro("COLORGENERALACCESOFONDO", (object) SystemColors.Window.ToArgb(), Parametro.TipoParametro.Global, new DateTime?()));
      parametros.Add(new Parametro("COLORGENERALACCESOFRENTE", (object) SystemColors.WindowText.ToArgb(), Parametro.TipoParametro.Global, new DateTime?()));
      parametros.Add(new Parametro("COLORLOCALHABILITADO", (object) 1, Parametro.TipoParametro.Global, new DateTime?()));
      parametros.Add(new Parametro("DOCUMENTOUNICO", (object) 1, Parametro.TipoParametro.Global, new DateTime?()));
      parametros.Add(new Parametro("DOCUMENTOUNICOADVERTENCIA", (object) 1, Parametro.TipoParametro.Global, new DateTime?()));
      parametros.Add(new Parametro("CARACTERESMINIMOSBUSQUEDA", (object) 3, Parametro.TipoParametro.Global, new DateTime?()));
      parametros.Add(new Parametro("CONTRASEÑALONGITUDMINIMA", (object) 3, Parametro.TipoParametro.Global, new DateTime?()));
      parametros.Add(new Parametro("PERSONASCARACTERESBUSCAR", (object) 3, Parametro.TipoParametro.Global, new DateTime?()));
      parametros.Add(new Parametro("PERMANENCIASCALCULAR", (object) 1, Parametro.TipoParametro.Global, new DateTime?()));
      parametros.Add(new Parametro("PERMANENCIATIEMPOMAXIMO", (object) 1080, Parametro.TipoParametro.Global, new DateTime?()));
      parametros.Add(new Parametro("PLACASTIPOPUERTO", (object) string.Empty, Parametro.TipoParametro.Global, new DateTime?()));
      parametros.Add(new Parametro("ENTIDADNOMBRE", (object) string.Empty, Parametro.TipoParametro.Global, new DateTime?()));
      parametros.Add(new Parametro("INFORMESIMAGEN", (object) string.Empty, Parametro.TipoParametro.Global, new DateTime?()));
      parametros.Add(new Parametro("TARJETACALCULADA", (object) string.Empty, Parametro.TipoParametro.Global, new DateTime?()));
      parametros.Add(new Parametro("TARJETACALCULADAFACILITYCODE", (object) string.Empty, Parametro.TipoParametro.Global, new DateTime?()));
      parametros.Add(new Parametro("TARJETACALCULADABITS", (object) string.Empty, Parametro.TipoParametro.Global, new DateTime?()));
      parametros.Add(new Parametro("TARJETACALCULADAPOSICIONES", (object) string.Empty, Parametro.TipoParametro.Global, new DateTime?()));
      parametros.Add(new Parametro("TICKETIDENTIFICADORDIGITOS", (object) 3, Parametro.TipoParametro.Global, new DateTime?()));
      parametros.Add(new Parametro("FOTOASPECTOX", (object) 1, Parametro.TipoParametro.Global, new DateTime?()));
      parametros.Add(new Parametro("FOTOASPECTOY", (object) 1, Parametro.TipoParametro.Global, new DateTime?()));
      parametros.Add(new Parametro("INFORMESRUTA", (object) "C:\\", Parametro.TipoParametro.Local, new DateTime?()));
      parametros.Add(new Parametro("RUTAEXPORTADOR", (object) "C:\\", Parametro.TipoParametro.Local, new DateTime?()));
      parametros.Add(new Parametro("PDFEXTERNO", (object) "acrord32.exe", Parametro.TipoParametro.Local, new DateTime?()));
      parametros.Add(new Parametro("VISORPDF", (object) 0, Parametro.TipoParametro.Local, new DateTime?()));
      parametros.Add(new Parametro("INFORMEDELIMITADORPLANO", (object) 0, Parametro.TipoParametro.Local, new DateTime?()));
      parametros.Add(new Parametro("COLORGRILLAA", (object) -1, Parametro.TipoParametro.Local, new DateTime?()));
      parametros.Add(new Parametro("COLORGRILLAB", (object) -1828, Parametro.TipoParametro.Local, new DateTime?()));
      parametros.Add(new Parametro("COLORPANELSUPERIOR", (object) SystemColors.Window.ToArgb(), Parametro.TipoParametro.Local, new DateTime?()));
      parametros.Add(new Parametro("COLORPANELINFERIOR", (object) SystemColors.Window.ToArgb(), Parametro.TipoParametro.Local, new DateTime?()));
      parametros.Add(new Parametro("COLORMENUFONDO", (object) SystemColors.Window.ToArgb(), Parametro.TipoParametro.Local, new DateTime?()));
      parametros.Add(new Parametro("COLORMENUSELECCIONADO", (object) SystemColors.ActiveCaption.ToArgb(), Parametro.TipoParametro.Local, new DateTime?()));
      parametros.Add(new Parametro("COLORSUBMENUFONDO", (object) SystemColors.Window.ToArgb().ToString(), Parametro.TipoParametro.Local, new DateTime?()));
      parametros.Add(new Parametro("COLORSUBMENUSELECCIONADO", (object) SystemColors.ActiveCaption.ToArgb(), Parametro.TipoParametro.Local, new DateTime?()));
      parametros.Add(new Parametro("COLORPOPUPFONDO", (object) SystemColors.Window.ToArgb().ToString(), Parametro.TipoParametro.Local, new DateTime?()));
      parametros.Add(new Parametro("COLORPOPUPFRENTE", (object) SystemColors.WindowText.ToArgb(), Parametro.TipoParametro.Local, new DateTime?()));
      parametros.Add(new Parametro("COLORPOPUPINFOFONDO", (object) SystemColors.Window.ToArgb().ToString(), Parametro.TipoParametro.Local, new DateTime?()));
      parametros.Add(new Parametro("COLORPOPUPINFOFRENTE", (object) SystemColors.WindowText.ToArgb(), Parametro.TipoParametro.Local, new DateTime?()));
      parametros.Add(new Parametro("COLORPOPUPOKFONDO", (object) Color.Green.ToArgb().ToString(), Parametro.TipoParametro.Local, new DateTime?()));
      parametros.Add(new Parametro("COLORPOPUPOKFRENTE", (object) Color.White.ToArgb(), Parametro.TipoParametro.Local, new DateTime?()));
      parametros.Add(new Parametro("COLORPOPUPERRFONDO", (object) Color.Red.ToArgb().ToString(), Parametro.TipoParametro.Local, new DateTime?()));
      parametros.Add(new Parametro("COLORPOPUPERRFRENTE", (object) Color.White.ToArgb(), Parametro.TipoParametro.Local, new DateTime?()));
      parametros.Add(new Parametro("COLORPOPUPADVFONDO", (object) Color.Yellow.ToArgb().ToString(), Parametro.TipoParametro.Local, new DateTime?()));
      parametros.Add(new Parametro("COLORPOPUPADVFRENTE", (object) Color.Black.ToArgb(), Parametro.TipoParametro.Local, new DateTime?()));
      parametros.Add(new Parametro("COLORPADREFRENTE", (object) SystemColors.WindowText.ToArgb(), Parametro.TipoParametro.Local, new DateTime?()));
      parametros.Add(new Parametro("COLORPADREFONDO", (object) SystemColors.Window.ToArgb(), Parametro.TipoParametro.Local, new DateTime?()));
      parametros.Add(new Parametro("COLORSTATUSFRENTE", (object) SystemColors.WindowText.ToArgb(), Parametro.TipoParametro.Local, new DateTime?()));
      parametros.Add(new Parametro("COLORSTATUSFONDO", (object) SystemColors.Window.ToArgb(), Parametro.TipoParametro.Local, new DateTime?()));
      parametros.Add(new Parametro("COLORACCESOFONDO", (object) SystemColors.Window.ToArgb(), Parametro.TipoParametro.Local, new DateTime?()));
      parametros.Add(new Parametro("COLORACCESOFRENTE", (object) SystemColors.WindowText.ToArgb(), Parametro.TipoParametro.Local, new DateTime?()));
      parametros.Add(new Parametro("ADMINPWD", (object) string.Empty, Parametro.TipoParametro.Local, new DateTime?()));
      parametros.Add(new Parametro("ACCESOCIERREFORMULARIO", (object) 2, Parametro.TipoParametro.Local, new DateTime?()));
      parametros.Add(new Parametro("LECTORPCAUTOMATICO", (object) string.Empty, Parametro.TipoParametro.Local, new DateTime?()));
      parametros.Add(new Parametro("LECTORPCTIPOPUERTO", (object) string.Empty, Parametro.TipoParametro.Local, new DateTime?()));
      parametros.Add(new Parametro("LECTORPCPLACA", (object) string.Empty, Parametro.TipoParametro.Local, new DateTime?()));
      parametros.Add(new Parametro("LECTORPCPLACADIRECCION", (object) 1, Parametro.TipoParametro.Local, new DateTime?()));
      parametros.Add(new Parametro("LECTORPCPLACALADO", (object) "A", Parametro.TipoParametro.Local, new DateTime?()));
      parametros.Add(new Parametro("LECTORPCCOMNUMERO", (object) 1, Parametro.TipoParametro.Local, new DateTime?()));
      parametros.Add(new Parametro("LECTORPCCOMVELOCIDAD", (object) 9600, Parametro.TipoParametro.Local, new DateTime?()));
      parametros.Add(new Parametro("WEBCAMHABILITADA", (object) 1, Parametro.TipoParametro.Local, new DateTime?()));
      parametros.Add(new Parametro("TIPOTARJETAPREDETERMINADOID", (object) 0, Parametro.TipoParametro.Local, new DateTime?()));
      parametros.Add(new Parametro("TIPOLECTORAPREDETERMINADOID", (object) 0, Parametro.TipoParametro.Local, new DateTime?()));
      parametros.Add(new Parametro("SYSTEMTRAY", (object) 0, Parametro.TipoParametro.Local, new DateTime?()));
      parametros.Add(new Parametro("VISITASANTIPASSBACK", (object) 0, Parametro.TipoParametro.Local, new DateTime?()));
      parametros.Add(new Parametro("VISITASTARJETATIEMPOLIMITE", (object) 0, Parametro.TipoParametro.Local, new DateTime?()));
      parametros.Add(new Parametro("MONITORACTIVARALINICIAR", (object) 0, Parametro.TipoParametro.Local, new DateTime?()));
      parametros.Add(new Parametro("MONITORCOLORGRILLA1", (object) -4139, Parametro.TipoParametro.Local, new DateTime?()));
      parametros.Add(new Parametro("MONITORCOLORGRILLA2", (object) -983056, Parametro.TipoParametro.Local, new DateTime?()));
      parametros.Add(new Parametro("MONITORCOMPORTAMIENTOIMAGEN", (object) 0, Parametro.TipoParametro.Local, new DateTime?()));
      parametros.Add(new Parametro("MONITORFRECUENCIA", (object) 5000, Parametro.TipoParametro.Local, new DateTime?()));
      parametros.Add(new Parametro("MONITORLECTORES", (object) string.Empty, Parametro.TipoParametro.Local, new DateTime?()));
      parametros.Add(new Parametro("MONITORMOSTRARSEGUNDOS", (object) 5, Parametro.TipoParametro.Local, new DateTime?()));
      parametros.Add(new Parametro("MONITORPANTALLA", (object) 0, Parametro.TipoParametro.Local, new DateTime?()));
      parametros.Add(new Parametro("MONITORREGISTROSCANTIDAD", (object) 100, Parametro.TipoParametro.Local, new DateTime?()));
      parametros.Add(new Parametro("VENCIMIENTOEXTERNOMOTOR", (object) -1, Parametro.TipoParametro.Global, new DateTime?()));
      parametros.Add(new Parametro("VENCIMIENTOEXTERNOTABLA", (object) string.Empty, Parametro.TipoParametro.Global, new DateTime?()));
      parametros.Add(new Parametro("VENCIMIENTOEXTERNOPERSONA", (object) -1, Parametro.TipoParametro.Global, new DateTime?()));
      parametros.Add(new Parametro("VENCIMIENTOEXTERNOCOLUMNAPERSONA", (object) string.Empty, Parametro.TipoParametro.Global, new DateTime?()));
      parametros.Add(new Parametro("VENCIMIENTOEXTERNOTIPOVENCIMIENTO", (object) -1, Parametro.TipoParametro.Global, new DateTime?()));
      parametros.Add(new Parametro("VENCIMIENTOEXTERNOCOLUMNAVALOR", (object) string.Empty, Parametro.TipoParametro.Global, new DateTime?()));
      parametros.Add(new Parametro("VENCIMIENTOEXTERNOTIPODATO", (object) -1, Parametro.TipoParametro.Global, new DateTime?()));
      parametros.Add(new Parametro("VENCIMIENTOEXTERNOCONDICION", (object) -1, Parametro.TipoParametro.Global, new DateTime?()));
      parametros.Add(new Parametro("VENCIMIENTOEXTERNOCONDICIONVALOR", (object) string.Empty, Parametro.TipoParametro.Global, new DateTime?()));
      parametros.Add(new Parametro("VENCIMIENTOEXTERNOUBICACION", (object) string.Empty, Parametro.TipoParametro.Global, new DateTime?()));
      parametros.Add(new Parametro("VENCIMIENTOEXTERNOEXTENDERDIAS", (object) 30, Parametro.TipoParametro.Global, new DateTime?()));
      parametros.Add(new Parametro("VISITASMODOACCESO", (object) "TARJETA", Parametro.TipoParametro.Local, new DateTime?()));
      return parametros;
    }

    public static bool GuardarParametro(
      int terminalId,
      string clave,
      object valor,
      List<Parametro> parametros)
    {
      QueryEngine query = new QueryEngine();
      int num = 0;
      try
      {
        Parametro parametro = parametros.Find((Predicate<Parametro>) (p => p.Nombre == clave));
        if (parametro != null)
        {
          query.QueryName = "OS_parametroInsertUpdate";
          query.AddNames("parametroClave", "parametroValor", nameof (terminalId));
          query.AddTypes(DbType.String, DbType.String, DbType.Int32);
          if (parametro.Alcance == Parametro.TipoParametro.Local)
            query.AddValues((object) clave, (object) valor.ToString(), (object) terminalId);
          else
            query.AddValues((object) clave, (object) valor.ToString(), (object) DBNull.Value);
          num = Utils.oConexiones["CP"].DbExecuteNonQuery(query);
          if (num > 0)
          {
            parametro.Valor = valor;
            parametro.UltimaLecturaDesdeBase = Utils.RelojInterno.CalcularFechaHoraServidor();
          }
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

    public static int GuardarParametros(
      int terminalId,
      Parametros parametrosAGuardar,
      List<Parametro> parametrosSistema)
    {
      DateTime? nullable1 = new DateTime?();
      QueryEngine query = new QueryEngine();
      int num = 0;
      try
      {
        DateTime? nullable2 = Utils.RelojInterno.CalcularFechaHoraServidor();
        query.QueryName = "OS_parametroInsertUpdate";
        query.AddNames("parametroClave", "parametroValor", nameof (terminalId));
        query.AddTypes(DbType.String, DbType.String, DbType.Int32);
        using (List<Parametro>.Enumerator enumerator = parametrosAGuardar.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            Parametro parametroAGuardar = enumerator.Current;
            Parametro parametro = parametrosSistema.Find((Predicate<Parametro>) (p => p.Nombre == parametroAGuardar.Nombre));
            if (parametro != null)
            {
              if (parametro.Alcance == Parametro.TipoParametro.Local)
                query.AddValues((object) parametroAGuardar.Nombre, (object) parametroAGuardar.Valor.ToString(), (object) terminalId);
              else
                query.AddValues((object) parametroAGuardar.Nombre, (object) parametroAGuardar.Valor.ToString(), (object) DBNull.Value);
              num += Utils.oConexiones["CP"].DbExecuteNonQuery(query);
              query.RemoveValues();
              if (num > 0)
              {
                parametro.Valor = parametroAGuardar.Valor;
                parametro.UltimaLecturaDesdeBase = nullable2;
              }
            }
          }
        }
      }
      catch
      {
        throw;
      }
      finally
      {
      }
      return num;
    }

    public enum TipoParametro
    {
      Global,
      Local,
    }
  }
}
