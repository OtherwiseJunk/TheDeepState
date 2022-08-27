using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DeepState.Utilities
{
    public static class ExtensionMethods
    {
        public static void AddJWTAuthorization(this HttpRequestMessage req, Func<string> callback)
        {
            req.Headers.Add("Authorization", $"Bearer {callback()}");
        }
    }
}
