using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Qiniu.Util;

namespace Qiniu.Http
{
	public class HttpManager
	{
		private bool allowAutoRedirect;

		private string userAgent;

		public HttpManager(bool allowAutoRedirect = false)
		{
			this.allowAutoRedirect = allowAutoRedirect;
			userAgent = GetUserAgent();
		}

		public static string GetUserAgent()
		{
			string text = string.Concat(Environment.OSVersion.Platform, "; ", Environment.OSVersion.Version);
			return string.Format("{0}/{1} ({2}; {3})", "QiniuCSharpSDK", "7.2.15", "NET20", text);
		}

		public void SetUserAgent(string userAgent)
		{
			if (!string.IsNullOrEmpty(userAgent))
			{
				this.userAgent = userAgent;
			}
		}

		public static string CreateFormDataBoundary()
		{
			string str = DateTime.UtcNow.Ticks.ToString();
			return string.Format("-------{0}Boundary{1}", "QiniuCSharpSDK", Hashing.CalcMD5X(str));
		}

		public HttpResult Get(string url, string token, bool binaryMode = false)
		{
			HttpResult hr = new HttpResult();
			HttpWebRequest httpWebRequest = null;
			try
			{
				httpWebRequest = WebRequest.Create(url) as HttpWebRequest;
				httpWebRequest.Method = "GET";
				if (!string.IsNullOrEmpty(token))
				{
					httpWebRequest.Headers.Add("Authorization", token);
				}
				httpWebRequest.UserAgent = userAgent;
				httpWebRequest.AllowAutoRedirect = allowAutoRedirect;
				HttpWebResponse httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse;
				if (httpWebResponse != null)
				{
					hr.Code = (int)httpWebResponse.StatusCode;
					hr.RefCode = (int)httpWebResponse.StatusCode;
					getHeaders(ref hr, httpWebResponse);
					if (binaryMode)
					{
						int num = (int)httpWebResponse.ContentLength;
						hr.Data = new byte[num];
						int num2 = num;
						int num3 = 0;
						using (BinaryReader binaryReader = new BinaryReader(httpWebResponse.GetResponseStream()))
						{
							while (num2 > 0)
							{
								num3 = binaryReader.Read(hr.Data, num - num2, num2);
								num2 -= num3;
							}
						}
					}
					else
					{
						using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
						{
							hr.Text = streamReader.ReadToEnd();
						}
					}
					httpWebResponse.Close();
					return hr;
				}
				return hr;
			}
			catch (WebException ex)
			{
				HttpWebResponse httpWebResponse2 = ex.Response as HttpWebResponse;
				if (httpWebResponse2 != null)
				{
					hr.Code = (int)httpWebResponse2.StatusCode;
					hr.RefCode = (int)httpWebResponse2.StatusCode;
					getHeaders(ref hr, httpWebResponse2);
					using (StreamReader streamReader2 = new StreamReader(httpWebResponse2.GetResponseStream()))
					{
						hr.Text = streamReader2.ReadToEnd();
					}
					httpWebResponse2.Close();
					return hr;
				}
				return hr;
			}
			catch (Exception ex2)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat("[{0}] [{1}] [HTTP-GET] Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), userAgent);
				for (Exception ex3 = ex2; ex3 != null; ex3 = ex3.InnerException)
				{
					stringBuilder.Append(ex3.Message + " ");
				}
				stringBuilder.AppendLine();
				hr.RefCode = -252;
				hr.RefText += stringBuilder.ToString();
				return hr;
			}
			finally
			{
				if (httpWebRequest != null)
				{
					httpWebRequest.Abort();
				}
			}
		}

