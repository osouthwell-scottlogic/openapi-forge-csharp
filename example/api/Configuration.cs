using System;
using System.Collections.Generic;
using System.Text;

namespace OpenApiForge
{
    public class Configuration
    {
        public string[] Servers { get; set; } = new string[] { "/api/v3" };

        public string BasePath { get; set; }

        public string BearerToken { get; set; }

        public int SelectedServerIndex { get; set; } = 0;

        public virtual string GetBaseAddress()
        {
            var sb = RemoveTrailingSlash(new StringBuilder(Servers[SelectedServerIndex]));

            if (!string.IsNullOrWhiteSpace(BasePath))
            {
                sb.Append(BasePath.StartsWith("/")
                    ? BasePath
                    : $"/{BasePath}");
            }

            return RemoveTrailingSlash(sb).ToString();
        }

        private static StringBuilder RemoveTrailingSlash(StringBuilder sb)
        {
            return sb.Length > 0 && sb[sb.Length - 1] == '/'
                ? sb.Remove(sb.Length - 1, 1)
                : sb;
        }
    }
}