// Decompiled with JetBrains decompiler
// Type: DASYS.Framework.SocketChatClient
// Assembly: Framework, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1B503700-E29D-4D7A-BD70-519F036595D0
// Assembly location: C:\Program Files (x86)\NAPSA\Colector III\Framework.dll

using System;
using System.Net.Sockets;

namespace DASYS.Framework
{
  internal class SocketChatClient
  {
    private byte[] m_byBuff = new byte[50];
    private Socket m_sock;

    public SocketChatClient(Socket sock)
    {
      this.m_sock = sock;
    }

    public Socket Sock
    {
      get
      {
        return this.m_sock;
      }
    }

    public void SetupRecieveCallback(NetReceiver app)
    {
      try
      {
        this.m_sock.BeginReceive(this.m_byBuff, 0, this.m_byBuff.Length, SocketFlags.None, new AsyncCallback(app.OnRecievedData), (object) this);
      }
      catch (Exception ex)
      {
        Console.WriteLine("Recieve callback setup failed! {0}", (object) ex.Message);
      }
    }

    public byte[] GetRecievedData(IAsyncResult ar)
    {
      int length = 0;
      try
      {
        length = this.m_sock.EndReceive(ar);
      }
      catch
      {
      }
      byte[] numArray = new byte[length];
      Array.Copy((Array) this.m_byBuff, (Array) numArray, length);
      return numArray;
    }
  }
}
