// Decompiled with JetBrains decompiler
// Type: ACL.Productos
// Assembly: ACL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A55A6FFB-1772-4E30-9250-F6DDD06AF421
// Assembly location: C:\Program Files (x86)\NAPSA\Colector III\ACL.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace ACLBase
{
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0")]
  [CompilerGenerated]
  [DebuggerNonUserCode]
  internal class Productos
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal Productos()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (object.ReferenceEquals((object) Productos.resourceMan, (object) null))
          Productos.resourceMan = new ResourceManager("ACL.Productos", typeof (Productos).Assembly);
        return Productos.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get
      {
        return Productos.resourceCulture;
      }
      set
      {
        Productos.resourceCulture = value;
      }
    }

    internal static Bitmap OS
    {
      get
      {
        return (Bitmap) Productos.ResourceManager.GetObject(nameof (OS), Productos.resourceCulture);
      }
    }

    internal static Bitmap OSmini
    {
      get
      {
        return (Bitmap) Productos.ResourceManager.GetObject(nameof (OSmini), Productos.resourceCulture);
      }
    }

    internal static Bitmap SCATmini
    {
      get
      {
        return (Bitmap) Productos.ResourceManager.GetObject(nameof (SCATmini), Productos.resourceCulture);
      }
    }
  }
}
