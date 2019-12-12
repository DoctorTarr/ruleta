// Decompiled with JetBrains decompiler
// Type: DASYS.DAL.Connectivity
// Assembly: DAL, Version=3.4.5.5, Culture=neutral, PublicKeyToken=null
// MVID: D8AEA125-C248-431D-9EBF-103DF8547D67
// Assembly location: C:\Program Files (x86)\NAPSA\Colector III\DAL.dll

namespace DASYS.DAL
{
  public class Connectivity
  {
    public bool VerificarConexionInicio;
    public string DSNName;
    public string ConnectionName;
    public string ServerName;
    public string DataBaseName;
    public DataBaseType DataBaseType;
    public bool PersistSecurityInfo;
    public bool IntegratedSecurity;
    public int Timeout;
    public string UserName;
    public string Password;
    public string ParameterPrefix;
    public int Port;

    public Connectivity()
    {
    }

    public Connectivity(
      DataBaseType dataBaseType,
      string connectionName,
      string serverName,
      string dataBaseName,
      bool persistSecurityInfo,
      bool integratedSecurity,
      int timeout,
      string userName,
      string password,
      int port)
    {
      this.DSNName = (string) null;
      this.VerificarConexionInicio = false;
      this.DataBaseType = dataBaseType;
      this.ConnectionName = connectionName;
      this.ServerName = serverName;
      this.DataBaseName = dataBaseName;
      this.PersistSecurityInfo = persistSecurityInfo;
      this.IntegratedSecurity = integratedSecurity;
      this.Timeout = timeout;
      this.UserName = userName;
      this.Password = password;
      this.ParameterPrefix = string.Empty;
      this.Port = port;
    }

    public Connectivity(
      string connectionName,
      string dsnName,
      string userName,
      string password,
      DataBaseType dataBaseType)
    {
      this.DSNName = dsnName;
      this.UserName = userName;
      this.Password = password;
      this.DataBaseType = dataBaseType;
      this.ConnectionName = connectionName;
      this.DataBaseName = string.Empty;
      this.VerificarConexionInicio = false;
      this.ServerName = string.Empty;
      this.PersistSecurityInfo = false;
      this.IntegratedSecurity = false;
      this.Timeout = 30;
      this.ParameterPrefix = string.Empty;
      this.Port = 0;
    }
  }
}
