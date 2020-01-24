// Decompiled with JetBrains decompiler
// Type: DASYS.Recolector.BLL.ResultadoStatus
// Assembly: BLL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 684D872A-58E1-4C16-9B83-6ABA379FCE9D
// Assembly location: C:\Program Files (x86)\NAPSA\Colector III\BLL.dll

using System;

namespace DASYS.Recolector.BLL
{
  public class ResultadoStatus : IResultadosPaquete
  {
    private byte numeroGanador = byte.MaxValue;
    private ResultadoStatus.EstadoSentidoGiro sentidoGiro = ResultadoStatus.EstadoSentidoGiro.Indeterminado;
    private ResultadoStatus.EstadoError error = ResultadoStatus.EstadoError.Indeterminado;
    private const ProtocoloNAPSA.ProtocoloTipoPaquete tipoPaquete = ProtocoloNAPSA.ProtocoloTipoPaquete.Status;
    private string cadenaOriginal;
    private ResultadoStatus.EstadoJuego estado;
    private byte velocidadGiro;

    public ResultadoStatus()
    {
    }

    public ResultadoStatus(string cadenaRecibida)
    {
      this.cadenaOriginal = cadenaRecibida;
      this.Parsear();
    }

    public ResultadoStatus(
      byte numeroGanador,
      ResultadoStatus.EstadoJuego estado,
      byte velocidadGiro,
      ResultadoStatus.EstadoSentidoGiro sentidoGiro,
      ResultadoStatus.EstadoError error)
    {
      this.numeroGanador = numeroGanador;
      this.estado = estado;
      this.velocidadGiro = velocidadGiro;
      this.sentidoGiro = sentidoGiro;
      this.error = error;
    }

    public ProtocoloNAPSA.ProtocoloTipoPaquete TipoPaquete
    {
      get
      {
        return ProtocoloNAPSA.ProtocoloTipoPaquete.Status;
      }
    }

    public byte NumeroGanador
    {
      get
      {
        return this.numeroGanador;
      }
    }

    public ResultadoStatus.EstadoJuego Estado
    {
      get
      {
        return this.estado;
      }
    }

    public byte VelocidadGiro
    {
      get
      {
        return this.velocidadGiro;
      }
    }

    public ResultadoStatus.EstadoSentidoGiro SentidoGiro
    {
      get
      {
        return this.sentidoGiro;
      }
    }

    public ResultadoStatus.EstadoError Error
    {
      get
      {
        return this.error;
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
            this.estado = (ResultadoStatus.EstadoJuego) Math.Abs((short) Common.Datos.NullToByte((object) this.cadenaOriginal.Substring(4, 1), (byte) 0));
            this.velocidadGiro = (byte) Math.Abs(Common.Datos.NullToInt32((object) this.cadenaOriginal.Substring(5, 2), 0));
            this.sentidoGiro = (ResultadoStatus.EstadoSentidoGiro) Math.Abs((short) Common.Datos.NullToByte((object) this.cadenaOriginal.Substring(7, 1), (byte) 2));
            this.error = (ResultadoStatus.EstadoError) Math.Abs((short) Common.Datos.NullToByte((object) this.cadenaOriginal.Substring(8, 1), (byte) 10));
          }
        }
      }
      catch
      {
        throw;
      }
      return (IResultadosPaquete) this;
    }

    public enum EstadoJuego
    {
      Indeterminado,
      BeforeGame,
      PlaceYourBet,
      FinishBetting,
      NoMoreBets,
      WinningNumber,
      CloseTable,
    }

    public enum EstadoSentidoGiro
    {
      AntiHorario,
      Horario,
      Indeterminado,
    }

    public enum EstadoError
    {
      OK,
      Estrobos,
      Giro,
      Compuerta,
      LecturaDeBola,
      NC,
      Disparo,
      MultipleDeteccion,
      ExtraccionDeBola,
      Inicializacion,
      Indeterminado,
    }
  }
}
