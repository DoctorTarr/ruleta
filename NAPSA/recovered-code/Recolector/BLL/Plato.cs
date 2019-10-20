// Decompiled with JetBrains decompiler
// Type: DASYS.Recolector.BLL.Plato
// Assembly: BLL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 684D872A-58E1-4C16-9B83-6ABA379FCE9D
// Assembly location: C:\Program Files (x86)\NAPSA\Colector III\BLL.dll

using System.Collections.Generic;

namespace DASYS.Recolector.BLL
{
  public class Plato
  {
    private byte[] numerosPlato = new byte[38]
    {
      (byte) 0,
      (byte) 32,
      (byte) 15,
      (byte) 19,
      (byte) 4,
      (byte) 21,
      (byte) 2,
      (byte) 25,
      (byte) 17,
      (byte) 34,
      (byte) 6,
      (byte) 27,
      (byte) 13,
      (byte) 36,
      (byte) 11,
      (byte) 30,
      (byte) 8,
      (byte) 23,
      (byte) 10,
      (byte) 37,
      (byte) 5,
      (byte) 24,
      (byte) 16,
      (byte) 33,
      (byte) 1,
      (byte) 20,
      (byte) 14,
      (byte) 31,
      (byte) 9,
      (byte) 22,
      (byte) 18,
      (byte) 29,
      (byte) 7,
      (byte) 28,
      (byte) 12,
      (byte) 35,
      (byte) 3,
      (byte) 26
    };
    public static Numeros NumerosJuegoRuleta;
    private bool esConDobleCero;
    private List<Plato.Sector> sectores;

    public Plato()
    {
      Plato.NumerosJuegoRuleta = this.ObtenerColeccion();
    }

    public Plato(bool esConDobleCero)
    {
      this.esConDobleCero = esConDobleCero;
      Plato.NumerosJuegoRuleta = this.ObtenerColeccion();
    }

    public bool EsConDobleCero
    {
      get
      {
        return this.esConDobleCero;
      }
    }

    public List<Plato.Sector> Sectores
    {
      get
      {
        return this.sectores;
      }
      set
      {
        this.sectores = value;
      }
    }

    private Numeros ObtenerColeccion()
    {
      Numeros numeros = new Numeros();
      numeros.Add(new Numero((byte) 0, "cero", "0", Numero.ColorFicha.Verde, (byte) 26, (byte) 32));
      numeros.Add(new Numero((byte) 32, "treinta y dos", "32", Numero.ColorFicha.Rojo, (byte) 0, (byte) 15));
      numeros.Add(new Numero((byte) 15, "quince", "15", Numero.ColorFicha.Negro, (byte) 32, (byte) 19));
      numeros.Add(new Numero((byte) 19, "diez y nueve", "19", Numero.ColorFicha.Rojo, (byte) 15, (byte) 4));
      numeros.Add(new Numero((byte) 4, "cuatro", "4", Numero.ColorFicha.Negro, (byte) 19, (byte) 21));
      numeros.Add(new Numero((byte) 21, "veinte y uno", "21", Numero.ColorFicha.Rojo, (byte) 4, (byte) 2));
      numeros.Add(new Numero((byte) 2, "dos", "2", Numero.ColorFicha.Negro, (byte) 21, (byte) 25));
      numeros.Add(new Numero((byte) 25, "veinte y cinco", "25", Numero.ColorFicha.Rojo, (byte) 2, (byte) 17));
      numeros.Add(new Numero((byte) 17, "diez y siete", "17", Numero.ColorFicha.Negro, (byte) 25, (byte) 34));
      numeros.Add(new Numero((byte) 34, "treinta y cuatro", "34", Numero.ColorFicha.Rojo, (byte) 17, (byte) 6));
      numeros.Add(new Numero((byte) 6, "seis", "6", Numero.ColorFicha.Negro, (byte) 34, (byte) 27));
      numeros.Add(new Numero((byte) 27, "veinte y siete", "27", Numero.ColorFicha.Rojo, (byte) 6, (byte) 13));
      numeros.Add(new Numero((byte) 13, "trece", "13", Numero.ColorFicha.Negro, (byte) 27, (byte) 36));
      numeros.Add(new Numero((byte) 36, "treinta y seis", "36", Numero.ColorFicha.Rojo, (byte) 13, (byte) 11));
      numeros.Add(new Numero((byte) 11, "once", "11", Numero.ColorFicha.Negro, (byte) 36, (byte) 30));
      numeros.Add(new Numero((byte) 30, "treinta", "30", Numero.ColorFicha.Rojo, (byte) 11, (byte) 8));
      numeros.Add(new Numero((byte) 8, "ocho", "8", Numero.ColorFicha.Negro, (byte) 30, (byte) 23));
      numeros.Add(new Numero((byte) 23, "veinte y tres", "23", Numero.ColorFicha.Rojo, (byte) 8, (byte) 10));
      numeros.Add(new Numero((byte) 10, "diez", "10", Numero.ColorFicha.Negro, (byte) 23, this.esConDobleCero ? (byte) 37 : (byte) 5));
      if (this.esConDobleCero)
        numeros.Add(new Numero((byte) 37, "doble cero", "00", Numero.ColorFicha.Verde, (byte) 10, (byte) 5));
      numeros.Add(new Numero((byte) 5, "cinco", "5", Numero.ColorFicha.Rojo, this.esConDobleCero ? (byte) 37 : (byte) 10, (byte) 24));
      numeros.Add(new Numero((byte) 24, "veinte y cuatro", "24", Numero.ColorFicha.Negro, (byte) 5, (byte) 16));
      numeros.Add(new Numero((byte) 16, "diez y seis", "16", Numero.ColorFicha.Rojo, (byte) 24, (byte) 33));
      numeros.Add(new Numero((byte) 33, "treinta y tres", "33", Numero.ColorFicha.Negro, (byte) 16, (byte) 1));
      numeros.Add(new Numero((byte) 1, "uno", "1", Numero.ColorFicha.Rojo, (byte) 33, (byte) 20));
      numeros.Add(new Numero((byte) 20, "veinte", "20", Numero.ColorFicha.Negro, (byte) 1, (byte) 14));
      numeros.Add(new Numero((byte) 14, "catorce", "14", Numero.ColorFicha.Rojo, (byte) 20, (byte) 31));
      numeros.Add(new Numero((byte) 31, "treinta y uno", "31", Numero.ColorFicha.Negro, (byte) 14, (byte) 9));
      numeros.Add(new Numero((byte) 9, "nueve", "9", Numero.ColorFicha.Rojo, (byte) 31, (byte) 22));
      numeros.Add(new Numero((byte) 22, "veinte y dos", "22", Numero.ColorFicha.Negro, (byte) 9, (byte) 18));
      numeros.Add(new Numero((byte) 18, "diez y ocho", "18", Numero.ColorFicha.Rojo, (byte) 22, (byte) 29));
      numeros.Add(new Numero((byte) 29, "veinte y nueve", "29", Numero.ColorFicha.Negro, (byte) 18, (byte) 7));
      numeros.Add(new Numero((byte) 7, "siete", "7", Numero.ColorFicha.Rojo, (byte) 29, (byte) 28));
      numeros.Add(new Numero((byte) 28, "veinte y ocho", "28", Numero.ColorFicha.Negro, (byte) 7, (byte) 12));
      numeros.Add(new Numero((byte) 12, "doce", "12", Numero.ColorFicha.Rojo, (byte) 28, (byte) 35));
      numeros.Add(new Numero((byte) 35, "treinta y cinco", "35", Numero.ColorFicha.Negro, (byte) 12, (byte) 3));
      numeros.Add(new Numero((byte) 3, "tres", "3", Numero.ColorFicha.Rojo, (byte) 35, (byte) 26));
      numeros.Add(new Numero((byte) 26, "veinte y seis", "26", Numero.ColorFicha.Negro, (byte) 3, (byte) 0));
      Plato.NumerosJuegoRuleta = numeros;
      return numeros;
    }