		public HttpResult Post(string url, string token, bool binaryMode = false)
		{
			HttpResult hr = new HttpResult();
			HttpWebRequest httpWebRequest = null;
			try
			{
				httpWebRequest = WebRequest.Create(url) as HttpWebRequest;
				httpWebRequest.Method = "POST";
				if (!string.IsNullOrEmpty(token))
				{
					httpWebRequest.Headers.Add("Authorization", token);
				}
				httpWebRequest.UserAgent = userAgent;
				httpWebRequest.AllowAutoRedirect = allowAutoRedirect;
				HttpWebResponse httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse;
				if (httpWebResponse != null)
				{
					hr.Code = (int)httpWebResponse.StatusCode;
					hr.RefCode = (int)httpWebResponse.StatusCode;
					getHeaders(ref hr, httpWebResponse);
					if (binaryMode)
					{
						int num = (int)httpWebResponse.ContentLength;
						hr.Data = new byte[num];
						int num2 = num;
						int num3 = 0;
						using (BinaryReader binaryReader = new BinaryReader(httpWebResponse.GetResponseStream()))
						{
							while (num2 > 0)
							{
								num3 = binaryReader.Read(hr.Data, num - num2, num2);
								num2 -= num3;
							}
						}
					}
					else
					{
						using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
						{
							hr.Text = streamReader.ReadToEnd();
						}
					}
					httpWebResponse.Close();
					return hr;
				}
				return hr;
			}
			catch (WebException ex)
			{
				HttpWebResponse httpWebResponse2 = ex.Response as HttpWebResponse;
				if (httpWebResponse2 != null)
				{
					hr.Code = (int)httpWebResponse2.StatusCode;
					hr.RefCode = (int)httpWebResponse2.StatusCode;
					getHeaders(ref hr, httpWebResponse2);
					using (StreamReader streamReader2 = new StreamReader(httpWebResponse2.GetResponseStream()))
					{
						hr.Text = streamReader2.ReadToEnd();
					}
					httpWebResponse2.Close();
					return hr;
				}
				return hr;
			}
			catch (Exception ex2)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat("[{0}] [{1}] [HTTP-POST] Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), userAgent);
				for (Exception ex3 = ex2; ex3 != null; ex3 = ex3.InnerException)
				{
					stringBuilder.Append(ex3.Message + " ");
				}
				stringBuilder.AppendLine();
				hr.RefCode = -252;
				hr.RefText += stringBuilder.ToString();
				return hr;
			}
			finally
			{
				if (httpWebRequest != null)
				{
					httpWebRequest.Abort();
				}
			}
		}

		public HttpResult PostData(string url, byte[] data, string token, bool binaryMode = false)
		{
			HttpResult hr = new HttpResult();
			HttpWebRequest httpWebRequest = null;
			try
			{
				httpWebRequest = WebRequest.Create(url) as HttpWebRequest;
				httpWebRequest.Method = "POST";
				if (!string.IsNullOrEmpty(token))
				{
					httpWebRequest.Headers.Add("Authorization", token);
				}
				httpWebRequest.ContentType = ContentType.APPLICATION_OCTET_STREAM;
				httpWebRequest.UserAgent = userAgent;
				httpWebRequest.AllowAutoRedirect = allowAutoRedirect;
				if (data != null)
				{
					httpWebRequest.AllowWriteStreamBuffering = true;
					using (Stream stream = httpWebRequest.GetRequestStream())
					{
						stream.Write(data, 0, data.Length);
						stream.Flush();
					}
				}
				HttpWebResponse httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse;
				if (httpWebResponse != null)
				{
					hr.Code = (int)httpWebResponse.StatusCode;
					hr.RefCode = (int)httpWebResponse.StatusCode;
					getHeaders(ref hr, httpWebResponse);
					if (binaryMode)
					{
						int num = (int)httpWebResponse.ContentLength;
						hr.Data = new byte[num];
						int num2 = num;
						int num3 = 0;
						using (BinaryReader binaryReader = new BinaryReader(httpWebResponse.GetResponseStream()))
						{
							while (num2 > 0)
							{
								num3 = binaryReader.Read(hr.Data, num - num2, num2);
								num2 -= num3;
							}
						}
					}
					else
					{
						using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
						{
							hr.Text = streamReader.ReadToEnd();
						}
					}
					httpWebResponse.Close();
					return hr;
				}
				return hr;
			}
			catch (WebException ex)
			{
				HttpWebResponse httpWebResponse2 = ex.Response as HttpWebResponse;
				if (httpWebResponse2 != null)
				{
					hr.Code = (int)httpWebResponse2.StatusCode;
					hr.RefCode = (int)httpWebResponse2.StatusCode;
					getHeaders(ref hr, httpWebResponse2);
					using (StreamReader streamReader2 = new StreamReader(httpWebResponse2.GetResponseStream()))
					{
						hr.Text = streamReader2.ReadToEnd();
					}
					httpWebResponse2.Close();
					return hr;
				}
				return hr;
			}
			catch (Exception ex2)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat("[{0}] [{1}] [HTTP-POST-BIN] Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), userAgent);
				for (Exception ex3 = ex2; ex3 != null; ex3 = ex3.InnerException)
				{
					stringBuilder.Append(ex3.Message + " ");
				}
				stringBuilder.AppendLine();
				hr.RefCode = -252;
				hr.RefText += stringBuilder.ToString();
				return hr;
			}
			finally
			{
				if (httpWebRequest != null)
				{
					httpWebRequest.Abort();
				}
			}
		}

