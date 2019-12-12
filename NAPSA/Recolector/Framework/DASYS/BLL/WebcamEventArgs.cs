// Decompiled with JetBrains decompiler
// Type: DASYS.BLL.WebcamEventArgs
// Assembly: Framework, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1B503700-E29D-4D7A-BD70-519F036595D0
// Assembly location: C:\Program Files (x86)\NAPSA\Colector III\Framework.dll

using System;
using System.Drawing;

namespace DASYS.BLL
{
  public class WebcamEventArgs : EventArgs
  {
    private Image m_Image;
    private ulong m_FrameNumber;

    public Image WebCamImage
    {
      get
      {
        return this.m_Image;
      }
      set
      {
        this.m_Image = value;
      }
    }

    public ulong FrameNumber
    {
      get
      {
        return this.m_FrameNumber;
      }
      set
      {
        this.m_FrameNumber = value;
      }
    }
  }
}
