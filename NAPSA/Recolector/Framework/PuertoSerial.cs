// Decompiled with JetBrains decompiler
// Type: DASYS.Framework.PuertoSerial
// Assembly: Framework, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1B503700-E29D-4D7A-BD70-519F036595D0
// Assembly location: C:\Program Files (x86)\NAPSA\Colector III\Framework.dll

using System;
using System.Drawing;
using System.IO.Ports;
using System.Text;
using System.Windows.Forms;

namespace DASYS.Framework
{
  public class PuertoSerial
  {
    private string _baudRate = string.Empty;
    private string _parity = string.Empty;
    private string _stopBits = string.Empty;
    private string _dataBits = string.Empty;
    private string _portName = string.Empty;
    private Color[] MessageColor = new Color[5]
    {
      Color.Blue,
      Color.Green,
      Color.Black,
      Color.Orange,
      Color.Red
    };
    private SerialPort comPort = new SerialPort();
    private PuertoSerial.TransmissionType _transType;
    private RichTextBox _displayWindow;

    public string BaudRate
    {
      get
      {
        return this._baudRate;
      }
      set
      {
        this._baudRate = value;
      }
    }

    public string Parity
    {
      get
      {
        return this._parity;
      }
      set
      {
        this._parity = value;
      }
    }

    public string StopBits
    {
      get
      {
        return this._stopBits;
      }
      set
      {
        this._stopBits = value;
      }
    }

    public string DataBits
    {
      get
      {
        return this._dataBits;
      }
      set
      {
        this._dataBits = value;
      }
    }

    public string PortName
    {
      get
      {
        return this._portName;
      }
      set
      {
        this._portName = value;
      }
    }

    public PuertoSerial.TransmissionType CurrentTransmissionType
    {
      get
      {
        return this._transType;
      }
      set
      {
        this._transType = value;
      }
    }

    public RichTextBox DisplayWindow
    {
      get
      {
        return this._displayWindow;
      }
      set
      {
        this._displayWindow = value;
      }
    }

    public PuertoSerial(
      string baud,
      string par,
      string sBits,
      string dBits,
      string name,
      RichTextBox rtb)
    {
      this._baudRate = baud;
      this._parity = par;
      this._stopBits = sBits;
      this._dataBits = dBits;
      this._portName = name;
      this._displayWindow = rtb;
      this.comPort.DataReceived += new SerialDataReceivedEventHandler(this.comPort_DataReceived);
    }

    public PuertoSerial()
    {
      this._baudRate = string.Empty;
      this._parity = string.Empty;
      this._stopBits = string.Empty;
      this._dataBits = string.Empty;
      this._portName = "COM1";
      this._displayWindow = (RichTextBox) null;
      this.comPort.DataReceived += new SerialDataReceivedEventHandler(this.comPort_DataReceived);
    }

    public void WriteData(string msg)
    {
      switch (this.CurrentTransmissionType)
      {
        case PuertoSerial.TransmissionType.Text:
          if (!this.comPort.IsOpen)
            this.comPort.Open();
          this.comPort.Write(msg);
          this.DisplayData(PuertoSerial.MessageType.Outgoing, msg + "\n");
          break;
        case PuertoSerial.TransmissionType.Hex:
          try
          {
            byte[] numArray = this.HexToByte(msg);
            this.comPort.Write(numArray, 0, numArray.Length);
            this.DisplayData(PuertoSerial.MessageType.Outgoing, this.ByteToHex(numArray) + "\n");
            break;
          }
          catch (FormatException ex)
          {
            this.DisplayData(PuertoSerial.MessageType.Error, ex.Message);
            break;
          }
          finally
          {
            this._displayWindow.SelectAll();
          }
        default:
          if (!this.comPort.IsOpen)
            this.comPort.Open();
          this.comPort.Write(msg);
          this.DisplayData(PuertoSerial.MessageType.Outgoing, msg + "\n");
          break;
      }
    }