		public HttpResult PostData(string url, byte[] data, string mimeType, string token, bool binaryMode = false)
		{
			HttpResult hr = new HttpResult();
			HttpWebRequest httpWebRequest = null;
			try
			{
				httpWebRequest = WebRequest.Create(url) as HttpWebRequest;
				httpWebRequest.Method = "POST";
				if (!string.IsNullOrEmpty(token))
				{
					httpWebRequest.Headers.Add("Authorization", token);
				}
				httpWebRequest.ContentType = mimeType;
				httpWebRequest.UserAgent = userAgent;
				httpWebRequest.AllowAutoRedirect = allowAutoRedirect;
				if (data != null)
				{
					httpWebRequest.AllowWriteStreamBuffering = true;
					using (Stream stream = httpWebRequest.GetRequestStream())
					{
						stream.Write(data, 0, data.Length);
						stream.Flush();
					}
				}
				HttpWebResponse httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse;
				if (httpWebResponse != null)
				{
					hr.Code = (int)httpWebResponse.StatusCode;
					hr.RefCode = (int)httpWebResponse.StatusCode;
					getHeaders(ref hr, httpWebResponse);
					if (binaryMode)
					{
						int num = (int)httpWebResponse.ContentLength;
						hr.Data = new byte[num];
						int num2 = num;
						int num3 = 0;
						using (BinaryReader binaryReader = new BinaryReader(httpWebResponse.GetResponseStream()))
						{
							while (num2 > 0)
							{
								num3 = binaryReader.Read(hr.Data, num - num2, num2);
								num2 -= num3;
							}
						}
					}
					else
					{
						using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
						{
							hr.Text = streamReader.ReadToEnd();
						}
					}
					httpWebResponse.Close();
					return hr;
				}
				return hr;
			}
			catch (WebException ex)
			{
				HttpWebResponse httpWebResponse2 = ex.Response as HttpWebResponse;
				if (httpWebResponse2 != null)
				{
					hr.Code = (int)httpWebResponse2.StatusCode;
					hr.RefCode = (int)httpWebResponse2.StatusCode;
					getHeaders(ref hr, httpWebResponse2);
					using (StreamReader streamReader2 = new StreamReader(httpWebResponse2.GetResponseStream()))
					{
						hr.Text = streamReader2.ReadToEnd();
					}
					httpWebResponse2.Close();
					return hr;
				}
				return hr;
			}
			catch (Exception ex2)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat("[{0}] [{1}] [HTTP-POST-BIN] Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), userAgent);
				for (Exception ex3 = ex2; ex3 != null; ex3 = ex3.InnerException)
				{
					stringBuilder.Append(ex3.Message + " ");
				}
				stringBuilder.AppendLine();
				hr.RefCode = -252;
				hr.RefText += stringBuilder.ToString();
				return hr;
			}
			finally
			{
				if (httpWebRequest != null)
				{
					httpWebRequest.Abort();
				}
			}
		}

