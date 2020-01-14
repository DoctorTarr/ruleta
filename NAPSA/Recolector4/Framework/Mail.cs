// Decompiled with JetBrains decompiler
// Type: DASYS.Framework.Mail
// Assembly: Framework, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1B503700-E29D-4D7A-BD70-519F036595D0
// Assembly location: C:\Program Files (x86)\NAPSA\Colector III\Framework.dll

using System;
using System.Collections;
using System.Net;
using System.Net.Mail;

namespace DASYS.Framework
{
  public class Mail
  {
    private string _destinatario = string.Empty;
    private string _remitente = string.Empty;
    private string _asunto = string.Empty;
    private string _cuerpo = string.Empty;
    private string _servidorSMTP = "localhost";
    private int _puerto = 25;
    private string _usuario = string.Empty;
    private string _contraseña = string.Empty;
    private string[] _copiaCarbonica;
    private ArrayList _adjuntos;
    private bool _ssl;

    public Mail()
    {
    }

    public Mail(string destinatario, string remitente, string asunto, string cuerpo)
    {
      this._destinatario = destinatario;
      this._remitente = remitente;
      this._asunto = asunto;
      this._cuerpo = cuerpo;
    }

    public string Destinatario
    {
      get
      {
        return this._destinatario;
      }
      set
      {
        this._destinatario = value;
      }
    }

    public string Remitente
    {
      get
      {
        return this._remitente;
      }
      set
      {
        this._remitente = value;
      }
    }

    public string Asunto
    {
      get
      {
        return this._asunto;
      }
      set
      {
        this._asunto = value;
      }
    }

    public string Cuerpo
    {
      get
      {
        return this._cuerpo;
      }
      set
      {
        this._cuerpo = value;
      }
    }

    public string ServidorSMTP
    {
      get
      {
        return this._servidorSMTP;
      }
      set
      {
        this._servidorSMTP = value;
      }
    }

    public ArrayList Adjuntos
    {
      get
      {
        return this._adjuntos;
      }
      set
      {
        this._adjuntos = value;
      }
    }

    public string[] CopiaCarbonica
    {
      get
      {
        return this._copiaCarbonica;
      }
      set
      {
        this._copiaCarbonica = value;
      }
    }

    public bool SSL
    {
      get
      {
        return this._ssl;
      }
      set
      {
        this._ssl = value;
      }
    }

    public int Puerto
    {
      get
      {
        return this._puerto;
      }
      set
      {
        this._puerto = value;
      }
    }

    public string Usuario
    {
      get
      {
        return this._usuario;
      }
      set
      {
        this._usuario = value;
      }
    }

    public string Contraseña
    {
      get
      {
        return this._contraseña;
      }
      set
      {
        this._contraseña = value;
      }
    }

    public bool Enviar()
    {
      try
      {
        MailAddress mailAddress1 = new MailAddress(this.Remitente);
        MailAddress mailAddress2 = new MailAddress(this.Destinatario);
        MailMessage message = new MailMessage(this.Remitente, this.Destinatario);
        message.Subject = this.Asunto;
        message.Body = this.Cuerpo;
        if (this.CopiaCarbonica != null)
        {
          foreach (string addresses in this.CopiaCarbonica)
            message.CC.Add(addresses);
        }
        if (this.Adjuntos != null)
        {
          foreach (string adjunto in this.Adjuntos)
            message.Attachments.Add(new Attachment(adjunto));
        }
        new SmtpClient(this.ServidorSMTP)
        {
          Host = this.ServidorSMTP,
          Port = this.Puerto,
          EnableSsl = this.SSL,
          Credentials = (!(this.Usuario == string.Empty) ? (ICredentialsByHost) new NetworkCredential(this.Usuario, this.Contraseña) : (ICredentialsByHost) CredentialCache.DefaultNetworkCredentials)
        }.Send(message);
        return true;
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message);
      }
    }
  }
}
