// Decompiled with JetBrains decompiler
// Type: DASYS.Recolector.BLL.ResultadoNumero
// Assembly: BLL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 684D872A-58E1-4C16-9B83-6ABA379FCE9D
// Assembly location: C:\Program Files (x86)\NAPSA\Colector III\BLL.dll

using System;

namespace DASYS.Recolector.BLL
{
  public class ResultadoNumero : IResultadosPaquete
  {
    private byte numeroGanador = byte.MaxValue;
    private const ProtocoloNAPSA.ProtocoloTipoPaquete tipoPaquete = ProtocoloNAPSA.ProtocoloTipoPaquete.NumeroGanador;
    private string cadenaOriginal;
    private char checkSum;

    public ResultadoNumero()
    {
    }

    public ResultadoNumero(string cadenaOriginal)
    {
      this.cadenaOriginal = cadenaOriginal;
      this.Parsear();
    }

    public ResultadoNumero(byte numeroGanador, char checksum)
    {
      this.numeroGanador = numeroGanador;
      this.checkSum = checksum;
    }

    public ProtocoloNAPSA.ProtocoloTipoPaquete TipoPaquete
    {
      get
      {
        return ProtocoloNAPSA.ProtocoloTipoPaquete.NumeroGanador;
      }
    }

    public byte NumeroGanador
    {
      get
      {
        return this.numeroGanador;
      }
    }

    public char CheckSum
    {
      get
      {
        return this.checkSum;
      }
    }

    public string CadenaOriginal
    {
      get
      {
        return this.cadenaOriginal;
      }
      set
      {
        this.cadenaOriginal = value;
      }
    }

    public IResultadosPaquete Parsear()
    {
      try
      {
        if (!string.IsNullOrEmpty(this.cadenaOriginal))
        {
          if (this.cadenaOriginal.Length == 9)
          {
            this.numeroGanador = (byte) Math.Abs((short) Common.Datos.NullToByte((object) this.cadenaOriginal.Substring(2, 2), byte.MaxValue));
            this.checkSum = this.cadenaOriginal.Substring(8, 1)[0];
          }
        }
      }
      catch
      {
        throw;
      }
      return (IResultadosPaquete) this;
    }
  }
}