		public HttpResult PostJson(string url, string data, string token, bool binaryMode = false)
		{
			HttpResult hr = new HttpResult();
			HttpWebRequest httpWebRequest = null;
			try
			{
				httpWebRequest = WebRequest.Create(url) as HttpWebRequest;
				httpWebRequest.Method = "POST";
				if (!string.IsNullOrEmpty(token))
				{
					httpWebRequest.Headers.Add("Authorization", token);
				}
				httpWebRequest.ContentType = ContentType.APPLICATION_JSON;
				httpWebRequest.UserAgent = userAgent;
				httpWebRequest.AllowAutoRedirect = allowAutoRedirect;
				if (data != null)
				{
					httpWebRequest.AllowWriteStreamBuffering = true;
					using (Stream stream = httpWebRequest.GetRequestStream())
					{
						stream.Write(Encoding.UTF8.GetBytes(data), 0, data.Length);
						stream.Flush();
					}
				}
				HttpWebResponse httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse;
				if (httpWebResponse != null)
				{
					hr.Code = (int)httpWebResponse.StatusCode;
					hr.RefCode = (int)httpWebResponse.StatusCode;
					getHeaders(ref hr, httpWebResponse);
					if (binaryMode)
					{
						int num = (int)httpWebResponse.ContentLength;
						hr.Data = new byte[num];
						int num2 = num;
						int num3 = 0;
						using (BinaryReader binaryReader = new BinaryReader(httpWebResponse.GetResponseStream()))
						{
							while (num2 > 0)
							{
								num3 = binaryReader.Read(hr.Data, num - num2, num2);
								num2 -= num3;
							}
						}
					}
					else
					{
						using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
						{
							hr.Text = streamReader.ReadToEnd();
						}
					}
					httpWebResponse.Close();
					return hr;
				}
				return hr;
			}
			catch (WebException ex)
			{
				HttpWebResponse httpWebResponse2 = ex.Response as HttpWebResponse;
				if (httpWebResponse2 != null)
				{
					hr.Code = (int)httpWebResponse2.StatusCode;
					hr.RefCode = (int)httpWebResponse2.StatusCode;
					getHeaders(ref hr, httpWebResponse2);
					using (StreamReader streamReader2 = new StreamReader(httpWebResponse2.GetResponseStream()))
					{
						hr.Text = streamReader2.ReadToEnd();
					}
					httpWebResponse2.Close();
					return hr;
				}
				return hr;
			}
			catch (Exception ex2)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat("[{0}] [{1}] [HTTP-POST-JSON] Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), userAgent);
				for (Exception ex3 = ex2; ex3 != null; ex3 = ex3.InnerException)
				{
					stringBuilder.Append(ex3.Message + " ");
				}
				stringBuilder.AppendLine();
				hr.RefCode = -252;
				hr.RefText += stringBuilder.ToString();
				return hr;
			}
			finally
			{
				if (httpWebRequest != null)
				{
					httpWebRequest.Abort();
				}
			}
		}

