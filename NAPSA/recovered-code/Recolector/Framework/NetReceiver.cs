// Decompiled with JetBrains decompiler
// Type: DASYS.Framework.NetReceiver
// Assembly: Framework, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1B503700-E29D-4D7A-BD70-519F036595D0
// Assembly location: C:\Program Files (x86)\NAPSA\Colector III\Framework.dll

using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace DASYS.Framework
{
  public class NetReceiver
  {
    public string recibido = string.Empty;
    private ArrayList m_aryClients = new ArrayList();
    private Socket listener;

    public NetReceiver(NetConexion netConexion)
    {
      IPAddress[] ipAddressArray = (IPAddress[]) null;
      string hostNameOrAddress = "";
      try
      {
        hostNameOrAddress = Dns.GetHostName();
        ipAddressArray = Dns.GetHostEntry(hostNameOrAddress).AddressList;
      }
      catch (Exception ex)
      {
        Console.WriteLine("Error trying to get local address {0} ", (object) ex.Message);
      }
      if (ipAddressArray == null || ipAddressArray.Length < 1)
      {
        Console.WriteLine("Unable to get local address");
      }
      else
      {
        Console.WriteLine("Listening on : [{0}] {1}:{2}", (object) hostNameOrAddress, (object) ipAddressArray[0], (object) netConexion.Puerto);
        this.listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        this.listener.Bind((EndPoint) new IPEndPoint(ipAddressArray[0], netConexion.Puerto));
        this.listener.Listen(10);
        this.listener.BeginAccept(new AsyncCallback(this.OnConnectRequest), (object) this.listener);
        Console.WriteLine("Press Enter to exit");
        Console.ReadLine();
        Console.WriteLine("OK that does it! Screw you guys I'm going home...");
      }
    }

    public void OnConnectRequest(IAsyncResult ar)
    {
      Socket asyncState = (Socket) ar.AsyncState;
      this.NewConnection(asyncState.EndAccept(ar));
      asyncState.BeginAccept(new AsyncCallback(this.OnConnectRequest), (object) asyncState);
    }

    public void NewConnection(Socket sockClient)
    {
      SocketChatClient socketChatClient = new SocketChatClient(sockClient);
      this.m_aryClients.Add((object) socketChatClient);
      Console.WriteLine("Client {0}, joined", (object) socketChatClient.Sock.RemoteEndPoint);
      string str = "Inicio de sesión ";
      DateTime? nullable = Utils.RelojInterno.CalcularFechaHoraServidor();
      if (nullable.HasValue)
        str += nullable.Value.ToString("dd/MM/yyyy HH:mm:ss");
      byte[] bytes = Encoding.UTF8.GetBytes((str + "\n\r").ToCharArray());
      socketChatClient.Sock.Send(bytes, bytes.Length, SocketFlags.None);
      socketChatClient.SetupRecieveCallback(this);
    }

    public void Desconectar()
    {
      try
      {
        this.listener.Close();
        GC.Collect();
        GC.WaitForPendingFinalizers();
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message);
      }
    }

    public void OnRecievedData(IAsyncResult ar)
    {
      SocketChatClient asyncState = (SocketChatClient) ar.AsyncState;
      byte[] recievedData = asyncState.GetRecievedData(ar);
      if (recievedData.Length < 1)
      {
        Console.WriteLine("Client {0}, disconnected", (object) asyncState.Sock.RemoteEndPoint);
        asyncState.Sock.Close();
        this.m_aryClients.Remove((object) asyncState);
      }
      else
      {
        foreach (SocketChatClient aryClient in this.m_aryClients)
        {
          try
          {
            aryClient.Sock.Send(recievedData);
          }
          catch
          {
            Console.WriteLine("Send to client {0} failed", (object) asyncState.Sock.RemoteEndPoint);
            aryClient.Sock.Close();
            this.m_aryClients.Remove((object) asyncState);
            return;
          }
        }
        asyncState.SetupRecieveCallback(this);
      }
    }
  }
}
