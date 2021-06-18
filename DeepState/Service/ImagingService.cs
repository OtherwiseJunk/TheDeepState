using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.Extensions.Logging;

namespace DeepState.Service
{
	public class ImagingService
	{
		public static string DOPublicKey { get; set; }
		public static string DOSecretKey { get; set; }
		public static string DOUrl { get; set; }
		public static string DOBucket { get; set; }
		public static ILogger Log { get; set; }

		public ImagingService()
		{
			DOPublicKey = Environment.GetEnvironmentVariable("DOPUBLIC");
			DOSecretKey = Environment.GetEnvironmentVariable("DOSECRET");
			DOUrl = Environment.GetEnvironmentVariable("DOURL");
			DOBucket = Environment.GetEnvironmentVariable("DOBUCKET");
		}
		public string UploadImage(string folderName, Stream ImageStream)
		{
			
			Guid guid = Guid.NewGuid();
			string filename = $"{guid.ToString()}.png";
			AmazonS3Config s3ClientConfig = new AmazonS3Config
			{
				ServiceURL = DOUrl,
			};
			using(AmazonS3Client client = new AmazonS3Client(DOPublicKey,DOSecretKey,s3ClientConfig))
			{
				using (TransferUtility fileTransferUtility = new TransferUtility(client)){
					try
					{
						TransferUtilityUploadRequest request = new TransferUtilityUploadRequest
						{
							BucketName = $"{DOBucket}/{folderName}",
							InputStream = ImageStream,
							Key = filename,
							CannedACL = S3CannedACL.PublicRead
						};
						fileTransferUtility.Upload(request);
						return $"https://{DOBucket}.nyc3.cdn.digitaloceanspaces.com/{folderName}/{filename}";
					}
					catch (AmazonS3Exception e)
					{
						Console.WriteLine("Error encountered ***. Message:'{0}' when writing an object", e.Message);
					}
					catch (Exception e)
					{
						Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
					}
				}
			}
			return null;
		}

		public async void DeleteImage(string folderName, string key)
		{
			AmazonS3Config s3ClientConfig = new AmazonS3Config
			{
				ServiceURL = DOUrl,
			};
			using (AmazonS3Client client = new AmazonS3Client(DOPublicKey, DOSecretKey, s3ClientConfig))
			{
				try
				{
					await client.DeleteObjectAsync($"{DOBucket}/{folderName}", key);
				}
				catch (AmazonS3Exception e)
				{
					Console.WriteLine("Error encountered ***. Message:'{0}' when deleting an object", e.Message);
				}
				catch (Exception e)
				{
					Console.WriteLine("Unknown encountered on server. Message:'{0}' when deleting an object", e.Message);
				}
			}
		}

		public async Task<string> GetBase64ImageFromURL(string url){
			using (var client = new HttpClient())
			{
				byte[] bytes = await client.GetByteArrayAsync(url);
				return Convert.ToBase64String(bytes);
			}
		}

	}
}
