using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DeepState.Service
{
	public class ImagingService
	{
		public async Task<string> GetBase64ImageFromURL(string url){
			using (var client = new HttpClient())
			{
				byte[] bytes = await client.GetByteArrayAsync(url);
				return Convert.ToBase64String(bytes);
			}
		}
	}
}
