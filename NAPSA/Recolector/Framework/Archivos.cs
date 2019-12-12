// Decompiled with JetBrains decompiler
// Type: DASYS.Framework.Archivos
// Assembly: Framework, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1B503700-E29D-4D7A-BD70-519F036595D0
// Assembly location: C:\Program Files (x86)\NAPSA\Colector III\Framework.dll

using DASYS.DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;

namespace DASYS.Framework
{
  public class Archivos
  {
    public static List<string> ObtenerArchivos(string root)
    {
      return Archivos.ObtenerArchivos(root, (string) null);
    }

    public static List<string> ObtenerArchivos(string root, string extension)
    {
      if (string.IsNullOrEmpty(extension))
        extension = "*";
      List<string> stringList = new List<string>();
      Stack<string> stringStack = new Stack<string>(50);
      if (!Directory.Exists(root))
        throw new ArgumentException();
      stringStack.Push(root);
      while (stringStack.Count > 0)
      {
        string path = stringStack.Pop();
        string[] directories;
        try
        {
          directories = Directory.GetDirectories(path);
        }
        catch (UnauthorizedAccessException ex)
        {
          Console.WriteLine(ex.Message);
          continue;
        }
        catch (DirectoryNotFoundException ex)
        {
          Console.WriteLine(ex.Message);
          continue;
        }
        string[] files;
        try
        {
          files = Directory.GetFiles(path, "*." + extension);
        }
        catch (UnauthorizedAccessException ex)
        {
          Console.WriteLine(ex.Message);
          continue;
        }
        catch (DirectoryNotFoundException ex)
        {
          Console.WriteLine(ex.Message);
          continue;
        }
        foreach (string str in files)
        {
          try
          {
            stringList.Add(str);
          }
          catch (FileNotFoundException ex)
          {
            Console.WriteLine(ex.Message);
          }
        }
        foreach (string str in directories)
          stringStack.Push(str);
      }
      return stringList;
    }

    public class XML
    {
      public static Hashtable LeerXML(string nombreElemento)
      {
        return Archivos.XML.LeerXML(nombreElemento, "DASYS.OS.config.xml");
      }

      public static Hashtable LeerXML(string nombreElemento, string rutaArchivo)
      {
        Hashtable hashtable = new Hashtable();
        try
        {
          XPathNavigator navigator = new XPathDocument(rutaArchivo).CreateNavigator();
          navigator.MoveToRoot();
          navigator.MoveToFirstChild();
          do
          {
            if (navigator.NodeType == XPathNodeType.Element && navigator.HasChildren)
            {
              navigator.MoveToFirstChild();
              do
              {
                if (navigator.Name.ToLower() == nombreElemento.ToLower())
                  hashtable = Archivos.XML.ObtenerDatos(navigator);
                int num = navigator.HasAttributes ? 1 : 0;
              }
              while (navigator.MoveToNext());
            }
          }
          while (navigator.MoveToNext());
          return hashtable;
        }
        catch (Exception ex)
        {
          throw ex;
        }
        finally
        {
        }
      }

      private static Hashtable ObtenerDatos(XPathNavigator nav)
      {
        Hashtable hashtable = new Hashtable();
        try
        {
          if (nav.HasChildren)
          {
            nav.MoveToFirstChild();
            do
            {
              hashtable.Add((object) nav.Name, (object) nav.Value);
            }
            while (nav.MoveToNext());
            nav.MoveToParent();
          }
          return hashtable;
        }
        catch (Exception ex)
        {
          throw new Exception(ex.Message);
        }
      }

      public static Connectivities LeerConexionXML()
      {
        return Archivos.XML.LeerConexionXML("DASYS.OS.config.xml");
      }

