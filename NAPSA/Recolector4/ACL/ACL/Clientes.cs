// Decompiled with JetBrains decompiler
// Type: ACL.Clientes
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
  [CompilerGenerated]
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0")]
  [DebuggerNonUserCode]
  internal class Clientes
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal Clientes()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (object.ReferenceEquals((object) Clientes.resourceMan, (object) null))
          Clientes.resourceMan = new ResourceManager("ACL.Clientes", typeof (Clientes).Assembly);
        return Clientes.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get
      {
        return Clientes.resourceCulture;
      }
      set
      {
        Clientes.resourceCulture = value;
      }
    }

    internal static Bitmap CAL
    {
      get
      {
        return (Bitmap) Clientes.ResourceManager.GetObject(nameof (CAL), Clientes.resourceCulture);
      }
    }

    internal static Bitmap CALmini
    {
      get
      {
        return (Bitmap) Clientes.ResourceManager.GetObject(nameof (CALmini), Clientes.resourceCulture);
      }
    }

    internal static Bitmap DASYS
    {
      get
      {
        return (Bitmap) Clientes.ResourceManager.GetObject(nameof (DASYS), Clientes.resourceCulture);
      }
    }

    internal static Bitmap DASYSmini
    {
      get
      {
        return (Bitmap) Clientes.ResourceManager.GetObject(nameof (DASYSmini), Clientes.resourceCulture);
      }
    }

    internal static Bitmap TEACSACLIENTE
    {
      get
      {
        return (Bitmap) Clientes.ResourceManager.GetObject(nameof (TEACSACLIENTE), Clientes.resourceCulture);
      }
    }

    internal static Bitmap TEACSACLIENTEmini
    {
      get
      {
        return (Bitmap) Clientes.ResourceManager.GetObject(nameof (TEACSACLIENTEmini), Clientes.resourceCulture);
      }
    }
  }
}