		public HttpResult PostText(string url, string data, string token, bool binaryMode = false)
		{
			HttpResult hr = new HttpResult();
			HttpWebRequest httpWebRequest = null;
			try
			{
				httpWebRequest = WebRequest.Create(url) as HttpWebRequest;
				httpWebRequest.Method = "POST";
				if (!string.IsNullOrEmpty(token))
				{
					httpWebRequest.Headers.Add("Authorization", token);
				}
				httpWebRequest.ContentType = ContentType.TEXT_PLAIN;
				httpWebRequest.UserAgent = userAgent;
				httpWebRequest.AllowAutoRedirect = allowAutoRedirect;
				if (data != null)
				{
					httpWebRequest.AllowWriteStreamBuffering = true;
					using (Stream stream = httpWebRequest.GetRequestStream())
					{
						stream.Write(Encoding.UTF8.GetBytes(data), 0, data.Length);
						stream.Flush();
					}
				}
				HttpWebResponse httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse;
				if (httpWebResponse != null)
				{
					hr.Code = (int)httpWebResponse.StatusCode;
					hr.RefCode = (int)httpWebResponse.StatusCode;
					getHeaders(ref hr, httpWebResponse);
					if (binaryMode)
					{
						int num = (int)httpWebResponse.ContentLength;
						hr.Data = new byte[num];
						int num2 = num;
						int num3 = 0;
						using (BinaryReader binaryReader = new BinaryReader(httpWebResponse.GetResponseStream()))
						{
							while (num2 > 0)
							{
								num3 = binaryReader.Read(hr.Data, num - num2, num2);
								num2 -= num3;
							}
						}
					}
					else
					{
						using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
						{
							hr.Text = streamReader.ReadToEnd();
						}
					}
					httpWebResponse.Close();
					return hr;
				}
				return hr;
			}
			catch (WebException ex)
			{
				HttpWebResponse httpWebResponse2 = ex.Response as HttpWebResponse;
				if (httpWebResponse2 != null)
				{
					hr.Code = (int)httpWebResponse2.StatusCode;
					hr.RefCode = (int)httpWebResponse2.StatusCode;
					getHeaders(ref hr, httpWebResponse2);
					using (StreamReader streamReader2 = new StreamReader(httpWebResponse2.GetResponseStream()))
					{
						hr.Text = streamReader2.ReadToEnd();
					}
					httpWebResponse2.Close();
					return hr;
				}
				return hr;
			}
			catch (Exception ex2)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat("[{0}] [{1}] [HTTP-POST-TEXT] Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), userAgent);
				for (Exception ex3 = ex2; ex3 != null; ex3 = ex3.InnerException)
				{
					stringBuilder.Append(ex3.Message + " ");
				}
				stringBuilder.AppendLine();
				hr.RefCode = -252;
				hr.RefText += stringBuilder.ToString();
				return hr;
			}
			finally
			{
				if (httpWebRequest != null)
				{
					httpWebRequest.Abort();
				}
			}
		}

