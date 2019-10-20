// Decompiled with JetBrains decompiler
// Type: Recolector.Properties.Resources
// Assembly: Recolector, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0D03609E-ECAA-4078-98A3-46CE568862AA
// Assembly location: C:\Program Files (x86)\NAPSA\Colector III\Recolector.exe

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Recolector.Properties
{
  [DebuggerNonUserCode]
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0")]
  [CompilerGenerated]
  internal class Resources
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal Resources()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (object.ReferenceEquals((object) Recolector.Properties.Resources.resourceMan, (object) null))
          Recolector.Properties.Resources.resourceMan = new ResourceManager("Recolector.Properties.Resources", typeof (Recolector.Properties.Resources).Assembly);
        return Recolector.Properties.Resources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get
      {
        return Recolector.Properties.Resources.resourceCulture;
      }
      set
      {
        Recolector.Properties.Resources.resourceCulture = value;
      }
    }

    internal static Bitmap miniAbrirCarpeta
    {
      get
      {
        return (Bitmap) Recolector.Properties.Resources.ResourceManager.GetObject(nameof (miniAbrirCarpeta), Recolector.Properties.Resources.resourceCulture);
      }
    }
  }
}
