// Decompiled with JetBrains decompiler
// Type: DASYS.Framework.Accesorios
// Assembly: Framework, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1B503700-E29D-4D7A-BD70-519F036595D0
// Assembly location: C:\Program Files (x86)\NAPSA\Colector III\Framework.dll

using System;
using System.Collections;
using System.Xml.Serialization;

namespace DASYS.Framework
{
  public class Accesorios
  {
    [XmlRoot(ElementName = "Parte", IsNullable = false)]
    public class Parte
    {
      private string clave = string.Empty;
      private object valor;

      public Parte()
      {
      }

      public Parte(string clave)
      {
        this.clave = clave;
      }

      public Parte(string clave, object valor)
      {
        this.clave = clave;
        this.valor = valor;
      }

      public string Clave
      {
        get
        {
          return this.clave;
        }
        set
        {
          this.clave = value;
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
    }

    [XmlRoot(ElementName = "Partes", IsNullable = false)]
    public class Partes : CollectionBase
    {
      public Accesorios.Parte this[int index]
      {
        get
        {
          return (Accesorios.Parte) this.List[index];
        }
        set
        {
          this.List[index] = (object) value;
        }
      }

      public object this[string clave]
      {
        get
        {
          foreach (Accesorios.Parte parte in (IEnumerable) this.List)
          {
            if (parte.Clave == clave)
              return parte.Valor;
          }
          return (object) null;
        }
        set
        {
          if (!this.List.Contains((object) clave))
            throw new Exception("El valor no existe");
          this[clave] = value;
        }
      }

      public int Add(Accesorios.Parte value)
      {
        return this.List.Add((object) value);
      }

      public int Add(string clave)
      {
        return this.List.Add((object) new Accesorios.Parte(clave));
      }

      public int Add(string clave, object valor)
      {
        return this.List.Add((object) new Accesorios.Parte(clave, valor));
      }

      public int IndexOf(Accesorios.Parte value)
      {
        return this.List.IndexOf((object) value);
      }

      public void Insert(int index, Accesorios.Parte value)
      {
        this.List.Insert(index, (object) value);
      }

      public void Remove(Accesorios.Parte value)
      {
        this.List.Remove((object) value);
      }

      public bool Contains(Accesorios.Parte value)
      {
        return this.List.Contains((object) value);
      }

      public bool Contains(string clave)
      {
        foreach (Accesorios.Parte parte in (IEnumerable) this.List)
        {
          if (parte.Clave == clave)
            return true;
        }
        return false;
      }
    }
  }
}