    public Numero ObtenerNumeroPorValor(byte valorNumero)
    {
      Numero numero1 = (Numero) null;
      if (Plato.NumerosJuegoRuleta != null)
      {
        foreach (Numero numero2 in (List<Numero>) Plato.NumerosJuegoRuleta)
        {
          if ((int) numero2.Valor == (int) valorNumero)
          {
            numero1 = numero2;
            break;
          }
        }
      }
      return numero1;
    }

    public class Sector
    {
      private string nombre = string.Empty;
      private Numero desdeNumero;
      private Numero hastaNumero;
      private List<Numero> numeros;

      public Sector()
      {
      }

      public Sector(string nombre)
      {
        this.nombre = nombre;
      }

      public Sector(Numero desdeNumero, Numero hastaNumero)
      {
        this.desdeNumero = desdeNumero;
        this.hastaNumero = hastaNumero;
        this.armarRangoSector();
      }

      public Sector(string nombre, Numero desdeNumero, Numero hastaNumero)
        : this(desdeNumero, hastaNumero)
      {
        this.nombre = nombre;
      }

      public string Nombre
      {
        get
        {
          return this.nombre;
        }
      }

      public Numero DesdeNumero
      {
        get
        {
          return this.desdeNumero;
        }
      }

      public Numero HastaNumero
      {
        get
        {
          return this.hastaNumero;
        }
      }

      public List<Numero> Numeros
      {
        get
        {
          return this.numeros;
        }
      }

      public bool ContieneNumero(byte numero)
      {
        bool flag = false;
        if (this.numeros != null && this.numeros.Count > 0)
        {
          foreach (Numero numero1 in this.numeros)
          {
            if ((int) numero1.Valor == (int) numero)
            {
              flag = true;
              break;
            }
          }
        }
        return flag;
      }

      private void armarRangoSector()
      {
        if (Plato.NumerosJuegoRuleta == null || Plato.NumerosJuegoRuleta.Count <= 0 || (this.desdeNumero == null || this.hastaNumero == null))
          return;
        Numero numero = this.desdeNumero;
        this.numeros = new List<Numero>();
        for (int index = 0; index < Plato.NumerosJuegoRuleta.Count; ++index)
        {
          this.numeros.Add(numero);
          numero = numero.NumeroSiguientePlato;
          if ((int) numero.NumeroAnteriorPlato.Valor == (int) this.hastaNumero.Valor)
            break;
        }
      }
    }
  }
}
