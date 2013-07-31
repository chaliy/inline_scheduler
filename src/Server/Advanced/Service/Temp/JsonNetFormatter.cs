using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace WebApiContrib.Formatters.JsonNet
{
    /// Used this implementation: http://code.msdn.microsoft.com/Using-JSONNET-with-ASPNET-b2423706
    public class JsonNetFormatter : JsonMediaTypeFormatter
    {
        readonly Encoding _defaultEncoding = new UTF8Encoding();

        public JsonNetFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/json"));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
        }

        public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        {
            return Task.Factory.StartNew(() =>
            {
                var serializer = CreateSerializer();
                var streamReader = new StreamReader(readStream, _defaultEncoding);
                var jsonTextReader = new JsonTextReader(streamReader);
                var deserialized = serializer.Deserialize(jsonTextReader, type);
                return deserialized;
            });
        }

        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, TransportContext transportContext)
        {
            return Task.Factory.StartNew(() =>
            {
                var serializer = CreateSerializer();

                var streamWriter = new StreamWriter(writeStream, _defaultEncoding);
                var jsonTextWriter = new JsonTextWriter(streamWriter);

                serializer.Serialize(jsonTextWriter, value);

                jsonTextWriter.Flush();
                streamWriter.Flush();
            });
        }

        static JsonSerializer CreateSerializer()
        {
            var serializer = new JsonSerializer { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            serializer.Converters.Add(new StringEnumConverter());
            serializer.Converters.Add(new IsoDateTimeConverter());
            return serializer;
        }
    }
}
