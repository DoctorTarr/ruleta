// Decompiled with JetBrains decompiler
// Type: DASYS.Recolector.BLL.Persistencia
// Assembly: BLL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 684D872A-58E1-4C16-9B83-6ABA379FCE9D
// Assembly location: C:\Program Files (x86)\NAPSA\Colector III\BLL.dll

using DASYS.DAL;
using System;
using System.Data;

namespace DASYS.Recolector.BLL
{
  public static class Persistencia
  {
    private static int estadoReciente;
    private static byte velocidadReciente;
    private static byte sentidoGiroReciente;

    public static bool Guardar(string cadena)
    {
      QueryEngine query = new QueryEngine();
      bool flag1 = false;
      try
      {
        IResultadosPaquete resultadoPaquete = new ProtocoloNAPSA(cadena).ResultadoPaquete;
        if (resultadoPaquete != null)
        {
          switch (resultadoPaquete.TipoPaquete)
          {
            case ProtocoloNAPSA.ProtocoloTipoPaquete.NumeroGanador:
              bool flag2 = false;
              if (DateTime.Now.Subtract(Pase.UltimoPase.FechaHora).Seconds < 3)
              {
                flag2 = true;
              }
              else
              {
                Pase.UltimoPase.NumeroGanador = ((ResultadoNumero) resultadoPaquete).NumeroGanador;
                Pase.UltimoPase.FechaHora = DateTime.Now;
                Pase.UltimoPase.SentidoGiro = Persistencia.sentidoGiroReciente;
                Pase.UltimoPase.Velocidad = Persistencia.velocidadReciente;
                ++Pase.UltimoPase.NumeroTiro;
                Persistencia.estadoReciente = 0;
              }
              if (!flag2)
              {
                flag1 = Pase.EscribirEnBase(Pase.UltimoPase.NumeroTiro, ((ResultadoNumero) resultadoPaquete).NumeroGanador, Pase.UltimoPase.Velocidad, Pase.UltimoPase.SentidoGiro, true);
                Pase.GuardarUltimoEstado(ResultadoStatus.StatusEstado.WinningNumber);
                break;
              }
              break;
            case ProtocoloNAPSA.ProtocoloTipoPaquete.Status:
              bool flag3 = false;
              bool flag4 = false;
              byte numeroGanador = ((ResultadoStatus) resultadoPaquete).NumeroGanador;
              ResultadoStatus.StatusError error = ((ResultadoStatus) resultadoPaquete).Error;
              ResultadoStatus.StatusEstado estado = ((ResultadoStatus) resultadoPaquete).Estado;
              ResultadoStatus.StatusSentidoGiro sentidoGiro = ((ResultadoStatus) resultadoPaquete).SentidoGiro;
              byte velocidadGiro = ((ResultadoStatus) resultadoPaquete).VelocidadGiro;
              if (error != ResultadoStatus.StatusError.OK)
              {
                flag3 = true;
                Persistencia.estadoReciente = 0;
                flag4 = true;
              }
              else if (estado == ResultadoStatus.StatusEstado.NoMoreBets && Persistencia.estadoReciente != 4)
              {
                Persistencia.velocidadReciente = velocidadGiro;
                Persistencia.sentidoGiroReciente = (byte) sentidoGiro;
                Persistencia.estadoReciente = 4;
                flag3 = true;
                flag4 = true;
              }
              else if (estado == ResultadoStatus.StatusEstado.WinningNumber && Persistencia.estadoReciente != 5)
              {
                Persistencia.estadoReciente = 5;
                flag4 = true;
              }
              else if (estado == ResultadoStatus.StatusEstado.BeforeGame && Persistencia.estadoReciente != 1)
              {
                Persistencia.estadoReciente = 1;
                flag4 = true;
              }
              else if (estado == ResultadoStatus.StatusEstado.PlaceYourBet && Persistencia.estadoReciente != 2)
              {
                Persistencia.estadoReciente = 2;
                flag4 = true;
              }
              else if (estado == ResultadoStatus.StatusEstado.FinishBetting && Persistencia.estadoReciente != 3)
              {
                Persistencia.estadoReciente = 3;
                flag4 = true;
              }
              else if (estado == ResultadoStatus.StatusEstado.CloseTable && Persistencia.estadoReciente != 6)
              {
                Persistencia.estadoReciente = 6;
                flag4 = true;
              }
              if (flag3)
              {
                flag1 = Status.EscribirEnBase(numeroGanador, (byte) estado, velocidadGiro, (byte) sentidoGiro, (byte) error);
                if (Persistencia.estadoReciente == 4 && error == ResultadoStatus.StatusError.OK)
                  Persistencia.actualizarEstadoNMB();
              }
              if (flag4)
              {
                Pase.GuardarUltimoEstado(estado);
                break;
              }
              break;
            default:
              query.QueryName = "errorInsert";
              query.AddNames("errorCadena");
              query.AddTypes(DbType.String);
              query.AddValues((object) cadena);
              flag1 = Common.oConexiones[0].DbExecuteNonQuery(query) > 0;
              break;
          }
        }
      }
      catch
      {
        throw;
      }
      return flag1;
    }

    private static void actualizarEstadoNMB()
    {
      QueryEngine query1 = new QueryEngine();
      try
      {
        query1.QueryName = "displayDataDelete";
        Common.oConexiones[0].DbExecuteNonQuery(query1);
        QueryEngine query2 = new QueryEngine();
        query2.QueryName = "displayDataInsert";
        query2.AddNames("displayDataNMB");
        query2.AddTypes(DbType.Byte);
        query2.AddValues((object) 1);
        Common.oConexiones[0].DbExecuteNonQuery(query2);
      }
      catch
      {
        throw;
      }
    }
  }
}
