using System.Security.Cryptography;
using System.Text;

namespace Qiniu.Util
{
	public class Hashing
	{
		public static byte[] CalcSHA1(byte[] data)
		{
			return SHA1.Create().ComputeHash(data);
		}

		public static string CalcMD5(string str)
		{
			MD5 mD = MD5.Create();
			byte[] bytes = Encoding.UTF8.GetBytes(str);
			byte[] array = mD.ComputeHash(bytes);
			StringBuilder stringBuilder = new StringBuilder(array.Length * 2);
			byte[] array2 = array;
			foreach (byte b in array2)
			{
				stringBuilder.AppendFormat("{0:x2}", b);
			}
			return stringBuilder.ToString();
		}

		public static string CalcMD5X(string str)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(str);
			return new LabMD5().ComputeHash(bytes);
		}
	}
}
