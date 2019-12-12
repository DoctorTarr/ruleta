// Decompiled with JetBrains decompiler
// Type: DASYS.Recolector.BLL.Mesa
// Assembly: BLL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 684D872A-58E1-4C16-9B83-6ABA379FCE9D
// Assembly location: C:\Program Files (x86)\NAPSA\Colector III\BLL.dll

using DASYS.DAL;
using System.Data;

namespace DASYS.Recolector.BLL
{
  public static class Mesa
  {
    public static int Numero;
    public static float ApuestaMinima;
    public static float ApuestaMaxima;

    public static bool Guardar()
    {
      int num;
      try
      {
        QueryEngine query = new QueryEngine();
        query.QueryName = "mesaSave";
        query.AddNames("numero", "minimo", "maximo");
        query.AddTypes(DbType.Int32, DbType.Single, DbType.Single);
        query.AddValues((object) Mesa.Numero, (object) Mesa.ApuestaMinima, (object) Mesa.ApuestaMaxima);
        num = Common.oConexiones[0].DbExecuteNonQuery(query);
      }
      catch
      {
        throw;
      }
      return num > 0;
    }

    public static bool Cargar()
    {
      int num;
      try
      {
        QueryEngine query = new QueryEngine();
        query.QueryName = "mesaSelect";
        query.AddNames("numero", "minimo", "maximo");
        query.AddTypes(DbType.Int32, DbType.Single, DbType.Single);
        query.AddValues((object) Mesa.Numero, (object) Mesa.ApuestaMinima, (object) Mesa.ApuestaMaxima);
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
