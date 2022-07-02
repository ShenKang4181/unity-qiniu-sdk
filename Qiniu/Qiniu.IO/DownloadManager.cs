using System;
using System.IO;
using System.Text;
using Qiniu.Http;
using Qiniu.Util;

namespace Qiniu.IO
{
	public class DownloadManager
	{
		public static string CreateSignedUrl(Mac mac, string url, int expireInSeconds = 3600)
		{
			long unixTimestamp = UnixTimestamp.GetUnixTimestamp(expireInSeconds);
			StringBuilder stringBuilder = new StringBuilder(url);
			if (url.Contains("?"))
			{
				stringBuilder.AppendFormat("&e={0}", unixTimestamp);
			}
			else
			{
				stringBuilder.AppendFormat("?e={0}", unixTimestamp);
			}
			string arg = Auth.CreateDownloadToken(mac, stringBuilder.ToString());
			stringBuilder.AppendFormat("&token={0}", arg);
			return stringBuilder.ToString();
		}

		public static HttpResult Download(string url, string saveasFile)
		{
			HttpResult httpResult = new HttpResult();
			try
			{
				httpResult = new HttpManager(false).Get(url, null, true);
				if (httpResult.Code == 200)
				{
					using (FileStream fileStream = File.Create(saveasFile, httpResult.Data.Length))
					{
						fileStream.Write(httpResult.Data, 0, httpResult.Data.Length);
						fileStream.Flush();
					}
					httpResult.RefText += string.Format("[{0}] [Download] Success: (Remote file) ==> \"{1}\"\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), saveasFile);
					return httpResult;
				}
				httpResult.RefText += string.Format("[{0}] [Download] Error: code = {1}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), httpResult.Code);
				return httpResult;
			}
			catch (Exception ex)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat("[{0}] [Download] Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
				for (Exception ex2 = ex; ex2 != null; ex2 = ex2.InnerException)
				{
					stringBuilder.Append(ex2.Message + " ");
				}
				stringBuilder.AppendLine();
				httpResult.RefCode = -252;
				httpResult.RefText += stringBuilder.ToString();
				return httpResult;
			}
		}
	}
}
