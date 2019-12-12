// Decompiled with JetBrains decompiler
// Type: DASYS.Framework.NetSender
// Assembly: Framework, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1B503700-E29D-4D7A-BD70-519F036595D0
// Assembly location: C:\Program Files (x86)\NAPSA\Colector III\Framework.dll

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace DASYS.Framework
{
  public class NetSender
  {
    private byte[] m_byBuff = new byte[256];
    private string _recibido = string.Empty;
    private Socket m_sock;
    private NetConexion _netConexion;

    private event AddMessage m_AddMessage;

    public NetConexion NetConexion
    {
      get
      {
        return this._netConexion;
      }
      set
      {
        this._netConexion = value;
      }
    }

    public string Recibido
    {
      get
      {
        return this._recibido;
      }
      set
      {
        this._recibido = value;
      }
    }

    public NetSender(NetConexion netConexion)
    {
      this.m_AddMessage = new AddMessage(this.OnAddMessage);
      this.NetConexion = netConexion;
    }

    public void Conectar()
    {
      try
      {
        if (this.m_sock != null && this.m_sock.Connected)
        {
          this.m_sock.Shutdown(SocketShutdown.Both);
          Thread.Sleep(10);
          this.m_sock.Close();
        }
        this.m_sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(this.NetConexion.IP), this.NetConexion.Puerto);
        this.m_sock.Blocking = false;
        AsyncCallback callback = new AsyncCallback(this.OnConnect);
        this.m_sock.BeginConnect((EndPoint) ipEndPoint, callback, (object) this.m_sock);
      }
      catch (Exception ex)
      {
        throw new Exception("Server Connect failed!: " + ex.Message);
      }
    }

    public void Desconectar()
    {
      if (this.m_sock == null || !this.m_sock.Connected)
        return;
      this.m_sock.Shutdown(SocketShutdown.Both);
      this.m_sock.Close();
    }

    public void Enviar(string mensaje)
    {
      if (this.m_sock != null)
      {
        if (this.m_sock.Connected)
        {
          try
          {
            byte[] bytes = Encoding.UTF8.GetBytes(mensaje.ToCharArray());
            this.m_sock.Send(bytes, bytes.Length, SocketFlags.None);
            return;
          }
          catch (Exception ex)
          {
            throw new Exception("Send Message Failed: " + ex.Message);
          }
        }
      }
      throw new Exception("Must be connected to Send a message");
    }

    public void OnConnect(IAsyncResult ar)
    {
      Socket asyncState = (Socket) ar.AsyncState;
      try
      {
        if (!asyncState.Connected)
          throw new Exception("Connect Failed!");
        this.SetupRecieveCallback(asyncState);
      }
      catch (Exception ex)
      {
        throw new Exception("Unusual error during Connect: " + ex.Message);
      }
    }

    public void OnRecievedData(IAsyncResult ar)
    {
      Socket asyncState = (Socket) ar.AsyncState;
      try
      {
        int count = asyncState.EndReceive(ar);
        if (count > 0)
        {
          this.OnAddMessage(Encoding.UTF8.GetString(this.m_byBuff, 0, count));
          this.SetupRecieveCallback(asyncState);
        }
        else
        {
          Console.WriteLine("Client {0}, disconnected", (object) asyncState.RemoteEndPoint);
          asyncState.Shutdown(SocketShutdown.Both);
          asyncState.Close();
        }
      }
      catch (Exception ex)
      {
        throw new Exception("Unusual error druing Recieve:" + ex.Message);
      }
    }

    public void OnAddMessage(string sMessage)
    {
      this.Recibido += sMessage;
    }

    public void SetupRecieveCallback(Socket sock)
    {
      try
      {
        AsyncCallback callback = new AsyncCallback(this.OnRecievedData);
        sock.BeginReceive(this.m_byBuff, 0, this.m_byBuff.Length, SocketFlags.None, callback, (object) sock);
      }
      catch (Exception ex)
      {
        throw new Exception("Setup Recieve Callback failed: " + ex.Message);
      }
    }
  }
}
