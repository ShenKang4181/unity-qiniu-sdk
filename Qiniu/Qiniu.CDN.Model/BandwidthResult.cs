using System.Collections.Generic;
using System.Text;
using Qiniu.Http;
using Qiniu.JSON;

namespace Qiniu.CDN.Model
{
	public class BandwidthResult : HttpResult
	{
		public BandwidthInfo Result
		{
			get
			{
				BandwidthInfo obj = null;
				if (base.Code == 200 && !string.IsNullOrEmpty(base.Text))
				{
					JsonHelper.Deserialize<BandwidthInfo>(base.Text, out obj);
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
				if (Result.Time != null)
				{
					stringBuilder.Append("time:");
					foreach (string item in Result.Time)
					{
						stringBuilder.Append(item + " ");
					}
					stringBuilder.AppendLine();
				}
				if (Result.Data != null && Result.Data.Count > 0)
				{
					stringBuilder.Append("bandwidth:");
					foreach (KeyValuePair<string, BandWidthData> datum in Result.Data)
					{
						stringBuilder.AppendFormat("{0}:\nChina: {1}, Oversea={2}\n", datum.Key, datum.Value.China, datum.Value.Oversea);
					}
					stringBuilder.AppendLine();
				}
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
				foreach (KeyValuePair<string, string> item2 in base.RefInfo)
				{
					stringBuilder.AppendLine(string.Format("{0}:{1}", item2.Key, item2.Value));
				}
			}
			return stringBuilder.ToString();
		}
	}
}
