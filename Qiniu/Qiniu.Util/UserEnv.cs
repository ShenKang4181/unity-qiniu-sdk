using System;
using System.IO;

namespace Qiniu.Util
{
	public class UserEnv
	{
		public static string GetHomeFolder()
		{
			string text = Environment.GetEnvironmentVariable("HOMEDRIVE") + Environment.GetEnvironmentVariable("HOMEPATH");
			if (string.IsNullOrEmpty(text))
			{
				text = Path.GetFullPath(".");
			}
			string text2 = Path.Combine(text, "QHome");
			if (!Directory.Exists(text2))
			{
				Directory.CreateDirectory(text2);
			}
			return text2;
		}
	}
}