      public static Connectivities LeerConexionXML(string rutaArchivo)
      {
        Connectivities connectivities = new Connectivities();
        Connectivity connectivity1 = new Connectivity();
        Hashtable hashtable = new Hashtable();
        try
        {
          XmlTextReader xmlTextReader = new XmlTextReader((TextReader) new StreamReader(rutaArchivo, Encoding.UTF8));
          XmlDocument xmlDocument = new XmlDocument();
          xmlDocument.Load(rutaArchivo);
          XmlNodeReader xmlNodeReader = new XmlNodeReader((XmlNode) xmlDocument);
          foreach (XmlNode selectNode in xmlDocument.SelectNodes("configuration/dbConnections/dbConnection"))
          {
            Connectivity connectivity2 = new Connectivity();
            connectivity2.ConnectionName = selectNode.Attributes.GetNamedItem("name").InnerText;
            connectivity2.VerificarConexionInicio = Utils.Datos.StringToBoolean((object) selectNode.Attributes.GetNamedItem("testConnection").InnerText);
            connectivity2.DSNName = selectNode.SelectSingleNode("DSNName").InnerText;
            switch (selectNode.SelectSingleNode("dataBaseEngine").InnerText.ToUpper())
            {
              case "MYSQL":
                connectivity2.DataBaseType = DataBaseType.MySQL;
                break;
              case "SQL":
                connectivity2.DataBaseType = DataBaseType.SQL;
                break;
              case "ACCESS":
                connectivity2.DataBaseType = DataBaseType.ACCESS;
                break;
            }
            connectivity2.ServerName = selectNode.SelectSingleNode("server").InnerText;
            connectivity2.DataBaseName = selectNode.SelectSingleNode("dataBase").InnerText;
            connectivity2.UserName = selectNode.SelectSingleNode("user").InnerText;
            connectivity2.Password = selectNode.SelectSingleNode("password").InnerText;
            connectivity2.Timeout = Utils.Datos.NullToInt32((object) selectNode.SelectSingleNode("timeout").InnerText);
            connectivity2.PersistSecurityInfo = Utils.Datos.StringToBoolean((object) selectNode.SelectSingleNode("persistSecurity").InnerText.ToUpper());
            connectivity2.IntegratedSecurity = Utils.Datos.StringToBoolean((object) selectNode.SelectSingleNode("integratedSecurity").InnerText.ToUpper());
            connectivity2.Port = Utils.Datos.NullToInt32((object) selectNode.SelectSingleNode("port").InnerText);
            connectivities.Add(connectivity2);
          }
        }
        catch (Exception ex)
        {
          throw new Exception(ex.Message);
        }
        return connectivities;
      }

      public static bool EscribirConexionXML(Connectivity conexion)
      {
        return Archivos.XML.EscribirConexionXML("DASYS.OS.config.xml", conexion);
      }

      public static bool EscribirConexionXML(string rutaArchivo, Connectivity conexion)
      {
        bool flag = false;
        try
        {
          XmlDocument xmlDocument = new XmlDocument();
          xmlDocument.Load(rutaArchivo);
          foreach (XmlNode selectNode in xmlDocument.SelectNodes("configuration/dbConnections/dbConnection"))
          {
            if (selectNode.Attributes["name"].InnerText == conexion.ConnectionName)
            {
              selectNode.Attributes["testConnection"].InnerText = conexion.VerificarConexionInicio ? "1" : "0";
              selectNode.SelectSingleNode("DSNName").InnerText = conexion.DSNName == null ? string.Empty : conexion.DSNName.ToString();
              selectNode.SelectSingleNode("dataBaseEngine").InnerText = conexion.DataBaseType.ToString();
              selectNode.SelectSingleNode("server").InnerText = conexion.ServerName;
              selectNode.SelectSingleNode("dataBase").InnerText = conexion.DataBaseName;
              selectNode.SelectSingleNode("user").InnerText = conexion.UserName;
              selectNode.SelectSingleNode("password").InnerText = conexion.Password;
              selectNode.SelectSingleNode("timeout").InnerText = conexion.Timeout.ToString();
              selectNode.SelectSingleNode("persistSecurity").InnerText = conexion.PersistSecurityInfo ? "1" : "0";
              selectNode.SelectSingleNode("integratedSecurity").InnerText = conexion.IntegratedSecurity ? "1" : "0";
              selectNode.SelectSingleNode("port").InnerText = conexion.Port.ToString();
              xmlDocument.Save(rutaArchivo);
              flag = true;
              break;
            }
          }
        }
        catch (Exception ex)
        {
          throw new Exception(ex.Message);
        }
        return flag;
      }

