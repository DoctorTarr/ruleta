// Decompiled with JetBrains decompiler
// Type: DASYS.DAL.Connectivities
// Assembly: DAL, Version=3.4.5.5, Culture=neutral, PublicKeyToken=null
// MVID: D8AEA125-C248-431D-9EBF-103DF8547D67
// Assembly location: C:\Program Files (x86)\NAPSA\Colector III\DAL.dll

using System;
using System.Collections.Generic;

namespace DASYS.DAL
{
  public class Connectivities : List<Connectivity>
  {
    public Connectivity this[string name]
    {
      get
      {
        foreach (Connectivity connectivity in (List<Connectivity>) this)
        {
          if (connectivity.ConnectionName.Equals(name, StringComparison.OrdinalIgnoreCase))
            return connectivity;
        }
        return (Connectivity) null;
      }
    }
  }
}
