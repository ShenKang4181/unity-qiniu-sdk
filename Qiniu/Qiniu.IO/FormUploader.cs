using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Qiniu.Common;
using Qiniu.Http;

namespace Qiniu.IO
{
	public class FormUploader
	{
		private string uploadHost;

		private HttpManager httpManager;

		public FormUploader(bool uploadFromCDN = false)
		{
			httpManager = new HttpManager(false);
			uploadHost = (uploadFromCDN ? Config.ZONE.UploadHost : Config.ZONE.UpHost);
		}

		public HttpResult UploadFile(string localFile, string saveKey, string token)
		{
			HttpResult httpResult = new HttpResult();
			try
			{
				string text = HttpManager.CreateFormDataBoundary();
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("--" + text);
				stringBuilder.AppendLine("Content-Disposition: form-data; name=key");
				stringBuilder.AppendLine();
				stringBuilder.AppendLine(saveKey);
				stringBuilder.AppendLine("--" + text);
				stringBuilder.AppendLine("Content-Disposition: form-data; name=token");
				stringBuilder.AppendLine();
				stringBuilder.AppendLine(token);
				stringBuilder.AppendLine("--" + text);
				stringBuilder.AppendFormat("Content-Disposition: form-data; name=file; filename=\"{0}\"", saveKey);
				stringBuilder.AppendLine();
				stringBuilder.AppendLine();
				StringBuilder stringBuilder2 = new StringBuilder();
				stringBuilder2.AppendLine();
				stringBuilder2.AppendLine("--" + text + "--");
				byte[] bytes = Encoding.UTF8.GetBytes(stringBuilder.ToString());
				byte[] array = File.ReadAllBytes(localFile);
				byte[] bytes2 = Encoding.UTF8.GetBytes(stringBuilder2.ToString());
				MemoryStream memoryStream = new MemoryStream();
				memoryStream.Write(bytes, 0, bytes.Length);
				memoryStream.Write(array, 0, array.Length);
				memoryStream.Write(bytes2, 0, bytes2.Length);
				httpResult = httpManager.PostMultipart(uploadHost, memoryStream.ToArray(), text, null);
				if (httpResult.Code == 200)
				{
					httpResult.RefText += string.Format("[{0}] [FormUpload] Uploaded: \"{1}\" ==> \"{2}\"\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), localFile, saveKey);
					return httpResult;
				}
				httpResult.RefText += string.Format("[{0}] [FormUpload] Failed: code = {1}, text = {2}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), httpResult.Code, httpResult.Text);
				return httpResult;
			}
			catch (Exception ex)
			{
				StringBuilder stringBuilder3 = new StringBuilder();
				stringBuilder3.AppendFormat("[{0}] [FormUpload] Error: ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
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

		public HttpResult UploadFile(string localFile, string saveKey, string token, Dictionary<string, string> extraParams)
		{
			HttpResult httpResult = new HttpResult();
			try
			{
				string text = HttpManager.CreateFormDataBoundary();
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("--" + text);
				stringBuilder.AppendLine("Content-Disposition: form-data; name=key");
				stringBuilder.AppendLine();
				stringBuilder.AppendLine(saveKey);
				stringBuilder.AppendLine("--" + text);
				stringBuilder.AppendLine("Content-Disposition: form-data; name=token");
				stringBuilder.AppendLine();
				stringBuilder.AppendLine(token);
				stringBuilder.AppendLine("--" + text);
				foreach (KeyValuePair<string, string> extraParam in extraParams)
				{
					stringBuilder.AppendFormat("Content-Disposition: form-data; name=\"{0}\"", extraParam.Key);
					stringBuilder.AppendLine();
					stringBuilder.AppendLine();
					stringBuilder.AppendLine(extraParam.Value);
					stringBuilder.AppendLine("--" + text);
				}
				stringBuilder.AppendFormat("Content-Disposition: form-data; name=file; filename=\"{0}\"", saveKey);
				stringBuilder.AppendLine();
				stringBuilder.AppendLine();
				StringBuilder stringBuilder2 = new StringBuilder();
				stringBuilder2.AppendLine();
				stringBuilder2.AppendLine("--" + text + "--");
				byte[] bytes = Encoding.UTF8.GetBytes(stringBuilder.ToString());
				byte[] array = File.ReadAllBytes(localFile);
				byte[] bytes2 = Encoding.UTF8.GetBytes(stringBuilder2.ToString());
				MemoryStream memoryStream = new MemoryStream();
				memoryStream.Write(bytes, 0, bytes.Length);
				memoryStream.Write(array, 0, array.Length);
				memoryStream.Write(bytes2, 0, bytes2.Length);
				httpResult = httpManager.PostMultipart(uploadHost, memoryStream.ToArray(), text, null);
				if (httpResult.Code == 200)
				{
					httpResult.RefText += string.Format("[{0}] [FormUpload] Uploaded: \"{1}\" ==> \"{2}\"\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), localFile, saveKey);
					return httpResult;
				}
				httpResult.RefText += string.Format("[{0}] [FormUpload] Failed: code = {1}, text = {2}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), httpResult.Code, httpResult.Text);
				return httpResult;
			}
			catch (Exception ex)
			{
				StringBuilder stringBuilder3 = new StringBuilder();
				stringBuilder3.AppendFormat("[{0}] [FormUpload] Error: ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
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

		public static byte[] ReadToByteArray(string file)
		{
			byte[] array = null;
			using (FileStream fileStream = new FileStream(file, FileMode.Open))
			{
				array = new byte[fileStream.Length];
				fileStream.Read(array, 0, (int)fileStream.Length);
				return array;
			}
		}

		public HttpResult UploadData(byte[] data, string saveKey, string token)
		{
			HttpResult httpResult = new HttpResult();
			try
			{
				string text = HttpManager.CreateFormDataBoundary();
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("--" + text);
				stringBuilder.AppendLine("Content-Disposition: form-data; name=key");
				stringBuilder.AppendLine();
				stringBuilder.AppendLine(saveKey);
				stringBuilder.AppendLine("--" + text);
				stringBuilder.AppendLine("Content-Disposition: form-data; name=token");
				stringBuilder.AppendLine();
				stringBuilder.AppendLine(token);
				stringBuilder.AppendLine("--" + text);
				stringBuilder.AppendFormat("Content-Disposition: form-data; name=file; filename=\"{0}\"", saveKey);
				stringBuilder.AppendLine();
				stringBuilder.AppendLine();
				StringBuilder stringBuilder2 = new StringBuilder();
				stringBuilder2.AppendLine();
				stringBuilder2.AppendLine("--" + text + "--");
				byte[] bytes = Encoding.UTF8.GetBytes(stringBuilder.ToString());
				byte[] bytes2 = Encoding.UTF8.GetBytes(stringBuilder2.ToString());
				MemoryStream memoryStream = new MemoryStream();
				memoryStream.Write(bytes, 0, bytes.Length);
				memoryStream.Write(data, 0, data.Length);
				memoryStream.Write(bytes2, 0, bytes2.Length);
				httpResult = httpManager.PostMultipart(uploadHost, memoryStream.ToArray(), text, null);
				if (httpResult.Code == 200)
				{
					httpResult.RefText += string.Format("[{0}] [FormUpload] Uploaded: #DATA# ==> \"{1}\"\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), saveKey);
					return httpResult;
				}
				httpResult.RefText += string.Format("[{0}] [FormUpload] Failed: code = {1}, text = {2}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), httpResult.Code, httpResult.Text);
				return httpResult;
			}
			catch (Exception ex)
			{
				StringBuilder stringBuilder3 = new StringBuilder();
				stringBuilder3.AppendFormat("[{0}] [FormUpload] Error: ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
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

		public HttpResult UploadStream(Stream stream, string saveKey, string token)
		{
			HttpResult httpResult = new HttpResult();
			try
			{
				string text = HttpManager.CreateFormDataBoundary();
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("--" + text);
				stringBuilder.AppendLine("Content-Disposition: form-data; name=key");
				stringBuilder.AppendLine();
				stringBuilder.AppendLine(saveKey);
				stringBuilder.AppendLine("--" + text);
				stringBuilder.AppendLine("Content-Disposition: form-data; name=token");
				stringBuilder.AppendLine();
				stringBuilder.AppendLine(token);
				stringBuilder.AppendLine("--" + text);
				stringBuilder.AppendFormat("Content-Disposition: form-data; name=file; filename=\"{0}\"", saveKey);
				stringBuilder.AppendLine();
				stringBuilder.AppendLine();
				int num = 1048576;
				byte[] buffer = new byte[num];
				int num2 = 0;
				MemoryStream memoryStream = new MemoryStream();
				while (true)
				{
					num2 = stream.Read(buffer, 0, num);
					if (num2 == 0)
					{
						break;
					}
					memoryStream.Write(buffer, 0, num2);
				}
				StringBuilder stringBuilder2 = new StringBuilder();
				stringBuilder2.AppendLine();
				stringBuilder2.AppendLine("--" + text + "--");
				byte[] bytes = Encoding.UTF8.GetBytes(stringBuilder.ToString());
				byte[] array = memoryStream.ToArray();
				byte[] bytes2 = Encoding.UTF8.GetBytes(stringBuilder2.ToString());
				MemoryStream memoryStream2 = new MemoryStream();
				memoryStream2.Write(bytes, 0, bytes.Length);
				memoryStream2.Write(array, 0, array.Length);
				memoryStream2.Write(bytes2, 0, bytes2.Length);
				httpResult = httpManager.PostMultipart(uploadHost, memoryStream2.ToArray(), text, null);
				if (httpResult.Code == 200)
				{
					httpResult.RefText += string.Format("[{0}] [FormUpload] Uploaded: #STREAM# ==> \"{1}\"\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), saveKey);
					return httpResult;
				}
				httpResult.RefText += string.Format("[{0}] [FormUpload] Failed: code = {1}, text = {2}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), httpResult.Code, httpResult.Text);
				return httpResult;
			}
			catch (Exception ex)
			{
				StringBuilder stringBuilder3 = new StringBuilder();
				stringBuilder3.AppendFormat("[{0}] [FormUpload] Error: ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
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