      public static Hashtable leerXMLPartes(XmlNode nodo)
      {
        Hashtable hashtable = new Hashtable();
        try
        {
          foreach (XmlElement xmlElement in nodo)
            hashtable.Add((object) xmlElement.Name, (object) xmlElement.InnerText);
        }
        catch (Exception ex)
        {
          throw new Exception(ex.Message);
        }
        return hashtable;
      }

      public static void EscribirXML(string seccion, Hashtable keyValue, string archivo)
      {
        try
        {
          XmlDocument xmlDocument = new XmlDocument();
          xmlDocument.Load(archivo);
          XmlNodeList xmlNodeList = xmlDocument.SelectNodes("//" + seccion);
          if (xmlNodeList[0] == null)
          {
            XmlNode xmlNode = xmlDocument.SelectSingleNode("configuration");
            XmlNode element1 = (XmlNode) xmlDocument.CreateElement(seccion);
            foreach (DictionaryEntry dictionaryEntry in keyValue)
            {
              XmlNode element2 = (XmlNode) xmlDocument.CreateElement(dictionaryEntry.Key.ToString());
              element2.InnerText = dictionaryEntry.Value.ToString();
              element1.AppendChild(element2);
            }
            xmlNode.AppendChild(element1);
          }
          else
          {
            foreach (DictionaryEntry dictionaryEntry in keyValue)
            {
              XmlNode element = (XmlNode) xmlNodeList[0][dictionaryEntry.Key.ToString()];
              if (element == null)
              {
                element = (XmlNode) xmlDocument.CreateElement(dictionaryEntry.Key.ToString());
                xmlNodeList[0].AppendChild(element);
              }
              element.InnerText = dictionaryEntry.Value.ToString();
            }
          }
          xmlDocument.PreserveWhitespace = true;
          XmlTextWriter xmlTextWriter = new XmlTextWriter(archivo, Encoding.UTF8);
          xmlDocument.WriteTo((XmlWriter) xmlTextWriter);
          xmlTextWriter.Close();
        }
        catch
        {
          throw;
        }
      }
    }

    public class Serializador
    {
      private string UTF8ByteArrayToString(byte[] characters)
      {
        return new UTF8Encoding().GetString(characters);
      }

      private byte[] StringToUTF8ByteArray(string pXmlString)
      {
        return new UTF8Encoding().GetBytes(pXmlString);
      }

      public string Serializar(object Object)
      {
        try
        {
          MemoryStream memoryStream = new MemoryStream();
          XmlSerializer xmlSerializer = new XmlSerializer(typeof (Accesorios.Partes));
          XmlTextWriter xmlTextWriter = new XmlTextWriter((Stream) memoryStream, Encoding.UTF8);
          xmlSerializer.Serialize((XmlWriter) xmlTextWriter, Object);
          return this.UTF8ByteArrayToString(((MemoryStream) xmlTextWriter.BaseStream).ToArray());
        }
        catch (Exception ex)
        {
          throw new Exception(ex.Message);
        }
      }

      public Accesorios.Partes DeserializarPartes(string pXmlizedString)
      {
        try
        {
          XmlSerializer xmlSerializer = new XmlSerializer(typeof (Accesorios.Partes));
          MemoryStream memoryStream = new MemoryStream(this.StringToUTF8ByteArray(pXmlizedString));
          XmlTextWriter xmlTextWriter = new XmlTextWriter((Stream) memoryStream, Encoding.UTF8);
          return xmlSerializer.Deserialize((Stream) memoryStream) as Accesorios.Partes;
        }
        catch (Exception ex)
        {
          throw new Exception(ex.Message);
        }
      }

      public static bool Guardar(string rutaCompleta, string contenido)
      {
        try
        {
          StreamWriter streamWriter = new StreamWriter(rutaCompleta);
          streamWriter.Write(contenido);
          streamWriter.Flush();
          streamWriter.Close();
        }
        catch (Exception ex)
        {
          throw new Exception(ex.Message);
        }
        return true;
      }