    private byte[] HexToByte(string msg)
    {
      msg = msg.Replace(" ", "");
      byte[] numArray = new byte[msg.Length / 2];
      for (int startIndex = 0; startIndex < msg.Length; startIndex += 2)
        numArray[startIndex / 2] = Convert.ToByte(msg.Substring(startIndex, 2), 16);
      return numArray;
    }

    private string ByteToHex(byte[] comByte)
    {
      StringBuilder stringBuilder = new StringBuilder(comByte.Length * 3);
      foreach (byte num in comByte)
        stringBuilder.Append(Convert.ToString(num, 16).PadLeft(2, '0').PadRight(3, ' '));
      return stringBuilder.ToString().ToUpper();
    }

    [STAThread]
    private void DisplayData(PuertoSerial.MessageType type, string msg)
    {
      this._displayWindow.Invoke((Action) delegate
      {
        this._displayWindow.SelectedText = string.Empty;
        this._displayWindow.SelectionFont = new Font(this._displayWindow.SelectionFont, FontStyle.Bold);
        this._displayWindow.SelectionColor = this.MessageColor[(int) type];
        this._displayWindow.AppendText(msg);
        this._displayWindow.ScrollToCaret();
      });
    }

    public bool OpenPort()
    {
      try
      {
        if (this.comPort.IsOpen)
          this.comPort.Close();
        this.comPort.BaudRate = int.Parse(this._baudRate);
        this.comPort.DataBits = int.Parse(this._dataBits);
        this.comPort.StopBits = (System.IO.Ports.StopBits) Enum.Parse(typeof (System.IO.Ports.StopBits), this._stopBits);
        this.comPort.Parity = (System.IO.Ports.Parity) Enum.Parse(typeof (System.IO.Ports.Parity), this._parity);
        this.comPort.PortName = this._portName;
        this.comPort.Open();
        this.DisplayData(PuertoSerial.MessageType.Normal, "Puerto abierto " + (object) DateTime.Now + "\n");
        return true;
      }
      catch (Exception ex)
      {
        this.DisplayData(PuertoSerial.MessageType.Error, ex.Message);
        return false;
      }
    }

    public bool ClosePort()
    {
      try
      {
        if (this.comPort.IsOpen)
          this.comPort.Close();
        this.comPort.Open();
        this.DisplayData(PuertoSerial.MessageType.Normal, "Puerto cerrado " + (object) DateTime.Now + "\n");
        return true;
      }
      catch (Exception ex)
      {
        this.DisplayData(PuertoSerial.MessageType.Error, ex.Message);
        return false;
      }
    }

    public string[] GetParityValues()
    {
      return Enum.GetNames(typeof (System.IO.Ports.Parity));
    }

    public string[] GetStopBitValues()
    {
      return Enum.GetNames(typeof (System.IO.Ports.StopBits));
    }

    public string[] GetPortNameValues()
    {
      return SerialPort.GetPortNames();
    }

    private void comPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
    {
      switch (this.CurrentTransmissionType)
      {
        case PuertoSerial.TransmissionType.Text:
          this.DisplayData(PuertoSerial.MessageType.Incoming, this.comPort.ReadExisting() + "\n");
          break;
        case PuertoSerial.TransmissionType.Hex:
          int bytesToRead = this.comPort.BytesToRead;
          byte[] numArray = new byte[bytesToRead];
          this.comPort.Read(numArray, 0, bytesToRead);
          this.DisplayData(PuertoSerial.MessageType.Incoming, this.ByteToHex(numArray) + "\n");
          break;
        default:
          this.DisplayData(PuertoSerial.MessageType.Incoming, this.comPort.ReadExisting() + "\n");
          break;
      }
    }

    public enum TransmissionType
    {
      Text,
      Hex,
    }

    public enum MessageType
    {
      Incoming,
      Outgoing,
      Normal,
      Warning,
      Error,
    }
  }
}
