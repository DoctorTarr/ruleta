// Decompiled with JetBrains decompiler
// Type: DASYS.Recolector.BLL.IResultadosPaquete
// Assembly: BLL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 684D872A-58E1-4C16-9B83-6ABA379FCE9D
// Assembly location: C:\Program Files (x86)\NAPSA\Colector III\BLL.dll

namespace DASYS.Recolector.BLL
{
  public interface IResultadosPaquete
  {
    ProtocoloNAPSA.ProtocoloTipoPaquete TipoPaquete { get; }

    string CadenaOriginal { get; set; }

    IResultadosPaquete Parsear();
  }
}
