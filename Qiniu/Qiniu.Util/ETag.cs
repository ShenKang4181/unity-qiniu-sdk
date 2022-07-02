using System;
using System.IO;

namespace Qiniu.Util
{
	public class ETag
	{
		private const int BLOCK_SIZE = 4194304;

		private static int BLOCK_SHA1_SIZE = 20;

		public static string CalcHash(string filePath)
		{
			string result = "";
			try
			{
				using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
				{
					long length = fileStream.Length;
					byte[] array = new byte[4194304];
					byte[] array2 = new byte[BLOCK_SHA1_SIZE + 1];
					if (length <= 4194304)
					{
						int num = fileStream.Read(array, 0, 4194304);
						byte[] array3 = new byte[num];
						Array.Copy(array, array3, num);
						byte[] array4 = Hashing.CalcSHA1(array3);
						array2[0] = 22;
						Array.Copy(array4, 0, array2, 1, array4.Length);
					}
					else
					{
						long num2 = ((length % 4194304 == 0L) ? (length / 4194304) : (length / 4194304 + 1));
						byte[] array5 = new byte[BLOCK_SHA1_SIZE * num2];
						for (int i = 0; i < num2; i++)
						{
							int num3 = fileStream.Read(array, 0, 4194304);
							byte[] array6 = new byte[num3];
							Array.Copy(array, array6, num3);
							byte[] array7 = Hashing.CalcSHA1(array6);
							Array.Copy(array7, 0, array5, i * BLOCK_SHA1_SIZE, array7.Length);
						}
						byte[] array8 = Hashing.CalcSHA1(array5);
						array2[0] = 150;
						Array.Copy(array8, 0, array2, 1, array8.Length);
					}
					result = Base64.UrlSafeBase64Encode(array2);
					return result;
				}
			}
			catch (Exception)
			{
				return result;
			}
		}
	}
}
