using DeepState.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DeepState.Services
{
    public class WaffleHouseApocalypseService
    {
        public StoresClosedResponse GetStoresClosed()
        {
            StoresClosedResponse response;
            using (WebClient wc = new WebClient())
            {
                string jsonResponse = wc.DownloadString("https://wafflehouseindex.live/stores/closed");
                response = JsonSerializer.Deserialize<StoresClosedResponse>(jsonResponse);
            }

            return response;
        }
    }
}
