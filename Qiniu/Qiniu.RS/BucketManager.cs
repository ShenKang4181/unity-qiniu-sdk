using System;
using System.Text;
using Qiniu.Common;
using Qiniu.Http;
using Qiniu.RS.Model;
using Qiniu.Util;

namespace Qiniu.RS
{
	public class BucketManager
	{
		private Auth auth;

		private HttpManager httpManager;

		public BucketManager(Mac mac)
		{
			auth = new Auth(mac);
			httpManager = new HttpManager(false);
		}

		public StatResult Stat(string bucket, string key)
		{
			StatResult statResult = new StatResult();
			try
			{
				string url = Config.ZONE.RsHost + StatOp(bucket, key);
				string token = auth.CreateManageToken(url);
				HttpResult hr = httpManager.Get(url, token);
				statResult.Shadow(hr);
				return statResult;
			}
			catch (Exception ex)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat("[{0}] [stat] Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
				for (Exception ex2 = ex; ex2 != null; ex2 = ex2.InnerException)
				{
					stringBuilder.Append(ex2.Message + " ");
				}
				stringBuilder.AppendLine();
				statResult.RefCode = -252;
				statResult.RefText += stringBuilder.ToString();
				return statResult;
			}
		}

		public BucketsResult Buckets()
		{
			BucketsResult bucketsResult = new BucketsResult();
			try
			{
				string url = Config.ZONE.RsHost + "/buckets";
				string token = auth.CreateManageToken(url);
				HttpResult hr = httpManager.Get(url, token);
				bucketsResult.Shadow(hr);
				return bucketsResult;
			}
			catch (Exception ex)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat("[{0}] [buckets] Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
				for (Exception ex2 = ex; ex2 != null; ex2 = ex2.InnerException)
				{
					stringBuilder.Append(ex2.Message + " ");
				}
				stringBuilder.AppendLine();
				bucketsResult.RefCode = -252;
				bucketsResult.RefText += stringBuilder.ToString();
				return bucketsResult;
			}
		}

		public BucketResult Bucket(string bucketName)
		{
			BucketResult bucketResult = new BucketResult();
			try
			{
				string url = Config.ZONE.RsHost + "/bucket/" + bucketName;
				string token = auth.CreateManageToken(url);
				HttpResult hr = httpManager.Get(url, token);
				bucketResult.Shadow(hr);
				return bucketResult;
			}
			catch (Exception ex)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat("[{0}] [bucket] Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
				for (Exception ex2 = ex; ex2 != null; ex2 = ex2.InnerException)
				{
					stringBuilder.Append(ex2.Message + " ");
				}
				stringBuilder.AppendLine();
				bucketResult.RefCode = -252;
				bucketResult.RefText += stringBuilder.ToString();
				return bucketResult;
			}
		}

