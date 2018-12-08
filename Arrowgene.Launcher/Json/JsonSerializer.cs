using System;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Xml;

namespace Arrowgene.Launcher.Json
{
    public class JsonSerializer
    {
        public static T Deserialize<T>(string json)
        {
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            T obj = default(T);
            try
            {
                using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
                {
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
                    obj = (T)serializer.ReadObject(stream);
                    stream.Close();
                    Thread.CurrentThread.CurrentCulture = currentCulture;
                }
            }
            catch (Exception exception)
            {
                App.Logger.Log(exception, "JsonSerializer::Deserialize");
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = currentCulture;
            }

            return obj;
        }

        public static string Serialize<T>(T obj)
        {
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            string json = null;
            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    using (XmlDictionaryWriter writer = JsonReaderWriterFactory.CreateJsonWriter(stream, Encoding.UTF8))
                    {
                        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
                        serializer.WriteObject(writer, obj);
                        writer.Flush();
                    }

                    byte[] jsonBytes = stream.ToArray();
                    json = Encoding.UTF8.GetString(jsonBytes, 0, jsonBytes.Length);
                }
            }
            catch (Exception exception)
            {
                App.Logger.Log(exception, "JsonSerializer::Serialize");
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = currentCulture;
            }

            return json;
        }
    }
}