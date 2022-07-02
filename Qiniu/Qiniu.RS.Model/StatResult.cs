using System.Collections.Generic;
using System.Text;
using Qiniu.Http;
using Qiniu.JSON;

namespace Qiniu.RS.Model
{
	public class StatResult : HttpResult
	{
		public StatInfo Result
		{
			get
			{
				StatInfo obj = null;
				if (base.Code == 200 && !string.IsNullOrEmpty(base.Text))
				{
					JsonHelper.Deserialize<StatInfo>(base.Text, out obj);
				}
				return obj;
			}
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("code: {0}\n", base.Code);
			if (Result != null)
			{
				stringBuilder.AppendFormat("Size={0}, Type={1}, Hash={2}, Time={3}\n", Result.Fsize, Result.MimeType, Result.Hash, Result.PutTime);
			}
			else if (!string.IsNullOrEmpty(base.Text))
			{
				stringBuilder.AppendLine("text:");
				stringBuilder.AppendLine(base.Text);
			}
			stringBuilder.AppendLine();
			stringBuilder.AppendFormat("ref-code: {0}\n", base.RefCode);
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
					stringBuilder.AppendLine(string.Format("{0}: {1}", item.Key, item.Value));
				}
			}
			return stringBuilder.ToString();
		}
	}
}
