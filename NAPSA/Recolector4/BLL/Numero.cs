// Decompiled with JetBrains decompiler
// Type: DASYS.Recolector.BLL.Numero
// Assembly: BLL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 684D872A-58E1-4C16-9B83-6ABA379FCE9D
// Assembly location: C:\Program Files (x86)\NAPSA\Colector III\BLL.dll

using System;
using System.Collections.Generic;

namespace DASYS.Recolector.BLL
{
  public class Numero
  {
    private string numeroPalabra = string.Empty;
    private string representacion = string.Empty;
    private Numero.ColorFicha color = Numero.ColorFicha.Ninguno;
    private Numero.DocenaUbicacion docena = Numero.DocenaUbicacion.Ninguna;
    private Numero.ParidadValor paridad = Numero.ParidadValor.Ninguno;
    private Numero.ColumnaUbicacion columna = Numero.ColumnaUbicacion.Ninguna;
    private Numero.MitadUbicacion mitad = Numero.MitadUbicacion.Ninguna;
    private byte valorAnteriorPlato = byte.MaxValue;
    private byte valorSiguientePlato = byte.MaxValue;
    private byte valor;

    public byte Valor
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

    public string NumeroPalabra
    {
      get
      {
        return this.numeroPalabra;
      }
      set
      {
        this.numeroPalabra = value;
      }
    }

    public string Representacion
    {
      get
      {
        return this.representacion;
      }
      set
      {
        this.representacion = value;
      }
    }

    public Numero.ColorFicha Color
    {
      get
      {
        return this.color;
      }
      set
      {
        this.color = value;
      }
    }

    public Numero.DocenaUbicacion Docena
    {
      get
      {
        return this.docena;
      }
    }

    public Numero.ParidadValor Paridad
    {
      get
      {
        return this.paridad;
      }
    }

    public Numero.ColumnaUbicacion Columna
    {
      get
      {
        return this.columna;
      }
    }

    public Numero.MitadUbicacion Mitad
    {
      get
      {
        return this.mitad;
      }
    }

    public Numero NumeroAnteriorPlato
    {
      get
      {
        return Numero.ObtenerPropiedadesNumero(this.valorAnteriorPlato);
      }
    }

    public Numero NumeroSiguientePlato
    {
      get
      {
        return Numero.ObtenerPropiedadesNumero(this.valorSiguientePlato);
      }
    }

    public Numero()
    {
    }

    public Numero(
      byte valor,
      string numero,
      string representacion,
      Numero.ColorFicha color,
      byte valorAnteriorPlato,
      byte valorSiguientePlato)
    {
      this.valor = valor;
      this.numeroPalabra = numero;
      this.representacion = representacion;
      this.color = color;
      if (valor < (byte) 0 || valor > (byte) 37)
        throw new OverflowException("El número debe ser entre cero y treinta y seis, o treinta y siete (que representa el doble cero).");
      if (valor > (byte) 0 && valor < (byte) 37)
      {
        this.docena = (Numero.DocenaUbicacion) (((int) valor - 1) / 12);
        this.columna = (int) valor % 3 == 0 ? Numero.ColumnaUbicacion.Tercera : (Numero.ColumnaUbicacion) ((int) valor % 3 - 1);
        this.paridad = (int) valor % 2 == 0 ? Numero.ParidadValor.Par : Numero.ParidadValor.Impar;
        this.mitad = (Numero.MitadUbicacion) (((int) valor - 1) / 18);
      }
      this.valorAnteriorPlato = valorAnteriorPlato;
      this.valorSiguientePlato = valorSiguientePlato;
    }

    public static Numero ObtenerPropiedadesNumero(byte numero)
    {
      Numero numero1 = (Numero) null;
      if (Plato.NumerosJuegoRuleta == null || Plato.NumerosJuegoRuleta.Count == 0)
        throw new NullReferenceException("No se han cargado los números del juego de la ruleta.");
      if (numero < (byte) 0 || numero > (byte) 37)
        throw new OverflowException("El número debe ser entre cero y treinta y seis, o treinta y siete (doble cero).");
      foreach (Numero numero2 in (List<Numero>) Plato.NumerosJuegoRuleta)
      {
        if ((int) numero == (int) numero2.Valor)
        {
          numero1 = numero2;
          break;
        }
      }
      return numero1;
    }

    public static Numero ObtenerNumeroPorPosicion(byte posicion)
    {
      if (Plato.NumerosJuegoRuleta == null || Plato.NumerosJuegoRuleta.Count == 0)
        throw new NullReferenceException("No se han cargado los números del juego de la ruleta.");
      if (posicion < (byte) 0 || (int) posicion > Plato.NumerosJuegoRuleta.Count)
        throw new OverflowException("El número debe ser entre cero y treinta y seis, o treinta y siete (doble cero).");
      return Plato.NumerosJuegoRuleta[(int) posicion];
    }

    public enum ParidadValor
    {
      Par,
      Impar,
      Ninguno,
    }

    public enum ColorFicha
    {
      Rojo,
      Negro,
      Verde,
      Ninguno,
    }

    public enum DocenaUbicacion
    {
      Primera,
      Segunda,
      Tercera,
      Ninguna,
    }

    public enum ColumnaUbicacion
    {
      Primera,
      Segunda,
      Tercera,
      Ninguna,
    }

    public enum MitadUbicacion
    {
      Primera,
      Segunda,
      Ninguna,
    }
  }
}
