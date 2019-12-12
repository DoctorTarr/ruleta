// Decompiled with JetBrains decompiler
// Type: DASYS.Recolector.BLL.Producto
// Assembly: BLL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 684D872A-58E1-4C16-9B83-6ABA379FCE9D
// Assembly location: C:\Program Files (x86)\NAPSA\Colector III\BLL.dll

using DASYS.Framework;

namespace DASYS.Recolector.BLL
{
  public static class Producto
  {
    public static string Licence;
    public static string Activation;

    public static bool VerificarActivacion()
    {
      bool flag = false;
      try
      {
        Producto.Licence = Utils.Datos.NullToString(Seguridad.Registry.LeerRegistro(Seguridad.Registry.RegistryKeys.HKEY_LOCAL_MACHINE, "napsa", "display2", "product", "licence"));
        Producto.Activation = Utils.Datos.NullToString(Seguridad.Registry.LeerRegistro(Seguridad.Registry.RegistryKeys.HKEY_LOCAL_MACHINE, "napsa", "display2", "product", "activation"));
        if (!string.IsNullOrEmpty(Producto.Licence))
        {
          if (!string.IsNullOrEmpty(Producto.Activation))
            flag = Seguridad.Activacion.VerificarLicencia(Producto.Licence, Producto.Activation, "Néstor Pastor");
        }
      }
      catch
      {
        throw;
      }
      return flag;
    }
  }
}
