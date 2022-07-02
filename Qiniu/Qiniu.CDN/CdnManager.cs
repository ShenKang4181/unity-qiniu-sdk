using System;
using System.Text;
using Qiniu.CDN.Model;
using Qiniu.Http;
using Qiniu.Util;

namespace Qiniu.CDN
{
	public class CdnManager
	{
		private Auth auth;

		private HttpManager httpManager;

		public CdnManager(Mac mac)
		{
			auth = new Auth(mac);
			httpManager = new HttpManager(false);
		}

		private string refreshEntry()
		{
			return string.Format("{0}/v2/tune/refresh", "http://fusion.qiniuapi.com");
		}

		private string prefetchEntry()
		{
			return string.Format("{0}/v2/tune/prefetch", "http://fusion.qiniuapi.com");
		}

		private string bandwidthEntry()
		{
			return string.Format("{0}/v2/tune/bandwidth", "http://fusion.qiniuapi.com");
		}

		private string fluxEntry()
		{
			return string.Format("{0}/v2/tune/flux", "http://fusion.qiniuapi.com");
		}

		private string logListEntry()
		{
			return string.Format("{0}/v2/tune/log/list", "http://fusion.qiniuapi.com");
		}

		public RefreshResult RefreshUrlsAndDirs(RefreshRequest request)
		{
			RefreshResult refreshResult = new RefreshResult();
			try
			{
				string url = refreshEntry();
				string data = request.ToJsonStr();
				string token = auth.CreateManageToken(url);
				HttpResult hr = httpManager.PostJson(url, data, token);
				refreshResult.Shadow(hr);
				return refreshResult;
			}
			catch (Exception ex)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat("[{0}] [refresh] Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
				for (Exception ex2 = ex; ex2 != null; ex2 = ex2.InnerException)
				{
					stringBuilder.Append(ex2.Message + " ");
				}
				stringBuilder.AppendLine();
				refreshResult.RefCode = -252;
				refreshResult.RefText += stringBuilder.ToString();
				return refreshResult;
			}
		}

		public RefreshResult RefreshUrls(string[] urls)
		{
			RefreshRequest request = new RefreshRequest(urls, null);
			return RefreshUrlsAndDirs(request);
		}

		public RefreshResult RefreshDirs(string[] dirs)
		{
			RefreshRequest request = new RefreshRequest(null, dirs);
			return RefreshUrlsAndDirs(request);
		}

		public RefreshResult RefreshUrlsAndDirs(string[] urls, string[] dirs)
		{
			RefreshRequest request = new RefreshRequest(urls, dirs);
			return RefreshUrlsAndDirs(request);
		}

		public PrefetchResult PrefetchUrls(PrefetchRequest request)
		{
			PrefetchResult prefetchResult = new PrefetchResult();
			try
			{
				string url = prefetchEntry();
				string data = request.ToJsonStr();
				string token = auth.CreateManageToken(url);
				HttpResult hr = httpManager.PostJson(url, data, token);
				prefetchResult.Shadow(hr);
				return prefetchResult;
			}
			catch (Exception ex)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat("[{0}] [prefetch] Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
				for (Exception ex2 = ex; ex2 != null; ex2 = ex2.InnerException)
				{
					stringBuilder.Append(ex2.Message + " ");
				}
				stringBuilder.AppendLine();
				prefetchResult.RefCode = -252;
				prefetchResult.RefText += stringBuilder.ToString();
				return prefetchResult;
			}
		}

		public PrefetchResult PrefetchUrls(string[] urls)
		{
			PrefetchRequest request = new PrefetchRequest(urls);
			return PrefetchUrls(request);
		}

		public BandwidthResult GetBandwidthData(BandwidthRequest request)
		{
			BandwidthResult bandwidthResult = new BandwidthResult();
			try
			{
				string url = bandwidthEntry();
				string data = request.ToJsonStr();
				string token = auth.CreateManageToken(url);
				HttpResult hr = httpManager.PostJson(url, data, token);
				bandwidthResult.Shadow(hr);
				return bandwidthResult;
			}
			catch (Exception ex)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat("[{0}] [bandwidth] Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
				for (Exception ex2 = ex; ex2 != null; ex2 = ex2.InnerException)
				{
					stringBuilder.Append(ex2.Message + " ");
				}
				stringBuilder.AppendLine();
				bandwidthResult.RefCode = -252;
				bandwidthResult.RefText += stringBuilder.ToString();
				return bandwidthResult;
			}
		}

