using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;

namespace InlineScheduler.Server.Advanced.Service.Content
{
    public static class Accessor
    {
        public static HttpResponseMessage Get(string path)
        {
            // index.html            
            // ccs/style.css

            if (String.IsNullOrEmpty(path))
            {
                path = "index.html";
            }

            var clearPath = Regex.Replace(path, "\\?[^\\?]*", "");
            var extension = Regex.Match(clearPath, "\\.[^\\.\\?]*$").Value.ToLower();

            var resourceName = typeof(Accessor).Namespace + "." + clearPath.Replace("/", ".");
            var raw = typeof (Accessor).Assembly.GetManifestResourceStream(resourceName);
            if (raw == null)
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.NotFound);
            }

            var content = new StreamContent(raw);

            switch (extension)
            {
                case ".css":
                    content.Headers.ContentType = new MediaTypeHeaderValue("text/css");
                    break;

                case ".js":
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/javascript");
                    break;

                case ".gif":
                    content.Headers.ContentType = new MediaTypeHeaderValue("image/gif");
                    break;

                case ".png":
                    content.Headers.ContentType = new MediaTypeHeaderValue("image/png");
                    break;
            }

            return new HttpResponseMessage { Content = content };            
        }
    }
}
