// Decompiled with JetBrains decompiler
// Type: DASYS.Recolector.BLL.ProtocoloNAPSA
// Assembly: BLL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 684D872A-58E1-4C16-9B83-6ABA379FCE9D
// Assembly location: C:\Program Files (x86)\NAPSA\Colector III\BLL.dll

using System;

namespace DASYS.Recolector.BLL
{
  public class ProtocoloNAPSA
  {
    private string cadenaRecibida = string.Empty;
    private IResultadosPaquete resultadoPaquete;

    public ProtocoloNAPSA()
    {
    }

    public ProtocoloNAPSA(string cadenaRecibida)
    {
      this.cadenaRecibida = cadenaRecibida;
      this.resultadoPaquete = this.analizarCadena();
    }

    public string CadenaRecibida
    {
      get
      {
        return this.cadenaRecibida;
      }
    }

    public IResultadosPaquete ResultadoPaquete
    {
      get
      {
        return this.resultadoPaquete;
      }
    }

    private IResultadosPaquete analizarCadena()
    {
      IResultadosPaquete resultadosPaquete = (IResultadosPaquete) null;
      try
      {
        if (!string.IsNullOrEmpty(this.cadenaRecibida))
        {
          if (this.cadenaRecibida.Length == 9)
          {
            switch (this.cadenaRecibida.Substring(0, 2).ToUpper())
            {
              case "NN":
                resultadosPaquete = (IResultadosPaquete) new ResultadoNumero(this.cadenaRecibida);
                break;
              case "NS":
                resultadosPaquete = (IResultadosPaquete) new ResultadoStatus(this.cadenaRecibida);
                break;
            }
          }
        }
      }
      catch (Exception ex)
      {
        throw;
      }
      return resultadosPaquete;
    }

    static public string FormatearCadenaEstado(byte numero, EstadoJuego estado, int rpm, int sentidoGiro, byte error)
    {
            int estadoJuego = (int)estado;
            string cadena = cadena = "NS" + numero.ToString("00") + estadoJuego.ToString() + rpm.ToString("00") + sentidoGiro.ToString() + error.ToString();

            return (cadena);
    }

        static public string FormatearCadenaNumeroGanador(byte numero)
        {
            string cadena = "NN" + numero.ToString("00") + "    N";

            return (cadena);
        }

        public enum ProtocoloTipoPaquete
    {
      NumeroGanador,
      Status,
      Null,
    }

    public enum EstadoJuego : int
    {
        StartingApp,
        BeforeGame,
        PlaceYourBet,
        FinishBetting,
        NoMoreBets,
        WinningNumber,
        CloseTable
    }

  }
}
