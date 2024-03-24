using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DeepState.Extensions
{
    public static class PanopticonExtensionMethods
    {
        public static void AddJWTAuthorization(this HttpRequestMessage req, Func<string> callback)
        {
            req.Headers.Add("Authorization", $"Bearer {callback()}");
        }
    }
}
