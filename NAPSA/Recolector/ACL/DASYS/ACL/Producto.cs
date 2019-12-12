// Decompiled with JetBrains decompiler
// Type: DASYS.ACL.Producto
// Assembly: ACL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A55A6FFB-1772-4E30-9250-F6DDD06AF421
// Assembly location: C:\Program Files (x86)\NAPSA\Colector III\ACL.dll

using ACLBase;
using System.Collections.Generic;
using System.Drawing;

namespace DASYS.ACL
{
  public class Producto
  {
    public static string[] ClientesFinales = new string[2]
    {
      "Club Atlético Lanús",
      "Tecnologías de Seguridad y Acceso S.A."
    };
    public static string DASYSProductoVersion = string.Empty;
    public static Producto.Licenciatarios Licenciatario = Producto.Licenciatarios.TEACSA;
    public static string ClienteFinalNombre = Producto.ClientesFinales[0];
    private string licenciatarioEntidadNombre = string.Empty;
    private string licenciatarioEntidadWEB = string.Empty;
    private string licenciatarioConectorNombre = string.Empty;
    private string licenciatarioProductoNombre = string.Empty;
    private string licenciatarioProductoVersion = string.Empty;
    private string licenciatarioInicialesArchivos = string.Empty;
    private string licenciatarioInicialesRecursos = string.Empty;
    private string clienteEntidadNombre = string.Empty;
    public const string DASYSEntidadNombre = "DASYS";
    public const string DASYSProductoNombre = "Open Shields";
    public const string DASYSEntidadWEB = "www.dasys.com.ar";
    public const string DASYSConectorNombre = "Cop Control";
    public const string DASYSInicialesArchivos = "OS";
    public const string DASYSInicialesRecursos = "OS";
    public const bool ReemplazaNullPorNativo = false;

    public Producto(string version)
    {
      Producto.DASYSProductoVersion = version;
    }

    public string LicenciatarioEntidadNombre
    {
      get
      {
        return this.licenciatarioEntidadNombre;
      }
      set
      {
        this.licenciatarioEntidadNombre = value;
      }
    }

    public Bitmap LicenciatarioEntidadLogo
    {
      get
      {
        switch (Producto.Licenciatario)
        {
          case Producto.Licenciatarios.OS:
            return ACLBase.Licenciatarios.DASYS;
          case Producto.Licenciatarios.PST:
            return ACLBase.Licenciatarios.PST;
          case Producto.Licenciatarios.TEACSA:
            return ACLBase.Licenciatarios.TEACSA;
          default:
            return ACLBase.Licenciatarios.DASYS;
        }
      }
    }

    public Bitmap LicenciatarioEntidadLogoMini
    {
      get
      {
        switch (Producto.Licenciatario)
        {
          case Producto.Licenciatarios.OS:
            return ACLBase.Licenciatarios.DASYSmini;
          case Producto.Licenciatarios.PST:
            return ACLBase.Licenciatarios.PSTmini;
          case Producto.Licenciatarios.TEACSA:
            return ACLBase.Licenciatarios.TEACSAmini;
          default:
            return ACLBase.Licenciatarios.DASYSmini;
        }
      }
    }

    public string LicenciatarioEntidadWEB
    {
      get
      {
        return this.licenciatarioEntidadWEB;
      }
      set
      {
        this.licenciatarioEntidadWEB = value;
      }
    }

    public string LicenciatarioProductoNombre
    {
      get
      {
        return this.licenciatarioProductoNombre;
      }
      set
      {
        this.licenciatarioProductoNombre = value;
      }
    }

    public string LicenciatarioConectorNombre
    {
      get
      {
        return this.licenciatarioConectorNombre;
      }
      set
      {
        this.licenciatarioConectorNombre = value;
      }
    }

    public string LicenciatarioProductoVersion
    {
      get
      {
        return this.licenciatarioProductoVersion;
      }
      set
      {
        this.licenciatarioProductoVersion = value;
      }
    }

    public Bitmap LicenciatarioProductoLogo
    {
      get
      {
        switch (Producto.Licenciatario)
        {
          case Producto.Licenciatarios.OS:
            return Productos.OS;
          case Producto.Licenciatarios.PST:
            return Productos.OS;
          case Producto.Licenciatarios.TEACSA:
            return Productos.SCATmini;
          default:
            return Productos.OS;
        }
      }
    }

    public Bitmap LicenciatarioProductoLogoMini
    {
      get
      {
        switch (Producto.Licenciatario)
        {
          case Producto.Licenciatarios.OS:
            return Productos.OS;
          case Producto.Licenciatarios.PST:
            return Productos.OS;
          case Producto.Licenciatarios.TEACSA:
            return Productos.SCATmini;
          default:
            return Productos.OS;
        }
      }
    }

    public List<Placa.ModeloPlaca> LicenciatarioPlacas
    {
      get
      {
        List<Placa.ModeloPlaca> modeloPlacaList = new List<Placa.ModeloPlaca>();
        switch (Producto.Licenciatario)
        {
          case Producto.Licenciatarios.OS:
            modeloPlacaList.Add(Placa.ModeloPlaca.CDT1000);
            modeloPlacaList.Add(Placa.ModeloPlaca.DC900);
            modeloPlacaList.Add(Placa.ModeloPlaca.M2L);
            break;
          case Producto.Licenciatarios.PST:
            modeloPlacaList.Add(Placa.ModeloPlaca.DC900);
            modeloPlacaList.Add(Placa.ModeloPlaca.M2L);
            break;
          case Producto.Licenciatarios.TEACSA:
            modeloPlacaList.Add(Placa.ModeloPlaca.CDT1000);
            break;
          default:
            modeloPlacaList.Add(Placa.ModeloPlaca.Ninguna);
            break;
        }
        return modeloPlacaList;
      }
    }

    public string LicenciatarioInicialesArchivos
    {
      get
      {
        return this.licenciatarioInicialesArchivos;
      }
      set
      {
        this.licenciatarioInicialesArchivos = value;
      }
    }

    public string LicenciatarioInicialesRecursos
    {
      get
      {
        return this.licenciatarioInicialesRecursos;
      }
      set
      {
        this.licenciatarioInicialesRecursos = value;
      }
    }

    public string ClienteEntidadNombre
    {
      get
      {
        return this.clienteEntidadNombre;
      }
      set
      {
        this.clienteEntidadNombre = value;
      }
    }

    public Bitmap ClienteLogo
    {
      get
      {
        return Producto.ClienteFinalNombre == null ? Clientes.DASYS : (!Producto.ClienteFinalNombre.Equals(Producto.ClientesFinales[0]) ? (!Producto.ClienteFinalNombre.Equals(Producto.ClientesFinales[1]) ? Clientes.DASYS : Clientes.TEACSACLIENTE) : Clientes.CAL);
      }
    }

    public Bitmap ClienteLogoMini
    {
      get
      {
        return Producto.ClienteFinalNombre == null ? Clientes.DASYS : (!Producto.ClienteFinalNombre.Equals(Producto.ClientesFinales[0]) ? (!Producto.ClienteFinalNombre.Equals(Producto.ClientesFinales[1]) ? Clientes.DASYSmini : Clientes.TEACSACLIENTEmini) : Clientes.CALmini);
      }
    }

    public enum Licenciatarios
    {
      OS,
      PST,
      TEACSA,
    }
  }
}