		public HttpResult PostForm(string url, Dictionary<string, string> kvData, string token, bool binaryMode = false)
		{
			HttpResult hr = new HttpResult();
			HttpWebRequest httpWebRequest = null;
			try
			{
				httpWebRequest = WebRequest.Create(url) as HttpWebRequest;
				httpWebRequest.Method = "POST";
				if (!string.IsNullOrEmpty(token))
				{
					httpWebRequest.Headers.Add("Authorization", token);
				}
				httpWebRequest.ContentType = ContentType.WWW_FORM_URLENC;
				httpWebRequest.UserAgent = userAgent;
				httpWebRequest.AllowAutoRedirect = allowAutoRedirect;
				if (kvData != null)
				{
					StringBuilder stringBuilder = new StringBuilder();
					foreach (KeyValuePair<string, string> kvDatum in kvData)
					{
						stringBuilder.AppendFormat("{0}={1}&", Uri.EscapeDataString(kvDatum.Key), Uri.EscapeDataString(kvDatum.Value));
					}
					httpWebRequest.AllowWriteStreamBuffering = true;
					using (Stream stream = httpWebRequest.GetRequestStream())
					{
						stream.Write(Encoding.UTF8.GetBytes(stringBuilder.ToString()), 0, stringBuilder.Length - 1);
						stream.Flush();
					}
				}
				HttpWebResponse httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse;
				if (httpWebResponse != null)
				{
					hr.Code = (int)httpWebResponse.StatusCode;
					hr.RefCode = (int)httpWebResponse.StatusCode;
					getHeaders(ref hr, httpWebResponse);
					if (binaryMode)
					{
						int num = (int)httpWebResponse.ContentLength;
						hr.Data = new byte[num];
						int num2 = num;
						int num3 = 0;
						using (BinaryReader binaryReader = new BinaryReader(httpWebResponse.GetResponseStream()))
						{
							while (num2 > 0)
							{
								num3 = binaryReader.Read(hr.Data, num - num2, num2);
								num2 -= num3;
							}
						}
					}
					else
					{
						using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
						{
							hr.Text = streamReader.ReadToEnd();
						}
					}
					httpWebResponse.Close();
					return hr;
				}
				return hr;
			}
			catch (WebException ex)
			{
				HttpWebResponse httpWebResponse2 = ex.Response as HttpWebResponse;
				if (httpWebResponse2 != null)
				{
					hr.Code = (int)httpWebResponse2.StatusCode;
					hr.RefCode = (int)httpWebResponse2.StatusCode;
					getHeaders(ref hr, httpWebResponse2);
					using (StreamReader streamReader2 = new StreamReader(httpWebResponse2.GetResponseStream()))
					{
						hr.Text = streamReader2.ReadToEnd();
					}
					httpWebResponse2.Close();
					return hr;
				}
				return hr;
			}
			catch (Exception ex2)
			{
				StringBuilder stringBuilder2 = new StringBuilder();
				stringBuilder2.AppendFormat("[{0}] [{1}] [HTTP-POST-FORM] Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), userAgent);
				for (Exception ex3 = ex2; ex3 != null; ex3 = ex3.InnerException)
				{
					stringBuilder2.Append(ex3.Message + " ");
				}
				stringBuilder2.AppendLine();
				hr.RefCode = -252;
				hr.RefText += stringBuilder2.ToString();
				return hr;
			}
			finally
			{
				if (httpWebRequest != null)
				{
					httpWebRequest.Abort();
				}
			}
		}

		public HttpResult PostForm(string url, string data, string token, bool binaryMode = false)
		{
			HttpResult hr = new HttpResult();
			HttpWebRequest httpWebRequest = null;
			try
			{
				httpWebRequest = WebRequest.Create(url) as HttpWebRequest;
				httpWebRequest.Method = "POST";
				if (!string.IsNullOrEmpty(token))
				{
					httpWebRequest.Headers.Add("Authorization", token);
				}
				httpWebRequest.ContentType = ContentType.WWW_FORM_URLENC;
				httpWebRequest.UserAgent = userAgent;
				httpWebRequest.AllowAutoRedirect = allowAutoRedirect;
				if (!string.IsNullOrEmpty(data))
				{
					httpWebRequest.AllowWriteStreamBuffering = true;
					using (Stream stream = httpWebRequest.GetRequestStream())
					{
						stream.Write(Encoding.UTF8.GetBytes(data), 0, data.Length);
						stream.Flush();
					}
				}
				HttpWebResponse httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse;
				if (httpWebResponse != null)
				{
					hr.Code = (int)httpWebResponse.StatusCode;
					hr.RefCode = (int)httpWebResponse.StatusCode;
					getHeaders(ref hr, httpWebResponse);
					if (binaryMode)
					{
						int num = (int)httpWebResponse.ContentLength;
						hr.Data = new byte[num];
						int num2 = num;
						int num3 = 0;
						using (BinaryReader binaryReader = new BinaryReader(httpWebResponse.GetResponseStream()))
						{
							while (num2 > 0)
							{
								num3 = binaryReader.Read(hr.Data, num - num2, num2);
								num2 -= num3;
							}
						}
					}
					else
					{
						using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
						{
							hr.Text = streamReader.ReadToEnd();
						}
					}
					httpWebResponse.Close();
					return hr;
				}
				return hr;
			}
			catch (WebException ex)
			{
				HttpWebResponse httpWebResponse2 = ex.Response as HttpWebResponse;
				if (httpWebResponse2 != null)
				{
					hr.Code = (int)httpWebResponse2.StatusCode;
					hr.RefCode = (int)httpWebResponse2.StatusCode;
					getHeaders(ref hr, httpWebResponse2);
					using (StreamReader streamReader2 = new StreamReader(httpWebResponse2.GetResponseStream()))
					{
						hr.Text = streamReader2.ReadToEnd();
					}
					httpWebResponse2.Close();
					return hr;
				}
				return hr;
			}
			catch (Exception ex2)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat("[{0}] [{1}] [HTTP-POST-FORM] Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), userAgent);
				for (Exception ex3 = ex2; ex3 != null; ex3 = ex3.InnerException)
				{
					stringBuilder.Append(ex3.Message + " ");
				}
				stringBuilder.AppendLine();
				hr.RefCode = -252;
				hr.RefText += stringBuilder.ToString();
				return hr;
			}
			finally
			{
				if (httpWebRequest != null)
				{
					httpWebRequest.Abort();
				}
			}
		}

		public HttpResult PostForm(string url, byte[] data, string token, bool binaryMode = false)
		{
			HttpResult hr = new HttpResult();
			HttpWebRequest httpWebRequest = null;
			try
			{
				httpWebRequest = WebRequest.Create(url) as HttpWebRequest;
				httpWebRequest.Method = "POST";
				if (!string.IsNullOrEmpty(token))
				{
					httpWebRequest.Headers.Add("Authorization", token);
				}
				httpWebRequest.ContentType = ContentType.WWW_FORM_URLENC;
				httpWebRequest.UserAgent = userAgent;
				httpWebRequest.AllowAutoRedirect = allowAutoRedirect;
				if (data != null)
				{
					httpWebRequest.AllowWriteStreamBuffering = true;
					using (Stream stream = httpWebRequest.GetRequestStream())
					{
						stream.Write(data, 0, data.Length);
						stream.Flush();
					}
				}
				HttpWebResponse httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse;
				if (httpWebResponse != null)
				{
					hr.Code = (int)httpWebResponse.StatusCode;
					hr.RefCode = (int)httpWebResponse.StatusCode;
					getHeaders(ref hr, httpWebResponse);
					if (binaryMode)
					{
						int num = (int)httpWebResponse.ContentLength;
						hr.Data = new byte[num];
						int num2 = num;
						int num3 = 0;
						using (BinaryReader binaryReader = new BinaryReader(httpWebResponse.GetResponseStream()))
						{
							while (num2 > 0)
							{
								num3 = binaryReader.Read(hr.Data, num - num2, num2);
								num2 -= num3;
							}
						}
					}
					else
					{
						using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
						{
							hr.Text = streamReader.ReadToEnd();
						}
					}
					httpWebResponse.Close();
					return hr;
				}
				return hr;
			}
			catch (WebException ex)
			{
				HttpWebResponse httpWebResponse2 = ex.Response as HttpWebResponse;
				if (httpWebResponse2 != null)
				{
					hr.Code = (int)httpWebResponse2.StatusCode;
					hr.RefCode = (int)httpWebResponse2.StatusCode;
					getHeaders(ref hr, httpWebResponse2);
					using (StreamReader streamReader2 = new StreamReader(httpWebResponse2.GetResponseStream()))
					{
						hr.Text = streamReader2.ReadToEnd();
					}
					httpWebResponse2.Close();
					return hr;
				}
				return hr;
			}
			catch (Exception ex2)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat("[{0}] [{1}] [HTTP-POST-FORM] Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), userAgent);
				for (Exception ex3 = ex2; ex3 != null; ex3 = ex3.InnerException)
				{
					stringBuilder.Append(ex3.Message + " ");
				}
				stringBuilder.AppendLine();
				hr.RefCode = -252;
				hr.RefText += stringBuilder.ToString();
				return hr;
			}
			finally
			{
				if (httpWebRequest != null)
				{
					httpWebRequest.Abort();
				}
			}
		}

		public HttpResult PostMultipart(string url, byte[] data, string boundary, string token, bool binaryMode = false)
		{
			HttpResult hr = new HttpResult();
			HttpWebRequest httpWebRequest = null;
			try
			{
				httpWebRequest = WebRequest.Create(url) as HttpWebRequest;
				httpWebRequest.Method = "POST";
				if (!string.IsNullOrEmpty(token))
				{
					httpWebRequest.Headers.Add("Authorization", token);
				}
				httpWebRequest.ContentType = string.Format("{0}; boundary={1}", ContentType.MULTIPART_FORM_DATA, boundary);
				httpWebRequest.UserAgent = userAgent;
				httpWebRequest.AllowAutoRedirect = allowAutoRedirect;
				httpWebRequest.AllowWriteStreamBuffering = true;
				using (Stream stream = httpWebRequest.GetRequestStream())
				{
					stream.Write(data, 0, data.Length);
					stream.Flush();
				}
				HttpWebResponse httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse;
				if (httpWebResponse != null)
				{
					hr.Code = (int)httpWebResponse.StatusCode;
					hr.RefCode = (int)httpWebResponse.StatusCode;
					getHeaders(ref hr, httpWebResponse);
					if (binaryMode)
					{
						int num = (int)httpWebResponse.ContentLength;
						hr.Data = new byte[num];
						int num2 = num;
						int num3 = 0;
						using (BinaryReader binaryReader = new BinaryReader(httpWebResponse.GetResponseStream()))
						{
							while (num2 > 0)
							{
								num3 = binaryReader.Read(hr.Data, num - num2, num2);
								num2 -= num3;
							}
						}
					}
					else
					{
						using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
						{
							hr.Text = streamReader.ReadToEnd();
						}
					}
					httpWebResponse.Close();
					return hr;
				}
				return hr;
			}
			catch (WebException ex)
			{
				HttpWebResponse httpWebResponse2 = ex.Response as HttpWebResponse;
				if (httpWebResponse2 != null)
				{
					hr.Code = (int)httpWebResponse2.StatusCode;
					hr.RefCode = (int)httpWebResponse2.StatusCode;
					getHeaders(ref hr, httpWebResponse2);
					using (StreamReader streamReader2 = new StreamReader(httpWebResponse2.GetResponseStream()))
					{
						hr.Text = streamReader2.ReadToEnd();
					}
					httpWebResponse2.Close();
					return hr;
				}
				return hr;
			}
			catch (Exception ex2)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat("[{0}] [{1}] [HTTP-POST-MPART] Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), userAgent);
				for (Exception ex3 = ex2; ex3 != null; ex3 = ex3.InnerException)
				{
					stringBuilder.Append(ex3.Message + " ");
				}
				stringBuilder.AppendLine();
				hr.RefCode = -252;
				hr.RefText += stringBuilder.ToString();
				return hr;
			}
			finally
			{
				if (httpWebRequest != null)
				{
					httpWebRequest.Abort();
				}
			}
		}

		private void getHeaders(ref HttpResult hr, HttpWebResponse resp)
		{
			if (resp == null)
			{
				return;
			}
			if (hr.RefInfo == null)
			{
				hr.RefInfo = new Dictionary<string, string>();
			}
			hr.RefInfo.Add("ProtocolVersion", resp.ProtocolVersion.ToString());
			if (!string.IsNullOrEmpty(resp.CharacterSet))
			{
				hr.RefInfo.Add("Characterset", resp.CharacterSet);
			}
			if (!string.IsNullOrEmpty(resp.ContentEncoding))
			{
				hr.RefInfo.Add("ContentEncoding", resp.ContentEncoding);
			}
			if (!string.IsNullOrEmpty(resp.ContentType))
			{
				hr.RefInfo.Add("ContentType", resp.ContentType);
			}
			hr.RefInfo.Add("ContentLength", resp.ContentLength.ToString());
			WebHeaderCollection headers = resp.Headers;
			if (headers != null && headers.Count > 0)
			{
				if (hr.RefInfo == null)
				{
					hr.RefInfo = new Dictionary<string, string>();
				}
				string[] allKeys = headers.AllKeys;
				foreach (string text in allKeys)
				{
					hr.RefInfo.Add(text, headers[text]);
				}
			}
		}
	}
}
