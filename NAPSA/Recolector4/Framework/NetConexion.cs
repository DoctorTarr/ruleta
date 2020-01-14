// Decompiled with JetBrains decompiler
// Type: DASYS.Framework.NetConexion
// Assembly: Framework, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1B503700-E29D-4D7A-BD70-519F036595D0
// Assembly location: C:\Program Files (x86)\NAPSA\Colector III\Framework.dll

using System;
using System.Net;

namespace DASYS.Framework
{
  public class NetConexion
  {
    private string _ip = string.Empty;
    private string _nombre = string.Empty;
    private int _puerto;

    public NetConexion()
    {
    }

    public NetConexion(string ip, int puerto)
    {
      this.IP = ip;
      this.Puerto = puerto;
    }

    public NetConexion(string ip, int puerto, string nombre)
    {
      this.IP = ip;
      this.Puerto = puerto;
      this.Nombre = nombre;
    }

    public string IP
    {
      get
      {
        return this._ip;
      }
      set
      {
        this._ip = value;
      }
    }

    public string Nombre
    {
      get
      {
        return this._nombre;
      }
      set
      {
        this._nombre = value;
      }
    }

    public int Puerto
    {
      get
      {
        return this._puerto;
      }
      set
      {
        this._puerto = value;
      }
    }

    public void ObtenerIPPropia()
    {
      try
      {
        this.Nombre = Dns.GetHostName();
        foreach (object address in Dns.GetHostEntry(this.Nombre).AddressList)
          this.IP = address.ToString();
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message);
      }
    }
  }
}