		public HttpResult Delete(string bucket, string key)
		{
			HttpResult httpResult = new HttpResult();
			try
			{
				string url = Config.ZONE.RsHost + DeleteOp(bucket, key);
				string token = auth.CreateManageToken(url);
				httpResult = httpManager.Post(url, token);
				return httpResult;
			}
			catch (Exception ex)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat("[{0}] [delete] Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
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

		public HttpResult Copy(string srcBucket, string srcKey, string dstBucket, string dstKey)
		{
			HttpResult httpResult = new HttpResult();
			try
			{
				string url = Config.ZONE.RsHost + CopyOp(srcBucket, srcKey, dstBucket, dstKey);
				string token = auth.CreateManageToken(url);
				httpResult = httpManager.Post(url, token);
				return httpResult;
			}
			catch (Exception ex)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat("[{0}] [copy] Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
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

		public HttpResult Copy(string srcBucket, string srcKey, string dstBucket, string dstKey, bool force)
		{
			HttpResult httpResult = new HttpResult();
			try
			{
				string url = Config.ZONE.RsHost + CopyOp(srcBucket, srcKey, dstBucket, dstKey, force);
				string token = auth.CreateManageToken(url);
				httpResult = httpManager.Post(url, token);
				return httpResult;
			}
			catch (Exception ex)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat("[{0}] [copy] Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
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

		public HttpResult Move(string srcBucket, string srcKey, string dstBucket, string dstKey)
		{
			HttpResult httpResult = new HttpResult();
			try
			{
				string url = Config.ZONE.RsHost + MoveOp(srcBucket, srcKey, dstBucket, dstKey);
				string token = auth.CreateManageToken(url);
				httpResult = httpManager.Post(url, token);
				return httpResult;
			}
			catch (Exception ex)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat("[{0}] [move] Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
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

		public HttpResult Move(string srcBucket, string srcKey, string dstBucket, string dstKey, bool force)
		{
			HttpResult httpResult = new HttpResult();
			try
			{
				string url = Config.ZONE.RsHost + MoveOp(srcBucket, srcKey, dstBucket, dstKey, force);
				string token = auth.CreateManageToken(url);
				httpResult = httpManager.Post(url, token);
				return httpResult;
			}
			catch (Exception ex)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat("[{0}] [move] Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
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

		public HttpResult Rename(string bucket, string oldKey, string newKey)
		{
			return Move(bucket, oldKey, bucket, newKey);
		}

		public HttpResult Chgm(string bucket, string key, string mimeType)
		{
			HttpResult httpResult = new HttpResult();
			try
			{
				string url = Config.ZONE.RsHost + ChgmOp(bucket, key, mimeType);
				string token = auth.CreateManageToken(url);
				httpResult = httpManager.Post(url, token);
				return httpResult;
			}
			catch (Exception ex)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat("[{0}] [chgm] Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
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

		public BatchResult Batch(string batchOps)
		{
			BatchResult batchResult = new BatchResult();
			try
			{
				string url = Config.ZONE.RsHost + "/batch";
				byte[] bytes = Encoding.UTF8.GetBytes(batchOps);
				string token = auth.CreateManageToken(url, bytes);
				HttpResult hr = httpManager.PostForm(url, bytes, token);
				batchResult.Shadow(hr);
				return batchResult;
			}
			catch (Exception ex)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat("[{0}] [batch] Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
				for (Exception ex2 = ex; ex2 != null; ex2 = ex2.InnerException)
				{
					stringBuilder.Append(ex2.Message + " ");
				}
				stringBuilder.AppendLine();
				batchResult.RefCode = -252;
				batchResult.RefText += stringBuilder.ToString();
				return batchResult;
			}
		}

		public BatchResult Batch(string[] ops)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("op={0}", ops[0]);
			for (int i = 1; i < ops.Length; i++)
			{
				stringBuilder.AppendFormat("&op={0}", ops[i]);
			}
			return Batch(stringBuilder.ToString());
		}

		public BatchResult BatchStat(string bucket, string[] keys)
		{
			string[] array = new string[keys.Length];
			for (int i = 0; i < keys.Length; i++)
			{
				array[i] = StatOp(bucket, keys[i]);
			}
			return Batch(array);
		}

		public BatchResult BatchDelete(string bucket, string[] keys)
		{
			string[] array = new string[keys.Length];
			for (int i = 0; i < keys.Length; i++)
			{
				array[i] = DeleteOp(bucket, keys[i]);
			}
			return Batch(array);
		}

		public HttpResult Fetch(string resUrl, string bucket, string key)
		{
			HttpResult httpResult = new HttpResult();
			try
			{
				string url = Config.ZONE.IovipHost + FetchOp(resUrl, bucket, key);
				string token = auth.CreateManageToken(url);
				httpResult = httpManager.Post(url, token);
				return httpResult;
			}
			catch (Exception ex)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat("[{0}] [fetch] Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
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

		public HttpResult Prefetch(string bucket, string key)
		{
			HttpResult httpResult = new HttpResult();
			try
			{
				string url = Config.ZONE.IovipHost + PrefetchOp(bucket, key);
				string token = auth.CreateManageToken(url);
				httpResult = httpManager.Post(url, token);
				return httpResult;
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
				httpResult.RefCode = -252;
				httpResult.RefText += stringBuilder.ToString();
				return httpResult;
			}
		}

		public DomainsResult Domains(string bucket)
		{
			DomainsResult domainsResult = new DomainsResult();
			try
			{
				string url = Config.ZONE.ApiHost + "/v6/domain/list";
				string s = string.Format("tbl={0}", bucket);
				byte[] bytes = Encoding.UTF8.GetBytes(s);
				string token = auth.CreateManageToken(url, bytes);
				HttpResult hr = httpManager.PostForm(url, bytes, token);
				domainsResult.Shadow(hr);
				return domainsResult;
			}
			catch (Exception ex)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat("[{0}] [domains] Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
				for (Exception ex2 = ex; ex2 != null; ex2 = ex2.InnerException)
				{
					stringBuilder.Append(ex2.Message + " ");
				}
				stringBuilder.AppendLine();
				domainsResult.RefCode = -252;
				domainsResult.RefText += stringBuilder.ToString();
				return domainsResult;
			}
		}

		public ListResult ListFiles(string bucket, string prefix, string marker, int limit, string delimiter)
		{
			ListResult listResult = new ListResult();
			try
			{
				StringBuilder stringBuilder = new StringBuilder("/list?bucket=" + bucket);
				if (!string.IsNullOrEmpty(marker))
				{
					stringBuilder.Append("&marker=" + marker);
				}
				if (!string.IsNullOrEmpty(prefix))
				{
					stringBuilder.Append("&prefix=" + prefix);
				}
				if (!string.IsNullOrEmpty(delimiter))
				{
					stringBuilder.Append("&delimiter=" + delimiter);
				}
				if (limit > 1000 || limit < 1)
				{
					stringBuilder.Append("&limit=1000");
				}
				else
				{
					stringBuilder.Append("&limit=" + limit);
				}
				string url = Config.ZONE.RsfHost + stringBuilder.ToString();
				string token = auth.CreateManageToken(url);
				HttpResult hr = httpManager.Post(url, token);
				listResult.Shadow(hr);
				return listResult;
			}
			catch (Exception ex)
			{
				StringBuilder stringBuilder2 = new StringBuilder();
				stringBuilder2.AppendFormat("[{0}] [listFiles] Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
				for (Exception ex2 = ex; ex2 != null; ex2 = ex2.InnerException)
				{
					stringBuilder2.Append(ex2.Message + " ");
				}
				stringBuilder2.AppendLine();
				listResult.RefCode = -252;
				listResult.RefText += stringBuilder2.ToString();
				return listResult;
			}
		}

		public HttpResult UpdateLifecycle(string bucket, string key, int deleteAfterDays)
		{
			HttpResult httpResult = new HttpResult();
			try
			{
				string url = Config.ZONE.RsHost + UpdateLifecycleOp(bucket, key, deleteAfterDays);
				string token = auth.CreateManageToken(url);
				httpResult = httpManager.Post(url, token);
				return httpResult;
			}
			catch (Exception ex)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat("[{0}] [deleteAfterDays] Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
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

		public string StatOp(string bucket, string key)
		{
			return string.Format("/stat/{0}", Base64.UrlSafeBase64Encode(bucket, key));
		}

		public string DeleteOp(string bucket, string key)
		{
			return string.Format("/delete/{0}", Base64.UrlSafeBase64Encode(bucket, key));
		}

		public string CopyOp(string srcBucket, string srcKey, string dstBucket, string dstKey)
		{
			return string.Format("/copy/{0}/{1}", Base64.UrlSafeBase64Encode(srcBucket, srcKey), Base64.UrlSafeBase64Encode(dstBucket, dstKey));
		}

		public string CopyOp(string srcBucket, string srcKey, string dstBucket, string dstKey, bool force)
		{
			string arg = (force ? "force/true" : "force/false");
			return string.Format("/copy/{0}/{1}/{2}", Base64.UrlSafeBase64Encode(srcBucket, srcKey), Base64.UrlSafeBase64Encode(dstBucket, dstKey), arg);
		}

		public string MoveOp(string srcBucket, string srcKey, string dstBucket, string dstKey)
		{
			return string.Format("/move/{0}/{1}", Base64.UrlSafeBase64Encode(srcBucket, srcKey), Base64.UrlSafeBase64Encode(dstBucket, dstKey));
		}

		public string MoveOp(string srcBucket, string srcKey, string dstBucket, string dstKey, bool force)
		{
			string arg = (force ? "force/true" : "force/false");
			return string.Format("/move/{0}/{1}/{2}", Base64.UrlSafeBase64Encode(srcBucket, srcKey), Base64.UrlSafeBase64Encode(dstBucket, dstKey), arg);
		}

		public string ChgmOp(string bucket, string key, string mimeType)
		{
			return string.Format("/chgm/{0}/mime/{1}", Base64.UrlSafeBase64Encode(bucket, key), Base64.UrlSafeBase64Encode(mimeType));
		}

		public string FetchOp(string url, string bucket, string key)
		{
			return string.Format("/fetch/{0}/to/{1}", Base64.UrlSafeBase64Encode(url), Base64.UrlSafeBase64Encode(bucket, key));
		}

		public string PrefetchOp(string bucket, string key)
		{
			return string.Format("/prefetch/{0}", Base64.UrlSafeBase64Encode(bucket, key));
		}

		public string UpdateLifecycleOp(string bucket, string key, int deleteAfterDays)
		{
			return string.Format("/deleteAfterDays/{0}/{1}", Base64.UrlSafeBase64Encode(bucket, key), deleteAfterDays);
		}
	}
}
