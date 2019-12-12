// Decompiled with JetBrains decompiler
// Type: DASYS.DAL.QueryEngine
// Assembly: DAL, Version=3.4.5.5, Culture=neutral, PublicKeyToken=null
// MVID: D8AEA125-C248-431D-9EBF-103DF8547D67
// Assembly location: C:\Program Files (x86)\NAPSA\Colector III\DAL.dll

using System.Collections;
using System.Data;

namespace DASYS.DAL
{
  public class QueryEngine
  {
    private ArrayList parameterName = new ArrayList();
    private ArrayList parameterValue = new ArrayList();
    private ArrayList parameterType = new ArrayList();
    private ArrayList parameterSize = new ArrayList();
    private ArrayList parameterDirection = new ArrayList();
    private ArrayList parameterScale = new ArrayList();
    private ArrayList parameterPrecision = new ArrayList();
    private string queryName;

    public QueryEngine()
    {
    }

    public QueryEngine(string QueryName)
    {
      this.queryName = QueryName;
    }

    public ArrayList ParameterName
    {
      get
      {
        return this.parameterName;
      }
      set
      {
        this.parameterName = value;
      }
    }

    public ArrayList ParameterType
    {
      get
      {
        return this.parameterType;
      }
      set
      {
        this.parameterType = value;
      }
    }

    public ArrayList ParameterValue
    {
      get
      {
        return this.parameterValue;
      }
      set
      {
        this.parameterValue = value;
      }
    }

    public ArrayList ParameterSize
    {
      get
      {
        return this.parameterSize;
      }
      set
      {
        this.parameterSize = value;
      }
    }

    public ArrayList ParameterDirection
    {
      get
      {
        return this.parameterDirection;
      }
      set
      {
        this.parameterDirection = value;
      }
    }

    public ArrayList ParameterScale
    {
      get
      {
        return this.parameterScale;
      }
      set
      {
        this.parameterScale = value;
      }
    }

    public ArrayList ParameterPrecision
    {
      get
      {
        return this.parameterPrecision;
      }
      set
      {
        this.parameterPrecision = value;
      }
    }

    public string QueryName
    {
      get
      {
        return this.queryName;
      }
      set
      {
        this.queryName = value;
      }
    }

    public void AddNames(params string[] parameterNameList)
    {
      foreach (object parameterName in parameterNameList)
        this.parameterName.Add(parameterName);
    }

    public void AddTypes(params DbType[] parameterTypeList)
    {
      foreach (int parameterType in parameterTypeList)
        this.parameterType.Add((object) (DbType) parameterType);
    }

    public void AddValues(params object[] parameterValueList)
    {
      foreach (object parameterValue in parameterValueList)
        this.parameterValue.Add(parameterValue);
    }

    public void RemoveValues()
    {
      this.parameterValue = new ArrayList();
    }

    public void AddParameter(string parameterName, DbType parameterType, object parameterValue)
    {
      this.parameterName.Add((object) parameterName);
      this.parameterType.Add((object) parameterType);
      this.parameterValue.Add(parameterValue);
    }

    public void RemoveParameters()
    {
      this.parameterValue = new ArrayList();
      this.parameterType = new ArrayList();
      this.parameterName = new ArrayList();
    }

    public void AddSizes(params int[] parameterSizeList)
    {
      foreach (int parameterSize in parameterSizeList)
        this.parameterSize.Add((object) parameterSize);
    }
  }
}
