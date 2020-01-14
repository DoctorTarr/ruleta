// Decompiled with JetBrains decompiler
// Type: DASYS.Framework.ParametrosOE
// Assembly: Framework, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1B503700-E29D-4D7A-BD70-519F036595D0
// Assembly location: C:\Program Files (x86)\NAPSA\Colector III\Framework.dll

using System;
using System.Collections.Generic;

namespace DASYS.Framework
{
  public class ParametrosOE : List<ParametroOE>
  {
    public ParametroOE this[string clave]
    {
      get
      {
        foreach (ParametroOE parametroOe in (List<ParametroOE>) this)
        {
          if (parametroOe.Clave.Equals(clave, StringComparison.OrdinalIgnoreCase))
            return parametroOe;
        }
        return (ParametroOE) null;
      }
    }
  }
}
