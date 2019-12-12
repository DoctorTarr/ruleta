// Decompiled with JetBrains decompiler
// Type: DASYS.Framework.NetCommunication
// Assembly: Framework, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1B503700-E29D-4D7A-BD70-519F036595D0
// Assembly location: C:\Program Files (x86)\NAPSA\Colector III\Framework.dll

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace DASYS.Framework
{
  public class NetCommunication
  {
    private Socket socket;
    private IPAddress ip;
    private int port;

    public IPAddress IP
    {
      get
      {
        return this.ip;
      }
      set
      {
        this.ip = value;
      }
    }

    public int Port
    {
      get
      {
        return this.port;
      }
      set
      {
        this.port = value;
      }
    }

    public Socket Socket
    {
      get
      {
        return this.socket;
      }
      set
      {
        this.socket = value;
      }
    }

    public NetCommunication(IPAddress ip, int port)
    {
      this.ip = ip;
      this.port = port;
    }

    public bool Conectar()
    {
      bool flag = false;
      try
      {
        this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint ipEndPoint = new IPEndPoint(this.ip, this.port);
        this.socket.ReceiveTimeout = 10000;
        this.socket.Connect((EndPoint) ipEndPoint);
        if (this.socket.Connected)
          flag = true;
      }
      catch (SocketException ex)
      {
        if (ex.ErrorCode != 10061)
          throw;
      }
      return flag;
    }

    public bool Enviar(string message)
    {
      try
      {
        this.socket.Send(Encoding.ASCII.GetBytes(message));
        return true;
      }
      catch (SocketException ex)
      {
        throw;
      }
    }

    public string Recibir()
    {
      try
      {
        byte[] numArray = new byte[1024];
        int byteCount = this.socket.Receive(numArray);
        char[] chars = new char[byteCount];
        Encoding.UTF8.GetDecoder().GetChars(numArray, 0, byteCount, chars, 0);
        return new string(chars);
      }
      catch (SocketException ex)
      {
        if (ex.ErrorCode == 10035)
        {
          this.socket.Close();
          this.socket = (Socket) null;
          throw;
        }
        else
          throw;
      }
    }

    public bool Cerrar()
    {
      try
      {
        this.socket.Close();
        return true;
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    ~NetCommunication()
    {
      if (this.socket == null)
        return;
      if (!this.socket.IsBound)
        return;
      try
      {
        this.socket.Close();
        this.socket = (Socket) null;
      }
      catch (Exception ex)
      {
      }
    }
  }
}
