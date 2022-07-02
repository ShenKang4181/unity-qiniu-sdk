using System;
using System.IO;
using System.Text;
using Qiniu.Common;
using Qiniu.Http;
using Qiniu.RSF.Model;
using Qiniu.Util;

namespace Qiniu.RSF
{
	public class OperationManager
	{
		private Auth auth;

		private HttpManager httpManager;

		public OperationManager(Mac mac)
		{
			auth = new Auth(mac);
			httpManager = new HttpManager(false);
		}

		public PfopResult Pfop(string bucket, string key, string fops, string pipeline, string notifyUrl, bool force)
		{
			PfopResult pfopResult = new PfopResult();
			try
			{
				string url = Config.ZONE.ApiHost + "/pfop/";
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat("bucket={0}&key={1}&fops={2}", bucket, key, StringHelper.UrlEncode(fops));
				if (!string.IsNullOrEmpty(notifyUrl))
				{
					stringBuilder.AppendFormat("&notifyURL={0}", notifyUrl);
				}
				if (force)
				{
					stringBuilder.Append("&force=1");
				}
				if (!string.IsNullOrEmpty(pipeline))
				{
					stringBuilder.AppendFormat("&pipeline={0}", pipeline);
				}
				byte[] bytes = Encoding.UTF8.GetBytes(stringBuilder.ToString());
				string token = auth.CreateManageToken(url, bytes);
				HttpResult hr = httpManager.PostForm(url, bytes, token);
				pfopResult.Shadow(hr);
				return pfopResult;
			}
			catch (Exception ex)
			{
				StringBuilder stringBuilder2 = new StringBuilder();
				stringBuilder2.AppendFormat("[{0}] [pfop] Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
				for (Exception ex2 = ex; ex2 != null; ex2 = ex2.InnerException)
				{
					stringBuilder2.Append(ex2.Message + " ");
				}
				stringBuilder2.AppendLine();
				pfopResult.RefCode = -252;
				pfopResult.RefText += stringBuilder2.ToString();
				return pfopResult;
			}
		}

		public PfopResult Pfop(string bucket, string key, string[] fops, string pipeline, string notifyUrl, bool force)
		{
			string fops2 = string.Join(";", fops);
			return Pfop(bucket, key, fops2, pipeline, notifyUrl, force);
		}

		public static HttpResult Prefop(string persistentId)
		{
			HttpResult httpResult = new HttpResult();
			try
			{
				string url = string.Format("{0}/status/get/prefop?id={1}", Config.ZONE.ApiHost, persistentId);
				httpResult = new HttpManager(false).Get(url, null);
				return httpResult;
			}
			catch (Exception ex)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat("[{0}] [prefop] Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
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

		public HttpResult Dfop(string fop, string uri)
		{
			if (UrlHelper.IsValidUrl(uri))
			{
				return DfopUrl(fop, uri);
			}
			return DfopData(fop, uri);
		}

		public HttpResult DfopText(string fop, string text)
		{
			HttpResult httpResult = new HttpResult();
			try
			{
				string url = string.Format("{0}/dfop?fop={1}", "http://api.qiniu.com", fop);
				string token = auth.CreateManageToken(url);
				string text2 = HttpManager.CreateFormDataBoundary();
				string text3 = "--" + text2;
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine(text3);
				stringBuilder.AppendFormat("Content-Type: {0}", ContentType.TEXT_PLAIN);
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("Content-Disposition: form-data; name=data; filename=text");
				stringBuilder.AppendLine();
				stringBuilder.AppendLine(text);
				stringBuilder.AppendLine(text3 + "--");
				byte[] bytes = Encoding.UTF8.GetBytes(stringBuilder.ToString());
				httpResult = httpManager.PostMultipart(url, bytes, text2, token, true);
				return httpResult;
			}
			catch (Exception ex)
			{
				StringBuilder stringBuilder2 = new StringBuilder();
				stringBuilder2.AppendFormat("[{0}] [dfop] Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
				for (Exception ex2 = ex; ex2 != null; ex2 = ex2.InnerException)
				{
					stringBuilder2.Append(ex2.Message + " ");
				}
				stringBuilder2.AppendLine();
				httpResult.RefCode = -252;
				httpResult.RefText += stringBuilder2.ToString();
				return httpResult;
			}
		}

		public HttpResult DfopTextFile(string fop, string textFile)
		{
			HttpResult httpResult = new HttpResult();
			if (File.Exists(textFile))
			{
				httpResult = DfopText(fop, File.ReadAllText(textFile));
			}
			else
			{
				httpResult.RefCode = -252;
				httpResult.RefText = "[dfop-error] File not found: " + textFile;
			}
			return httpResult;
		}

		public HttpResult DfopUrl(string fop, string url)
		{
			HttpResult httpResult = new HttpResult();
			try
			{
				string arg = StringHelper.UrlEncode(url);
				string url2 = string.Format("{0}/dfop?fop={1}&url={2}", "http://api.qiniu.com", fop, arg);
				string token = auth.CreateManageToken(url2);
				httpResult = httpManager.Post(url2, token, true);
				return httpResult;
			}
			catch (Exception ex)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat("[{0}] [dfop] Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
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

		public HttpResult DfopData(string fop, string localFile)
		{
			HttpResult httpResult = new HttpResult();
			try
			{
				string url = string.Format("{0}/dfop?fop={1}", "http://api.qiniu.com", fop);
				string token = auth.CreateManageToken(url);
				string text = HttpManager.CreateFormDataBoundary();
				string text2 = "--" + text;
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine(text2);
				string fileName = Path.GetFileName(localFile);
				stringBuilder.AppendFormat("Content-Type: {0}", ContentType.APPLICATION_OCTET_STREAM);
				stringBuilder.AppendLine();
				stringBuilder.AppendFormat("Content-Disposition: form-data; name=\"data\"; filename={0}", fileName);
				stringBuilder.AppendLine();
				stringBuilder.AppendLine();
				StringBuilder stringBuilder2 = new StringBuilder();
				stringBuilder2.AppendLine();
				stringBuilder2.AppendLine(text2 + "--");
				byte[] bytes = Encoding.UTF8.GetBytes(stringBuilder.ToString());
				byte[] array = File.ReadAllBytes(localFile);
				byte[] bytes2 = Encoding.UTF8.GetBytes(stringBuilder2.ToString());
				MemoryStream memoryStream = new MemoryStream();
				memoryStream.Write(bytes, 0, bytes.Length);
				memoryStream.Write(array, 0, array.Length);
				memoryStream.Write(bytes2, 0, bytes2.Length);
				httpResult = httpManager.PostMultipart(url, memoryStream.ToArray(), text, token, true);
				return httpResult;
			}
			catch (Exception ex)
			{
				StringBuilder stringBuilder3 = new StringBuilder();
				stringBuilder3.AppendFormat("[{0}] [dfop] Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
				for (Exception ex2 = ex; ex2 != null; ex2 = ex2.InnerException)
				{
					stringBuilder3.Append(ex2.Message + " ");
				}
				stringBuilder3.AppendLine();
				httpResult.RefCode = -252;
				httpResult.RefText += stringBuilder3.ToString();
				return httpResult;
			}
		}
	}
}
