// Decompiled with JetBrains decompiler
// Type: ACL.Licenciatarios
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
  [DebuggerNonUserCode]
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0")]
  [CompilerGenerated]
  internal class Licenciatarios
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal Licenciatarios()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (object.ReferenceEquals((object) Licenciatarios.resourceMan, (object) null))
          Licenciatarios.resourceMan = new ResourceManager("ACL.Licenciatarios", typeof (Licenciatarios).Assembly);
        return Licenciatarios.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get
      {
        return Licenciatarios.resourceCulture;
      }
      set
      {
        Licenciatarios.resourceCulture = value;
      }
    }

    internal static Bitmap DASYS
    {
      get
      {
        return (Bitmap) Licenciatarios.ResourceManager.GetObject(nameof (DASYS), Licenciatarios.resourceCulture);
      }
    }

    internal static Bitmap DASYSmini
    {
      get
      {
        return (Bitmap) Licenciatarios.ResourceManager.GetObject(nameof (DASYSmini), Licenciatarios.resourceCulture);
      }
    }

    internal static Bitmap PST
    {
      get
      {
        return (Bitmap) Licenciatarios.ResourceManager.GetObject(nameof (PST), Licenciatarios.resourceCulture);
      }
    }

    internal static Bitmap PSTmini
    {
      get
      {
        return (Bitmap) Licenciatarios.ResourceManager.GetObject(nameof (PSTmini), Licenciatarios.resourceCulture);
      }
    }

    internal static Bitmap TEACSA
    {
      get
      {
        return (Bitmap) Licenciatarios.ResourceManager.GetObject(nameof (TEACSA), Licenciatarios.resourceCulture);
      }
    }

    internal static Bitmap TEACSAmini
    {
      get
      {
        return (Bitmap) Licenciatarios.ResourceManager.GetObject(nameof (TEACSAmini), Licenciatarios.resourceCulture);
      }
    }
  }
}
