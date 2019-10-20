// Decompiled with JetBrains decompiler
// Type: DASYS.DAL.Connections
// Assembly: DAL, Version=3.4.5.5, Culture=neutral, PublicKeyToken=null
// MVID: D8AEA125-C248-431D-9EBF-103DF8547D67
// Assembly location: C:\Program Files (x86)\NAPSA\Colector III\DAL.dll

using System.Collections.Generic;

namespace DASYS.DAL
{
  public class Connections : List<Connection>
  {
    public Connection this[string name]
    {
      get
      {
        foreach (Connection connection in (List<Connection>) this)
        {
          if (connection.ConnectionName == name)
            return connection;
        }
        return (Connection) null;
      }
    }
  }
}
