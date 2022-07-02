using System.Collections.Generic;
using System.Text;
using Qiniu.Http;
using Qiniu.JSON;

namespace Qiniu.CDN.Model
{
	public class RefreshResult : HttpResult
	{
		public RefreshInfo Result
		{
			get
			{
				RefreshInfo obj = null;
				if (base.Code == 200 && !string.IsNullOrEmpty(base.Text))
				{
					JsonHelper.Deserialize<RefreshInfo>(base.Text, out obj);
				}
				return obj;
			}
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("code:{0}\n", base.Code);
			stringBuilder.AppendLine();
			if (Result != null)
			{
				stringBuilder.AppendLine("result:");
				stringBuilder.AppendFormat("code:{0}\n", Result.Code);
				if (!string.IsNullOrEmpty(Result.Error))
				{
					stringBuilder.AppendFormat("error:{0}\n", Result.Error);
				}
				if (!string.IsNullOrEmpty(Result.RequestId))
				{
					stringBuilder.AppendFormat("requestId:{0}\n", Result.RequestId);
				}
				if (Result.InvalidDirs != null && Result.InvalidDirs.Count > 0)
				{
					stringBuilder.Append("invalidDirs:");
					foreach (string invalidDir in Result.InvalidDirs)
					{
						stringBuilder.Append(invalidDir + " ");
					}
					stringBuilder.AppendLine();
				}
				if (Result.InvalidUrls != null && Result.InvalidUrls.Count > 0)
				{
					stringBuilder.Append("invalidUrls:");
					foreach (string invalidUrl in Result.InvalidUrls)
					{
						stringBuilder.Append(invalidUrl + " ");
					}
					stringBuilder.AppendLine();
				}
				stringBuilder.AppendFormat("dirQuotaDay:{0}\n", Result.DirQuotaDay);
				stringBuilder.AppendFormat("dirSurplusaDay:{0}\n", Result.DirSurplusaDay);
				stringBuilder.AppendFormat("urlQuotaDay:{0}\n", Result.UrlQuotaDay);
				stringBuilder.AppendFormat("urlSurplusaDay:{0}\n", Result.UrlSurplusaDay);
			}
			else if (!string.IsNullOrEmpty(base.Text))
			{
				stringBuilder.AppendLine("text:");
				stringBuilder.AppendLine(base.Text);
			}
			stringBuilder.AppendLine();
			stringBuilder.AppendFormat("ref-code:{0}\n", base.RefCode);
			if (!string.IsNullOrEmpty(base.RefText))
			{
				stringBuilder.AppendLine("ref-text:");
				stringBuilder.AppendLine(base.RefText);
			}
			if (base.RefInfo != null)
			{
				stringBuilder.AppendFormat("ref-info:\n");
				foreach (KeyValuePair<string, string> item in base.RefInfo)
				{
					stringBuilder.AppendLine(string.Format("{0}:{1}", item.Key, item.Value));
				}
			}
			return stringBuilder.ToString();
		}
	}
}
