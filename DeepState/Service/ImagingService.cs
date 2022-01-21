using System;
using System.IO;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.Extensions.Logging;

using System.Linq;
using SkiaSharp;

namespace DeepState.Service
{
	public class ImagingService
	{
		public static string DOPublicKey { get; set; }
		public static string DOSecretKey { get; set; }
		public static string DOUrl { get; set; }
		public static string DOBucket { get; set; }
		public static ILogger Log { get; set; }
		public static Random _rand { get; set; }

		public ImagingService()
		{
			DOPublicKey = Environment.GetEnvironmentVariable("DOPUBLIC");
			DOSecretKey = Environment.GetEnvironmentVariable("DOSECRET");
			DOUrl = Environment.GetEnvironmentVariable("DOURL");
			DOBucket = Environment.GetEnvironmentVariable("DOBUCKET");
			_rand = new Random(Guid.NewGuid().GetHashCode());
		}
		public string UploadImage(string folderName, Stream ImageStream)
		{
			string filename = $"{Guid.NewGuid()}.png";
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

		public async Task<SKBitmap> GenerateJuliaSetImage(int width, int height, bool rainbowMode)
		{
			SKBitmap bitmap = new SKBitmap(width, height);
			double zoom = (15 * _rand.NextDouble()) +1;
			int maxiter = 255;
			double moveX = 0;
			double moveY = 0;
			double cX = -0.7 - (0.001 * _rand.NextDouble());
			double cY = 0.27015 + (0.001 * _rand.NextDouble());
			double zx, zy, tmp;
			int i;
			SKColor[] colors;

			if (rainbowMode) {
				colors = (from c in Enumerable.Range(0, 256)
						  select new SKColor((byte)((c >> _rand.Next(255)) * 36), (byte)((c >> _rand.Next(255)) * 36), (byte)((c & _rand.Next(255)) * 85))).ToArray();
			}
			else
			{
				int red = _rand.Next(255);
				int green = _rand.Next(255);				
				int blue = _rand.Next(255);
				colors = (from c in Enumerable.Range(0, 256)
				   select new SKColor((byte)((c >> red) * 36), (byte)((c >> green) * 36), (byte)((c & blue) * 85))).ToArray();
			}

				var calculatedPoints = Enumerable.Range(0, width * height).AsParallel().Select(xy =>
			{
				double zx, zy, tmp;
				int x, y;
				int i = maxiter;
				y = xy / width;
				x = xy % width;
				zx = 1.5 * (x - width / 2) / (0.5 * zoom * width) + moveX;
				zy = 1.0 * (y - height / 2) / (0.5 * zoom * height) + moveY;
				while (zx * zx + zy * zy < 4 && i > 1)
				{
					tmp = zx * zx - zy * zy + cX;
					zy = 2.0 * zx * zy + cY;
					zx = tmp;
					i -= 1;
				}
				return new CalculatedPoint { xCor = x, yCor = y, intensity = i };
			});

			foreach (CalculatedPoint cp in calculatedPoints)
				bitmap.SetPixel(cp.xCor, cp.yCor, colors[cp.intensity]);

			return bitmap;
		}
	}
	public class CalculatedPoint
	{
		public int xCor;
		public int yCor;
		public int intensity;
	}
}