		public BandwidthResult GetBandwidthData(string[] domains, string startDate, string endDate, string granularity)
		{
			BandwidthRequest request = new BandwidthRequest(startDate, endDate, granularity, StringHelper.Join(domains, ";"));
			return GetBandwidthData(request);
		}

		public FluxResult GetFluxData(FluxRequest request)
		{
			FluxResult fluxResult = new FluxResult();
			try
			{
				string url = fluxEntry();
				string data = request.ToJsonStr();
				string token = auth.CreateManageToken(url);
				HttpResult hr = httpManager.PostJson(url, data, token);
				fluxResult.Shadow(hr);
				return fluxResult;
			}
			catch (Exception ex)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat("[{0}] [flux] Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
				for (Exception ex2 = ex; ex2 != null; ex2 = ex2.InnerException)
				{
					stringBuilder.Append(ex2.Message + " ");
				}
				stringBuilder.AppendLine();
				fluxResult.RefCode = -252;
				fluxResult.RefText += stringBuilder.ToString();
				return fluxResult;
			}
		}

		public FluxResult GetFluxData(string[] domains, string startDate, string endDate, string granularity)
		{
			FluxRequest request = new FluxRequest(startDate, endDate, granularity, StringHelper.Join(domains, ";"));
			return GetFluxData(request);
		}

		public LogListResult GetCdnLogList(LogListRequest request)
		{
			LogListResult logListResult = new LogListResult();
			try
			{
				string url = logListEntry();
				string data = request.ToJsonStr();
				string token = auth.CreateManageToken(url);
				HttpResult hr = httpManager.PostJson(url, data, token);
				logListResult.Shadow(hr);
				return logListResult;
			}
			catch (Exception ex)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat("[{0}] [loglist] Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
				for (Exception ex2 = ex; ex2 != null; ex2 = ex2.InnerException)
				{
					stringBuilder.Append(ex2.Message + " ");
				}
				stringBuilder.AppendLine();
				logListResult.RefCode = -252;
				logListResult.RefText += stringBuilder.ToString();
				return logListResult;
			}
		}

		public LogListResult GetCdnLogList(string[] domains, string date)
		{
			LogListRequest request = new LogListRequest(date, StringHelper.Join(domains, ";"));
			return GetCdnLogList(request);
		}

		public string CreateTimestampAntiLeechUrl(TimestampAntiLeechUrlRequest request)
		{
			string rawUrl = request.RawUrl;
			string text = "&";
			if (string.IsNullOrEmpty(request.Query))
			{
				text = "?";
			}
			string key = request.Key;
			string text2 = request.Path;
			if (!string.IsNullOrEmpty(text2))
			{
				text2 = Uri.EscapeUriString(text2);
			}
			string file = request.File;
			string text3 = long.Parse(request.Timestamp).ToString("x");
			string text4 = Hashing.CalcMD5X(key + text2 + file + text3);
			return string.Format("{0}{1}sign={2}&t={3}", rawUrl, text, text4, text3);
		}

		public string CreateTimestampAntiLeechUrl(string host, string path, string fileName, string query, string encryptKey, int expireInSeconds)
		{
			TimestampAntiLeechUrlRequest timestampAntiLeechUrlRequest = new TimestampAntiLeechUrlRequest();
			timestampAntiLeechUrlRequest.Host = host;
			timestampAntiLeechUrlRequest.Path = path;
			timestampAntiLeechUrlRequest.File = fileName;
			timestampAntiLeechUrlRequest.Query = query;
			timestampAntiLeechUrlRequest.Key = encryptKey;
			timestampAntiLeechUrlRequest.SetLinkExpire(expireInSeconds);
			return CreateTimestampAntiLeechUrl(timestampAntiLeechUrlRequest);
		}
	}
}
