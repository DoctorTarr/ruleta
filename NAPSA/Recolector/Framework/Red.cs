// Decompiled with JetBrains decompiler
// Type: DASYS.Framework.Red
// Assembly: Framework, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1B503700-E29D-4D7A-BD70-519F036595D0
// Assembly location: C:\Program Files (x86)\NAPSA\Colector III\Framework.dll

using System.Collections.Generic;
using System.Net;

namespace DASYS.Framework
{
  public static class Red
  {
    public static List<string> ObtenerIP()
    {
      List<string> stringList = new List<string>();
      try
      {
        foreach (IPAddress address in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
          stringList.Add(address.ToString());
      }
      catch
      {
        throw;
      }
      return stringList;
    }
  }
}
