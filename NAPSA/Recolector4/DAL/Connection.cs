// Decompiled with JetBrains decompiler
// Type: DASYS.DAL.Connection
// Assembly: DAL, Version=3.4.5.5, Culture=neutral, PublicKeyToken=null
// MVID: D8AEA125-C248-431D-9EBF-103DF8547D67
// Assembly location: C:\Program Files (x86)\NAPSA\Colector III\DAL.dll

using System;
using System.Data;
using System.Data.Common;

namespace DASYS.DAL
{
  public class Connection
  {
    private string connectionString = string.Empty;
    private string connectionName = string.Empty;
    private DateTime? ultimaConexionCorrecta = new DateTime?();
    private bool? estadoUltimaConexion = new bool?();
    private Connectivity connectivity;
    private DbTransaction transaccion;
    private DbConnection MyDbConnection;
    private DbProviderFactory dataFactory;

    public Connection()
    {
    }

    public Connection(Connectivity connInfo)
    {
      this.Connectivity = connInfo;
      this.connectionName = connInfo.ConnectionName;
      try
      {
        if (string.IsNullOrEmpty(connInfo.DSNName))
        {
          switch (connInfo.DataBaseType)
          {
            case DataBaseType.SQL:
              this.ConnectionString = string.Format("Persist Security Info={0};Integrated Security={1};Initial Catalog={2};Data Source={3};Connect Timeout={4};user id={5}; password={6}; ", (object) connInfo.PersistSecurityInfo.ToString(), (object) connInfo.IntegratedSecurity, (object) connInfo.DataBaseName, (object) connInfo.ServerName, (object) connInfo.Timeout.ToString(), (object) connInfo.UserName, (object) connInfo.Password);
              this.dataFactory = DbProviderFactories.GetFactory("System.Data.SqlClient");
              break;
            case DataBaseType.MySQL:
              if (connInfo.Port == 0)
                this.ConnectionString = string.Format("Database={0};Data Source={1};User ID={2};Password={3}", (object) connInfo.DataBaseName, (object) connInfo.ServerName, (object) connInfo.UserName, (object) connInfo.Password);
              else
                this.ConnectionString = string.Format("Database={0};Data Source={1};User ID={2};Password={3};Port={4}", (object) connInfo.DataBaseName, (object) connInfo.ServerName, (object) connInfo.UserName, (object) connInfo.Password, (object) connInfo.Port);
              this.dataFactory = DbProviderFactories.GetFactory("MySql.Data.MySqlClient");
              break;
            case DataBaseType.ACCESS:
              this.ConnectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data source={0};Password={1}", (object) connInfo.DataBaseName, (object) connInfo.Password);
              this.dataFactory = DbProviderFactories.GetFactory("System.Data.OleDb");
              break;
            default:
              throw new Exception("Base de datos no implementada aún.");
          }
        }
        else
          this.connectionString = string.Format("DSN={0};Uid={1};Pwd={2};", (object) connInfo.DSNName, (object) connInfo.UserName, (object) connInfo.Password);
        this.MyDbConnection = this.dataFactory.CreateConnection();
        this.MyDbConnection.ConnectionString = this.ConnectionString;
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    public Connection(string connectionName, string connectionString)
    {
      this.connectionName = connectionName;
      this.connectionString = this.ConnectionString;
    }

    public Connection(string connectionString)
    {
      this.connectionName = string.Empty;
      this.connectionString = connectionString;
    }

    public string ConnectionName
    {
      get
      {
        return this.connectionName;
      }
      set
      {
        this.connectionName = value;
      }
    }

    public string ConnectionString
    {
      get
      {
        return this.connectionString;
      }
      set
      {
        this.connectionString = value;
      }
    }

    public Connectivity Connectivity
    {
      get
      {
        return this.connectivity;
      }
      set
      {
        this.connectivity = value;
      }
    }

    public DateTime? UltimaConexionCorrecta
    {
      get
      {
        return this.ultimaConexionCorrecta;
      }
    }

    public bool? EstadoUltimaConexion
    {
      get
      {
        return this.estadoUltimaConexion;
      }
    }

    public bool AbrirConexion()
    {
      bool flag = false;
      try
      {
        if (this.MyDbConnection == null)
          throw new Exception("El objeto conexión no está inicializado.");
        if (this.MyDbConnection.ConnectionString == string.Empty)
          throw new Exception("No se ha establecido la cadena de conexión.");
        if (this.MyDbConnection.State == ConnectionState.Open)
        {
          flag = true;
        }
        else
        {
            if (this.MyDbConnection.State != ConnectionState.Closed)
            {
                if (this.MyDbConnection.State != ConnectionState.Broken)
                    return flag;
            }

            this.MyDbConnection.Open();
            this.estadoUltimaConexion = new bool?(true);
            this.ultimaConexionCorrecta = new DateTime?(DateTime.Now);
            flag = true;
        }
      }
      catch (Exception ex)
      {
        this.estadoUltimaConexion = new bool?(false);
        throw ex;
      }

      return flag;
    }

    public bool CerrarConexión()
    {
      bool flag = false;
      try
      {
        if (this.MyDbConnection != null)
        {
          if (this.MyDbConnection.State == ConnectionState.Open)
          {
            this.MyDbConnection.Close();
            this.estadoUltimaConexion = new bool?(true);
            this.ultimaConexionCorrecta = new DateTime?(DateTime.Now);
            flag = true;
          }
        }
      }
      catch
      {
        this.estadoUltimaConexion = new bool?(false);
        throw;
      }
      return flag;
    }

    public bool ProbarConexion()
    {
      bool flag = false;
      try
      {
        if (this.MyDbConnection != null)
        {
          if (this.MyDbConnection.ConnectionString != string.Empty)
          {
            this.MyDbConnection.Open();
            if (this.MyDbConnection.State == ConnectionState.Open)
            {
              this.estadoUltimaConexion = new bool?(true);
              this.ultimaConexionCorrecta = new DateTime?(DateTime.Now);
              flag = true;
            }
            else
              this.estadoUltimaConexion = new bool?(true);
          }
        }
      }
      catch
      {
        this.estadoUltimaConexion = new bool?(false);
      }
      finally
      {
        this.MyDbConnection.Close();
      }
      return flag;
    }

    public DbDataReader DbExecuteReader(string queryName)
    {
      return this.DbExecuteReader(new QueryEngine(queryName));
    }

    public DbDataReader DbExecuteReader(QueryEngine query)
    {
      DbDataReader dbDataReader = (DbDataReader) null;
      try
      {
        if (this.AbrirConexion())
        {
          DbCommand command = this.dataFactory.CreateCommand();
          command.Connection = this.MyDbConnection;
          command.CommandType = CommandType.StoredProcedure;
          command.CommandText = query.QueryName;
          command.Transaction = this.transaccion;
          if (query.ParameterName.Count != 0 && query.ParameterType.Count != 0 && query.ParameterValue.Count != 0)
          {
            if (query.ParameterName.Count != query.ParameterValue.Count || query.ParameterName.Count != query.ParameterType.Count || query.ParameterValue.Count != query.ParameterType.Count)
              throw new Exception("Parameters (name, type and value) must be the same length!");
            for (int index = 0; index < query.ParameterName.Count; ++index)
            {
              DbParameter parameter = this.dataFactory.CreateParameter();
              parameter.ParameterName = this.Connectivity.ParameterPrefix + (string) query.ParameterName[index];
              if (query.ParameterType.Count != 0)
                parameter.DbType = (DbType) query.ParameterType[index];
              if (query.ParameterValue.Count != 0)
                parameter.Value = query.ParameterValue[index];
              if (query.ParameterSize.Count != 0)
                parameter.Size = (int) query.ParameterSize[index];
              command.Parameters.Add((object) parameter);
            }
          }
          else
          {
            if (query.ParameterName.Count == 0 && query.ParameterType.Count == 0)
            {
              if (query.ParameterValue.Count == 0)
                goto label_17;
            }
            throw new Exception("Must give Name, Type and Value to set parameters");
          }
label_17:
          try
          {
            dbDataReader = command.ExecuteReader(CommandBehavior.CloseConnection);
            //if (this.transaccion == null)
            //  this.CerrarConexión();
          }
          catch (DbException ex)
          {
            this.DeshacerTransaccion();
            throw;
          }
        }
      }
      catch
      {
        throw;
      }
      return dbDataReader;
    }

    public object DbExecuteScalar(string queryName)
    {
      return this.DbExecuteScalar(new QueryEngine(queryName));
    }

    public object DbExecuteScalar(QueryEngine query)
    {
      object obj = (object) null;
      try
      {
        if (this.AbrirConexion())
        {
          DbCommand command = this.dataFactory.CreateCommand();
          command.Connection = this.MyDbConnection;
          command.CommandType = CommandType.StoredProcedure;
          command.CommandText = query.QueryName;
          command.Transaction = this.transaccion;
          if (query.ParameterName.Count != 0 && query.ParameterType.Count != 0 && query.ParameterValue.Count != 0)
          {
            if (query.ParameterName.Count != query.ParameterValue.Count || query.ParameterName.Count != query.ParameterType.Count || query.ParameterValue.Count != query.ParameterType.Count)
              throw new Exception("Parameters (name, type and value) must be the same length!");
            for (int index = 0; index < query.ParameterName.Count; ++index)
            {
              DbParameter parameter = this.dataFactory.CreateParameter();
              parameter.ParameterName = this.Connectivity.ParameterPrefix + (string) query.ParameterName[index];
              if (query.ParameterSize.Count != 0)
                parameter.Size = (int) query.ParameterSize[index];
              parameter.DbType = (DbType) query.ParameterType[index];
              parameter.Value = query.ParameterValue[index];
              command.Parameters.Add((object) parameter);
            }
          }
//          else
//          {
//            if (query.ParameterName.Count == 0 && query.ParameterType.Count == 0)
//            {
//              if (query.ParameterValue.Count == 0)
//                goto label_13;
//            }
//            throw new Exception("Must give Name, Type and Value to set parameters");
//          }
//label_13:
//          try
          //{
            obj = command.ExecuteScalar();
            //if (this.transaccion == null)
            //  this.CerrarConexión();
          //}
          //catch (DbException ex)
          //{
          //  this.DeshacerTransaccion();
          //  throw;
          //}
        }
      }
      catch (Exception ex)
      {
        throw ex;
      }
      return obj;
    }

    public int DbExecuteNonQuery(string queryName)
    {
      return this.DbExecuteNonQuery(new QueryEngine(queryName));
    }

    public int DbExecuteNonQuery(QueryEngine query)
    {
      int num = -1;
      try
      {
        if (this.AbrirConexion())
        {
          DbCommand command = this.dataFactory.CreateCommand();
          command.Connection = this.MyDbConnection;
          command.CommandType = CommandType.StoredProcedure;
          command.CommandText = query.QueryName;
          command.Transaction = this.transaccion;
          if (query.ParameterName.Count != 0 && query.ParameterType.Count != 0 && query.ParameterValue.Count != 0)
          {
            if (query.ParameterName.Count != query.ParameterValue.Count)
              throw new Exception("Parameters (name and value) must be the same length!");
            for (int index = 0; index < query.ParameterName.Count; ++index)
            {
              DbParameter parameter = this.dataFactory.CreateParameter();
              parameter.ParameterName = this.Connectivity.ParameterPrefix + (string) query.ParameterName[index];
              parameter.Value = query.ParameterValue[index];
              if (query.ParameterSize.Count != 0 && index <= query.ParameterSize.Count - 1)
                parameter.Size = (int) query.ParameterSize[index];
              if (query.ParameterType.Count != 0 && index <= query.ParameterType.Count - 1)
                parameter.DbType = (DbType) query.ParameterType[index];
              command.Parameters.Add((object) parameter);
            }
          }
          else
          {
            if (query.ParameterName.Count == 0 && query.ParameterType.Count == 0)
            {
              if (query.ParameterValue.Count == 0)
                goto label_15;
            }
            throw new Exception("Must give Name, Type and Value to set parameters");
          }
label_15:
          try
          {
            
            num = (int)command.ExecuteNonQuery();
            //if (this.transaccion == null)
            //  this.CerrarConexión();
          }
          catch (DbException ex)
          {
            this.DeshacerTransaccion();
            throw;
          }
        }
      }
      catch
      {
        throw;
      }
      return num;
    }

    public DbDataReader DbExecuteReaderString(string queryString)
    {
      DbDataReader dbDataReader = (DbDataReader) null;
      DbCommand dbCommand = (DbCommand) null;
      try
      {
        if (this.AbrirConexion())
        {
          DbCommand command = this.dataFactory.CreateCommand();
          command.Connection = this.MyDbConnection;
          command.CommandType = CommandType.Text;
          command.CommandText = queryString;
          dbDataReader = command.ExecuteReader(CommandBehavior.CloseConnection);
          this.CerrarConexión();
        }
      }
      catch
      {
        throw;
      }
      finally
      {
        dbCommand = (DbCommand) null;
      }
      return dbDataReader;
    }

    public object DbExecuteScalarString(string queryString)
    {
      object obj = (object) null;
      DbCommand dbCommand = (DbCommand) null;
      try
      {
        if (this.AbrirConexion())
        {
          DbCommand command = this.dataFactory.CreateCommand();
          command.Connection = this.MyDbConnection;
          command.CommandType = CommandType.Text;
          command.CommandText = queryString;
          obj = command.ExecuteScalar();
          this.CerrarConexión();
        }
      }
      catch
      {
        throw;
      }
      finally
      {
        dbCommand = (DbCommand) null;
      }
      return obj;
    }

    public int DbExecuteNonQueryString(QueryEngine query)
    {
      return this.DbExecuteNonQueryString(query.QueryName);
    }

    public int DbExecuteNonQueryString(string queryString)
    {
      DbCommand dbCommand = (DbCommand) null;
      int num = -1;
      try
      {
        if (this.AbrirConexion())
        {
          DbCommand command = this.dataFactory.CreateCommand();
          command.Connection = this.MyDbConnection;
          command.CommandType = CommandType.Text;
          command.CommandText = queryString;
          command.Transaction = this.transaccion;
          try
          {
            num = command.ExecuteNonQuery();
            //if (this.transaccion == null)
            //  this.CerrarConexión();
          }
          catch (DbException ex)
          {
            this.DeshacerTransaccion();
            throw;
          }
        }
      }
      catch
      {
        throw;
      }
      finally
      {
        dbCommand = (DbCommand) null;
      }
      return num;
    }

    public DataSet DataSetExecuteReaderString(QueryEngine query)
    {
      return this._dataSetExecuteReader(query, false);
    }

    public DataSet DataSetExecuteReader(string queryName)
    {
      return this._dataSetExecuteReader(new QueryEngine(queryName), true);
    }

    public DataSet DataSetExecuteReader(QueryEngine query)
    {
      return this._dataSetExecuteReader(query, true);
    }

    private DataSet _dataSetExecuteReader(QueryEngine query, bool esSP)
    {
      DbCommand dbCommand = (DbCommand) null;
      DataSet dataSet = (DataSet) null;
      try
      {
        if (this.AbrirConexion())
        {
          DbCommand command = this.dataFactory.CreateCommand();
          command.Connection = this.MyDbConnection;
          command.CommandType = !esSP ? CommandType.Text : CommandType.StoredProcedure;
          command.CommandText = query.QueryName;
          if (query.ParameterName.Count != 0 && query.ParameterType.Count != 0 && query.ParameterValue.Count != 0)
          {
            if (query.ParameterName.Count != query.ParameterValue.Count || query.ParameterName.Count != query.ParameterType.Count || query.ParameterValue.Count != query.ParameterType.Count)
              throw new Exception("Parameters (name, type and value) must be the same length!");
            for (int index = 0; index < query.ParameterName.Count; ++index)
            {
              DbParameter parameter = this.dataFactory.CreateParameter();
              parameter.ParameterName = this.Connectivity.ParameterPrefix + (string) query.ParameterName[index];
              if (query.ParameterType.Count != 0)
                parameter.DbType = (DbType) query.ParameterType[index];
              if (query.ParameterValue.Count != 0)
                parameter.Value = query.ParameterValue[index];
              if (query.ParameterSize.Count != 0)
                parameter.Size = (int) query.ParameterSize[index];
              command.Parameters.Add((object) parameter);
            }
          }
          else if (query.ParameterName.Count != 0 || query.ParameterType.Count != 0 || query.ParameterValue.Count != 0)
            throw new Exception("Must give Name, Type and Value to set parameters");
          DbDataAdapter dataAdapter = this.dataFactory.CreateDataAdapter();
          dataAdapter.SelectCommand = command;
          dataSet = new DataSet();
          dataAdapter.Fill(dataSet, "tabla");
          this.CerrarConexión();
        }
      }
      catch
      {
        throw;
      }
      finally
      {
        dbCommand = (DbCommand) null;
      }
      return dataSet;
    }

    public void ComenzarTransaccion()
    {
      try
      {
        if (this.MyDbConnection.State == ConnectionState.Closed)
        {
          this.MyDbConnection.Open();
          this.estadoUltimaConexion = new bool?(true);
          this.ultimaConexionCorrecta = new DateTime?(DateTime.Now);
        }
        this.transaccion = this.MyDbConnection.BeginTransaction();
      }
      catch
      {
        this.estadoUltimaConexion = new bool?(false);
        this.transaccion = (DbTransaction) null;
        throw;
      }
    }

    public void ConfirmarTransaccion()
    {
      try
      {
        if (this.transaccion != null)
        {
          this.transaccion.Commit();
          this.transaccion.Dispose();
          this.transaccion = (DbTransaction) null;
        }
        if (this.MyDbConnection.State == ConnectionState.Closed)
          return;
        this.MyDbConnection.Close();
        this.estadoUltimaConexion = new bool?(true);
        this.ultimaConexionCorrecta = new DateTime?(DateTime.Now);
      }
      catch
      {
        this.estadoUltimaConexion = new bool?(false);
        this.transaccion = (DbTransaction) null;
        throw;
      }
    }

    public void DeshacerTransaccion()
    {
      try
      {
        if (this.transaccion != null)
        {
          this.transaccion.Rollback();
          this.transaccion.Dispose();
          this.transaccion = (DbTransaction) null;
        }
        if (this.MyDbConnection.State == ConnectionState.Closed)
          return;
        this.MyDbConnection.Close();
        this.estadoUltimaConexion = new bool?(true);
        this.ultimaConexionCorrecta = new DateTime?(DateTime.Now);
      }
      catch
      {
        this.estadoUltimaConexion = new bool?(false);
        this.transaccion = (DbTransaction) null;
        throw;
      }
    }
  }
}
