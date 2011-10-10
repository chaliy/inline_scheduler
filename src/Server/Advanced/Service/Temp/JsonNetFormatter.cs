using System;
using System.IO;
using System.Net;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace WebApiContrib.Formatters.JsonNet
{
    /// <summary>
    /// Formats requests for text/json and application/json using Json.Net.
    /// </summary>
    /// <remarks>
    /// Christian Weyer is the author of this MediaTypeProcessor.
    /// <see href="http://weblogs.thinktecture.com/cweyer/2010/12/using-jsonnet-as-a-default-serializer-in-wcf-httpwebrest-vnext.html"/>
    /// Daniel Cazzulino (kzu): 
    ///		- updated to support in a single processor both binary and text Json. 
    ///		- fixed to support query composition services properly.
    /// Darrel Miller
    ///     - Converted to Preview 4 MediaTypeFormatter
    /// </remarks>
    public class JsonNetFormatter : JsonMediaTypeFormatter
    {        
        public JsonNetFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/json"));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));                        
        }

        protected override object OnReadFromStream(Type type, Stream stream, HttpContentHeaders contentHeaders)
        {
            var serializer = CreateSerializer();
            var reader = new JsonTextReader(new StreamReader(stream));
            
            var result = serializer.Deserialize(reader, type);

            return result;
        }        

        protected override void OnWriteToStream(Type type, object value, Stream stream, HttpContentHeaders contentHeaders, TransportContext context)
        {
            var serializer = CreateSerializer();
            // NOTE: we don't dispose or close these as they would 
            // close the stream, which is used by the rest of the pipeline.
            var writer = new JsonTextWriter(new StreamWriter(stream));                
            
            serializer.Serialize(writer, value);            
            writer.Flush();

        }

        private static JsonSerializer CreateSerializer()
        {
            var serializer = new JsonSerializer { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            serializer.Converters.Add(new StringEnumConverter());
            serializer.Converters.Add(new IsoDateTimeConverter());
            return serializer;
        }
    }
}
