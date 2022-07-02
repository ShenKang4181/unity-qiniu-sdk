using System.Collections.Generic;
using System.Text;
using Qiniu.Http;
using Qiniu.JSON;

namespace Qiniu.CDN.Model
{
	public class LogListResult : HttpResult
	{
		public LogListInfo Result
		{
			get
			{
				LogListInfo obj = null;
				if (base.Code == 200 && !string.IsNullOrEmpty(base.Text))
				{
					JsonHelper.Deserialize<LogListInfo>(base.Text, out obj);
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
				if (Result.Data != null && Result.Data.Count > 0)
				{
					stringBuilder.AppendLine("log:");
					foreach (string key in Result.Data.Keys)
					{
						stringBuilder.AppendFormat("{0}:\n", key);
						foreach (KeyValuePair<string, List<LogData>> datum in Result.Data)
						{
							if (datum.Value == null)
							{
								continue;
							}
							stringBuilder.AppendFormat("Domain:{0}\n", datum.Key);
							foreach (LogData item in datum.Value)
							{
								if (item != null)
								{
									stringBuilder.AppendFormat("Name:{0}\nSize:{1}\nMtime:{2}\nUrl:{3}\n\n", item.Name, item.Size, item.Mtime, item.Url);
								}
							}
						}
						stringBuilder.AppendLine();
					}
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
