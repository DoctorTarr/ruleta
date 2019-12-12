// Decompiled with JetBrains decompiler
// Type: DASYS.Recolector.BLL.Status
// Assembly: BLL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 684D872A-58E1-4C16-9B83-6ABA379FCE9D
// Assembly location: C:\Program Files (x86)\NAPSA\Colector III\BLL.dll

using DASYS.DAL;
using System.Data;

namespace DASYS.Recolector.BLL
{
  public static class Status
  {
    public static bool EscribirEnBase(
      byte statusNumero,
      byte statusEstado,
      byte statusVelocidad,
      byte statusSentidoGiro,
      byte statusError)
    {
      int num;
      try
      {
        QueryEngine query = new QueryEngine();
        query.QueryName = "statusInsert";
        query.AddNames(nameof (statusNumero), nameof (statusEstado), nameof (statusVelocidad), nameof (statusSentidoGiro), nameof (statusError));
        query.AddTypes(DbType.Byte, DbType.Byte, DbType.Byte, DbType.Byte, DbType.Byte);
        query.AddValues((object) statusNumero, (object) statusEstado, (object) statusVelocidad, (object) statusSentidoGiro, (object) statusError);
        num = Common.oConexiones[0].DbExecuteNonQuery(query);
      }
      catch
      {
        throw;
      }
      return num > 0;
    }
  }
}
