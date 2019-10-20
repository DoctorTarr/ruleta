// Decompiled with JetBrains decompiler
// Type: DASYS.Framework.Parametros
// Assembly: Framework, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1B503700-E29D-4D7A-BD70-519F036595D0
// Assembly location: C:\Program Files (x86)\NAPSA\Colector III\Framework.dll

using System.Collections.Generic;

namespace DASYS.Framework
{
  public class Parametros : List<Parametro>
  {
    public Parametro this[string name]
    {
      get
      {
        foreach (Parametro parametro in (List<Parametro>) this)
        {
          if (parametro.Valor == (object) name)
            return parametro;
        }
        return (Parametro) null;
      }
    }
  }
}
