// Decompiled with JetBrains decompiler
// Type: DASYS.Recolector.BLL.Numeros
// Assembly: BLL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 684D872A-58E1-4C16-9B83-6ABA379FCE9D
// Assembly location: C:\Program Files (x86)\NAPSA\Colector III\BLL.dll

using System.Collections.Generic;

namespace DASYS.Recolector.BLL
{
  public class Numeros : List<Numero>
  {
    public int ObtenerIndice(byte numeroValor)
    {
      int num = -1;
      foreach (Numero numero in (List<Numero>) this)
      {
        ++num;
        if ((int) numero.Valor == (int) numeroValor)
          break;
      }
      return num;
    }
  }
}
