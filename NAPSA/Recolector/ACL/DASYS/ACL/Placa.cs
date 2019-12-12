// Decompiled with JetBrains decompiler
// Type: DASYS.ACL.Placa
// Assembly: ACL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A55A6FFB-1772-4E30-9250-F6DDD06AF421
// Assembly location: C:\Program Files (x86)\NAPSA\Colector III\ACL.dll

using System;

namespace DASYS.ACL
{
  public class Placa
  {
    public static Placa.ModeloPlaca Nombre = Placa.ModeloPlaca.Ninguna;
    public static int PoleoMilisegundos = 10;

    public static Placa.ModeloPlaca ParseModeloPlaca(string nombreModeloPlaca)
    {
      return Placa.ParseModeloPlaca(nombreModeloPlaca, Placa.ModeloPlaca.Ninguna);
    }

    public static Placa.ModeloPlaca ParseModeloPlaca(
      string nombreModeloPlaca,
      Placa.ModeloPlaca modeloPlacaDefault)
    {
      Placa.ModeloPlaca modeloPlaca1 = modeloPlacaDefault;
      foreach (Placa.ModeloPlaca modeloPlaca2 in Enum.GetValues(typeof (Placa.ModeloPlaca)))
      {
        if (modeloPlaca2.ToString().Equals(nombreModeloPlaca, StringComparison.OrdinalIgnoreCase))
        {
          modeloPlaca1 = modeloPlaca2;
          break;
        }
      }
      return modeloPlaca1;
    }

    public enum ModeloPlaca
    {
      Ninguna,
      DC900,
      CDT1000,
      M2L,
    }
  }
}