      public static string Leer(string rutaCompleta)
      {
        string end;
        try
        {
          StreamReader streamReader = new StreamReader(rutaCompleta);
          end = streamReader.ReadToEnd();
          streamReader.Close();
        }
        catch (FileNotFoundException ex)
        {
          return (string) null;
        }
        catch (Exception ex)
        {
          throw new Exception(ex.Message);
        }
        return end;
      }
    }

    public class Texto
    {
      public static string LeerArchivo(string rutaCompleta)
      {
        return Archivos.Texto.LeerArchivo(rutaCompleta, Encoding.Default);
      }

      public static string LeerArchivo(string rutaCompleta, Encoding encoding)
      {
        StreamReader streamReader = (StreamReader) null;
        string str = string.Empty;
        try
        {
          if (File.Exists(rutaCompleta))
          {
            streamReader = new StreamReader(rutaCompleta, encoding);
            str = streamReader.ReadToEnd();
          }
        }
        catch (Exception ex)
        {
          throw new Exception(ex.Message);
        }
        finally
        {
          streamReader.Close();
        }
        return str;
      }

      public static bool EscribirArchivo(string rutaCompleta, string contenido, bool agregar)
      {
        return Archivos.Texto.EscribirArchivo(rutaCompleta, contenido, agregar, Encoding.Default);
      }

      public static bool EscribirArchivo(
        string rutaCompleta,
        string contenido,
        bool agregar,
        Encoding encoding)
      {
        StreamWriter streamWriter = (StreamWriter) null;
        try
        {
          streamWriter = new StreamWriter(rutaCompleta, agregar, encoding);
          streamWriter.Write(contenido);
          streamWriter.Flush();
          return true;
        }
        catch (Exception ex)
        {
          throw new Exception(ex.Message);
        }
        finally
        {
          streamWriter.Close();
        }
      }
    }

    public class Imagenes
    {
      public static Image Cargar(string rutaNombre)
      {
        Bitmap bitmap1 = (Bitmap) null;
        try
        {
          Image original = Image.FromFile(rutaNombre);
          Bitmap bitmap2 = new Bitmap(original);
          bitmap1 = new Bitmap(original.Width, original.Height, PixelFormat.Format32bppRgb);
          Graphics graphics = Graphics.FromImage((Image) bitmap1);
          graphics.DrawImage((Image) bitmap2, 0, 0);
          graphics.Dispose();
          bitmap2.Dispose();
        }
        catch (FileLoadException ex)
        {
        }
        catch
        {
          throw;
        }
        return (Image) bitmap1;
      }

      public static bool Guardar(string rutaNombre, Image imagen)
      {
        try
        {
          Bitmap bitmap1 = new Bitmap(imagen);
          Bitmap bitmap2 = new Bitmap(imagen.Width, imagen.Height, PixelFormat.Format32bppRgb);
          Graphics graphics = Graphics.FromImage((Image) bitmap2);
          graphics.DrawImage((Image) bitmap1, 0, 0);
          graphics.Dispose();
          bitmap1.Dispose();
          if (!Directory.GetParent(rutaNombre).Exists)
            throw new IOException(string.Format("Ruta inexistente o inaccesible: {0}", (object) Directory.GetParent(rutaNombre).FullName));
          if (File.Exists(rutaNombre))
            File.Delete(rutaNombre);
          bitmap2.Save(rutaNombre);
          return true;
        }
        catch
        {
          throw;
        }
      }

      public static void CambiarNombreImagen(string nombreViejoCompleto, string nombreNuevoCompleto)
      {
        try
        {
          if (!File.Exists(nombreViejoCompleto))
            return;
          File.Move(nombreViejoCompleto, nombreNuevoCompleto);
          File.Delete(nombreViejoCompleto);
        }
        catch (Exception ex)
        {
          throw ex;
        }
      }

      public static Archivos.Imagenes.ImagenInfo ObtenerImagenInfo(
        List<Archivos.Imagenes.ImagenInfo> imagenesInfo,
        string ruta)
      {
        foreach (Archivos.Imagenes.ImagenInfo imagenInfo in imagenesInfo)
        {
          if (imagenInfo.Ruta == ruta)
            return imagenInfo;
        }
        return (Archivos.Imagenes.ImagenInfo) null;
      }

