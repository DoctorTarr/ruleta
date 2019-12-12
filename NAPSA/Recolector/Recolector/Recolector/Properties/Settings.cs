// Decompiled with JetBrains decompiler
// Type: Recolector.Properties.Settings
// Assembly: Recolector, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0D03609E-ECAA-4078-98A3-46CE568862AA
// Assembly location: C:\Program Files (x86)\NAPSA\Colector III\Recolector.exe

using System.CodeDom.Compiler;
using System.Configuration;
using System.Runtime.CompilerServices;

namespace Recolector.Properties
{
  [CompilerGenerated]
  [GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "9.0.0.0")]
  internal sealed class Settings : ApplicationSettingsBase
  {
    private static Settings defaultInstance = (Settings) SettingsBase.Synchronized((SettingsBase) new Settings());

    public static Settings Default
    {
      get
      {
        return Settings.defaultInstance;
      }
    }
  }
}