      public static Image CrearImagenTamanioFijo(Image imagen, int ancho, int alto)
      {
        return Archivos.Imagenes.CrearImagenTamanioFijo(imagen, ancho, alto, Color.Transparent);
      }

      public static Image CrearImagenTamanioFijo(
        Image imagen,
        int ancho,
        int alto,
        Color colorComplementario)
      {
        Bitmap bitmap;
        try
        {
          int width1 = imagen.Width;
          int height1 = imagen.Height;
          int x1 = 0;
          int y1 = 0;
          int x2 = 0;
          int y2 = 0;
          float num1 = (float) ancho / (float) width1;
          float num2 = (float) alto / (float) height1;
          float num3;
          if ((double) num2 < (double) num1)
          {
            num3 = num2;
            x2 = (int) Convert.ToInt16((float) (((double) ancho - (double) width1 * (double) num3) / 2.0));
          }
          else
          {
            num3 = num1;
            y2 = (int) Convert.ToInt16((float) (((double) alto - (double) height1 * (double) num3) / 2.0));
          }
          int width2 = (int) ((double) width1 * (double) num3);
          int height2 = (int) ((double) height1 * (double) num3);
          bitmap = new Bitmap(ancho, alto, PixelFormat.Format32bppArgb);
          bitmap.SetResolution(imagen.HorizontalResolution, imagen.VerticalResolution);
          Graphics graphics = Graphics.FromImage((Image) bitmap);
          graphics.Clear(colorComplementario);
          graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
          graphics.DrawImage(imagen, new Rectangle(x2, y2, width2, height2), new Rectangle(x1, y1, width1, height1), GraphicsUnit.Pixel);
          graphics.Dispose();
        }
        catch
        {
          throw;
        }
        return (Image) bitmap;
      }

      public static Bitmap CrearImagenStretch(Image imagen, int nuevoAncho, int nuevoAlto)
      {
        return Archivos.Imagenes.CrearImagenStretch(imagen, nuevoAncho, nuevoAlto, 96, 96);
      }

      public static Bitmap CrearImagenStretch(
        Image imagen,
        int nuevoAncho,
        int nuevoAlto,
        int dpiX,
        int dpiY)
      {
        Bitmap bitmap;
        try
        {
          bitmap = new Bitmap(imagen, nuevoAncho, nuevoAlto);
          bitmap.SetResolution((float) dpiX, (float) dpiY);
        }
        catch
        {
          throw;
        }
        return bitmap;
      }

      public class ImagenesInfo : List<Archivos.Imagenes.ImagenInfo>
      {
        public string MensajeError;
      }

      [Serializable]
      public class ImagenInfo
      {
        private string ruta = string.Empty;
        private Image bitMap;

        public ImagenInfo()
        {
        }

        public ImagenInfo(Image bitMap)
        {
          this.bitMap = bitMap;
        }

        public ImagenInfo(string ruta)
        {
          this.ruta = ruta;
        }

        public ImagenInfo(Image bitMap, string ruta)
        {
          this.bitMap = bitMap;
          this.ruta = ruta;
        }

        public Image BitMap
        {
          set
          {
            this.bitMap = value;
          }
          get
          {
            return this.bitMap;
          }
        }

        public string Ruta
        {
          set
          {
            this.ruta = value;
          }
          get
          {
            return this.ruta;
          }
        }

        public static bool Comparar(
          List<Archivos.Imagenes.ImagenInfo> imagenesOriginales,
          List<Archivos.Imagenes.ImagenInfo> imagenesActuales)
        {
          bool flag = true;
          try
          {
            if (imagenesOriginales == null && imagenesActuales == null)
              flag = true;
            else if (imagenesActuales == null && imagenesOriginales != null || imagenesActuales != null && imagenesOriginales == null)
              flag = false;
            else if (imagenesActuales.Count != imagenesOriginales.Count)
            {
              flag = false;
            }
            else
            {
              for (int index = 0; index < imagenesActuales.Count; ++index)
              {
                if (!imagenesActuales[index].BitMap.Equals((object) imagenesOriginales[index].BitMap))
                {
                  flag = false;
                  break;
                }
              }
            }
          }
          catch
          {
            throw;
          }
          return flag;
        }
      }
    }
  }
}
